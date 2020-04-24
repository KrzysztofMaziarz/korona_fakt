using System;
using FakeNewsCovid.Domain.Models.Enum;
using Nest;

namespace FakeNewsCovid.Domain.Models
{
    [ElasticsearchType(RelationName = "attibute_mapping_index")]
    public class FakeNewsCovidIndexAttributeMapping
    {
        public Guid Id { get; set; }

        [Keyword(Name = "url")]
        public string Url { get; set; }

        public string Title { get; set; }

        [Text(Fielddata = true, Analyzer = "whitespace", Name = "body")]
        public string Body { get; set; }

        public string TitleShingle { get; set; }

        [Number(NumberType.Byte, Name = "fake")]
        public FakebilityEnum Fakebility { get; set; }

        [Date(Format = "ddMMyyyy")]
        public DateTime TimeStamp { get; set; }

        public int FakeRating { get; set; }
    }
}
