using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace ESG.Compliance.Api.Tests
{
    // PASSO 1: Criamos uma "f�brica" de aplica��o customizada para nossos testes
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Esta linha diz ao teste onde encontrar os arquivos da API, como o appsettings.json
            builder.UseSolutionRelativeContentRoot("ESG.Compliance.Api");

            // Substitui o DbContext por um provider InMemory para testes (sem necessidade de SQL Server)
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ESG.Compliance.Api.Data.ApplicationDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                services.AddDbContext<ESG.Compliance.Api.Data.ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("EsgComplianceDbTest");
                });
            });
        }
    }

    // PASSO 2: Modificamos a classe de teste para usar nossa nova "f�brica"
    public class LicencasControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public LicencasControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_Licencas_DeveRetornarStatusCode200()
        {
            // Arrange
            var request = "/api/licencas";

            // Act
            var response = await _client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode(); // Isso tamb�m verifica se o status � 2xx
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}