using IMDB_Browser.ViewModels;
using System;
using System.Collections.Generic;

namespace IMDB_Browser.Navigation
{
    public interface INavigationService
    {
        void NavigateTo<TViewModel>(object parameter = null) where TViewModel : class;
        void GoBack();
    }

    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Stack<object> _navigationStack = new Stack<object>();
        private MainViewModel _mainViewModel;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void SetMainViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        public void NavigateTo<TViewModel>(object parameter = null) where TViewModel : class
        {
            var viewModel = _serviceProvider.GetService(typeof(TViewModel)) as TViewModel;
            if (viewModel != null)
            {
                if (viewModel is IParameterReceiver parameterReceiver)
                {
                    parameterReceiver.ReceiveParameter(parameter);
                }

                _navigationStack.Push(viewModel);
                _mainViewModel.CurrentViewModel = viewModel;
            }
        }

        public void GoBack()
        {
            if (_navigationStack.Count > 1)
            {
                _navigationStack.Pop();
                var viewModel = _navigationStack.Peek();
                _mainViewModel.CurrentViewModel = viewModel;
            }
        }
    }

    public interface IParameterReceiver
    {
        void ReceiveParameter(object parameter);
    }
}
