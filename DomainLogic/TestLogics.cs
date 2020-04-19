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
    public static class TestLogics
    {
        #region Validations

        public static ServiceResponse ValidateTest(TestVM model)
        {
            ServiceResponse sr = new ServiceResponse();

            if (String.IsNullOrEmpty(model.TestName))
                sr.AddError("The [Test Name] field cannot be empty.");

            return sr;
        }
        public static ServiceResponse SaveTest(TestVM model,string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            MockTest test = new MockTest()
            {
                TestId = model.TestId,
                SectionId = model.SectionId,
                CategoryId = model.CategoryId,
                CourseId = model.CourseId,
                TestName = model.TestName,
                Description = model.Description,
                LanguageCode = model.LanguageCode,
                TimeInMinutes = model.TimeInMinutes,
                //ExamTypeId = model.ExamTypeId,
                CreateBy = auditlogin,
                CreateOn = DateTime.Now,
                Isactive = true
            };
            sr = TestService.SaveTest(test, auditlogin);

            return sr;
        }
        public static List<TestVM> TestList(string Lang)
        {
            List<TestVM> model = new List<TestVM>();
            var tests = TestService.TestAllList(Lang);
            if (tests != null && tests.Count()>0)
            {
                foreach (var item in tests)
                {
                    model.Add(new TestVM()
                    {
                        TestId = item.TestId,
                        SectionId = item.SectionId,
                        CourseId = item.CourseId,
                        CategoryId = item.CategoryId,
                        TestName = item.TestName,
                        SectionName = SectionService.Fetch(item.SectionId)?.SectionName,
                        CourseName = CourseService.Fetch(item.CourseId)?.CourseName,
                        CategoryName = CategoryService.Fetch(item.CategoryId)?.CategoryName,
                        Description=item.Description,
                        TimeInMinutes=item.TimeInMinutes,
                        LanguageCode =item.LanguageCode,
                        CreateOn = item.CreateOn,
                        //ExamTypeId = item.ExamTypeId,
                        //ExamTypeName = TestService.FetchMockExamType(item.ExamTypeId).MockExamTypeName,
                        //ExamTypeCode = TestService.FetchMockExamType(item.ExamTypeId).MockExamTypeCode,


                    });
                }
            }
            return model;
        }


        #endregion
    }
}