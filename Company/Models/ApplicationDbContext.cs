using Microsoft.EntityFrameworkCore;

namespace Company.Models
{
       public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
            }

            public DbSet<Employee> Employees { get; set; }
            public DbSet<Role> Roles { get; set; }
        }

    
}
