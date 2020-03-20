using System;
using System.Linq;
using System.Threading.Tasks;
using FakeNewsCovid.Domain.Context;
using FakeNewsCovid.Domain.Models;
using FakeNewsCovid.Domain.Models.Enum;
using FakeNewsCovid.Domain.Services.Base;
using Nest;

namespace FakeNewsCovid.Domain.Services
{
    public class ElasticSearchService : IElasticSearchService
    {
        private readonly ElasticClient client;

        public ElasticSearchService()
        {
            var connectionSetting = new ConnectionSettings(new Uri("http://10.50.0.10:9200"))
                    .DisableDirectStreaming()
                    .DefaultMappingFor<FakeNewsCovidIndex>(d => d
                        .IndexName("fake_news"));

            client = new ElasticClient(connectionSetting);
        }

        public async Task InsertToIndexAsync(FakeNewsCovidIndex item)
        {
            await client.IndexDocumentAsync<FakeNewsCovidIndex>(item);
        }

        public async Task<FakebilityEnum> MLT(string innerHtml)
        {
            var search = await client.SearchAsync<FakeNewsCovidIndex>(s => s
                .Query(q => q
                    .MoreLikeThis(mlt => mlt
                                    .Fields(fs => fs
                                        .Field(f => f.InnerHtml))
                                    .Like(l => l
                                        .Text(innerHtml))
                                        .MinTermFrequency(1)
                                        .MinimumShouldMatch(new MinimumShouldMatch("70%")))));

            var result = search.Documents.ToList();

            if (result != null && result.Count > 0)
            {
                return FakebilityEnum.Suspected;
            }

            return FakebilityEnum.None;
        }
    }
}
