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
	public sealed class ViewResultTests
	{
		[TestMethod()]
		public void Create()
		{
			var view = new FrameworkElement();
			var title = Guid.NewGuid().ToString();

			var viewResult = new ViewResult(view, title);

			Assert.AreSame(view, viewResult.View);
			Assert.AreEqual(title, viewResult.Title);
		}
	}
}
