namespace ESG.Compliance.Api.Domain
{
    public class AuditoriaAmbiental
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime DataAgendada { get; set; }
        public string AuditorResponsavel { get; set; }
        public string Status { get; set; } // Ex: "Agendada", "Em Andamento", "Concluída"
        public string? Observacoes { get; set; }
    }
}