using System;
using System.Collections.Generic;
using CourseProject.DataModels;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace CourseProject.Data;

public partial class EmploymentServiceContext : DbContext
{
    public EmploymentServiceContext()
    {
    }

    public EmploymentServiceContext(DbContextOptions<EmploymentServiceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vacancy> Vacancies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("data source=localhost;initial catalog=course_project;user id=логин;password=пароль", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.44-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PRIMARY");

            entity.ToTable("event");

            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.AvailableSpace).HasColumnName("available_space");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.EventDate)
                .HasColumnType("datetime")
                .HasColumnName("event_date");
            entity.Property(e => e.Photo)
                .HasColumnType("blob")
                .HasColumnName("photo");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'Планируется'")
                .HasColumnType("enum('Отменено','Проведено','Отложено','Планируется')")
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedOn).HasColumnName("updated_on");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PRIMARY");

            entity.ToTable("request");

            entity.HasIndex(e => e.EventId, "FK_request_event_idx");

            entity.HasIndex(e => e.UserId, "FK_request_user_idx");

            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.Content)
                .HasMaxLength(100)
                .HasColumnName("content");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.Institution)
                .HasMaxLength(100)
                .HasDefaultValueSql("'Нет'")
                .HasColumnName("institution");
            entity.Property(e => e.PeopleNumber)
                .HasDefaultValueSql("'1'")
                .HasColumnName("people_number");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'На рассмотрении'")
                .HasColumnType("enum('На рассмотрении','Отклонена','Принята')")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedOn).HasColumnName("updated_on");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Event).WithMany(p => p.Requests)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_request_event");

            entity.HasOne(d => d.User).WithMany(p => p.Requests)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_request_user");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.ToTable("role");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.RoleId, "FK_user_role_idx");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(16)
                .HasColumnName("password");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(25)
                .HasColumnName("patronymic");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Surname)
                .HasMaxLength(20)
                .HasColumnName("surname");
            entity.Property(e => e.Username)
                .HasMaxLength(20)
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_user_role");
        });

        modelBuilder.Entity<Vacancy>(entity =>
        {
            entity.HasKey(e => e.VacancyId).HasName("PRIMARY");

            entity.ToTable("vacancy");

            entity.Property(e => e.VacancyId).HasColumnName("vacancy_id");
            entity.Property(e => e.Address)
                .HasMaxLength(50)
                .HasColumnName("address");
            entity.Property(e => e.Company)
                .HasMaxLength(150)
                .HasColumnName("company");
            entity.Property(e => e.JobInformation)
                .HasMaxLength(500)
                .HasColumnName("job_information");
            entity.Property(e => e.MaxSalary).HasColumnName("max_salary");
            entity.Property(e => e.MinSalary).HasColumnName("min_salary");
            entity.Property(e => e.Region)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Архангельская область'")
                .HasColumnName("region");
            entity.Property(e => e.Requirements)
                .HasMaxLength(500)
                .HasColumnName("requirements");
            entity.Property(e => e.Responsibility)
                .HasMaxLength(500)
                .HasColumnName("responsibility");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedOn).HasColumnName("updated_on");
            entity.Property(e => e.Workplace)
                .HasMaxLength(100)
                .HasColumnName("workplace");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
