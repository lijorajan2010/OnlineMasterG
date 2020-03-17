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
    public static class ClassLogics
    {
        #region Validations

        public static ServiceResponse ValidateClass(ClassVM model)
        {
            ServiceResponse sr = new ServiceResponse();

            if (String.IsNullOrEmpty(model.ClassName))
                sr.AddError("The [Class Name] field cannot be empty.");

            return sr;
        }
      
        public static ServiceResponse SaveSchoolClass(ClassVM model, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            SchoolClass schclass = new SchoolClass()
            {
                ClassId = model.ClassId,
                ClassName = model.ClassName,
                Sequence = model.Sequence,
                LanguageCode = model.LanguageCode,
                Isactive = model.IsActive,
                CreateBy = auditlogin,
                CreateOn = DateTime.Now
            };
            sr = ClassService.SaveSchoolClass(schclass, auditlogin);

            return sr;
        }

        #endregion
    }
}