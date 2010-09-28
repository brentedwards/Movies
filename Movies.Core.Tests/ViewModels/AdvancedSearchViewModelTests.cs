using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Movies.Core.ViewModels;
using System.ComponentModel;
using Castle.Windsor;
using Rhino.Mocks;
using Movies.Core.Messaging;

namespace Movies.Core.Tests.ViewModels
{
	[TestClass()]
	public sealed class AdvancedSearchViewModelTests
	{
		private List<String> _ChangedProperties;
		private void HandlePropertyChanged(Object sender, PropertyChangedEventArgs args)
		{
			_ChangedProperties.Add(args.PropertyName);
		}

		[TestInitialize()]
		public void TestInitialize()
		{
			_ChangedProperties = new List<String>();
		}

		[TestMethod()]
		public void Keywords()
		{
			var viewModel = new AdvancedSearchViewModel();
			viewModel.PropertyChanged += HandlePropertyChanged;
			var keywords = Guid.NewGuid().ToString();

			viewModel.Keywords = keywords;

			Assert.AreEqual(keywords, viewModel.Keywords);
			Assert.IsTrue(_ChangedProperties.Contains("Keywords"));
		}

		[TestMethod()]
		public void Genres()
		{
			var viewModel = new AdvancedSearchViewModel();

			Assert.IsNotNull(viewModel.Genres);
		}

		[TestMethod()]
		public void SelectedGenre()
		{
			var viewModel = new AdvancedSearchViewModel();
			viewModel.PropertyChanged += HandlePropertyChanged;
			var genre = Core.Models.Genres.Action;

			viewModel.SelectedGenre = genre;

			Assert.AreEqual(genre, viewModel.SelectedGenre);
			Assert.IsTrue(_ChangedProperties.Contains("SelectedGenre"));
		}

		[TestMethod()]
		public void Ratings()
		{
			var viewModel = new AdvancedSearchViewModel();

			Assert.IsNotNull(viewModel.Ratings);
		}

		[TestMethod()]
		public void SelectedRating()
		{
			var viewModel = new AdvancedSearchViewModel();
			viewModel.PropertyChanged += HandlePropertyChanged;
			var rating = Core.Models.Ratings.G;

			viewModel.SelectedRating = rating;

			Assert.AreEqual(rating, viewModel.SelectedRating);
			Assert.IsTrue(_ChangedProperties.Contains("SelectedRating"));
		}

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

			var viewModel = new AdvancedSearchViewModel();

			var keywords = Guid.NewGuid().ToString();
			var genre = Core.Models.Genres.Action;
			var rating = Core.Models.Ratings.G;

			viewModel.Keywords = keywords;
			viewModel.SelectedGenre = genre;
			viewModel.SelectedRating = rating;

			viewModel.SearchCommand.Execute(null);

			Assert.IsNotNull(searchMessage);
			Assert.AreEqual(keywords, searchMessage.Keywords);
			Assert.AreEqual(genre, searchMessage.Genre);
			Assert.AreEqual(rating, searchMessage.Rating);
		}
	}
}
