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
		}

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
	}
}
