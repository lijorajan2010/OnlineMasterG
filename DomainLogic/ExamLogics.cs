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

        public static MockTestAttemptVM GetMockTestAttemptDetails(string Login, int TestId, string auditlogin)
        {
            MockTestAttemptVM model = new MockTestAttemptVM();

            var TestAttepts = ExamService.GetAttemptListByLoginAndTestId(Login, TestId);
            // if list not empty, take first not completed attempt order by create date desc
            if (TestAttepts != null && TestAttepts.Count() > 0)
            {
                var FirstNotCompletedAttempt = TestAttepts.Where(m => m.IsCompleted == false).OrderByDescending(m => m.CreateDate).FirstOrDefault();
                model = SetAttemptVMModel(FirstNotCompletedAttempt);
            }
            else
            {
                // create a dummy data / insert data into Mock ttest attempt table then return that table
                var firstTimeAttempt = GetFirstAttemptMocktest(Login, TestId);
                var sr = ExamService.SaveMockTestAttempt(firstTimeAttempt, auditlogin);
                if (sr.Status)
                {
                    var newData = ExamService.Fetch(sr.ReturnId);
                    model = SetAttemptVMModel(newData);
                }
                else
                {
                    return null;
                }
            }

            return model;
        }


        private static MockTestAttempt GetFirstAttemptMocktest(string login, int? testId)
        {
            var Test = TestService.Fetch(testId);

            MockTestAttempt mockTestAttempt = new MockTestAttempt();
            mockTestAttempt.Login = login;
            mockTestAttempt.TestId = testId;
            mockTestAttempt.IsPaused = false;
            mockTestAttempt.IsCompleted = false;
            mockTestAttempt.CreateBy = login;
            mockTestAttempt.CreateDate = DateTime.Now;
            mockTestAttempt.TimeLeftInMinutes = Test != null ? Test.TimeInMinutes : 0;
            return mockTestAttempt;

        }

        private static MockTestAttemptVM SetAttemptVMModel(MockTestAttempt firstNotCompletedAttempt)
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
                                });
                            }
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
                            ProblemsReporteds = problemsReporteds

                        });
                    }

                    model.MockTestAttemptDetails = mockTestAttemptDetails;
                }

            }

            return model;
        }
    }
}