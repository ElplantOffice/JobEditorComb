using Messages;
using Patterns.EventAggregator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Communication.Pc
{
	public class SocketChannel : ICommChannel
	{
		private System.Timers.Timer _timerKeepAliveSend;

		private System.Timers.Timer _timerKeepAliveTimeOut;

		private byte[] _recvbuffer;

		private int _recvbuffernumberunhandledbytes;

		private IPacket _packets = new MessagePacket();

		public Messages.Address Address
		{
			get
			{
				return JustDecompileGenerated_get_Address();
			}
			set
			{
				JustDecompileGenerated_set_Address(value);
			}
		}

		private Messages.Address JustDecompileGenerated_Address_k__BackingField;

		public Messages.Address JustDecompileGenerated_get_Address()
		{
			return this.JustDecompileGenerated_Address_k__BackingField;
		}

		private void JustDecompileGenerated_set_Address(Messages.Address value)
		{
			this.JustDecompileGenerated_Address_k__BackingField = value;
		}

		public string ChannelId
		{
			get
			{
				return JustDecompileGenerated_get_ChannelId();
			}
			set
			{
				JustDecompileGenerated_set_ChannelId(value);
			}
		}

		private string JustDecompileGenerated_ChannelId_k__BackingField;

		public string JustDecompileGenerated_get_ChannelId()
		{
			return this.JustDecompileGenerated_ChannelId_k__BackingField;
		}

		public void JustDecompileGenerated_set_ChannelId(string value)
		{
			this.JustDecompileGenerated_ChannelId_k__BackingField = value;
		}

		public bool ConnectionValid
		{
			get
			{
				bool flag = false;
				Status channelState = this.GetChannelState();
				if (channelState == Status.Connected || channelState == Status.Linked)
				{
					flag = true;
				}
				return flag;
			}
		}

		public Socket IPSocket
		{
			get;
			private set;
		}

		public Messages.Address OwnerAddress
		{
			get;
			private set;
		}

		public IPacket Packet
		{
			get
			{
				return this._packets;
			}
			set
			{
				this.Send(value);
			}
		}

		public SocketState State
		{
			get;
			set;
		}

		public SocketSettings TCPIPSettings
		{
			get;
			set;
		}

		public SocketChannel(ICommSettings settings, IChannelState state, Messages.Address owner)
		{
			this.TCPIPSettings = settings as SocketSettings;
			this.State = state as SocketState;
			this.State.CommState = Status.Disconnected;
			this._recvbuffer = new byte[this.TCPIPSettings.Buffers.ReceiveBufferSize];
			this.OwnerAddress = owner;
			this.Address = owner.GetBasePart(typeof(SocketChannel));
			this.IPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		}

		public void Connect()
		{
			this.State.CommState = Status.Connecting;
			this.IPSocket.ReceiveBufferSize = this.TCPIPSettings.Buffers.ReceiveBufferSize;
			this.IPSocket.SendBufferSize = this.TCPIPSettings.Buffers.SendBufferSize;
			this.IPSocket.Blocking = false;
			AsyncCallback asyncCallback = new AsyncCallback(this.OnConnect);
			//this.IPSocket.BeginConnect(this.TCPIPSettings.IpEndPoint, asyncCallback, this.IPSocket);
		}

		public int Disconnect()
		{
			if (this.State.CommState == Status.Disconnecting)
			{
				return 0;
			}
			if (this.State.CommState == Status.Disconnected)
			{
				return 0;
			}
			this.SetChannelState(Status.Disconnecting);
			if (this.IPSocket.Connected)
			{
				this.IPSocket.Disconnect(true);
			}
			this.SetChannelState(Status.Disconnected);
			return 0;
		}

		private void DisposeKeepAliveTimers()
		{
			if (this._timerKeepAliveSend != null)
			{
				this._timerKeepAliveSend.Elapsed -= new ElapsedEventHandler(this.OnTimerKeepAliveSend);
				this._timerKeepAliveSend.Stop();
				this._timerKeepAliveSend.Dispose();
				this._timerKeepAliveSend = null;
			}
			if (this._timerKeepAliveTimeOut != null)
			{
				this._timerKeepAliveTimeOut.Elapsed -= new ElapsedEventHandler(this.OnTimerKeepAliveTimeOut);
				this._timerKeepAliveTimeOut.Stop();
				this._timerKeepAliveTimeOut.Dispose();
				this._timerKeepAliveTimeOut = null;
			}
		}

		public static ICommChannel GetChannel(ICommChannel channel, string application)
		{
			SocketChannel socketChannel = channel as SocketChannel;
			socketChannel.ChannelId = application;
			return socketChannel;
		}

		public Status GetChannelOldState()
		{
			return this.State.OldCommState;
		}

		public Status GetChannelState()
		{
			return this.State.CommState;
		}

		public ICommSettings GetSettings()
		{
			return this.TCPIPSettings;
		}

		public void Listen()
		{
			this.State.CommState = Status.Connecting;
			this.IPSocket.Bind(this.TCPIPSettings.IpEndPoint);
			this.IPSocket.Listen(2);
			this.IPSocket.ReceiveBufferSize = this.TCPIPSettings.Buffers.ReceiveBufferSize;
			this.IPSocket.SendBufferSize = this.TCPIPSettings.Buffers.SendBufferSize;
			this.IPSocket.Blocking = false;
			this.IPSocket.BeginAccept(new AsyncCallback(this.OnAccept), this.IPSocket);
		}

		public void OnAccept(IAsyncResult ar)
		{
			Socket asyncState = (Socket)ar.AsyncState;
			try
			{
				Socket socket = asyncState.EndAccept(ar);
				if (socket.Connected)
				{
					this.OnAcceptChannel(this, new OnAcceptEventArgs(socket.Connected, socket));
				}
				asyncState.BeginAccept(new AsyncCallback(this.OnAccept), this.IPSocket);
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.Data.ToString());
				this.SetChannelState(Status.Error);
			}
		}

		public void OnConnect(IAsyncResult ar)
		{
			Socket asyncState = (Socket)ar.AsyncState;
			try
			{
				if (!asyncState.Connected)
				{
					this.OnConnectChannel(this, new OnConnectEventArgs(asyncState.Connected));
				}
				else
				{
					this.SetupRecieveCallback(asyncState);
					this.OnConnectChannel(this, new OnConnectEventArgs(asyncState.Connected));
				}
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.Data.ToString());
				this.SetChannelState(Status.Error);
			}
		}

		private void OnRecievedData(IAsyncResult ar)
		{
			Task.Factory.StartNew(() => {
				Socket asyncState = (Socket)ar.AsyncState;
				try
				{
					int num = 0;
					int u003cu003e4_this = asyncState.EndReceive(ar);
					if (u003cu003e4_this != 0)
					{
						if (this.State.CommState == Status.Linked)
						{
							this.ResetKeepAliveTimeOutTimer();
						}
						u003cu003e4_this += this._recvbuffernumberunhandledbytes;
						while (true)
						{
							if (!this.ConnectionValid)
							{
								return;
							}
							else if ((int)this._recvbuffer.Length >= 5)
							{
								if (u003cu003e4_this < num + 5)
								{
									break;
								}
								byte[] numArray = new byte[4];
								Array.Copy(this._recvbuffer, num, numArray, 0, 4);
								int num1 = BitConverter.ToInt32(numArray, 0);
								byte[] numArray1 = new byte[1];
								Array.Copy(this._recvbuffer, num + 4, numArray1, 0, 1);
								bool flag = BitConverter.ToBoolean(numArray1, 0);
								if (num1 < 0)
								{
									throw new ArgumentOutOfRangeException("datasize", (object)num1, "Negative packet data size value recieved");
								}
								int num2 = 5 + num1;
								int maxPacketBufferSize = this.TCPIPSettings.Buffers.MaxPacketBufferSize;
								if (maxPacketBufferSize == 0)
								{
									maxPacketBufferSize = this.TCPIPSettings.Buffers.ReceiveBufferSize;
								}
								if (maxPacketBufferSize != -1 && num2 > maxPacketBufferSize)
								{
									throw new ArgumentOutOfRangeException("packetsize", (object)num2, "Packet size value exceeds max recieve buffer size");
								}
								if ((int)this._recvbuffer.Length >= num2)
								{
									if (u003cu003e4_this < num + num2)
									{
										break;
									}
									if (num1 > 0)
									{
										byte[] numArray2 = new byte[num1];
										Array.Copy(this._recvbuffer, num + 5, numArray2, 0, num1);
										IPacket packet = this.UnPackData(numArray2, flag);
										if (this.ConnectionValid)
										{
											this.OnReceivePacket(this, new OnReceiveEventArgs(packet));
										}
										else
										{
											return;
										}
									}
									num = num + 5 + num1;
								}
								else
								{
									Array.Resize<byte>(ref this._recvbuffer, num2);
									break;
								}
							}
							else
							{
								Array.Resize<byte>(ref this._recvbuffer, 5);
								break;
							}
						}
						if (num > 0 && u003cu003e4_this > num)
						{
							Array.Copy(this._recvbuffer, num, this._recvbuffer, 0, u003cu003e4_this - num);
						}
						this._recvbuffernumberunhandledbytes = u003cu003e4_this - num;
						if (this.ConnectionValid)
						{
							this.SetupRecieveCallback(asyncState);
							return;
						}
					}
					else
					{
						this.Disconnect();
					}
				}
				catch (SocketException socketException1)
				{
					SocketException socketException = socketException1;
					if (this.ConnectionValid)
					{
						Console.WriteLine(socketException.Data.ToString());
						this.SetChannelState(Status.Error);
					}
					return;
				}
				catch (ArgumentOutOfRangeException argumentOutOfRangeException)
				{
					Console.WriteLine(argumentOutOfRangeException.ToString());
					this.SetChannelState(Status.Error);
					return;
				}
				catch (Exception exception)
				{
					Console.WriteLine(exception.ToString());
					this.SetChannelState(Status.Error);
					return;
				}
			});
		}

		private void OnTimerKeepAliveSend(object source, ElapsedEventArgs e)
		{
			if (this._timerKeepAliveSend == null)
			{
				return;
			}
			if (!this._timerKeepAliveSend.Enabled)
			{
				return;
			}
			MessagePacket messagePacket = new MessagePacket();
			Telegram telegram = new Telegram(null, 1, null, this.ChannelId);
			messagePacket.Add(telegram);
			this.Packet = messagePacket;
		}

		private void OnTimerKeepAliveTimeOut(object source, ElapsedEventArgs e)
		{
			if (this._timerKeepAliveTimeOut == null)
			{
				return;
			}
			if (!this._timerKeepAliveTimeOut.Enabled)
			{
				return;
			}
			this.DisposeKeepAliveTimers();
			if (this.GetChannelState() == Status.Linked)
			{
				this.SetChannelState(Status.KeepAliveTimeOut);
			}
		}

		public byte[] PackData(IPacket packet)
		{
			byte[] bytes;
			byte[] numArray;
			byte[] array = null;
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			MemoryStream memoryStream = new MemoryStream();
			bool flag = false;
			try
			{
				memoryStream.Position = (long)5;
				Serializer.Serialize(memoryStream, packet);
				array = memoryStream.ToArray();
			}
			catch (Exception exception1)
			{
				IDictionary data = exception1.Data;
				try
				{
					memoryStream.Position = (long)5;
					binaryFormatter.Serialize(memoryStream, packet);
					array = memoryStream.ToArray();
					flag = true;
					bytes = BitConverter.GetBytes((int)array.Length - 5);
					for (int i = 0; i < (int)bytes.Length; i++)
					{
						array.SetValue(bytes[i], i);
					}
					numArray = BitConverter.GetBytes(flag);
					array.SetValue(numArray[0], 4);
					return array;
				}
				catch (Exception exception)
				{
					Console.WriteLine(exception.Data.ToString());
				}
			}
			bytes = BitConverter.GetBytes((int)array.Length - 5);
			for (int j = 0; j < (int)bytes.Length; j++)
			{
				array.SetValue(bytes[j], j);
			}
			numArray = BitConverter.GetBytes(flag);
			array.SetValue(numArray[0], 4);
			return array;
		}

		private void ResetKeepAliveSendTimer()
		{
			if (this._timerKeepAliveSend != null)
			{
				this._timerKeepAliveSend.Stop();
				this._timerKeepAliveSend.Start();
			}
		}

		private void ResetKeepAliveTimeOutTimer()
		{
			if (this._timerKeepAliveTimeOut != null)
			{
				this._timerKeepAliveTimeOut.Stop();
				this._timerKeepAliveTimeOut.Start();
			}
		}

		public int Send(IPacket packet)
		{
			if (this.GetChannelState() == Status.Linked)
			{
				bool flag = false;
				if (packet.Messages.Count == 1)
				{
					IMessage item = packet.Messages[0];
					if (item.Address == null && item.Command == 1)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					this.ResetKeepAliveSendTimer();
				}
			}
			if (this.ConnectionValid)
			{
				try
				{
					byte[] numArray = null;
					numArray = this.PackData(packet);
					SocketError socketError = SocketError.WouldBlock;
					while (this.ConnectionValid)
					{
						this.IPSocket.Send(numArray, 0, (int)numArray.Length, SocketFlags.None, out socketError);
						if (socketError != SocketError.WouldBlock)
						{
							return 0;
						}
						else if (this.ConnectionValid)
						{
							Thread.Sleep(30);
						}
						else
						{
							return 0;
						}
					}
				}
				catch (SocketException socketException1)
				{
					SocketException socketException = socketException1;
					if (this.ConnectionValid)
					{
						Console.WriteLine(socketException.Data.ToString());
						this.SetChannelState(Status.Error);
					}
				}
			}
			return 0;
		}

		public void SetChannelState(Status state)
		{
			this.State.CommState = state;
			if (this.State.CommState != Status.Linked)
			{
				this.DisposeKeepAliveTimers();
			}
			else
			{
				this.SetupKeepAliveTimers();
			}
			if (this.OnChangeStateChannel != null)
			{
				this.OnChangeStateChannel(this, new OnChangeStateEventArgs(this.State, this.ChannelId));
			}
		}

		private void SetupKeepAliveTimers()
		{
			ICommSettings settings = this.GetSettings();
			if (settings == null)
			{
				return;
			}
			if (settings.Timers == null)
			{
				return;
			}
			if (!settings.Timers.KeepAliveEnable)
			{
				return;
			}
			if (this._timerKeepAliveSend != null)
			{
				this._timerKeepAliveSend.Elapsed -= new ElapsedEventHandler(this.OnTimerKeepAliveSend);
				this._timerKeepAliveSend.Stop();
				this._timerKeepAliveSend.Dispose();
				this._timerKeepAliveSend = null;
			}
			this._timerKeepAliveSend = new System.Timers.Timer();
			this._timerKeepAliveSend.Elapsed += new ElapsedEventHandler(this.OnTimerKeepAliveSend);
			this._timerKeepAliveSend.Interval = (double)settings.Timers.KeepAliveSendInterval;
			this._timerKeepAliveSend.Start();
			if (this._timerKeepAliveTimeOut != null)
			{
				this._timerKeepAliveTimeOut.Elapsed -= new ElapsedEventHandler(this.OnTimerKeepAliveTimeOut);
				this._timerKeepAliveTimeOut.Stop();
				this._timerKeepAliveTimeOut.Dispose();
				this._timerKeepAliveTimeOut = null;
			}
			this._timerKeepAliveTimeOut = new System.Timers.Timer();
			this._timerKeepAliveTimeOut.Elapsed += new ElapsedEventHandler(this.OnTimerKeepAliveTimeOut);
			this._timerKeepAliveTimeOut.Interval = (double)settings.Timers.KeepAliveTimeOutTime;
			this._timerKeepAliveTimeOut.Start();
		}

		private void SetupRecieveCallback(Socket socket)
		{
			try
			{
				AsyncCallback asyncCallback = new AsyncCallback(this.OnRecievedData);
				socket.BeginReceive(this._recvbuffer, this._recvbuffernumberunhandledbytes, (int)this._recvbuffer.Length - this._recvbuffernumberunhandledbytes, SocketFlags.None, asyncCallback, socket);
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.Data.ToString());
				this.SetChannelState(Status.Error);
			}
		}

		public ICommChannel ShallowCopy(OnAcceptEventArgs e)
		{
			SocketChannel address = this.MemberwiseClone() as SocketChannel;
			address.Address = new Messages.Address(this.Address, typeof(SocketChannel));
			address.State = new SocketState(this.Address);
			address.IPSocket = e.SocketAfterAccept;
			return address;
		}

		public void StartReceiving()
		{
			this.SetupRecieveCallback(this.IPSocket);
		}

		public IPacket UnPackData(byte[] recvBuffer, bool usingBinaryFormatter)
		{
			Task<IPacket> task = Task.Factory.StartNew<IPacket>(() => {
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				MemoryStream memoryStream = new MemoryStream();
				IPacket packet = null;
				memoryStream = new MemoryStream(recvBuffer);
				try
				{
					if (!usingBinaryFormatter)
					{
						memoryStream.Position = (long)0;
						packet = Serializer.Deserialize(memoryStream) as IPacket;
					}
					else
					{
						memoryStream.Position = (long)0;
						packet = binaryFormatter.Deserialize(memoryStream) as IPacket;
					}
				}
				catch (FileNotFoundException fileNotFoundException)
				{
					string message = fileNotFoundException.Message;
					Assembly.LoadFile(fileNotFoundException.FileName);
					packet = binaryFormatter.Deserialize(memoryStream) as IPacket;
				}
				return packet;
			});
			task.Wait();
			return task.Result;
		}

		public event OnAcceptEventHandler OnAcceptChannel;

		public event OnChangeStateEventHandler OnChangeStateChannel;

		public event OnConnectEventHandler OnConnectChannel;

		public event OnReceiveEventHandler OnReceivePacket;
	}
}