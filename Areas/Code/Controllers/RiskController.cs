using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MO.Areas.Code.Models;
using MO.Helpers;
using MO.Hubs;
using MO.Models;

namespace MO.Areas.Code.Controllers
{
  [Authorize(Roles = "risk")]
  public class RiskController : Controller
  {
    public IRiskRepository riskRepository;
    protected readonly Lazy<Microsoft.AspNet.SignalR.IHubContext> AHub = new Lazy<Microsoft.AspNet.SignalR.IHubContext>(() => Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<AHub>());

    public RiskController(IRiskRepository _riskRepository)
    {
      riskRepository = _riskRepository;
    }

    public ActionResult Index()
    {
      return View();
    }

    public ActionResult getRiskEventsList(int? RiskType, string sort, string dir)
    {
      return new JsonnResult { Data = new { success = true, data = riskRepository.getRiskEventsList(RiskType, sort, dir) } };
    }

    public ActionResult addRiskEvent(List<tRiskEvent> data)
    {
      return new JsonnResult { Data = new { success = true, data = riskRepository.addRiskEvent(data, User.Identity.Name) } };
    }

    public ActionResult updRiskEvent(List<tRiskEvent> data)
    {
      return new JsonnResult { Data = new { success = true, data = riskRepository.updRiskEvent(data, User.Identity.Name) } };
    }

    public ActionResult delRiskEvent(List<tRiskEvent> data)
    {
      return new JsonnResult { Data = new { success = riskRepository.delRiskEvent(data) } };
    }

    public ActionResult GetObjClsByParent(int id)
    {
      return new JsonnResult { Data = new { success = true, data = riskRepository.GetObjClsByParent(id) } };
    }

    public ActionResult GetObjClsNodes(int node)
    {
      return Json(riskRepository.GetObjClsNode(node), JsonRequestBehavior.AllowGet);
    }

    public ActionResult FURisk(HttpPostedFileBase FileName)
    {
      if (FileName != null && FileName.ContentLength > 0)
      {
        var prefix = @"\\fc.uralsibbank.ru\uralsib\MSK\COMMON\VOL1\ASSETS\CorpEvents\Risk";
        var dir = Path.Combine(prefix, DateTime.Today.ToString("yy"));
        if (!Directory.Exists(dir))
          Directory.CreateDirectory(dir);
        var file = Path.Combine(DateTime.Today.ToString("yy"), string.Format("{0:MMdd}_{1}", DateTime.Today, Path.GetFileName(FileName.FileName)));
        var path = Path.Combine(prefix, file);
        if (System.IO.File.Exists(path))
          System.IO.File.Delete(path);
        FileName.SaveAs(path);
        return new JsonnResult { Data = new { success = true, message = "Сохранено", file = file }, ContentType = "text/html" };
      }
      return new JsonnResult { Data = new { success = false, message = "Нет файла" }, ContentType = "text/html" };
    }

    public ActionResult GetFileRisk(string data)
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
      var prefix = @"\\fc.uralsibbank.ru\uralsib\MSK\COMMON\VOL1\ASSETS\CorpEvents\Risk";
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

    public ActionResult Var()
    {
      return View();
    }

    public ActionResult FUVar(HttpPostedFileBase FileName)
    {
      if (FileName != null && FileName.ContentLength > 0)
      {
        var dir = @"\\am-uralsib.ru\uralsib\MSK\COMMON\VOL1\ASSETS\CorpEvents\Var";
        if (!Directory.Exists(dir))
          Directory.CreateDirectory(dir);
        var file = Path.Combine(dir, "var.csv");
        if (System.IO.File.Exists(file))
          System.IO.File.Delete(file);
        FileName.SaveAs(file);
        return new JsonnResult { Data = new { success = true, message = "Сохранено", file = FileName.FileName }, ContentType = "text/html" };
      }
      return new JsonnResult { Data = new { success = false, message = "Нет файла" }, ContentType = "text/html" };
    }

    [HttpPost]
    public ActionResult LoadVar()
    {
      return Json(new { success = riskRepository.LoadVar() });
    }

  }
}
