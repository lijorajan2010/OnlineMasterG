using OnlineMasterG.Base;
using OnlineMasterG.Models.DAL;
using OnlineMasterG.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineMasterG.CommonServices
{
    public static class DailyQuizService
    {
        private static OnlinemasterjiEntities DB = new OnlinemasterjiEntities();
        internal static List<DailyQuizCourse> DailyQuizCourseList(string Lang, bool isActive)
        {
            return DB.DailyQuizCourses.Where(m => m.LanguageCode == Lang && m.Isactive == isActive).ToList();
        }
    
        internal static DailyQuizCourse Fetch(int? DailyQuizCourseId)
        {
            return DB.DailyQuizCourses.Where(m => m.DailyQuizCourseId == (DailyQuizCourseId.HasValue? DailyQuizCourseId.Value:0)).FirstOrDefault();
        }

        internal static DailyQuizSubject FetchDailyQuizSubject(int? DailyQuizSubjectId)
        {
            return DB.DailyQuizSubjects.Where(m => m.DailyQuizSubjectId == (DailyQuizSubjectId.HasValue ? DailyQuizSubjectId.Value : 0)).FirstOrDefault();
        }

        public static ServiceResponse SaveDailyQuizCourse(DailyQuizCourse course, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            if (course.DailyQuizCourseId == 0)
            {
                DB.DailyQuizCourses.Add(course);
                DB.SaveChanges();
            }
            else
            {
                var dbCourse = Fetch(course.DailyQuizCourseId);
                if (dbCourse == null)
                {
                    sr.AddError($"CourseId for {course.DailyQuizCourseName} was not found.");
                    return sr;
                }
                else
                {
                    dbCourse.DailyQuizCourseName = course.DailyQuizCourseName;
                    dbCourse.LanguageCode = course.LanguageCode;
                    dbCourse.Sequence = course.Sequence;
                    dbCourse.Isactive = course.Isactive;
                    dbCourse.EditBy = auditlogin;
                    dbCourse.EditOn = DateTime.Now;
                    // Save in DB
                    DB.SaveChanges();

                    // Return
                    sr.ReturnId = dbCourse.DailyQuizCourseId;
                    sr.ReturnName = dbCourse.DailyQuizCourseName;

                    return sr;
                }
            }
            return sr;
        }

        public static ServiceResponse SaveDailyQuizSubject(DailyQuizSubject subjects, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            if (subjects.DailyQuizSubjectId == 0)
            {
                DB.DailyQuizSubjects.Add(subjects);
                DB.SaveChanges();
            }
            else
            {
                var dbSubject = FetchDailyQuizSubject(subjects.DailyQuizCourseId);
                if (dbSubject == null)
                {
                    sr.AddError($"SubjectId for {subjects.DailyQuizSubjectName} was not found.");
                    return sr;
                }
                else
                {
                    dbSubject.DailyQuizSubjectName = subjects.DailyQuizSubjectName;
                    dbSubject.LanguageCode = subjects.LanguageCode;
                    dbSubject.Isactive = subjects.Isactive;
                    dbSubject.EditBy = auditlogin;
                    dbSubject.EditOn = DateTime.Now;
                    // Save in DB
                    DB.SaveChanges();

                    // Return
                    sr.ReturnId = dbSubject.DailyQuizSubjectId;
                    sr.ReturnName = dbSubject.DailyQuizSubjectName;

                    return sr;
                }
            }
            return sr;
        }
        public static List<DailyQuizSubject> DailyQuizSubjectList(string Lang, bool IsActive)
        {
            return DB.DailyQuizSubjects
                  .Where(m => m.LanguageCode == Lang && m.Isactive == IsActive)
                  .ToList();
        }

        public static List<DailyQuiz> DailyQuizList(string Lang, bool IsActive)
        {
            return DB.DailyQuizs
                  .Where(m => m.LanguageCode == Lang && m.Isactive == IsActive)
                  .ToList();
        }


        public static DailyQuizSubject FetchDailyQuizSubject(int subjectId)
        {
            return DB.DailyQuizSubjects
                  .Where(m => m.DailyQuizSubjectId == subjectId ).FirstOrDefault();

        }
        public static ServiceResponse DeleteDailyQuizCourse(int courseId)
        {
            var sr = new ServiceResponse();

            try
            {
                if (!DailyQuizSubjectList("en-US", true).Any(m => m.DailyQuizCourseId == courseId))
                {
                    var course = Fetch(courseId);
                    DB.Entry(course).State = EntityState.Deleted;
                    DB.SaveChanges();
                }
                else
                {
                    var categoriesused = DailyQuizSubjectList("en-US", true).Where(m => m.DailyQuizCourseId == courseId).Select(m => m.DailyQuizSubjectName).ToList();
                    sr.AddError($"You can't delete this course as it is being used by courses such as { string.Join(",", categoriesused)}. If you want to delete, please delete these subjects first.");
                }

            }
            catch (Exception exception)
            {
                sr.AddError(exception.Message);
            }

            return sr;
        }
        public static ServiceResponse DeleteDailyQuizSubject(int subjectId)
        {
            var sr = new ServiceResponse();

            try
            {
                if (!DailyQuizList("en-US", true).Any(m => m.DailyQuizSubjectId == subjectId))
                {
                    var subject = FetchDailyQuizSubject(subjectId);
                    DB.Entry(subject).State = EntityState.Deleted;
                    DB.SaveChanges();
                }
                else
                {
                    var quizused = DailyQuizList("en-US", true).Where(m => m.DailyQuizSubjectId == subjectId).Select(m => m.DailyQuizName).ToList();
                    sr.AddError($"You can't delete this subject as it is being used by quiz such as { string.Join(",", quizused)}. If you want to delete, please delete these quizes first.");
                }

            }
            catch (Exception exception)
            {
                sr.AddError(exception.Message);
            }

            return sr;
        }

    }
}