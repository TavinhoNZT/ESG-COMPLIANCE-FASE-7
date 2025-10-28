using System.ComponentModel.DataAnnotations;

namespace ESG.Compliance.Api.DTOs
{
    public class AgendarAuditoriaDto
    {
        [Required]
        [StringLength(250)]
        public string Titulo { get; set; }

        [Required]
        public DateTime DataAgendada { get; set; }

        [Required]
        [StringLength(150)]
        public string AuditorResponsavel { get; set; }

        [StringLength(1000)]
        public string? Observacoes { get; set; }
    }
}
