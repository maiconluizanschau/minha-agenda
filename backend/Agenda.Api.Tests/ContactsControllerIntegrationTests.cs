using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Agenda.Api.Application.Dtos;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Agenda.Api.Tests
{
    public class ContactsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ContactsControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder => { });
        }

        [Fact]
        public async Task Patch_Favorite_Should_Toggle_Favorito_And_Return_Ok()
        {
            // Arrange
            var client = _factory.CreateClient();

            // 1) Autentica para obter JWT
            var loginResponse = await client.PostAsJsonAsync("/api/auth/login", new LoginRequestDto
            {
                Username = "admin",
                Password = "P@ssw0rd"
            });

            loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var loginContent = await loginResponse.Content.ReadFromJsonAsync<LoginResponseDto>();
            loginContent.Should().NotBeNull();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginContent!.Token);

            // 2) Cria um contato
            var createDto = new CreateContactDto
            {
                Nome = "Contato Integração",
                Email = $"int_{Guid.NewGuid():N}@teste.com",
                Telefone = "11999999999"
            };

            var createResponse = await client.PostAsJsonAsync("/api/contacts", createDto);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var createdContact = await createResponse.Content.ReadFromJsonAsync<ContactDto>();
            createdContact.Should().NotBeNull();
            createdContact!.Favorito.Should().BeFalse();

            // 3) Chama PATCH /favorite
            var patchResponse = await client.PatchAsync($"/api/contacts/{createdContact.Id}/favorite", null);
            patchResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedContact = await patchResponse.Content.ReadFromJsonAsync<ContactDto>();
            updatedContact.Should().NotBeNull();
            updatedContact!.Favorito.Should().BeTrue();
        }
    }
}
