using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FakeNewsCovid.Domain.Models;
using FakeNewsCovid.Domain.Services;
using FakeNewsCovid.Domain.Settings;
using Nest;
using Shouldly;
using Xunit;

namespace FakeNewsCovid.UnitTests.Elasticsearch
{
    public class ElasticsearchSearchingTests
    {
        // term
        [Theory]
        [InlineAutoData("http://localhost:9200", "fake_news", "koronawirus")]
        public async Task TermQuery_IsThereResult_True(string connection, string indexName, string term)
        {
            // Arrange
            var settings = new ElasticsearchSettings { ConnectionString = connection, DefaultIndex = indexName };
            var service = new ElasticSearchService(settings);

            // Act
            var searchResponse = await service.Client.SearchAsync<FakeNewsCovidIndex>(s => s
                .Query(q => q
                    .Term(t => t
                        .Field(f => f.Title)
                        .Value(term))));

            // Assert
            searchResponse.Documents.ShouldNotBeEmpty();
        }

        [Theory]
        [InlineAutoData("http://localhost:9200", "fake_news", "koronawirus")]
        public async Task TermQuerySourceExcluded_IsThereResult_True(string connection, string indexName, string term)
        {
            // Arrange
            var settings = new ElasticsearchSettings { ConnectionString = connection, DefaultIndex = indexName };
            var service = new ElasticSearchService(settings);

            // Act
            var searchResponse = await service.Client.SearchAsync<FakeNewsCovidIndex>(s => s
                .Source(s => s.Excludes(e => e.Fields(f => f.Body, s => s.TitleShingle, l => l.FakeRating)))
                .Query(q => q
                    .Term(t => t
                        .Field(f => f.Title)
                        .Value(term))));

            // Assert
            searchResponse.Documents.ShouldNotBeEmpty();
        }

        [Theory]
        [InlineAutoData("http://localhost:9200", "fake_news", "koronawirus")]
        public async Task TermQuerySourceExcludedAddedSortScriptField_IsThereResult_True(string connection, string indexName, string term)
        {
            // Arrange
            var settings = new ElasticsearchSettings { ConnectionString = connection, DefaultIndex = indexName };
            var service = new ElasticSearchService(settings);

            // Act
            var searchResponse = await service.Client.SearchAsync<FakeNewsCovidIndex>(s => s
                .Source(s => s.Excludes(e => e.Fields(f => f.Body, s => s.TitleShingle)))
                .Size(3)
                .Query(q => q
                    .Term(t => t
                        .Field(f => f.Title)
                        .Value(term)))
                .Sort(so => so
                    .Descending(d => d.FakeRating))
                .ScriptFields(sf => sf.ScriptField("Stems", sc => sc.Source("doc['title']"))));

            // Assert
            searchResponse.Documents.ShouldNotBeEmpty();
        }

        // bool, term, dateRange
        // w słowniku - premierem
        // nie w słowniku - koronawirus
        [Theory]
        [InlineAutoData("http://localhost:9200", "fake_news", "koronawirus")]
        public async Task TermQueryDateRange_IsThereResult_True(string connection, string indexName, string term)
        {
            // Arrange
            var settings = new ElasticsearchSettings { ConnectionString = connection, DefaultIndex = indexName };
            var service = new ElasticSearchService(settings);

            // Act
            var searchResponse = await service.Client.SearchAsync<FakeNewsCovidIndex>(s => s
                .Query(q => q
                    .Bool(b => b
                        .Must(
                            m => m
                            .DateRange(d => d
                                .Field(fi => fi.TimeStamp)
                                .GreaterThanOrEquals(DateTime.Now.AddMonths(-1).Date)
                                .LessThan(DateTime.Now.Date)),
                            m => m
                            .Term(t => t
                                .Field(f => f.Body)
                                .Value(term))))));

            // Assert
            searchResponse.Documents.ShouldNotBeEmpty();
        }

        // term vs fuzzy -> premier, premjer, premiera, premjera
        [Theory]
        [InlineAutoData("http://localhost:9200", "fake_news", "premjer")]
        public async Task FuzzyQuery_IsThereResult_True(string connection, string indexName, string term)
        {
            // Arrange
            var settings = new ElasticsearchSettings { ConnectionString = connection, DefaultIndex = indexName };
            var service = new ElasticSearchService(settings);

            // Act
            var termResponse = await service.Client.SearchAsync<FakeNewsCovidIndex>(s => s
                .Source(s => s.Excludes(e => e.Fields(f => f.Body, s => s.TitleShingle)))
                .Size(100)
                .Query(q => q
                    .Term(t => t
                        .Field(f => f.Title)
                        .Value(term))));

            var fuzzyResponse = await service.Client.SearchAsync<FakeNewsCovidIndex>(s => s
                .Source(s => s.Excludes(e => e.Fields(f => f.Body, s => s.TitleShingle)))
                .Size(100)
                .Query(q => q
                    .Fuzzy(t => t
                        .Field(f => f.Title)
                        .Value(term))));

            var termTitles = new List<string>();
            termTitles.AddRange(termResponse.Documents.Select(x => x.Title));
            var fuzzyTitles = new List<string>();
            fuzzyTitles.AddRange(fuzzyResponse.Documents.Select(x => x.Title));

            // Assert
            termResponse.Total.ShouldBeLessThanOrEqualTo(fuzzyResponse.Total);
        }

        // mlt -> 50, 70, 90
        [Theory]
        [InlineAutoData("http://localhost:9200", "fake_news", "Premier League Jose Mourinho")]
        public async Task MLT_IsThereResult_True(string connection, string indexName, string term)
        {
            // Arrange
            var settings = new ElasticsearchSettings { ConnectionString = connection, DefaultIndex = indexName };
            var service = new ElasticSearchService(settings);

            // Act
            var searchResult = await service.Client.SearchAsync<FakeNewsCovidIndex>(search => search
                .Size(50)
                .Source(s => s.Excludes(e => e.Fields(f => f.Body, s => s.TitleShingle)))
                .Query(q => q
                    .Bool(b => b
                        .Must(
                            m => m.DateRange(dr => dr
                                    .Field(fd => fd.TimeStamp)
                                    .GreaterThanOrEquals(DateTime.Today.AddDays(-90))
                                    .LessThan(DateTime.Today.AddDays(1))),
                            m => m.MoreLikeThis(ml => ml
                                    .Fields(fds => fds
                                        .Field(fd => fd.Title))
                                    .Like(l => l
                                    .Text(term))
                                    .MinTermFrequency(1)
                                    .MinDocumentFrequency(1)
                                    .MinimumShouldMatch(new MinimumShouldMatch("90%")))))));

            // Assert
            searchResult.Total.ShouldBeGreaterThan(0);
        }
    }
}
