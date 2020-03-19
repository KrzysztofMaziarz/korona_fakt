using System.Threading.Tasks;
using FakeNewsCovid.Domain.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FakeNewsCovid.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FakeNewsController : ControllerBase
    {
        private readonly IMediator mediator;

        public FakeNewsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> IsFakeNews([FromBody] IsFakeNewsQuery query)
        {
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}
