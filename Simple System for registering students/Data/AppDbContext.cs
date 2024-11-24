using Microsoft.EntityFrameworkCore;
using Simple_System_for_registering_students.Models;

namespace Simple_System_for_registering_students.Data
{
    /// <summary>
    /// The database context for the application.
    /// </summary>

    public class AppDbContext : DbContext
        {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>
        /// The Staff table.
        /// </summary>
        public DbSet<Staff> Staffs { get; set; }

        /// <summary>
        /// The Students table.
        /// </summary>
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

   
            modelBuilder.Entity<Staff>()
                .HasMany(s => s.Students)
                .WithOne(st => st.Staff)
                .HasForeignKey(st => st.StaffId)        
                .OnDelete(DeleteBehavior.Cascade);

   
     
        }
    }
    }



