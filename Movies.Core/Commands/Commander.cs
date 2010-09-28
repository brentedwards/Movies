using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using Microsoft.Practices.Composite.Presentation.Commands;

namespace Movies.Core.Commands
{
	/// <summary>
	/// Class that holds all Dependency Properties and Shared methods to allow an
	/// event of a DependencyObject class to be attached to a Command.
	/// </summary>
	public class Commander
	{
		private static readonly DependencyProperty EventCommandBehaviorProperty = DependencyProperty.RegisterAttached(
			"EventCommandBehavior",
			typeof(CommandBehaviorBinder<Control>),
			typeof(Commander),
			null);

		/// <summary>
		/// Command to execute on when the event is fired.
		/// </summary>
		public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
			"Command",
			typeof(ICommand),
			typeof(Commander),
			new PropertyMetadata(OnSetCommandCallback));

		/// <summary>
		/// Command parameter to supply on command execution.
		/// </summary>
		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached(
			"CommandParameter",
			typeof(Object),
			typeof(Commander),
			new PropertyMetadata(OnSetCommandParameterCallback));

		/// <summary>
		/// Event name to bind command to.
		/// </summary>
		public static readonly DependencyProperty EventNameProperty = DependencyProperty.RegisterAttached(
			"EventName",
			typeof(String),
			typeof(Commander),
			null);

		/// <summary>
		/// Sets the <see cref="ICommand"/> to execute when the event is fired.
		/// </summary>
		/// <param name="control">DependencyObject to attach Command.</param>
		/// <param name="command">Command to attach.</param>
		public static void SetCommand(Control control, ICommand command)
		{
			control.SetValue(CommandProperty, command);
		}

		/// <summary>
		/// Retrieves the <see cref="ICommand"/> attached to the <see cref="Control"/>.
		/// </summary>
		/// <param name="control">DependencyObject containing the Command dependency property.</param>
		/// <returns>The value of the command attached.</returns>
		public static ICommand GetCommand(Control control)
		{
			return control.GetValue(CommandProperty) as ICommand;
		}

		/// <summary>
		/// Sets the value for the CommandParameter attached property on the provided <see cref="Control"/>.
		/// </summary>
		/// <param name="control">DependencyObject to attach CommandParameter.</param>
		/// <param name="parameter">Parameter value to attach.</param>
		public static void SetCommandParameter(Control control, Object parameter)
		{
			control.SetValue(CommandParameterProperty, parameter);
		}

		/// <summary>
		/// Gets the value in CommandParameter attached property on the provided <see cref="Control"/>.
		/// </summary>
		/// <param name="control">DependencyObject that has the CommandParameter.</param>
		/// <returns>The value of the property.</returns>
		public static Object GetCommandParameter(Control control)
		{
			return control.GetValue(CommandParameterProperty);
		}

		/// <summary>
		/// Sets the value for the EventName attached property on the provided <see cref="Control"/>.
		/// </summary>
		/// <param name="control">DependencyObject to attach the EventName to.</param>
		/// <param name="eventName">The name of the event to attach.</param>
		public static void SetEventName(Control control, String eventName)
		{
			control.SetValue(EventNameProperty, eventName);
		}

		/// <summary>
		/// Gets the value in EventName attached property on the provided <see cref="Control"/>.
		/// </summary>
		/// <param name="control">The DependencyObject that has the EventName.</param>
		/// <returns>The value of the property.</returns>
		public static String GetEventName(Control control)
		{
			return control.GetValue(EventNameProperty).ToString();
		}

		private static void OnSetCommandCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			var control = dependencyObject as Control;
			if (control != null)
			{
				var behavior = GetOrCreateBehavior(control);
				behavior.Command = e.NewValue as ICommand;
			}
		}

		private static void OnSetCommandParameterCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			var control = dependencyObject as Control;
			if (control != null)
			{
				var behavior = GetOrCreateBehavior(control);
				behavior.CommandParameter = e.NewValue;
			}
		}

		private static CommandBehaviorBase<Control> GetOrCreateBehavior(Control control)
		{
			var behavior = control.GetValue(EventCommandBehaviorProperty) as CommandBehaviorBase<Control>;

			if (behavior == null)
			{
				behavior = CreateCommandBehavior(control, control.GetValue(EventNameProperty).ToString());
				control.SetValue(EventCommandBehaviorProperty, behavior);
			}

			return behavior;
		}

		private static CommandBehaviorBase<Control> CreateCommandBehavior(Control control, String eventName)
		{
			var type = control.GetType();
			var behavior = new CommandBehaviorBinder<Control>(control);
			var info = type.GetEvent(eventName);

			if (info != null)
			{
				var methodInfo = behavior.GetType().GetMethod("Execute");
				var handler = Delegate.CreateDelegate(info.EventHandlerType, behavior, methodInfo, true);
				info.AddEventHandler(control, handler);
			}
			else
			{
				throw new ArgumentException(String.Format("Target object '{0}' doesn't have the event '{1}'.", type.Name, eventName));
			}

			return behavior;
		}
	}
}
