using Communication.Plc.Shared;
using Messages;
using Patterns.EventAggregator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Models
{
    public class UITreeElementStringCreator
    {
        private List<object> flatList;

        private List<short> treeLevelId = new List<short>();

        private int dataId;

        private List<ISubscription<Telegram>> subScriptions = new List<ISubscription<Telegram>>();

        public UITreeElementStringCreator(List<object> flatList)
        {
            this.flatList = flatList;
        }

        public void AddChildAction(ref PlcTreeElementStringRaw newItem, ref PlcTreeElementStringRaw parentItem, PlcControlDataRaw data, IAddress address, Expression<Action<Telegram>> expression, bool enaUpdateMenuInfo, short row, short column)
        {
            this.AddChildAction(ref newItem, ref parentItem, data, address, expression, enaUpdateMenuInfo, 0, row, column);
        }

        public void AddChildAction(ref PlcTreeElementStringRaw newItem, ref PlcTreeElementStringRaw parentItem, PlcControlDataRaw data, IAddress address, Expression<Action<Telegram>> expression, bool enaUpdateMenuInfo, short level, short row, short column)
        {
            IEventAggregator instance = SingletonProvider<EventAggregator>.Instance;
            if (expression != null && expression.NodeType != ExpressionType.Lambda)
            {
                throw new ArgumentException("Value must be a lamda expression", "expression");
            }
            newItem.Data = data;
            newItem.Data.Visibility = 2;
            newItem.Id.LevelId = (short)(parentItem.Id.LevelId + 1);
            if (expression != null)
            {
                string name = VariableInfo.GetName(expression);
                Action<Telegram> action = expression.Compile();
                IAddress address1 = new Address(address, name);
                if (this.subScriptions.Count <= 0)
                {
                    this.subScriptions.Add(instance.Subscribe<Telegram>(action, address1.Owner));
                }
                else if ((
                    from x in this.subScriptions
                    where x.Target == address1.Owner
                    select x).FirstOrDefault<ISubscription<Telegram>>() == null)
                {
                    this.subScriptions.Add(instance.Subscribe<Telegram>(action, address1.Owner));
                }
                newItem.Handle.Target = address1.Owner;
            }
            if (enaUpdateMenuInfo)
            {
                newItem.Handle.Type = 4;
            }
            else
            {
                newItem.Handle.Type = 7;
            }
            if (newItem.Id.LevelId < this.treeLevelId.Count)
            {
                this.treeLevelId[newItem.Id.LevelId] = (short)(this.treeLevelId[newItem.Id.LevelId] + 1);
            }
            else
            {
                this.treeLevelId.Add(0);
            }
            newItem.Id.Id = this.treeLevelId[newItem.Id.LevelId];
            newItem.Id.ParentId = parentItem.Id.Id;
            newItem.Handle.Command = level;
            newItem.Location.Column = column;
            newItem.Location.Row = row;
            newItem.DataId = (uint)this.dataId;
            this.flatList.Add(newItem);
        }

        public void AddChildDefault(ref PlcTreeElementStringRaw newItem, ref PlcTreeElementStringRaw parentItem, PlcControlDataRaw data, bool enaUpdateMenuInfo, short row, short column)
        {
            newItem.Data = data;
            newItem.Data.Visibility = 2;
            newItem.Id.LevelId = (short)(parentItem.Id.LevelId + 1);
            if (newItem.Id.LevelId < this.treeLevelId.Count)
            {
                this.treeLevelId[newItem.Id.LevelId] = (short)(this.treeLevelId[newItem.Id.LevelId] + 1);
            }
            else
            {
                this.treeLevelId.Add(0);
            }
            if (!enaUpdateMenuInfo)
            {
                newItem.Handle.Type = 6;
            }
            newItem.Id.Id = this.treeLevelId[newItem.Id.LevelId];
            newItem.Id.ParentId = parentItem.Id.Id;
            newItem.Handle.Command = 1;
            newItem.Location.Column = column;
            newItem.Location.Row = row;
            newItem.DataId = (uint)this.dataId;
            this.flatList.Add(newItem);
        }

        public void AddChildToLevel(ref PlcTreeElementStringRaw newItem, ref PlcTreeElementStringRaw parentItem, PlcControlDataRaw data, bool enaUpdateMenuInfo, short level, short row, short column)
        {
            newItem.Data = data;
            newItem.Data.Visibility = 2;
            newItem.Id.LevelId = (short)(parentItem.Id.LevelId + 1);
            if (newItem.Id.LevelId < this.treeLevelId.Count)
            {
                this.treeLevelId[newItem.Id.LevelId] = (short)(this.treeLevelId[newItem.Id.LevelId] + 1);
            }
            else
            {
                this.treeLevelId.Add(0);
            }
            newItem.Id.Id = this.treeLevelId[newItem.Id.LevelId];
            newItem.Id.ParentId = parentItem.Id.Id;
            if (!enaUpdateMenuInfo)
            {
                newItem.Handle.Type = 6;
            }
            newItem.Handle.Command = level;
            newItem.Location.Column = column;
            newItem.Location.Row = row;
            newItem.DataId = (uint)this.dataId;
            this.flatList.Add(newItem);
        }

        public void AddChildToNode(ref PlcTreeElementStringRaw newItem, ref PlcTreeElementStringRaw parentItem, PlcControlDataRaw data, PlcTreeElementStringRaw node, IAddress address, Expression<Action<Telegram>> expression, bool enaUpdateMenuInfo, short row, short column)
        {
            IEventAggregator instance = SingletonProvider<EventAggregator>.Instance;
            if (expression != null && expression.NodeType != ExpressionType.Lambda)
            {
                throw new ArgumentException("Value must be a lamda expression", "expression");
            }
            newItem.Data = data;
            newItem.Data.Visibility = 2;
            newItem.Id.LevelId = (short)(parentItem.Id.LevelId + 1);
            if (expression != null)
            {
                string name = VariableInfo.GetName(expression);
                Action<Telegram> action = expression.Compile();
                IAddress address1 = new Address(address, name);
                if (this.subScriptions.Count <= 0)
                {
                    this.subScriptions.Add(instance.Subscribe<Telegram>(action, address1.Owner));
                }
                else if ((
                    from x in this.subScriptions
                    where x.Target == address1.Owner
                    select x).FirstOrDefault<ISubscription<Telegram>>() == null)
                {
                    this.subScriptions.Add(instance.Subscribe<Telegram>(action, address1.Owner));
                }
                newItem.Handle.Target = address1.Owner;
            }
            if (newItem.Id.LevelId < this.treeLevelId.Count)
            {
                this.treeLevelId[newItem.Id.LevelId] = (short)(this.treeLevelId[newItem.Id.LevelId] + 1);
            }
            else
            {
                this.treeLevelId.Add(0);
            }
            newItem.Id.Id = this.treeLevelId[newItem.Id.LevelId];
            newItem.Id.ParentId = parentItem.Id.Id;
            newItem.Handle.Command = (short)node.DataId;
            if (enaUpdateMenuInfo)
            {
                newItem.Handle.Type = 5;
            }
            else
            {
                newItem.Handle.Type = 8;
            }
            newItem.Location.Column = column;
            newItem.Location.Row = row;
            newItem.DataId = (uint)this.dataId;
            this.flatList.Add(newItem);
        }

        public PlcControlDataRaw InitMenuData(bool enable, string text, string icon)
        {
            PlcControlDataRaw plcControlDataRaw = new PlcControlDataRaw()
            {
                TextId = text,
                ContentText = text,
                IsEnabled = enable,
                ContentImage = icon
            };
            this.dataId++;
            return plcControlDataRaw;
        }

        public PlcControlDataRaw InitMenuData(bool enable, string text, string icon, int localisation)
        {
            PlcControlDataRaw plcControlDataRaw = new PlcControlDataRaw()
            {
                TextId = text,
                ContentText = text,
                IsEnabled = enable,
                ContentImage = icon,
                Localisation = (short)localisation
            };
            this.dataId++;
            return plcControlDataRaw;
        }

        public void RootMenu(ref PlcTreeElementStringRaw root, PlcControlDataRaw data)
        {
            root.Data = data;
            root.Data.Visibility = 2;
            root.Id.LevelId = 0;
            if (root.Id.LevelId < this.treeLevelId.Count)
            {
                this.treeLevelId[root.Id.LevelId] = (short)(this.treeLevelId[root.Id.LevelId] + 1);
            }
            else
            {
                this.treeLevelId.Add(0);
            }
            root.Id.Id = this.treeLevelId[root.Id.LevelId];
            root.Id.ParentId = -1;
            root.Location.Column = 0;
            root.Location.Row = 0;
            root.DataId = (uint)this.dataId;
            this.flatList.Add(root);
        }
    }
}