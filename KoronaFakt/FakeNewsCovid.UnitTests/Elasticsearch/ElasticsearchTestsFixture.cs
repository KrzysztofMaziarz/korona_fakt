using AutoFixture;
using FakeNewsCovid.Domain.Models;
using FakeNewsCovid.Domain.Services;
using FakeNewsCovid.Domain.Settings;
using Nest;
using System;
using System.Collections.Generic;

namespace FakeNewsCovid.UnitTests.Elasticsearch
{
    public static class ElasticsearchTestsFixture
    {
        public static IEnumerable<object[]> GetTestSettings()
        {
            var settings = new ElasticsearchSettings
            {
                ConnectionString = "http://localhost:9200",
                DefaultIndex = "fake_news"
            };

            return new List<object[]>
            {
                new object[]
                {
                    settings
                }
            };
        }

        public static IEnumerable<object[]> GetElasticClient()
        {
            var settings = new ElasticsearchSettings
            {
                ConnectionString = "http://localhost:9200",
                DefaultIndex = "fake_news"
            };

            var connectionSetting = new ConnectionSettings(new Uri(settings.ConnectionString))
                                .DisableDirectStreaming()
                                .DefaultMappingFor<FakeNewsCovidIndex>(d => d
                                    .IndexName(settings.DefaultIndex));

            var client = new ElasticClient(connectionSetting);

            return new List<object[]>
            {
                new object[]
                {
                    client
                }
            };
        }
    }
}
