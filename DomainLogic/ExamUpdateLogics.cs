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
    public class ExamUpdateLogics
    {
        #region Validations

        public static ServiceResponse ValidateSection(ExamUpdateSectionVM model)
        {
            ServiceResponse sr = new ServiceResponse();

            if (String.IsNullOrEmpty(model.SectionName))
                sr.AddError("The [Section Name] field cannot be empty.");

            return sr;
        }
        public static ServiceResponse ValidateSectionLinks(ExamUpdateSectionVM model)
        {
            ServiceResponse sr = new ServiceResponse();

            if (String.IsNullOrEmpty(model.Link))
                sr.AddError("The [Link] field cannot be empty.");

            return sr;
        }

        public static ServiceResponse SaveExamUpdateSection(ExamUpdateSectionVM model, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            ExamSection examSection = new ExamSection()
            {
                SectionId = model.SectionId,
                SectionName = model.SectionName,
                Sequence = model.Sequence,
                LanguageCode = model.LanguageCode,
                Isactive = model.IsActive,
                CreateBy = auditlogin,
                CreateOn = DateTime.Now
            };
            sr = ExamUpdateService.SaveExamSection(examSection, auditlogin);

            return sr;
        }

        public static List<ExamUpdateSectionVM> ExamSectionLinkList(string Lang, bool IsActive)
        {
            List<ExamUpdateSectionVM> model = new List<ExamUpdateSectionVM>();
            var links = ExamUpdateService.ExamLinksList(Lang, IsActive);
            if (links != null && links.Count() > 0)
            {
                foreach (var item in links)
                {
                    model.Add(new ExamUpdateSectionVM()
                    {
                        LinkId = item.LinkId,
                        Link = item.LinkName,
                        SectionName = ExamUpdateService.Fetch(item.SectionId)?.SectionName,
                        SectionLinkId = item.SectionId,
                        IsActive = item.Isactive,
                        Sequence = item.Sequence,
                        LanguageCode = item.LanguageCode

                    });
                }
            }
            return model;
        }
        public static ServiceResponse SaveExamUpdateSectionLink(ExamUpdateSectionVM model, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            ExamSectionLink examSection = new ExamSectionLink()
            {
                LinkId = model.LinkId,
                LinkName = model.Link,
                SectionId = model.SectionLinkId,
                Sequence = model.Sequence,
                LanguageCode = model.LanguageCode,
                Isactive = model.IsActive,
                CreateBy = auditlogin,
                CreateOn = DateTime.Now
            };
            sr = ExamUpdateService.SaveExamSectionLink(examSection, auditlogin);

            return sr;
        }

        #endregion
    }
}