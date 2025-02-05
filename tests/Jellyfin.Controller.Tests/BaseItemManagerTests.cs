using System;
using MediaBrowser.Controller.BaseItemManager;
using MediaBrowser.Controller.Configuration;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Audio;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Model.Configuration;
using Moq;
using Xunit;

namespace Jellyfin.Controller.Tests
{
    public class BaseItemManagerTests
    {
        [Theory]
        [InlineData(typeof(Book), "LibraryEnabled", true)]
        [InlineData(typeof(Book), "LibraryDisabled", false)]
        [InlineData(typeof(MusicArtist), "Enabled", true)]
        [InlineData(typeof(MusicArtist), "ServerDisabled", false)]
        public void IsMetadataFetcherEnabled_ChecksOptions_ReturnsExpected(Type itemType, string fetcherName, bool expected)
        {
            BaseItem item = (BaseItem)Activator.CreateInstance(itemType)!;

            var libraryOptions = new LibraryOptions
            {
                TypeOptions = new[]
                {
                    new TypeOptions
                    {
                        Type = "Book",
                        MetadataFetchers = new[] { "LibraryEnabled" }
                    }
                }
            };

            var serverConfiguration = new ServerConfiguration();
            foreach (var typeConfig in serverConfiguration.MetadataOptions)
            {
                typeConfig.DisabledMetadataFetchers = new[] { "ServerDisabled" };
            }

            var serverConfigurationManager = new Mock<IServerConfigurationManager>();
            serverConfigurationManager.Setup(scm => scm.Configuration)
                .Returns(serverConfiguration);

            var baseItemManager = new BaseItemManager(serverConfigurationManager.Object);
            var actual = baseItemManager.IsMetadataFetcherEnabled(item, libraryOptions, fetcherName);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(typeof(Book), "LibraryEnabled", true)]
        [InlineData(typeof(Book), "LibraryDisabled", false)]
        [InlineData(typeof(MusicArtist), "Enabled", true)]
        [InlineData(typeof(MusicArtist), "ServerDisabled", false)]
        public void IsImageFetcherEnabled_ChecksOptions_ReturnsExpected(Type itemType, string fetcherName, bool expected)
        {
            BaseItem item = (BaseItem)Activator.CreateInstance(itemType)!;

            var libraryOptions = new LibraryOptions
            {
                TypeOptions = new[]
                {
                    new TypeOptions
                    {
                        Type = "Book",
                        ImageFetchers = new[] { "LibraryEnabled" }
                    }
                }
            };

            var serverConfiguration = new ServerConfiguration();
            foreach (var typeConfig in serverConfiguration.MetadataOptions)
            {
                typeConfig.DisabledImageFetchers = new[] { "ServerDisabled" };
            }

            var serverConfigurationManager = new Mock<IServerConfigurationManager>();
            serverConfigurationManager.Setup(scm => scm.Configuration)
                .Returns(serverConfiguration);

            var baseItemManager = new BaseItemManager(serverConfigurationManager.Object);
            var actual = baseItemManager.IsImageFetcherEnabled(item, libraryOptions, fetcherName);

            Assert.Equal(expected, actual);
        }
    }
}
