using MediatR;
using Microsoft.AspNetCore.Mvc;

using Application.Commands.Anime.CreateAnime;
using Application.Queries.Anime.GetAnimeById;
using Application.Commands.Anime.UpdateAnime;
using Application.Commands.Anime.DeleteAnime;
using Application.Queries.Anime.GetAnimeList;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/anime")]
public class AnimeController : ControllerBase
{
    private readonly IMediator _mediator;

    public AnimeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAnimeCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.ID }, result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var anime = await _mediator.Send(new GetAnimeByIdQuery(id));
        if (anime == null) return NotFound();
        return Ok(anime);
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string? title, [FromQuery] string? author)
    {
        var animes = await _mediator.Send(new GetAnimeListQuery(title, author));
        return Ok(animes);
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateById(int id, [FromBody] UpdateAnimeCommand command)
    {
        var result = await _mediator.Send(command with { ID = id });
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteById([FromRoute] int id)
    {
        await _mediator.Send(new DeleteAnimeCommand(id));
        return NoContent();
    }
}