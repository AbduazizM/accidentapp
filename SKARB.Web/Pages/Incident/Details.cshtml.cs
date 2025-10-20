using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SKARB.Web.Data;
using SKARB.Web.Models;

namespace SKARB.Web.Pages.Incident
{
    public class DetailsModel : PageModel
    {
        private readonly SKARB.Web.Data.SkarbDbContext _context;

        public DetailsModel(SKARB.Web.Data.SkarbDbContext context)
        {
            _context = context;
        }

        public SKARB.Web.Models.Incident Incident { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incident = await _context.Incidents.Include(p => p.Decision).FirstOrDefaultAsync(m => m.IncidentId == id);

            if (incident is not null)
            {
                Incident = incident;

                return Page();
            }

            return NotFound();
        }
    }
}
