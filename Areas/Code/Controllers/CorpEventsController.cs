using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using MO.Areas.Code.Models;
using MO.Helpers;
using MO.Hubs;
using MO.Models;

namespace MO.Areas.Code.Controllers
{

  public class CorpEventsController : Controller
  {
    public ICorpEventsRepository corpEventsRepository;
    protected readonly Lazy<Microsoft.AspNet.SignalR.IHubContext> AHub = new Lazy<Microsoft.AspNet.SignalR.IHubContext>(() => Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<AHub>());

    public CorpEventsController(ICorpEventsRepository _corpEventsRepository)
    {
      corpEventsRepository = _corpEventsRepository;
    }

    public ActionResult Index()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);
      CorpEventsViewModel ceVM = new CorpEventsViewModel
      {
        fields = corpEventsRepository.getCorpEventsFields(),
        columns = corpEventsRepository.getCorpEventsColumns()
      };
      return View("Index", ceVM);
    }

    public ActionResult GetEmitents(DateTime? d1, DateTime? d2, string n)
    {
      var q = corpEventsRepository.up_avgGetEmitents(d1, d2, n);
      return Json(new { data = q });
    }

    public ActionResult GetEmitSecurity(DateTime? d, int? issuerID)
    {
      var q = corpEventsRepository.up_avgGetEmitSecurity(d, issuerID);
      return Json(new { data = q });
    }

    public ActionResult GetRestSecurity(DateTime? d, int? securityID, string sort, string dir)
    {
      var q = corpEventsRepository.up_avgGetRestSecurity(d, securityID, sort, dir);
      return Json(new { data = q });
    }

    public ActionResult GetCorpEventsList()
    {
      return Json(new { data = corpEventsRepository.getCorpEventsList() });
    }

    public ActionResult GetCorpEventsDateList(int id)
    {
      return Json(new { data = corpEventsRepository.getCorpEventsDateList(id) });
    }

    public ActionResult GetCorpEmitentEvents(int? FinInstID, int? SecurityID, int? eventID, int? dateID, DateTime? d1, DateTime? d2, Boolean? all, Boolean? isuk, string n, int? start, int? limit)
    {
      int? c = 0;
      var q = corpEventsRepository.getCorpEmitentEvents(FinInstID, SecurityID, eventID, dateID, d1, d2, all, isuk, n, start, limit, ref c);
      return new JsonnResult { Data = new { data = q, totalCount = c } };
    }

    public ActionResult GetCorpEventHtml(string id)
    {
      return Content(corpEventsRepository.getEventHtml(id));
    }

    public ActionResult GetEmitClients(DateTime? d, string n)
    {
      var q = corpEventsRepository.up_avgGetEmitClients(d, n);

      return Json(new { data = q });
    }

    public ActionResult GetEmitClientRests(DateTime? d, int? f)
    {
      return Json(new { data = corpEventsRepository.up_avgGetEmitClientRests(d, f) });
    }

    public ActionResult GetEventSecurity(int? EventID, int? DateID)
    {
      var q = corpEventsRepository.up_avgGetEventSecurity(EventID, DateID);
      return new JsonnResult { Data = new { data = q } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult CorpEmitentEventsUpdate2(List<up_avgGetEmitentEvents2> data)
    {
      var q = corpEventsRepository.CorpEmitentEventsUpdate2(data);
      return new JsonnResult { Data = new { success = true, message = "Сохранено", data = q } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult CorpEmitentEventsUpdate3(List<up_avgGetEmitentEvents3> data)
    {
      var q = corpEventsRepository.CorpEmitentEventsUpdate3(data);
      return new JsonnResult { Data = new { success = true, message = "Сохранено", data = q } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult CorpEmitentEventsUpdate4(List<up_avgGetEmitentEvents4> data)
    {
      var q = corpEventsRepository.CorpEmitentEventsUpdate4(data);
      return new JsonnResult { Data = new { success = true, message = "Сохранено", data = q } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult CorpEmitentEventsUpdate5(List<up_avgGetEmitentEvents5> data)
    {
      var q = corpEventsRepository.CorpEmitentEventsUpdate5(data);
      return new JsonnResult { Data = new { success = true, message = "Сохранено", data = q } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult CorpEmitentEventsUpdate95(List<up_avgGetEmitentEvents95> data)
    {
      var q = corpEventsRepository.CorpEmitentEventsUpdate95(data);
      return new JsonnResult { Data = new { success = true, message = "Сохранено", data = q } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult FileUpload(int? ID, HttpPostedFileBase FileName_)
    {
      if (FileName_ != null && FileName_.ContentLength > 0)
      {
        var prefix = @"\\am-uralsib.ru\uralsib\MSK\COMMON\VOL1\ASSETS\CorpEvents\";
        var dir = Path.Combine(prefix, DateTime.Today.ToString("yyMM"));
        if (!Directory.Exists(dir))
          Directory.CreateDirectory(dir);
        var file = Path.Combine(DateTime.Today.ToString("yyMM"), string.Format("{0}_{1}", ID, Path.GetFileName(FileName_.FileName)));
        var path = Path.Combine(prefix, file);
        if (System.IO.File.Exists(path))
          System.IO.File.Delete(path);
        FileName_.SaveAs(path);
        return new JsonnResult { Data = new { success = true, message = "Сохранено", file = file }, ContentType = "text/html" };
      }
      return new JsonnResult { Data = new { success = false, message = "Нет файла" }, ContentType = "text/html" };
    }

    public ActionResult FileUpload1(int? ID, string Comment, string FileName)
    {
      return Json(new { success = true, message = "Сохранено", file = FileName });
    }

    [Authorize(Roles = "mo")]
    public ActionResult GetFile(string data)
    {
      string mimeType = "application/octet-stream";
      string ext = Path.GetExtension(data).ToLower();
      if (ext == ".zip")
        mimeType = "application/x-zip-compressed";
      else if (ext == ".rar")
        mimeType = "application/x-rar-compressed";
      else
      {
        Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
        if (regKey != null && regKey.GetValue("Content Type") != null)
        {
          mimeType = regKey.GetValue("Content Type").ToString();
        }
      }
      var prefix = @"\\am-uralsib.ru\uralsib\MSK\COMMON\VOL1\ASSETS\CorpEvents\";
      var path = Path.Combine(prefix, data);
      if (!System.IO.File.Exists(path))
        return new HttpNotFoundResult("File not found");
      using (FileStream fs = new FileStream(path, FileMode.Open))
      {
        BinaryReader reader = new BinaryReader(fs);
        Byte[] bytes = reader.ReadBytes(Convert.ToInt32(fs.Length));

        return File(bytes, mimeType, Path.GetFileName(data));
      }
    }

    public ActionResult GetDividends(DateTime? d1, DateTime? d2)
    {
      return new JsonnResult { Data = new { success = true, data = corpEventsRepository.up_avgGetDividends(d1, d2) } };
    }

    public ActionResult GetDividends3(DateTime? d1, DateTime? d2, string n, string sort, string dir)
    {
      return new JsonnResult { Data = new { success = true, data = corpEventsRepository.GetDividends3(d1, d2, n, sort, dir) } };
    }

    public ActionResult GetDividends3_2(int? IssuerID, decimal? Category, DateTime? d, decimal? Stavka, string sort, string dir)
    {
      return new JsonnResult { Data = new { success = true, data = corpEventsRepository.GetDividends3_2(IssuerID, Category, d, Stavka, sort, dir) } };
    }
    public ActionResult GetDividends3_3(int? SecurityID, int? TreatyID, DateTime? d, string sort, string dir)
    {
      return new JsonnResult { Data = new { success = true, data = corpEventsRepository.GetDividends3_3(SecurityID, TreatyID, d, sort, dir) } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult ClnEventsAdd(List<ClnEvents> data)
    {
      corpEventsRepository.ClnEventsAdd(data);
      return new JsonnResult { Data = new { success = true, data = data } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult up_avgGetEngagements(DateTime? d, string sort, string dir)
    {
      return new JsonnResult { Data = new { success = true, data = corpEventsRepository.up_avgGetEngagements(d, sort, dir) } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult up_avgGetEngagementDeals(DateTime? d, int? f, int? s, string r)
    {
      return new JsonnResult { Data = new { success = true, data = corpEventsRepository.up_avgGetEngagementDeals(d, f, s, r) } };
    }

    [Authorize(Roles = "mo, jrpc, jrpcv")]
    public ActionResult getEnregList(DateTime? d1, DateTime? d2, Boolean? sd, string sort, string dir)
    {
      return new JsonnResult { Data = new { success = true, data = corpEventsRepository.getEnregList(d1, d2, sd, sort, dir) } };
    }

    [Authorize(Roles = "mo, jrpc")]
    public ActionResult getTreaties(string query, int? start, int? limit)
    {
      return new JsonnResult { Data = new { success = true, data = corpEventsRepository.getTreaties(query ?? "", limit ?? 10) } };
    }

    [Authorize(Roles = "mo, jrpc")]
    public ActionResult addEnreg(List<tEnregistrement> data)
    {
      return new JsonnResult { Data = new { success = true, data = corpEventsRepository.addEnreg(data) } };
    }

    [Authorize(Roles = "mo, jrpc")]
    public ActionResult updEnreg(List<tEnregistrement> data)
    {
      return new JsonnResult { Data = new { success = true, data = corpEventsRepository.updEnreg(data, User.Identity.Name) } };
    }

    [Authorize(Roles = "mo, jrpc")]
    public ActionResult delEnreg(List<tEnregistrement> data)
    {
      try
      {
        return new JsonnResult { Data = new { success = corpEventsRepository.delEnreg(data) } };
      }
      catch (Exception ex)
      {
        return new JsonnResult { Data = new { success = false, message = ex.Message } };
      }
    }

    [Authorize(Roles = "mo, jrpc")]
    public ActionResult getObjClsByParent(int id)
    {
      return new JsonnResult { Data = new { success = true, data = corpEventsRepository.getObjClsByParent(id) } };
    }

    [Authorize(Roles = "mo, jrpc")]
    public ActionResult getStrategy()
    {
      return new JsonnResult { Data = new { success = true, data = corpEventsRepository.getStrategy() } };
    }

    [Authorize(Roles = "mo, jrpc, jrpcv")]
    public ActionResult getEnregDetList(int? id)
    {
      return new JsonnResult { Data = new { success = true, data = corpEventsRepository.getEnregDetList(id) } };
    }

    [Authorize(Roles = "mo, jrpc")]
    public ActionResult addEnregDet(List<tEnregistrementDet> data)
    {
      return new JsonnResult { Data = new { success = true, data = corpEventsRepository.addEnregDet(data) } };
    }

    [Authorize(Roles = "mo, jrpc")]
    public ActionResult updEnregDet(List<tEnregistrementDet> data)
    {
      return new JsonnResult { Data = new { success = true, data = corpEventsRepository.updEnregDet(data, User.Identity.Name) } };
    }

    [Authorize(Roles = "mo, jrpc")]
    public ActionResult delEnregDet(List<tEnregistrementDet> data)
    {
      try
      {
        return new JsonnResult { Data = new { success = corpEventsRepository.delEnregDet(data) } };
      }
      catch (Exception ex)
      {
        return new JsonnResult { Data = new { success = false, message = ex.Message } };
      }
    }

    [Authorize(Roles = "mo, jrpc")]
    public ActionResult FUEnreg(int? ID, HttpPostedFileBase FileName_)
    {
      if (FileName_ != null && FileName_.ContentLength > 0)
      {
        var prefix = @"\\am-uralsib.ru\uralsib\MSK\COMMON\VOL1\ASSETS\CorpEvents\Enreg";
        var dir = Path.Combine(prefix, DateTime.Today.ToString("yy"));
        if (!Directory.Exists(dir))
          Directory.CreateDirectory(dir);
        var file = Path.Combine(DateTime.Today.ToString("yy"), string.Format("{0}_{1}", ID, Path.GetFileName(FileName_.FileName)));
        var path = Path.Combine(prefix, file);
        if (System.IO.File.Exists(path))
          System.IO.File.Delete(path);
        FileName_.SaveAs(path);
        return new JsonnResult { Data = new { success = true, message = "Сохранено", file = file }, ContentType = "text/html" };
      }
      return new JsonnResult { Data = new { success = false, message = "Нет файла" }, ContentType = "text/html" };
    }

    [Authorize(Roles = "mo, jrpc, jrpcv")]
    public ActionResult GetFileEnr(string data)
    {
      string mimeType = "application/octet-stream";
      string ext = Path.GetExtension(data).ToLower();
      if (ext == ".zip")
        mimeType = "application/x-zip-compressed";
      else if (ext == ".rar")
        mimeType = "application/x-rar-compressed";
      else
      {
        Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
        if (regKey != null && regKey.GetValue("Content Type") != null)
        {
          mimeType = regKey.GetValue("Content Type").ToString();
        }
      }
      var prefix = @"\\am-uralsib.ru\uralsib\MSK\COMMON\VOL1\ASSETS\CorpEvents\Enreg";
      var path = Path.Combine(prefix, data);
      if (!System.IO.File.Exists(path))
        return new HttpNotFoundResult("File not found");
      using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
      {
        BinaryReader reader = new BinaryReader(fs);
        Byte[] bytes = reader.ReadBytes(Convert.ToInt32(fs.Length));

        return File(bytes, mimeType, Path.GetFileName(data));
      }
    }

    [Authorize(Roles = "mo, jrpc")]
    public ActionResult getMaxNumEnreg()
    {
      return new JsonnResult { Data = new { success = true, data = corpEventsRepository.getMaxNumEnreg() } };
    }

    [Authorize(Roles = "mo, jrpc")]
    public ActionResult confirmEnrDet(Guid id, numDep t, string a)
    {
      bool isSendMail = false;
      string dep = "";
      if (corpEventsRepository.confirmEnrDet(id, t, User.Identity.Name, ref isSendMail, ref dep))
      {
        if (isSendMail)
          corpEventsRepository.enrdetCourriel(id, (HttpContext.Request).Url.Authority, next: enNext.isNext, descr: "");
        return View(new { dep = dep, NameTo = a, ed = corpEventsRepository.getEnregDet(id) }.ToExpando());
      }
      return View("Error");
    }

    [Authorize(Roles = "mo, jrpc")]
    public ActionResult refusalEnrDet(Guid id, numDep t, string a)
    {
      if (corpEventsRepository.canRefuseEnregDet(id, t))
      {
        return View(new { id = id, t = t, a = a, ed = corpEventsRepository.getEnregDet(id) }.ToExpando());
      }
      return View("cantRefuse", new { id = id, NameTo = a, ed = corpEventsRepository.getEnregDet(id) }.ToExpando());
    }

    [Authorize(Roles = "mo, jrpc")]
    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult refusalEnrDet(Guid id, numDep t, string a, string descr)
    {
      int td = 0;
      if (corpEventsRepository.refusalEnrDet(id, t, User.Identity.Name, descr, ref td))
      {
        corpEventsRepository.enrdetCourriel(id, (HttpContext.Request).Url.Authority, t == numDep.IsKM ? enNext.isSS : enNext.isNext, descr);
        return View("refusalSuccess", new { id = id, NameTo = a, ed = corpEventsRepository.getEnregDet(id), descr = descr }.ToExpando());
      }
      return View("Error");
    }

    [Authorize(Roles = "mo, jrpc")]
    public ActionResult confirmNextEnrDet(Guid id)
    {
      bool isSendMail = false;
      string dep = "";
      var t = corpEventsRepository.findNextEnrDet(id, true);
      if (t != numDep.IsNone)
      {
        if (corpEventsRepository.confirmEnrDet(id, t, User.Identity.Name, ref isSendMail, ref dep))
        {
          if (isSendMail)
            corpEventsRepository.enrdetCourriel(id, (HttpContext.Request).Url.Authority, enNext.isNext, "");
        }
        return new JsonnResult { Data = new { success = true } };
      }
      throw new Exception("Error: нет неподтвержденных действий!");
    }

    [Authorize(Roles = "mo, jrpc")]
    public ActionResult enrdetCourriel(Guid id)
    {
      var success = corpEventsRepository.enrdetCourriel(id, (HttpContext.Request).Url.Authority, enNext.isAll, "");
      if (success)
        corpEventsRepository.enrdetCourriel(id, (HttpContext.Request).Url.Authority, enNext.isNext, "");
      return new JsonnResult { Data = new { success = success } };
    }

    [Authorize(Roles = "mo, jrpc")]
    public ActionResult enrdetRappel()
    {
      var success = corpEventsRepository.enrdetRappel((HttpContext.Request).Url.Authority, 0);
      return new JsonnResult { Data = new { success = success } };
    }

    [Authorize(Roles = "mo, jrpc")]
    public ActionResult enrdetRappelKM()
    {
      var success = corpEventsRepository.enrdetRappel((HttpContext.Request).Url.Authority, 1);
      return new JsonnResult { Data = new { success = success } };
    }

    [Authorize(Roles = "mo, jrpc")]
    public ActionResult enrdetRappelUA()
    {
      var success = corpEventsRepository.enrdetRappel((HttpContext.Request).Url.Authority, 2);
      return new JsonnResult { Data = new { success = success } };
    }

    [Authorize(Roles = "mo, jrpc, jrpcv")]
    public ActionResult getEnregDetLogList(int id)
    {
      return new JsonnResult { Data = new { success = true, data = corpEventsRepository.getEnregDetLogList(id) } };
    }

    [Authorize(Roles = "mo, jrpc")]
    public ActionResult getDaysDog(int id, int? dtid)
    {
      return new JsonnResult { Data = new { success = true, data = corpEventsRepository.getDaysDog(id, dtid) } };
    }

    [Authorize(Roles = "jrpc")]
    public ActionResult getEnreg(int id, numDep? t, string a)
    {
      var q = corpEventsRepository.getEnreg(id);
      ViewBag.t = t;
      ViewBag.a = a;
      return View(q);
    }

    [Authorize(Roles = "jrpc")]
    public ActionResult toggleStatus(int id, string status)
    {
      return new JsonnResult { Data = new { success = true, data = corpEventsRepository.toggleStatus(id, status, User.Identity.Name) } };
    }

    [Authorize(Roles = "jrpc")]
    public ActionResult getCancelDate(int TreatyID, DateTime recuDate)
    {
      var cd = corpEventsRepository.getCancelDate(TreatyID, recuDate);
      if (cd.HasValue)
        return new JsonnResult { Data = new { success = true, data = new { cancelDate = cd.Value, outDate = corpEventsRepository.getOutDate(TreatyID, cd.Value) } } };
      return new JsonnResult { Data = new { success = false } };
    }

    public static string RenderViewToString(ControllerContext context, string viewPath, object model = null, bool partial = false)
    {
      ViewEngineResult viewEngineResult = null;
      if (partial)
        viewEngineResult = ViewEngines.Engines.FindPartialView(context, viewPath);
      else
        viewEngineResult = ViewEngines.Engines.FindView(context, viewPath, null);

      if (viewEngineResult == null)
        throw new FileNotFoundException("View cannot be found.");

      var view = viewEngineResult.View;
      context.Controller.ViewData.Model = model;
      string result = null;
      using (var sw = new StringWriter())
      {
        var ctx = new ViewContext(context, view, context.Controller.ViewData, context.Controller.TempData, sw);
        view.Render(ctx, sw);
        result = sw.ToString();
      }
      return result;
    }

  }
}
