using System.Threading.Tasks;
using FakeNewsCovid.Domain.Command;
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

        [HttpPost("[action]")]
        public async Task<IActionResult> MarkAsFake([FromBody] MarkAsFakeCommand command)
        {
            bool isMarked = await mediator.Send(command);
            return Ok(isMarked);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GetFakebility([FromBody] FakebilityQuery query)
        {
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}
