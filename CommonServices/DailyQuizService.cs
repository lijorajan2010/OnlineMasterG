using OnlineMasterG.Base;
using OnlineMasterG.Models.DAL;
using OnlineMasterG.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineMasterG.CommonServices
{
    public static class DailyQuizService
    {
        private static OnlinemasterjiEntities DB = new OnlinemasterjiEntities();
        internal static List<DailyQuizCourse> DailyQuizCourseList(string Lang, bool isActive)
        {
            return DB.DailyQuizCourses.Where(m => m.LanguageCode == Lang && m.Isactive == isActive).ToList();
        }
        internal static List<DailyQuizCourse> DailyQuizAllCourseList(string Lang)
        {
            return DB.DailyQuizCourses.Where(m => m.LanguageCode == Lang).ToList();
        }
       
        internal static DailyQuizCourse Fetch(int? DailyQuizCourseId)
        {
            return DB.DailyQuizCourses.Where(m => m.DailyQuizCourseId == (DailyQuizCourseId.HasValue ? DailyQuizCourseId.Value : 0)).FirstOrDefault();
        }

        internal static DailyQuizSubject FetchDailyQuizSubject(int? DailyQuizSubjectId)
        {
            return DB.DailyQuizSubjects.Where(m => m.DailyQuizSubjectId == (DailyQuizSubjectId.HasValue ? DailyQuizSubjectId.Value : 0)).FirstOrDefault();
        }

        internal static DailyQuiz FetchDailyQuiz(int? DailyQuizId)
        {
            return DB.DailyQuizs.Where(m => m.DailyQuizId == (DailyQuizId.HasValue ? DailyQuizId.Value : 0)).FirstOrDefault();
        }

        internal static DailyQuizUpload FetchDailyQuizUpload(int? DailyQuizUploadId)
        {
            return DB.DailyQuizUploads.Where(m => m.DailyQuizUploadId == (DailyQuizUploadId.HasValue ? DailyQuizUploadId.Value : 0)).FirstOrDefault();
        }
        internal static QuizTest FetchQuizTest(int? QuizTestId)
        {
            return DB.QuizTests.Where(m => m.QuizTestId == (QuizTestId.HasValue ? QuizTestId.Value : 0)).FirstOrDefault();
        }
        internal static List<QuizTest> GetQuizTestsList(string Lang, bool isActive)
        {
            return DB.QuizTests.Where(m => m.LanguageCode == Lang && m.Isactive == isActive).ToList();
        }
        public static List<DailyQuizAttempt> GetDailyQuizAttemptListByLoginAndTestId(string Login, int DailyQuizId)
        {
            return DB.DailyQuizAttempts.Where(m => m.Login == Login && m.DailyQuizId == DailyQuizId).ToList();
        }
        public static List<DailyQuizAttempt> GetDailyQuizAttemptListByLogin(string Login)
        {
            return DB.DailyQuizAttempts.Where(m => m.Login == Login).ToList();
        }
        public static DailyQuizAttempt GetDailyQuizAttempt(int? AttemptId)
        {
            return DB.DailyQuizAttempts.Where(m => m.AttemptId == (AttemptId.HasValue? AttemptId.Value:0)).FirstOrDefault();
        }
        public static List<DailyQuizAttempt> GetDailyQuizAttemptList(int? AttemptId)
        {
            return DB.DailyQuizAttempts.Where(m => m.AttemptId == (AttemptId.HasValue ? AttemptId.Value : 0)).ToList();
        }
        public static List<QuizTest> GetQuizQuestionsBasedOnTestAndSubject(int? DailyQuizId, int? SubjectId)
        {
            return DB.QuizTests
                  .Include(m => m.DailyQuizUpload)
                  .Where(m => m.Isactive == true
                           && m.DailyQuizUpload.QuestionStatus == "VAL"
                           && m.DailyQuizUpload.DailyQuizId == DailyQuizId
                           && m.DailyQuizUpload.DailyQuizSubjectId == SubjectId)
                  .ToList();
        }
        public static ServiceResponse SaveDailyQuizCourse(DailyQuizCourse course, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            if (course.DailyQuizCourseId == 0)
            {
                DB.DailyQuizCourses.Add(course);
                DB.SaveChanges();
            }
            else
            {
                var dbCourse = Fetch(course.DailyQuizCourseId);
                if (dbCourse == null)
                {
                    sr.AddError($"CourseId for {course.DailyQuizCourseName} was not found.");
                    return sr;
                }
                else
                {
                    dbCourse.DailyQuizCourseName = course.DailyQuizCourseName;
                    dbCourse.LanguageCode = course.LanguageCode;
                    dbCourse.Sequence = course.Sequence;
                    dbCourse.Isactive = course.Isactive;
                    dbCourse.EditBy = auditlogin;
                    dbCourse.EditOn = DateTime.Now;
                    // Save in DB
                    DB.SaveChanges();

                    // Return
                    sr.ReturnId = dbCourse.DailyQuizCourseId;
                    sr.ReturnName = dbCourse.DailyQuizCourseName;

                    return sr;
                }
            }
            return sr;
        }

        public static ServiceResponse SaveDailyQuizSubject(DailyQuizSubject subjects, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            if (subjects.DailyQuizSubjectId == 0)
            {
                DB.DailyQuizSubjects.Add(subjects);
                DB.SaveChanges();
            }
            else
            {
                var dbSubject = FetchDailyQuizSubject(subjects.DailyQuizSubjectId);
                if (dbSubject == null)
                {
                    sr.AddError($"SubjectId for {subjects.DailyQuizSubjectName} was not found.");
                    return sr;
                }
                else
                {
                    dbSubject.DailyQuizSubjectName = subjects.DailyQuizSubjectName;
                    dbSubject.LanguageCode = subjects.LanguageCode;
                    dbSubject.Isactive = subjects.Isactive;
                    dbSubject.EditBy = auditlogin;
                    dbSubject.EditOn = DateTime.Now;
                    // Save in DB
                    DB.SaveChanges();

                    // Return
                    sr.ReturnId = dbSubject.DailyQuizSubjectId;
                    sr.ReturnName = dbSubject.DailyQuizSubjectName;

                    return sr;
                }
            }
            return sr;
        }

        public static ServiceResponse SaveDailyQuiz(DailyQuiz dailyQuiz, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            if (dailyQuiz.DailyQuizId == 0)
            {
                DB.DailyQuizs.Add(dailyQuiz);
                DB.SaveChanges();
            }
            else
            {
                var dbDailyQuiz = FetchDailyQuiz(dailyQuiz.DailyQuizId);
                if (dbDailyQuiz == null)
                {
                    sr.AddError($"DailyQuizId for {dailyQuiz.DailyQuizName} was not found.");
                    return sr;
                }
                else
                {
                    dbDailyQuiz.DailyQuizSubjectId = dailyQuiz.DailyQuizSubjectId;
                    dbDailyQuiz.DailyQuizCourseId = dailyQuiz.DailyQuizCourseId;
                    dbDailyQuiz.DailyQuizName = dailyQuiz.DailyQuizName;
                    dbDailyQuiz.LanguageCode = dailyQuiz.LanguageCode;
                    dbDailyQuiz.Description = dailyQuiz.Description;
                    dbDailyQuiz.NoOfQuestions = dailyQuiz.NoOfQuestions;
                    dbDailyQuiz.TimeInMinutes = dailyQuiz.TimeInMinutes;
                    dbDailyQuiz.Isactive = dailyQuiz.Isactive;
                    dbDailyQuiz.EditBy = auditlogin;
                    dbDailyQuiz.EditOn = DateTime.Now;
                    // Save in DB
                    DB.SaveChanges();

                    // Return
                    sr.ReturnId = dbDailyQuiz.DailyQuizId;
                    sr.ReturnName = dbDailyQuiz.DailyQuizName;

                    return sr;
                }
            }
            return sr;
        }
        internal static ServiceResponse SaveDailyQuizAttempt(DailyQuizAttempt firstTimeAttempt, string audiLogin)
        {
            ServiceResponse sr = new ServiceResponse();
            if (firstTimeAttempt.AttemptId == 0)
            {
                DB.DailyQuizAttempts.Add(firstTimeAttempt);
                DB.SaveChanges();
                sr.ReturnId = firstTimeAttempt.AttemptId;
            }
            else
            {
                try
                {
                    var dbDailyQuizAttempt = GetDailyQuizAttempt(firstTimeAttempt.AttemptId);
                    if (dbDailyQuizAttempt != null)
                    {
                        dbDailyQuizAttempt.IsPaused = firstTimeAttempt.IsPaused;
                        dbDailyQuizAttempt.TimeLeftInMinutes = firstTimeAttempt.TimeLeftInMinutes;
                        dbDailyQuizAttempt.IsCompleted = firstTimeAttempt.IsCompleted;
                        dbDailyQuizAttempt.EditBy = audiLogin;
                        dbDailyQuizAttempt.EditDate = DateTime.Now;
                        dbDailyQuizAttempt.FinalMarksScoredForRank = firstTimeAttempt.FinalMarksScoredForRank;

                        if (firstTimeAttempt.DailyQuizAttemptDetails != null && firstTimeAttempt.DailyQuizAttemptDetails.Count() > 0)
                        {
                            //// first removing
                            //DB.MockTestAttemptDetails.RemoveRange(dbMockTestAttempt.MockTestAttemptDetails);
                            //DB.SaveChanges();
                            //// and saving new list
                            //DB.MockTestAttemptDetails.AddRange(firstTimeAttempt.MockTestAttemptDetails);
                            //DB.SaveChanges();

                            foreach (var item in firstTimeAttempt.DailyQuizAttemptDetails)
                            {
                                var DBMockTestAttemptDetails = dbDailyQuizAttempt.DailyQuizAttemptDetails.Where(m => m.AttemptDetailId == item.AttemptDetailId).FirstOrDefault();
                                if (DBMockTestAttemptDetails != null)
                                {
                                    DBMockTestAttemptDetails.AnswerChoiceId = item.AnswerChoiceId;
                                    DBMockTestAttemptDetails.QuizQuestionAnswerChoiceId = item.QuizQuestionAnswerChoiceId;
                                    DBMockTestAttemptDetails.IsAnswerCorrect = item.IsAnswerCorrect;
                                    DBMockTestAttemptDetails.MarksScored = item.MarksScored;
                                    DBMockTestAttemptDetails.AnswerStatus = item.AnswerStatus;
                                }

                            }
                        }

                        DB.SaveChanges();
                        sr.ReturnId = dbDailyQuizAttempt.AttemptId;
                    }
                }
                catch (Exception ex)
                {
                    sr.AddError(ex.Message);
                }

            }
            return sr;

        }
        public static ServiceResponse SaveQuizUpload(DailyQuizUpload question)
        {
            ServiceResponse sr = new ServiceResponse();
            try
            {
                DB.DailyQuizUploads.Add(question);
                DB.SaveChanges();
                // Return
                sr.ReturnId = question.DailyQuizUploadId;
            }
            catch (Exception ex)
            {
                sr.AddError(ex.Message);
            }
            return sr;
        }
        public static ServiceResponse SaveQuestionQuizTest(List<QuizTest> questions, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            try
            {
                DB.QuizTests.AddRange(questions);
                DB.SaveChanges();
            }
            catch (Exception ex)
            {
                sr.AddError(ex.Message);
            }
            return sr;
        }
        public static List<DailyQuizSubject> DailyQuizSubjectList(string Lang, bool IsActive)
        {
            return DB.DailyQuizSubjects
                  .Where(m => m.LanguageCode == Lang && m.Isactive == IsActive)
                  .ToList();
        }
        public static List<DailyQuizSubject> DailyQuizAllSubjectList(string Lang)
        {
            return DB.DailyQuizSubjects
                  .Where(m => m.LanguageCode == Lang)
                  .ToList();
        }

        public static List<DailyQuiz> DailyQuizList(string Lang, bool IsActive)
        {
            return DB.DailyQuizs
                  .Where(m => m.LanguageCode == Lang && m.Isactive == IsActive)
                  .ToList();
        }

        public static List<DailyQuizUpload> DailyQuizUploadList(string Lang, bool IsActive)
        {
            return DB.DailyQuizUploads
                  .Where(m => m.Isactive == IsActive)
                  .ToList();
        }


        public static DailyQuizSubject FetchDailyQuizSubject(int subjectId)
        {
            return DB.DailyQuizSubjects
                  .Where(m => m.DailyQuizSubjectId == subjectId).FirstOrDefault();

        }
        public static ServiceResponse DeleteDailyQuizCourse(int courseId)
        {
            var sr = new ServiceResponse();

            try
            {
                if (!DailyQuizSubjectList("en-US", true).Any(m => m.DailyQuizCourseId == courseId))
                {
                    var course = Fetch(courseId);
                    DB.Entry(course).State = EntityState.Deleted;
                    DB.SaveChanges();
                }
                else
                {
                    var categoriesused = DailyQuizSubjectList("en-US", true).Where(m => m.DailyQuizCourseId == courseId).Select(m => m.DailyQuizSubjectName).ToList();
                    sr.AddError($"You can't delete this course as it is being used by courses such as { string.Join(",", categoriesused)}. If you want to delete, please delete these subjects first.");
                }

            }
            catch (Exception exception)
            {
                sr.AddError(exception.Message);
            }

            return sr;
        }
        public static ServiceResponse DeleteDailyQuizSubject(int subjectId)
        {
            var sr = new ServiceResponse();

            try
            {
                if (!DailyQuizList("en-US", true).Any(m => m.DailyQuizSubjectId == subjectId))
                {
                    var subject = FetchDailyQuizSubject(subjectId);
                    DB.Entry(subject).State = EntityState.Deleted;
                    DB.SaveChanges();
                }
                else
                {
                    var quizused = DailyQuizList("en-US", true).Where(m => m.DailyQuizSubjectId == subjectId).Select(m => m.DailyQuizName).ToList();
                    sr.AddError($"You can't delete this subject as it is being used by quiz such as { string.Join(",", quizused)}. If you want to delete, please delete these quizes first.");
                }

            }
            catch (Exception exception)
            {
                sr.AddError(exception.Message);
            }

            return sr;
        }

        public static ServiceResponse DeleteDailyQuiz(int dailyQuizId)
        {
            var sr = new ServiceResponse();

            try
            {
                if (!DailyQuizUploadList("en-US", true).Any(m => m.DailyQuizId == dailyQuizId))
                {
                    var dailyQuiz = FetchDailyQuiz(dailyQuizId);
                    DB.Entry(dailyQuiz).State = EntityState.Deleted;
                    DB.SaveChanges();
                }
                else
                {
                    var quizused = DailyQuizUploadList("en-US", true).Where(m => m.DailyQuizId == dailyQuizId).Any();
                    sr.AddError($"You can't delete this daily quiz as it is being used by quiz upload.  If you want to delete, please delete these quiz uploads first.");
                }

            }
            catch (Exception exception)
            {
                sr.AddError(exception.Message);
            }

            return sr;
        }
        public static ServiceResponse DeleteQuizUpload(int dailyQuizUploadId)
        {
            var sr = new ServiceResponse();

            try
            {
                var QuestionUpload = FetchDailyQuizUpload(dailyQuizUploadId);

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
        public static ServiceResponse ApproveQuizUpload(int dailyQuizUploadId)
        {
            var sr = new ServiceResponse();

            try
            {
                var QuestionUpload = FetchDailyQuizUpload(dailyQuizUploadId);

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
        public static ServiceResponse DenyQuizUpload(int dailyQuizUploadId)
        {
            var sr = new ServiceResponse();

            try
            {
                var QuestionUpload = FetchDailyQuizUpload(dailyQuizUploadId);

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

        internal static ServiceResponse EditSaveQuestionReview(QuizTest existingMockTest, List<QuizQuestionAnswerChoice> newquestionAnswerChoices, List<QuizQuestionPoint> newquestionPoint)
        {
            var sr = new ServiceResponse();
            try
            {
                // First remove and Add new Questions and Answers the update existing

                DB.QuizQuestionPoints.RemoveRange(existingMockTest.QuizQuestionPoints);
                DB.QuizQuestionAnswerChoices.RemoveRange(existingMockTest.QuizQuestionAnswerChoices);
                DB.SaveChanges();

                DB.QuizQuestionPoints.AddRange(newquestionPoint);
                DB.QuizQuestionAnswerChoices.AddRange(newquestionAnswerChoices);

                DB.SaveChanges();

                sr.ReturnId = existingMockTest.QuizTestId;
            }
            catch (Exception exception)
            {
                sr.AddError(exception.Message);
            }
            return sr;
        }

    }
}