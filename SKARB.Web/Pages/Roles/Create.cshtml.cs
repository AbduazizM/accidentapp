using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SKARB.Web.Data;
using SKARB.Web.Models;

namespace SKARB.Web.Pages.Roles
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
            return Page();
        }

        [BindProperty]
        public IncidentRole IncidentRole { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.IncidentRoles.Add(IncidentRole);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
