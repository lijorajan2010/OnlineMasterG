using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.Base
{
	public enum ServiceErrorLevel
	{
		VALIDATION_ERROR,
		VALIDATION_WARNING,
		EXCEPTION
	}

	public class ServiceError
	{
		public ServiceError()
		{
		}

		public ServiceError(Exception ex)
		{
			Exception finalException = ex;

			if (ex.InnerException != null)
				finalException = ex.InnerException;

			ErrorMessage = finalException.Message;
			ErrorDetail = finalException.ToString();
			ErrorLevel = ServiceErrorLevel.EXCEPTION;
			ErrorCode = "";
		}

		public ServiceError(string errorMessage)
		{
			this.ErrorMessage = errorMessage;
			this.ErrorLevel = ServiceErrorLevel.VALIDATION_ERROR;
			this.ErrorCode = "N/A";
		}

		public ServiceError(string errorCode, string errorMessage)
		{
			this.ErrorMessage = errorMessage;
			this.ErrorLevel = ServiceErrorLevel.VALIDATION_ERROR;
			this.ErrorCode = errorCode;
		}

		public ServiceError(string errorCode, string errorMessage, ServiceErrorLevel errorLevel)
		{
			this.ErrorMessage = errorMessage;
			this.ErrorLevel = errorLevel;
			this.ErrorCode = errorCode;
		}

		public string ErrorCode { get; set; }
		public string ErrorMessage { get; set; }
		public string ErrorDetail { get; set; }
		public ServiceErrorLevel ErrorLevel { get; set; }
	}
}