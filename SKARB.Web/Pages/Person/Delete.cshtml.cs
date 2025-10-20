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

namespace SKARB.Web.Pages.Person
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
        public SKARB.Web.Models.Person Person { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People.FirstOrDefaultAsync(m => m.PersonId == id);

            if (person is not null)
            {
                Person = person;

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

            var person = await _context.People.FindAsync(id);
            if (person != null)
            {
                Person = person;
                _context.People.Remove(Person);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
