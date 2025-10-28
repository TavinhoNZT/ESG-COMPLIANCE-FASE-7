using ESG.Compliance.Api.Domain;

namespace ESG.Compliance.Api.Data.Repositories
{
    public interface ILicencaRepository
    {
        Task<IEnumerable<LicencaAmbiental>> GetAllAsync(int pagina, int tamanhoPagina);
        Task<LicencaAmbiental> GetByIdAsync(int id);
        Task<IEnumerable<LicencaAmbiental>> GetLicencasExpirandoAsync(int dias);
        Task AddAsync(LicencaAmbiental licenca);
        void Update(LicencaAmbiental licenca);
        void Delete(LicencaAmbiental licenca);
        Task<int> SaveChangesAsync();
    }
}
