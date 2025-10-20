using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SKARB.Web.Models;

namespace SKARB.Web.Data;

public partial class SkarbDbContext : DbContext
{
    public SkarbDbContext()
    {
    }

    public SkarbDbContext(DbContextOptions<SkarbDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Decision> Decisions { get; set; }

    public virtual DbSet<Incident> Incidents { get; set; }

    public virtual DbSet<IncidentRole> IncidentRoles { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<PersonIncident> PersonIncidents { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=skarb;Username=postgres;Password=1");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Decision>(entity =>
        {
            entity.HasKey(e => e.DecisionId).HasName("decision_pkey");

            entity.ToTable("decision");

            entity.Property(e => e.DecisionId).HasColumnName("decision_id");
            entity.Property(e => e.DecisionName).HasColumnName("decision_name");
        });

        modelBuilder.Entity<Incident>(entity =>
        {
            entity.HasKey(e => e.IncidentId).HasName("incident_pkey");

            entity.ToTable("incident");

            entity.Property(e => e.IncidentId).HasColumnName("incident_id");
            entity.Property(e => e.DecisionId).HasColumnName("decision_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IncidentDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("incident_date");
            entity.Property(e => e.RegNumber).HasColumnName("reg_number");

            entity.HasOne(d => d.Decision).WithMany(p => p.Incidents)
                .HasForeignKey(d => d.DecisionId)
                .HasConstraintName("incident_decision_id_fkey");
        });

        modelBuilder.Entity<IncidentRole>(entity =>
        {
            entity.HasKey(e => e.IncidentRoleId).HasName("incident_role_pkey");

            entity.ToTable("incident_role");

            entity.Property(e => e.IncidentRoleId).HasColumnName("incident_role_id");
            entity.Property(e => e.IncidentRoleName).HasColumnName("incident_role_name");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.PersonId).HasName("person_pkey");

            entity.ToTable("person");

            entity.Property(e => e.PersonId).HasColumnName("person_id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Convictoins).HasColumnName("convictoins");
            entity.Property(e => e.FirstName).HasColumnName("first_name");
            entity.Property(e => e.LastName).HasColumnName("last_name");
            entity.Property(e => e.MiddleName).HasColumnName("middle_name");
            entity.Property(e => e.RegNumber).HasColumnName("reg_number");
        });

        modelBuilder.Entity<PersonIncident>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("person_incident_pkey");

            entity.ToTable("person_incident");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IncidentId).HasColumnName("incident_id");
            entity.Property(e => e.IncidentRoleId).HasColumnName("incident_role_id");
            entity.Property(e => e.PersonId).HasColumnName("person_id");

            entity.HasOne(d => d.Incident).WithMany(p => p.PersonIncidents)
                .HasForeignKey(d => d.IncidentId)
                .HasConstraintName("person_incident_incident_id_fkey");

            entity.HasOne(d => d.IncidentRole).WithMany(p => p.PersonIncidents)
                .HasForeignKey(d => d.IncidentRoleId)
                .HasConstraintName("person_incident_incident_role_id_fkey");

            entity.HasOne(d => d.Person).WithMany(p => p.PersonIncidents)
                .HasForeignKey(d => d.PersonId)
                .HasConstraintName("person_incident_person_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
