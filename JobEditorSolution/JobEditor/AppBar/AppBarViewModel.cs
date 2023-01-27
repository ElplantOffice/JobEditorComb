using Models;
using Patterns.EventAggregator;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace JobEditor.AppBar
{
	public class AppBarViewModel : ViewModelBase
	{
		private IEventAggregator aggregator = SingletonProvider<EventAggregator>.Instance;

		private ViewModelProtoType viewModelProtoType;

		public ViewModelAttributedEventType<string> Button1
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> Button2
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> Button3
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> Button4
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> Button5
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> Button6
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> Button7
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> Button8
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> DesktopCommand
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> ImageButton1
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> ImageButton2
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> ImageButton3
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> ImageButton4
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> ImageButton5
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> ImageButton6
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> ImageButton7
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> ImageButton8
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> LabelButton1
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> LabelButton2
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> LabelButton3
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> LabelButton4
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> LabelButton5
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> LabelButton6
		{
			get;
			private set;
		}

		public ViewModelAttributedEventType<string> LabelButton8
		{
			get;
			private set;
		}

		public AppBarViewModel(ViewModelProtoType viewModelProtoType)
		{
            if (viewModelProtoType == null)
			{
				throw new ArgumentNullException("viewModelProtoType");
			}
			this.viewModelProtoType = viewModelProtoType;
			this.LabelButton1 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarViewModel).GetMethod("get_LabelButton1").MethodHandle)), new ParameterExpression[0]));
			this.LabelButton1.RegisterAsListener(null);
			this.ImageButton1 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarViewModel).GetMethod("get_ImageButton1").MethodHandle)), new ParameterExpression[0]));
			this.ImageButton1.RegisterAsListener(null);
			this.Button1 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarViewModel).GetMethod("get_Button1").MethodHandle)), new ParameterExpression[0]));
			this.Button1.Register(null, null);
			this.LabelButton2 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarViewModel).GetMethod("get_LabelButton2").MethodHandle)), new ParameterExpression[0]));
			this.LabelButton2.RegisterAsListener(null);
			this.ImageButton2 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarViewModel).GetMethod("get_ImageButton2").MethodHandle)), new ParameterExpression[0]));
			this.ImageButton2.RegisterAsListener(null);
			this.Button2 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarViewModel).GetMethod("get_Button2").MethodHandle)), new ParameterExpression[0]));
			this.Button2.Register(null, null);
			this.LabelButton3 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarViewModel).GetMethod("get_LabelButton3").MethodHandle)), new ParameterExpression[0]));
			this.LabelButton3.RegisterAsListener(null);
			this.ImageButton3 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarViewModel).GetMethod("get_ImageButton3").MethodHandle)), new ParameterExpression[0]));
			this.ImageButton3.RegisterAsListener(null);
			this.Button3 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarViewModel).GetMethod("get_Button3").MethodHandle)), new ParameterExpression[0]));
			this.Button3.Register(null, null);
			this.LabelButton4 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarViewModel).GetMethod("get_LabelButton4").MethodHandle)), new ParameterExpression[0]));
			this.LabelButton4.RegisterAsListener(null);
			this.ImageButton4 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarViewModel).GetMethod("get_ImageButton4").MethodHandle)), new ParameterExpression[0]));
			this.ImageButton4.RegisterAsListener(null);
			this.Button4 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarViewModel).GetMethod("get_Button4").MethodHandle)), new ParameterExpression[0]));
			this.Button4.Register(null, null);
			this.LabelButton5 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarViewModel).GetMethod("get_LabelButton5").MethodHandle)), new ParameterExpression[0]));
			this.LabelButton5.RegisterAsListener(null);
			this.ImageButton5 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarViewModel).GetMethod("get_ImageButton5").MethodHandle)), new ParameterExpression[0]));
			this.ImageButton5.RegisterAsListener(null);
			this.Button5 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarViewModel).GetMethod("get_Button5").MethodHandle)), new ParameterExpression[0]));
			this.Button5.Register(null, null);
			this.LabelButton6 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarViewModel).GetMethod("get_LabelButton6").MethodHandle)), new ParameterExpression[0]));
			this.LabelButton6.RegisterAsListener(null);
			this.ImageButton6 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarViewModel).GetMethod("get_ImageButton6").MethodHandle)), new ParameterExpression[0]));
			this.ImageButton6.RegisterAsListener(null);
			this.Button6 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarViewModel).GetMethod("get_Button6").MethodHandle)), new ParameterExpression[0]));
			this.Button6.Register(null, null);
			this.LabelButton8 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarViewModel).GetMethod("get_LabelButton8").MethodHandle)), new ParameterExpression[0]));
			this.LabelButton8.RegisterAsListener(null);
			this.ImageButton8 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarViewModel).GetMethod("get_ImageButton8").MethodHandle)), new ParameterExpression[0]));
			this.ImageButton8.RegisterAsListener(null);
			this.Button8 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarViewModel).GetMethod("get_Button8").MethodHandle)), new ParameterExpression[0]));
			this.Button8.Register(null, null);
			this.ImageButton7 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarViewModel).GetMethod("get_ImageButton7").MethodHandle)), new ParameterExpression[0]));
			this.ImageButton7.RegisterAsListener(null);
			this.Button7 = new ViewModelAttributedEventType<string>(this, viewModelProtoType, Expression.Lambda<Func<ViewModelAttributedEventType<string>>>(Expression.Property(Expression.Constant(this, typeof(AppBarViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AppBarViewModel).GetMethod("get_Button7").MethodHandle)), new ParameterExpression[0]));
			this.Button7.Register(null, null);
		}
	}
}