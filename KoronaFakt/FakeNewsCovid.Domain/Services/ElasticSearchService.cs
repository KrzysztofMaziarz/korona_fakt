using FakeNewsCovid.Domain.Context;
using FakeNewsCovid.Domain.Services.Base;

namespace FakeNewsCovid.Domain.Services
{
    public class ElasticSearchService : IElasticSearchService
    {
        private readonly FakeNewsCovidContext context;

        public ElasticSearchService(FakeNewsCovidContext context)
        {
            this.context = context;
        }

    }
}
