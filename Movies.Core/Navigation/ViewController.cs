using System.Windows.Controls;
using Movies.Core.Messaging;

namespace Movies.Core.Navigation
{
	public sealed class ViewController
	{
		private TabControl _MainTabControl;

		public ViewController(TabControl mainTabControl)
		{
			_MainTabControl = mainTabControl;

			Initialize();
		}

		private void Initialize()
		{
			var bus = ComponentContainer.Container.Resolve<IMessageBus>();
			bus.Subscribe<ShowViewMessage>(HandleShowView);
			bus.Subscribe<CloseViewMessage>(HandleCloseView);
		}

		private void HandleShowView(ShowViewMessage args)
		{
			var viewFactory = ComponentContainer.Container.Resolve<IViewFactory>();
			var viewResult = viewFactory.Build(args.ViewTarget, args.LoadArgs);

			var exists = false;
			foreach (TabItem tabItem in _MainTabControl.Items)
			{
				if (tabItem.Header.ToString() == viewResult.Title)
				{
					exists = true;
					break;
				}
			}

			if (!exists)
			{
				var newTabItem = new TabItem() { Header = viewResult.Title, Content = viewResult.View };
				_MainTabControl.Items.Add(newTabItem);

				newTabItem.Focus();
			}
		}

		private void HandleCloseView(CloseViewMessage args)
		{
			TabItem openTabItem = null;
			foreach (TabItem tabItem in _MainTabControl.Items)
			{
				if (tabItem.Header.ToString() == args.ViewName)
				{
					openTabItem = tabItem;
					break;
				}
			}

			if (openTabItem != null)
			{
				_MainTabControl.Items.Remove(openTabItem);
			}
		}
	}
}
