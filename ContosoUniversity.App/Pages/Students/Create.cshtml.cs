﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ContosoUniversity.App.Data;
using ContosoUniversity.App.Models;

namespace ContosoUniversity.App.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly ContosoUniversity.App.Data.ContosoUniContextDb _context;

        public CreateModel(ContosoUniversity.App.Data.ContosoUniContextDb context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Student Student { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var emptyStudent = new Student();

            if (await TryUpdateModelAsync<Student>(
                emptyStudent,
                "student", //Prefix for form value
                 s=>s.FirstMidName, s=>s.LastName, s=>s.EnrollmentDate))
            {
                _context.Students.Add(emptyStudent);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");

               
            }

            return Page();

        }
    }
}
