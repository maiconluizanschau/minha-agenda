using System;
using System.Threading;
using System.Threading.Tasks;
using Agenda.Api.Application.Cqrs.Contacts.Commands;
using Agenda.Api.Application.Cqrs.Contacts.Handlers;
using Agenda.Api.Application.Dtos;
using Agenda.Api.Application.Mapping;
using Agenda.Api.Domain.Entities;
using Agenda.Api.Domain.Interfaces;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Agenda.Api.Tests
{
    public class ToggleFavoriteContactCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IContactRepository> _repositoryMock;
        private readonly Mock<IMediator> _mediatorMock;

        public ToggleFavoriteContactCommandHandlerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            _mapper = config.CreateMapper();
            _repositoryMock = new Mock<IContactRepository>();
            _mediatorMock = new Mock<IMediator>();
        }

        [Fact]
        public async Task Handle_Should_Toggle_Favorite_From_False_To_True()
        {
            // Arrange
            var id = Guid.NewGuid();
            var contact = new Contact
            {
                Id = id,
                Nome = "Contato Teste",
                Email = "teste@teste.com",
                Telefone = "11999999999",
                Ativo = true,
                Favorito = false
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(contact);

            var handler = new ToggleFavoriteContactCommandHandler(
                _repositoryMock.Object,
                _mapper,
                _mediatorMock.Object);

            // Act
            var result = await handler.Handle(new ToggleFavoriteContactCommand(id), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.Favorito.Should().BeTrue();

            _repositoryMock.Verify(r => r.UpdateAsync(It.Is<Contact>(c => c.Favorito)), Times.Once);
            _mediatorMock.Verify(m => m.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Null_When_Contact_Not_Found()
        {
            // Arrange
            var id = Guid.NewGuid();

            _repositoryMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync((Contact?)null);

            var handler = new ToggleFavoriteContactCommandHandler(
                _repositoryMock.Object,
                _mapper,
                _mediatorMock.Object);

            // Act
            var result = await handler.Handle(new ToggleFavoriteContactCommand(id), CancellationToken.None);

            // Assert
            result.Should().BeNull();
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Contact>()), Times.Never);
            _mediatorMock.Verify(m => m.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
