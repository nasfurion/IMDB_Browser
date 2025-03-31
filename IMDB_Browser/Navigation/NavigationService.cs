using IMDB_Browser.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB_Browser.Navigation
{
    public interface INavigationService
    {
        // Navigation methods
        void NavigateTo<TViewModel>() where TViewModel : class;
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

        public void NavigateTo<TViewModel>() where TViewModel : class
        {
            // Get the ViewModel instance from the service provider
            var viewModel = _serviceProvider.GetService(typeof(TViewModel)) as TViewModel;
            if (viewModel != null)
            {
                // Push the ViewModel onto the navigation stack
                _navigationStack.Push(viewModel);

                // Set the current ViewModel in the MainViewModel
                _mainViewModel.CurrentViewModel = viewModel;
            }
        }

        public void GoBack()
        {
            if (_navigationStack.Count > 1)
            {
                // Pop the current ViewModel from the stack
                _navigationStack.Pop();

                // Get the previous ViewModel from the stack
                var viewModel = _navigationStack.Peek();

                // Set the current ViewModel in the MainViewModel
                _mainViewModel.CurrentViewModel = viewModel;
            }
        }
    }

}
