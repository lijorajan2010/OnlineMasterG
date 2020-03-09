using OnlineMasterG.Base;
using OnlineMasterG.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineMasterG.CommonServices
{
    public static class CategoryService
    {
        private static OnlinemasterjiEntities DB = new OnlinemasterjiEntities();
        public static ServiceResponse SaveCategory(Category category, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            if (category.CategoryId == 0)
            {
                DB.Categories.Add(category);
                DB.SaveChanges();
            }
            else
            {
                var dbCategory = Fetch(category.CategoryId);
                if (dbCategory == null)
                {
                    sr.AddError($"CategoryName {category.CategoryName} is not found.");
                    return sr;
                }
                else
                {
                    dbCategory.CategoryName = category.CategoryName;
                    dbCategory.CourseId = category.CourseId;
                    dbCategory.LanguageCode = category.LanguageCode;
                    dbCategory.Sequence = category.Sequence;
                    dbCategory.Isactive = category.Isactive;
                    dbCategory.EditBy = auditlogin;
                    dbCategory.EditOn = DateTime.Now;
                    // Save in DB
                    DB.SaveChanges();

                    // Return
                    sr.ReturnId = dbCategory.CategoryId;
                    sr.ReturnName = dbCategory.CategoryName;

                    return sr;
                }
            }
            return sr;
        }
        public static Category Fetch(int? categoryId)
        {
          return  DB.Categories
                   .Where(m => m.CategoryId == (categoryId.HasValue?categoryId.Value:0))
                   .FirstOrDefault();
        }
        public static List<Category> CategoryList(string Lang,bool IsActive)
        {
            return DB.Categories
                  .Where(m => m.LanguageCode == Lang && m.Isactive == IsActive)
                  .ToList();
        }
        public static ServiceResponse DeleteCategory(int CategoryId)
        {
            var sr = new ServiceResponse();

            try
            {
                var Category = Fetch(CategoryId);

                DB.Entry(Category).State = EntityState.Deleted;
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