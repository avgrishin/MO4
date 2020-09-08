using System;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace MO.Helpers
{
  public class JsonnResult : JsonResult
  {
    public override void ExecuteResult(ControllerContext context)
    {
      if (context == null)
      {
        throw new ArgumentNullException("context");
      }
      //if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
      //    String.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
      //{
      //  throw new InvalidOperationException(MvcResources.JsonRequest_GetNotAllowed);
      //}

      HttpResponseBase response = context.HttpContext.Response;

      if (!String.IsNullOrEmpty(ContentType))
      {
        response.ContentType = ContentType;
      }
      else
      {
        response.ContentType = "application/json";
      }
      if (ContentEncoding != null)
      {
        response.ContentEncoding = ContentEncoding;
      }
      if (Data != null)
      {
        response.Write(JsonConvert.SerializeObject(Data, Formatting.None, new JsonSerializerSettings()
        {
          ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        }));
      }
    }

  }
}