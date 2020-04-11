using OnlineMasterG.Base;
using OnlineMasterG.CommonFramework;
using OnlineMasterG.CommonServices;
using OnlineMasterG.Models.DAL;
using OnlineMasterG.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace OnlineMasterG.DomainLogic
{
    public static class DailyQuizLogics
    {
        #region Validations

        public static ServiceResponse ValidateDailyQuizCourse(DailyQuizCourseVM model)
        {
            ServiceResponse sr = new ServiceResponse();

            if (String.IsNullOrEmpty(model.DailyQuizCourseName))
                sr.AddError("The [Course Name] field cannot be empty.");

            return sr;
        }
        public static ServiceResponse ValidateDailyQuiz(DailyQuizVM model)
        {
            ServiceResponse sr = new ServiceResponse();

            if (String.IsNullOrEmpty(model.DailyQuizName))
                sr.AddError("The [Quiz Name] field cannot be empty.");

            return sr;
        }
        public static ServiceResponse SaveDailyQuizCourse(DailyQuizCourseVM model, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            DailyQuizCourse course = new DailyQuizCourse()
            {
                DailyQuizCourseId = model.DailyQuizCourseId,
                DailyQuizCourseName = model.DailyQuizCourseName,
                Sequence = model.Sequence,
                LanguageCode = model.LanguageCode,
                Isactive = model.IsActive,
                CreateBy = auditlogin,
                CreateOn = DateTime.Now
            };
            sr = DailyQuizService.SaveDailyQuizCourse(course, auditlogin);

            return sr;
        }



        public static ServiceResponse ValidateQuizUpload(DailyQuizUploadVM model)
        {
            ServiceResponse sr = new ServiceResponse();
            // Verify that the user selected a file
            if (model.postedFile != null && model.postedFile.ContentLength > 0)
            {
                // extract only the fielname
                DataContent dataContent = new DataContent();
                DataFile dataFile = new DataFile();

                using (MemoryStream ms = new MemoryStream())
                {
                    model.postedFile.InputStream.CopyTo(ms);
                    dataContent.RawData = ms.GetBuffer();
                }

                dataFile.FileName = Path.GetFileName(model.postedFile.FileName);
                dataFile.Extension = Path.GetExtension(model.postedFile.FileName);
                dataFile.SourceCode = "QUIZ";
                dataFile.DataContent = dataContent;
                if (!String.IsNullOrEmpty(dataFile.Extension) && (dataFile.Extension != ".xls" && dataFile.Extension != ".xlsx"))
                    sr.AddError("Please upload a file with .xls or .xlsx extension.");
                if (!sr.Status)
                    return sr;
                sr = DataFileService.ValidateDataFileSetup(dataFile);

                return sr;
            }
            else
            {
                sr.AddError("The [Upload File] cannot be empty.");
            }



            return sr;
        }
        public static ServiceResponse SaveDailyQuiz(DailyQuizVM model, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            DailyQuiz quiz = new DailyQuiz()
            {
                DailyQuizId = model.DailyQuizId,
                DailyQuizName = model.DailyQuizName,
                DailyQuizCourseId = model.DailyQuizCourseId,
                DailyQuizSubjectId = model.DailyQuizSubjectId,
                NoOfQuestions = model.NoOfQuestions,
                Description = model.Description,
                LanguageCode = model.LanguageCode,
                TimeInMinutes = model.TimeInMinutes,
                Isactive = true,
                CreateBy = auditlogin,
                CreateOn = DateTime.Now
            };
            sr = DailyQuizService.SaveDailyQuiz(quiz, auditlogin);

            return sr;
        }

        public static ServiceResponse SaveQuizUpload(DailyQuizUploadVM model, byte[] bytes, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();

            int ExcelDataFileId;
            // Verify that the user selected a file
            if (model.postedFile == null || model.postedFile.ContentLength == 0)
            {
                sr.AddError("Please upload a file");
                return sr;
            }

            // extract only the fielname
            DataContent dataContent = new DataContent();
            DataFile dataFile = new DataFile();
            dataContent.RawData = bytes;
            dataFile.FileName = Path.GetFileName(model.postedFile.FileName);
            dataFile.Extension = Path.GetExtension(model.postedFile.FileName);
            dataFile.SourceCode = "QUESTIONS";
            dataFile.DataContent = dataContent;
            dataFile.CreateBy = auditlogin;
            dataFile.CreateDate = DateTime.Now;
            // Add file
            sr = DataFileService.AddDataFile(dataFile);

            if (!sr.Status)
                return sr;
            ExcelDataFileId = sr.ReturnId;

            // Get ExcelData from DataFile and Prepare for saving
            var questionTab = QuestionUploadLogics.GetDataTable(ExcelDataFileId, true);

            //Verify Existing Columns
            if (!questionTab.Columns.Contains("QuestionNumber"))
                sr.AddError("Column [QuestionNumber] is missing.");

            if (!questionTab.Columns.Contains("Question"))
                sr.AddError("Column [Question] is missing.");

            if (!questionTab.Columns.Contains("QuestionOption1"))
                sr.AddError("Column [QuestionOption1] is missing.");

            if (!questionTab.Columns.Contains("QuestionOption2"))
                sr.AddError("Column [QuestionOption2] is missing.");

            if (!questionTab.Columns.Contains("QuestionOption3"))
                sr.AddError("Column [QuestionOption3] is missing.");

            if (!questionTab.Columns.Contains("QuestionOption4"))
                sr.AddError("Column [QuestionOption4] is missing.");

            if (!questionTab.Columns.Contains("QuestionOption5"))
                sr.AddError("Column [QuestionOption5] is missing.");

            if (!questionTab.Columns.Contains("AnswerOption1"))
                sr.AddError("Column [AnswerOption1] is missing.");

            if (!questionTab.Columns.Contains("AnswerOption2"))
                sr.AddError("Column [AnswerOption2] is missing.");

            if (!questionTab.Columns.Contains("AnswerOption3"))
                sr.AddError("Column [AnswerOption3] is missing.");

            if (!questionTab.Columns.Contains("AnswerOption4"))
                sr.AddError("Column [AnswerOption4] is missing.");

            if (!questionTab.Columns.Contains("AnswerOption5"))
                sr.AddError("Column [AnswerOption5] is missing.");

            if (!questionTab.Columns.Contains("CorrectAnswer"))
                sr.AddError("Column [CorrectAnswer] is missing.");

            if (!questionTab.Columns.Contains("QuestionSet"))
                sr.AddError("Column [QuestionSet] is missing.");

            if (!questionTab.Columns.Contains("Description"))
                sr.AddError("Column [Description] is missing.");

            if (!questionTab.Columns.Contains("Solution"))
                sr.AddError("Column [Solution] is missing.");

            //bool anyQuestionsetEmpty = questionTab.Columns.

            //if (anyQuestionsetEmpty)
            //{
            //    sr.AddError("Column [QuestionSet] field can't be empty");
            //}

            if (!sr.Status)
            {
                return sr;
            }

            List<ExcelQuestions> excelQuestions = new List<ExcelQuestions>();
            excelQuestions = DataTableToList.ConvertDataTable<ExcelQuestions>(questionTab);

            // questionset
            var QuestionSet = from g in excelQuestions
                              group g by g.QuestionSet into QuestionSetGroup
                              select new { setKey = QuestionSetGroup.Key, Properties = QuestionSetGroup };



            DailyQuizUpload dailyQuizUpload = new DailyQuizUpload()
            {
                QuestionStatus = "PEN",
                LanguageCode = "en-US",
                Isactive = true,
                CreateBy = auditlogin,
                CreateOn = DateTime.Now,
                DailyQuizCourseId = model.DailyQuizCourseId,
                DailyQuizSubjectId = model.DailyQuizSubjectId,
                DailyQuizId = model.DailyQuizId,
                DataFileId = ExcelDataFileId,
            };


            sr = DailyQuizService.SaveQuizUpload(dailyQuizUpload);

            if (!sr.Status)
                return sr;

            int questionUploadId = sr.ReturnId;

            List<QuizTest> questionsMockTest = new List<QuizTest>();
            if (QuestionSet != null && QuestionSet.Count() > 0)
            {
                foreach (var qset in QuestionSet)
                {// each questionset

                    string Description = string.Empty;

                    foreach (var item in qset.Properties.Select((x, y) => new { Data = x, Index = y }))
                    {


                        int QuestionNumber = Convert.ToInt32(item.Data.QuestionNumber);
                        string Question = item.Data.Question;
                        string QuestionOption1 = item.Data.QuestionOption1;
                        string QuestionOption2 = item.Data.QuestionOption2;
                        string QuestionOption3 = item.Data.QuestionOption3;
                        string QuestionOption4 = item.Data.QuestionOption4;
                        string QuestionOption5 = item.Data.QuestionOption5;
                        string AnswerOption1 = item.Data.AnswerOption1;
                        string AnswerOption2 = item.Data.AnswerOption2;
                        string AnswerOption3 = item.Data.AnswerOption3;
                        string AnswerOption4 = item.Data.AnswerOption4;
                        string AnswerOption5 = item.Data.AnswerOption5;
                        string CorrectAnswer = item.Data.CorrectAnswer;
                        string Solution = item.Data.Solution;
                        bool IscorrectAnswer1 = CorrectAnswer == "AnswerOption1" ? true : false;
                        bool IscorrectAnswer2 = CorrectAnswer == "AnswerOption2" ? true : false;
                        bool IscorrectAnswer3 = CorrectAnswer == "AnswerOption3" ? true : false;
                        bool IscorrectAnswer4 = CorrectAnswer == "AnswerOption4" ? true : false;
                        bool IscorrectAnswer5 = CorrectAnswer == "AnswerOption5" ? true : false;
                        string QSet = item.Data.QuestionSet;
                        if (item.Index == 0)
                        {
                            Description = item.Data.Description;
                        }

                        List<QuizQuestionPoint> questionPoints = new List<QuizQuestionPoint>();
                        List<QuizQuestionAnswerChoice> questionAnswerChoices = new List<QuizQuestionAnswerChoice>();

                        // Question points
                        questionPoints.Add(new QuizQuestionPoint() { QuizQPoint = QuestionOption1 });
                        questionPoints.Add(new QuizQuestionPoint() { QuizQPoint = QuestionOption2 });
                        questionPoints.Add(new QuizQuestionPoint() { QuizQPoint = QuestionOption3 });
                        questionPoints.Add(new QuizQuestionPoint() { QuizQPoint = QuestionOption4 });
                        questionPoints.Add(new QuizQuestionPoint() { QuizQPoint = QuestionOption5 });
                        // AnserChices
                        questionAnswerChoices.Add(new QuizQuestionAnswerChoice() { QuizQuestionAnswer = AnswerOption1, ChoiceId = 1, IsCorrect = IscorrectAnswer1 });
                        questionAnswerChoices.Add(new QuizQuestionAnswerChoice() { QuizQuestionAnswer = AnswerOption2, ChoiceId = 2, IsCorrect = IscorrectAnswer2 });
                        questionAnswerChoices.Add(new QuizQuestionAnswerChoice() { QuizQuestionAnswer = AnswerOption3, ChoiceId = 3, IsCorrect = IscorrectAnswer3 });
                        questionAnswerChoices.Add(new QuizQuestionAnswerChoice() { QuizQuestionAnswer = AnswerOption4, ChoiceId = 4, IsCorrect = IscorrectAnswer4 });
                        questionAnswerChoices.Add(new QuizQuestionAnswerChoice() { QuizQuestionAnswer = AnswerOption5, ChoiceId = 5, IsCorrect = IscorrectAnswer5 });

                        questionsMockTest.Add(new QuizTest()
                        {
                            DailyQuizUploadId = questionUploadId,
                            LanguageCode = "en-US",
                            Isactive = true,
                            CreateBy = auditlogin,
                            CreateDate = DateTime.Now,
                            Question = Question,
                            QuestionNumber = QuestionNumber,
                            Description = Description,
                            QuestionSet = QSet,
                            Solution = Solution,
                            QuizQuestionAnswerChoices = questionAnswerChoices,
                            QuizQuestionPoints = questionPoints
                        });

                    }
                }
            }

            sr = DailyQuizService.SaveQuestionQuizTest(questionsMockTest, auditlogin);

            return sr;
        }

        public static ServiceResponse SaveEditQuestions(QuestionReviewEdit model, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            // Data we get in Grouped Questionsets
            // First convert those into single list. but Description need to save in corresponding questions.
            if (model != null)
            {
                string Description = string.Empty;
                int QuestionUploadId = 0;
                if (model.EditQuestionSet != null && model.EditQuestionSet.Count() > 0)
                {
                    List<QuestionsMockTestReview> SingleList = new List<QuestionsMockTestReview>();
                    foreach (var Qset in model.EditQuestionSet)
                    {
                        Description = Qset.Description;
                        QuestionUploadId = Qset.QuestionUploadId;
                        SingleList.AddRange(Qset.EditQuestions);
                    }
                    // Now we have single list of questions now prepare DB Model.

                    if (SingleList != null && SingleList.Count > 0)
                    {
                        foreach (var item in SingleList)
                        {
                            int CorrectAnswer = Convert.ToInt32(item.CorrectAnswer);
                            // Save data based on Question
                            var ExistingMockTest = DailyQuizService.FetchQuizTest(item.QuestionsMockTestId);
                            if (ExistingMockTest != null)
                            {
                                ExistingMockTest.Question = item.Question;
                                ExistingMockTest.QuizImageFileId = item.QuestionImageFileId;
                                ExistingMockTest.Solution = item.Solution;
                                ExistingMockTest.Description = Description;

                                // New List
                                List<QuizQuestionAnswerChoice> NewquestionAnswerChoices = new List<QuizQuestionAnswerChoice>();
                                List<QuizQuestionPoint> NewquestionPoint = new List<QuizQuestionPoint>();

                                if (item.EditQuestionPoints != null && item.EditQuestionPoints.Count() > 0)
                                {
                                    foreach (var qItem in item.EditQuestionPoints)
                                    {
                                        if (!string.IsNullOrEmpty(qItem.QPoint))
                                        {
                                            NewquestionPoint.Add(new QuizQuestionPoint()
                                            {
                                                QuizQPoint = qItem.QPoint,
                                                QuizTestId = item.QuestionsMockTestId,
                                            });
                                        }

                                    }
                                }
                                if (item.EditAnswerChoice != null && item.EditAnswerChoice.Count() > 0)
                                {
                                    foreach (var qItem in item.EditAnswerChoice)
                                    {
                                        int ChoiceId = Convert.ToInt32(qItem.ChoiceId);
                                        bool isCorrect = (ChoiceId == CorrectAnswer) ? true : false;

                                        if (!string.IsNullOrEmpty(qItem.QuestionAnswer))
                                        {
                                            NewquestionAnswerChoices.Add(new QuizQuestionAnswerChoice()
                                            {
                                                QuizQuestionAnswer = qItem.QuestionAnswer,
                                                IsCorrect = isCorrect,
                                                QuizTestId = item.QuestionsMockTestId,
                                                ChoiceId = ChoiceId
                                            });
                                        }

                                    }
                                }

                                DailyQuizService.EditSaveQuestionReview(ExistingMockTest, NewquestionAnswerChoices, NewquestionPoint);
                            }


                        }

                    }

                }
            }

            return sr;
        }

        public static ServiceResponse ValidateDailyQuizSubject(DailyQuizSubjectVM model)
        {
            ServiceResponse sr = new ServiceResponse();

            if (String.IsNullOrEmpty(model.DailyQuizSubjectName))
                sr.AddError("The [Subject Name] field cannot be empty.");

            return sr;
        }
        public static ServiceResponse SaveDailyQuizSubject(DailyQuizSubjectVM model, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            DailyQuizSubject dailyQuizSubject = new DailyQuizSubject()
            {
                DailyQuizSubjectId = model.DailyQuizSubjectId,
                DailyQuizSubjectName = model.DailyQuizSubjectName,
                DailyQuizCourseId = model.DailyQuizCourseId,
                LanguageCode = model.LanguageCode,
                Isactive = model.IsActive,
                CreateBy = auditlogin,
                CreateOn = DateTime.Now
            };
            sr = DailyQuizService.SaveDailyQuizSubject(dailyQuizSubject, auditlogin);

            return sr;
        }
        public static List<DailyQuizSubjectVM> SubjectList(string Lang)
        {
            List<DailyQuizSubjectVM> model = new List<DailyQuizSubjectVM>();
            var courses = DailyQuizService.DailyQuizAllSubjectList(Lang);
            if (courses != null && courses.Count() > 0)
            {
                foreach (var item in courses)
                {
                    model.Add(new DailyQuizSubjectVM()
                    {
                        DailyQuizSubjectId = item.DailyQuizSubjectId,
                        DailyQuizSubjectName = item.DailyQuizSubjectName,
                        DailyQuizCourseName = DailyQuizService.Fetch(item.DailyQuizCourseId)?.DailyQuizCourseName,
                        DailyQuizCourseId = item.DailyQuizCourseId,
                        IsActive = item.Isactive,
                        LanguageCode = item.LanguageCode

                    });
                }
            }
            return model;
        }
        public static List<DailyQuizVM> DailyQuizList(string Lang, bool IsActive)
        {
            List<DailyQuizVM> model = new List<DailyQuizVM>();
            var dailyQuizes = DailyQuizService.DailyQuizList(Lang, IsActive);
            if (dailyQuizes != null && dailyQuizes.Count() > 0)
            {
                foreach (var item in dailyQuizes)
                {
                    model.Add(new DailyQuizVM()
                    {
                        DailyQuizId = item.DailyQuizId,
                        DailyQuizSubjectId = item.DailyQuizSubjectId,
                        DailyQuizName = item.DailyQuizName,
                        DailyQuizSubjectName = DailyQuizService.FetchDailyQuizSubject(item.DailyQuizSubjectId)?.DailyQuizSubjectName,
                        DailyQuizCourseName = DailyQuizService.Fetch(item.DailyQuizCourseId)?.DailyQuizCourseName,
                        DailyQuizCourseId = item.DailyQuizCourseId,
                        NoOfQuestions = item.NoOfQuestions,
                        TimeInMinutes = item.TimeInMinutes,
                        Description = item.Description,
                        Isactive = item.Isactive,
                        LanguageCode = item.LanguageCode
                    });
                }
            }
            return model;
        }
        public static List<DailyQuizUploadVM> DailyQuizUploadList(string Lang, bool IsActive)
        {
            List<DailyQuizUploadVM> model = new List<DailyQuizUploadVM>();
            var dailyQuizUploades = DailyQuizService.DailyQuizUploadList(Lang, IsActive);
            if (dailyQuizUploades != null && dailyQuizUploades.Count() > 0)
            {
                foreach (var item in dailyQuizUploades)
                {
                    model.Add(new DailyQuizUploadVM()
                    {
                        DailyQuizUploadId = item.DailyQuizUploadId,
                        DailyQuizId = item.DailyQuizId,
                        DailyQuizSubjectId = item.DailyQuizSubjectId,
                        DailyQuizName = DailyQuizService.FetchDailyQuiz(item.DailyQuizId)?.DailyQuizName,
                        DailyQuizSubjectName = DailyQuizService.FetchDailyQuizSubject(item.DailyQuizSubjectId)?.DailyQuizSubjectName,
                        DailyQuizCourseName = DailyQuizService.Fetch(item.DailyQuizCourseId)?.DailyQuizCourseName,
                        DailyQuizCourseId = item.DailyQuizCourseId,
                        Isactive = item.Isactive,
                        DataFileId = item.DataFileId,
                        QuestionStatus = item.QuestionStatus
                    }); ;
                }
            }
            return model;
        }
        public static DailyQuizAttemptVM GetDailyQuizAttemptDetails(string Login, int DailyQuizId, string auditlogin)
        {
            DailyQuizAttemptVM model = new DailyQuizAttemptVM();

            var TestAttepts = DailyQuizService.GetDailyQuizAttemptListByLoginAndTestId(Login, DailyQuizId);
            // if list not empty, take first not completed attempt order by create date desc
            if (TestAttepts != null && TestAttepts.Count() > 0)
            {
                var FirstNotCompletedAttempt = TestAttepts.Where(m => m.IsCompleted == false).OrderByDescending(m => m.CreateDate).FirstOrDefault();
                if (FirstNotCompletedAttempt != null)
                {
                    model = SetAttemptVMModel(FirstNotCompletedAttempt, DailyQuizId);
                }
                else
                {
                    // create a dummy data / insert data into Mock ttest attempt table then return that table
                    var CreateAnotherAttempt = GetFirstAttemptDailyQuiz(Login, DailyQuizId);
                    var sr = DailyQuizService.SaveDailyQuizAttempt(CreateAnotherAttempt, auditlogin);
                    if (sr.Status)
                    {
                        var newData = DailyQuizService.GetDailyQuizAttempt(sr.ReturnId);
                        model = SetAttemptVMModel(newData, DailyQuizId);
                    }
                    else
                    {
                        return null;
                    }
                }

            }
            else
            {
                // create a dummy data / insert data into Mock ttest attempt table then return that table
                var firstTimeAttempt = GetFirstAttemptDailyQuiz(Login, DailyQuizId);
                var sr = DailyQuizService.SaveDailyQuizAttempt(firstTimeAttempt, auditlogin);
                if (sr.Status)
                {
                    var newData = DailyQuizService.GetDailyQuizAttempt(sr.ReturnId);
                    model = SetAttemptVMModel(newData, DailyQuizId);
                }
                else
                {
                    return null;
                }
            }

            return model;
        }
        private static DailyQuizAttempt GetFirstAttemptDailyQuiz(string login, int? DailyQuizId)
        {
            var Quiz = DailyQuizService.FetchDailyQuiz(DailyQuizId);

            DailyQuizAttempt dailyQuizAttempt = new DailyQuizAttempt();
            List<DailyQuizAttemptDetail> dailyQuizAttemptDetails = new List<DailyQuizAttemptDetail>();

            dailyQuizAttempt.Login = login;
            dailyQuizAttempt.DailyQuizId = DailyQuizId;
            dailyQuizAttempt.IsPaused = false;
            dailyQuizAttempt.IsCompleted = false;
            dailyQuizAttempt.CreateBy = login;
            dailyQuizAttempt.CreateDate = DateTime.Now;
            dailyQuizAttempt.TimeLeftInMinutes = Quiz != null ? Quiz.TimeInMinutes : 0;


            if (AppInfo.GetQuizTests != null && AppInfo.GetQuizTests.Count() > 0)
            {
                var QuestionBasedOnQuiz = AppInfo.GetQuizTests.Where(m => m.DailyQuizUpload?.DailyQuizId == DailyQuizId).ToList();

                foreach (var Q in QuestionBasedOnQuiz.OrderBy(m => m.QuestionNumber).Select((x, y) => new { Data = x, Index = y }))
                {
                    DailyQuizAttemptDetail detail = new DailyQuizAttemptDetail();

                    detail.QuizTestId = Q.Data.QuizTestId;
                    detail.DailyQuizCourseId = Q.Data.DailyQuizUpload?.DailyQuizCourseId;
                    detail.DailyQuizSubjectId = Q.Data.DailyQuizUpload?.DailyQuizSubjectId;
                    detail.MarksScored = 0;
                    detail.AnswerStatus = ExamLogics.AnswerStatus.NOTATTEMPTED.ToString();
                    detail.QuestionNumber = Q.Data.QuestionNumber;
                    dailyQuizAttemptDetails.Add(detail);
                }
            }

            dailyQuizAttempt.DailyQuizAttemptDetails = dailyQuizAttemptDetails;
            return dailyQuizAttempt;

        }
        private static DailyQuizAttemptVM SetAttemptVMModel(DailyQuizAttempt firstNotCompletedAttempt, int? DailyQuizId)
        {
            DailyQuizAttemptVM model = new DailyQuizAttemptVM();

            if (firstNotCompletedAttempt != null)
            {
                model.AttemptId = firstNotCompletedAttempt.AttemptId;
                model.DailyQuizId = firstNotCompletedAttempt.DailyQuizId;
                model.Login = firstNotCompletedAttempt.Login;
                model.IsPaused = firstNotCompletedAttempt.IsPaused;
                model.TimeLeftInMinutes = firstNotCompletedAttempt.TimeLeftInMinutes;
                model.IsCompleted = firstNotCompletedAttempt.IsCompleted;
                model.QuizName = DailyQuizService.FetchDailyQuiz(firstNotCompletedAttempt.DailyQuizId)?.DailyQuizName;

                if (firstNotCompletedAttempt.DailyQuizAttemptDetails != null && firstNotCompletedAttempt.DailyQuizAttemptDetails.Count() > 0)
                {
                    List<DailyQuizAttemptVMDetailVM> mockTestAttemptDetails = new List<DailyQuizAttemptVMDetailVM>();
                    foreach (var item in firstNotCompletedAttempt.DailyQuizAttemptDetails)
                    {
                        var OriginalQuestion = DailyQuizService.GetQuizQuestionsBasedOnTestAndSubject(DailyQuizId, item.DailyQuizSubjectId).Where(m => m.QuestionNumber == item.QuestionNumber && m.DailyQuizUpload.QuestionStatus == "VAL").FirstOrDefault();
                        List<QuestionAnswerChoiceVM> questionAnswerChoiceVMs = new List<QuestionAnswerChoiceVM>();
                        List<QuestionPointVM> QuestionPointVMs = new List<QuestionPointVM>();

                        if (OriginalQuestion != null && OriginalQuestion.QuizQuestionAnswerChoices != null && OriginalQuestion.QuizQuestionAnswerChoices.Count() > 0)
                        {
                            foreach (var Ans in OriginalQuestion.QuizQuestionAnswerChoices)
                            {
                                questionAnswerChoiceVMs.Add(new QuestionAnswerChoiceVM()
                                {
                                    QuestionAnswer = Ans.QuizQuestionAnswer,
                                    QuestionAnswerChoiceId = Ans.QuizQuestionAnswerChoiceId,
                                    ChoiceId = Ans.ChoiceId,
                                    IsCorrect = Ans.IsCorrect
                                });
                            }
                        }

                        if (OriginalQuestion != null && OriginalQuestion.QuizQuestionPoints != null && OriginalQuestion.QuizQuestionPoints.Count() > 0)
                        {
                            foreach (var Ques in OriginalQuestion.QuizQuestionPoints)
                            {
                                QuestionPointVMs.Add(new QuestionPointVM()
                                {
                                    QuestionPointId = Ques.QuizQuestionPointId,
                                    QPoint = Ques.QuizQPoint
                                });
                            }
                        }

                        QuizTestVM Question = new QuizTestVM();
                        if (OriginalQuestion != null)
                        {
                            Question.Description = OriginalQuestion.Description;
                            Question.Isactive = OriginalQuestion.Isactive;
                            Question.Question = OriginalQuestion.Question;
                            Question.LanguageCode = OriginalQuestion?.LanguageCode;
                            Question.QuestionNumber = OriginalQuestion.QuestionNumber;
                            Question.QuestionSet = OriginalQuestion.QuestionSet;
                            Question.QuizTestId = OriginalQuestion.QuizTestId;
                            Question.QuizImageFileId = OriginalQuestion.QuizImageFileId;
                            Question.Solution = OriginalQuestion.Solution;
                            Question.QuestionPoint = QuestionPointVMs;
                            Question.QuestionAnswerChoice = questionAnswerChoiceVMs;
                        }

                        mockTestAttemptDetails.Add(new DailyQuizAttemptVMDetailVM()
                        {
                            AttemptDetailId = item.AttemptDetailId,
                            AttemptId = item.AttemptId,
                            DailyQuizCourseId = item.DailyQuizCourseId,
                            DailyQuizSubjectId = item.DailyQuizSubjectId,
                            QuizTestId = item.QuizTestId,
                            QuestionNumber = item.QuestionNumber,
                            QuizQuestionAnswerChoiceId = item.QuizQuestionAnswerChoiceId,
                            AnswerChoiceId = item.AnswerChoiceId,
                            AnswerStatus = item.AnswerStatus,
                            MarksScored = item.MarksScored,
                            QuizTest = Question

                        });
                    }
                    model.dailyQuizAttemptVMDetail = mockTestAttemptDetails;
                }

            }

            return model;
        }

        internal static ServiceResponse SaveDailyQuizAttempts(DailyQuizAttemptVM model, string auditLogin)
        {
            ServiceResponse sr = new ServiceResponse();
            try
            {
                if (model != null)
                {
                    DailyQuizAttempt dailyQuizAttempt = new DailyQuizAttempt();

                    int? AttemptId = model.dailyQuizAttemptVMDetail?.FirstOrDefault()?.AttemptId;
                    dailyQuizAttempt.AttemptId = AttemptId.Value;
                    dailyQuizAttempt.IsPaused = model.IsPaused;
                    dailyQuizAttempt.IsCompleted = model.IsCompleted;
                    dailyQuizAttempt.TimeLeftInMinutes = model.TimeLeftInMinutes;

                    List<DailyQuizAttemptDetail> dailyQuizAttemptDetailList = new List<DailyQuizAttemptDetail>();

                    if (model.dailyQuizAttemptVMDetail != null && model.dailyQuizAttemptVMDetail.Count() > 0)
                    {
                        foreach (var item in model.dailyQuizAttemptVMDetail)
                        {
                            DailyQuizAttemptDetail dailyQuizAttemptDetail = new DailyQuizAttemptDetail();
                            dailyQuizAttemptDetail.QuizQuestionAnswerChoiceId = item.QuizQuestionAnswerChoiceId;
                            dailyQuizAttemptDetail.AnswerChoiceId = item.AnswerChoiceId;
                            dailyQuizAttemptDetail.QuestionNumber = item.QuestionNumber;
                            dailyQuizAttemptDetail.AnswerStatus = item.AnswerStatus;
                            dailyQuizAttemptDetail.QuizTestId = item.QuizTestId;
                            dailyQuizAttemptDetail.AttemptDetailId = item.AttemptDetailId;
                            dailyQuizAttemptDetail.IsAnswerCorrect = GetIsCorrectAnswer(item.QuizTestId, item.QuizQuestionAnswerChoiceId);
                            dailyQuizAttemptDetail.MarksScored = GetMarksScored(dailyQuizAttemptDetail.IsAnswerCorrect, model.TestId, item.DailyQuizSubjectId);

                            dailyQuizAttemptDetailList.Add(dailyQuizAttemptDetail);
                        }
                    }

                    dailyQuizAttempt.DailyQuizAttemptDetails = dailyQuizAttemptDetailList;
                    dailyQuizAttempt.FinalMarksScoredForRank = dailyQuizAttemptDetailList.Sum(m => m.MarksScored);
                    sr = DailyQuizService.SaveDailyQuizAttempt(dailyQuizAttempt, auditLogin);

                }
                else
                {
                    sr.AddError("Sorry. There is a problem in processing your request.");
                }

            }
            catch (Exception ex)
            {

                sr.AddError(ex.Message);
            }



            return sr;
        }
        private static bool GetIsCorrectAnswer(int? QuizTestId, int? choosenAnswerChoiceId)
        {
            bool isCorrect = false;
            if (QuizTestId.HasValue && choosenAnswerChoiceId.HasValue)
            {
                var QuestionCorrectAnswerId = DailyQuizService.FetchQuizTest(QuizTestId).QuizQuestionAnswerChoices.Where(m => m.IsCorrect == true).FirstOrDefault().QuizQuestionAnswerChoiceId;
                isCorrect = QuestionCorrectAnswerId == choosenAnswerChoiceId ? true : false;
            }
            return isCorrect;
        }
        private static decimal GetMarksScored(bool isAnswerCorrect, int? testId, int? subjectId)
        {
            decimal markScored = 0;
            decimal? CorrectMark = 1;
            decimal? NegetiveMark = 0;

            if (isAnswerCorrect)
            {
                markScored = CorrectMark.HasValue ? CorrectMark.Value : 1;
            }
            else
            {
                markScored = NegetiveMark.HasValue ? Decimal.Negate(NegetiveMark.Value) : 0;
            }

            return markScored;
        }
        #endregion
    }
}