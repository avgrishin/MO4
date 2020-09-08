using System.Web.Mvc;

namespace MO.Areas.Code
{
  public class CodeAreaRegistration : AreaRegistration
  {
    public override string AreaName
    {
      get
      {
        return "Code";
      }
    }

    public override void RegisterArea(AreaRegistrationContext context)
    {
      context.MapRoute(
          "Code_default",
          "Code/{controller}/{action}/{id}",
          new { action = "Index", id = UrlParameter.Optional }
      );
    }
  }
}
