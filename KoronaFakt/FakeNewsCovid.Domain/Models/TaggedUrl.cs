using System.Collections.Generic;
using FakeNewsCovid.Domain.Models.Enum;

namespace FakeNewsCovid.Domain.Models
{
    public class TaggedUrl
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public int TaggedFakeCount { get; set; }

        public FakebilityEnum Fakebility { get; set; }

        public string InnerWeb { get; set; }

        public string Title { get; set; }

        public List<FakeReason> FakeReasons { get; set; }

        public string DomainHost { get; set; }
    }
}
