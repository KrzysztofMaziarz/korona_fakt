using System;
using FakeNewsCovid.Domain.QueryResult;
using MediatR;

namespace FakeNewsCovid.Domain.Query
{
    public class IsFakeNewsQuery : IRequest<IsFakeNewsQueryResult>
    {
        public Uri UrlAddress { get; set; }
    }
}
