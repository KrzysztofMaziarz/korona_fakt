using FakeNewsCovid.Domain.Context;
using FakeNewsCovid.Domain.Models;
using FakeNewsCovid.Domain.Models.Enum;
using FakeNewsCovid.Domain.Services.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeNewsCovid.Domain.Services
{
    public class FakeNewsDbService : IFakeNewsDbService
    {
        private readonly FakeNewsCovidContext context;

        public FakeNewsDbService(FakeNewsCovidContext context)
        {
            this.context = context;
        }

        public async Task<TaggedUrl> AddFakeUrlAsync(string url, string innerWebHtml, ICollection<string> fakeReasonUrls)
        {
            var existing = await context.TaggedUrls.Include(i => i.FakeReasons).SingleOrDefaultAsync(x => x.Url == url);

            if (existing == null)
            {
                var fakeReasons = new List<FakeReason>();
                if (fakeReasonUrls != null)
                {
                    foreach (var u in fakeReasonUrls)
                    {
                        fakeReasons.Add(new FakeReason { ReasonNotFakeUrl = u });
                    }
                }

                var newFake = new TaggedUrl
                {
                    Fakebility = FakebilityEnum.Suspected,
                    InnerWeb = innerWebHtml,
                    TaggedFakeCount = 0,
                    FakeReasons = fakeReasons.Count > 0 ? fakeReasons : null,
                    Url = url
                };

                await context.TaggedUrls.AddAsync(newFake);
                await context.SaveChangesAsync();

                return newFake;
            }

            existing.TaggedFakeCount++;

            if (existing.TaggedFakeCount >= 2)
            {
                existing.Fakebility = FakebilityEnum.Fake;
            }

            if (fakeReasonUrls != null)
            {
                foreach (var u in fakeReasonUrls)
                {
                    if (!existing.FakeReasons.Any(x => x.ReasonNotFakeUrl == u))
                    {
                        existing.FakeReasons.Add(new FakeReason { ReasonNotFakeUrl = u });
                    }
                    else
                    {
                        existing.FakeReasons.SingleOrDefault(x => x.ReasonNotFakeUrl == u).ReasonNotFakeCount++;
                    }
                }
            }

            context.Update(existing);
            await context.SaveChangesAsync();
            return existing;
        }

        public async Task<FakebilityEnum> CheckUrlFakebilityAsync(string urlToCheck)
        {
            var result = await context.TaggedUrls.SingleOrDefaultAsync(x => x.Url == urlToCheck);

            if (result != null)
            {
                return result.Fakebility;
            }

            return FakebilityEnum.None;
        }

        public async Task<bool> IsVerifiedDomainAsync(string hostDomain)
        {
            var result = await context.VerifiedDomains.SingleOrDefaultAsync(x => x.DomainHost == hostDomain);
            if (result != null)
            {
                return true;
            }

            return false;
        }
    }
}
