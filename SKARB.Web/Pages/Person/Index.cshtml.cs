using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SKARB.Web.Data;
using SKARB.Web.Models;

namespace SKARB.Web.Pages.Person
{
    public class IndexModel : PageModel
    {
        private readonly SKARB.Web.Data.SkarbDbContext _context;

        public IndexModel(SKARB.Web.Data.SkarbDbContext context)
        {
            _context = context;
        }

        public IList<SKARB.Web.Models.Person> Person { get; set; } = default!;

        public string? SearchLastName { get; set; }
        public string? SearchFirstName { get; set; }
        public string? SearchMiddleName { get; set; }
        public string? SearchFio { get; set; }
        public string? SearchAddress { get; set; }
        public int? MinConvictions { get; set; }
        public int? MaxConvictions { get; set; }

        public async Task OnGetAsync(
    string? searchFio,
    string? searchAddress,
    int? minConvictions,
    int? maxConvictions)
        {
            SearchFio = searchFio;
            SearchAddress = searchAddress;
            MinConvictions = minConvictions;
            MaxConvictions = maxConvictions;

            IQueryable<SKARB.Web.Models.Person> query = _context.People;

            if (!string.IsNullOrWhiteSpace(searchFio))
            {
                string searchTerm = searchFio.Trim().ToLower();
                query = query.Where(p =>
                    (p.FirstName != null && p.FirstName.ToLower().Contains(searchTerm)) ||
                    (p.MiddleName != null && p.MiddleName.ToLower().Contains(searchTerm)) ||
                    (p.LastName != null && p.LastName.ToLower().Contains(searchTerm)));
            }

            if (!string.IsNullOrWhiteSpace(searchAddress))
            {
                string addrTerm = searchAddress.Trim().ToLower();
                query = query.Where(p => p.Address != null && p.Address.ToLower().Contains(addrTerm));
            }

            if (minConvictions.HasValue)
                query = query.Where(p => p.Convictoins >= minConvictions.Value);

            if (maxConvictions.HasValue)
                query = query.Where(p => p.Convictoins <= maxConvictions.Value);

            Person = await query.ToListAsync();
        }
    }
}