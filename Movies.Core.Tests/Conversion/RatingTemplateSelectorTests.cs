using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using Movies.Core.Conversion;
using Movies.Core.Models;

namespace Movies.Core.Tests.Conversion
{
	[TestClass()]
	public sealed class RatingTemplateSelectorTests
	{
		[TestMethod()]
		public void SelectTemplateG()
		{
			var gTemplate = new DataTemplate();
			var rTemplate = new DataTemplate();
			var selector = new RatingTemplateSelector() { GTemplate = gTemplate, RTemplate = rTemplate };

			var actualTemplate = selector.SelectTemplate(Ratings.G, null);

			Assert.AreSame(gTemplate, actualTemplate);
		}

		[TestMethod()]
		public void SelectTemplateR()
		{
			var gTemplate = new DataTemplate();
			var rTemplate = new DataTemplate();
			var selector = new RatingTemplateSelector() { GTemplate = gTemplate, RTemplate = rTemplate };

			var actualTemplate = selector.SelectTemplate(Ratings.R, null);

			Assert.AreSame(rTemplate, actualTemplate);
		}
	}
}
