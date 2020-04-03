﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineMasterG.Models.DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class OnlinemasterjiEntities : DbContext
    {
        public OnlinemasterjiEntities()
            : base("name=OnlinemasterjiEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ColleageCourse> ColleageCourses { get; set; }
        public virtual DbSet<CollegePaper> CollegePapers { get; set; }
        public virtual DbSet<CollegeSubject> CollegeSubjects { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<DataContent> DataContents { get; set; }
        public virtual DbSet<DataFile> DataFiles { get; set; }
        public virtual DbSet<ExamSection> ExamSections { get; set; }
        public virtual DbSet<ExamSectionLink> ExamSectionLinks { get; set; }
        public virtual DbSet<GeneralInstruction> GeneralInstructions { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<MockTest> MockTests { get; set; }
        public virtual DbSet<MockTestAttemptDetail> MockTestAttemptDetails { get; set; }
        public virtual DbSet<MockTestAttempt> MockTestAttempts { get; set; }
        public virtual DbSet<ProblemMaster> ProblemMasters { get; set; }
        public virtual DbSet<ProblemsReported> ProblemsReporteds { get; set; }
        public virtual DbSet<QuestionAnswerChoice> QuestionAnswerChoices { get; set; }
        public virtual DbSet<QuestionMaster> QuestionMasters { get; set; }
        public virtual DbSet<QuestionPoint> QuestionPoints { get; set; }
        public virtual DbSet<QuestionsMockTest> QuestionsMockTests { get; set; }
        public virtual DbSet<QuestionUpload> QuestionUploads { get; set; }
        public virtual DbSet<SchoolClass> SchoolClasses { get; set; }
        public virtual DbSet<SchoolPaper> SchoolPapers { get; set; }
        public virtual DbSet<SchoolSection> SchoolSections { get; set; }
        public virtual DbSet<SchoolSubject> SchoolSubjects { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<SubjectMaster> SubjectMasters { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<CurrentAffairsCategory> CurrentAffairsCategories { get; set; }
        public virtual DbSet<CurrentAffairsUpload> CurrentAffairsUploads { get; set; }
        public virtual DbSet<DailyQuizCourse> DailyQuizCourses { get; set; }
        public virtual DbSet<DailyQuizSubject> DailyQuizSubjects { get; set; }
        public virtual DbSet<UserType> UserTypes { get; set; }
        public virtual DbSet<DailyQuiz> DailyQuizs { get; set; }
        public virtual DbSet<DailyQuizUpload> DailyQuizUploads { get; set; }
    }
}
