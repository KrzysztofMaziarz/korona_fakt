using FakeNewsCovid.Domain.Models.Enum;
using System;

namespace FakeNewsCovid.Domain.Models
{
    public class FakeNewsCovidIndex
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string BodyShingle { get; set; }

        public FakebilityEnum Fakebility { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
