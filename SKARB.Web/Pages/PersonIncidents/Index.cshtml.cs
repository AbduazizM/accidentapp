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
    public class IndexModel : PageModel
    {
        private readonly SKARB.Web.Data.SkarbDbContext _context;

        public IndexModel(SKARB.Web.Data.SkarbDbContext context)
        {
            _context = context;
        }

        public IList<PersonIncident> PersonIncident { get;set; } = default!;

        public async Task OnGetAsync()
        {
            PersonIncident = await _context.PersonIncidents
                .Include(p => p.Incident)
                .Include(p => p.IncidentRole)
                .Include(p => p.Person).ToListAsync();
        }
    }
}
