using System;
using System.Collections.Generic;

namespace SKARB.Web.Models;

public partial class IncidentRole
{
    public int IncidentRoleId { get; set; }

    public string? IncidentRoleName { get; set; }

    public virtual ICollection<PersonIncident> PersonIncidents { get; set; } = new List<PersonIncident>();
}
