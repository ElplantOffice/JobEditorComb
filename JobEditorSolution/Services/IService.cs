using System;
using System.Runtime.CompilerServices;

namespace Services
{
	public interface IService
	{
		event EventHandler OnNotify;
	}
}