using MediatR;
using Domain.Models;
using Api.Middleware;
using Microsoft.AspNetCore.Mvc;

using Application.Commands.Anime.CreateAnime;
using Application.Commands.Anime.UpdateAnime;
using Application.Commands.Anime.DeleteAnime;
using Application.Queries.Anime.GetAnimeById;
using Application.Queries.Anime.GetAnimeList;

namespace Api.Controllers;


/// <summary>
/// Controlador responsável pelas operações relacionadas a animes.
/// </summary>
/// <remarks>
/// Fornece endpoints para criar, consultar, atualizar e remover animes do sistema.
/// </remarks>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AnimeController : ControllerBase
{
    private readonly IMediator _mediator;


    /// <summary>
    /// Construtor do controlador de animes.
    /// </summary>
    /// <param name="mediator">Instância do mediator para envio de comandos e queries (CQRS)</param>
    public AnimeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Cria um novo anime.
    /// </summary>
    /// <remarks>
    /// Envia um comando contendo os dados do anime para ser persistido no sistema.
    /// Retorna o anime criado.
    /// </remarks>
    /// <param name="command">Objeto contendo titulo, descrição e autor.</param>
    /// <response code="201">Anime criado com sucesso.</response>
    /// <response code="409">Conflito ao criar o anime. Pode ocorrer se já existir um anime com o mesmo titulo.</response>
    /// <response code="500">Erro interno no servidor.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Anime), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IResult> Create([FromBody] CreateAnimeCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToIResult(StatusCodes.Status201Created);
    }

    /// <summary>
    /// Requisita um anime pelo ID.
    /// </summary>
    /// <remarks>
    /// Busca no sistema um anime específico a partir do seu id.
    /// Retorna os dados completos do anime se encontrado.
    /// </remarks>
    /// <param name="id">ID do anime a ser buscado.</param>
    /// <response code="200">Anime encontrado e retornado com sucesso.</response>
    /// <response code="404">Nenhum anime foi encontrado para o ID informado.</response>
    /// <response code="500">Erro interno no servidor.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Anime), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetAnimeByIdQuery(id));
        Console.WriteLine(result.IsSuccess);
        return result.ToIResult(StatusCodes.Status200OK);
    }

    /// <summary>
    /// Requisita uma Lista de animes cadastrados.
    /// </summary>
    /// <remarks>
    /// Retorna uma lista de animes cadastrados no sistema.
    /// Pode ser filtrada por título e autor usando parâmetros de query.
    /// </remarks>
    /// <param name="title">Filtrar pelo título do anime (opcional).</param>
    /// <param name="author">Filtrar pelo autor do anime (opcional).</param>
    /// <response code="200">Lista de animes retornada com sucesso.</response>
    /// <response code="500">Erro interno no servidor.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Anime>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IResult> Get([FromQuery] string? title, [FromQuery] string? author)
    {
        var result = await _mediator.Send(new GetAnimeListQuery(title, author));
        return result.ToIResult(StatusCodes.Status200OK);
    }

    /// <summary>
    /// Atualiza um anime pelo ID.
    /// </summary>
    /// <remarks>
    /// Atualiza as informações de um anime existente com base no ID fornecido.
    /// Retorna o anime atualizado.
    /// </remarks>
    /// <param name="id">ID do anime a ser atualizado.</param>
    /// <param name="command">Objeto contendo os novos dados do anime.</param>
    /// <response code="200">Anime atualizado com sucesso.</response>
    /// <response code="404">Anime não encontrado.</response>
    /// <response code="409">Conflito na atualização (ex.: dados duplicados).</response>
    /// <response code="500">Erro interno no servidor.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Anime), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IResult> UpdateById(int id, [FromBody] UpdateAnimeCommand command)
    {
        var result = await _mediator.Send(command with { ID = id });
        return result.ToIResult(StatusCodes.Status200OK);
    }

    /// <summary>
    /// Remove um anime pelo ID.
    /// </summary>
    /// <remarks>
    /// Exclui o anime com o ID informado.
    /// </remarks>
    /// <param name="id">ID do anime a ser removido.</param>
    /// <response code="204">Anime removido com sucesso.</response>
    /// <response code="404">Anime não encontrado.</response>
    /// <response code="500">Erro interno no servidor.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IResult> DeleteById([FromRoute] int id)
    {
        var result = await _mediator.Send(new DeleteAnimeCommand(id));
        return result.ToIResult(StatusCodes.Status204NoContent);
    }
}

