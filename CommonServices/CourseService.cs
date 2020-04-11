using OnlineMasterG.Base;
using OnlineMasterG.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineMasterG.CommonServices
{
    public static class CourseService
    {
        private static OnlinemasterjiEntities DB = new OnlinemasterjiEntities();
        public static ServiceResponse SaveCourse(Course course, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            if (course.CourseId == 0)
            {
                DB.Courses.Add(course);
                DB.SaveChanges();
            }
            else
            {
                var dbCourse = Fetch(course.CourseId);
                if (dbCourse == null)
                {
                    sr.AddError($"CourseId for {course.CourseName} was not found.");
                    return sr;
                }
                else
                {
                    dbCourse.CourseName = course.CourseName;
                    dbCourse.LanguageCode = course.LanguageCode;
                    dbCourse.Sequence = course.Sequence;
                    dbCourse.Isactive = course.Isactive;
                    dbCourse.EditBy = auditlogin;
                    dbCourse.EditOn = DateTime.Now;
                    // Save in DB
                    DB.SaveChanges();

                    // Return
                    sr.ReturnId = dbCourse.CourseId;
                    sr.ReturnName = dbCourse.CourseName;

                    return sr;
                }
            }
            return sr;
        }
        public static ServiceResponse SaveCollegeCourse(ColleageCourse course, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            if (course.CourseId == 0)
            {
                DB.ColleageCourses.Add(course);
                DB.SaveChanges();
            }
            else
            {
                var dbCourse = FetchCollegeCourse(course.CourseId);
                if (dbCourse == null)
                {
                    sr.AddError($"CourseId for {course.CourseName} was not found.");
                    return sr;
                }
                else
                {
                    dbCourse.CourseName = course.CourseName;
                    dbCourse.LanguageCode = course.LanguageCode;
                    dbCourse.Sequence = course.Sequence;
                    dbCourse.Isactive = course.Isactive;
                    dbCourse.EditBy = auditlogin;
                    dbCourse.EditOn = DateTime.Now;
                    // Save in DB
                    DB.SaveChanges();

                    // Return
                    sr.ReturnId = dbCourse.CourseId;
                    sr.ReturnName = dbCourse.CourseName;

                    return sr;
                }
            }
            return sr;
        }
        public static Course Fetch(int? courseId)
        {
          return  DB.Courses
                   .Where(m => m.CourseId == (courseId.HasValue ? courseId.Value :0))
                   .FirstOrDefault();
        }
        public static ColleageCourse FetchCollegeCourse(int? courseId)
        {
            return DB.ColleageCourses
                     .Where(m => m.CourseId == (courseId.HasValue ? courseId.Value : 0))
                     .FirstOrDefault();
        }
        public static List<Course> CourseList(string Lang,bool IsActive)
        {
            return DB.Courses
                  .Where(m => m.LanguageCode == Lang && m.Isactive == IsActive)
                  .ToList();
        }
        public static List<Course> CourseAllList(string Lang)
        {
            return DB.Courses
                  .Where(m => m.LanguageCode == Lang)
                  .ToList();
        }
        public static List<ColleageCourse> CollegeCourseList(string Lang, bool IsActive)
        {
            return DB.ColleageCourses
                  .Where(m => m.LanguageCode == Lang && m.Isactive == IsActive)
                  .ToList();
        }
        public static List<ColleageCourse> CollegeAllCourseList(string Lang)
        {
            return DB.ColleageCourses
                  .Where(m => m.LanguageCode == Lang)
                  .ToList();
        }
        public static ServiceResponse DeleteCourse(int courseId)
        {
            var sr = new ServiceResponse();

            try
            {
                if (!CategoryService.CategoryList("en-US", true).Any(m => m.CourseId == courseId))
                {
                    var course = Fetch(courseId);
                    DB.Entry(course).State = EntityState.Deleted;
                    DB.SaveChanges();
                }
                else
                {
                    var categoriesused = CategoryService.CategoryList("en-US", true).Where(m => m.CourseId == courseId).Select(m => m.CategoryName).ToList();
                    sr.AddError($"You can't delete this course as it is being used by categories such as { string.Join(",", categoriesused)}. If you want to delete, please delete these categories first.");
                }

            }
            catch (Exception exception)
            {
                sr.AddError(exception.Message);
            }

            return sr;
        }
        public static ServiceResponse DeleteCollegeCourse(int courseId)
        {
            var sr = new ServiceResponse();

            try
            {
                var course = FetchCollegeCourse(courseId);

                DB.Entry(course).State = EntityState.Deleted;
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