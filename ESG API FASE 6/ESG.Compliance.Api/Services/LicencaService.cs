using ESG.Compliance.Api.Data.Repositories;
using ESG.Compliance.Api.Domain;

namespace ESG.Compliance.Api.Services
{
    public class LicencaService : ILicencaService
    {
        private readonly ILicencaRepository _licencaRepository;

        public LicencaService(ILicencaRepository licencaRepository)
        {
            _licencaRepository = licencaRepository;
        }

        public async Task<LicencaAmbiental> CriarLicencaAsync(LicencaAmbiental novaLicenca)
        {
            // Aqui poderíamos ter lógicas de negócio, como validar campos, etc.
            await _licencaRepository.AddAsync(novaLicenca);
            await _licencaRepository.SaveChangesAsync();
            return novaLicenca;
        }

        public async Task<IEnumerable<LicencaAmbiental>> ObterAlertasDeExpiracaoAsync(int dias)
        {
            return await _licencaRepository.GetLicencasExpirandoAsync(dias);
        }

        public async Task<LicencaAmbiental> ObterLicencaPorIdAsync(int id)
        {
            return await _licencaRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<LicencaAmbiental>> ObterTodasAsLicencasPaginadoAsync(int pagina, int tamanhoPagina)
        {
            return await _licencaRepository.GetAllAsync(pagina, tamanhoPagina);
        }

        public async Task<LicencaAmbiental> RenovarLicencaAsync(int id, LicencaAmbiental licencaAtualizada)
        {
            var licencaExistente = await _licencaRepository.GetByIdAsync(id);
            if (licencaExistente == null)
            {
                return null; // ou lançar uma exceção
            }

            // Atualiza as propriedades
            licencaExistente.DataDeExpiracao = licencaAtualizada.DataDeExpiracao;
            licencaExistente.Status = "Ativa";
            //... outras propriedades que podem ser atualizadas

            _licencaRepository.Update(licencaExistente);
            await _licencaRepository.SaveChangesAsync();

            return licencaExistente;
        }
    }
}