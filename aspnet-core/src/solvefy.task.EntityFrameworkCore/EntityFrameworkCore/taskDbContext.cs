using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using solvefy.task.Authorization.Roles;
using solvefy.task.Authorization.Users;
using solvefy.task.MultiTenancy;
using solvefy.task.Entities;

namespace solvefy.task.EntityFrameworkCore
{
    public class taskDbContext : AbpZeroDbContext<Tenant, Role, User, taskDbContext>
    {
        /* Define a DbSet for each entity of the application */

        public DbSet<JobPosition> JobPositions { get; set; }
        public DbSet<Candidate> Candidates { get; set; }

        public taskDbContext(DbContextOptions<taskDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure JobPosition entity
            modelBuilder.Entity<JobPosition>(b =>
            {
                b.ToTable("AppJobPositions");
                b.HasKey(x => x.Id);
                b.Property(x => x.Title).IsRequired().HasMaxLength(256);
                b.Property(x => x.Description).HasMaxLength(2000);
                b.Property(x => x.Location).HasMaxLength(256);
                b.Property(x => x.IsActive).IsRequired();
            });

            // Configure Candidate entity
            modelBuilder.Entity<Candidate>(b =>
            {
                b.ToTable("AppCandidates");
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).IsRequired().HasMaxLength(256);
                b.Property(x => x.Email).IsRequired().HasMaxLength(256);
                b.Property(x => x.Phone).HasMaxLength(50);
                b.Property(x => x.ResumePath).HasMaxLength(500);

                // Configure foreign key
                b.HasOne(c => c.JobPosition)
                 .WithMany()
                 .HasForeignKey(c => c.JobPositionId)
                 .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}