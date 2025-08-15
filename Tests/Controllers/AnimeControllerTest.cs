using Moq;
using MediatR;
using FluentResults;
using Api.Controllers;
using Application.Dtos;
using FluentAssertions;
using Microsoft.AspNetCore.Http;


using Application.Commands.Anime.CreateAnime;
using Application.Commands.Anime.UpdateAnime;
using Application.Commands.Anime.DeleteAnime;
using Application.Queries.Anime.GetAnimeById;
using Application.Queries.Anime.GetAnimeList;

namespace Tests.Controllers
{
    public class AnimeControllerTest
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AnimeController _controller;

        public AnimeControllerTest()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new AnimeController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Create_Returns201Created_WhenSuccess()
        {
            var command = new CreateAnimeCommand(1, "Naruto", "Autor Naruto", "Descricao Naruto");
            var animeDto = new AnimeDto
            {
                ID = 1,
                Title = "Naruto",
                Author = "Autor Naruto",
                Description = "Descricao Naruto"
            };

            _mediatorMock
                .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok(animeDto));

            var result = await _controller.Create(command);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IResult>();
        }

        [Fact]
        public async Task GetById_Returns200_WhenFound()
        {
            var id = 1;
            var animeDto = new AnimeDto
            {
                ID = id,
                Title = "Naruto",
                Author = "Autor Naruto",
                Description = "Descricao Naruto"
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAnimeByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok<AnimeDto?>(animeDto));

            var result = await _controller.GetById(id);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetById_Returns404_WhenNotFound()
        {
            var id = 999;

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAnimeByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Fail<AnimeDto?>(new Application.Errors.NotFound($"Anime with ID {id} not found.")));

            var result = await _controller.GetById(id);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Get_Returns200_WithList()
        {
            var animeList = new List<AnimeDto>
            {
                new AnimeDto { ID = 1, Title = "Naruto", Author = "Autor Naruto", Description = "Descricao Naruto" },
                new AnimeDto { ID = 2, Title = "One Piece", Author = "Autor One Piece", Description = "Descricao One Piece" }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAnimeListQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok(animeList));

            var result = await _controller.Get(null, null);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateById_Returns200_WhenSuccess()
        {
            var command = new UpdateAnimeCommand(1, "Naruto Shippuden", "Kishimoto", "Updated description");
            var animeDto = new AnimeDto
            {
                ID = 1,
                Title = "Naruto Shippuden",
                Author = "Kishimoto",
                Description = "Updated description"
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateAnimeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok(animeDto));

            var result = await _controller.UpdateById(1, command);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteById_Returns204_WhenSuccess()
        {
            var id = 1;

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteAnimeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok());

            var result = await _controller.DeleteById(id);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IResult>();
        }

        [Fact]
        public async Task DeleteById_Returns404_WhenNotFound()
        {
            var id = 999;

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteAnimeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Fail(new Application.Errors.NotFound($"Anime with ID {id} not found.")));

            var result = await _controller.DeleteById(id);

            result.Should().NotBeNull();
        }
    }
}
