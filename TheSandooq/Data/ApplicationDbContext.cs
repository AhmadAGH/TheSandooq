using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sandooqna.Models;

namespace TheSandooq.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sandooq>()
                .HasOne(s => s.creator)
                .WithMany(u => u.CreatedSandooqs)
                .HasForeignKey(s => s.creatorID)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete to avoid cycles

            // Many-to-Many Relationship: Sandooq and ApplicationUser (Members)
            modelBuilder.Entity<Sandooq>()
                 .HasMany(s => s.members)
                 .WithMany(u => u.Sandooqs)
                 .UsingEntity<Dictionary<string, object>>(
                     "SandooqMembers",
                     j => j
                         .HasOne<ApplicationUser>()
                         .WithMany()
                         .HasForeignKey("memberId")
                         .OnDelete(DeleteBehavior.Restrict), // Use Restrict or NoAction instead of Cascade
                     j => j
                         .HasOne<Sandooq>()
                         .WithMany()
                         .HasForeignKey("Sandooqid")
                         .OnDelete(DeleteBehavior.Cascade), // You can choose which one to keep as Cascade
                     j => j.ToTable("SandooqMembers")
                 );

            modelBuilder.Entity<Expense>()
            .HasOne(e => e.sandooq)
            .WithMany(s => s.expenses)
            .HasForeignKey(e => e.SandooqId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Income>()
            .HasOne(e => e.sandooq)
            .WithMany(s => s.incomes)
            .HasForeignKey(e => e.SandooqId)
            .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
            }
        public DbSet<Sandooq> dbSandooqs { get; set; }
        public DbSet<Income> dbIncomes { get; set; }
        public DbSet<Expense> dbExpenses { get; set; }
        public DbSet<Category> dbCategories { get; set; }
    }
}
