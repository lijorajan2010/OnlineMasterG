using OnlineMasterG.CommonServices;
using OnlineMasterG.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using OnlineMasterG.Code;

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
            get
            {
                var CourseName = CourseService.CourseList("en-US", true).OrderBy(m => m.Sequence).Take(5).ToList();
                return CourseName.Select(m => m.CourseName).ToList();
            }

        }
        public static List<Course> GetCourseList
        {
            get
            {
                return CourseService.CourseList("en-US", true).OrderBy(m => m.Sequence).ToList();

            }

        }
        public static List<SchoolClass> GetClasses
        {
            get
            {
                return ClassService.SchoolClassList("en-US", true).OrderBy(m => m.Sequence).Distinct().ToList();

            }

        }
        public static List<Category> GetCategories
        {
            get
            {
                return CategoryService.CategoryList("en-US", true).OrderBy(m => m.Sequence).Distinct().ToList();

            }

        }
        public static List<SchoolSubject> GetSchoolSubjects
        {
            get
            {
                return SubjectService.SchoolSubjectList("en-US", true).OrderBy(m => m.Sequence).Distinct().ToList();
            }

        }

        public static List<Subject> GetSubjects
        {
            get
            {
                return SubjectService.SubjectList("en-US", true).OrderBy(m => m.Sequence).Distinct().ToList();
            }

        }
        public static List<SchoolSection> GetSchoolSections
        {
            get
            {
                return SectionService.SchoolSectionList("en-US", true).Distinct().ToList();

            }

        }
        public static List<Section> GetSections
        {
            get
            {
                return SectionService.SectionList("en-US", true).Distinct().ToList();

            }

        }
        public static List<CollegePaper> GetCollegePapers
        {
            get
            {
                return PaperService.CollegePaperList("en-US", true).Distinct().ToList();

            }

        }
        public static List<SchoolPaper> GetSchoolPapers
        {
            get
            {
                return PaperService.SchoolPaperList("en-US", true).Distinct().ToList();

            }
            set { }

        }
        public static List<DailyQuiz> GetDailyQuizs
        {
            get
            {
                return DailyQuizService.DailyQuizList("en-US", true).Distinct().ToList();

            }
            set { }

        }
        public static List<QuizTest> GetQuizTests
        {
            get
            {
                return DailyQuizService.GetQuizTestsList("en-US", true).Distinct().ToList();

            }
            set { }

        }
        public static List<MockTest> GetMockTests
        {
            get
            {
                return TestService.TestList("en-US", true).Distinct().ToList();

            }
            set { }

        }

        public static List<QuestionsMockTest> GetQuestionsMockTests
        {
            get
            {
                return QuestionUploadService.GetAllQuestions();

            }
            set { }

        }
        public static List<ColleageCourse> GetColleageCourses
        {
            get
            {
                return CourseService.CollegeCourseList("en-US", true).OrderBy(m => m.Sequence).Distinct().ToList();

            }
            set { }

        }
        public static List<DailyQuizCourse> GetDailyQuizCourses
        {
            get
            {
                return DailyQuizService.DailyQuizCourseList("en-US", true).OrderBy(m => m.Sequence).Distinct().ToList();

            }
            set { }

        }
        public static List<CurrentAffairsCategory> GetCurrentAffairsCategories
        {
            get
            {
                return CurrentAffairsService.CurrentAffairsCategoryList("en-US", true).OrderBy(m => m.Sequence).Distinct().ToList();

            }
            set { }

        }
        public static List<CurrentAffairsUpload> GetCurrentAffairsUploads
        {
            get
            {
                return CurrentAffairsService.CurrentAffairsUploadsList("en-US", true).Distinct().ToList();

            }
            set { }

        }
        public static List<ExamSection> GetExamSections
        {
            get
            {
                return ExamUpdateService.ExamSectionList("en-US", true).OrderBy(m => m.Sequence).Distinct().ToList();

            }
            set { }

        }
        public static List<CollegeSubject> GetCollegeSubjects
        {
            get
            {
                return SubjectService.CollegeSubjectList("en-US", true).OrderBy(m => m.Sequence).Distinct().ToList();

            }
            set { }

        }
        public static List<DailyQuizSubject> GetDailyQuizSubjects
        {
            get
            {
                return DailyQuizService.DailyQuizSubjectList("en-US", true).Distinct().ToList();

            }
            set { }

        }
        public static List<ExamSectionLink> GetExamSectionLinks
        {
            get
            {
                return ExamUpdateService.ExamLinksList("en-US", true).OrderBy(m => m.Sequence).Distinct().ToList();

            }
            set { }

        }
        public static List<LatestUpdate> GetLatestUpdates
        {
            get
            {
                return GeneralService.LatestUpdatesList("en-US", true).OrderBy(m => m.Sequence).Distinct().ToList();

            }
            set { }

        }

        public static ActiveUser ActiveUser
        {
            get
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated) return null;

                return new ActiveUser(HttpContext.Current.User.Identity.Name);
            }
        }

    }
}