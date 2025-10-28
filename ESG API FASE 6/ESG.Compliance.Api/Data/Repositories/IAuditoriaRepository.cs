using ESG.Compliance.Api.Domain;

namespace ESG.Compliance.Api.Data.Repositories
{
    public interface IAuditoriaRepository
    {
        Task<IEnumerable<AuditoriaAmbiental>> GetAllAsync();
        Task<AuditoriaAmbiental> GetByIdAsync(int id);
        Task AddAsync(AuditoriaAmbiental auditoria);
        Task<int> SaveChangesAsync();
    }
}
