using System;
using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Movies.Core.Messaging;
using Movies.Core.ViewModels;
using Rhino.Mocks;
using System.ComponentModel;
using System.Collections.Generic;

namespace Movies.Core.Tests.ViewModels
{
	[TestClass()]
	public sealed class QuickSearchViewModelTests
	{
		[TestMethod()]
		public void Search()
		{
			var container = new WindsorContainer();
			ComponentContainer.Container = container;

			SearchMessage searchMessage = null;
			var messageBus = MockRepository.GenerateStub<IMessageBus>();
			messageBus.Stub(bus => bus.Publish<SearchMessage>(Arg<SearchMessage>.Is.Anything))
				.WhenCalled(inv => searchMessage = inv.Arguments[0] as SearchMessage);
			container.Kernel.AddComponentInstance<IMessageBus>(messageBus);

			var viewModel = new QuickSearchViewModel();

			var keywords = Guid.NewGuid().ToString();
			viewModel.Keywords = keywords;

			viewModel.SearchCommand.Execute(null);

			Assert.IsNotNull(searchMessage);
			Assert.AreEqual(keywords, searchMessage.Keywords);
		}

		private List<String> _ChangedProperties;
		private void HandlePropertyChanged(Object sender, PropertyChangedEventArgs args)
		{
			_ChangedProperties.Add(args.PropertyName);
		}

		[TestMethod()]
		public void Keywords()
		{
			_ChangedProperties = new List<String>();
			var viewModel = new QuickSearchViewModel();
			viewModel.PropertyChanged += HandlePropertyChanged;
			var keywords = Guid.NewGuid().ToString();
			
			viewModel.Keywords = keywords;

			Assert.AreEqual(keywords, viewModel.Keywords);
			Assert.IsTrue(_ChangedProperties.Contains("Keywords"));
		}
	}
}
