using FakeNewsCovid.Domain.Models;
using FakeNewsCovid.Domain.Models.Enum;
using System.Threading.Tasks;

namespace FakeNewsCovid.Domain.Services.Base
{
    public interface IElasticSearchService
    {
        Task InsertToIndexAsync(FakeNewsCovidIndex item);

        Task<FakebilityEnum> MLT(string innerHtml);
    }
}
