using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.App.Data;
using ContosoUniversity.App.Models;

namespace ContosoUniversity.App.Pages.Courses
{
    public class DetailsModel : PageModel
    {
        private readonly ContosoUniversity.App.Data.ContosoUniContextDb _context;

        public DetailsModel(ContosoUniversity.App.Data.ContosoUniContextDb context)
        {
            _context = context;
        }

        public Course Course { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FirstOrDefaultAsync(m => m.CourseId == id);

            if (course is not null)
            {
                Course = course;

                return Page();
            }

            return NotFound();
        }
    }
}
