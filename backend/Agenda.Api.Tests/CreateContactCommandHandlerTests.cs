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
using MediatR;
using FluentAssertions;
using Moq;
using Xunit;

namespace Agenda.Api.Tests
{
    public class CreateContactCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IContactRepository> _repositoryMock;
        private readonly Mock<IMediator> _mediatorMock;

        public CreateContactCommandHandlerTests()
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
        public async Task Handle_Should_Create_New_Contact_When_Email_Not_Exists()
        {
            // Arrange
            var dto = new CreateContactDto
            {
                Nome = "Teste",
                Email = "teste@teste.com",
                Telefone = "11999999999"
            };

            _repositoryMock.Setup(r => r.GetByEmailAsync(dto.Email))
                .ReturnsAsync((Contact?)null);

            var handler = new CreateContactCommandHandler(_repositoryMock.Object, _mapper, _mediatorMock.Object);

            // Act
            var result = await handler.Handle(new CreateContactCommand(dto), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Nome.Should().Be(dto.Nome);
            result.Email.Should().Be(dto.Email);

            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Contact>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Email_Already_Exists()
        {
            // Arrange
            var dto = new CreateContactDto
            {
                Nome = "Duplicado",
                Email = "dup@teste.com",
                Telefone = "11999999999"
            };

            _repositoryMock.Setup(r => r.GetByEmailAsync(dto.Email))
                .ReturnsAsync(new Contact { Email = dto.Email, Ativo = true });

            var handler = new CreateContactCommandHandler(_repositoryMock.Object, _mapper, _mediatorMock.Object);

            // Act
            var act = async () => await handler.Handle(new CreateContactCommand(dto), CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Já existe um contato ativo com esse e-mail.");

            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Contact>()), Times.Never);
        }
    }
}
