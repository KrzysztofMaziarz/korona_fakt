using FakeNewsCovid.Domain.Models;
using FakeNewsCovid.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FakeNewsCovid.Domain.Services.Base
{
    public interface IFakeNewsDbService
    {
        Task<(FakebilityEnum, List<FakeReason>)> CheckUrlFakebilityAsync(string urlToCheck);

        Task<TaggedUrl> AddFakeUrlAsync(string url, string innerWebHtml, ICollection<string> fakeReasons);

        Task<bool> IsVerifiedDomainAsync(string hostDomain);
    }
}
