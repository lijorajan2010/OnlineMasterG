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
    public static class GeneralLogics
    {
        #region Validations

        public static ServiceResponse ValidateGeneralInstruction(List<GenneralInstructionVM> model)
        {
            ServiceResponse sr = new ServiceResponse();
            var TestId = model.FirstOrDefault()?.TestId;
            var Test = TestService.Fetch(TestId);
            if (Test != null)
            {
                if (String.IsNullOrEmpty(Test.TestName))
                    sr.AddError("The [Test Name] field cannot be empty.");
            }
            else
            {
                sr.AddError("Test not found.");
            }


            return sr;
        }

        public static ServiceResponse SaveGeneralInstructions(List<GenneralInstructionVM> model, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();

            List<GeneralInstruction> generalInstructions = new List<GeneralInstruction>();
            var instructions = GeneralService.GeneralInstructionList("en-US", true);
         //   var instructionsToremove = instructions.Where(x => !model.Select(y => y.TestId).Contains(x.DataFileId)).ToList();
            var instructionsToremove = instructions.Where(x => model.Select(y => y.TestId).Contains(x.TestId)
                                       && model.Select(y => y.SubjectId).Contains(x.SubjectId)).ToList();
            // delete existing with same testid and subjectid
            if (instructionsToremove!=null && instructionsToremove.Count()>0)
            {
                GeneralService.DeleteRangeInstructions(instructionsToremove);
            }

            if (model != null && model.Count() > 0)
            {
                foreach (var item in model)
                {
                    generalInstructions.Add(new GeneralInstruction()
                    {
                        TestId = item.TestId,
                        SubjectId = item.SubjectId,
                        TestName = TestService.Fetch(item.TestId)?.TestName,
                        LanguageCode = "en-US",
                        CorrectMarks = item.CorrectMarks,
                        NegativeMarks = item.NegativeMarks,
                        Isactive = true,
                        CreateBy = auditlogin,
                        CreateOn = DateTime.Now

                    });
                }
            }

            sr = GeneralService.SaveInstruction(generalInstructions, auditlogin);

            return sr;
        }

        public static List<GenneralInstructionVM> LoadGeneralInstructionVM(int? TestId)
        {
            List<GenneralInstructionVM> genneralInstructionVMs = new List<GenneralInstructionVM>();
            var subjects = SubjectService.SubjectList("en-US", true).Where(m => m.TestId == TestId).ToList();
            if (subjects != null && subjects.Count() > 0)
            {
                var QuestionUpload = QuestionUploadService.QuestionUploadList("en-US", true);
                foreach (var item in subjects)
                {
                   
                    genneralInstructionVMs.Add(new GenneralInstructionVM()
                    {
                        TestId = TestId,
                        SubjectId = item.SubjectId,
                        SubjectName = item.SubjectName,
                        QuestionCount = QuestionUploadService.GetQuestionsBasedOnTestAndSubject(item.TestId,item.SubjectId).Count(),
                        CorrectMarks = 1,
                        NegativeMarks = 0

                    });
                }
            }

            return genneralInstructionVMs;
        }

        public static List<GenneralInstructionVM> LoadDBGeneralInstruction(string Lang, bool isActive,int? TestId)
        {
            List<GenneralInstructionVM> genneralInstructionVMs = new List<GenneralInstructionVM>();
            var instructions = GeneralService.GeneralInstructionList(Lang, isActive).Where(m=>m.TestId== TestId);
            if (instructions != null && instructions.Count() > 0)
            {
                foreach (var item in instructions)
                {
                    genneralInstructionVMs.Add(new GenneralInstructionVM()
                    {
                        InstructionId = item.InstructionId,
                        TestId = item.TestId,
                        TestName = TestService.Fetch(item.TestId)?.TestName,
                        SubjectId = item.SubjectId,
                        SubjectName = SubjectService.Fetch(item.SubjectId)?.SubjectName,
                        QuestionCount = QuestionUploadService.GetQuestionsBasedOnTestAndSubject(item.TestId, item.SubjectId).Count(),
                        CorrectMarks = item.CorrectMarks,
                        NegativeMarks = item.NegativeMarks,

                    });
                }
            }

            return genneralInstructionVMs;
        }
        public static List<GenneralInstructionVM> GeneralInstructionList(string Lang,bool isActive)
        {
            List<GenneralInstructionVM> genneralInstructionVMs = new List<GenneralInstructionVM>();
            var instructions = GeneralService.GeneralInstructionList(Lang, isActive);
            if (instructions != null && instructions.Count() > 0)
            {
                foreach (var item in instructions)
                {
                    genneralInstructionVMs.Add(new GenneralInstructionVM()
                    {
                        InstructionId = item.InstructionId,
                        TestId = item.TestId,
                        TestName = TestService.Fetch(item.TestId)?.TestName,
                        SubjectId = item.SubjectId,
                        SubjectName = SubjectService.Fetch(item.SubjectId)?.SubjectName,
                        QuestionCount = QuestionUploadService.GetQuestionsBasedOnTestAndSubject(item.TestId, item.SubjectId).Count(),
                        CorrectMarks = item.CorrectMarks,
                        NegativeMarks = item.NegativeMarks,

                    });
                }
            }

            return genneralInstructionVMs;
        }

        

        #endregion
    }
}