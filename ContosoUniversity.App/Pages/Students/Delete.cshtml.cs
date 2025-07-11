using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.App.Data;
using ContosoUniversity.App.Models;

namespace ContosoUniversity.App.Pages.Students
{
    public class DeleteModel : PageModel
    {
        private readonly ContosoUniversity.App.Data.ContosoUniContextDb _context;

        public DeleteModel(ContosoUniversity.App.Data.ContosoUniContextDb context)
        {
            _context = context;
        }

        [BindProperty]
        public Student Student { get; set; } = default!;
        public int PageIndex { get; set; }
        public int CurrentPageSize { get; set; }
        public string CurrentFilter { get; set; }
        public string SearchString { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, string currentFilter, string searchString, int pageIndex, int currentPageSize)
        {
            PageIndex = pageIndex;
            CurrentPageSize = currentPageSize;
            CurrentFilter = currentFilter;
            SearchString = searchString;

            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FirstOrDefaultAsync(m => m.Id == id);

            if (student is not null)
            {
                Student = student;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id, int pageIndex, int currentPageSize)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                Student = student;
                _context.Students.Remove(Student);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index", new
            {
                pageIndex = pageIndex,
                currentPageSize = currentPageSize
            });
        }
    }
}
