using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MO.Helpers
{
  public class JsonpResult : JsonResult
  {
    public string Callback { get; set; }   
    public override void ExecuteResult(ControllerContext context)    
    {  
      
      if (context == null)            
        throw new ArgumentNullException("context");
      //if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
      //    String.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
      //{
      //  throw new InvalidOperationException("MvcResources.JsonRequest_GetNotAllowed");
      //}
      HttpResponseBase response = context.HttpContext.Response;        
      if (!String.IsNullOrEmpty(ContentType))            
        response.ContentType = ContentType;        
      else            
        response.ContentType = "application/javascript";        
      if (ContentEncoding != null)            
        response.ContentEncoding = ContentEncoding;        
      if (Callback == null || Callback.Length == 0)            
        Callback = context.HttpContext.Request.QueryString["callback"];        
      if (Data != null)        
      {            
        JavaScriptSerializer serializer = new JavaScriptSerializer();            
        string ser = serializer.Serialize(Data);            
        response.Write(Callback + "(" + ser + ");");
      }    
    }
  }
}