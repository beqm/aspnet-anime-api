using MediatR;
using Domain.Models;
using Api.Middleware;
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
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(Anime), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateAnimeCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.ID }, result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Anime), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(int id)
    {
        var anime = await _mediator.Send(new GetAnimeByIdQuery(id));
        if (anime == null) return NotFound();
        return Ok(anime);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Anime>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] string? title, [FromQuery] string? author)
    {
        var animes = await _mediator.Send(new GetAnimeListQuery(title, author));
        return Ok(animes);
    }


    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Anime), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateById(int id, [FromBody] UpdateAnimeCommand command)
    {
        var result = await _mediator.Send(command with { ID = id });
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteById([FromRoute] int id)
    {
        await _mediator.Send(new DeleteAnimeCommand(id));
        return NoContent();
    }
}