using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Movies.Core.Navigation;
using Movies.Core.Messaging;

namespace Movies.Core.Tests.Messaging
{
	[TestClass()]
	public sealed class ShowViewMessageTests
	{
		[TestMethod()]
		public void CreateWithViewTarget()
		{
			var viewTarget = ViewTargets.Detail;

			var message = new ShowViewMessage(viewTarget);

			Assert.AreEqual(viewTarget, message.ViewTarget);
			Assert.IsNull(message.LoadArgs);
		}

		[TestMethod()]
		public void Create()
		{
			var viewTarget = ViewTargets.Detail;
			var loadArgs = new Object();

			var message = new ShowViewMessage(viewTarget, loadArgs);

			Assert.AreEqual(viewTarget, message.ViewTarget);
			Assert.AreSame(loadArgs, message.LoadArgs);
		}
	}
}
