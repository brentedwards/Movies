using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Movies.Core.ViewModels;

namespace Movies.Core.Navigation
{
	/// <summary>
	/// Dynamic view builder that will instantiate views and their associated view
	/// models based on given parameters.
	/// </summary>
	public sealed class ViewFactory : IViewFactory
	{
		private const String LOAD = "Load";

		public ViewResult Build(ViewTargets viewTarget)
		{
			return Build(viewTarget, null);
		}

		public ViewResult Build(ViewTargets viewTarget, Object viewParams)
		{
			var viewConfig = ComponentContainer.Container.Resolve<ViewConfiguration>(viewTarget.ToString());
			var view = viewConfig.View;

			if (viewConfig.ViewModel != null)
			{
				// There was a view model explicitly defined, use it.
				view.DataContext = viewConfig.ViewModel;
			}

			LoadViewModelHelper(view.DataContext, viewParams);

			var viewResult = new ViewResult(view, GetTitleFromViewModel(view.DataContext));
			return viewResult;
		}

		private void LoadViewModelHelper(Object viewModel, Object viewParams)
		{
			var viewModelType = viewModel.GetType();

			Object[] parms = null;
			Type[] paramTypes = null;

			if (viewParams != null)
			{
				// Attempt to find a Load method on the view model which takes the given parameters.
				paramTypes = new Type[] { viewParams.GetType() };
				parms = new Object[] { viewParams };
			}
			else
			{
				paramTypes = new Type[] { };
			}

			var methodInfo = viewModelType.GetMethod(LOAD, paramTypes);

			// If view params are given, a Load method which takes those params is required.  If
			// no params are given, a Load method which takes no params is optional.
			if (methodInfo == null && viewParams != null)
			{
				throw new ArgumentException(
					String.Format("'{0}' does not have a 'Load' method with the matching parameters.",
					viewModelType.Name));
			}
			else if (methodInfo != null)
			{
				methodInfo.Invoke(viewModel, BindingFlags.ExactBinding, null, parms, null);
			}
		}

		private String GetTitleFromViewModel(Object viewModel)
		{
			var title = String.Empty;

			var titledViewModel = viewModel as ITitledViewModel;
			if (titledViewModel != null)
			{
				title = titledViewModel.Title;
			}
			else
			{
				throw new ArgumentException(
					String.Format("'{0}' does not inherit from ITitledViewModel.",
					viewModel.GetType().Name));
			}

			return title;
		}
	}
}
