using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MO.Areas.Code.Models;
using MO.Helpers;
using MO.Hubs;
using MO.Models;
using MvcContrib.Sorting;
using System.Web;
using System.IO;

namespace MO.Areas.Code.Controllers
{
  [Authorize(Roles = "admin,mo")]
  public class DivController : Controller
  {
    public IDivRepository divRepository;
    protected readonly Lazy<Microsoft.AspNet.SignalR.IHubContext> AHub = new Lazy<Microsoft.AspNet.SignalR.IHubContext>(() => Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<AHub>());

    public DivController(IDivRepository _divRepository)
    {
      divRepository = _divRepository;
    }

    public ActionResult Index()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);
      return View();
    }

    public ActionResult getDiv(string sort, string dir, DateTime? d1, DateTime? d2, string n)
    {
      return new JsonnResult { Data = new { success = true, data = divRepository.getDivList(sort ?? "RDate", dir ?? "ASC", d1, d2, n) } };
    }

    public ActionResult getDiv2(string sort, string dir, int id)
    {
      return new JsonnResult { Data = new { success = true, data = divRepository.getDiv2List(sort ?? "SecName", dir ?? "ASC", id) } };
    }

    public ActionResult getDiv3(string sort, string dir, int? id, int? TreatyID)
    {
      return new JsonnResult { Data = new { success = true, data = divRepository.getDiv3List(sort, dir, id, TreatyID) } };
    }

    public ActionResult getDiv4(string sort, string dir, DateTime? d1, DateTime? d2, string n)
    {
      return new JsonnResult { Data = new { success = true, data = divRepository.getDiv4List(sort, dir, d1, d2, n) } };
    }

    public ActionResult getDiv5(string sort, string dir, decimal? id)
    {
      return new JsonnResult { Data = new { success = true, data = divRepository.getDiv5List(sort, dir, id) } };
    }

    public ActionResult addDiv(List<tDiv> data)
    {
      return new JsonnResult { Data = new { success = true, data = divRepository.addDiv(data) } };
    }

    public ActionResult updDiv(List<tDiv> data)
    {
      return new JsonnResult { Data = new { success = true, data = divRepository.updDiv(data) } };
    }

    public ActionResult delDiv(List<tDiv> data)
    {
      return new JsonnResult { Data = new { success = true, data = divRepository.delDiv(data) } };
    }

    public ActionResult addDivRel(List<tDivRel> data)
    {
      return new JsonnResult { Data = new { success = true, data = divRepository.addDivRel(data) } };
    }

    public ActionResult GetSec(int? id, string query, int? start, int? limit)
    {
      if (id.HasValue)
        return Json(new { data = divRepository.getSecurity(id.Value) }, JsonRequestBehavior.AllowGet);
      else
        return Json(new { data = divRepository.getSecurities(query ?? "", limit ?? 10) }, JsonRequestBehavior.AllowGet);
    }

    public ActionResult GetFunds()
    {
      return Json(new { data = divRepository.GetFunds() }, JsonRequestBehavior.AllowGet);
    }

    public ActionResult getDiv7(string sort, string dir, DateTime? d1, DateTime? d2, string n)
    {
      return new JsonnResult { Data = new { success = true, data = divRepository.getDiv7List(sort, dir, d1, d2, n) } };
    }

    public ActionResult getDiv8(string sort, string dir, DateTime? d1, DateTime? d2, int? tid, string n)
    {
      return new JsonnResult { Data = new { success = true, data = divRepository.getDiv8List(sort, dir, d1, d2, tid, n) } };
    }

    public ActionResult getDiv9(string sort, string dir, int? id, int? TreatyID)
    {
      return new JsonnResult { Data = new { success = true, data = divRepository.getDiv3List(sort, dir, id, TreatyID) } };
    }

  }
}
