using OnlineMasterG.Base;
using OnlineMasterG.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineMasterG.CommonServices
{
    public class TestService : ServiceBase
    {
       
        public static ServiceResponse SaveTest(MockTest test, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            if (test.TestId == 0)
            {
                DB.MockTests.Add(test);
                DB.SaveChanges();
            }
            else
            {
                var dbTest = Fetch(test.TestId);
                if (dbTest == null)
                {
                    sr.AddError($"Test Name {test.TestName} is not found.");
                    return sr;
                }
                else
                {
                    dbTest.TestName = test.TestName;
                    dbTest.CourseId = test.CourseId;
                    dbTest.Category = test.Category;
                    dbTest.SectionId = test.SectionId;
                    dbTest.LanguageCode = test.LanguageCode;
                    dbTest.Description = test.Description;
                    dbTest.TimeInMinutes = test.TimeInMinutes;
                    //dbTest.ExamTypeId = test.ExamTypeId;
                    dbTest.EditBy = auditlogin;
                    dbTest.Isactive = true;
                    dbTest.EditOn = DateTime.Now;
                    // Save in DB
                    DB.SaveChanges();

                    // Return
                    sr.ReturnId = dbTest.TestId;
                    sr.ReturnName = dbTest.TestName;

                    return sr;
                }
            }
            return sr;
        }
        public static MockTest Fetch(int? testId)
        {
          return  DB.MockTests
                   .Where(m => m.TestId == (testId.HasValue ? testId.Value:0))
                   .FirstOrDefault();
        }
        public static List<MockTest> TestList(string Lang,bool IsActive)
        {
            return DB.MockTests
                  .Where(m => m.LanguageCode == Lang && m.Isactive == IsActive)
                  .ToList();
        }
        public static List<MockExamType> MockExamTypeList()
        {
            return DB.MockExamTypes
                  .ToList();
        }
        public static MockExamType FetchMockExamType(int? ExamTypeId)
        {
            return DB.MockExamTypes
                .Where(m=>m.MockExamTypeId == (ExamTypeId.HasValue? ExamTypeId:0))
                  .FirstOrDefault();
        }
        public static List<MockTest> TestAllList(string Lang)
        {
            return DB.MockTests
                  .Where(m => m.LanguageCode == Lang)
                  .ToList();
        }
        public static ServiceResponse DeleteTest(int TestId)
        {
            var sr = new ServiceResponse();

            try
            {
                if (!SubjectService.SubjectList("en-US", true).Any(m => m.TestId == TestId))
                {
                    var Test = Fetch(TestId);

                    DB.Entry(Test).State = EntityState.Deleted;
                    DB.SaveChanges();
                }
                else
                {
                    var subjectsUsed = SubjectService.SubjectList("en-US", true).Where(m => m.TestId == TestId).Select(m => m.SubjectName).ToList();
                    sr.AddError($"You can't delete this Test as it is being used by Subjects such as { string.Join(",", subjectsUsed)}. If you want to delete, please delete these Subjects first.");

                }

            }
            catch (Exception exception)
            {
                sr.AddError(exception.Message);
            }

            return sr;
        }
    }
}