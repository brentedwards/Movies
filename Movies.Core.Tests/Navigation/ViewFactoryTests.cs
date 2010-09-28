using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Movies.Core.ViewModels;
using Castle.Windsor;
using System.Windows;
using Rhino.Mocks;
using Movies.Core.Navigation;
using Castle.MicroKernel.Registration;

namespace Movies.Core.Tests.Navigation
{
	[TestClass()]
	public sealed class ViewFactoryTests
	{
		private FrameworkElement _View;
		private Object _ViewModel;

		public interface IViewModelWithNoArgumentLoad
			: ITitledViewModel
		{
			void Load();
		}

		public interface IViewModelWithParameterizedLoad
			: ITitledViewModel
		{
			void Load(String parameter);
		}

		private void CreateContainer()
		{
			var container = new WindsorContainer();
			ComponentContainer.Container = container;

			container.Kernel.AddComponentInstance("DetailView", _View);

			if (_ViewModel != null)
			{
				container.Kernel.AddComponentInstance("DetailViewModel", _ViewModel);

				container.Register(
				Component.For<ViewConfiguration>()
					.Named(ViewTargets.Detail.ToString())
					.Parameters(
						Parameter.ForKey("View").Eq("${DetailView}"),
						Parameter.ForKey("ViewModel").Eq("${DetailViewModel}")));
			}
			else
			{
				container.Register(
					Component.For<ViewConfiguration>()
						.Named(ViewTargets.Detail.ToString())
						.Parameters(
							Parameter.ForKey("View").Eq("${DetailView}")));
			}
		}

		[TestInitialize()]
		public void TestInitialize()
		{
			_View = null;
			_ViewModel = null;
		}

		[TestMethod(), ExpectedException(typeof(ArgumentException))]
		public void BuildWithParamsThrowsExceptionWhenNoMatchingLoadFound()
		{
			_View = new FrameworkElement();
			_ViewModel = MockRepository.GenerateStub<ITitledViewModel>();

			CreateContainer();

			var viewBuilder = new ViewFactory();

			viewBuilder.Build(ViewTargets.Detail, new Object());
		}

		[TestMethod(), ExpectedException(typeof(NullReferenceException))]
		public void BuildThrowsExceptionWithViewAndNoViewModel()
		{
			var view = new FrameworkElement();

			var container = new WindsorContainer();
			ComponentContainer.Container = container;

			container.Kernel.AddComponentInstance("DetailView", view);

			container.Register(
				Component.For<ViewConfiguration>()
					.Named(ViewTargets.Detail.ToString())
					.Parameters(
						Parameter.ForKey("View").Eq("${DetailView}")));

			var viewBuilder = new ViewFactory();

			viewBuilder.Build(ViewTargets.Detail);
		}

		[TestMethod(), ExpectedException(typeof(ArgumentException))]
		public void BuildWithParamsAndViewModelWithNoLoadMethodThrowsException()
		{
			_View = new FrameworkElement();
			_ViewModel = MockRepository.GenerateStub<ITitledViewModel>();

			CreateContainer();

			var viewBuilder = new ViewFactory();

			viewBuilder.Build(ViewTargets.Detail, Guid.NewGuid().ToString());
		}

		[TestMethod(), ExpectedException(typeof(ArgumentException))]
		public void BuildWithBadViewModelThrowsException()
		{
			_View = new FrameworkElement();
			_ViewModel = new Object();

			CreateContainer();

			var viewBuilder = new ViewFactory();

			viewBuilder.Build(ViewTargets.Detail);
		}

		[TestMethod()]
		public void BuildWithNoParamsAndViewModelWithNoLoadReturnsView()
		{
			_View = new FrameworkElement();

			var title = Guid.NewGuid().ToString();
			var viewModel = MockRepository.GenerateStub<ITitledViewModel>();
			viewModel.Expect(vm => vm.Title).Return(title);
			_ViewModel = viewModel;

			CreateContainer();

			var viewBuilder = new ViewFactory();

			var viewResult = viewBuilder.Build(ViewTargets.Detail);

			Assert.IsNotNull(viewResult);

			var resultViewModel = (ITitledViewModel)viewResult.View.DataContext;
			Assert.AreEqual(title, resultViewModel.Title);
		}

		[TestMethod()]
		public void BuildWithNoParamsAndViewModelWithLoadWithNoParamsReturnsControl()
		{
			_View = new FrameworkElement();

			var title = Guid.NewGuid().ToString();
			var viewModel = MockRepository.GenerateMock<IViewModelWithNoArgumentLoad>();
			viewModel.Expect(vm => vm.Title).Return(title);
			_ViewModel = viewModel;

			CreateContainer();

			var viewBuilder = new ViewFactory();

			var viewResult = viewBuilder.Build(ViewTargets.Detail);

			Assert.IsNotNull(viewResult);

			var resultViewModel = (ITitledViewModel)viewResult.View.DataContext;
			Assert.AreEqual(title, resultViewModel.Title);
			viewModel.AssertWasCalled(vm => vm.Load());
		}

		[TestMethod()]
		public void BuildWithParamsReturnsControlAndLoadedViewModel()
		{
			_View = new FrameworkElement();

			var title = Guid.NewGuid().ToString();
			var viewModel = MockRepository.GenerateMock<IViewModelWithParameterizedLoad>();
			viewModel.Stub(vm => vm.Load(Arg<String>.Is.Anything));
			viewModel.Expect(vm => vm.Title).Return(title);
			_ViewModel = viewModel;

			CreateContainer();

			var viewBuilder = new ViewFactory();

			var param = Guid.NewGuid().ToString();
			var viewResult = viewBuilder.Build(ViewTargets.Detail, param);

			Assert.IsNotNull(viewResult);

			var resultViewModel = (ITitledViewModel)viewResult.View.DataContext;
			Assert.AreEqual(title, resultViewModel.Title);
			viewModel.AssertWasCalled(vm => vm.Load(Arg<String>.Is.Equal(param)));
		}

		[TestMethod()]
		public void BuildWithImpliedViewModelReturnsControlAndLoadedViewModel()
		{
			_View = new FrameworkElement();

			var title = Guid.NewGuid().ToString();
			var viewModel = MockRepository.GenerateMock<IViewModelWithNoArgumentLoad>();
			viewModel.Expect(vm => vm.Title).Return(title);
			_View.DataContext = viewModel;

			CreateContainer();

			var viewBuilder = new ViewFactory();

			var viewResult = viewBuilder.Build(ViewTargets.Detail);

			Assert.IsNotNull(viewResult);

			var resultViewModel = (ITitledViewModel)viewResult.View.DataContext;
			Assert.AreEqual(title, resultViewModel.Title);
			viewModel.AssertWasCalled(vm => vm.Load());
		}
	}
}
