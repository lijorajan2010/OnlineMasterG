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
    public static class CategoryLogics
    {
        #region Validations

        public static ServiceResponse ValidateCategory(CategoryVM model)
        {
            ServiceResponse sr = new ServiceResponse();

            if (String.IsNullOrEmpty(model.CategoryName))
                sr.AddError("The [Category Name] field cannot be empty.");

            return sr;
        }
        public static ServiceResponse SaveCategory(CategoryVM model,string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            Category category = new Category()
            {
                CategoryId = model.CategoryId,
                CourseId = model.CourseId,
                CategoryName = model.CategoryName,
                Sequence = model.Sequence,
                LanguageCode = model.LanguageCode,
                Isactive = model.IsActive,
                CreateBy = auditlogin,
                CreateOn = DateTime.Now
            };
            sr = CategoryService.SaveCategory(category, auditlogin);

            return sr;
        }
        public static List<CategoryVM> CategoryList(string Lang)
        {
            List<CategoryVM> model = new List<CategoryVM>();
            var categories = CategoryService.CategoryAllList(Lang);
            if (categories!=null && categories.Count()>0)
            {
                foreach (var item in categories)
                {
                    model.Add(new CategoryVM()
                    {
                        CategoryId = item.CategoryId,
                        CategoryName = item.CategoryName,
                        CourseName = CourseService.Fetch(item.CourseId)?.CourseName,
                        CourseId = item.CourseId,
                        IsActive = item.Isactive,
                        Sequence=item.Sequence,
                        LanguageCode =item.LanguageCode,
                        CreateOn = item.CreateOn

                    });
                }
            }
            return model;
        }


        #endregion
    }
}