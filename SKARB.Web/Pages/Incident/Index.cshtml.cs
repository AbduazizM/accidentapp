using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SKARB.Web.Data;
using SKARB.Web.Models;

namespace SKARB.Web.Pages.Incident
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly SKARB.Web.Data.SkarbDbContext _context;

        public IndexModel(SKARB.Web.Data.SkarbDbContext context)
        {
            _context = context;
        }

        public IList<SKARB.Web.Models.Incident> Incident { get; set; } = default!;

        public string? SearchRegNumber { get; set; }
        public string? SearchDescription { get; set; }
        public int? DecisionId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public List<Decision> Decisions { get; set; } = new();

        public async Task OnGetAsync(
            string? searchRegNumber,
            string? searchDescription,
            int? decisionId,
            DateTime? dateFrom,
            DateTime? dateTo)
        {
            SearchRegNumber = searchRegNumber;
            SearchDescription = searchDescription;
            DecisionId = decisionId;
            DateFrom = dateFrom;
            DateTo = dateTo;

            Decisions = await _context.Decisions.ToListAsync();

            IQueryable<SKARB.Web.Models.Incident> query = _context.Incidents.Include(i => i.Decision);

            if (!string.IsNullOrWhiteSpace(searchRegNumber))
            {
                string term = searchRegNumber.Trim().ToLower();
                query = query.Where(i => i.RegNumber != null && i.RegNumber.ToLower().Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(searchDescription))
            {
                string term = searchDescription.Trim().ToLower();
                query = query.Where(i => i.Description != null && i.Description.ToLower().Contains(term));
            }

            if (decisionId.HasValue && decisionId.Value != -1)
            {
                query = query.Where(i => i.DecisionId == decisionId.Value);
            }

            if (dateFrom.HasValue)
            {
                query = query.Where(i => i.IncidentDate >= dateFrom.Value.Date);
            }

            if (dateTo.HasValue)
            {
                var endDate = dateTo.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(i => i.IncidentDate <= endDate);
            }

            Incident = await query.ToListAsync();
        }
    }
}