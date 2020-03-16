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
    public static class PaperLogics
    {
        #region Validations

        public static ServiceResponse ValidatePaper(PaperVM model)
        {
            ServiceResponse sr = new ServiceResponse();

            if (String.IsNullOrEmpty(model.PaperName))
                sr.AddError("The [Paper Name] field cannot be empty.");

            return sr;
        }
       
        public static ServiceResponse SaveCollegePaper(PaperVM model, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            CollegePaper collegepaper = new CollegePaper()
            {
                PaperId = model.PaperId,
                CourseId = model.CourseId,
                SubjectId = model.SubjectId,
                PaperName = model.PaperName,
                Description = model.Description,
                LanguageCode = model.LanguageCode,
                DataFileId = model.DataFileId,
                CreateBy = auditlogin,
                CreateOn = DateTime.Now,
                Isactive = true
            };
            sr = PaperService.SaveCollegePaper(collegepaper, auditlogin);

            return sr;
        }
        public static List<PaperVM> CollegePaperList(string Lang, bool IsActive)
        {
            List<PaperVM> model = new List<PaperVM>();
            var collegepapr = PaperService.CollegePaperList(Lang, IsActive);
            if (collegepapr != null && collegepapr.Count() > 0)
            {
                foreach (var item in collegepapr)
                {

                    DataFileVM datafile = new DataFileVM();
                    if (item.DataFile != null)
                    {
                        datafile.DataFileId = item.DataFile.DataFileId;
                        datafile.FileName = item.DataFile.FileName;
                        datafile.Extension = item.DataFile.Extension;
                    }
                   
                    model.Add(new PaperVM()
                    {
                        PaperId = item.PaperId,
                        CourseId = item.CourseId,
                        SubjectId = item.SubjectId,
                        PaperName = item.PaperName,
                        CourseName = CourseService.FetchCollegeCourse(item.CourseId)?.CourseName,
                        SubjectName = SubjectService.FetchCollegeSubject(item.SubjectId)?.SubjectName,
                        Description = item.Description,
                        LanguageCode = item.LanguageCode,
                        DataFileId = item.DataFileId,
                        DataFile = datafile

                    });
                }
            }
            return model;
        }


        #endregion
    }
}