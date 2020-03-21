using System;
using System.Collections.Generic;
using MediatR;

namespace FakeNewsCovid.Domain.Command
{
    public class MarkAsFakeCommand : IRequest<bool>
    {
        public string UrlToMark { get; set; }

        public string InnerHtml { get; set; }

        public ICollection<string> FakeReasons { get; set; }
    }
}
