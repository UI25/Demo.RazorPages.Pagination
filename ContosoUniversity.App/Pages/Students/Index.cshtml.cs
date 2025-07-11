using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.App.Data;
using ContosoUniversity.App.Models;
using ContosoUniversity.App.Extensions;
using System.ComponentModel.Design;

namespace ContosoUniversity.App.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly ContosoUniContextDb _context;
        private readonly IConfiguration Configuration;

        public IndexModel(ContosoUniContextDb context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public string Active {get; set;}
        public int TotalCount { get; set; }
        public string LastNameSort { get; set; }
        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public string SearchString { get; set; }
        [BindProperty]
        public int CurrentPageSize { get; set; } = 10; // Default page size, can be overridden by configuration

        public PaginatedList<Student> Student { get;set; } = default!;

        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex, int currentPageSize)
        {
            CurrentSort = sortOrder;
            LastNameSort = string.IsNullOrEmpty(sortOrder) ? "lastname_desc" : "";
            NameSort = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "name";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";
            

            if (searchString != null)
            {
                SearchString = searchString;
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            CurrentFilter = searchString;

            IQueryable<Student> studentsIQ = from s in _context.Students
                                             select s;

            TotalCount = studentsIQ.Count();

            if (!String.IsNullOrEmpty(searchString))
            {
                studentsIQ = studentsIQ.Where(s => s.LastName.Contains(searchString)
                                                || s.FirstMidName.Contains(searchString));

            }
            if (currentPageSize > 0)
            {
                CurrentPageSize = currentPageSize;
            }
            else
            {
                CurrentPageSize = Configuration.GetValue<int>("PageSize"); // Default to 4 if not set
            }


            switch (sortOrder)
            {
                case "lastname_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.LastName);
                    break;
                case "name_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.FirstMidName); 
                    break;
                case "name":
                    studentsIQ = studentsIQ.OrderBy(s => s.FirstMidName);
                    break;
                case "Date":
                    studentsIQ = studentsIQ.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    studentsIQ = studentsIQ.OrderBy(s => s.LastName);
                    break;
            }

            Student = await PaginatedList<Student>.CreateAsync(
                studentsIQ.AsNoTracking(),
                pageIndex ?? 1, 
                CurrentPageSize);
        }
    }
}
