using System;
using System.Collections.Generic;

namespace SKARB.Web.Models;

public partial class Decision
{
    public int DecisionId { get; set; }

    public string? DecisionName { get; set; }

    public virtual ICollection<Incident> Incidents { get; set; } = new List<Incident>();
}
