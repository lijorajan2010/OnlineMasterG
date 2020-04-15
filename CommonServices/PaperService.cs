using OnlineMasterG.Base;
using OnlineMasterG.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineMasterG.CommonServices
{
    public static class PaperService
    {
        public static OnlinemasterjiEntities DB { get; private set; }
        static PaperService()
        {
            DB = new OnlinemasterjiEntities();
        }
        public static ServiceResponse SaveCollegePaper(CollegePaper model, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            if (model.PaperId == 0)
            {
                DB.CollegePapers.Add(model);
                DB.SaveChanges();
            }
            else
            {
                var dbSection = Fetch(model.PaperId);
                if (dbSection == null)
                {
                    sr.AddError($"Paper Name {model.PaperName} is not found.");
                    return sr;
                }
                else
                {
                    dbSection.PaperName = model.PaperName;
                    dbSection.CourseId = model.CourseId;
                    dbSection.SubjectId = model.SubjectId;
                    dbSection.DataFileId = model.DataFileId;
                    dbSection.LanguageCode = model.LanguageCode;
                    dbSection.Description = model.Description; 
                    dbSection.EditBy = auditlogin;
                    dbSection.EditOn = DateTime.Now;
                    // Save in DB
                    DB.SaveChanges();

                    // Return
                    sr.ReturnId = dbSection.PaperId;
                    sr.ReturnName = dbSection.PaperName;

                    return sr;
                }
            }
            return sr;
        }
        public static ServiceResponse SaveSchoolPaper(SchoolPaper model, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            if (model.PaperId == 0)
            {
                DB.SchoolPapers.Add(model);
                DB.SaveChanges();
            }
            else
            {
                var dbSection = FetchSchoolPaper(model.PaperId);
                if (dbSection == null)
                {
                    sr.AddError($"Paper Name {model.PaperName} is not found.");
                    return sr;
                }
                else
                {
                    dbSection.PaperName = model.PaperName;
                    dbSection.ClassId = model.ClassId;
                    dbSection.SubjectId = model.SubjectId;
                    dbSection.SectionId = model.SectionId;
                    dbSection.DataFileId = model.DataFileId;
                    dbSection.LanguageCode = model.LanguageCode;
                    dbSection.Description = model.Description;
                    dbSection.EditBy = auditlogin;
                    dbSection.EditOn = DateTime.Now;
                    // Save in DB
                    DB.SaveChanges();

                    // Return
                    sr.ReturnId = dbSection.PaperId;
                    sr.ReturnName = dbSection.PaperName;

                    return sr;
                }
            }
            return sr;
        }
        public static CollegePaper Fetch(int? paperId)
        {
          return  DB.CollegePapers
                   .Where(m => m.PaperId == (paperId.HasValue ? paperId.Value:0))
                   .FirstOrDefault();
        }
        public static SchoolPaper FetchSchoolPaper(int? paperId)
        {
            return DB.SchoolPapers
                     .Where(m => m.PaperId == (paperId.HasValue ? paperId.Value : 0))
                     .FirstOrDefault();
        }
        public static List<CollegePaper> CollegePaperList(string Lang,bool IsActive)
        {
            return DB.CollegePapers
                  .Where(m => m.LanguageCode == Lang && m.Isactive == IsActive)
                  .ToList();
        }
        public static List<SchoolPaper> SchoolPaperList(string Lang, bool IsActive)
        {
            return DB.SchoolPapers
                  .Where(m => m.LanguageCode == Lang && m.Isactive == IsActive)
                  .ToList();
        }
        public static ServiceResponse DeleteCollegePaper(int PaperId)
        {
            var sr = new ServiceResponse();

            try
            {
                var collegePaper = Fetch(PaperId);

                DB.Entry(collegePaper).State = EntityState.Deleted;
                DB.SaveChanges();
            }
            catch (Exception exception)
            {
                sr.AddError(exception.Message);
            }

            return sr;
        }
        public static ServiceResponse DeleteSchoolPaper(int PaperId)
        {
            var sr = new ServiceResponse();

            try
            {
                var schoolPaper = FetchSchoolPaper(PaperId);

                DB.Entry(schoolPaper).State = EntityState.Deleted;
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