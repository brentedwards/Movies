using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Movies.Core.Models;
using System.Windows.Input;
using Microsoft.Practices.Composite.Presentation.Commands;
using Movies.Core.Repositories;
using Movies.Core.Messaging;
using Movies.Core.ModalDialogs;
using System.Windows;

namespace Movies.Core.ViewModels
{
	public sealed class DetailViewModel : ViewModelBase, ITitledViewModel
	{
		public DetailViewModel()
		{
			SaveCommand = new DelegateCommand<Object>(obj => Save());
			CloseCommand = new DelegateCommand<Object>(obj => Close());
		}

		public ICommand SaveCommand { get; private set; }
		public ICommand CloseCommand { get; private set; }

		public IMessageBus MessageBus { get; set; }
		public IMovieRepository MovieRepository { get; set; }

		private Boolean _IsEditable;
		public Boolean IsEditable
		{
			get { return _IsEditable; }
			set
			{
				_IsEditable = value;
				NotifyPropertyChanged("IsEditable");
			}
		}

		private Movie _Movie;
		public Movie Movie
		{
			get { return _Movie; }
			private set
			{
				_Movie = value;
				NotifyPropertyChanged("Movie");
			}
		}

		public Array Genres
		{
			get
			{
				return Enum.GetValues(typeof(Genres));
			}
		}

		public Array Ratings
		{
			get
			{
				return Enum.GetValues(typeof(Ratings));
			}
		}

		#region ITitledViewModel Members

		public String Title { get; private set; }

		#endregion

		public void Load(Movie movie)
		{
			Movie = movie;
			if (movie.Id == -1)
			{
				IsEditable = true;
			}

			Title = String.Format("{0} Details", Movie.Name);
		}

		public void Save()
		{
			MovieRepository.Save(Movie);
		}

		public void Close()
		{
			var message = new CloseViewMessage(Title);

			MessageBus.Publish<CloseViewMessage>(message);
		}
	}
}
