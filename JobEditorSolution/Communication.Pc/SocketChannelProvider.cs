using Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace Communication.Pc
{
	public class SocketChannelProvider : IChannelProvider
	{
		private Dictionary<string, ICommChannel> _undefinedchannels = new Dictionary<string, ICommChannel>();

		private Dictionary<string, ICommChannel> _definedchannels = new Dictionary<string, ICommChannel>();

		private ICommChannel _listenchannel;

		private Messages.Address Address;

		public SocketChannelProvider(Messages.Address parent, ICommChannel listenChannel)
		{
			this.AssignParent(parent);
			this._listenchannel = listenChannel;
		}

		public SocketChannelProvider(Messages.Address parent)
		{
			this.AssignParent(parent);
		}

		public ICommChannel AddClientChannel(ICommChannel channel)
		{
			SocketChannel str = (SocketChannel)channel;
			int port = (channel.GetSettings() as SocketSettings).IpEndPoint.Port;
			str.ChannelId = port.ToString();
			lock (this._undefinedchannels)
			{
				this._undefinedchannels.Remove(str.ChannelId);
				this._undefinedchannels.Add(str.ChannelId, channel);
			}
			return str;
		}

		public ICommChannel AddServerChannel(ICommChannel channel, OnAcceptEventArgs e)
		{
			SocketChannel str = channel.ShallowCopy(e) as SocketChannel;
			IPEndPoint remoteEndPoint = (IPEndPoint)str.IPSocket.RemoteEndPoint;
			str.ChannelId = remoteEndPoint.Port.ToString();
			lock (this._definedchannels)
			{
				this._undefinedchannels.Remove(str.ChannelId);
				this._undefinedchannels.Add(str.ChannelId, str);
			}
			return str;
		}

		private void AssignParent(Messages.Address parent)
		{
			this.Address = new Messages.Address(parent, typeof(SocketChannelProvider));
		}

		private T DeepClone<T>(T obj)
		{
			T t;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(memoryStream, obj);
				memoryStream.Position = (long)0;
				t = (T)binaryFormatter.Deserialize(memoryStream);
			}
			return t;
		}

		public void DefineChannel(string channelId, string application)
		{
			if (channelId != null)
			{
				lock (this._undefinedchannels)
				{
					if (this._undefinedchannels.ContainsKey(channelId))
					{
						ICommChannel channel = SocketChannel.GetChannel(this._undefinedchannels[channelId], application);
						lock (this._undefinedchannels)
						{
							this._definedchannels.Remove(application);
							this._definedchannels.Add(application, channel);
						}
						this._undefinedchannels.Remove(channelId);
					}
				}
			}
		}

		public ICommChannel GetDefinedChannel(string application)
		{
			ICommChannel item;
			lock (this._definedchannels)
			{
				if (!this._definedchannels.ContainsKey(application))
				{
					item = null;
				}
				else
				{
					item = this._definedchannels[application];
				}
			}
			return item;
		}

		public List<ICommChannel> GetDefinedChannelsList()
		{
			List<ICommChannel> list;
			lock (this._definedchannels)
			{
				list = this._definedchannels.Values.ToList<ICommChannel>();
			}
			return list;
		}

		public ICommChannel GetUnDefinedChannel(string schannelId)
		{
			ICommChannel item;
			lock (this._undefinedchannels)
			{
				if (!this._undefinedchannels.ContainsKey(schannelId))
				{
					item = null;
				}
				else
				{
					item = this._undefinedchannels[schannelId];
				}
			}
			return item;
		}

		public void RemoveClientChannel(ICommChannel channel)
		{
			SocketChannel socketChannel = (SocketChannel)channel;
			lock (this._definedchannels)
			{
				this._definedchannels.Remove(socketChannel.ChannelId);
			}
		}
	}
}