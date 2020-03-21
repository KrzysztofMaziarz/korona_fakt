using System;
using System.Linq;
using System.Text.RegularExpressions;
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
            var uri = new Uri(request.UrlAddress.Contains("\"") ? request.UrlAddress.Replace("\"", string.Empty) : request.UrlAddress);
            if (await dbService.IsVerifiedDomainAsync(uri.Host))
            {
                return new FakebilityQueryResult { Fakebility = Models.Enum.FakebilityEnum.Verified };
            }

            var result = await dbService.CheckUrlFakebilityAsync(uri.AbsoluteUri);

            if (result.Item1 == Models.Enum.FakebilityEnum.None)
            {
                var formatted = HtmlHelper.FormatHtml(request.InnerHtml);
                if (formatted.Length == 0)
                {
                    formatted = request.InnerHtml;
                }

                result.Item1 = await esService.MLT(formatted);
            }

            return new FakebilityQueryResult { Fakebility = result.Item1, FakeReasons = result.Item2 != null ? result.Item2.Select(x => x.ReasonNotFakeUrl) : null };
        }
    }
}
