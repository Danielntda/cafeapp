using System;
using CafeApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CafeApp.Persistence
{
    public partial class CafeDbContext : DbContext
    {
        public CafeDbContext() { }

        public CafeDbContext(DbContextOptions<CafeDbContext> options)
            : base(options) { }

        public virtual DbSet<Cafe> Cafe { get; set; } = null!;
        public virtual DbSet<Employee> Employee { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Only configure if not already configured (runtime MySQL or test InMemory)
            if (!optionsBuilder.IsConfigured)
            {
                // Optional: throw or log
                // throw new InvalidOperationException("DbContextOptions not configured. Configure in DI or use UseMySql/UseInMemoryDatabase.");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Cafe>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");
                entity.ToTable("cafe");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Description).HasColumnType("text").HasColumnName("description");
                entity.Property(e => e.Location).HasMaxLength(255).HasColumnName("location");
                entity.Property(e => e.Logo).HasMaxLength(500).HasColumnName("logo");
                entity.Property(e => e.Name).HasMaxLength(255).HasColumnName("name");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");
                entity.ToTable("employee");

                entity.HasIndex(e => e.EmailAddress, "email_address").IsUnique();
                entity.HasIndex(e => e.CafeId, "fk_employee_cafe");

                entity.Property(e => e.Id).HasMaxLength(10).HasColumnName("id");
                entity.Property(e => e.CafeId).HasColumnName("cafe_id");
                entity.Property(e => e.EmailAddress).HasColumnName("email_address");
                entity.Property(e => e.Gender).HasColumnType("enum('Male','Female')").HasColumnName("gender");
                entity.Property(e => e.Name).HasMaxLength(100).HasColumnName("name");
                entity.Property(e => e.PhoneNumber).HasMaxLength(8).IsFixedLength().HasColumnName("phone_number");
                entity.Property(e => e.StartDate).HasColumnName("start_date");

                entity.HasOne(d => d.Cafe)
                      .WithMany(p => p.Employees)
                      .HasForeignKey(d => d.CafeId)
                      .HasConstraintName("fk_employee_cafe");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
