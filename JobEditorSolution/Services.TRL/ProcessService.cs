using Models;
using Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Services.TRL
{
	public class ProcessService : ServiceBase<ProcessService>, IServiceBaseDerived, IModel
	{
		private Task processTask;

		public ProcessService(UIProtoType uiProtoType)
		{
			this.Init(uiProtoType, null);
			base.RegisterType<PlcProcessClientDataRaw>();
		}

		private void ActivateConfig(string path)
		{
			string directoryName = Path.GetDirectoryName(path);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
			Process[] processesByName = Process.GetProcessesByName(fileNameWithoutExtension);
			if (processesByName.Length != 0)
			{
				Process[] processArray = processesByName;
				for (int i = 0; i < (int)processArray.Length; i++)
				{
					processArray[i].CloseMainWindow();
				}
				return;
			}
			Process.Start(new ProcessStartInfo(path)
			{
				WorkingDirectory = directoryName
			});
			int num = 0;
			do
			{
				processesByName = Process.GetProcessesByName(fileNameWithoutExtension);
				if (processesByName.Length != 0)
				{
					ProcessService.SetForegroundWindow(processesByName[0].MainWindowHandle);
					return;
				}
				Thread.Sleep(100);
				num++;
			}
			while (num <= 10);
		}

		private void HandleCommand(int command)
		{
		}

		public override void OnNotifiedByClient(int command)
		{
		}

		public override void OnNotifiedByPlc(int command)
		{
			switch (command)
			{
				case 1:
				{
					Process.Start("C:\\Users\\Pieter\\Desktop\\TC3_Mini_Booklet.pdf");
					return;
				}
				case 2:
				{
					ProcessService.SearchGoogle("Beckhoff");
					return;
				}
				case 3:
				{
					PlcProcessClientDataRaw plcProcessClientDataRaw = base.Read<PlcProcessClientDataRaw>();
					if (!string.IsNullOrEmpty(plcProcessClientDataRaw.Path))
					{
						this.processTask = Task.Factory.StartNew(() => this.ActivateConfig(plcProcessClientDataRaw.Path));
					}
					return;
				}
				default:
				{
					return;
				}
			}
		}

		private static void SearchGoogle(string t)
		{
			Process.Start(string.Concat("http://google.com/search?q=", t));
		}

		[DllImport("User32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern IntPtr SetForegroundWindow(IntPtr hWnd);

		private enum Command
		{
			OpenSomePdf = 1,
			OpenWebsite = 2,
			StartStopExe = 3
		}
	}
}