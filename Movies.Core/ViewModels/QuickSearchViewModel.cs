using System;
using System.Windows.Input;
using Microsoft.Practices.Composite.Presentation.Commands;
using Movies.Core.Messaging;
using Movies.Core.Models;

namespace Movies.Core.ViewModels
{
	public sealed class QuickSearchViewModel : ViewModelBase
	{
		public QuickSearchViewModel()
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

		public void Search()
		{
			var search = new SearchMessage(_Keywords);

			MessageBus.Publish<SearchMessage>(search);
		}
	}
}
