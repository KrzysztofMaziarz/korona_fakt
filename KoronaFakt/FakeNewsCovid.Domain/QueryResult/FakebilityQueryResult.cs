using FakeNewsCovid.Domain.Models.Enum;
using System.Collections.Generic;

namespace FakeNewsCovid.Domain.QueryResult
{
    public class FakebilityQueryResult
    {
        public FakebilityEnum Fakebility { get; set; }

        public IEnumerable<string> FakeReasons { get; set; }
    }
}
