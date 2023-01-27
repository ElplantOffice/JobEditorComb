using Messages;
using Models;
using Patterns.EventAggregator;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace JobEditor.AppBar
{
	public class AppBarModel : ModelBase
	{
		private List<ISubscription<Telegram>> SubScription = new List<ISubscription<Telegram>>();

		private IEventAggregator aggregator = SingletonProvider<EventAggregator>.Instance;

		private UiLocalisation localisation = SingletonProvider<UiLocalisation>.Instance;

		private UIProtoType uiProtoType;

		public ModelAttributedEventType<string> Button1
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> Button2
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> Button3
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> Button4
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> Button5
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> Button6
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> Button7
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> Button8
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> ImageButton1
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> ImageButton2
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> ImageButton3
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> ImageButton4
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> ImageButton5
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> ImageButton6
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> ImageButton7
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> ImageButton8
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> LabelButton1
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> LabelButton2
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> LabelButton3
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> LabelButton4
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> LabelButton5
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> LabelButton6
		{
			get;
			private set;
		}

		public ModelAttributedEventType<string> LabelButton8
		{
			get;
			private set;
		}

		public AppBarModel(UIProtoType uiProtoType)
		{
			if (uiProtoType == null)
			{
				throw new ArgumentNullException("uiProtoType");
			}
			this.uiProtoType = uiProtoType;
			Address address = new Address(uiProtoType.Model.Address.Owner, uiProtoType.ViewModels[0].Address.Owner, "", null);
			this.LabelButton1 = new ModelAttributedEventType<string>(this, uiProtoType.Model.Address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarModel).GetMethod("get_LabelButton1").MethodHandle)), new ParameterExpression[0]));
			this.LabelButton1.RegisterRelay();
			this.ImageButton1 = new ModelAttributedEventType<string>(this, uiProtoType.Model.Address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarModel).GetMethod("get_ImageButton1").MethodHandle)), new ParameterExpression[0]));
			this.ImageButton1.RegisterRelay();
			this.Button1 = new ModelAttributedEventType<string>(this, uiProtoType.Model.Address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarModel).GetMethod("get_Button1").MethodHandle)), new ParameterExpression[0]));
			this.Button1.RegisterRelay();
			this.LabelButton2 = new ModelAttributedEventType<string>(this, uiProtoType.Model.Address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarModel).GetMethod("get_LabelButton2").MethodHandle)), new ParameterExpression[0]));
			this.LabelButton2.RegisterRelay();
			this.ImageButton2 = new ModelAttributedEventType<string>(this, uiProtoType.Model.Address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarModel).GetMethod("get_ImageButton2").MethodHandle)), new ParameterExpression[0]));
			this.ImageButton2.RegisterRelay();
			this.Button2 = new ModelAttributedEventType<string>(this, uiProtoType.Model.Address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarModel).GetMethod("get_Button2").MethodHandle)), new ParameterExpression[0]));
			this.Button2.RegisterRelay();
			this.LabelButton3 = new ModelAttributedEventType<string>(this, uiProtoType.Model.Address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarModel).GetMethod("get_LabelButton3").MethodHandle)), new ParameterExpression[0]));
			this.LabelButton3.RegisterRelay();
			this.ImageButton3 = new ModelAttributedEventType<string>(this, uiProtoType.Model.Address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarModel).GetMethod("get_ImageButton3").MethodHandle)), new ParameterExpression[0]));
			this.ImageButton3.RegisterRelay();
			this.Button3 = new ModelAttributedEventType<string>(this, uiProtoType.Model.Address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarModel).GetMethod("get_Button3").MethodHandle)), new ParameterExpression[0]));
			this.Button3.RegisterRelay();
			this.LabelButton4 = new ModelAttributedEventType<string>(this, uiProtoType.Model.Address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarModel).GetMethod("get_LabelButton4").MethodHandle)), new ParameterExpression[0]));
			this.LabelButton4.RegisterRelay();
			this.ImageButton4 = new ModelAttributedEventType<string>(this, uiProtoType.Model.Address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarModel).GetMethod("get_ImageButton4").MethodHandle)), new ParameterExpression[0]));
			this.ImageButton4.RegisterRelay();
			this.Button4 = new ModelAttributedEventType<string>(this, uiProtoType.Model.Address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarModel).GetMethod("get_Button4").MethodHandle)), new ParameterExpression[0]));
			this.Button4.RegisterRelay();
			this.LabelButton5 = new ModelAttributedEventType<string>(this, uiProtoType.Model.Address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarModel).GetMethod("get_LabelButton5").MethodHandle)), new ParameterExpression[0]));
			this.LabelButton5.RegisterRelay();
			this.ImageButton5 = new ModelAttributedEventType<string>(this, uiProtoType.Model.Address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarModel).GetMethod("get_ImageButton5").MethodHandle)), new ParameterExpression[0]));
			this.ImageButton5.RegisterRelay();
			this.Button5 = new ModelAttributedEventType<string>(this, uiProtoType.Model.Address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarModel).GetMethod("get_Button5").MethodHandle)), new ParameterExpression[0]));
			this.Button5.RegisterRelay();
			this.LabelButton6 = new ModelAttributedEventType<string>(this, uiProtoType.Model.Address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarModel).GetMethod("get_LabelButton6").MethodHandle)), new ParameterExpression[0]));
			this.LabelButton6.RegisterRelay();
			this.ImageButton6 = new ModelAttributedEventType<string>(this, uiProtoType.Model.Address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarModel).GetMethod("get_ImageButton6").MethodHandle)), new ParameterExpression[0]));
			this.ImageButton6.RegisterRelay();
			this.Button6 = new ModelAttributedEventType<string>(this, uiProtoType.Model.Address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarModel).GetMethod("get_Button6").MethodHandle)), new ParameterExpression[0]));
			this.Button6.RegisterRelay();
			this.ImageButton7 = new ModelAttributedEventType<string>(this, uiProtoType.Model.Address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarModel).GetMethod("get_ImageButton7").MethodHandle)), new ParameterExpression[0]));
			this.ImageButton7.RegisterRelay();
			this.Button7 = new ModelAttributedEventType<string>(this, uiProtoType.Model.Address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarModel).GetMethod("get_Button7").MethodHandle)), new ParameterExpression[0]));
			this.Button7.RegisterRelay();
			this.LabelButton8 = new ModelAttributedEventType<string>(this, uiProtoType.Model.Address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarModel).GetMethod("get_LabelButton8").MethodHandle)), new ParameterExpression[0]));
			this.LabelButton8.RegisterRelay();
			this.ImageButton8 = new ModelAttributedEventType<string>(this, uiProtoType.Model.Address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarModel).GetMethod("get_ImageButton8").MethodHandle)), new ParameterExpression[0]));
			this.ImageButton8.RegisterRelay();
			this.Button8 = new ModelAttributedEventType<string>(this, uiProtoType.Model.Address, Expression.Lambda<Func<ModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarModel).GetMethod("get_Button8").MethodHandle)), new ParameterExpression[0]));
			this.Button8.RegisterRelay();
			base.Show(uiProtoType, TelegramCommand.Show);
		}

		public void Translate(Telegram telegram)
		{
		}
	}
}