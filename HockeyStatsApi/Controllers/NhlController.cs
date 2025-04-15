using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces;
using MediatR;
using HockeyStatsApp.Queries;

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
    public async Task<ActionResult> GetTeamStats()
    {
        //var query = new GetHockeyStatsQuery();

        //var resp = await _mediator.Send(query);
        //if (!resp.Success)
        //{
        //    return StatusCode(500, resp.ErrorMessage);
        //}
        //return Ok(resp.Value);

        var query = new GetHockeyStatsQuery();
        var resp = await _mediator.Send(query);

        if (!resp.Success)
        {
            return StatusCode(500, resp.ErrorMessage);
        }   

        return Ok(resp.Value); // return raw JSON for debugging
    }
}
