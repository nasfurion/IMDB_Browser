using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using IMDB_Browser.Data;
using IMDB_Browser.Models;
using IMDB_Browser.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace TestProject1
{
    [TestClass]
    public class MediaDetailViewModelTests
    {
        private Mock<ImdbContext> _mockDbContext;
        private MediaDetailViewModel _viewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockDbContext = new Mock<ImdbContext>();
            _viewModel = new MediaDetailViewModel(_mockDbContext.Object);
        }

        [TestMethod]
        public void SelectedTitle_SetProperty_RaisesPropertyChanged()
        {
            // Arrange
            var title = new Title { TitleId = "tt1234567" };
            var propertyChangedRaised = false;
            _viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(MediaDetailViewModel.SelectedTitle))
                {
                    propertyChangedRaised = true;
                }
            };

            // Act
            _viewModel.SelectedTitle = title;

            // Assert
            Assert.IsTrue(propertyChangedRaised);
        }

        [TestMethod]
        public void Genres_SetProperty_RaisesPropertyChanged()
        {
            // Arrange
            var genres = new ObservableCollection<Genre>();
            var propertyChangedRaised = false;
            _viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(MediaDetailViewModel.Genres))
                {
                    propertyChangedRaised = true;
                }
            };

            // Act
            _viewModel.Genres = genres;

            // Assert
            Assert.IsTrue(propertyChangedRaised);
        }

        [TestMethod]
        public void ReceiveParameter_ValidTitle_SetsSelectedTitle()
        {
            // Arrange
            var title = new Title { TitleId = "tt1234567" };

            // Act
            _viewModel.ReceiveParameter(title);

            // Assert
            Assert.AreEqual(title, _viewModel.SelectedTitle);
        }
    }
}
