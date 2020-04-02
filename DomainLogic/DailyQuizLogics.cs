using OnlineMasterG.Base;
using OnlineMasterG.CommonServices;
using OnlineMasterG.Models.DAL;
using OnlineMasterG.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.DomainLogic
{
    public static class DailyQuizLogics
    {
        #region Validations

        public static ServiceResponse ValidateDailyQuizCourse(DailyQuizCourseVM model)
        {
            ServiceResponse sr = new ServiceResponse();

            if (String.IsNullOrEmpty(model.DailyQuizCourseName))
                sr.AddError("The [Course Name] field cannot be empty.");

            return sr;
        }
        public static ServiceResponse SaveDailyQuizCourse(DailyQuizCourseVM model, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            DailyQuizCourse course = new DailyQuizCourse()
            {
                DailyQuizCourseId = model.DailyQuizCourseId,
                DailyQuizCourseName = model.DailyQuizCourseName,
                Sequence = model.Sequence,
                LanguageCode = model.LanguageCode,
                Isactive = model.IsActive,
                CreateBy = auditlogin,
                CreateOn = DateTime.Now
            };
            sr = DailyQuizService.SaveDailyQuizCourse(course, auditlogin);

            return sr;
        }
        public static ServiceResponse ValidateDailyQuizSubject(DailyQuizSubjectVM model)
        {
            ServiceResponse sr = new ServiceResponse();

            if (String.IsNullOrEmpty(model.DailyQuizSubjectName))
                sr.AddError("The [Subject Name] field cannot be empty.");

            return sr;
        }
        public static ServiceResponse SaveDailyQuizSubject(DailyQuizSubjectVM model, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            DailyQuizSubject dailyQuizSubject = new DailyQuizSubject()
            {
                DailyQuizSubjectId = model.DailyQuizSubjectId,
                DailyQuizSubjectName = model.DailyQuizSubjectName,
                DailyQuizCourseId = model.DailyQuizCourseId,
                LanguageCode = model.LanguageCode,
                Isactive = model.IsActive,
                CreateBy = auditlogin,
                CreateOn = DateTime.Now
            };
            sr = DailyQuizService.SaveDailyQuizSubject(dailyQuizSubject, auditlogin);

            return sr;
        }
        public static List<DailyQuizSubjectVM> SubjectList(string Lang, bool IsActive)
        {
            List<DailyQuizSubjectVM> model = new List<DailyQuizSubjectVM>();
            var courses = DailyQuizService.DailyQuizSubjectList(Lang, IsActive);
            if (courses != null && courses.Count() > 0)
            {
                foreach (var item in courses)
                {
                    model.Add(new DailyQuizSubjectVM()
                    {
                        DailyQuizSubjectId = item.DailyQuizSubjectId,
                        DailyQuizSubjectName = item.DailyQuizSubjectName,
                        DailyQuizCourseName = DailyQuizService.Fetch(item.DailyQuizCourseId)?.DailyQuizCourseName,
                        DailyQuizCourseId = item.DailyQuizCourseId,
                        IsActive = item.Isactive,
                        LanguageCode = item.LanguageCode

                    });
                }
            }
            return model;
        }

        #endregion
    }
}