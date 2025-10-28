using ESG.Compliance.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace ESG.Compliance.Api.Data.Repositories
{
    public class LicencaRepository : ILicencaRepository
    {
        private readonly ApplicationDbContext _context;

        public LicencaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LicencaAmbiental>> GetAllAsync(int pagina, int tamanhoPagina)
        {
            return await _context.LicencasAmbientais
                                 .Skip((pagina - 1) * tamanhoPagina)
                                 .Take(tamanhoPagina)
                                 .ToListAsync();
        }

        public async Task<LicencaAmbiental> GetByIdAsync(int id)
        {
            return await _context.LicencasAmbientais.FindAsync(id);
        }

        public async Task<IEnumerable<LicencaAmbiental>> GetLicencasExpirandoAsync(int dias)
        {
            var dataLimite = DateTime.Now.AddDays(dias);
            return await _context.LicencasAmbientais
                                 .Where(l => l.DataDeExpiracao <= dataLimite && l.Status == "Ativa")
                                 .ToListAsync();
        }

        public async Task AddAsync(LicencaAmbiental licenca)
        {
            await _context.LicencasAmbientais.AddAsync(licenca);
        }

        public void Update(LicencaAmbiental licenca)
        {
            _context.LicencasAmbientais.Update(licenca);
        }

        public void Delete(LicencaAmbiental licenca)
        {
            _context.LicencasAmbientais.Remove(licenca);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
