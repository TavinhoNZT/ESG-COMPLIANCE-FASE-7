using ESG.Compliance.Api.Domain;
using ESG.Compliance.Api.DTOs;
using ESG.Compliance.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ESG.Compliance.Api.Controllers
{
    [ApiController]
    [Route("api/auditorias")]
    public class AuditoriasController : ControllerBase
    {
        private readonly IAuditoriaService _auditoriaService;

        public AuditoriasController(IAuditoriaService auditoriaService)
        {
            _auditoriaService = auditoriaService;
        }

        // Endpoint para agendar uma nova auditoria
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AgendarAuditoriaDto agendarAuditoriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var novaAuditoria = new AuditoriaAmbiental
            {
                Titulo = agendarAuditoriaDto.Titulo,
                DataAgendada = agendarAuditoriaDto.DataAgendada,
                AuditorResponsavel = agendarAuditoriaDto.AuditorResponsavel,
                Observacoes = agendarAuditoriaDto.Observacoes,
                Status = "Agendada"
            };

            var auditoriaAgendada = await _auditoriaService.AgendarAuditoriaAsync(novaAuditoria);

            // Retornando um 201 Created sem a rota de "GetById" por simplicidade,
            // já que não criamos esse endpoint para auditorias.
            return StatusCode(201, auditoriaAgendada);
        }
    }
}
