using Microsoft.VisualStudio.TestTools.UnitTesting;
using IMDB_Browser.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using IMDB_Browser.Models;

namespace TestProject1
{
    [TestClass]
    public class WatchListViewModelTests
    {
        private WatchListViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _viewModel = new WatchListViewModel();
        }

        [TestMethod]
        public void UpdateWatchList_AddsAndRemovesTitlesCorrectly()
        {
            var title = new Title { TitleId = "tt1234567", IsInWatchlist = true };

            _viewModel.UpdateWatchList(title);
            Assert.IsTrue(_viewModel.WatchListMovies.Contains(title));

            title.IsInWatchlist = false;
            _viewModel.UpdateWatchList(title);
            Assert.IsFalse(_viewModel.WatchListMovies.Contains(title));
        }

        [TestMethod]
        public void SearchQuery_FiltersMoviesCorrectly()
        {
            var title1 = new Title { TitleId = "tt1234567", PrimaryTitle = "Test Movie 1", IsInWatchlist = true };
            var title2 = new Title { TitleId = "tt7654321", PrimaryTitle = "Another Movie", IsInWatchlist = true };

            _viewModel.UpdateWatchList(title1);
            _viewModel.UpdateWatchList(title2);

            _viewModel.SearchQuery = "Test";
            Assert.AreEqual(1, _viewModel.WatchListMovies.Count);
            Assert.AreEqual(title1, _viewModel.WatchListMovies.First());

            _viewModel.SearchQuery = "Another";
            Assert.AreEqual(1, _viewModel.WatchListMovies.Count);
            Assert.AreEqual(title2, _viewModel.WatchListMovies.First());

            _viewModel.SearchQuery = string.Empty;
            Assert.AreEqual(2, _viewModel.WatchListMovies.Count);
        }

        [TestMethod]
        public void ToggleFavoriteCommand_TogglesFavoriteStatus()
        {
            var title = new Title { TitleId = "tt1234567", IsFavorite = false };

            _viewModel.ToggleFavoriteCommand.Execute(title);
            Assert.IsTrue(title.IsFavorite);

            _viewModel.ToggleFavoriteCommand.Execute(title);
            Assert.IsFalse(title.IsFavorite);
        }

        [TestMethod]
        public void ToggleWatchlistCommand_TogglesWatchlistStatus()
        {
            var title = new Title { TitleId = "tt1234567", IsInWatchlist = false };

            _viewModel.ToggleWatchlistCommand.Execute(title);
            Assert.IsTrue(title.IsInWatchlist);
            Assert.IsTrue(_viewModel.WatchListMovies.Contains(title));

            _viewModel.ToggleWatchlistCommand.Execute(title);
            Assert.IsFalse(title.IsInWatchlist);
            Assert.IsFalse(_viewModel.WatchListMovies.Contains(title));
        }
    }
}
