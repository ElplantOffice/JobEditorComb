using Messages;
using Models;
using Patterns.EventAggregator;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace JobEditor.Menu
{
	public class MenuModel : ModelBase
	{
		private const int rowSize = 2;

		private IEventAggregator aggregator = SingletonProvider<EventAggregator>.Instance;

		private UiLocalisation localisation = SingletonProvider<UiLocalisation>.Instance;

		private UIProtoType uiProtoType;

		public ModelAttributedEventType<string> Grid
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> InfoCommand
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> InfoMenu
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> Menu00
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> Menu01
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> Menu02
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> Menu03
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> Menu04
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> Menu05
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> Menu06
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> Menu07
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> Menu08
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> Menu09
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> Menu10
		{
			get;
			private set;
		}

		public List<ModelAttributedEventType<string>> MenuButtons
		{
			get;
			private set;
		}

		public ModelTreeEventType Tree
		{
			get;
			private set;
		}

		public MenuModel(UIProtoType uiProtoType)
		{
			if (uiProtoType == null)
			{
				throw new ArgumentNullException("uiProtoType");
			}
			this.uiProtoType = uiProtoType;
			Address address = new Address(uiProtoType.Model.Address.Owner, uiProtoType.ViewModels[0].Address.Owner, "", null);
			this.InfoMenu = new ModelAttributedEventType<string>(this, address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(MenuModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(MenuModel).GetMethod("get_InfoMenu").MethodHandle)), new ParameterExpression[0]));
			this.InfoMenu.RegisterAsPublisher(new Func<Telegram, bool>(this.localisation.Translate));
			this.InfoCommand = new ModelAttributedEventType<string>(this, address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(MenuModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(MenuModel).GetMethod("get_InfoCommand").MethodHandle)), new ParameterExpression[0]));
			this.InfoCommand.RegisterAsPublisher();
			this.aggregator.Subscribe<Telegram>(new Action<Telegram>(this.UpdateInfoMenu), string.Concat(uiProtoType.Model.Address.Owner, ".UpdateMenuInfo"));
			this.MenuButtons = new List<ModelAttributedEventType<string>>();
			List<ModelAttributedEventType<string>> menuButtons = this.MenuButtons;
			ModelAttributedEventType<string> modelAttributedEventType = new ModelAttributedEventType<string>(this, address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(MenuModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(MenuModel).GetMethod("get_Menu00").MethodHandle)), new ParameterExpression[0]));
			ModelAttributedEventType<string> modelAttributedEventType1 = modelAttributedEventType;
			this.Menu00 = modelAttributedEventType;
			menuButtons.Add(modelAttributedEventType1);
			List<ModelAttributedEventType<string>> modelAttributedEventTypes = this.MenuButtons;
			ModelAttributedEventType<string> modelAttributedEventType2 = new ModelAttributedEventType<string>(this, address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(MenuModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(MenuModel).GetMethod("get_Menu01").MethodHandle)), new ParameterExpression[0]));
			modelAttributedEventType1 = modelAttributedEventType2;
			this.Menu01 = modelAttributedEventType2;
			modelAttributedEventTypes.Add(modelAttributedEventType1);
			List<ModelAttributedEventType<string>> menuButtons1 = this.MenuButtons;
			ModelAttributedEventType<string> modelAttributedEventType3 = new ModelAttributedEventType<string>(this, address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(MenuModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(MenuModel).GetMethod("get_Menu02").MethodHandle)), new ParameterExpression[0]));
			modelAttributedEventType1 = modelAttributedEventType3;
			this.Menu02 = modelAttributedEventType3;
			menuButtons1.Add(modelAttributedEventType1);
			List<ModelAttributedEventType<string>> modelAttributedEventTypes1 = this.MenuButtons;
			ModelAttributedEventType<string> modelAttributedEventType4 = new ModelAttributedEventType<string>(this, address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(MenuModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(MenuModel).GetMethod("get_Menu03").MethodHandle)), new ParameterExpression[0]));
			modelAttributedEventType1 = modelAttributedEventType4;
			this.Menu03 = modelAttributedEventType4;
			modelAttributedEventTypes1.Add(modelAttributedEventType1);
			List<ModelAttributedEventType<string>> menuButtons2 = this.MenuButtons;
			ModelAttributedEventType<string> modelAttributedEventType5 = new ModelAttributedEventType<string>(this, address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(MenuModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(MenuModel).GetMethod("get_Menu04").MethodHandle)), new ParameterExpression[0]));
			modelAttributedEventType1 = modelAttributedEventType5;
			this.Menu04 = modelAttributedEventType5;
			menuButtons2.Add(modelAttributedEventType1);
			List<ModelAttributedEventType<string>> modelAttributedEventTypes2 = this.MenuButtons;
			ModelAttributedEventType<string> modelAttributedEventType6 = new ModelAttributedEventType<string>(this, address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(MenuModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(MenuModel).GetMethod("get_Menu05").MethodHandle)), new ParameterExpression[0]));
			modelAttributedEventType1 = modelAttributedEventType6;
			this.Menu05 = modelAttributedEventType6;
			modelAttributedEventTypes2.Add(modelAttributedEventType1);
			List<ModelAttributedEventType<string>> menuButtons3 = this.MenuButtons;
			ModelAttributedEventType<string> modelAttributedEventType7 = new ModelAttributedEventType<string>(this, address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(MenuModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(MenuModel).GetMethod("get_Menu06").MethodHandle)), new ParameterExpression[0]));
			modelAttributedEventType1 = modelAttributedEventType7;
			this.Menu06 = modelAttributedEventType7;
			menuButtons3.Add(modelAttributedEventType1);
			List<ModelAttributedEventType<string>> modelAttributedEventTypes3 = this.MenuButtons;
			ModelAttributedEventType<string> modelAttributedEventType8 = new ModelAttributedEventType<string>(this, address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(MenuModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(MenuModel).GetMethod("get_Menu07").MethodHandle)), new ParameterExpression[0]));
			modelAttributedEventType1 = modelAttributedEventType8;
			this.Menu07 = modelAttributedEventType8;
			modelAttributedEventTypes3.Add(modelAttributedEventType1);
			List<ModelAttributedEventType<string>> menuButtons4 = this.MenuButtons;
			ModelAttributedEventType<string> modelAttributedEventType9 = new ModelAttributedEventType<string>(this, address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(MenuModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(MenuModel).GetMethod("get_Menu08").MethodHandle)), new ParameterExpression[0]));
			modelAttributedEventType1 = modelAttributedEventType9;
			this.Menu08 = modelAttributedEventType9;
			menuButtons4.Add(modelAttributedEventType1);
			List<ModelAttributedEventType<string>> modelAttributedEventTypes4 = this.MenuButtons;
			ModelAttributedEventType<string> modelAttributedEventType10 = new ModelAttributedEventType<string>(this, address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(MenuModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(MenuModel).GetMethod("get_Menu09").MethodHandle)), new ParameterExpression[0]));
			modelAttributedEventType1 = modelAttributedEventType10;
			this.Menu09 = modelAttributedEventType10;
			modelAttributedEventTypes4.Add(modelAttributedEventType1);
			List<ModelAttributedEventType<string>> menuButtons5 = this.MenuButtons;
			ModelAttributedEventType<string> modelAttributedEventType11 = new ModelAttributedEventType<string>(this, address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(MenuModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(MenuModel).GetMethod("get_Menu10").MethodHandle)), new ParameterExpression[0]));
			modelAttributedEventType1 = modelAttributedEventType11;
			this.Menu10 = modelAttributedEventType11;
			menuButtons5.Add(modelAttributedEventType1);
			this.Tree = new ModelTreeEventType(this.MenuButtons, this.InfoCommand, null, address, 2, Expression.Lambda<Func<ModelTreeEventType>>(Expression.Property(Expression.Constant(this, typeof(MenuModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(MenuModel).GetMethod("get_Tree").MethodHandle)), new ParameterExpression[0]));
			this.Tree.AddControls(this.MenuButtons);
			this.Tree.RegisterAsRelay(new Func<Telegram, bool>(this.localisation.Translate));
			base.Show(uiProtoType.ViewModels[0], TelegramCommand.Show);
		}

		public void Translate(Telegram telegram)
		{
			UiElementData<string> value = (UiElementData<string>)telegram.Value;
			value.ContentText = string.Concat(value.ContentText, " translated");
		}

		public void UpdateInfoMenu(Telegram telegram)
		{
			if (telegram.Value is string)
			{
				this.InfoMenu.ContentText = (string)telegram.Value;
			}
		}
	}
}