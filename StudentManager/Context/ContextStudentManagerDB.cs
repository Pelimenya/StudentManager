using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using StudentManager.Models;

namespace StudentManager.Context;

public partial class ContextStudentManagerDB : DbContext
{
    public ContextStudentManagerDB()
    {
    }

    public ContextStudentManagerDB(DbContextOptions<ContextStudentManagerDB> options)
        : base(options)
    {
    }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Identification> Identifications { get; set; }

    public virtual DbSet<LibraryCard> LibraryCards { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("host=localhost; port=5432; database=StudentManagerDB; username=Pelimenya; password=root");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => e.AttendanceId).HasName("Attendance_pkey");

            entity.ToTable("Attendance");

            entity.Property(e => e.AttendanceId).HasColumnName("attendance_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");
            entity.Property(e => e.StudentId).HasColumnName("student_id");

            entity.HasOne(d => d.Student).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Attendance_student_id_fkey");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("Contact_pkey");

            entity.ToTable("Contact");

            entity.Property(e => e.ContactId).HasColumnName("contact_id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.StudentId).HasColumnName("student_id");

            entity.HasOne(d => d.Student).WithMany(p => p.Contacts)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Contact_student_id_fkey");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("Course_pkey");

            entity.ToTable("Course");

            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.CourseName)
                .HasMaxLength(255)
                .HasColumnName("course_name");
            entity.Property(e => e.CourseNumber).HasColumnName("course_number");
        });

        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.ExamId).HasName("Exam_pkey");

            entity.ToTable("Exam");

            entity.Property(e => e.ExamId).HasColumnName("exam_id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.ExamDate).HasColumnName("exam_date");
            entity.Property(e => e.Grade)
                .HasMaxLength(20)
                .HasColumnName("grade");
            entity.Property(e => e.Groupid).HasColumnName("groupid");

            entity.HasOne(d => d.Course).WithMany(p => p.Exams)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Exam_course_id_fkey");

            entity.HasOne(d => d.Group).WithMany(p => p.Exams)
                .HasForeignKey(d => d.Groupid)
                .HasConstraintName("Exam_group_id_fkey");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("Group_pkey");

            entity.ToTable("Group");

            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.CourseNumber).HasColumnName("course_number");
            entity.Property(e => e.GroupName)
                .HasMaxLength(50)
                .HasColumnName("group_name");
        });

        modelBuilder.Entity<Identification>(entity =>
        {
            entity.HasKey(e => e.IdentificationId).HasName("Identification_pkey");

            entity.ToTable("Identification");

            entity.Property(e => e.IdentificationId).HasColumnName("identification_id");
            entity.Property(e => e.Inn)
                .HasMaxLength(12)
                .HasColumnName("inn");
            entity.Property(e => e.PassportNumber)
                .HasMaxLength(6)
                .HasColumnName("passport_number");
            entity.Property(e => e.PassportSeries)
                .HasMaxLength(4)
                .HasColumnName("passport_series");
            entity.Property(e => e.StudentId).HasColumnName("student_id");

            entity.HasOne(d => d.Student).WithMany(p => p.Identifications)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Identification_student_id_fkey");
        });

        modelBuilder.Entity<LibraryCard>(entity =>
        {
            entity.HasKey(e => e.CardId).HasName("LibraryCard_pkey");

            entity.ToTable("LibraryCard");

            entity.Property(e => e.CardId).HasColumnName("card_id");
            entity.Property(e => e.ExpiryDate).HasColumnName("expiry_date");
            entity.Property(e => e.IssueDate).HasColumnName("issue_date");
            entity.Property(e => e.StudentId).HasColumnName("student_id");

            entity.HasOne(d => d.Student).WithMany(p => p.LibraryCards)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("LibraryCard_student_id_fkey");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("Student_pkey");

            entity.ToTable("Student");

            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("full_name");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Group).WithMany(p => p.Students)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Student_group_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Students)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_user_id");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("Users_pkey");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("role");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });
        modelBuilder.HasSequence<int>("Attendance_attendance_id_seq");
        modelBuilder.HasSequence<int>("Contact_contact_id_seq");
        modelBuilder.HasSequence<int>("Course_course_id_seq");
        modelBuilder.HasSequence<int>("Exam_exam_id_seq");
        modelBuilder.HasSequence<int>("Group_group_id_seq");
        modelBuilder.HasSequence<int>("Identification_identification_id_seq");
        modelBuilder.HasSequence<int>("LibraryCard_card_id_seq");
        modelBuilder.HasSequence<int>("Student_student_id_seq");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
