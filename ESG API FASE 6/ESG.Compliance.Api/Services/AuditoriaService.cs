using ESG.Compliance.Api.Data.Repositories;
using ESG.Compliance.Api.Domain;

namespace ESG.Compliance.Api.Services
{
    public class AuditoriaService : IAuditoriaService
    {
        private readonly IAuditoriaRepository _auditoriaRepository;

        public AuditoriaService(IAuditoriaRepository auditoriaRepository)
        {
            _auditoriaRepository = auditoriaRepository;
        }

        public async Task<AuditoriaAmbiental> AgendarAuditoriaAsync(AuditoriaAmbiental novaAuditoria)
        {
            if (novaAuditoria == null)
                throw new ArgumentNullException(nameof(novaAuditoria));

            await _auditoriaRepository.AddAsync(novaAuditoria);
            await _auditoriaRepository.SaveChangesAsync(); // Agora sim, estamos salvando.

            return novaAuditoria;
        }
    }
}