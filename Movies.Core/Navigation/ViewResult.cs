using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Movies.Core.Navigation
{
	public sealed class ViewResult
	{
		public ViewResult(FrameworkElement view, String title)
		{
			View = view;
			Title = title;
		}

		public FrameworkElement View { get; private set; }
		public String Title { get; private set; }
	}
}
