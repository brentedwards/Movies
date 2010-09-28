using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Practices.Composite.Presentation.Commands;
using Movies.Core.Messaging;
using Movies.Core.Models;
using Movies.Core.Navigation;
using Movies.Core.Repositories;
using System.Collections.Generic;

namespace Movies.Core.ViewModels
{
	public sealed class MasterViewModel : ViewModelBase
	{
		public MasterViewModel()
			: base()
		{
			MovieCommand = new DelegateCommand<Object>(SelectMovie);
			NewMovieCommand = new DelegateCommand<Object>(obj => NewMovie());

			MessageBus.Subscribe<SearchMessage>(HandleSearch);

			Movies = MovieRepository.Load();
		}

		public ICommand MovieCommand { get; private set; }
		public ICommand NewMovieCommand { get; private set; }

		public IMessageBus MessageBus { get; set; }
		public IMovieRepository MovieRepository { get; set; }

		private IList<Movie> _Movies;
		public IList<Movie> Movies
		{
			get { return _Movies; }
			private set
			{
				_Movies = value;
				NotifyPropertyChanged("Movies");
			}
		}

		public void SelectMovie(Object movie)
		{
			var message = new ShowViewMessage(ViewTargets.Detail, movie);

			MessageBus.Publish<ShowViewMessage>(message);
		}

		public void NewMovie()
		{
			var message = new ShowViewMessage(ViewTargets.Detail, new Movie());

			MessageBus.Publish<ShowViewMessage>(message);
		}

		private void HandleSearch(SearchMessage search)
		{
			Movies = MovieRepository.Search(search.Keywords, search.Genre, search.Rating);
		}
	}
}
