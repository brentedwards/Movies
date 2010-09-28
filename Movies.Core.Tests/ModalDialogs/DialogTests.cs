using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Castle.Windsor;
using Rhino.Mocks;
using Movies.Core.ModalDialogs;
using System.Windows;

namespace Movies.Core.Tests.ModalDialogs
{
	[TestClass()]
	public sealed class DialogTests
	{
		[TestMethod()]
		public void ShowMessage()
		{
			var container = new WindsorContainer();
			ComponentContainer.Container = container;

			var message = Guid.NewGuid().ToString();
			var caption = Guid.NewGuid().ToString();
			var button = MessageBoxButton.OK;
			var result = MessageBoxResult.OK;
			var messageShower = MockRepository.GenerateMock<IMessageShower>();
			messageShower.Expect(ms => ms.Show(
				Arg<String>.Is.Equal(message),
				Arg<String>.Is.Equal(caption),
				Arg<MessageBoxButton>.Is.Equal(button)))
				.Return(result);
			container.Kernel.AddComponentInstance<IMessageShower>(messageShower);

			var actualResult = Dialog.ShowMessage(message, caption, button);

			Assert.AreEqual(result, actualResult);
			messageShower.VerifyAllExpectations();
		}
	}
}
