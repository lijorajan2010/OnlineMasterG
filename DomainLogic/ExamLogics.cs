using OnlineMasterG.CommonFramework;
using OnlineMasterG.CommonServices;
using OnlineMasterG.Models.DAL;
using OnlineMasterG.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.DomainLogic
{
    public static class ExamLogics
    {
        public enum AnswerStatus
        {
            NOTATTEMPTED,
            MARKED,
            ANSWERED,
            NOTANSWERED

        }

        public static MockTestAttemptVM GetMockTestAttemptDetails(string Login, int TestId, string auditlogin)
        {
            MockTestAttemptVM model = new MockTestAttemptVM();

            var TestAttepts = ExamService.GetAttemptListByLoginAndTestId(Login, TestId);
            // if list not empty, take first not completed attempt order by create date desc
            if (TestAttepts != null && TestAttepts.Count() > 0)
            {
                var FirstNotCompletedAttempt = TestAttepts.Where(m => m.IsCompleted == false).OrderByDescending(m => m.CreateDate).FirstOrDefault();
                model = SetAttemptVMModel(FirstNotCompletedAttempt, TestId);
            }
            else
            {
                // create a dummy data / insert data into Mock ttest attempt table then return that table
                var firstTimeAttempt = GetFirstAttemptMocktest(Login, TestId);
                var sr = ExamService.SaveMockTestAttempt(firstTimeAttempt, auditlogin);
                if (sr.Status)
                {
                    var newData = ExamService.Fetch(sr.ReturnId);
                    model = SetAttemptVMModel(newData, TestId);
                }
                else
                {
                    return null;
                }
            }

            return model;
        }

        public static List<string> getAnswerStatuses()
        {
            List<string> AnswerStatusList = new List<string>();
            AnswerStatusList.Add(AnswerStatus.ANSWERED.ToString());
            AnswerStatusList.Add(AnswerStatus.MARKED.ToString());
            AnswerStatusList.Add(AnswerStatus.NOTANSWERED.ToString());
            AnswerStatusList.Add(AnswerStatus.NOTATTEMPTED.ToString());

            return AnswerStatusList;
        }

        private static MockTestAttempt GetFirstAttemptMocktest(string login, int? testId)
        {
            var Test = TestService.Fetch(testId);

            MockTestAttempt mockTestAttempt = new MockTestAttempt();
            List<MockTestAttemptDetail> mockTestAttemptDetails = new List<MockTestAttemptDetail>();

            mockTestAttempt.Login = login;
            mockTestAttempt.TestId = testId;
            mockTestAttempt.IsPaused = false;
            mockTestAttempt.IsCompleted = false;
            mockTestAttempt.CreateBy = login;
            mockTestAttempt.CreateDate = DateTime.Now;
            mockTestAttempt.TimeLeftInMinutes = Test != null ? Test.TimeInMinutes : 0;

            var Subjects = AppInfo.GetSubjects.Where(m => m.TestId == testId).ToList();
            var Problems = ExamService.GetProblemMasters();
            foreach (var item in Subjects.Select((x, y) => new { Data = x, Index = y }))
            {
                if (AppInfo.GetQuestionsMockTests != null && AppInfo.GetQuestionsMockTests.Count() > 0)
                {
                    var QuestionBasedOnTestAndSubjects = AppInfo.GetQuestionsMockTests.Where(m => m.QuestionUpload?.TestId == testId && m.QuestionUpload?.SubjectId == item.Data.SubjectId).ToList();

                    foreach (var Q in QuestionBasedOnTestAndSubjects.OrderBy(m => m.QuestionNumber).Select((x, y) => new { Data = x, Index = y }))
                    {
                        MockTestAttemptDetail detail = new MockTestAttemptDetail();

                        List<ProblemsReported> problemsOfThisQuestion = new List<ProblemsReported>();
                        foreach (var prob in Problems)
                        {
                            problemsOfThisQuestion.Add(new ProblemsReported()
                            {
                                ProblemId = prob.ProblemId,
                                IsReported = false
                            });
                        }

                        detail.QuestionsMockTestId = Q.Data.QuestionsMockTestId;
                        detail.CourseId = Q.Data.QuestionUpload?.CourseId;
                        detail.CategoryId = Q.Data.QuestionUpload?.CategoryId;
                        detail.SectionId = Q.Data.QuestionUpload?.SectionId;
                        detail.SubjectId = Q.Data.QuestionUpload?.SubjectId;
                        detail.QuestionsMockTestId = Q.Data.QuestionsMockTestId;
                        detail.MarksScored = 0;
                        detail.AnswerStatus = AnswerStatus.NOTATTEMPTED.ToString();
                        detail.QuestionNumber = Q.Data.QuestionNumber;
                        detail.ProblemsReporteds = problemsOfThisQuestion;

                        //if (Q.Data.QuestionAnswerChoices != null && Q.Data.QuestionAnswerChoices.Count() > 0)
                        //{
                        //    foreach (var QA in Q.Data.QuestionAnswerChoices)
                        //    {
                        //        var b = QA.QuestionAnswer;
                        //    }
                        //}
                        mockTestAttemptDetails.Add(detail);
                    }
                }
               
            }

            mockTestAttempt.MockTestAttemptDetails = mockTestAttemptDetails;

            return mockTestAttempt;

        }

        private static MockTestAttemptVM SetAttemptVMModel(MockTestAttempt firstNotCompletedAttempt, int? TestId)
        {
            MockTestAttemptVM model = new MockTestAttemptVM();

            if (firstNotCompletedAttempt != null)
            {
                model.AttemptId = firstNotCompletedAttempt.AttemptId;
                model.TestId = firstNotCompletedAttempt.TestId;
                model.Login = firstNotCompletedAttempt.Login;
                model.IsPaused = firstNotCompletedAttempt.IsPaused;
                model.TimeLeftInMinutes = firstNotCompletedAttempt.TimeLeftInMinutes;
                model.IsCompleted = firstNotCompletedAttempt.IsCompleted;
                model.TestName = TestService.Fetch(firstNotCompletedAttempt.TestId)?.TestName;
                model.TestId = firstNotCompletedAttempt.TestId;

                if (firstNotCompletedAttempt.MockTestAttemptDetails != null && firstNotCompletedAttempt.MockTestAttemptDetails.Count() > 0)
                {
                    List<MockTestAttemptDetailVM> mockTestAttemptDetails = new List<MockTestAttemptDetailVM>();
                    foreach (var item in firstNotCompletedAttempt.MockTestAttemptDetails)
                    {

                        List<ProblemsReportedVM> problemsReporteds = new List<ProblemsReportedVM>();

                        if (item.ProblemsReporteds != null && item.ProblemsReporteds.Count() > 0)
                        {
                            foreach (var pbrptd in item.ProblemsReporteds)
                            {
                                problemsReporteds.Add(new ProblemsReportedVM()
                                {
                                    ProblemReportId = pbrptd.ProblemReportId,
                                    ProblemId = pbrptd.ProblemId,
                                    IsReported = pbrptd.IsReported
                                });
                            }
                        }

                        var OriginalQuestion = QuestionUploadService.GetQuestionsBasedOnTestAndSubject(TestId, item.SubjectId).Where(m=>m.QuestionNumber == item.QuestionNumber).FirstOrDefault();

                        List<QuestionAnswerChoiceVM> questionAnswerChoiceVMs = new List<QuestionAnswerChoiceVM>();
                        List<QuestionPointVM> QuestionPointVMs = new List<QuestionPointVM>();

                        if (OriginalQuestion!=null && OriginalQuestion.QuestionAnswerChoices!=null && OriginalQuestion.QuestionAnswerChoices.Count()>0)
                        {
                            foreach (var Ans in OriginalQuestion.QuestionAnswerChoices)
                            {
                                questionAnswerChoiceVMs.Add(new QuestionAnswerChoiceVM()
                                {
                                    QuestionAnswer = Ans.QuestionAnswer,
                                    QuestionAnswerChoiceId = Ans.QuestionAnswerChoiceId,
                                    ChoiceId = Ans.ChoiceId,
                                    IsCorrect = Ans.IsCorrect
                                });
                            }
                        }

                        if (OriginalQuestion != null && OriginalQuestion.QuestionPoints != null && OriginalQuestion.QuestionPoints.Count() > 0)
                        {
                            foreach (var Ques in OriginalQuestion.QuestionPoints)
                            {
                                QuestionPointVMs.Add(new QuestionPointVM()
                                {
                                    QuestionPointId = Ques.QuestionPointId,
                                    QPoint =Ques.QPoint
                                });
                            }
                        }

                        QuestionsMockTestVM Question = new QuestionsMockTestVM();
                        if (OriginalQuestion!=null)
                        {
                            Question.Description = OriginalQuestion.Description;
                            Question.Isactive = OriginalQuestion.Isactive;
                            Question.Question = OriginalQuestion.Question;
                            Question.LanguageCode = OriginalQuestion?.LanguageCode;
                            Question.QuestionNumber = OriginalQuestion.QuestionNumber;
                            Question.QuestionSet = OriginalQuestion.QuestionSet;
                            Question.QuestionsMockTestId = OriginalQuestion.QuestionsMockTestId;
                            Question.QuestionImageFileId = OriginalQuestion.QuestionImageFileId;
                            Question.Solution = OriginalQuestion.Solution;
                            Question.QuestionPoints = QuestionPointVMs;
                            Question.QuestionAnswerChoices = questionAnswerChoiceVMs;
                        }
                      
                        mockTestAttemptDetails.Add(new MockTestAttemptDetailVM()
                        {
                            AttemptDetailId = item.AttemptDetailId,
                            CourseId = item.CourseId,
                            CategoryId = item.CategoryId,
                            SectionId = item.SectionId,
                            SubjectId = item.SubjectId,
                            QuestionsMockTestId = item.QuestionsMockTestId,
                            QuestionNumber = item.QuestionNumber,
                            ChoosenAnswerChoiceId = item.ChoosenAnswerChoiceId,
                            AnswerChoiceId = item.AnswerChoiceId,
                            AnswerStatus = item.AnswerStatus,
                            MarksScored = item.MarksScored,
                            ProblemsReporteds = problemsReporteds,
                            QuestionsMockTests = Question

                        });
                    }
                    model.MockTestAttemptDetails = mockTestAttemptDetails;
                }

            }

            return model;
        }
    }
}