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

            var formatted = HtmlHelper.FormatHtml(request.InnerHtml, "div");
            if (formatted.Length == 0)
            {
                formatted = request.InnerHtml;
            }

            var result = await dbService.AddFakeUrlAsync(uri.AbsoluteUri, formatted, HtmlHelper.FormatHtml(request.InnerHtml, "h1"), request.FakeReasons);

            await esService.InsertToIndexAsync(new FakeNewsCovidIndex
            {
                Id = result.Id,
                Fakebility = result.Fakebility,
                Body = formatted,
                BodyShingle = formatted,
                Url = uri.AbsoluteUri,
                Title = HtmlHelper.FormatHtml(request.InnerHtml, "h1"),
                TimeStamp = GetRandomDate()
            });

            return true;
        }

        private DateTime GetRandomDate()
        {
            Random gen = new Random();
            DateTime start = new DateTime(2019, 11, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }
    }
}
