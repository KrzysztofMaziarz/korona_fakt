using FakeNewsCovid.Domain.Models.Enum;

namespace FakeNewsCovid.Domain.Models
{
    public class FakeNewsCovidIndex
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public FakebilityEnum Fakebility { get; set; }
    }
}
