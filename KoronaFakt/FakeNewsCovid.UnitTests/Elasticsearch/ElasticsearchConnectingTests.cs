using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Elasticsearch.Net;
using FakeNewsCovid.Domain.Models;
using FakeNewsCovid.Domain.Settings;
using Nest;
using Objectivity.AutoFixture.XUnit2.AutoMoq.Attributes;
using Shouldly;
using Xunit;

namespace FakeNewsCovid.UnitTests.Elasticsearch
{
    public class ElasticsearchConnectingTests
    {
        [Theory]
        [InlineAutoData("http://localhost:9200", "fake_news")]
        public async Task ConnectToESInstance_IsESInstanceValidAndRunning_True(string connection, string indexName)
        {
            // Arrange
            var settings = new ElasticsearchSettings { ConnectionString = connection, DefaultIndex = indexName };

            using var connectionSetting = new ConnectionSettings(new Uri(settings.ConnectionString))
                    .DisableDirectStreaming()
                    .DefaultMappingFor<FakeNewsCovidIndex>(d => d
                        .IndexName(settings.DefaultIndex));

            var client = new ElasticClient(connectionSetting);

            // Act
            var result = await client.Cluster.HealthAsync();

            // Assert
            result.Status.ShouldNotBe(Health.Red);
            result.Status.ShouldBe(Health.Green);
        }

        [Theory]
        [MemberAutoMockData(nameof(ElasticsearchTestsFixture.GetElasticClient), MemberType = typeof(ElasticsearchTestsFixture))]
        public async Task CheckIndecies_AtLeastOneIndex_True(ElasticClient client)
        {
            // Arrange

            // Act
            var result = await client.Cat.IndicesAsync();

            // Assert
            result.Records.ShouldNotBeEmpty();
        }
    }
}
