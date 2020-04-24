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
    public class ElasticsearchIndexingTests
    {
        [Theory]
        [MemberAutoMockData(nameof(ElasticsearchTestsFixture.GetTestSettings), MemberType = typeof(ElasticsearchTestsFixture))]
        public async Task CreateIndex_IsIndexCreated_True(
            ElasticsearchSettings settings)
        {
            // Arrange
            var service = new ElasticSearchService(settings);

            // Act
            var indexReponse = await service.Client.IndexDocumentAsync<FakeNewsCovidIndex>(new FakeNewsCovidIndex
            {
                Title = "title"
            });

            // Assert
            indexReponse.IsValid.ShouldBeTrue();
        }
    }
}
