using FakeNewsCovid.Domain.Models.Enum;

namespace FakeNewsCovid.Domain.Models
{
    public class FakeNewsCovidIndex
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string InnerHtml { get; set; }

        public FakebilityEnum Fakebility { get; set; }
    }
}
