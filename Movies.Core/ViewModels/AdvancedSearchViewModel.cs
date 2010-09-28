using System;
using Movies.Core.Messaging;
using Movies.Core.Models;
using System.Windows.Input;
using Microsoft.Practices.Composite.Presentation.Commands;

namespace Movies.Core.ViewModels
{
	public sealed class AdvancedSearchViewModel : ViewModelBase
	{
		public AdvancedSearchViewModel()
			: base()
		{
			SearchCommand = new DelegateCommand<Object>(obj => Search());
		}

		public ICommand SearchCommand { get; private set; }

		public IMessageBus MessageBus { get; set; }

		private String _Keywords;
		public String Keywords
		{
			get { return _Keywords; }
			set
			{
				_Keywords = value;
				NotifyPropertyChanged("Keywords");
			}
		}

		public Array Genres
		{
			get
			{
				return Enum.GetValues(typeof(Genres));
			}
		}

		private Genres? _SelectedGenre;
		public Genres? SelectedGenre
		{
			get { return _SelectedGenre; }
			set
			{
				_SelectedGenre = value;
				NotifyPropertyChanged("SelectedGenre");
			}
		}

		public Array Ratings
		{
			get
			{
				return Enum.GetValues(typeof(Ratings));
			}
		}

		private Ratings? _SelectedRating;
		public Ratings? SelectedRating
		{
			get { return _SelectedRating; }
			set
			{
				_SelectedRating = value;
				NotifyPropertyChanged("SelectedRating");
			}
		}

		public void Search()
		{
			var message = new SearchMessage(Keywords, SelectedGenre, SelectedRating);

			MessageBus.Publish<SearchMessage>(message);
		}
	}
}
