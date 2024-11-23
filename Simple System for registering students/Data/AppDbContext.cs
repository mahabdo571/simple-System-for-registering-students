using Microsoft.EntityFrameworkCore;
using Simple_System_for_registering_students.Models;

namespace Simple_System_for_registering_students.Data
{

 
        public class AppDbContext : DbContext
        {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

            public DbSet<Student> Students { get; set; }
            public DbSet<Staff> Staffs { get; set; }
        }
    }



