using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SKARB.Web.Data;
using SKARB.Web.Models;

namespace SKARB.Web.Pages.PersonIncidents
{
    public class DetailsModel : PageModel
    {
        private readonly SKARB.Web.Data.SkarbDbContext _context;

        public DetailsModel(SKARB.Web.Data.SkarbDbContext context)
        {
            _context = context;
        }

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
    }
}
