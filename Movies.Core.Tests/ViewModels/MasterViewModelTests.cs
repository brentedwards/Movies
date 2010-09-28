using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Castle.Windsor;
using Rhino.Mocks;
using Movies.Core.Messaging;
using Movies.Core.Repositories;
using Movies.Core.Models;
using Movies.Core.ViewModels;

namespace Movies.Core.Tests.ViewModels
{
	[TestClass()]
	public sealed class MasterViewModelTests
	{
		private IMessageBus _MessageBus;
		private IMovieRepository _MovieRepository;

		private void CreateContainer(Boolean includeMockBus)
		{
			var container = new WindsorContainer();
			ComponentContainer.Container = container;

			if (includeMockBus)
			{
				_MessageBus = MockRepository.GenerateMock<IMessageBus>();
				container.Kernel.AddComponentInstance<IMessageBus>(_MessageBus);
			}

			var movies = new List<Movie>();
			_MovieRepository = MockRepository.GenerateStub<IMovieRepository>();
			_MovieRepository.Stub(repo => repo.Load()).Return(movies);
			container.Kernel.AddComponentInstance<IMovieRepository>(_MovieRepository);
		}

		[TestMethod()]
		public void Movies()
		{
			CreateContainer(true);
			var viewModel = new MasterViewModel();

			Assert.IsNotNull(viewModel.Movies);
		}

		[TestMethod()]
		public void MovieSelected()
		{
			CreateContainer(true);
			var viewModel = new MasterViewModel();

			viewModel.MovieCommand.Execute(null);

			_MessageBus.AssertWasCalled(bus => bus.Publish<ShowViewMessage>(Arg<ShowViewMessage>.Is.Anything));
		}

		[TestMethod()]
		public void NewMovie()
		{
			CreateContainer(true);
			var viewModel = new MasterViewModel();

			viewModel.NewMovieCommand.Execute(null);

			_MessageBus.AssertWasCalled(bus => bus.Publish<ShowViewMessage>(Arg<ShowViewMessage>.Is.Anything));
		}

		[TestMethod()]
		public void Search()
		{
			CreateContainer(false);
			_MessageBus = new MessageBus();
			ComponentContainer.Container.Kernel.AddComponentInstance<IMessageBus>(_MessageBus);

			var keywords = Guid.NewGuid().ToString();
			var genre = Genres.Action;
			var rating = Ratings.G;
			var searchMessage = new SearchMessage(keywords, genre, rating);

			var viewModel = new MasterViewModel();

			_MessageBus.Publish<SearchMessage>(searchMessage);

			_MovieRepository.AssertWasCalled(repo => repo.Search(
				Arg<String>.Is.Equal(keywords),
				Arg<Genres>.Is.Equal(genre),
				Arg<Ratings>.Is.Equal(rating)));
		}
	}
}
