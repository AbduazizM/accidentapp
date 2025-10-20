using System.ComponentModel.DataAnnotations;

namespace SKARB.Web.Models.Metadata
{
    public class PersonMetadata
    {
        [Display(Name = "Лицо")]
        public int PersonId { get; set; }
        [Display(Name = "Фамилия")]
        public string? LastName { get; set; }
        [Display(Name = "Имя")]
        public string? FirstName { get; set; }
        [Display(Name = "Отчество")]
        public string? MiddleName { get; set; }
        [Display(Name = "Адрес")]
        public string? Address { get; set; }
        [Display(Name = "Количество дел")]
        public int? Convictoins { get; set; }
        [Display(Name = "Регистрационный номер")]
        public string? RegNumber { get; set; }
        public virtual ICollection<PersonIncident> PersonIncidents { get; set; } = new List<PersonIncident>();
    }
}
