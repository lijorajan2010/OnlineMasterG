﻿using OnlineMasterG.Base;
using OnlineMasterG.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.CommonServices
{
    public static class ExamService
    {
        private static OnlinemasterjiEntities DB = new OnlinemasterjiEntities();
        public static List<MockTestAttempt> GetAttemptListByLoginAndTestId(string Login, int TestId)
        {
            return DB.MockTestAttempts.Where(m => m.Login == Login && m.TestId == TestId).ToList();
        }
        public static List<MockTestAttempt> GetAttemptListByTestId(int? TestId)
        {
            return DB.MockTestAttempts.Where(m =>  m.TestId == (TestId.HasValue? TestId : 0)).ToList();
        }
        public static MockTestAttempt Fetch(int attemptId)
        {
            return DB.MockTestAttempts.Where(m => m.AttemptId == attemptId).FirstOrDefault();
        }

    

        internal static ServiceResponse SaveMockTestAttempt(MockTestAttempt firstTimeAttempt, string audiLogin)
        {
            ServiceResponse sr = new ServiceResponse();
            if (firstTimeAttempt.AttemptId == 0)
            {
                DB.MockTestAttempts.Add(firstTimeAttempt);
                DB.SaveChanges();
                sr.ReturnId = firstTimeAttempt.AttemptId;
            }
            else
            {
                try
                {
                    var dbMockTestAttempt = Fetch(firstTimeAttempt.AttemptId);
                    if (dbMockTestAttempt != null)
                    {
                        dbMockTestAttempt.IsPaused = firstTimeAttempt.IsPaused;
                        dbMockTestAttempt.TimeLeftInMinutes = firstTimeAttempt.TimeLeftInMinutes;
                        dbMockTestAttempt.IsCompleted = firstTimeAttempt.IsCompleted;
                        dbMockTestAttempt.EditBy = audiLogin;
                        dbMockTestAttempt.EditDate = DateTime.Now;

                        if (firstTimeAttempt.MockTestAttemptDetails != null && firstTimeAttempt.MockTestAttemptDetails.Count() > 0)
                        {
                            //// first removing
                            //DB.MockTestAttemptDetails.RemoveRange(dbMockTestAttempt.MockTestAttemptDetails);
                            //DB.SaveChanges();
                            //// and saving new list
                            //DB.MockTestAttemptDetails.AddRange(firstTimeAttempt.MockTestAttemptDetails);
                            //DB.SaveChanges();

                            foreach (var item in firstTimeAttempt.MockTestAttemptDetails)
                            {
                               var  DBMockTestAttemptDetails = dbMockTestAttempt.MockTestAttemptDetails.Where(m => m.AttemptDetailId == item.AttemptDetailId).FirstOrDefault();
                                if (DBMockTestAttemptDetails!=null)
                                {
                                    DBMockTestAttemptDetails.AnswerChoiceId = item.AnswerChoiceId;
                                    DBMockTestAttemptDetails.ChoosenAnswerChoiceId = item.ChoosenAnswerChoiceId;
                                    DBMockTestAttemptDetails.IsAnswerCorrect = item.IsAnswerCorrect;
                                    DBMockTestAttemptDetails.MarksScored = item.MarksScored;
                                    DBMockTestAttemptDetails.AnswerStatus = item.AnswerStatus;
                                    DBMockTestAttemptDetails.SubjectTimeUsed = item.SubjectTimeUsed;
                                }
                             

                                if (DBMockTestAttemptDetails.ProblemsReporteds!=null && DBMockTestAttemptDetails.ProblemsReporteds.Count()>0
                                    && item.ProblemsReporteds!=null && item.ProblemsReporteds.Count()>0)
                                {
                                    foreach (var prob in item.ProblemsReporteds)
                                    {
                                        var DBProb = DBMockTestAttemptDetails.ProblemsReporteds.Where(m => m.ProblemId == prob.ProblemId).FirstOrDefault();
                                        if (DBProb!=null)
                                        {
                                            DBProb.IsReported = prob.IsReported;
                                            DBProb.IssueText = prob.IssueText;

                                        }

                                    }
                                }

                            }
                        }

                        DB.SaveChanges();
                        sr.ReturnId = dbMockTestAttempt.AttemptId;
                    }
                }
                catch (Exception ex)
                {
                    sr.AddError(ex.Message);
                }

            }
            return sr;
        
        }
        public static List<ProblemMaster> GetProblemMasters()
        {
            return DB.ProblemMasters.Where(m => m.IsActive == true).ToList();

        }
    }
}