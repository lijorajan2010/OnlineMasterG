﻿using OnlineMasterG.Base;
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
        public static Course Fetch(int? courseId)
        {
          return  DB.Courses
                   .Where(m => m.CourseId == (courseId.HasValue ? courseId.Value :0))
                   .FirstOrDefault();
        }
        public static List<Course> CourseList(string Lang,bool IsActive)
        {
            return DB.Courses
                  .Where(m => m.LanguageCode == Lang && m.Isactive == IsActive)
                  .ToList();
        }
        public static ServiceResponse DeleteCourse(int courseId)
        {
            var sr = new ServiceResponse();

            try
            {
                var course = Fetch(courseId);

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