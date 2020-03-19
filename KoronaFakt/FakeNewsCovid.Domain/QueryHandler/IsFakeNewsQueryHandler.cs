using System;
using System.Threading;
using System.Threading.Tasks;
using FakeNewsCovid.Domain.Query;
using FakeNewsCovid.Domain.QueryResult;
using MediatR;

namespace FakeNewsCovid.Domain.QueryHandler
{
    public class IsFakeNewsQueryHandler : IRequestHandler<IsFakeNewsQuery, IsFakeNewsQueryResult>
    {
        public IsFakeNewsQueryHandler()
        {

        }

        public Task<IsFakeNewsQueryResult> Handle(IsFakeNewsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
