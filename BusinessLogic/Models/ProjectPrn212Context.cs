using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Models;

public partial class ProjectPrn212Context : DbContext
{
    public ProjectPrn212Context()
    {
    }

    public ProjectPrn212Context(DbContextOptions<ProjectPrn212Context> options)
        : base(options)
    {
    }

    public virtual DbSet<AssessmentComponent> AssessmentComponents { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=8-NGUYENHUUHUNG\\HUNG;Initial Catalog=Project_Prn212;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AssessmentComponent>(entity =>
        {
            entity.HasKey(e => e.ComponentId).HasName("PK__Assessme__667AC126D530FABD");

            entity.ToTable("AssessmentComponent");

            entity.Property(e => e.ComponentId).HasColumnName("componentID");
            entity.Property(e => e.ComponentName)
                .HasMaxLength(100)
                .HasColumnName("componentName");
            entity.Property(e => e.SubjectId).HasColumnName("subjectID");
            entity.Property(e => e.Weight).HasColumnName("weight");

            entity.HasOne(d => d.Subject).WithMany(p => p.AssessmentComponents)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("FK__Assessmen__subje__276EDEB3");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK__Class__7577345ED558A6C8");

            entity.ToTable("Class");

            entity.HasIndex(e => e.ClassCode, "UQ__Class__0257F88155A884C7").IsUnique();

            entity.HasIndex(e => e.ClassName, "UQ__Class__B0303426FE6A76AB").IsUnique();

            entity.Property(e => e.ClassId).HasColumnName("classID");
            entity.Property(e => e.ClassCode)
                .HasMaxLength(20)
                .HasColumnName("classCode");
            entity.Property(e => e.ClassName)
                .HasMaxLength(100)
                .HasColumnName("className");
            entity.Property(e => e.Slot).HasColumnName("slot");
            entity.Property(e => e.SubjectId).HasColumnName("subjectID");
            entity.Property(e => e.TeacherId).HasColumnName("teacherID");

            entity.HasOne(d => d.Subject).WithMany(p => p.Classes)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("FK__Class__subjectID__30F848ED");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Classes)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK__Class__teacherID__31EC6D26");
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.EnrollmentId).HasName("PK__Enrollme__ACFF2C660664F303");

            entity.ToTable("Enrollment");

            entity.HasIndex(e => new { e.StudentId, e.ClassId }, "UQ__Enrollme__BA46A51893600411").IsUnique();

            entity.Property(e => e.EnrollmentId).HasColumnName("enrollmentID");
            entity.Property(e => e.ClassId).HasColumnName("classID");
            entity.Property(e => e.StudentId).HasColumnName("studentID");

            entity.HasOne(d => d.Class).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK__Enrollmen__class__3B75D760");

            entity.HasOne(d => d.Student).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Enrollmen__stude__3A81B327");
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => e.GradeId).HasName("PK__Grade__FB4362D9A1352E33");

            entity.ToTable("Grade");

            entity.HasIndex(e => new { e.EnrollmentId, e.ComponentId }, "UQ__Grade__DA988075A5C17B8C").IsUnique();

            entity.Property(e => e.GradeId).HasColumnName("gradeID");
            entity.Property(e => e.ComponentId).HasColumnName("componentID");
            entity.Property(e => e.EnrollmentId).HasColumnName("enrollmentID");
            entity.Property(e => e.Score).HasColumnName("score");

            entity.HasOne(d => d.Component).WithMany(p => p.Grades)
                .HasForeignKey(d => d.ComponentId)
                .HasConstraintName("FK__Grade__component__403A8C7D");

            entity.HasOne(d => d.Enrollment).WithMany(p => p.Grades)
                .HasForeignKey(d => d.EnrollmentId)
                .HasConstraintName("FK__Grade__enrollmen__3F466844");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Student__4D11D65C4255D48A");

            entity.ToTable("Student");

            entity.HasIndex(e => e.StudentCode, "UQ__Student__48F7EF36588ABF8E").IsUnique();

            entity.Property(e => e.StudentId).HasColumnName("studentID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("fullName");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Active")
                .HasColumnName("status");
            entity.Property(e => e.StudentCode)
                .HasMaxLength(20)
                .HasColumnName("studentCode");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("PK__Subject__ACF9A740BAAC7896");

            entity.ToTable("Subject");

            entity.HasIndex(e => e.SubjectCode, "UQ__Subject__97E46C97A5F1BB9C").IsUnique();

            entity.Property(e => e.SubjectId).HasColumnName("subjectID");
            entity.Property(e => e.SubjectCode)
                .HasMaxLength(20)
                .HasColumnName("subjectCode");
            entity.Property(e => e.SubjectName)
                .HasMaxLength(100)
                .HasColumnName("subjectName");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("PK__Teacher__98E9387505C6DD14");

            entity.ToTable("Teacher");

            entity.HasIndex(e => e.TeacherCode, "UQ__Teacher__3644755C455B0F26").IsUnique();

            entity.Property(e => e.TeacherId).HasColumnName("teacherID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("fullName");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Active")
                .HasColumnName("status");
            entity.Property(e => e.TeacherCode)
                .HasMaxLength(20)
                .HasColumnName("teacherCode");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
