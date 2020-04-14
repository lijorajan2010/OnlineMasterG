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
    public static class CurrentAffairsLogics
    {
        public static ServiceResponse ValidateCurrentAffairsCategory(CurrentAffairsVM model)
        {
            ServiceResponse sr = new ServiceResponse();

            if (String.IsNullOrEmpty(model.AffairsCategoryName))
                sr.AddError("The [Category Name] field cannot be empty.");

            return sr;
        }
        public static ServiceResponse ValidateCurrentAffairsUpload(CurrentAffairsUploadVM model)
        {
            ServiceResponse sr = new ServiceResponse();

            if (model.UploadDate == null)
                sr.AddError("The [Upload] field cannot be empty.");
            if (model.DataFileId == null || model.DataFileId == 0)
                sr.AddError("Please upload pdf file");

            return sr;
        }
        public static ServiceResponse SaveCurrentAffairsCategory(CurrentAffairsVM model, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            CurrentAffairsCategory course = new CurrentAffairsCategory()
            {
                CurrentAffairsCategoryId = model.CurrentAffairsCategoryId,
                CurrentAffairsCategoryName = model.AffairsCategoryName,
                Sequence = model.Sequence,
                LanguageCode = model.LanguageCode,
                Isactive = model.IsActive,
                CreateBy = auditlogin,
                CreateOn = DateTime.Now
            };
            sr = CurrentAffairsService.SaveCurrentAffairs(course, auditlogin);

            return sr;
        }

        public static ServiceResponse SaveCurrentAffairsUpload(CurrentAffairsUploadVM model, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            CurrentAffairsUpload currentUploads = new CurrentAffairsUpload()
            {
                CurrentAffairsUploadId = model.CurrentAffairsUploadId,
                CurrentAffairsCategoryId= model.CurrentAffairsCategoryId,
                UploadDate = model.UploadDate,
                LanguageCode = model.LanguageCode,
                DataFileId = model.DataFileId,
                CreateBy = auditlogin,
                CreateOn = DateTime.Now,
                Isactive = true
            };
            sr = CurrentAffairsService.SaveCurrentAffairsUpload(currentUploads, auditlogin);

            return sr;
        }
        public static List<CurrentAffairsUploadVM> CurrentAffairsUploadList(string Lang, bool IsActive)
        {
            List<CurrentAffairsUploadVM> model = new List<CurrentAffairsUploadVM>();
            var UploadList = CurrentAffairsService.CurrentAffairsUploadsList(Lang, IsActive);
            if (UploadList != null && UploadList.Count() > 0)
            {
                foreach (var item in UploadList)
                {

                    DataFileVM datafile = new DataFileVM();
                    if (item.DataFile != null)
                    {
                        datafile.DataFileId = item.DataFile.DataFileId;
                        datafile.FileName = item.DataFile.FileName;
                        datafile.Extension = item.DataFile.Extension;
                    }

                    model.Add(new CurrentAffairsUploadVM()
                    {
                        CurrentAffairsUploadId = item.CurrentAffairsUploadId,
                        CurrentAffairsCategoryId = item.CurrentAffairsCategoryId,
                        CurrentAffairsCategoryName = CurrentAffairsService.Fetch(item.CurrentAffairsCategoryId)?.CurrentAffairsCategoryName,
                        UploadDate = item.UploadDate,
                        LanguageCode = item.LanguageCode,
                        DataFileId = item.DataFileId,
                        DataFile = datafile,
                        CreateOn = item.CreateOn

                    });
                }
            }
            return model;
        }
       
    }
}