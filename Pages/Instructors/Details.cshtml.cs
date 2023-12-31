﻿using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Pages.Instructors;

public class DetailsModel : PageModel
{
    private readonly SchoolContext _context;

    public DetailsModel(SchoolContext context)
    {
        _context = context;
    }

    public Instructor Instructor { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Instructor instructor = await _context.Instructors
                .Include(i => i.Courses)
                .SingleAsync(i => i.ID == id);

        if (instructor == null)
        {
            return RedirectToPage("./Index");
        }

        var departments = await _context.Departments
            .Where(d => d.InstructorID == id)
            .ToListAsync();
        departments.ForEach(d => d.InstructorID = null);

        _context.Instructors.Remove(instructor);

        await _context.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
}
