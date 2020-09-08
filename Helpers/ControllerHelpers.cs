using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using MO.Models;

namespace MO.Helpers
{
  public static class ModelStateHelpers
  {
    public static void AddModelErrors(this ModelStateDictionary modelState, IEnumerable<RuleViolation> errors)
    {
      foreach (RuleViolation issue in errors)
      {
        modelState.AddModelError(issue.PropertyName, issue.ErrorMessage);
      }
    }
  }
  public static class MyHtmlHelpers
  {
    public static MvcHtmlString Chart(this HtmlHelper helper, string actionName)
    {
      return Chart(helper, actionName, "Charts", null, null);
    }

    public static MvcHtmlString Chart(this HtmlHelper helper, string actionName, string controllerName)
    {
      return Chart(helper, actionName, controllerName, null, null);
    }

    public static MvcHtmlString Chart(this HtmlHelper helper, string actionName, string controllerName, object routeValues)
    {
      return Chart(helper, actionName, controllerName, new RouteValueDictionary(routeValues), null);
    }

    public static MvcHtmlString Chart(this HtmlHelper helper, string actionName, string controllerName, RouteValueDictionary routeValues)
    {
      return Chart(helper, actionName, controllerName, routeValues, null);
    }

    public static MvcHtmlString Chart(this HtmlHelper helper, string actionName, string controllerName, object routeValues, object htmlAttributes)
    {
      return Chart(helper, actionName, controllerName, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlAttributes));
    }

    public static MvcHtmlString Chart(this HtmlHelper helper, string actionName, string controllerName, RouteValueDictionary routeValues, object htmlAttributes)
    {
      return Chart(helper, actionName, controllerName, routeValues, new RouteValueDictionary(htmlAttributes));
    }

    public static MvcHtmlString Chart(this HtmlHelper helper, string actionName, string controllerName, object routeValues, IDictionary<string, object> htmlAttributes)
    {
      return Chart(helper, actionName, controllerName, new RouteValueDictionary(routeValues), htmlAttributes);
    }

    public static MvcHtmlString Chart(this HtmlHelper helper, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
    {
      string imgUrl = UrlHelper.GenerateUrl(null, actionName, controllerName, routeValues, helper.RouteCollection, helper.ViewContext.RequestContext, false);

      var builder = new TagBuilder("img");
      builder.MergeAttributes<string, object>(htmlAttributes);
      builder.MergeAttribute("src", imgUrl);

      return MvcHtmlString.Create(builder.ToString());
    }
  }
}
