using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SKARB.Web.Data;
using SKARB.Web.Models;

namespace SKARB.Web.Pages.PersonIncidents
{
    [Authorize(Policy = "CanEditOrDelete")]
    public class EditModel : PageModel
    {
        private readonly SKARB.Web.Data.SkarbDbContext _context;

        public EditModel(SKARB.Web.Data.SkarbDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PersonIncident PersonIncident { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personincident =  await _context.PersonIncidents.FirstOrDefaultAsync(m => m.Id == id);
            if (personincident == null)
            {
                return NotFound();
            }
            PersonIncident = personincident;
           ViewData["IncidentId"] = new SelectList(_context.Incidents, "IncidentId", "IncidentId");
           ViewData["IncidentRoleId"] = new SelectList(_context.IncidentRoles, "IncidentRoleId", "IncidentRoleId");
           ViewData["PersonId"] = new SelectList(_context.People, "PersonId", "PersonId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(PersonIncident).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonIncidentExists(PersonIncident.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PersonIncidentExists(int id)
        {
            return _context.PersonIncidents.Any(e => e.Id == id);
        }
    }
}
