using OnlineMasterG.Base;
using OnlineMasterG.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineMasterG.CommonServices
{
    public class SubjectService : ServiceBase
    {
      
        public static ServiceResponse SaveSubject(Subject subject, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            if (subject.SubjectId == 0)
            {
                DB.Subjects.Add(subject);
                DB.SaveChanges();
            }
            else
            {
                var dbSubject = Fetch(subject.SubjectId);
                if (dbSubject == null)
                {
                    sr.AddError($"Subject Name {subject.SubjectName} is not found.");
                    return sr;
                }
                else
                {
                    dbSubject.SubjectId = subject.SubjectId;
                    dbSubject.SubjectName = subject.SubjectName;
                    dbSubject.CourseId = subject.CourseId;
                    dbSubject.Category = subject.Category;
                    dbSubject.SectionId = subject.SectionId;
                    dbSubject.TestId = subject.TestId;
                    dbSubject.LanguageCode = subject.LanguageCode;
                    dbSubject.Sequence = subject.Sequence;
                    dbSubject.Isactive = true;
                    dbSubject.EditBy = auditlogin;
                    dbSubject.EditOn = DateTime.Now;
                    // Save in DB
                    DB.SaveChanges();

                    // Return
                    sr.ReturnId = dbSubject.SubjectId;
                    sr.ReturnName = dbSubject.SubjectName;

                    return sr;
                }
            }
            return sr;
        }
        public static ServiceResponse SaveCollegeSubject(CollegeSubject subject, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            if (subject.SubjectId == 0)
            {
                DB.CollegeSubjects.Add(subject);
                DB.SaveChanges();
            }
            else
            {
                var dbSubject = FetchCollegeSubject(subject.SubjectId);
                if (dbSubject == null)
                {
                    sr.AddError($"Subject Name {subject.SubjectName} is not found.");
                    return sr;
                }
                else
                {
                    dbSubject.SubjectId = subject.SubjectId;
                    dbSubject.SubjectName = subject.SubjectName;
                    dbSubject.CourseId = subject.CourseId;
                    dbSubject.LanguageCode = subject.LanguageCode;
                    dbSubject.Sequence = subject.Sequence;
                    dbSubject.Isactive = true;
                    dbSubject.EditBy = auditlogin;
                    dbSubject.EditOn = DateTime.Now;
                    // Save in DB
                    DB.SaveChanges();

                    // Return
                    sr.ReturnId = dbSubject.SubjectId;
                    sr.ReturnName = dbSubject.SubjectName;

                    return sr;
                }
            }
            return sr;
        }
        public static ServiceResponse SaveSchoolSubject(SchoolSubject subject, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            if (subject.SubjectId == 0)
            {
                DB.SchoolSubjects.Add(subject);
                DB.SaveChanges();
            }
            else
            {
                var dbSubject = FetchSchoolSubject(subject.SubjectId);
                if (dbSubject == null)
                {
                    sr.AddError($"Subject Name {subject.SubjectName} is not found.");
                    return sr;
                }
                else
                {
                    dbSubject.SubjectId = subject.SubjectId;
                    dbSubject.SubjectName = subject.SubjectName;
                    dbSubject.ClassId = subject.ClassId;
                    dbSubject.LanguageCode = subject.LanguageCode;
                    dbSubject.Sequence = subject.Sequence;
                    dbSubject.Isactive = true;
                    dbSubject.EditBy = auditlogin;
                    dbSubject.EditOn = DateTime.Now;
                    // Save in DB
                    DB.SaveChanges();

                    // Return
                    sr.ReturnId = dbSubject.SubjectId;
                    sr.ReturnName = dbSubject.SubjectName;

                    return sr;
                }
            }
            return sr;
        }
        public static Subject Fetch(int? subjectId)
        {
          return  DB.Subjects
                   .Where(m => m.SubjectId == (subjectId.HasValue ? subjectId.Value:0))
                   .FirstOrDefault();
        }
        public static CollegeSubject FetchCollegeSubject(int? subjectId)
        {
            return DB.CollegeSubjects
                     .Where(m => m.SubjectId == (subjectId.HasValue ? subjectId.Value : 0))
                     .FirstOrDefault();
        }
        public static SchoolSubject FetchSchoolSubject(int? subjectId)
        {
            return DB.SchoolSubjects
                     .Where(m => m.SubjectId == (subjectId.HasValue ? subjectId.Value : 0))
                     .FirstOrDefault();
        }
        public static List<Subject> SubjectList(string Lang,bool IsActive)
        {
            return DB.Subjects
                  .Where(m => m.LanguageCode == Lang && m.Isactive == IsActive)
                  .ToList();
        }
        public static List<Subject> SubjectAllList(string Lang)
        {
            return DB.Subjects
                  .Where(m => m.LanguageCode == Lang )
                  .ToList();
        }
        public static List<GeneralInstruction> GeneralInstructionList(string Lang, bool IsActive)
        {
            return DB.GeneralInstructions
                  .Where(m => m.LanguageCode == Lang && m.Isactive == IsActive)
                  .ToList();
        }
        public static List<CollegeSubject> CollegeSubjectList(string Lang, bool IsActive)
        {
            return DB.CollegeSubjects
                  .Where(m => m.LanguageCode == Lang && m.Isactive == IsActive)
                  .ToList();
        }
        public static List<CollegeSubject> CollegeAllSubjectList(string Lang)
        {
            return DB.CollegeSubjects
                  .Where(m => m.LanguageCode == Lang)
                  .ToList();
        }
        public static List<SchoolSubject> SchoolSubjectList(string Lang, bool IsActive)
        {
            return DB.SchoolSubjects
                  .Where(m => m.LanguageCode == Lang && m.Isactive == IsActive)
                  .ToList();
        }
        public static List<SchoolSubject> SchoolAllSubjectList(string Lang)
        {
            return DB.SchoolSubjects
                  .Where(m => m.LanguageCode == Lang )
                  .ToList();
        }
        public static ServiceResponse DeleteSubject(int SubjectId)
        {
            var sr = new ServiceResponse();

            try
            {

                if (!QuestionUploadService.QuestionUploadList("en-US", true).Any(m => m.SubjectId == SubjectId))
                {
                    var Subject = Fetch(SubjectId);

                    DB.Entry(Subject).State = EntityState.Deleted;
                    DB.SaveChanges();
                }
                else
                {
                    var Qustions = QuestionUploadService.QuestionUploadList("en-US", true).Where(m => m.SubjectId == SubjectId).ToList();
                    sr.AddError($"You can't delete this Subject as it is being used in Question uploads.");

                }

            }
            catch (Exception exception)
            {
                sr.AddError(exception.Message);
            }

            return sr;
        }
        public static ServiceResponse DeleteCollegeSubject(int SubjectId)
        {
            var sr = new ServiceResponse();

            try
            {
                var Subject = FetchCollegeSubject(SubjectId);

                DB.Entry(Subject).State = EntityState.Deleted;
                DB.SaveChanges();
            }
            catch (Exception exception)
            {
                sr.AddError(exception.Message);
            }

            return sr;
        }
        public static ServiceResponse DeleteSchoolSubject(int SubjectId)
        {
            var sr = new ServiceResponse();

            try
            {
                var Subject = FetchSchoolSubject(SubjectId);

                DB.Entry(Subject).State = EntityState.Deleted;
                DB.SaveChanges();
            }
            catch (Exception exception)
            {
                sr.AddError(exception.Message);
            }

            return sr;
        }
    }
}