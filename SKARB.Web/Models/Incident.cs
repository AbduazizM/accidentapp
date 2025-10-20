using System;
using System.Collections.Generic;

namespace SKARB.Web.Models;

public partial class Incident
{
    public int IncidentId { get; set; }

    public DateTime? IncidentDate { get; set; }

    public string? Description { get; set; }

    public int? DecisionId { get; set; }

    public string? RegNumber { get; set; }

    public virtual Decision? Decision { get; set; }

    public virtual ICollection<PersonIncident> PersonIncidents { get; set; } = new List<PersonIncident>();
}
