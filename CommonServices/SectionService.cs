using OnlineMasterG.Base;
using OnlineMasterG.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineMasterG.CommonServices
{
    public static class SectionService
    {
        private static OnlinemasterjiEntities DB = new OnlinemasterjiEntities();
        public static ServiceResponse SaveSection(Section section, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            if (section.SectionId == 0)
            {
                DB.Sections.Add(section);
                DB.SaveChanges();
            }
            else
            {
                var dbSection = Fetch(section.SectionId);
                if (dbSection == null)
                {
                    sr.AddError($"Section Name {section.SectionName} is not found.");
                    return sr;
                }
                else
                {
                    dbSection.SectionName = section.SectionName;
                    dbSection.CourseId = section.CourseId;
                    dbSection.Category = section.Category;
                    dbSection.LanguageCode = section.LanguageCode;
                    dbSection.Description = section.Description; 
                    dbSection.EditBy = auditlogin;
                    dbSection.EditOn = DateTime.Now;
                    // Save in DB
                    DB.SaveChanges();

                    // Return
                    sr.ReturnId = dbSection.SectionId;
                    sr.ReturnName = dbSection.SectionName;

                    return sr;
                }
            }
            return sr;
        }
        public static Section Fetch(int? sectionId)
        {
          return  DB.Sections
                   .Where(m => m.SectionId == (sectionId.HasValue ? sectionId.Value:0))
                   .FirstOrDefault();
        }
        public static List<Section> SectionList(string Lang,bool IsActive)
        {
            return DB.Sections
                  .Where(m => m.LanguageCode == Lang && m.Isactive == IsActive)
                  .ToList();
        }
        public static ServiceResponse DeleteSection(int SectionId)
        {
            var sr = new ServiceResponse();

            try
            {
                var Section = Fetch(SectionId);

                DB.Entry(Section).State = EntityState.Deleted;
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