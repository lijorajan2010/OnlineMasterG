using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OnlineMasterG.Models.Shared;

namespace OnlineMasterG.Base
{
	public class JSonResponse
	{
		#region Members
		public int ReturnId;
		public string ReturnCode;
		public string ReturnName;
		public bool Status = true;
		public string ValidationHTML;
		public string ViewHTML;
		public string ValidationTitle;
		public string Redirect;
		public object Data;
		public List<string> MessageList = new List<string>();
		#endregion

		#region Constructors
		public JSonResponse()
		{
		}

		public JSonResponse(ModelStateDictionary modelState)
		{
			foreach (var error in modelState.Values.Where(item => item.Errors.Count > 0).SelectMany(item => item.Errors).ToList())
				MessageList.Add(error.ErrorMessage);

			ValidationTitle = "Error Summary";
			Status = false;
		}

		public JSonResponse(ServiceResponse sr, string successMessage)
		{
			ReturnCode = sr.ReturnCode;
			ReturnId = sr.ReturnId;
			ReturnName = sr.ReturnName;
			Status = sr.Status;
			Data = sr.Data;

			if (sr.Status)
			{
				ValidationTitle = successMessage;
				if (sr.Warnings.Any())
				{
					ValidationTitle += string.Format("<div style=\"color:red\">{0}<div>", sr.WarningsString.Replace(";", "<br>"));
				}
			}
			else
			{
				ValidationTitle = "Error Summary";

				var limit = 50;
				foreach (ServiceError serviceError in sr.Errors.Take(limit))
					MessageList.Add(serviceError.ErrorMessage);

				if (sr.Errors.Count > limit)
					MessageList.Add($"{sr.Errors.Count - limit} more errors...");
			}
		}

		public JSonResponse(string successMessage)
		{
			ValidationTitle = successMessage;
			Status = true;
		}
		#endregion

		#region Methods
		public void RenderValidationHTML(ControllerContext controllerContext)
		{
			if (Status)
			{
				// Render ValidationSuccess view
				ValidationSuccessModel validationSuccess = new ValidationSuccessModel();
				validationSuccess.Title = ValidationTitle;

				ValidationHTML = ViewRendering.RenderViewToString("ValidationSuccess", validationSuccess, controllerContext);
			}
			else
			{
				// Render ValidationError view
				ValidationErrorModel validationError = new ValidationErrorModel();
				validationError.Title = ValidationTitle;

				foreach (string s in MessageList)
					validationError.Errors.Add(s);

				ValidationHTML = ViewRendering.RenderViewToString("ValidationError", validationError, controllerContext);
			}
		}
		#endregion
	}
}