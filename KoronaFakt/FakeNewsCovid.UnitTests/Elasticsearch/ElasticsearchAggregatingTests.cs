using System;
using System.Threading.Tasks;
using FakeNewsCovid.Domain.Models;
using Nest;
using Objectivity.AutoFixture.XUnit2.AutoMoq.Attributes;
using Shouldly;
using Xunit;

namespace FakeNewsCovid.UnitTests.Elasticsearch
{
    public class ElasticsearchAggregatingTests
    {
        [Theory]
        [MemberAutoMockData(nameof(ElasticsearchTestsFixture.GetElasticClient), MemberType = typeof(ElasticsearchTestsFixture))]
        public async Task TitleTermsAgg_GetBucketsWithWords_True(ElasticClient client)
        {
            // Arrange

            // Act
            var aggResponse = await client.SearchAsync<FakeNewsCovidIndex>(s => s
                .Size(0)
                .Aggregations(ag => ag
                    .Terms(nameof(ElasticsearchAggregatingTests), f => f
                        .Size(50)
                        .Field(fi => fi.Title))));

            // Assert
            aggResponse.Aggregations.Terms(nameof(ElasticsearchAggregatingTests)).ShouldNotBeNull();
        }

        [Theory]
        [MemberAutoMockData(nameof(ElasticsearchTestsFixture.GetElasticClient), MemberType = typeof(ElasticsearchTestsFixture))]
        public async Task MuliAggr_GetBucketsWithWords_True(ElasticClient client)
        {
            // Arrange

            // Act
            var aggResponse = await client.SearchAsync<FakeNewsCovidIndex>(search => search
                .Size(0)
                .Aggregations(a => a
                                .Sum("fake_sum", y => y.Field(c => c.FakeRating))
                                .Average("fake_avg", y => y.Field(c => c.FakeRating))
                                .Terms("fake_agg", t => t.Field(fi => fi.Fakebility))));

            // Assert
            aggResponse.Aggregations.Sum("fake_sum").ShouldNotBeNull();
            aggResponse.Aggregations.Average("fake_avg").ShouldNotBeNull();
            aggResponse.Aggregations.Terms("fake_agg").ShouldNotBeNull();
        }

        [Theory]
        [MemberAutoMockData(nameof(ElasticsearchTestsFixture.GetElasticClient), MemberType = typeof(ElasticsearchTestsFixture))]
        public async Task TitleShingleTermsAgg_GetBucketsWithWords_True(ElasticClient client)
        {
            // Arrange
            string keyword = "premier";

            // Act
            var aggResponse = await client.SearchAsync<FakeNewsCovidIndex>(search => search
                .Size(0)
                .Aggregations(ag => ag
                    .Filter("TrendFilter", tf => tf
                        .Filter(f => f
                            .DateRange(d => d
                                .Field(f => f.TimeStamp)
                                .GreaterThanOrEquals(DateTime.Now.AddMonths(-3))
                                .LessThanOrEquals(DateTime.Now)))
                        .Aggregations(sagr => sagr
                            .Terms("ShingleAggr", te => te
                                .Size(20)
                                .Field(fi => fi.TitleShingle)
                                .Include(".* " + keyword + "|" + keyword + " .*"))))));

            // Assert
            aggResponse.Aggregations.Filter("TrendFilter").ShouldNotBeNull();
        }

        [Theory]
        [MemberAutoMockData(nameof(ElasticsearchTestsFixture.GetElasticClient), MemberType = typeof(ElasticsearchTestsFixture))]
        public async Task TitleShingleTermsFakebilityTermsAgg_GetBucketsWithWords_True(ElasticClient client)
        {
            // Arrange
            string keyword = "premier";

            // Act
            var aggResponse = await client.SearchAsync<FakeNewsCovidIndex>(search => search
                .Size(0)
                .Aggregations(ag => ag
                    .Filter("TrendFilter", tf => tf
                        .Filter(f => f
                            .DateRange(d => d
                                .Field(f => f.TimeStamp)
                                .GreaterThanOrEquals(DateTime.Now.AddMonths(-3))
                                .LessThanOrEquals(DateTime.Now)))
                        .Aggregations(sagr => sagr
                            .Terms("ShingleAggr", te => te
                                .Size(50)
                                .Field(fi => fi.TitleShingle)
                                .Include(".* " + keyword + "|" + keyword + " .*")
                                .Aggregations(fak => fak
                                    .Terms("FakebilityEnum", te => te
                                    .Size(50)
                                    .Field(fi => fi.Fakebility))))))));

            // Assert
            aggResponse.Aggregations.Filter("TrendFilter").Terms("ShingleAggr").ShouldNotBeNull();
        }
    }
}
