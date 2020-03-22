using OnlineMasterG.Base;
using OnlineMasterG.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineMasterG.CommonServices
{
    public static class QuestionUploadService
    {
        private static OnlinemasterjiEntities DB = new OnlinemasterjiEntities();
        public static ServiceResponse SaveQuestionUpload(QuestionUpload question)
        {
            ServiceResponse sr = new ServiceResponse();
            try
            {
                DB.QuestionUploads.Add(question);
                DB.SaveChanges();
                // Return
                sr.ReturnId = question.QuestionUploadId;
            }
            catch (Exception ex)
            {
                sr.AddError(ex.Message);
            }
            return sr;
        }
        public static ServiceResponse SaveQuestionMockTest(List<QuestionsMockTest> questions, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            try
            {
                DB.QuestionsMockTests.AddRange(questions);
                DB.SaveChanges();
            }
            catch (Exception ex)
            {
                sr.AddError(ex.Message);
            }
            return sr;
        }
        public static QuestionUpload Fetch(int? questionUploadId)
        {
          return  DB.QuestionUploads
                   .Where(m => m.QuestionUploadId == (questionUploadId.HasValue ? questionUploadId.Value:0))
                   .FirstOrDefault();
        }
        public static QuestionsMockTest FetchQuestionsMock(int? QuestionsMockTestId)
        {
            return DB.QuestionsMockTests
                     .Where(m => m.QuestionsMockTestId == (QuestionsMockTestId.HasValue ? QuestionsMockTestId.Value : 0))
                     .FirstOrDefault();
        }
        public static List<QuestionUpload> QuestionUploadList(string Lang,bool IsActive)
        {
            return DB.QuestionUploads
                  .Where(m => m.LanguageCode == Lang && m.Isactive == IsActive)
                  .ToList();
        }
        public static List<QuestionsMockTest> GetQuestionsBasedOnTestAndSubject(int? TestId, int? SubjectId)
        {
            return DB.QuestionsMockTests
                  .Include(m=>m.QuestionUpload)
                  .Where(m => m.Isactive == true 
                           && m.QuestionUpload.QuestionStatus =="VAL"
                           && m.QuestionUpload.TestId == TestId
                           && m.QuestionUpload.SubjectId == SubjectId)
                  .ToList();
        }
        public static List<QuestionsMockTest> QuestionsMockTestList(int? QuestionUploadId)
        {
            return DB.QuestionsMockTests
                  .Where(m=>m.QuestionUploadId == QuestionUploadId)
                  .ToList();
        }
        public static ServiceResponse DeleteQuestionUpload(int questionUploadId)
        {
            var sr = new ServiceResponse();

            try
            {
                var QuestionUpload = Fetch(questionUploadId);

                QuestionUpload.QuestionStatus = "DEL";
                QuestionUpload.Isactive = false;
                DB.SaveChanges();
            }
            catch (Exception exception)
            {
                sr.AddError(exception.Message);
            }

            return sr;
        }
        public static ServiceResponse ApproveQuestionUpload(int questionUploadId)
        {
            var sr = new ServiceResponse();

            try
            {
                var QuestionUpload = Fetch(questionUploadId);

                QuestionUpload.QuestionStatus = "VAL";
                QuestionUpload.Isactive = true;
                DB.SaveChanges();
            }
            catch (Exception exception)
            {
                sr.AddError(exception.Message);
            }

            return sr;
        }
        public static ServiceResponse DenyQuestionUpload(int questionUploadId)
        {
            var sr = new ServiceResponse();

            try
            {
                var QuestionUpload = Fetch(questionUploadId);

                QuestionUpload.QuestionStatus = "PEN";
                QuestionUpload.Isactive = true;
                DB.SaveChanges();
            }
            catch (Exception exception)
            {
                sr.AddError(exception.Message);
            }

            return sr;
        }

        internal static ServiceResponse EditSaveQuestionReview(QuestionsMockTest existingMockTest, List<QuestionAnswerChoice> newquestionAnswerChoices, List<QuestionPoint> newquestionPoint)
        {
            var sr = new ServiceResponse();
            try
            {
                // First remove and Add new Questions and Answers the update existing

                DB.QuestionPoints.RemoveRange(existingMockTest.QuestionPoints);
                DB.QuestionAnswerChoices.RemoveRange(existingMockTest.QuestionAnswerChoices);
                DB.SaveChanges();

                DB.QuestionPoints.AddRange(newquestionPoint);
                DB.QuestionAnswerChoices.AddRange(newquestionAnswerChoices);

                DB.SaveChanges();

                sr.ReturnId = existingMockTest.QuestionsMockTestId;
            }
            catch (Exception exception)
            {
                sr.AddError(exception.Message);
            }
            return sr;
        }
    }
}