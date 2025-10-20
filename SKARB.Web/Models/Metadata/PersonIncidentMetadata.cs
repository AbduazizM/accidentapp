using System.ComponentModel.DataAnnotations;

namespace SKARB.Web.Models.Metadata
{
    public class PersonIncidentMetadata
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Лицо")]
        public int? PersonId { get; set; }

        [Display(Name = "Происшествие")]
        public int? IncidentId { get; set; }

        [Display(Name = "Роль в происшествии")]
        public int? IncidentRoleId { get; set; }

        [Display(Name = "Происшествие")]
        public virtual Incident? Incident { get; set; }

        [Display(Name = "Роль в происшествии")]
        public virtual IncidentRole? IncidentRole { get; set; }

        [Display(Name = "Лицо")]
        public virtual Person? Person { get; set; }
    }
}
