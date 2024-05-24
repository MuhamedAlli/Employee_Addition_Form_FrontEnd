using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace Employee_Addition_Form_FrontEnd.Filters
{
	public class AjaxOnlyAttribute : ActionMethodSelectorAttribute
	{
		public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
		{
			var request = routeContext.HttpContext.Request;
			var isAjax = request.Headers["X-Requested-With"] == "XMLHttpRequest";
			return isAjax;
		}
	}
}
