using FakeNewsCovid.Domain.Models;
using FakeNewsCovid.Domain.Models.Enum;
using FakeNewsCovid.Domain.Services;
using FakeNewsCovid.Domain.Settings;
using Objectivity.AutoFixture.XUnit2.AutoMoq.Attributes;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace FakeNewsCovid.UnitTests.Elasticsearch
{
    public class ElasticsearchMappingTests
    {
        [Theory]
        [MemberAutoMockData(nameof(ElasticsearchTestsFixture.GetTestSettingsAndQuery), MemberType = typeof(ElasticsearchTestsFixture))]
        public async Task FindMoreLikeThis_ThereIsMoreLikeThisInTheIndex_True(
            ElasticsearchSettings settings, string mlt)
        {
            // Arrange
            var service = new ElasticSearchService(settings);

            // Act
            var createIndexResponse = service.Client.Indices.Create("presentation_index", c => c
                .Map<TaggedUrl>(m => m.AutoMap()));

            // Assert
            createIndexResponse.ShardsAcknowledged.ShouldBeTrue();
        }
    }
}
