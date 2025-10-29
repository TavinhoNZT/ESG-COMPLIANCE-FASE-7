using ESG.Compliance.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace ESG.Compliance.Api.Data.Repositories
{
    public class AuditoriaRepository : IAuditoriaRepository
    {
        private readonly ApplicationDbContext _context;

        public AuditoriaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AuditoriaAmbiental auditoria)
        {
            await _context.AuditoriasAmbientais.AddAsync(auditoria);
        }

        // Precisamos também implementar os outros métodos da interface
        // mesmo que o teste não os use diretamente agora.
        public async Task<IEnumerable<AuditoriaAmbiental>> GetAllAsync()
        {
            return await _context.AuditoriasAmbientais.ToListAsync();
        }

        public async Task<AuditoriaAmbiental> GetByIdAsync(int id)
        {
            return await _context.AuditoriasAmbientais.FindAsync(id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}