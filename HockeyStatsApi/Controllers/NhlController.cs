using Domain.Interfaces;
using HockeyStatsApp.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class NhlStatsController : ControllerBase
{
    private readonly INhlClient _nhlClient;
    private readonly IMediator _mediator;

    public NhlStatsController(INhlClient nhlClient, IMediator mediator)
    {
        _nhlClient = nhlClient;
        _mediator = mediator;
    }

    [HttpGet("team-stats")]
    public async Task<ActionResult> SolveLitcoPlayoffPool()
    {
        var query = new SolveLitcoPlayoffPoolCommand();

        var resp = await _mediator.Send(query);
        if (!resp.Success)
        {
            return StatusCode(500, resp.ErrorMessage);
        }   

        return Ok(resp.Value);
    }
}
