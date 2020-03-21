using System;
using System.Threading;
using System.Threading.Tasks;
using FakeNewsCovid.Domain.Command;
using FakeNewsCovid.Domain.Helper;
using FakeNewsCovid.Domain.Models;
using FakeNewsCovid.Domain.Query;
using FakeNewsCovid.Domain.QueryResult;
using FakeNewsCovid.Domain.Services.Base;
using MediatR;

namespace FakeNewsCovid.Domain.CommandHandler
{
    public class MarkAsFakeCommandHandler : IRequestHandler<MarkAsFakeCommand, bool>
    {
        private readonly IFakeNewsDbService dbService;
        private readonly IElasticSearchService esService;

        public MarkAsFakeCommandHandler(IFakeNewsDbService dbService, IElasticSearchService esService)
        {
            this.dbService = dbService;
            this.esService = esService;
        }

        public async Task<bool> Handle(MarkAsFakeCommand request, CancellationToken cancellationToken)
        {
            var uri = new Uri(request.UrlToMark.Contains("\"") ? request.UrlToMark.Replace("\"", string.Empty) : request.UrlToMark);
            if (await dbService.IsVerifiedDomainAsync(uri.Host))
            {
                return false; // can not be added as fake becouse domain is verified
            }

            var formatted = HtmlHelper.FormatHtml(request.InnerHtml);
            if (formatted.Length == 0)
            {
                formatted = request.InnerHtml;
            }

            var result = await dbService.AddFakeUrlAsync(uri.AbsoluteUri, formatted, request.FakeReasons);

            await esService.InsertToIndexAsync(new FakeNewsCovidIndex
            {
                Id = result.Id,
                Fakebility = result.Fakebility,
                Body = formatted,
                Url = uri.AbsoluteUri
            });

            return true;
        }
    }
}
