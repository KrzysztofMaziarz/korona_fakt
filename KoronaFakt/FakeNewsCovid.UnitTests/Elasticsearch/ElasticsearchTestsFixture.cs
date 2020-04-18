using AutoFixture;
using FakeNewsCovid.Domain.Services;
using FakeNewsCovid.Domain.Settings;
using System.Collections.Generic;

namespace FakeNewsCovid.UnitTests.Elasticsearch
{
    public static class ElasticsearchTestsFixture
    {
        public static IEnumerable<object[]> GetTestSettingsAndQuery()
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
                    settings,
                    "sprawdzenie mlt wrocławska"
                }
            };
        }
    }
}
