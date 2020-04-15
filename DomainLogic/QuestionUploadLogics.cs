
using Excel;
using OfficeOpenXml;
using OnlineMasterG.Base;
using OnlineMasterG.CommonFramework;
using OnlineMasterG.CommonServices;
using OnlineMasterG.Models.DAL;
using OnlineMasterG.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace OnlineMasterG.DomainLogic
{
    public static class QuestionUploadLogics
    {
        #region Validations

        public static ServiceResponse ValidateQuestionUpload(QuestionUploadVM model)
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
                dataFile.SourceCode = "QUESTIONS";
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
        public static ServiceResponse SaveQuestionUpload(QuestionUploadVM model, byte[] bytes, string auditlogin)
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
            var questionTab = GetDataTable(ExcelDataFileId, true);

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

            if (!questionTab.Columns.Contains("Direction"))
                sr.AddError("Column [Direction] is missing.");

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



            QuestionUpload questionUpload = new QuestionUpload()
            {
                QuestionStatus = "PEN",
                LanguageCode = "en-US",
                Isactive = true,
                CreateBy = auditlogin,
                CreateDate = DateTime.Now,
                CourseId = model.CourseId,
                CategoryId = model.CategoryId,
                SectionId = model.SectionId,
                TestId = model.TestId,
                SubjectId = model.SubjectId,
                DataFileId = ExcelDataFileId,
            };

            sr = QuestionUploadService.SaveQuestionUpload(questionUpload);

            if (!sr.Status)
                return sr;

            int questionUploadId = sr.ReturnId;

            List<QuestionsMockTest> questionsMockTest = new List<QuestionsMockTest>();
            if (QuestionSet != null && QuestionSet.Count() > 0)
            {
                foreach (var qset in QuestionSet)
                {// each questionset

                    string Description = string.Empty;
                    string Direction = string.Empty;

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
                            Direction = item.Data.Direction;
                        }

                        List<QuestionPoint> questionPoints = new List<QuestionPoint>();
                        List<QuestionAnswerChoice> questionAnswerChoices = new List<QuestionAnswerChoice>();

                        // Question points
                        questionPoints.Add(new QuestionPoint() { QPoint = QuestionOption1 });
                        questionPoints.Add(new QuestionPoint() { QPoint = QuestionOption2 });
                        questionPoints.Add(new QuestionPoint() { QPoint = QuestionOption3 });
                        questionPoints.Add(new QuestionPoint() { QPoint = QuestionOption4 });
                        questionPoints.Add(new QuestionPoint() { QPoint = QuestionOption5 });
                        // AnserChices
                        questionAnswerChoices.Add(new QuestionAnswerChoice() { QuestionAnswer = AnswerOption1, ChoiceId = 1, IsCorrect = IscorrectAnswer1 });
                        questionAnswerChoices.Add(new QuestionAnswerChoice() { QuestionAnswer = AnswerOption2, ChoiceId = 2, IsCorrect = IscorrectAnswer2 });
                        questionAnswerChoices.Add(new QuestionAnswerChoice() { QuestionAnswer = AnswerOption3, ChoiceId = 3, IsCorrect = IscorrectAnswer3 });
                        questionAnswerChoices.Add(new QuestionAnswerChoice() { QuestionAnswer = AnswerOption4, ChoiceId = 4, IsCorrect = IscorrectAnswer4 });
                        questionAnswerChoices.Add(new QuestionAnswerChoice() { QuestionAnswer = AnswerOption5, ChoiceId = 5, IsCorrect = IscorrectAnswer5 });

                        questionsMockTest.Add(new QuestionsMockTest()
                        {
                            QuestionUploadId = questionUploadId,
                            LanguageCode = "en-US",
                            Isactive = true,
                            CreateBy = auditlogin,
                            CreateDate = DateTime.Now,
                            Question = Question,
                            QuestionNumber = QuestionNumber,
                            Description = Description,
                            QuestionSet = QSet,
                            Solution = Solution,
                            QuestionAnswerChoices = questionAnswerChoices,
                            QuestionPoints = questionPoints,
                            Direction =Direction
                        });

                    }
                }
            }

            sr = QuestionUploadService.SaveQuestionMockTest(questionsMockTest, auditlogin);

            return sr;
        }

        public static DataTable GetDataTable(int excelDataFileId, bool hasHeader)
        {
            var dataFile = DataFileService.LoadData(excelDataFileId);
            using (var pck = new ExcelPackage())
            {
                using (var stream = new MemoryStream(dataFile.DataContent.RawData))
                {
                    pck.Load(stream);
                }

                var ws = pck.Workbook.Worksheets.First();
                DataTable tbl = new DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }
                var startRow = hasHeader ? 2 : 1;
                for (var rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    var row = tbl.NewRow();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                    tbl.Rows.Add(row);
                }
                return tbl;
            }
        }
        public static List<QuestionUploadVM> QuestionUploadList(string Lang, bool IsActive)
        {
            List<QuestionUploadVM> model = new List<QuestionUploadVM>();
            var uploads = QuestionUploadService.QuestionUploadList(Lang, IsActive).Where(m => !m.QuestionStatus.Contains("DEL")).ToList();
            if (uploads != null && uploads.Count() > 0)
            {
                foreach (var item in uploads)
                {
                    model.Add(new QuestionUploadVM()
                    {
                        QuestionUploadId = item.QuestionUploadId,
                        CourseId = item.CourseId,
                        CategoryId = item.CategoryId,
                        SectionId = item.SectionId,
                        TestId = item.TestId,
                        SubjectId = item.SubjectId,
                        CourseName = CourseService.Fetch(item.CourseId)?.CourseName,
                        CategoryName = CategoryService.Fetch(item.CategoryId)?.CategoryName,
                        SectionName = SectionService.Fetch(item.SectionId)?.SectionName,
                        TestName = TestService.Fetch(item.TestId)?.TestName,
                        SubjectName = SubjectService.Fetch(item.SubjectId)?.SubjectName,
                        QuestionStatus = item.QuestionStatus,
                        IsActive = item.Isactive,
                        LanguageCode = item.LanguageCode,
                        CreateOn = item.CreateDate

                    });
                }
            }
            return model;
        }
        public static ServiceResponse SaveEditQuestions(QuestionReviewEdit model, string auditlogin)
        {
            ServiceResponse sr = new ServiceResponse();
            // Data we get in Grouped Questionsets
            // First convert those into single list. but Description need to save in corresponding questions.
            if (model != null)
            {
                string Description = string.Empty;
                string Direction = string.Empty;
                int QuestionUploadId = 0;
                if (model.EditQuestionSet != null && model.EditQuestionSet.Count() > 0)
                {
                    List<QuestionsMockTestReview> SingleList = new List<QuestionsMockTestReview>();
                    foreach (var Qset in model.EditQuestionSet)
                    {
                        Description = Qset.Description;
                        Direction = Qset.Direction;
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
                            var ExistingMockTest = QuestionUploadService.FetchQuestionsMock(item.QuestionsMockTestId);
                            if (ExistingMockTest != null)
                            {
                                ExistingMockTest.Question = item.Question;
                                ExistingMockTest.QuestionImageFileId = item.QuestionImageFileId;
                                ExistingMockTest.Solution = item.Solution;
                                ExistingMockTest.Description = Description;
                                ExistingMockTest.Direction = Direction;

                                // New List
                                List<QuestionAnswerChoice> NewquestionAnswerChoices = new List<QuestionAnswerChoice>();
                                List<QuestionPoint> NewquestionPoint = new List<QuestionPoint>();

                                if (item.EditQuestionPoints!=null && item.EditQuestionPoints.Count()>0)
                                {
                                    foreach (var qItem in item.EditQuestionPoints)
                                    {
                                        if (!string.IsNullOrEmpty(qItem.QPoint))
                                        {
                                            NewquestionPoint.Add(new QuestionPoint()
                                            {
                                                QPoint = qItem.QPoint,
                                                QuestionsMockTestId = item.QuestionsMockTestId,
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
                                            NewquestionAnswerChoices.Add(new QuestionAnswerChoice()
                                            {
                                                QuestionAnswer = qItem.QuestionAnswer,
                                                IsCorrect = isCorrect,
                                                QuestionsMockTestId = item.QuestionsMockTestId,
                                                ChoiceId = ChoiceId
                                            });
                                        }

                                    }
                                }

                                QuestionUploadService.EditSaveQuestionReview(ExistingMockTest, NewquestionAnswerChoices, NewquestionPoint);
                            }


                        }

                    }

                }
            }

            return sr;
        }

        #endregion
    }
}