using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using Movies.Core.Navigation;

namespace Movies.Core.Tests.Navigation
{
	[TestClass()]
	public sealed class ViewConfigurationTests
	{
		[TestMethod()]
		public void Create()
		{
			var view = new FrameworkElement();

			var viewConfiguration = new ViewConfiguration(view);

			Assert.AreSame(view, viewConfiguration.View);
			Assert.IsNull(viewConfiguration.ViewModel);
		}

		[TestMethod()]
		public void CreateWithViewModel()
		{
			var view = new FrameworkElement();
			var viewModel = new Object();

			var viewConfiguration = new ViewConfiguration(view, viewModel);

			Assert.AreSame(view, viewConfiguration.View);
			Assert.AreSame(viewModel, viewConfiguration.ViewModel);
		}
	}
}
