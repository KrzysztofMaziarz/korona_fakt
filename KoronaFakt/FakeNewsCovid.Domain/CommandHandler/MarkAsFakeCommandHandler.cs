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
    public class MarkAsFakeCommandHandler : IRequestHandler<MarkAsFakeCommand, Unit>
    {
        private readonly IFakeNewsDbService dbService;
        private readonly IElasticSearchService esService;

        public MarkAsFakeCommandHandler(IFakeNewsDbService dbService, IElasticSearchService esService)
        {
            this.dbService = dbService;
            this.esService = esService;
        }

        public async Task<Unit> Handle(MarkAsFakeCommand request, CancellationToken cancellationToken)
        {
            var result = await dbService.AddFakeUrlAsync(request.UrlToMark, request.InnerHtml, request.FakeReasons);

            await esService.InsertToIndexAsync(new FakeNewsCovidIndex
            {
                Id = result.Id,
                Fakebility = result.Fakebility,
                InnerHtml = request.InnerHtml,
                Url = request.UrlToMark
            });

            return Unit.Value;
        }
    }
}
