using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Company.Models;

namespace Company.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
              return _context.Employees != null ? 
                          View(await _context.Employees.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Employees'  is null.");
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeNo == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        private async Task PopulateRolesAndDepartments()
        {
            var departmentQuery = from r in _context.Roles
                                  orderby r.Department
                                  select r.Department;

            ViewData["Departments"] = new SelectList(departmentQuery.Distinct().ToList());
            var roles = await _context.Roles.OrderBy(r => r.Position).ToListAsync();
            ViewData["Roles"] = roles;
        }

        // GET: Employees/Create
        public async Task<IActionResult> Create()
        {
            var departmentQuery = from r in _context.Roles
                                  orderby r.Department
                                  select r.Department;

            ViewData["Departments"] = new SelectList(departmentQuery.Distinct().ToList());
            var roles = await _context.Roles.OrderBy(r => r.Position).ToListAsync();
            ViewData["Roles"] = roles;

            return View();
        }


        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
         public async Task<IActionResult> Create([Bind("EmployeeNo,Name,DateOfBirth,Gender,Department,Position")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                // Check if the employee number already exists in the database
                var existingEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeNo == employee.EmployeeNo);
                if (existingEmployee != null)
                {
                    // Add a custom validation error to the ModelState object
                    ModelState.AddModelError(nameof(Employee.EmployeeNo), "Employee number already exists.");

                    // Populate the ViewData for "Roles" and "Departments"
                    await PopulateRolesAndDepartments();

                    // Return the view with the invalid model
                    return View(employee);
                }

                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await PopulateRolesAndDepartments();
            return View(employee);
        }


        // GET: Employees/Edit/5

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            await PopulateRolesAndDepartments();

            return View(employee);
        }



        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeNo,Name,DateOfBirth,Gender,Department,Position")] Employee employee)
        {
            if (id != employee.EmployeeNo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeNo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }






        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeNo == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Employees'  is null.");
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
          return (_context.Employees?.Any(e => e.EmployeeNo == id)).GetValueOrDefault();
        }
    }
}
