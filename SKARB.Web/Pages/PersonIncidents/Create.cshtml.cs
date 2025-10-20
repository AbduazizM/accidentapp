using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SKARB.Web.Data;
using SKARB.Web.Models;

namespace SKARB.Web.Pages.PersonIncidents
{
    public class CreateModel : PageModel
    {
        private readonly SKARB.Web.Data.SkarbDbContext _context;

        public CreateModel(SKARB.Web.Data.SkarbDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["IncidentId"] = new SelectList(_context.Incidents, "IncidentId", "RegNumber");
        ViewData["IncidentRoleId"] = new SelectList(_context.IncidentRoles, "IncidentRoleId", "IncidentRoleName");
        ViewData["PersonId"] = new SelectList(_context.People, "PersonId", "FullName");
            return Page();
        }

        [BindProperty]
        public PersonIncident PersonIncident { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.PersonIncidents.Add(PersonIncident);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
