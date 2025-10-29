namespace ESG.Compliance.Api.Domain
{
    public class LicencaAmbiental
    {
        public int Id { get; set; }
        public string NomeDaLicenca { get; set; }
        public string OrgaoEmissor { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime DataDeEmissao { get; set; }
        public DateTime DataDeExpiracao { get; set; }
        public string Status { get; set; } // Ex: "Ativa", "Expirada", "Em Renovação"
    }
}
