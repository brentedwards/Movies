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
			Movies = MovieRepository.Load();
		}

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

		private void HandleSearch(SearchMessage search)
		{
			Movies = MovieRepository.Search(search.Keywords, search.Genre, search.Rating);
		}
	}
}
