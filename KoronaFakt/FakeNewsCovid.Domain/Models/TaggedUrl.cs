using FakeNewsCovid.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace FakeNewsCovid.Domain.Models
{
    public class TaggedUrl
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public int TaggedFakeCount { get; set; }

        public FakebilityEnum Fakebility { get; set; }

        public string InnerWeb { get; set; }
    }
}
