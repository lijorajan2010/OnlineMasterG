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
        public static List<QuestionUpload> QuestionUploadList(string Lang,bool IsActive)
        {
            return DB.QuestionUploads
                  .Where(m => m.LanguageCode == Lang && m.Isactive == IsActive)
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
        
    }
}