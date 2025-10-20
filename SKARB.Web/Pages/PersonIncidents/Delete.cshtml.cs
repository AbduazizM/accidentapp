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

namespace SKARB.Web.Pages.PersonIncidents
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
        public PersonIncident PersonIncident { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personincident = await _context.PersonIncidents.FirstOrDefaultAsync(m => m.Id == id);

            if (personincident is not null)
            {
                PersonIncident = personincident;

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

            var personincident = await _context.PersonIncidents.FindAsync(id);
            if (personincident != null)
            {
                PersonIncident = personincident;
                _context.PersonIncidents.Remove(PersonIncident);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
