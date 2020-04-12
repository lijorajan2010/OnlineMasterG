using OnlineMasterG.Base;
using OnlineMasterG.Code;
using OnlineMasterG.CommonFramework;
using OnlineMasterG.CommonServices;
using OnlineMasterG.Models.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace OnlineMasterG.Controllers
{
    [ActionAuthorization()]
    public class FileInputResponseModel
    {
        public string error { get; set; }
        public List<string> initialPreview { get; set; }
        public List<object> initialPreviewConfig { get; set; }
    }


    public class DataFileController : BaseController
    {
        #region Actions


        [HttpPost]
        public JsonResult Upload(HttpPostedFileBase postedFile)
        {
            // Verify that the user selected a file
            if (postedFile != null && postedFile.ContentLength > 0)
            {
                // extract only the fielname
                DataContent dataContent = new DataContent();
                DataFile dataFile = new DataFile();

                using (MemoryStream ms = new MemoryStream())
                {
                    postedFile.InputStream.CopyTo(ms);
                    dataContent.RawData = ms.GetBuffer();
                }

                dataFile.FileName = Path.GetFileName(postedFile.FileName);
                dataFile.Extension = Path.GetExtension(postedFile.FileName);
                dataFile.SourceCode = "QUESTIMG";
                dataFile.DataContent = dataContent;
                dataFile.CreateBy = HttpContext.User.Identity.Name;
                dataFile.CreateDate = DateTime.Now;

                // Add file
                DataFileService.AddDataFile(dataFile);

                var response = new
                {
                    DataFileId = dataFile.DataFileId,
                    FileName = dataFile.FileName,
                    Extension = dataFile.Extension,
                    Status = true
                };

                return GetJsonResult(response);
            }
            else
            {
                return GetJsonResult(null);
            }
        }

        [HttpPost]
        public JsonResult UploadImage(HttpPostedFileBase postedFile)
        {
            // Verify that the user selected a file
            if (postedFile != null && postedFile.ContentLength > 0)
            {
                // extract only the fielname
                DataContent dataContent = new DataContent();
                DataFile dataFile = new DataFile();

                using (MemoryStream ms = new MemoryStream())
                {
                    postedFile.InputStream.CopyTo(ms);
                    dataContent.RawData = ms.GetBuffer();
                }

                dataFile.FileName = Path.GetFileName(postedFile.FileName);
                dataFile.Extension = Path.GetExtension(postedFile.FileName);
                dataFile.SourceCode = "QUESTIMG";
                dataFile.DataContent = dataContent;
                dataFile.CreateBy = HttpContext.User.Identity.Name;
                dataFile.CreateDate = DateTime.Now;

                // Add file
                DataFileService.AddDataFile(dataFile);

                var response = new
                {
                    DataFileId = dataFile.DataFileId,
                    FileName = dataFile.FileName,
                    Extension = dataFile.Extension,
                    Status = true,
                    Preview = "" + Url.Action("View", "DataFile", new { p = CustomEncrypt.SafeUrlEncrypt(dataFile.DataFileId.ToString()) }) + ""
                };

                return GetJsonResult(response);
            }
            else
            {
                return GetJsonResult(null);
            }
        }
        [HttpPost]
        public JsonResult UploadLogo(IEnumerable<HttpPostedFileBase> postedFiles)
        {
            var response = new FileInputResponseModel();
            response.initialPreview = new List<string>();
            response.initialPreviewConfig = new List<object>();

            if (postedFiles == null)
                postedFiles = Request.Files.Cast<string>().Select(f => Request.Files[f]).ToList();
            else
                postedFiles = postedFiles.ToList();


            foreach (var postedFile in postedFiles)
            {
                var file = new DataFile();
                file.DataContent = new DataContent();
                file.FileName = postedFile.FileName;
                file.Extension = Path.GetExtension(postedFile.FileName);
                file.SourceCode = "LOGO";
                file.CreateBy = HttpContext.User.Identity.Name;
                file.CreateDate = DateTime.Now;

                using (var binaryReader = new BinaryReader(postedFile.InputStream))
                {
                    file.DataContent.RawData = binaryReader.ReadBytes(postedFile.ContentLength);
                }

                var sr = DataFileService.AddDataFile(file);
                if (!sr.Status)
                    return Json(response);

                response.initialPreview.Add("<img src='" + Url.Action("View", "DataFile", new { p = CustomEncrypt.SafeUrlEncrypt(file.DataFileId.ToString()) }) + "' class='file-preview-image js-partner-logo' data-file-id='" + file.DataFileId + "' style='max-width: 200px; margin: 0 auto; display: block;'>");
                response.initialPreviewConfig.Add(new
                {
                    key = file.DataFileId,
                });
            }

            return Json(response);
        }
        public ActionResult Download(string dataFileKey)
        {
            // Get DataFileId from encrypted key
            int dataFileId = CustomEncrypt.Decrypt(HttpHelper.SafeUrlDecode(dataFileKey)).ToInt();

            if (dataFileId < 0)
                return null;

            // Load data file from database
            DataFile dataFile = DataFileService.LoadData(dataFileId);

            // Return file
            return File(dataFile.DataContent.RawData, MediaTypeNames.Application.Octet, dataFile.FileName);
        }

        public ActionResult ViewInTab(string p)
        {
            int dataFileId = CustomEncrypt.SafeUrlDecrypt(p).ToInt();

            if (dataFileId < 0)
                return null;

            DataFile dataFile = DataFileService.LoadData(dataFileId);

            string contentType = "";
            if (dataFile == null)
            {
                return null;
            }
            else
            {
                if (dataFile.Extension.StartsWith("PDF"))
                {
                    contentType = "application/pdf";
                }
                else if (dataFile.Extension.StartsWith("JPG"))
                {
                    contentType = "image/jpeg";
                }
                else if (dataFile.Extension.StartsWith("JPEG"))
                {
                    contentType = "image/jpeg";
                }
                else if (dataFile.Extension.StartsWith("PNG"))
                {
                    contentType = "image/png";
                }
                else if (dataFile.Extension.StartsWith("DOC"))
                {
                    contentType = "application/msword";
                }
                else if (dataFile.Extension.StartsWith("XLS"))
                {
                    contentType = "application/msexcel";
                }
                else if (dataFile.Extension.StartsWith("TXT"))
                {
                    contentType = "text/plain";
                }
                else if (dataFile.Extension.StartsWith("CSV"))
                {
                    contentType = "text/plain";
                }

                if (string.IsNullOrEmpty(contentType))
                {
                    return File(dataFile.DataContent.RawData, System.Net.Mime.MediaTypeNames.Application.Octet, dataFile.FileName);
                }

                Response.AppendHeader("Content-Disposition", "inline; filename=" + dataFile.FileName);
                return File(dataFile.DataContent.RawData, contentType);
            }
        }

        public new FileContentResult View(string p)
        {
            int dataFileId = CustomEncrypt.SafeUrlDecrypt(p).ToInt();

            if (dataFileId < 0)
                return null;

            DataFile dataFile = DataFileService.LoadData(dataFileId);

            string contentType = "";
            if (dataFile == null)
            {
                return null;
            }
            else
            {
                if (dataFile.Extension.StartsWith("PDF"))
                {
                    contentType = "application/pdf";
                }
                else if (dataFile.Extension.StartsWith("JPG"))
                {
                    contentType = "image/jpeg";
                }
                else if (dataFile.Extension.StartsWith("JPEG"))
                {
                    contentType = "image/jpeg";
                }
                else if (dataFile.Extension.StartsWith("PNG"))
                {
                    contentType = "image/png";
                }
                else if (dataFile.Extension.StartsWith("DOC"))
                {
                    contentType = "application/msword";
                }
                else if (dataFile.Extension.StartsWith("XLS"))
                {
                    contentType = "application/msexcel";
                }
                else if (dataFile.Extension.StartsWith("TXT"))
                {
                    contentType = "text/plain";
                }
                else if (dataFile.Extension.StartsWith("CSV"))
                {
                    contentType = "text/plain";
                }

                if (string.IsNullOrEmpty(contentType))
                {
                    return File(dataFile.DataContent.RawData, System.Net.Mime.MediaTypeNames.Application.Octet, dataFile.FileName);
                }

                Response.AppendHeader("Content-Disposition", "inline; filename=" + dataFile.FileName);
                return File(dataFile.DataContent.RawData, contentType);
            }

        }
        #endregion
    }
}