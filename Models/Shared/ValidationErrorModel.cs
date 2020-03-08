using OnlineMasterG.Base;
using System.Collections.Generic;
using System.Web.Mvc;

namespace OnlineMasterG.Models.Shared
{
	public class ValidationErrorModel
	{
		public List<string> Errors = new List<string>();
		public string Title;

		public ValidationErrorModel()
		{
		}

		public ValidationErrorModel(string errorMessage)
		{
			Errors.Add(errorMessage);
			Title = "Error Summary";
		}

		public ValidationErrorModel(ModelStateDictionary m)
		{
			if (!m.IsValid)
			{
				Title = "Error Summary";

				foreach (ModelState modelState in m.Values)
				{
					foreach (ModelError error in modelState.Errors)
						Errors.Add(error.ErrorMessage);
				}
			}
		}

		public ValidationErrorModel(ServiceResponse sr)
		{
			if (!sr.Status)
			{
				Title = "Error Summary";

				foreach (ServiceError se in sr.Errors)
					Errors.Add(se.ErrorMessage);
			}
		}
	}
}