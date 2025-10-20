using Microsoft.AspNetCore.Mvc;
using SKARB.Web.Models.Metadata;

namespace SKARB.Web.Models;

[ModelMetadataType(typeof(PersonMetadata))]
public partial class Person
{
    public string FullName
    {
        get
        {
            var parts = new List<string>
            {
                LastName?.Trim(),
                FirstName?.Trim(),
                MiddleName?.Trim()
            };

            return string.Join(" ", parts.Where(part => !string.IsNullOrEmpty(part)));
        }
    }
}