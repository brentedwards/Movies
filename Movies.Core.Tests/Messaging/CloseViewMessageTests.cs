using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Movies.Core.Messaging;

namespace Movies.Core.Tests.Messaging
{
	[TestClass()]
	public sealed class CloseViewMessageTests
	{
		[TestMethod()]
		public void Create()
		{
			var viewName = Guid.NewGuid().ToString();

			var message = new CloseViewMessage(viewName);

			Assert.AreEqual(viewName, message.ViewName);
		}
	}
}
