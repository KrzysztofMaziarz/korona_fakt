using System;
using FakeNewsCovid.Domain.QueryResult;
using MediatR;

namespace FakeNewsCovid.Domain.Query
{
    public class FakebilityQuery : IRequest<FakebilityQueryResult>
    {
        public string UrlAddress { get; set; }

        public string InnerHtml { get; set; }
    }
}
