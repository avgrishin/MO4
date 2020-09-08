using MO.Areas.Code.Models;
using MO.Helpers;
using MO.Hubs;
using MO.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MO.Areas.Code.Controllers
{
  public class JurController : Controller
  {
    public IJurRepository jurRepository;
    protected readonly Lazy<Microsoft.AspNet.SignalR.IHubContext> AHub = new Lazy<Microsoft.AspNet.SignalR.IHubContext>(() => Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<AHub>());

    public JurController(IJurRepository _jurRepository)
    {
      jurRepository = _jurRepository;
    }

    [Authorize(Roles = "jur, jurv")]
    public ActionResult Index()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);
      return View();
    }

    [Authorize(Roles = "jur, jurv")]
    public ActionResult getWarrantList(string sort, string dir, DateTime? d1, DateTime? d2, int? type, bool? all)
    {
      return new JsonnResult { Data = new { success = true, data = jurRepository.getWarrantList(sort, dir, d1, d2, type, all) } };
    }

    [Authorize(Roles = "jur")]
    public ActionResult addWarrant(List<tWarrant> data)
    {
      return new JsonnResult { Data = new { success = true, data = jurRepository.addWarrant(data, User.Identity.Name) } };
    }

    [Authorize(Roles = "jur")]
    public ActionResult updWarrant(List<tWarrant> data)
    {
      return new JsonnResult { Data = new { success = true, data = jurRepository.updWarrant(data, User.Identity.Name) } };
    }

    [Authorize(Roles = "jur")]
    public ActionResult delWarrant(List<tWarrant> data)
    {
      return new JsonnResult { Data = new { success = true, data = jurRepository.delWarrant(data) } };
    }

    [Authorize(Roles = "jur")]
    public ActionResult warrantCourriel()
    {
      return new JsonnResult { Data = new { success = jurRepository.warrantCourriel((HttpContext.Request).Url.Authority) } };
    }

    [Authorize(Roles = "jur")]
    public ActionResult confirmCloseWarrant(int? ID)
    {
      var q = jurRepository.getWarrant(ID);
      return View(q);
    }

    [Authorize(Roles = "jur")]
    [HttpPost]
    public ActionResult closeWarrant(int? ID)
    {
      var q = jurRepository.closeWarrant(ID, User.Identity.Name);
      if (q)
      {
        return View();
      }
      return View("errorCloseWarrant");
    }
  }
}
