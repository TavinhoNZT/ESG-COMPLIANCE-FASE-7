using ESG.Compliance.Api.Domain;

namespace ESG.Compliance.Api.Services
{
    public interface IAuditoriaService
    {
        Task<AuditoriaAmbiental> AgendarAuditoriaAsync(AuditoriaAmbiental novaAuditoria);
    }
}