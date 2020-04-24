using System.Threading.Tasks;
using FakeNewsCovid.Domain.Models;
using Nest;
using Objectivity.AutoFixture.XUnit2.AutoMoq.Attributes;
using Shouldly;
using Xunit;

namespace FakeNewsCovid.UnitTests.Elasticsearch
{
    public class ElasticsearchMappingTests
    {
        // automap
        [Theory]
        [MemberAutoMockData(nameof(ElasticsearchTestsFixture.GetElasticClient), MemberType = typeof(ElasticsearchTestsFixture))]
        public async Task CreateIndex_IsIndexCreated_True(ElasticClient client)
        {
            // Arrange

            // Act
            var createIndexResponse = await client.Indices.CreateAsync("presentation_index", c => c
                .Map<FakeNewsCovidIndex>(m => m.AutoMap()));

            // Assert
            createIndexResponse.Acknowledged.ShouldBeTrue();
        }

        // automap + attribute mapping
        [Theory]
        [MemberAutoMockData(nameof(ElasticsearchTestsFixture.GetElasticClient), MemberType = typeof(ElasticsearchTestsFixture))]
        public async Task CreateIndexAttributeMapping_IsIndexCreated_True(ElasticClient client)
        {
            // Arrange

            // Act
            var createIndexResponse = await client.Indices.CreateAsync("presentation_index_att", c => c
                .Map<FakeNewsCovidIndexAttributeMapping>(m => m.AutoMap()));

            // Assert
            createIndexResponse.Acknowledged.ShouldBeTrue();
        }

        // automap + fluent mapping
        [Theory]
        [MemberAutoMockData(nameof(ElasticsearchTestsFixture.GetElasticClient), MemberType = typeof(ElasticsearchTestsFixture))]
        public async Task CreateIndexFluendMapping_IsIndexCreated_True(ElasticClient client)
        {
            // Arrange

            // Act
            var createIndexResponse = await client.Indices.CreateAsync("presentation_index_fluent", c => c
                .Map<FakeNewsCovidIndex>(m => m
                    .Properties(p => p
                        .Text(s => s.Name(n => n.TitleShingle))
                        .Text(s => s.Name(n => n.Title))
                        .Text(s => s.Name(n => n.Url)))));

            // Assert
            createIndexResponse.Acknowledged.ShouldBeTrue();
        }
    }
}
