using System.ComponentModel.DataAnnotations;

namespace SKARB.Web.Models.Metadata
{
    public class IncidentMetadata
    {
        [Display(Name = "Происшествие")]
        public int IncidentId { get; set; }

        [Display(Name = "Дата происшествия")]
        public DateTime? IncidentDate { get; set; }

        [Display(Name = "Описание происшествия")]
        public string? Description { get; set; }

        [Display(Name = "Решение")]
        public int? DecisionId { get; set; }

        [Display(Name = "Решение")]
        public virtual Decision? Decision { get; set; }
        [Display(Name = "Регистрационный номер")]
        public string? RegNumber { get; set; }
        public virtual ICollection<PersonIncident> PersonIncidents { get; set; } = new List<PersonIncident>();
    }
}
