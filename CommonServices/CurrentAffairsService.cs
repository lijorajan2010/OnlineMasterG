using OnlineMasterG.Base;
using OnlineMasterG.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineMasterG.CommonServices
{
    public class CurrentAffairsService : ServiceBase
    {
       
        internal static List<CurrentAffairsCategory> CurrentAffairsCategoryList(string Lang, bool IsActive)
        {
            return DB.CurrentAffairsCategories.Where(m => m.LanguageCode == Lang && m.Isactive == IsActive).ToList();
        }
        internal static List<CurrentAffairsCategory> CurrentAffairsAllCategoryList(string Lang)
        {
            return DB.CurrentAffairsCategories.Where(m => m.LanguageCode == Lang ).ToList();
        }
        internal static List<CurrentAffairsUpload> CurrentAffairsUploadsList(string Lang, bool IsActive)
        {
            return DB.CurrentAffairsUploads.Where(m => m.LanguageCode == Lang && m.Isactive == IsActive).ToList();
        }

        public static CurrentAffairsCategory Fetch(int? CategoryId)
        {
            return DB.CurrentAffairsCategories.Where(m => m.CurrentAffairsCategoryId == (CategoryId.HasValue ? CategoryId.Value : 0)).FirstOrDefault();
        }

        public static CurrentAffairsUpload FetchUpload(int? UploadId)
        {
            return DB.CurrentAffairsUploads.Where(m => m.CurrentAffairsUploadId == (UploadId.HasValue ? UploadId.Value : 0)).FirstOrDefault();
        }


        internal static ServiceResponse DeleteCurrentAffairsCategory(int categoryId)
        {
            var sr = new ServiceResponse();

            try
            {
                var category = Fetch(categoryId);

                DB.Entry(category).State = EntityState.Deleted;
                DB.SaveChanges();
            }
            catch (Exception exception)
            {
                sr.AddError(exception.Message);
            }

            return sr;
        }
        internal static ServiceResponse DeleteCurrentAffairsUpload (int UploadId)
        {
            var sr = new ServiceResponse();

            try
            {
                var currentAffairs = FetchUpload(UploadId);

                DB.Entry(currentAffairs).State = EntityState.Deleted;
                DB.SaveChanges();
            }
            catch (Exception exception)
            {
                sr.AddError(exception.Message);
            }

            return sr;
        }

        internal static ServiceResponse SaveCurrentAffairs(CurrentAffairsCategory category, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            if (category.CurrentAffairsCategoryId == 0)
            {
                DB.CurrentAffairsCategories.Add(category);
                DB.SaveChanges();
            }
            else
            {
                var dbCurrentAffairsCategory = Fetch(category.CurrentAffairsCategoryId);
                if (dbCurrentAffairsCategory == null)
                {
                    sr.AddError($"CategoryId for {category.CurrentAffairsCategoryName} was not found.");
                    return sr;
                }
                else
                {
                    dbCurrentAffairsCategory.CurrentAffairsCategoryName = category.CurrentAffairsCategoryName;
                    dbCurrentAffairsCategory.LanguageCode = category.LanguageCode;
                    dbCurrentAffairsCategory.Sequence = category.Sequence;
                    dbCurrentAffairsCategory.Isactive = category.Isactive;
                    dbCurrentAffairsCategory.EditBy = auditlogin;
                    dbCurrentAffairsCategory.EditOn = DateTime.Now;
                    // Save in DB
                    DB.SaveChanges();

                    // Return
                    sr.ReturnId = dbCurrentAffairsCategory.CurrentAffairsCategoryId;
                    sr.ReturnName = dbCurrentAffairsCategory.CurrentAffairsCategoryName;

                    return sr;
                }
            }
            return sr;
        }

        internal static ServiceResponse SaveCurrentAffairsUpload(CurrentAffairsUpload currentUploads, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            if (currentUploads.CurrentAffairsUploadId == 0)
            {
                DB.CurrentAffairsUploads.Add(currentUploads);
                DB.SaveChanges();
            }
            else
            {
                var dbCurrentAffairsUpload = FetchUpload(currentUploads.CurrentAffairsUploadId);
                if (dbCurrentAffairsUpload == null)
                {
                    sr.AddError($"CurrentAffairsUploadId for {currentUploads.UploadDate} was not found.");
                    return sr;
                }
                else
                {
                    dbCurrentAffairsUpload.CurrentAffairsCategoryId = currentUploads.CurrentAffairsCategoryId;
                    dbCurrentAffairsUpload.LanguageCode = currentUploads.LanguageCode;
                    dbCurrentAffairsUpload.UploadDate = currentUploads.UploadDate;
                    dbCurrentAffairsUpload.EditBy = auditlogin;
                    dbCurrentAffairsUpload.EditOn = DateTime.Now;
                    // Save in DB
                    DB.SaveChanges();

                    // Return
                    sr.ReturnId = dbCurrentAffairsUpload.CurrentAffairsUploadId;
                    sr.ReturnName = dbCurrentAffairsUpload.UploadDate.ToString();

                    return sr;
                }
            }
            return sr;
        }
    }
}