 
using Models;
using Patterns.EventAggregator;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Utils;

namespace JobEditor.Menu
{
	public class MenuViewModel : ViewModelBase
	{
		private IEventAggregator aggregator = SingletonProvider<EventAggregator>.Instance;

		private ViewModelProtoType viewModelProtoType;

		private const int NumberMenus = 16;

		public Toggle toggle;

		public ViewModelAttributedEventType<string> InfoCommand
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> InfoMenu
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> Menu00
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> Menu01
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> Menu02
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> Menu03
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> Menu04
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> Menu05
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> Menu06
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> Menu07
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> Menu08
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> Menu09
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> Menu10
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<bool> ToggleControl
		{
			get;
			set;
		}

		public MenuViewModel(ViewModelProtoType viewModelProtoType)
		{
			this.viewModelProtoType = viewModelProtoType;
			this.InfoMenu = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(MenuViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(MenuViewModel).GetMethod("get_InfoMenu").MethodHandle)), new ParameterExpression[0]));
			this.InfoCommand = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(MenuViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(MenuViewModel).GetMethod("get_InfoCommand").MethodHandle)), new ParameterExpression[0]));
			this.Menu00 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(MenuViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(MenuViewModel).GetMethod("get_Menu00").MethodHandle)), new ParameterExpression[0]));
			this.Menu00.Register(null, null);
			this.Menu01 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(MenuViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(MenuViewModel).GetMethod("get_Menu01").MethodHandle)), new ParameterExpression[0]));
			this.Menu01.Register(null, null);
			this.Menu02 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(MenuViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(MenuViewModel).GetMethod("get_Menu02").MethodHandle)), new ParameterExpression[0]));
			this.Menu02.Register(null, null);
			this.Menu03 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(MenuViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(MenuViewModel).GetMethod("get_Menu03").MethodHandle)), new ParameterExpression[0]));
			this.Menu03.Register(null, null);
			this.Menu04 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(MenuViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(MenuViewModel).GetMethod("get_Menu04").MethodHandle)), new ParameterExpression[0]));
			this.Menu04.Register(null, null);
			this.Menu05 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(MenuViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(MenuViewModel).GetMethod("get_Menu05").MethodHandle)), new ParameterExpression[0]));
			this.Menu05.Register(null, null);
			this.Menu06 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(MenuViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(MenuViewModel).GetMethod("get_Menu06").MethodHandle)), new ParameterExpression[0]));
			this.Menu06.Register(null, null);
			this.Menu07 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(MenuViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(MenuViewModel).GetMethod("get_Menu07").MethodHandle)), new ParameterExpression[0]));
			this.Menu07.Register(null, null);
			this.Menu08 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(MenuViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(MenuViewModel).GetMethod("get_Menu08").MethodHandle)), new ParameterExpression[0]));
			this.Menu08.Register(null, null);
			this.Menu09 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(MenuViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(MenuViewModel).GetMethod("get_Menu09").MethodHandle)), new ParameterExpression[0]));
			this.Menu09.Register(null, null);
			this.Menu10 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(MenuViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(MenuViewModel).GetMethod("get_Menu10").MethodHandle)), new ParameterExpression[0]));
			this.Menu10.Register(null, null);
			this.ToggleControl = new ViewModelAttributedEventType<bool>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<bool>>>(Expression.Property(Expression.Constant(this, typeof(MenuViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(MenuViewModel).GetMethod("get_ToggleControl").MethodHandle)), new ParameterExpression[0]));
			this.toggle = new Toggle((long)250, this.ToggleControl);
			this.InfoMenu.RegisterAsListener(null);
			this.InfoCommand.RegisterAsListener(null);
		}
	}
}