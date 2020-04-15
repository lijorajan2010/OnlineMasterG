using OnlineMasterG.Base;
using OnlineMasterG.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineMasterG.CommonServices
{
    public static class ClassService
    {
        public static OnlinemasterjiEntities DB { get; private set; }
        static ClassService()
        {
            DB = new OnlinemasterjiEntities();
        }
        public static ServiceResponse SaveSchoolClass(SchoolClass schoolClass, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            if (schoolClass.ClassId == 0)
            {
                DB.SchoolClasses.Add(schoolClass);
                DB.SaveChanges();
            }
            else
            {
                var dbschoolClass = Fetch(schoolClass.ClassId);
                if (dbschoolClass == null)
                {
                    sr.AddError($"ClassId for {schoolClass.ClassName} was not found.");
                    return sr;
                }
                else
                {
                    dbschoolClass.ClassName = schoolClass.ClassName;
                    dbschoolClass.LanguageCode = schoolClass.LanguageCode;
                    dbschoolClass.Sequence = schoolClass.Sequence;
                    dbschoolClass.Isactive = schoolClass.Isactive;
                    dbschoolClass.EditBy = auditlogin;
                    dbschoolClass.EditOn = DateTime.Now;
                    // Save in DB
                    DB.SaveChanges();

                    // Return
                    sr.ReturnId = dbschoolClass.ClassId;
                    sr.ReturnName = dbschoolClass.ClassName;

                    return sr;
                }
            }
            return sr;
        }
    
        public static SchoolClass Fetch(int? classId)
        {
          return  DB.SchoolClasses
                   .Where(m => m.ClassId == (classId.HasValue ? classId.Value :0))
                   .FirstOrDefault();
        }
      
        public static List<SchoolClass> SchoolClassList(string Lang,bool IsActive)
        {
            return DB.SchoolClasses
                  .Where(m => m.LanguageCode == Lang && m.Isactive == IsActive)
                  .ToList();
        }
        public static List<SchoolClass> SchoolAllClassList(string Lang)
        {
            return DB.SchoolClasses
                  .Where(m => m.LanguageCode == Lang)
                  .ToList();
        }

        public static ServiceResponse DeleteSchoolClass(int classId)
        {
            var sr = new ServiceResponse();

            try
            {
                var Schlclass = Fetch(classId);

                DB.Entry(Schlclass).State = EntityState.Deleted;
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