using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FakeNewsCovid.Domain.Models.Enum;
using FakeNewsCovid.Domain.Services;
using FakeNewsCovid.Domain.Services.Base;
using FakeNewsCovid.Domain.Settings;
using Moq;
using Objectivity.AutoFixture.XUnit2.AutoMoq.Attributes;
using Shouldly;
using Xunit;
using Elasticsearch.Net;

namespace FakeNewsCovid.UnitTests.Elasticsearch
{
    public class ElasticsearchTests
    {
        [Theory]
        [InlineAutoData("http://localhost:9200", "fake_news")]
        public async Task ConnectToESInstance_IsESInstanceValidAndRunning_True(string connection, string indexName)
        {
            // Arrange
            var settings = new ElasticsearchSettings { ConnectionString = connection, DefaultIndex = indexName };
            var service = new ElasticSearchService(settings);

            // Act
            var result = await service.Client.Cluster.HealthAsync();

            // Assert
            result.Status.ShouldNotBe(Health.Red);
        }

        [Theory]
        [MemberAutoMockData(nameof(ElasticsearchTestsFixture.GetTestSettingsAndQuery), MemberType = typeof(ElasticsearchTestsFixture))]
        public async Task FindMoreLikeThis_ThereIsMoreLikeThisInTheIndex_True(
            ElasticsearchSettings settings, string mlt)
        {
            // Arrange
            var service = new ElasticSearchService(settings);

            // Act
            var result = await service.MLT(mlt);

            // Assert
            result.ShouldBe(FakebilityEnum.None);
        }

        [Theory]
        [InlineAutoData("http://localhost:9200", "fake_news", "wrocławska")]
        public async Task TermQuery_ThereIsSimilarTerm_True(string connection, string indexName, string query)
        {
            // Arrange
            var settings = new ElasticsearchSettings { ConnectionString = connection, DefaultIndex = indexName };
            var service = new ElasticSearchService(settings);

            // Act
            var result = await service.MLT(query);

            // Assert
            result.ShouldBe(FakebilityEnum.None);
        }
    }
}
