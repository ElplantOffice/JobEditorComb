using Models;
using Services;
using System;

namespace Services.TRL
{
	public class FileService : ServiceBase<FileService>, IServiceBaseDerived, IModel
	{
		public FileService(UIProtoType uiProtoType)
		{
			this.Init(uiProtoType, null);
		}

		public override void OnNotifiedByPlc(int command)
		{
		}
	}
}