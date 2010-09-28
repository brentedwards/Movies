using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Movies.Core.ViewModels;
using System.ComponentModel;
using Movies.Core.Models;
using Castle.Windsor;
using Rhino.Mocks;
using Movies.Core.Repositories;
using Movies.Core.Messaging;
using Movies.Core.ModalDialogs;

namespace Movies.Core.Tests.ViewModels
{
	[TestClass()]
	public sealed class DetailViewModelTests
	{
		private List<String> _ChangedProperties;
		private void HandlePropertyChanged(Object sender, PropertyChangedEventArgs args)
		{
			_ChangedProperties.Add(args.PropertyName);
		}

		[TestMethod()]
		public void IsEditable()
		{
			_ChangedProperties = new List<String>();
			var viewModel = new DetailViewModel();
			viewModel.PropertyChanged += HandlePropertyChanged;

			viewModel.IsEditable = true;

			Assert.IsTrue(viewModel.IsEditable);
			Assert.IsTrue(_ChangedProperties.Contains("IsEditable"));
		}

		[TestMethod()]
		public void LoadMovie()
		{
			_ChangedProperties = new List<String>();
			var viewModel = new DetailViewModel();
			viewModel.PropertyChanged += HandlePropertyChanged;

			var movie = new Movie();
			viewModel.Load(movie);

			Assert.AreSame(movie, viewModel.Movie);
			Assert.IsTrue(_ChangedProperties.Contains("Movie"));
		}

		[TestMethod()]
		public void Title()
		{
			var viewModel = new DetailViewModel();

			var movie = new Movie(new Random().Next(),
				Guid.NewGuid().ToString(),
				Core.Models.Genres.Action,
				Core.Models.Ratings.G);
			viewModel.Load(movie);

			Assert.AreEqual(String.Format("{0} Details", movie.Name), viewModel.Title);
		}

		[TestMethod()]
		public void Save()
		{
			var container = new WindsorContainer();
			ComponentContainer.Container = container;

			var repository = MockRepository.GenerateMock<IMovieRepository>();
			container.Kernel.AddComponentInstance<IMovieRepository>(repository);

			var messageShower = MockRepository.GenerateStub<IMessageShower>();
			container.Kernel.AddComponentInstance<IMessageShower>(messageShower);

			var movie = new Movie();
			var viewModel = new DetailViewModel();
			viewModel.Load(movie);

			viewModel.SaveCommand.Execute(null);

			repository.AssertWasCalled(repo => repo.Save(Arg<Movie>.Is.Same(movie)));
		}

		[TestMethod()]
		public void Close()
		{
			var container = new WindsorContainer();
			ComponentContainer.Container = container;

			var actualViewName = String.Empty;
			var bus = MockRepository.GenerateMock<IMessageBus>();
			bus.Expect(mb => mb.Publish<CloseViewMessage>(Arg<CloseViewMessage>.Is.Anything))
				.WhenCalled(inv => actualViewName = ((CloseViewMessage)inv.Arguments[0]).ViewName);
			container.Kernel.AddComponentInstance<IMessageBus>(bus);

			var movie = new Movie();
			var viewModel = new DetailViewModel();
			viewModel.Load(movie);

			viewModel.CloseCommand.Execute(null);

			bus.VerifyAllExpectations();
			Assert.AreEqual(viewModel.Title, actualViewName);
		}

		[TestMethod()]
		public void Genres()
		{
			var viewModel = new DetailViewModel();

			var genres = viewModel.Genres;

			Assert.IsNotNull(genres);
			Assert.IsTrue(genres.Length > 0);
		}

		[TestMethod()]
		public void Ratings()
		{
			var viewModel = new DetailViewModel();

			var ratings = viewModel.Ratings;

			Assert.IsNotNull(ratings);
			Assert.IsTrue(ratings.Length > 0);
		}
	}
}
