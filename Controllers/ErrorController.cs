using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Web.UI;

namespace MO.Controllers
{
  public class ErrorController : Controller
  {

    private HttpStatusCode _httpStatusCode;
    private string _statusCodeDescription;

    // 404
    public ActionResult NotFound()
    {
      SetResponse(HttpStatusCode.NotFound);
      return View();
    }
    // 403
    public ActionResult Forbidden()
    {
      SetResponse(HttpStatusCode.Forbidden);
      return View();
    }
    // 500
    public ActionResult InternalServerError()
    {
      SetResponse(HttpStatusCode.InternalServerError);
      return View();
    }

    private void SetResponse(HttpStatusCode httpStatusCode)
    {
      _httpStatusCode = httpStatusCode;
      _statusCodeDescription = HttpWorkerRequest.GetStatusDescription((int)_httpStatusCode);
      Response.StatusCode = (int)_httpStatusCode;
      Response.StatusDescription = _statusCodeDescription;
    }

    protected override void OnActionExecuted(ActionExecutedContext context)
    {
    }

  }
}
