using ESG.Compliance.Api.Domain;
using ESG.Compliance.Api.DTOs;
using ESG.Compliance.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ESG.Compliance.Api.Controllers
{
    [ApiController]
    [Route("api/licencas")]
    public class LicencasController : ControllerBase
    {
        private readonly ILicencaService _licencaService;

        public LicencasController(ILicencaService licencaService)
        {
            _licencaService = licencaService;
        }

        // ENDPOINT 1: Listagem com Pagina��o
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 10)
        {
            var licencas = await _licencaService.ObterTodasAsLicencasPaginadoAsync(pagina, tamanhoPagina);
            return Ok(licencas);
        }

        // ENDPOINT 2: L�gica de Neg�cio Avan�ada
        [HttpGet("alertas-expiracao")]
        public async Task<IActionResult> GetAlertasExpiracao([FromQuery] int dias = 90)
        {
            var licencasExpirando = await _licencaService.ObterAlertasDeExpiracaoAsync(dias);
            return Ok(licencasExpirando);
        }

        // ENDPOINT ADICIONAL: Obter por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var licenca = await _licencaService.ObterLicencaPorIdAsync(id);
            if (licenca == null)
            {
                return NotFound();
            }
            return Ok(licenca);
        }

        // ENDPOINT 3: Valida��o Avan�ada (via DTO)
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CriarLicencaDto criarLicencaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var novaLicenca = new LicencaAmbiental
            {
                NomeDaLicenca = criarLicencaDto.NomeDaLicenca,
                OrgaoEmissor = criarLicencaDto.OrgaoEmissor,
                NumeroDocumento = criarLicencaDto.NumeroDocumento,
                DataDeEmissao = criarLicencaDto.DataDeEmissao,
                DataDeExpiracao = criarLicencaDto.DataDeExpiracao,
                Status = "Ativa" // Status padr�o ao criar
            };

            var licencaCriada = await _licencaService.CriarLicencaAsync(novaLicenca);

            return CreatedAtAction(nameof(GetById), new { id = licencaCriada.Id }, licencaCriada);
        }

        // ENDPOINT 4: Endpoint Cr�tico com Seguran�a
        // [Authorize] -> Adicionaremos o atributo de seguran�a aqui no futuro
        [HttpPut("{id}/renovar")]
        public async Task<IActionResult> PutRenovar(int id)
        {
            var licencaAtualizada = new LicencaAmbiental
            {
                DataDeExpiracao = DateTime.Now.AddYears(1) // L�gica de exemplo: renova por 1 ano
            };

            var licencaRenovada = await _licencaService.RenovarLicencaAsync(id, licencaAtualizada);

            if (licencaRenovada == null)
            {
                return NotFound("Licen�a n�o encontrada para renova��o.");
            }

            return Ok(licencaRenovada);
        }
    }
}
