using ESG.Compliance.Api.Domain;

namespace ESG.Compliance.Api.Services
{
    public interface ILicencaService
    {
        Task<IEnumerable<LicencaAmbiental>> ObterTodasAsLicencasPaginadoAsync(int pagina, int tamanhoPagina);
        Task<LicencaAmbiental> ObterLicencaPorIdAsync(int id);
        Task<IEnumerable<LicencaAmbiental>> ObterAlertasDeExpiracaoAsync(int dias);
        Task<LicencaAmbiental> CriarLicencaAsync(LicencaAmbiental novaLicenca);
        Task<LicencaAmbiental> RenovarLicencaAsync(int id, LicencaAmbiental licencaAtualizada);
    }
}