﻿using System;
using FakeNewsCovid.Domain.Models.Enum;

namespace FakeNewsCovid.Domain.Models
{
    public class FakeNewsCovidIndex
    {
        public Guid Id { get; set; }

        public string Url { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string TitleShingle { get; set; }

        public FakebilityEnum Fakebility { get; set; }

        public DateTime TimeStamp { get; set; }

        public int FakeRating { get; set; }
    }
}
