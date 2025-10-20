using System.ComponentModel.DataAnnotations;

namespace SKARB.Web.Models.Metadata
{
    public class IncidentRoleMetadata
    {
        [Display(Name = "Роль в происшествии")]
        public int IncidentRoleId { get; set; }
        
        [Display(Name = "Роль в происшествии")]
        public string? IncidentRoleName { get; set; }

        public virtual ICollection<PersonIncident> PersonIncidents { get; set; } = new List<PersonIncident>();
    }
}
