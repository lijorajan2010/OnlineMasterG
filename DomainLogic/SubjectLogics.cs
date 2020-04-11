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
    public static class SubjectLogics
    {
        #region Validations

        public static ServiceResponse ValidateSubject(SubjectVM model)
        {
            ServiceResponse sr = new ServiceResponse();

            if (String.IsNullOrEmpty(model.SubjectName))
                sr.AddError("The [Subject Name] field cannot be empty.");

            return sr;
        }
        public static ServiceResponse SaveSubject(SubjectVM model,string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            Subject subject = new Subject()
            {
                SubjectId = model.SubjectId,
                SubjectName = model.SubjectName,
                TestId = model.TestId,
                SectionId = model.SectionId,
                CategoryId = model.CategoryId,
                CourseId = model.CourseId,
                LanguageCode = model.LanguageCode,
                Sequence = model.Sequence,
                CreateBy = auditlogin,
                CreateOn = DateTime.Now,
                Isactive = true
            };
            sr = SubjectService.SaveSubject(subject, auditlogin);

            return sr;
        }
        public static ServiceResponse SaveCollegeSubject(SubjectVM model, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            CollegeSubject subject = new CollegeSubject()
            {
                SubjectId = model.SubjectId,
                SubjectName = model.SubjectName,
                CourseId = model.CourseId,
                LanguageCode = model.LanguageCode,
                Sequence = model.SequenceSub,
                CreateBy = auditlogin,
                CreateOn = DateTime.Now,
                Isactive = true
            };
            sr = SubjectService.SaveCollegeSubject(subject, auditlogin);

            return sr;
        }
        public static ServiceResponse SaveSchoolSubject(SubjectVM model, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            SchoolSubject subject = new SchoolSubject()
            {
                SubjectId = model.SubjectId,
                SubjectName = model.SubjectName,
                ClassId = model.ClassId,
                LanguageCode = model.LanguageCode,
                Sequence = model.SequenceSub,
                CreateBy = auditlogin,
                CreateOn = DateTime.Now,
                Isactive = true
            };
            sr = SubjectService.SaveSchoolSubject(subject, auditlogin);

            return sr;
        }
        public static List<SubjectVM> SubjectList(string Lang )
        {
            List<SubjectVM> model = new List<SubjectVM>();
            var subjects = SubjectService.SubjectAllList(Lang);
            if (subjects != null && subjects.Count()>0)
            {
                foreach (var item in subjects)
                {
                    model.Add(new SubjectVM()
                    {
                        SubjectId = item.SubjectId,
                        TestId = item.TestId,
                        SectionId = item.SectionId,
                        CourseId = item.CourseId,
                        CategoryId = item.CategoryId,
                        SubjectName = item.SubjectName,
                        SectionName = SectionService.Fetch(item.SectionId)?.SectionName,
                        CourseName = CourseService.Fetch(item.CourseId)?.CourseName,
                        CategoryName = CategoryService.Fetch(item.CategoryId)?.CategoryName,
                        TestName = TestService.Fetch(item.TestId)?.TestName,
                        Sequence=item.Sequence,
                        LanguageCode =item.LanguageCode

                    });
                }
            }
            return model;
        }
        public static List<SubjectVM> CollegeSubjectList(string Lang)
        {
            List<SubjectVM> model = new List<SubjectVM>();
            var subjects = SubjectService.CollegeAllSubjectList(Lang);
            if (subjects != null && subjects.Count() > 0)
            {
                foreach (var item in subjects)
                {
                    model.Add(new SubjectVM()
                    {
                        SubjectId = item.SubjectId, 
                        CourseId = item.CourseId,
                        SubjectName = item.SubjectName,
                        CourseName = CourseService.FetchCollegeCourse(item.CourseId)?.CourseName,
                        SequenceSub = item.Sequence,
                        LanguageCode = item.LanguageCode,
                        IsActive = item.Isactive

                    });
                }
            }
            return model;
        }
        public static List<SubjectVM> SchoolSubjectList(string Lang)
        {
            List<SubjectVM> model = new List<SubjectVM>();
            var subjects = SubjectService.SchoolAllSubjectList(Lang);
            if (subjects != null && subjects.Count() > 0)
            {
                foreach (var item in subjects)
                {
                    model.Add(new SubjectVM()
                    {
                        SubjectId = item.SubjectId, 
                        ClassId = item.ClassId,
                        SubjectName = item.SubjectName,
                        ClassName = ClassService.Fetch(item.ClassId)?.ClassName,
                        SequenceSub = item.Sequence,
                        LanguageCode = item.LanguageCode,
                        IsActive = item.Isactive

                    });
                }
            }
            return model;
        }

        #endregion
    }
}