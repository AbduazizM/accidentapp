using System;
using System.Collections.Generic;

namespace SKARB.Web.Models;

public partial class PersonIncident
{
    public int Id { get; set; }

    public int? PersonId { get; set; }

    public int? IncidentId { get; set; }

    public int? IncidentRoleId { get; set; }

    public virtual Incident? Incident { get; set; }

    public virtual IncidentRole? IncidentRole { get; set; }

    public virtual Person? Person { get; set; }
}
