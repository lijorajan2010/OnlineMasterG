using OnlineMasterG.Base;
using OnlineMasterG.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineMasterG.CommonServices
{

    public static class ExamUpdateService
    {
        private static OnlinemasterjiEntities DB = new OnlinemasterjiEntities();

        public static ServiceResponse SaveExamSection(ExamSection examSection, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            if (examSection.SectionId == 0)
            {
                DB.ExamSections.Add(examSection);
                DB.SaveChanges();
            }
            else
            {
                var dbexamSection = Fetch(examSection.SectionId);
                if (dbexamSection == null)
                {
                    sr.AddError($"SectionId for {examSection.SectionName} was not found.");
                    return sr;
                }
                else
                {
                    dbexamSection.SectionName = examSection.SectionName;
                    dbexamSection.LanguageCode = examSection.LanguageCode;
                    dbexamSection.Sequence = examSection.Sequence;
                    dbexamSection.Isactive = examSection.Isactive;
                    dbexamSection.EditBy = auditlogin;
                    dbexamSection.EditOn = DateTime.Now;
                    // Save in DB
                    DB.SaveChanges();

                    // Return
                    sr.ReturnId = dbexamSection.SectionId;
                    sr.ReturnName = dbexamSection.SectionName;

                    return sr;
                }
            }
            return sr;
        }
        public static ServiceResponse SaveExamSectionLink(ExamSectionLink examSectionlink, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            if (examSectionlink.LinkId == 0)
            {
                DB.ExamSectionLinks.Add(examSectionlink);
                DB.SaveChanges();
            }
            else
            {
                var dbexamSectionlink = FetchLink(examSectionlink.LinkId);
                if (dbexamSectionlink == null)
                {
                    sr.AddError($"LinkId for {examSectionlink.LinkName} was not found.");
                    return sr;
                }
                else
                {
                    dbexamSectionlink.LinkName = examSectionlink.LinkName;
                    dbexamSectionlink.LanguageCode = examSectionlink.LanguageCode;
                    dbexamSectionlink.Sequence = examSectionlink.Sequence;
                    dbexamSectionlink.Isactive = examSectionlink.Isactive;
                    dbexamSectionlink.EditBy = auditlogin;
                    dbexamSectionlink.EditOn = DateTime.Now;
                    // Save in DB
                    DB.SaveChanges();

                    // Return
                    sr.ReturnId = dbexamSectionlink.LinkId;
                    sr.ReturnName = dbexamSectionlink.LinkName;

                    return sr;
                }
            }
            return sr;
        }

        public static ExamSection Fetch(int? sectionId)
        {
            return DB.ExamSections
                     .Where(m => m.SectionId == (sectionId.HasValue ? sectionId.Value : 0))
                     .FirstOrDefault();
        }
        public static ExamSectionLink FetchLink(int? linkId)
        {
            return DB.ExamSectionLinks
                     .Where(m => m.LinkId == (linkId.HasValue ? linkId.Value : 0))
                     .FirstOrDefault();
        }

        public static List<ExamSection> ExamSectionList(string Lang, bool IsActive)
        {
            return DB.ExamSections
                  .Where(m => m.LanguageCode == Lang && m.Isactive == IsActive)
                  .ToList();
        }
        public static List<ExamSectionLink> ExamLinksList(string Lang, bool IsActive)
        {
            return DB.ExamSectionLinks
                  .Where(m => m.LanguageCode == Lang && m.Isactive == IsActive)
                  .ToList();
        }

        public static ServiceResponse DeleteExamSection(int sectionId)
        {
            var sr = new ServiceResponse();

            try
            {
                var section = Fetch(sectionId);

                DB.Entry(section).State = EntityState.Deleted;
                DB.SaveChanges();
            }
            catch (Exception exception)
            {
                sr.AddError(exception.Message);
            }

            return sr;
        }
        public static ServiceResponse DeleteExamSectionLink(int linkId)
        {
            var sr = new ServiceResponse();

            try
            {
                var section = FetchLink(linkId);

                DB.Entry(section).State = EntityState.Deleted;
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
