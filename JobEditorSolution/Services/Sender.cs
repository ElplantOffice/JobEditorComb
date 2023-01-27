using Messages;
using Models;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Services
{
	public class Sender
	{
		public string ApplicationEndPoint
		{
			get
			{
				return "Application";
			}
		}

		public string AppName
		{
			get;
			private set;
		}

		public string EndPoint
		{
			get
			{
				return "Start";
			}
		}

		public bool IsAppSender
		{
			get;
			private set;
		}

		public bool IsPlcSender
		{
			get;
			private set;
		}

		public string Owner
		{
			get
			{
				return string.Concat(new string[] { this.AppName, ".", this.SenderType, ".", this.EndPoint });
			}
		}

		public string PartialTarget
		{
			get
			{
				return string.Concat(".", this.ReceiverType, ".", this.EndPoint);
			}
		}

		public string ReceiverType
		{
			get;
			private set;
		}

		public string SenderType
		{
			get;
			private set;
		}

		public Sender(UIProtoType uiProtoType, Type receiverType, IModel model = null)
		{
			this.AppName = Sender.ExtractAppName(uiProtoType.Model.Address.Owner);
			this.SenderType = uiProtoType.Model.Type.Name;
			if (uiProtoType.Model.Type.BaseType.IsGenericType && uiProtoType.Model.Type.BaseType.GetGenericTypeDefinition() == typeof(ServiceBase<>))
			{
				this.IsPlcSender = true;
			}
			if (receiverType != null)
			{
				this.ReceiverType = receiverType.Name;
				this.IsAppSender = true;
			}
		}

		public string ApplicationTarget(string RecieverOwner)
		{
			return string.Concat(Sender.ExtractAppName(RecieverOwner), ".", this.ApplicationEndPoint);
		}

		public static string ExtractAppName(string adressLine)
		{
			if (!adressLine.Contains("."))
			{
				return adressLine;
			}
			return adressLine.Substring(0, adressLine.IndexOf('.'));
		}

		public string RecieverTarget(string RecieverOwner)
		{
			if (!RecieverOwner.Contains("."))
			{
				return string.Concat(RecieverOwner, this.PartialTarget);
			}
			return string.Concat(RecieverOwner.Substring(0, RecieverOwner.IndexOf('.')), this.PartialTarget);
		}
	}
}