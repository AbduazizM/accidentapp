using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SKARB.Web.Data;
using SKARB.Web.Models;

namespace SKARB.Web.Pages.Roles
{
    [Authorize(Policy = "CanEditOrDelete")]
    public class DeleteModel : PageModel
    {
        private readonly SKARB.Web.Data.SkarbDbContext _context;

        public DeleteModel(SKARB.Web.Data.SkarbDbContext context)
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

            var incidentrole = await _context.IncidentRoles.FirstOrDefaultAsync(m => m.IncidentRoleId == id);

            if (incidentrole is not null)
            {
                IncidentRole = incidentrole;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incidentrole = await _context.IncidentRoles.FindAsync(id);
            if (incidentrole != null)
            {
                IncidentRole = incidentrole;
                _context.IncidentRoles.Remove(IncidentRole);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
