using System;
using System.Threading;
using System.Threading.Tasks;
using FakeNewsCovid.Domain.Helper;
using FakeNewsCovid.Domain.Query;
using FakeNewsCovid.Domain.QueryResult;
using FakeNewsCovid.Domain.Services.Base;
using MediatR;

namespace FakeNewsCovid.Domain.QueryHandler
{
    public class FakebilityQueryHandler : IRequestHandler<FakebilityQuery, FakebilityQueryResult>
    {
        private readonly IFakeNewsDbService dbService;
        private readonly IElasticSearchService esService;

        public FakebilityQueryHandler(IFakeNewsDbService dbService, IElasticSearchService esService)
        {
            this.dbService = dbService;
            this.esService = esService;
        }

        public async Task<FakebilityQueryResult> Handle(FakebilityQuery request, CancellationToken cancellationToken)
        {
            if (await dbService.IsVerifiedDomainAsync(request.UrlAddress.Host))
            {
                return new FakebilityQueryResult { Fakebility = Models.Enum.FakebilityEnum.Verified };
            }

            var result = await dbService.CheckUrlFakebilityAsync(request.UrlAddress.AbsoluteUri);

            if (result == Models.Enum.FakebilityEnum.None)
            {
                var formatted = HtmlHelper.FormatHtml(request.InnerHtml);
                result = await esService.MLT(formatted);
            }

            return new FakebilityQueryResult { Fakebility = result };
        }
    }
}
