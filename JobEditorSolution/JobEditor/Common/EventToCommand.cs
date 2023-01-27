using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace JobEditor.Common
{
	public class EventToCommand : TriggerAction<FrameworkElement>, ICommandSource
	{
		public readonly static DependencyProperty EventArgsProperty;

		public readonly static DependencyProperty CommandParameterProperty;

		public readonly static DependencyProperty CommandProperty;

		public readonly static DependencyProperty MustToggleIsEnabledProperty;

		public ICommand Command
		{
			get
			{
				return JustDecompileGenerated_get_Command();
			}
			set
			{
				JustDecompileGenerated_set_Command(value);
			}
		}

		public ICommand JustDecompileGenerated_get_Command()
		{
			return (ICommand)base.GetValue(EventToCommand.CommandProperty);
		}

		public void JustDecompileGenerated_set_Command(ICommand value)
		{
			base.SetValue(EventToCommand.CommandProperty, value);
		}

		public object CommandParameter
		{
			get
			{
				return JustDecompileGenerated_get_CommandParameter();
			}
			set
			{
				JustDecompileGenerated_set_CommandParameter(value);
			}
		}

		public object JustDecompileGenerated_get_CommandParameter()
		{
			return base.GetValue(EventToCommand.CommandParameterProperty);
		}

		public void JustDecompileGenerated_set_CommandParameter(object value)
		{
			base.SetValue(EventToCommand.CommandParameterProperty, value);
		}

		public IInputElement CommandTarget
		{
			get
			{
				return base.AssociatedObject;
			}
		}

		public object EventArgs
		{
			get
			{
				return base.GetValue(EventToCommand.EventArgsProperty);
			}
			set
			{
				base.SetValue(EventToCommand.EventArgsProperty, value);
			}
		}

		public bool MustToggleIsEnabled
		{
			get
			{
				return (bool)base.GetValue(EventToCommand.MustToggleIsEnabledProperty);
			}
			set
			{
				base.SetValue(EventToCommand.MustToggleIsEnabledProperty, value);
			}
		}

		static EventToCommand()
		{
			EventToCommand.EventArgsProperty = DependencyProperty.Register("EventArgs", typeof(object), typeof(EventToCommand));
			EventToCommand.CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(EventToCommand), new PropertyMetadata(null, (DependencyObject s, DependencyPropertyChangedEventArgs e) => {
				EventToCommand eventToCommand = s as EventToCommand;
				if (eventToCommand == null)
				{
					return;
				}
				if (eventToCommand.AssociatedObject == null)
				{
					return;
				}
				eventToCommand.EnableDisableElement();
			}));
			EventToCommand.CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(EventToCommand), new PropertyMetadata(null, (DependencyObject s, DependencyPropertyChangedEventArgs e) => EventToCommand.OnCommandChanged(s as EventToCommand, e)));
			EventToCommand.MustToggleIsEnabledProperty = DependencyProperty.Register("MustToggleIsEnabled", typeof(bool), typeof(EventToCommand), new PropertyMetadata(false, (DependencyObject s, DependencyPropertyChangedEventArgs e) => {
				EventToCommand eventToCommand = s as EventToCommand;
				if (eventToCommand == null)
				{
					return;
				}
				if (eventToCommand.AssociatedObject == null)
				{
					return;
				}
				eventToCommand.EnableDisableElement();
			}));
		}

		public EventToCommand()
		{
		}

		private bool AssociatedElementIsDisabled()
		{
			if (base.AssociatedObject == null)
			{
				return false;
			}
			return !base.AssociatedObject.IsEnabled;
		}

		public bool CanExecute()
		{
			if (this.Command == null)
			{
				return false;
			}
			RoutedCommand command = this.Command as RoutedCommand;
			if (command == null)
			{
				return this.Command.CanExecute(this.CommandParameter);
			}
			return command.CanExecute(this.CommandParameter, this.CommandTarget);
		}

		private void EnableDisableElement()
		{
			if (base.AssociatedObject == null)
			{
				return;
			}
			if (this.MustToggleIsEnabled && this.Command != null)
			{
				base.AssociatedObject.IsEnabled = this.CanExecute();
			}
		}

		public void Execute()
		{
			if (this.Command == null)
			{
				return;
			}
			RoutedCommand command = this.Command as RoutedCommand;
			if (command == null)
			{
				this.Command.Execute(this.CommandParameter);
				return;
			}
			command.Execute(this.CommandParameter, this.CommandTarget);
		}

		public void Invoke()
		{
			this.Invoke(null);
		}

		protected override void Invoke(object parameter)
		{
			this.EventArgs = parameter;
			if (this.AssociatedElementIsDisabled())
			{
				return;
			}
			if (this.CanExecute())
			{
				this.Execute();
			}
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			this.EnableDisableElement();
		}

		private void OnCommandCanExecuteChanged(object sender, System.EventArgs e)
		{
			this.EnableDisableElement();
		}

		private static void OnCommandChanged(EventToCommand element, DependencyPropertyChangedEventArgs e)
		{
			if (element == null)
			{
				return;
			}
			if (e.OldValue != null)
			{
				((ICommand)e.OldValue).CanExecuteChanged -= new EventHandler(element.OnCommandCanExecuteChanged);
			}
			ICommand newValue = (ICommand)e.NewValue;
			if (newValue != null)
			{
				newValue.CanExecuteChanged += new EventHandler(element.OnCommandCanExecuteChanged);
			}
			element.EnableDisableElement();
		}
	}
}