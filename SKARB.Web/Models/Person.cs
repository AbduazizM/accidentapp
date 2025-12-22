using System;
using System.Collections.Generic;

namespace SKARB.Web.Models;

public partial class Person
{
    public int PersonId { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? Address { get; set; }

    public int? Convictoins { get; set; } = -1;

    public string? RegNumber { get; set; }

    public virtual ICollection<PersonIncident> PersonIncidents { get; set; } = new List<PersonIncident>();
}
