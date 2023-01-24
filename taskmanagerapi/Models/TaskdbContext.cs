using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using taskmanagerapi.Models.IdentityModels;

namespace taskmanagerapi.Models;

public partial class TaskdbContext : DbContext
{
    public TaskdbContext()
    {
    }

    public TaskdbContext(DbContextOptions<TaskdbContext> options)
        : base(options)
    {
    }


    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TaskUser> TaskUsers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("task_pk");

            entity.ToTable("Task");

            entity.Property(e => e.TaskId)
                .HasDefaultValueSql("nextval('task_seq'::regclass)")
                .HasColumnName("task_id");
            entity.Property(e => e.IsComplete).HasColumnName("is_complete");
            entity.Property(e => e.TaskDescription)
                .HasColumnType("character varying")
                .HasColumnName("task_description");
            entity.Property(e => e.TaskTitle)
                .HasColumnType("character varying")
                .HasColumnName("task_title");
        });

        modelBuilder.Entity<TaskUser>(entity =>
        {
            entity.HasKey(e => e.TaskuserId).HasName("taskuser_pk");

            entity.ToTable("TaskUser");

            entity.Property(e => e.TaskuserId)
                .HasDefaultValueSql("nextval('taskuser_seq'::regclass)")
                .HasColumnName("taskuser_id");
            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Task).WithMany(p => p.TaskUsers)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("taskuser_fk");

            entity.HasOne(d => d.User).WithMany(p => p.TaskUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("taskuser_user_fk");
        });


        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
