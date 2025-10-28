using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json; // Adicione este using para facilitar o trabalho com JSON
using ESG.Compliance.Api.DTOs; // Adicione este using para acessar nosso DTO

namespace ESG.Compliance.Api.Tests
{
    public class AuditoriasControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public AuditoriasControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Post_AgendarAuditoria_DeveRetornarStatusCode201()
        {
            // Arrange (Organizar)
            var request = "/api/auditorias";
            var novaAuditoriaDto = new AgendarAuditoriaDto
            {
                Titulo = "Auditoria Anual de Emissões",
                DataAgendada = DateTime.Now.AddDays(30),
                AuditorResponsavel = "Equipe de Auditores Internos"
            };

            // Act (Agir)
            var response = await _client.PostAsJsonAsync(request, novaAuditoriaDto);

            // Assert (Verificar)
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
