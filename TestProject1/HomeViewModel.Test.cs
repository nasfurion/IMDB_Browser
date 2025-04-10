using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using IMDB_Browser.Models;
using IMDB_Browser.Navigation;
using IMDB_Browser.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace TestProject1
{
    [TestClass]
    public class HomeViewModelTests
    {
        private Mock<INavigationService> _navigationServiceMock;
        private Mock<FavouritesViewModel> _favouritesViewModelMock;
        private Mock<WatchListViewModel> _watchListViewModelMock;
        private Mock<MainViewModel> _mainViewModelMock;
        private HomeViewModel _homeViewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _navigationServiceMock = new Mock<INavigationService>();
            _favouritesViewModelMock = new Mock<FavouritesViewModel>();
            _watchListViewModelMock = new Mock<WatchListViewModel>();
            _mainViewModelMock = new Mock<MainViewModel>(_navigationServiceMock.Object, _favouritesViewModelMock.Object, _watchListViewModelMock.Object);

            _homeViewModel = new HomeViewModel(_navigationServiceMock.Object, _favouritesViewModelMock.Object, _watchListViewModelMock.Object, _mainViewModelMock.Object);
        }

        [TestMethod]
        public void Titles_Setter_ShouldTriggerFilterTitles()
        {
            // Arrange
            var titles = new ObservableCollection<Title>
            {
                new Title { PrimaryTitle = "Movie 1", TitleType = "movie", StartYear = 2020 },
                new Title { PrimaryTitle = "Movie 2", TitleType = "movie", StartYear = 2021 }
            };

            // Act
            _homeViewModel.Titles = titles;

            // Assert
            Assert.AreEqual(2, _homeViewModel.FilteredTitles.Count);
        }

        [TestMethod]
        public void SearchQuery_Setter_ShouldTriggerFilterTitles()
        {
            // Arrange
            var titles = new ObservableCollection<Title>
            {
                new Title { PrimaryTitle = "Movie 1", TitleType = "movie", StartYear = 2020 },
                new Title { PrimaryTitle = "Movie 2", TitleType = "movie", StartYear = 2021 }
            };
            _homeViewModel.Titles = titles;

            // Act
            _homeViewModel.SearchQuery = "Movie 1";

            // Assert
            Assert.AreEqual(1, _homeViewModel.FilteredTitles.Count);
            Assert.AreEqual("Movie 1", _homeViewModel.FilteredTitles.First().PrimaryTitle);
        }

    }
}
