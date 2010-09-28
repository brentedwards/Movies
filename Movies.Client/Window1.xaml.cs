using System.Windows;
using Movies.Core.Navigation;

namespace Movies.Client
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window
	{
		private ViewController _ViewController;

		public Window1()
		{
			InitializeComponent();

			_ViewController = new ViewController(MainTabControl);
		}
	}
}
