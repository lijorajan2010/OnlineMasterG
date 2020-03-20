using OnlineMasterG.CommonServices;
using OnlineMasterG.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace OnlineMasterG.CommonFramework
{
    public static class AppInfo
    {
        public static string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static string GetCookieCulture
        {
            get
            {
                var langCookie = HttpContext.Current.Request.Cookies["lang"];

                if (langCookie == null)
                {
                    var cookie = new HttpCookie("lang", "en-US") { HttpOnly = true };
                    HttpContext.Current.Response.AppendCookie(cookie);
                }

                return langCookie == null ? "en-US" : langCookie.Value;
            }
        }
     
        public static List<string> GetCourses
        {
            get{ 
                var CourseName = CourseService.CourseList("en-US", true).OrderBy(m => m.Sequence).Take(5).ToList();
                return CourseName.Select(m => m.CourseName).ToList();
            }
        
        }
        public static List<SchoolClass> GetClasses
        {
            get
            {
                return ClassService.SchoolClassList("en-US", true).OrderBy(m => m.Sequence).ToList();
            
            }

        }
        public static List<SchoolSubject> GetSchoolSubjects
        {
            get
            {
                return SubjectService.SchoolSubjectList("en-US", true).OrderBy(m => m.Sequence).ToList();
            }

        }
        public static List<SchoolSection> GetSchoolSections
        {
            get
            {
               return SectionService.SchoolSectionList("en-US", true).ToList();
               
            }

        }
        public static List<SchoolPaper> GetSchoolPapers
        {
            get
            {
                return PaperService.SchoolPaperList("en-US", true).ToList();
                
            }
            set { }

        }

    }
}