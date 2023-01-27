using Messages;

namespace Communication.Pc
{
	public interface ICommSettings
	{
		Messages.Address Address
		{
			get;
		}

		SocketSettingsBuffers Buffers
		{
			get;
		}

		SocketSettingsTimers Timers
		{
			get;
		}
	}
}