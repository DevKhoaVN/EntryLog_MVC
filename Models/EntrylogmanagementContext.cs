using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TestMySql.Models;

public partial class EntrylogmanagementContext : DbContext
{
    public EntrylogmanagementContext()
    {
    }

    public EntrylogmanagementContext(DbContextOptions<EntrylogmanagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Absentreport> Absentreports { get; set; }

    public virtual DbSet<Alert> Alerts { get; set; }

    public virtual DbSet<Efmigrationshistory> Efmigrationshistories { get; set; }

    public virtual DbSet<Entrylog> Entrylogs { get; set; }

    public virtual DbSet<Parent> Parents { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userrole> Userroles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("Server=localhost;Database=entrylogmanagement;User ID=root;Password=Vakhoa205!");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Absentreport>(entity =>
        {
            entity.HasKey(e => e.AbsentReportId).HasName("PRIMARY");

            entity.ToTable("absentreport");

            entity.HasIndex(e => e.CreateDay, "idx_create_day");

            entity.HasIndex(e => e.ParentId, "idx_parent_id");

            entity.HasIndex(e => e.StudentId, "idx_student_id");

            entity.Property(e => e.CreateDay).HasColumnType("datetime");
            entity.Property(e => e.Reason).HasMaxLength(255);

            entity.HasOne(d => d.Parent).WithMany(p => p.Absentreports)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("absentreport_ibfk_2");

            entity.HasOne(d => d.Student).WithMany(p => p.Absentreports)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("absentreport_ibfk_1");
        });

        modelBuilder.Entity<Alert>(entity =>
        {
            entity.HasKey(e => e.AlertId).HasName("PRIMARY");

            entity.ToTable("alert");

            entity.HasIndex(e => e.AlertTime, "idx_alert_time");

            entity.HasIndex(e => e.StudentId, "idx_student_id");

            entity.Property(e => e.AlertTime).HasColumnType("datetime");

            entity.HasOne(d => d.Student).WithMany(p => p.Alerts)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("alert_ibfk_1");
        });

        modelBuilder.Entity<Efmigrationshistory>(entity =>
        {
            entity.HasKey(e => e.MigrationId).HasName("PRIMARY");

            entity.ToTable("__efmigrationshistory");

            entity.Property(e => e.MigrationId).HasMaxLength(150);
            entity.Property(e => e.ProductVersion).HasMaxLength(32);
        });

        modelBuilder.Entity<Entrylog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PRIMARY");

            entity.ToTable("entrylog");

            entity.HasIndex(e => e.LogTime, "idx_log_time");

            entity.HasIndex(e => e.StudentId, "idx_student_id");

            entity.Property(e => e.LogTime).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValueSql("'Out'");

            entity.HasOne(d => d.Student).WithMany(p => p.Entrylogs)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("entrylog_ibfk_1");
        });

        modelBuilder.Entity<Parent>(entity =>
        {
            entity.HasKey(e => e.ParentId).HasName("PRIMARY");

            entity.ToTable("parent");

            entity.HasIndex(e => e.Email, "idx_email");

            entity.HasIndex(e => e.Phone, "idx_phone");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(25);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PRIMARY");

            entity.ToTable("student");

            entity.HasIndex(e => e.Class, "idx_class");

            entity.HasIndex(e => e.DayOfBirth, "idx_day_of_birth");

            entity.HasIndex(e => e.ParentId, "idx_parent_id");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Class).HasMaxLength(5);
            entity.Property(e => e.DayOfBirth).HasColumnType("datetime");
            entity.Property(e => e.Gender).HasMaxLength(5);
            entity.Property(e => e.JoinDay)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(25);

            entity.HasOne(d => d.Parent).WithMany(p => p.Students)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("student_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.StudentId, "StudentID").IsUnique();

            entity.HasIndex(e => e.RoleId, "idx_role_id");

            entity.HasIndex(e => e.StudentId, "idx_student_id");

            entity.HasIndex(e => e.UserName, "idx_user_name");

            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.UserName).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_ibfk_1");
        });

        modelBuilder.Entity<Userrole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.ToTable("userrole");

            entity.HasIndex(e => e.RoleName, "idx_role_name");

            entity.Property(e => e.RoleName).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
