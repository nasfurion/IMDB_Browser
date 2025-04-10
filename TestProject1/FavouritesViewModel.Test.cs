using Microsoft.VisualStudio.TestTools.UnitTesting;
using IMDB_Browser.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using IMDB_Browser.Models;

namespace TestProject1
{
    [TestClass]
    public sealed class FavouritesViewModelTests
    {
        private FavouritesViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _viewModel = new FavouritesViewModel();
        }

        [TestMethod]
        public void UpdateFavorites_AddsAndRemovesFavoritesCorrectly()
        {
            var title = new Title { PrimaryTitle = "Test Movie", IsFavorite = true };

            _viewModel.UpdateFavorites(title);
            Assert.IsTrue(_viewModel.FavMovies.Contains(title));

            title.IsFavorite = false;
            _viewModel.UpdateFavorites(title);
            Assert.IsFalse(_viewModel.FavMovies.Contains(title));
        }

        [TestMethod]
        public void SearchQuery_FiltersMoviesCorrectly()
        {
            var title1 = new Title { PrimaryTitle = "Test Movie 1", IsFavorite = true };
            var title2 = new Title { PrimaryTitle = "Another Movie", IsFavorite = true };

            _viewModel.UpdateFavorites(title1);
            _viewModel.UpdateFavorites(title2);

            _viewModel.SearchQuery = "Test";
            Assert.AreEqual(1, _viewModel.FavMovies.Count);
            Assert.AreEqual(title1, _viewModel.FavMovies.First());

            _viewModel.SearchQuery = "Another";
            Assert.AreEqual(1, _viewModel.FavMovies.Count);
            Assert.AreEqual(title2, _viewModel.FavMovies.First());

            _viewModel.SearchQuery = string.Empty;
            Assert.AreEqual(2, _viewModel.FavMovies.Count);
        }

        [TestMethod]
        public void ToggleFavoriteCommand_TogglesFavoriteStatus()
        {
            var title = new Title { PrimaryTitle = "Test Movie", IsFavorite = false };

            _viewModel.ToggleFavoriteCommand.Execute(title);
            Assert.IsTrue(title.IsFavorite);

            _viewModel.ToggleFavoriteCommand.Execute(title);
            Assert.IsFalse(title.IsFavorite);
        }

        [TestMethod]
        public void ToggleWatchlistCommand_TogglesWatchlistStatus()
        {
            var title = new Title { PrimaryTitle = "Test Movie", IsInWatchlist = false };

            _viewModel.ToggleWatchlistCommand.Execute(title);
            Assert.IsTrue(title.IsInWatchlist);

            _viewModel.ToggleWatchlistCommand.Execute(title);
            Assert.IsFalse(title.IsInWatchlist);
        }
    }
}
