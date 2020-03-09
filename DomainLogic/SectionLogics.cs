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
    public static class SectionLogics
    {
        #region Validations

        public static ServiceResponse ValidateSection(SectionVM model)
        {
            ServiceResponse sr = new ServiceResponse();

            if (String.IsNullOrEmpty(model.SectionName))
                sr.AddError("The [Section Name] field cannot be empty.");

            return sr;
        }
        public static ServiceResponse SaveSection(SectionVM model,string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            Section section = new Section()
            {
                SectionId = model.SectionId,
                CategoryId = model.CategoryId,
                CourseId = model.CourseId,
                SectionName = model.SectionName,
                Description = model.Description,
                LanguageCode = model.LanguageCode,
                CreateBy = auditlogin,
                CreateOn = DateTime.Now,
                Isactive = true
            };
            sr = SectionService.SaveSection(section, auditlogin);

            return sr;
        }
        public static List<SectionVM> SectionList(string Lang, bool IsActive)
        {
            List<SectionVM> model = new List<SectionVM>();
            var sections = SectionService.SectionList(Lang, IsActive);
            if (sections != null && sections.Count()>0)
            {
                foreach (var item in sections)
                {
                    model.Add(new SectionVM()
                    {
                        SectionId = item.SectionId,
                        CourseId = item.CourseId,
                        CategoryId = item.CategoryId,
                        SectionName = item.SectionName,
                        CourseName = CourseService.Fetch(item.CourseId).CourseName,
                        CategoryName = CategoryService.Fetch(item.CategoryId).CategoryName,
                        Description=item.Description,
                        LanguageCode =item.LanguageCode

                    });
                }
            }
            return model;
        }


        #endregion
    }
}