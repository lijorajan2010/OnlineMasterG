using OnlineMasterG.Base;
using OnlineMasterG.Models.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace OnlineMasterG.CommonServices
{
    public static class DataFileService
    {
        public static OnlinemasterjiEntities DB { get; private set; }
        static DataFileService()
        {
            DB = new OnlinemasterjiEntities();
        }
        #region Lists & Fetchs

        public static DataFile Fetch(int dataFileId)
        {
            return DB
                .DataFiles
                .Where(x => x.DataFileId == dataFileId)
                .FirstOrDefault();
        }
        public static DataFile FetchNullData(int? dataFileId)
        {
            return DB
                .DataFiles
                .Where(x => x.DataFileId == (dataFileId.HasValue?dataFileId:0))
                .FirstOrDefault();
        }

        public static DataFile GetDataByFileName(string fileName, string extension, string sourceCode)
        {
            return DB
                 .DataFiles
                 .AsNoTracking()
                 .Where(x => (x.FileName == fileName && x.Extension == extension && x.SourceCode == sourceCode))
                 .FirstOrDefault();
        }

        public static DataFile GetData(int dataFileId)
        {
            return DB
                .DataFiles
                .AsNoTracking()
                .Where(x => x.DataFileId == dataFileId)
                .FirstOrDefault();
        }

        public static DataFile LoadData(int dataFileId)
        {
            return DB
                .DataFiles
                .Where(x => x.DataFileId == dataFileId)
                .FirstOrDefault();
        }

        public static List<DataFile> Search(DataFileSearchParams p)
        {
            return DB
                .DataFiles
                .AsNoTracking()
                .Where(x => string.IsNullOrEmpty(p.FileName) || x.FileName == p.FileName).ToList();
        }

        public static bool Exists(DataFileSearchParams p)
        {
            var query = DB
                .DataFiles
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(p.FileName))
                query = query.Where(x => x.FileName == p.FileName);

            return query.Any();
        }
        #endregion

        #region Setup

        public static ServiceResponse AddDataFile(DataFile dataFile)
        {
            ServiceResponse sr = ValidateDataFileSetup(dataFile);

            if (!sr.Status)
                return sr;

            DB.DataFiles.Add(dataFile);
            DB.SaveChanges();

            sr.ReturnId = dataFile.DataFileId;

            return sr;
        }

        public static ServiceResponse UpdateDataFile(DataFile dataFile)
        {
            ServiceResponse sr = ValidateDataFileSetup(dataFile, true);

            if (!sr.Status)
                return sr;

            // Existing File
            var dbDataFile = DB.DataFiles.FirstOrDefault(x => x.DataFileId == dataFile.DataFileId);

            if (dbDataFile != null)
            {
                dbDataFile.FileName = dataFile.FileName;
                dbDataFile.Extension = dataFile.Extension;
                dbDataFile.SourceCode = dataFile.SourceCode;
                dbDataFile.DataContent = dataFile.DataContent;
            }
            DB.SaveChanges();

            sr.ReturnId = dataFile.DataFileId;
            return sr;
        }

        public static void Remove(int dataFileId)
        {
            var dataFile = DB.DataFiles.FirstOrDefault(x => x.DataFileId == dataFileId);
            if (dataFile != null)
            {
                DB.DataContents.Remove(dataFile.DataContent);
                DB.DataFiles.Remove(dataFile);
                DB.SaveChanges();
            }
        }

        #endregion

        #region Validations
        public static ServiceResponse ValidateDataFileSetup(DataFile dataFile, bool isUpdate = false)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            if (String.IsNullOrEmpty(dataFile.FileName))
                serviceResponse.AddError("The [File Name] field cannot be empty.");

            if (String.IsNullOrEmpty(dataFile.Extension))
                serviceResponse.AddError("The [Extension] field cannot be empty.");

            if (String.IsNullOrEmpty(dataFile.SourceCode))
                serviceResponse.AddError("The [Source Code] field cannot be empty.");
            if (!isUpdate)
            {
                if (DB.DataFiles.SingleOrDefault(x => x.DataFileId == dataFile.DataFileId) != null)
                    serviceResponse.AddError("It already exists a file with that key");
            }
            return serviceResponse;
        }
        #endregion

        #region Utilities

        public static DataFile CreateDataFile(string filePath)
        {
            var dataContent = new DataContent();
            var dataFile = new DataFile();

            using (var ms = new MemoryStream())
            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
                ms.Write(bytes, 0, (int)file.Length);
                dataContent.RawData = ms.GetBuffer();
            }

            dataFile.FileName = Path.GetFileName(filePath);
            dataFile.Extension = Path.GetExtension(filePath);
            dataFile.DataContent = dataContent;

            return dataFile;
        }

        #endregion

        #region Classes

        public class DataFileSearchParams
        {
            public string FileName { get; set; }
        }

        #endregion

        public static ServiceResponse Delete(int dataFileId)
        {
            var sr = new ServiceResponse();

            var entity = DB.DataFiles.Single(x => x.DataFileId == dataFileId);
            DB.DataFiles.Remove(entity);
            DB.SaveChanges();

            return sr;
        }

        public static DataFile GetFileById(int dataFileId)
        {
            return DB.DataFiles.Single(x => x.DataFileId == dataFileId);
        }

        public static FileStruct LoadDataFile(int dataFileId)
        {
            return DB
                .DataFiles 
                .Where(x => x.DataFileId == dataFileId)
                .Select(x => new FileStruct { FileName = x.FileName, Extension = x.Extension, RawData = x.DataContent.RawData })
                .FirstOrDefault();
        }

    }

    public class FileStruct
    {
        public string FileName { get; set; }
        public string Extension { get; set; }
        public byte[] RawData { get; set; }

    }
}
