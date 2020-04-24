using System;
using System.Threading;
using System.Threading.Tasks;
using FakeNewsCovid.Domain.Command;
using FakeNewsCovid.Domain.Extensions;
using FakeNewsCovid.Domain.Helper;
using FakeNewsCovid.Domain.Models;
using FakeNewsCovid.Domain.Models.Enum;
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
                Id = GuidExtension.NewDeterministicGuid(uri.AbsoluteUri),
                Fakebility = GetRandomEnum(),
                Body = formatted,
                TitleShingle = HtmlHelper.FormatHtml(request.InnerHtml, "h1"),
                Url = uri.AbsoluteUri,
                Title = HtmlHelper.FormatHtml(request.InnerHtml, "h1"),
                TimeStamp = GetRandomDate(),
                FakeRating = RandomNumber(0, 100)
            });

            return true;
        }

        private DateTime GetRandomDate()
        {
            Random gen = new Random();
            DateTime start = new DateTime(2019, 12, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }

        private FakebilityEnum GetRandomEnum()
        {
            Array values = Enum.GetValues(typeof(FakebilityEnum));
            Random random = new Random();
            return (FakebilityEnum)values.GetValue(random.Next(values.Length));
        }

        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
    }
}
