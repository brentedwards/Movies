using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Presentation.Commands;
using System.Windows.Controls;

namespace Movies.Core.Commands
{
	/// <summary>
	/// Behavior that allows controls that derive from <see cref="Control"/>
	/// to hook up with <see cref="System.Windows.Input.ICommand"/> objects.
	/// </summary>
	/// <typeparam name="C">
	/// The type of <see cref="Control"/> to hook up.
	/// </typeparam>
	public class CommandBehaviorBinder<C>
	: CommandBehaviorBase<C> where C : Control
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CommandBehaviorBinder"/> class.
		/// </summary>
		/// <param name="control">The control.</param>
		public CommandBehaviorBinder(C control)
			: base(control)
		{
		}

		/// <summary>
		/// Executes the command.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void Execute(Object sender, EventArgs e)
		{
			ExecuteCommand();
		}
	}
}
