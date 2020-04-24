using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FakeNewsCovid.Domain.Services;
using FakeNewsCovid.Domain.Settings;
using Shouldly;
using Xunit;

namespace FakeNewsCovid.UnitTests.Elasticsearch
{
    public class ElasticsearchAnalyzingTests
    {

        [Theory]
        [InlineAutoData("http://localhost:9200", "fake_news", "premierem")]
        public async Task AnalyzeTermUsingAnalyzer_AreThereAnalyzedTokens_NotEmpty(string connection, string indexName, string term)
        {
            // Arrange
            var settings = new ElasticsearchSettings { ConnectionString = connection, DefaultIndex = indexName };
            var service = new ElasticSearchService(settings);

            // Act
            var searchResponse = await service.Client.Indices.AnalyzeAsync(x => x
                .Index(indexName)
                .Analyzer("polish_analyzer")
                .Text(term));

            // Assert
            searchResponse.Tokens.ShouldNotBeEmpty();
        }
    }
}
