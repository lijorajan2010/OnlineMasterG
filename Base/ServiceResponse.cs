using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.Base
{
	public class ServiceResponse
	{
		#region Constructors
		public ServiceResponse()
		{
			Errors = new List<ServiceError>();
			Warnings = new List<string>();
			ReturnId = -1;
			ReturnCode = "";
			ReturnName = "";
		}

		public ServiceResponse(params ServiceResponse[] srs)
		{
			Errors = new List<ServiceError>();
			Warnings = new List<string>();
			ReturnId = -1;
			ReturnCode = "";
			ReturnName = "";

			foreach (var sr in srs)
			{
				Errors.AddRange(sr.Errors);
				Warnings.AddRange(sr.Warnings);
			}
		}

		#endregion

		#region Properties
		public int ReturnId { get; set; }
		public string ReturnCode { get; set; }
		public string ReturnName { get; set; }
		public object Data { get; set; }
		public string DataColumns { get; set; }
		public List<ServiceError> Errors { get; set; }
		public List<string> Warnings { get; set; }

		public string ErrorsString
		{
			get { return String.Join(";", Errors.Select(e => e.ErrorMessage)); }
		}

		public string WarningsString
		{
			get { return String.Join(";", Warnings); }
		}

		public bool Status
		{
			get { return !Errors.Any(x => x.ErrorLevel == ServiceErrorLevel.VALIDATION_ERROR || x.ErrorLevel == ServiceErrorLevel.EXCEPTION); }
		}

		#endregion

		#region Methods
		public void AddError(Exception ex)
		{
			Errors.Add(new ServiceError(ex));
		}

		public void AddError(string errorMessage)
		{
			Errors.Add(new ServiceError(errorMessage));
		}

		public void AddError(string errorCode, string errorMessage)
		{
			Errors.Add(new ServiceError(errorCode, errorMessage));
		}

		public void AddError(string errorCode, string errorMessage, ServiceErrorLevel errorLevel)
		{
			Errors.Add(new ServiceError(errorCode, errorMessage, errorLevel));
		}

		public void AddErrors(List<ServiceError> serviceErrorList)
		{
			foreach (ServiceError e in serviceErrorList)
				Errors.Add(e);
		}

		public override string ToString()
		{
			string rsp = "";

			rsp += "Status: " + Status;
			rsp += "\nReturn Id: " + ReturnId;
			rsp += "\nReturn Code: " + ReturnCode;
			rsp += "\nReturn Name: " + ReturnName;
			rsp += "\nErrors:\n";

			foreach (ServiceError error in Errors)
				rsp += "  >" + error.ErrorMessage + " / " + error.ErrorDetail;

			rsp += "\n";

			return rsp;
		}
		#endregion


		#region Enums

		public enum StatusCodes
		{
			PurchaseSuccessful,
			PurchaseFailed,
			ServiceValidationFailed
		}

		#endregion

	}
}