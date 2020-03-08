using System.IO;
using System.Web.Mvc;

namespace OnlineMasterG.Base
{
	public class ViewRendering
	{
		public static string RenderViewToString(string viewName, object model, ControllerContext controllerContext)
		{
			// Controller context is NULL when executed from the NUnit tool
			if (controllerContext == null)
				return "";

			// Build view and return string
			controllerContext.Controller.ViewData.Model = model;

			using (var sw = new StringWriter())
			{
				var viewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
				var viewContext = new ViewContext(controllerContext, viewResult.View, controllerContext.Controller.ViewData, controllerContext.Controller.TempData, sw);

				viewResult.View.Render(viewContext, sw);
				viewResult.ViewEngine.ReleaseView(controllerContext, viewResult.View);

				return sw.GetStringBuilder().ToString();
			}
		}
	}
}