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
            if (model.DataFileId== null || model.DataFileId == 0)
                sr.AddError("Please upload pdf file");

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
        public static ServiceResponse SaveSchoolPaper(PaperVM model, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            SchoolPaper collegepaper = new SchoolPaper()
            {
                PaperId = model.PaperId,
                ClassId = model.ClassId,
                SubjectId = model.SubjectId,
                SectionId = model.SectionId,
                PaperName = model.PaperName,
                Description = model.Description,
                LanguageCode = model.LanguageCode,
                DataFileId = model.DataFileId,
                CreateBy = auditlogin,
                CreateOn = DateTime.Now,
                Isactive = true
            };
            sr = PaperService.SaveSchoolPaper(collegepaper, auditlogin);

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
                        DataFile = datafile,
                        CreateOn = item.CreateOn

                    });
                }
            }
            return model;
        }

        public static List<PaperVM> SchoolPaperList(string Lang, bool IsActive)
        {
            List<PaperVM> model = new List<PaperVM>();
            var schoolPaper = PaperService.SchoolPaperList(Lang, IsActive);
            if (schoolPaper != null && schoolPaper.Count() > 0)
            {
                foreach (var item in schoolPaper)
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
                        ClassId = item.ClassId,
                        SubjectId = item.SubjectId,
                        SectionId = item.SectionId,
                        PaperName = item.PaperName,
                        ClassName = ClassService.Fetch(item.ClassId)?.ClassName,
                        SubjectName = SubjectService.FetchSchoolSubject(item.SubjectId)?.SubjectName,
                        SectionName = SectionService.FetchSchoolSection(item.SectionId)?.SectionName,
                        Description = item.Description,
                        LanguageCode = item.LanguageCode,
                        DataFileId = item.DataFileId,
                        DataFile = datafile,
                        CreateOn = item.CreateOn

                    });
                }
            }
            return model;
        }


        #endregion
    }
}