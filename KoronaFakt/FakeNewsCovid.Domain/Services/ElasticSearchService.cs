using FakeNewsCovid.Domain.Models;
using FakeNewsCovid.Domain.Models.Enum;
using FakeNewsCovid.Domain.Services.Base;
using FakeNewsCovid.Domain.Settings;
using Nest;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FakeNewsCovid.Domain.Services
{
    public class ElasticSearchService : IElasticSearchService
    {
        public readonly ElasticClient Client;

        public ElasticSearchService(ElasticsearchSettings settings)
        {
            var connectionSetting = new ConnectionSettings(new Uri(settings.ConnectionString))
                    .DisableDirectStreaming()
                    .DefaultMappingFor<FakeNewsCovidIndex>(d => d
                        .IndexName(settings.DefaultIndex));

            Client = new ElasticClient(connectionSetting);
        }

        public async Task InsertToIndexAsync(FakeNewsCovidIndex item)
        {
            await Client.IndexDocumentAsync<FakeNewsCovidIndex>(item);
        }

        public async Task<FakebilityEnum> MLT(string innerHtml)
        {
            var search = await Client.SearchAsync<FakeNewsCovidIndex>(s => s
                .Query(q => q
                    .MoreLikeThis(mlt => mlt
                                    .Fields(fs => fs
                                        .Field(f => f.Body))
                                    .Like(l => l
                                        .Text(innerHtml))
                                        .MinTermFrequency(1)
                                        .MinDocumentFrequency(1)
                                        .MinimumShouldMatch(new MinimumShouldMatch("80%")))));

            var result = search.Documents.ToList();

            if (result != null && result.Count > 0)
            {
                return FakebilityEnum.Suspected;
            }

            return FakebilityEnum.None;
        }
    }
}
