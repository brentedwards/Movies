using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Movies.Core.Messaging;
using Castle.Windsor;
using Rhino.Mocks;
using Movies.Core.Navigation;
using System.Windows;
using System.Windows.Controls;

namespace Movies.Core.Tests.Navigation
{
	[TestClass()]
	public sealed class ViewControllerTests
	{
		private IMessageBus _MessageBus;

		private void CreateContainer()
		{
			var container = new WindsorContainer();
			ComponentContainer.Container = container;

			_MessageBus = new MessageBus();
			container.Kernel.AddComponentInstance<IMessageBus>(_MessageBus);
		}

		[TestMethod()]
		public void ShowView()
		{
			CreateContainer();
			var view = new FrameworkElement();
			var title = Guid.NewGuid().ToString();

			var viewResult = new ViewResult(view, title);
			var viewBuilder = MockRepository.GenerateStub<IViewFactory>();
			viewBuilder.Stub(vb => vb.Build(Arg<ViewTargets>.Is.Anything, Arg<Object>.Is.Anything))
				.Return(viewResult);
			ComponentContainer.Container.Kernel.AddComponentInstance<IViewFactory>(viewBuilder);

			var tabControl = new TabControl();
			var viewController = new ViewController(tabControl);

			var message = new ShowViewMessage(ViewTargets.Detail);
			_MessageBus.Publish<ShowViewMessage>(message);

			var viewFound = false;
			foreach (TabItem tabItem in tabControl.Items)
			{
				if (tabItem.Header.ToString() == title)
				{
					viewFound = true;
				}
			}

			Assert.IsTrue(viewFound);
		}

		[TestMethod()]
		public void ShowViewExists()
		{
			CreateContainer();
			var view = new FrameworkElement();
			var title = Guid.NewGuid().ToString();

			var viewResult = new ViewResult(view, title);
			var viewBuilder = MockRepository.GenerateStub<IViewFactory>();
			viewBuilder.Stub(vb => vb.Build(Arg<ViewTargets>.Is.Anything, Arg<Object>.Is.Anything))
				.Return(viewResult);
			ComponentContainer.Container.Kernel.AddComponentInstance<IViewFactory>(viewBuilder);

			var tabControl = new TabControl();
			var viewController = new ViewController(tabControl);
			var newTabItem = new TabItem() { Header = title };
			tabControl.Items.Add(newTabItem);

			var message = new ShowViewMessage(ViewTargets.Detail);
			_MessageBus.Publish<ShowViewMessage>(message);

			var viewsFound = 0;
			foreach (TabItem tabItem in tabControl.Items)
			{
				if (tabItem.Header.ToString() == title)
				{
					viewsFound++;
				}
			}

			Assert.IsTrue(viewsFound == 1);
		}

		[TestMethod()]
		public void CloseExists()
		{
			CreateContainer();
			var view = new FrameworkElement();
			var title = Guid.NewGuid().ToString();

			var viewResult = new ViewResult(view, title);
			var viewBuilder = MockRepository.GenerateStub<IViewFactory>();
			viewBuilder.Stub(vb => vb.Build(Arg<ViewTargets>.Is.Anything, Arg<Object>.Is.Anything))
				.Return(viewResult);
			ComponentContainer.Container.Kernel.AddComponentInstance<IViewFactory>(viewBuilder);

			var tabControl = new TabControl();
			var viewController = new ViewController(tabControl);
			var newTabItem = new TabItem() { Header = title };
			tabControl.Items.Add(newTabItem);

			var message = new CloseViewMessage(title);
			_MessageBus.Publish<CloseViewMessage>(message);

			Assert.AreEqual(0, tabControl.Items.Count);
		}

		[TestMethod()]
		public void CloseDoesNotExist()
		{
			CreateContainer();
			var view = new FrameworkElement();
			var title = Guid.NewGuid().ToString();

			var viewResult = new ViewResult(view, title);
			var viewBuilder = MockRepository.GenerateStub<IViewFactory>();
			viewBuilder.Stub(vb => vb.Build(Arg<ViewTargets>.Is.Anything, Arg<Object>.Is.Anything))
				.Return(viewResult);
			ComponentContainer.Container.Kernel.AddComponentInstance<IViewFactory>(viewBuilder);

			var tabControl = new TabControl();
			var viewController = new ViewController(tabControl);
			var newTabItem = new TabItem() { Header = Guid.NewGuid().ToString() };
			tabControl.Items.Add(newTabItem);

			var message = new CloseViewMessage(title);
			_MessageBus.Publish<CloseViewMessage>(message);

			Assert.AreEqual(1, tabControl.Items.Count);
		}
	}
}
