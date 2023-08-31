using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using QuestionnaireFillingService.Models;

namespace QuestionnaireFillingService;

public partial class DbServiceContext : DbContext
{
    public DbServiceContext()
    {
    }

    public DbServiceContext(DbContextOptions<DbServiceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BankDetail> BankDetails { get; set; }

    public virtual DbSet<LegalEntity> LegalEntities { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseSqlite("Data Source=dbService.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LegalEntity>(entity =>
        {
            entity.ToTable("LegalEntity");

            entity.HasIndex(e => e.Id, "Id").IsUnique();

            entity.HasIndex(e => e.Msrn, "Msrn").IsUnique();

            entity.HasIndex(e => e.Msrn, "Msrnie").IsUnique();

            entity.HasIndex(e => e.Tin, "Tin").IsUnique();

            entity.Property(e => e.IsLlc).HasColumnName("IsLLC");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id);
            entity.Property(e => e.IdBankDetails).HasColumnName("Id_BankDetails");
            entity.Property(e => e.IdLegalEntity).HasColumnName("Id_LegalEntity");

            entity.HasOne(d => d.IdBankDetailsNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdBankDetails);

            entity.HasOne(d => d.IdLegalEntityNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdLegalEntity);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
