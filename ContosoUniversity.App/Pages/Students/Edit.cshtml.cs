using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.App.Data;
using ContosoUniversity.App.Models;

namespace ContosoUniversity.App.Pages.Students
{
    public class EditModel : PageModel
    {
        private readonly ContosoUniversity.App.Data.ContosoUniContextDb _context;

        public EditModel(ContosoUniversity.App.Data.ContosoUniContextDb context)
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

            var student =  await _context.Students.FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            Student = student;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int pageIndex, int currentPageSize)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

         
            try
            {
                _context.Attach(Student).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                
                return RedirectToPage("./Index", new
                {
                    pageIndex = pageIndex,
                    currentPageSize = currentPageSize
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(Student.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }           
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
