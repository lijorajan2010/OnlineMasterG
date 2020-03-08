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
    public static class CourseLogics
    {
        #region Validations

        public static ServiceResponse ValidateCourse(CourseVM model)
        {
            ServiceResponse sr = new ServiceResponse();

            if (String.IsNullOrEmpty(model.CourseName))
                sr.AddError("The [Course Name] field cannot be empty.");

            return sr;
        }
        public static ServiceResponse SaveCourse(CourseVM model,string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            Course course = new Course()
            {
                CourseId = model.CourseId,
                CourseName = model.CourseName,
                Sequence = model.Sequence,
                LanguageCode = model.LanguageCode,
                Isactive = model.IsActive,
                CreateBy = auditlogin,
                CreateOn = DateTime.Now
            };
            sr = CourseService.SaveCourse(course,auditlogin);

            return sr;
        }

        #endregion
    }
}