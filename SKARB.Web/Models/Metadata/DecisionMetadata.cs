using System.ComponentModel.DataAnnotations;

namespace SKARB.Web.Models.Metadata
{
    public class DecisionMetadata
    {
        [Display(Name = "Решение")]
        public int DecisionId { get; set; }

        [Display(Name = "Решение")]
        public string? DecisionName { get; set; }

        [Display(Name = "Происшествия")]
        public virtual ICollection<Incident> Incidents { get; set; } = new List<Incident>();
    }
}
