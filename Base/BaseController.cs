using OnlineMasterG.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OnlineMasterG.Base
{
    public enum SetupMode
    {
        NEW,
        EDIT
    }

    [HandleError]
	public class BaseController : Controller
	{
		#region Constructors
		protected BaseController()
		{
		}
		#endregion

		#region JSon
		protected JsonResult GetJsonRedirect(string redirectUrl)
		{
			JSonResponse jSonResponse = new JSonResponse();
			jSonResponse.Redirect = redirectUrl;
			return base.Json(jSonResponse);
		}

		protected JsonResult GetJsonResult(object data)
		{
			JsonResult result = Json(data);
			result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
			return result;
		}

		protected JsonResult GetJsonGridError(string errorMessage)
		{
			return GetJsonGridError(new ValidationErrorModel(errorMessage));
		}

		protected JsonResult GetJsonGridError(ModelStateDictionary modelState)
		{
			return GetJsonGridError(new ValidationErrorModel(modelState));
		}

		protected JsonResult GetJsonGridError(ServiceResponse sr)
		{
			return GetJsonGridError(new ValidationErrorModel(sr));
		}

		private JsonResult GetJsonGridError(ValidationErrorModel model)
		{
			var gridResponseError = new
			{
				userdata = new { ValidationHTML = ViewRendering.RenderViewToString("ValidationError", model, ControllerContext) }
			};

			JsonResult result = Json(gridResponseError);
			result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
			return result;
		}


		protected JsonResult GetJsonValidation(ModelStateDictionary modelState)
		{
			JSonResponse jSonResponse = new JSonResponse(modelState);
			jSonResponse.RenderValidationHTML(ControllerContext);

			return base.Json(jSonResponse);
		}

		protected JsonResult GetJsonValidation(ServiceResponse sr)
		{
			JSonResponse jSonResponse = new JSonResponse(sr, "");
			jSonResponse.RenderValidationHTML(ControllerContext);

			return base.Json(jSonResponse);
		}

		protected JsonResult GetMaxJsonData(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
		{
			return new JsonResult()
			{
				Data = data,
				ContentType = contentType,
				ContentEncoding = contentEncoding,
				JsonRequestBehavior = behavior,
				MaxJsonLength = Int32.MaxValue
			};
		}

		protected JsonResult GetJsonValidation(ServiceResponse sr, string successMessage)
		{
			JSonResponse jSonResponse = new JSonResponse(sr, successMessage);
			jSonResponse.RenderValidationHTML(ControllerContext);

			return base.Json(jSonResponse);
		}

		protected JsonResult GetJsonValidation(string successMessage)
		{
			JSonResponse jSonResponse = new JSonResponse(successMessage);
			jSonResponse.RenderValidationHTML(ControllerContext);

			return base.Json(jSonResponse);
		}

		protected JsonResult GetMaxJsonResult(object data)
		{
			JsonResult result = Json(data);
			result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
			result.MaxJsonLength = Int32.MaxValue;
			return result;
		}


		#endregion
	}
}