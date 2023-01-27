using Communication.Plc;
using Communication.Plc.Channel;
using Communication.Plc.Shared;
using Messages;
using Patterns.EventAggregator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Models
{
    public class ModelTreeEventType
    {
        private Address address;

        private Address relayAddress;

        private string valueGUID;

        private Task msgTask;

        private FileResources resources = new FileResources();

        private MenuTreeCollectionCommand command;

        private TreeCollection<UiElementData<string>, ModelAttributedEventType<string>> menuTreeCollection;

        private EventAggregator aggregator;

        private CountingLatch latch;

        private ISubscription<Telegram> subscription;

        private Action<Telegram> OnValuePublisher;

        private PlcServices plcServices;

        public ModelTreeEventType(List<ModelAttributedEventType<string>> menuButtons, ModelAttributedEventType<string> infoCommand, ModelAttributedEventType<string> infoMenu, IAddress address, int rowSize, Expression<Func<ModelTreeEventType>> expression)
        {
            if (expression.NodeType != ExpressionType.Lambda)
            {
                throw new ArgumentException("Value must be a lamda expression", "expression");
            }
            this.valueGUID = VariableInfo.GetName(expression);
            this.address = new Address(address, this.valueGUID);
            this.relayAddress = new Address(this.address.Owner, this.address.Relay, this.address.Target, null);
            this.latch = new CountingLatch();
            this.aggregator = SingletonProvider<EventAggregator>.Instance;
            this.menuTreeCollection = new TreeCollection<UiElementData<string>, ModelAttributedEventType<string>>(rowSize, (CommandHandle handle) => this.CommandSwitch(handle))
            {
                InactiveTreeElementDataTemplate = new UiElementData<string>()
                {
                    IsEnabled = false,
                    Visibility = UiElementDataVisibility.Hidden
                }
            };
            this.command = new MenuTreeCollectionCommand(menuButtons, infoCommand, infoMenu);
            this.command.Init(this.menuTreeCollection);
            this.menuTreeCollection.Command = this.command;
            this.msgTask = Task.Factory.StartNew(() => {
            });
            this.resources.Data.SetNotify += new ResourceEventHandler((s, e) => { OnNotifyResources(s, e); });
        }

        private void Acknowledge(Telegram telegram)
        {
            Telegram telegram1 = new Telegram(new Address(telegram.Address.Target, telegram.Address.Owner, "", null), 202, (object)0, null);
            this.aggregator.Publish<Telegram>(telegram1, true);
        }

        public void AddControls(List<ModelAttributedEventType<string>> treeElementControls)
        {
            for (int i = 0; i < treeElementControls.Count; i++)
            {
                this.menuTreeCollection.AttachTreeElementControl(i, treeElementControls[i]);
                int num = i;
                treeElementControls[i].Register((Telegram telegram) => this.menuTreeCollection.Execute(num));
            }
        }

        private void CommandSwitch(CommandHandle handle)
        {
            switch (handle.Type)
            {
                case 2:
                    {
                        this.HandleByCallable(handle);
                        return;
                    }
                case 3:
                    {
                        this.HandleByEventSource(handle);
                        return;
                    }
                case 4:
                case 5:
                case 7:
                case 8:
                    {
                        this.HandleByAction(handle);
                        return;
                    }
                case 6:
                    {
                        return;
                    }
                default:
                    {
                        return;
                    }
            }
        }

        private void HandleByAction(CommandHandle handle)
        {
            Address address = new Address(this.address.Owner, handle.Target, "", null);
            this.aggregator.Publish<Telegram>(new Telegram(address, 0, null, null), true);
        }

        private void HandleByCallable(CommandHandle handle)
        {
            string target = handle.Target;
            if (this.plcServices.IsPlcAddress(target))
            {
                target = this.plcServices.AddChannel(target);
            }
            Address address = new Address(this.address.Owner, target, "", null);
            this.aggregator.Publish<Telegram>(new Telegram(address, handle.Command, handle, this.plcServices, null), true);
        }

        private void HandleByEventSource(CommandHandle handle)
        {
            if (this.plcServices == null)
            {
                return;
            }
            PlcEventSourceRaw plcEventSourceRaw = new PlcEventSourceRaw();
            if (!this.plcServices.GetEventSource(handle.Handle, out plcEventSourceRaw))
            {
                return;
            }
            object obj = this.plcServices.ReadByRefPntrHandle(plcEventSourceRaw.DataPntrHandle);
            if (obj is PlcTelegramRaw)
            {
                PlcTelegramRaw command = (PlcTelegramRaw)obj;
                command.CommAddress.Command = (ushort)handle.Command;
                command.CommDataPntr = this.plcServices.GetRefPntr(command.DataPntrHandle);
                BinaryReader binaryReader = this.plcServices.Read(command.CommDataPntr);
                Telegram telegram = PlcMapper.ConvertToTelegram(command, binaryReader, this.plcServices.PlcName);
                this.aggregator.Publish<Telegram>(telegram, false);
            }
            Address address = new Address(this.address.Owner, plcEventSourceRaw.Target.Name, "", null);
            this.aggregator.Publish<Telegram>(new Telegram(address, 0, obj, null), false);
        }

        private void OnNotifyResources(object sender, ResourceEventArgs e)
        {
            if (e.Name.ToLower().CompareTo("LanguageId".ToLower()) == 0)
            {
                lock (this.msgTask)
                {
                    this.Translate();
                }
            }
        }

        public void Register(Action<Telegram> action)
        {
            this.RegisterAsListener(action);
            this.RegisterAsPublisher();
        }

        public void RegisterAsListener(Action<Telegram> action)
        {
            this.subscription = this.aggregator.Subscribe<Telegram>(action, this.address.Owner);
        }

        public void RegisterAsPublisher()
        {
            this.OnValuePublisher = (Telegram telegram) => this.aggregator.Publish<Telegram>(telegram, true);
        }

        public void RegisterAsRelay()
        {
            this.RegisterAsListener(new Action<Telegram>(this.RelayAction));
        }

        public void RegisterAsRelay(Func<Telegram, bool> prePublishAction)
        {
            Action<Telegram> action = new Action<Telegram>(this.RelayAction);
            this.menuTreeCollection.PrePublishAction = prePublishAction;
            this.RegisterAsListener(action);
        }

        private void RelayAction(Telegram telegram)
        {
            lock (this.msgTask)
            {
                this.msgTask = this.msgTask.ContinueWith((Task ant) => this.RelayActionTask(telegram), TaskContinuationOptions.None);
            }
        }

        private void RelayActionSelectMenu(Telegram telegram)
        {
            if (telegram == null)
            {
                return;
            }
            if (telegram.Value == null)
            {
                return;
            }
            if (telegram.Value is PlcTreeElementStringRaw)
            {
                PlcTreeElementStringRaw value = (PlcTreeElementStringRaw)telegram.Value;
                this.menuTreeCollection.SelectMenuWithId((int)value.DataId, true, true);
            }
        }

        private void RelayActionTask(Telegram telegram)
        {
            int command = telegram.Command;
            if (command == 0)
            {
                this.RelayActionUpdateMenu(telegram);
                return;
            }
            if (command != 1)
            {
                return;
            }
            this.RelayActionSelectMenu(telegram);
        }

        private void RelayActionUpdateMenu(Telegram telegram)
        {
            if (string.Compare(this.address.Target, telegram.Address.Owner) == 0)
            {
                return;
            }
            TreeElement<UiElementData<string>> treeElement = new TreeElement<UiElementData<string>>()
            {
                DataLinks = new Dictionary<uint, UiElementData<string>>()
            };
            if (telegram.ServiceLink is PlcServices)
            {
                this.plcServices = telegram.ServiceLink as PlcServices;
            }
            if (treeElement.Map(telegram.Value))
            {
                if (treeElement.Identity.Level == 0)
                {
                    this.menuTreeCollection.AttachRootTreeElement(treeElement);
                    this.menuTreeCollection.AttachCommand(new MenuTreeItemCommand(this.command.InfoCommand));
                    this.Acknowledge(telegram);
                    this.menuTreeCollection.CreateLookup();
                    return;
                }
                this.menuTreeCollection.UpdateElement(treeElement);
            }
        }

        private void Translate()
        {
            this.menuTreeCollection.UpdateAttachedTreeElementData(true);
        }
    }
}