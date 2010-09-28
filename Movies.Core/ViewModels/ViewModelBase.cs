using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Movies.Core.Models;

namespace Movies.Core.ViewModels
{
	public abstract class ViewModelBase : ModelBase
	{
		public ViewModelBase()
		{
			ComponentContainer.BuildUp(this);
		}
	}
}
