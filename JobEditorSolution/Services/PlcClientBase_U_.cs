using Communication.Plc;
using Communication.Plc.Channel;
using Communication.Plc.Shared;
using Messages;
using Models;
using Patterns.EventAggregator;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Services
{
	public abstract class PlcClientBase<U> : CommonBase<U>
	where U : IServiceBaseDerived
	{
		public virtual CommandHandle Handler
		{
			get;
			set;
		}

		public virtual Communication.Plc.Channel.PlcServices PlcServices
		{
			get;
			set;
		}

		public List<ServiceDataObject> RawDataObjects
		{
			get;
			private set;
		}

		protected PlcClientBase()
		{
		}

		private List<ServiceDataObject> GetDataObjects()
		{
			List<ServiceDataObject> serviceDataObjects = new List<ServiceDataObject>();
			if (this.Handler == null)
			{
				return serviceDataObjects;
			}
			if (this.PlcServices == null)
			{
				return serviceDataObjects;
			}
			this.Handler = this.Handler.Reset();
			while (this.Handler != null)
			{
				ServiceDataObject serviceDataObject = new ServiceDataObject();
				PlcEventSourceRaw plcEventSourceRaw = new PlcEventSourceRaw();
				if (this.PlcServices.GetEventSource(this.Handler.Handle, out plcEventSourceRaw))
				{
					serviceDataObject.telegram = (PlcTelegramRaw)this.PlcServices.ReadByRefPntrHandle(plcEventSourceRaw.DataPntrHandle);
				}
				serviceDataObject.DataPntrHandle = this.Handler.DataPntrHandle;
				serviceDataObject.DataPntr = this.PlcServices.GetRefPntr(serviceDataObject.DataPntrHandle);
				serviceDataObject.Data = this.PlcServices.ReadByRefPntrHandle(serviceDataObject.DataPntrHandle);
				serviceDataObject.Source = plcEventSourceRaw;
				serviceDataObjects.Add(serviceDataObject);
				if (this.Handler.NextPntr.Offset == 0)
				{
					break;
				}
				PlcCommandHandleRaw plcCommandHandleRaw = (PlcCommandHandleRaw)this.PlcServices.ReadObject(this.Handler.NextPntr);
				this.Handler = this.Handler.Get(plcCommandHandleRaw);
			}
			this.Handler = this.Handler.Reset();
			this.RawDataObjects = serviceDataObjects;
			return serviceDataObjects;
		}

		public CommandHandle GetHandler(object obj)
		{
			return CommandHandle.Init(obj);
		}

		public void NotifyPlc(int cmd)
		{
			if (this.PlcServices == null)
			{
				return;
			}
			string name = this.RawDataObjects[0].telegram.CommAddress.Owner.Name;
			if (this.PlcServices.IsPlcAddress(name))
			{
				name = this.PlcServices.AddChannel(name);
			}
			Address address = new Address(this.Sender.Owner, name, "", null);
			this.aggregator.Publish<Telegram>(new Telegram(address, cmd, null, null), true);
		}

		public T Read<T>()
		where T : IMappable
		{
			T data = default(T);
			this.RawDataObjects = this.GetDataObjects();
			foreach (ServiceDataObject rawDataObject in this.RawDataObjects)
			{
				if (!(rawDataObject.DataPntr.Type == data.TypeName) || rawDataObject.DataPntr.IsArray)
				{
					continue;
				}
				data = (T)rawDataObject.Data;
			}
			return data;
		}

		public List<T> ReadList<T>()
		where T : IMappable
		{
			List<T> ts = new List<T>();
			this.RawDataObjects = this.GetDataObjects();
			foreach (ServiceDataObject rawDataObject in this.RawDataObjects)
			{
				T t = default(T);
				t = t;
				if (!(rawDataObject.DataPntr.Type == t.TypeName) || !rawDataObject.DataPntr.IsArray)
				{
					continue;
				}
				foreach (object datum in rawDataObject.Data as List<object>)
				{
					ts.Add((T)datum);
				}
			}
			return ts;
		}

		public void RegisterType<T>()
		where T : IMappable
		{
			PlcTypeResolver.Register<T>();
		}

		public void Write<T>(T element)
		where T : IMappable
		{
			if (this.PlcServices == null)
			{
				return;
			}
			foreach (ServiceDataObject rawDataObject in this.RawDataObjects)
			{
				if (!(rawDataObject.DataPntr.Type == element.TypeName) || rawDataObject.DataPntr.IsArray)
				{
					continue;
				}
				rawDataObject.Data = element;
				this.PlcServices.WriteObject(rawDataObject.Data, rawDataObject.DataPntr);
				this.NotifyPlc(0);
			}
		}

		public void WriteList<T>(List<T> elements)
		where T : IMappable
		{
			if (this.PlcServices == null)
			{
				return;
			}
			foreach (ServiceDataObject rawDataObject in this.RawDataObjects)
			{
				T element = default(T);
				element = element;
				if (!(rawDataObject.DataPntr.Type == element.TypeName) || !rawDataObject.DataPntr.IsArray)
				{
					continue;
				}
				if (elements.Count <= 0)
				{
					break;
				}
				List<object> objs = new List<object>();
				foreach (T el in elements)
				{
					objs.Add(el);
				}
				rawDataObject.Data = objs;
				this.PlcServices.WriteObject(rawDataObject.Data, rawDataObject.DataPntr);
				rawDataObject.DataPntr.Count = (short)elements.Count;
				this.PlcServices.WriteByRef(rawDataObject.DataPntr, rawDataObject.DataPntrHandle);
				this.NotifyPlc(0);
				return;
			}
		}
	}
}