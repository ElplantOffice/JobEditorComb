 
using Models;
using Patterns.EventAggregator;
using Services;
using System;

namespace Services.TRL
{
	public class AppService : ServiceBase<AppService>, IServiceBaseDerived, IModel
	{
		private UiLocalisation localisation = SingletonProvider<UiLocalisation>.Instance;

		public AppService(UIProtoType uiProtoType)
		{
			this.Init(uiProtoType, null);
			base.RegisterType<PlcAppClientDataRaw>();
		}

		public override void OnNotifiedByPlc(int command)
		{
			PlcAppClientDataRaw plcAppClientDataRaw = base.Read<PlcAppClientDataRaw>();
			this.localisation.Settings.LocalisationId = plcAppClientDataRaw.LanguageId;
			(new FileResources()).Data.Set<int>("LanguageId", plcAppClientDataRaw.LanguageId);
			this.RaiseOnNotify(this, new EventArgs());
		}
	}
}