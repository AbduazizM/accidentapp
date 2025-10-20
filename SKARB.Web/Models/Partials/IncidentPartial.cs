using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using SKARB.Web.Models.Metadata;

namespace SKARB.Web.Models;

[ModelMetadataType(typeof(IncidentMetadata))]
public partial class Incident
{
}