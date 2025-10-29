using System.ComponentModel.DataAnnotations;

namespace ESG.Compliance.Api.DTOs
{
    public class CriarLicencaDto
    {
        [Required]
        [StringLength(200)]
        public string NomeDaLicenca { get; set; }

        [Required]
        [StringLength(150)]
        public string OrgaoEmissor { get; set; }

        public string NumeroDocumento { get; set; }

        [Required]
        public DateTime DataDeEmissao { get; set; }

        [Required]
        public DateTime DataDeExpiracao { get; set; }
    }
}
