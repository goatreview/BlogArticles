using ExposeCommand.AddGoat;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExposeCommand;

[ApiController]
[Route("[controller]")]
public class GoatsClassicController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddGoat([FromBody] AddGoatCommand command)
    {
        var result = await mediator.Send(command);
        return CreatedAtAction("AddGoat", new { id = result.Id }, result);
    }
}