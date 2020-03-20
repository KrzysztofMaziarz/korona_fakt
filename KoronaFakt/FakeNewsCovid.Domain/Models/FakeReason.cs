using System;
using System.Collections.Generic;
using System.Text;

namespace FakeNewsCovid.Domain.Models
{
    public class FakeReason
    {
        public int Id { get; set; }

        public string ReasonNotFakeUrl { get; set; }

        public int TaggedUrlId { get; set; }

        public int ReasonNotFakeCount { get; set; }
    }
}
