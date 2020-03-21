using System;
using System.Threading;
using System.Threading.Tasks;
using FakeNewsCovid.Domain.Command;
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
            if (await dbService.IsVerifiedDomainAsync(request.UrlToMark.Host))
            {
                return false; // can not be added as fake becouse domain is verified
            }

            var result = await dbService.AddFakeUrlAsync(request.UrlToMark.AbsoluteUri, request.InnerHtml, request.FakeReasons);

            await esService.InsertToIndexAsync(new FakeNewsCovidIndex
            {
                Id = result.Id,
                Fakebility = result.Fakebility,
                InnerHtml = request.InnerHtml,
                Url = request.UrlToMark.AbsoluteUri
            });

            return true;
        }
    }
}
