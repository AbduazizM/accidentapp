using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SKARB.Web.Data;
using SKARB.Web.Models;

namespace SKARB.Web.Pages.Roles
{
    public class IndexModel : PageModel
    {
        private readonly SKARB.Web.Data.SkarbDbContext _context;

        public IndexModel(SKARB.Web.Data.SkarbDbContext context)
        {
            _context = context;
        }

        public IList<IncidentRole> IncidentRole { get;set; } = default!;

        public async Task OnGetAsync()
        {
            IncidentRole = await _context.IncidentRoles.ToListAsync();
        }
    }
}
