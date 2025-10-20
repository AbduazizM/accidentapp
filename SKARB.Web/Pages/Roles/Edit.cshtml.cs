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

namespace SKARB.Web.Pages.Roles
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
        public IncidentRole IncidentRole { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incidentrole =  await _context.IncidentRoles.FirstOrDefaultAsync(m => m.IncidentRoleId == id);
            if (incidentrole == null)
            {
                return NotFound();
            }
            IncidentRole = incidentrole;
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

            _context.Attach(IncidentRole).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IncidentRoleExists(IncidentRole.IncidentRoleId))
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

        private bool IncidentRoleExists(int id)
        {
            return _context.IncidentRoles.Any(e => e.IncidentRoleId == id);
        }
    }
}
