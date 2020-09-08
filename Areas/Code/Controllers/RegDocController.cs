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
  public class RegDocController : Controller
  {
    public IRegDocRepository regdocRepository;
    protected readonly Lazy<Microsoft.AspNet.SignalR.IHubContext> AHub = new Lazy<Microsoft.AspNet.SignalR.IHubContext>(() => Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<AHub>());

    public RegDocController(IRegDocRepository _regdocRepository)
    {
      regdocRepository = _regdocRepository;
    }

    [Authorize(Roles = "admin,regdoc, regdocv")]
    public ActionResult Index()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);
      return View();
    }

    [Authorize(Roles = "admin,regdoc, regdocv")]
    public ActionResult getRegDocList(string sort, string dir, DateTime? d1, DateTime? d2, int? type, Boolean? sd)
    {
      return new JsonnResult { Data = new { success = true, data = regdocRepository.getRegDocList(sort ?? "Id", dir ?? "DESC", d1, d2, type, sd) } };
    }

    [Authorize(Roles = "admin,regdoc")]
    public ActionResult addRegDoc(List<tRegDoc> data)
    {
      return new JsonnResult { Data = new { success = true, data = regdocRepository.addRegDoc(data, User.Identity.Name) } };
    }

    [Authorize(Roles = "admin,regdoc")]
    public ActionResult updRegDoc(List<tRegDoc> data)
    {
      return new JsonnResult { Data = new { success = true, data = regdocRepository.updRegDoc(data, User.Identity.Name) } };
    }

    [Authorize(Roles = "admin,regdoca")]
    public ActionResult delRegDoc(List<tRegDoc> data)
    {
      return new JsonnResult { Data = new { success = true, data = regdocRepository.delRegDoc(data) } };
    }

    [Authorize(Roles = "admin,regdoc")]
    public ActionResult regdocCourriel(int? id)
    {
      return new JsonnResult { Data = new { success = regdocRepository.regdocCourriel(id, (HttpContext.Request).Url.Authority) } };
    }

    [Authorize(Roles = "admin,regdoc")]
    public ActionResult getEMailList(string sort, string dir)
    {
      return new JsonnResult { Data = new { success = true, data = regdocRepository.getEMailList(sort, dir) } };
    }

    [Authorize(Roles = "admin,regdoc")]
    public ActionResult GetObjClsByParent(int id)
    {
      return new JsonnResult { Data = new { success = true, data = regdocRepository.GetObjClsByParent(id) } };
    }

    [Authorize(Roles = "admin,regdoc")]
    public ActionResult FileUploadI(int? Id, HttpPostedFileBase FileName)
    {
      if (FileName != null && FileName.ContentLength > 0)
      {
        var prefix = @"\\am-uralsib.ru\uralsib\MSK\COMMON\VOL1\ASSETS\CorpEvents\RegDoc\In";
        var dir = Path.Combine(prefix, DateTime.Today.ToString("yy"));
        if (!Directory.Exists(dir))
          Directory.CreateDirectory(dir);
        var file = Path.Combine(DateTime.Today.ToString("yy"), string.Format("{0}_{1}", Id, Path.GetFileName(FileName.FileName)));
        var path = Path.Combine(prefix, file);
        if (System.IO.File.Exists(path))
          System.IO.File.Delete(path);
        FileName.SaveAs(path);
        return new JsonnResult { Data = new { success = true, message = "Сохранено", file = file }, ContentType = "text/html" };
      }
      return new JsonnResult { Data = new { success = false, message = "Нет файла" }, ContentType = "text/html" };
    }

    [Authorize]
    public ActionResult GetFileI(string data)
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
      var prefix = @"\\am-uralsib.ru\uralsib\MSK\COMMON\VOL1\ASSETS\CorpEvents\RegDoc\In";
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

    [Authorize(Roles = "admin,regdoc")]
    public ActionResult FileUploadO(int? Id, HttpPostedFileBase FileName)
    {
      if (FileName != null && FileName.ContentLength > 0)
      {
        var prefix = @"\\am-uralsib.ru\uralsib\MSK\COMMON\VOL1\ASSETS\CorpEvents\RegDoc\Out";
        var dir = Path.Combine(prefix, DateTime.Today.ToString("yy"));
        if (!Directory.Exists(dir))
          Directory.CreateDirectory(dir);
        var file = Path.Combine(DateTime.Today.ToString("yy"), string.Format("{0}_{1}", Id, Path.GetFileName(FileName.FileName)));
        var path = Path.Combine(prefix, file);
        if (System.IO.File.Exists(path))
          System.IO.File.Delete(path);
        FileName.SaveAs(path);
        return new JsonnResult { Data = new { success = true, message = "Сохранено", file = file }, ContentType = "text/html" };
      }
      return new JsonnResult { Data = new { success = false, message = "Нет файла" }, ContentType = "text/html" };
    }

    [Authorize]
    public ActionResult GetFileO(string data)
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
      var prefix = @"\\am-uralsib.ru\uralsib\MSK\COMMON\VOL1\ASSETS\CorpEvents\RegDoc\Out";
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

    [Authorize(Roles = "admin,regdoc")]
    public ActionResult getNextRegNum()
    {
      return Content(regdocRepository.getNextRegNum1());
    }

    [Authorize(Roles = "admin,regdoc, regdocv")]
    public ActionResult Out()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);
      return View();
    }

    [Authorize(Roles = "admin,regdoc, regdocv")]
    public ActionResult getOutgoingList(string sort, string dir, DateTime? d1, DateTime? d2, int? type)
    {
      return new JsonnResult { Data = new { success = true, data = regdocRepository.getOutgoingList(sort ?? "Id", dir ?? "DESC", d1, d2) } };
    }

    [Authorize(Roles = "admin,regdoc")]
    public ActionResult addOutgoing(List<tOutgoing> data)
    {
      return new JsonnResult { Data = new { success = true, data = regdocRepository.addOutgoing(data, User.Identity.Name) } };
    }

    [Authorize(Roles = "admin,regdoc")]
    public ActionResult updOutgoing(List<tOutgoing> data)
    {
      return new JsonnResult { Data = new { success = true, data = regdocRepository.updOutgoing(data, User.Identity.Name) } };
    }

    [Authorize(Roles = "admin,regdoca")]
    public ActionResult delOutgoing(List<tOutgoing> data)
    {
      return new JsonnResult { Data = new { success = true, data = regdocRepository.delOutgoing(data) } };
    }

    [Authorize(Roles = "admin,regdoc")]
    public ActionResult getNextOutNum()
    {
      return Content(regdocRepository.getNextOutNum());
    }

    [Authorize(Roles = "admin,regdoc")]
    public ActionResult FileUploadOG(int? Id, HttpPostedFileBase fn)
    {
      if (fn != null && fn.ContentLength > 0)
      {
        var prefix = @"\\am-uralsib.ru\uralsib\MSK\COMMON\VOL1\ASSETS\CorpEvents\RegDoc\Outgoing";
        var dir = Path.Combine(prefix, DateTime.Today.ToString("yy"));
        if (!Directory.Exists(dir))
          Directory.CreateDirectory(dir);
        var file = Path.Combine(DateTime.Today.ToString("yy"), string.Format("{0}_{1}", Id, Path.GetFileName(fn.FileName)));
        var path = Path.Combine(prefix, file);
        if (System.IO.File.Exists(path))
          System.IO.File.Delete(path);
        fn.SaveAs(path);
        return new JsonnResult { Data = new { success = true, message = "Сохранено", file = file }, ContentType = "text/html" };
      }
      return new JsonnResult { Data = new { success = false, message = "Нет файла" }, ContentType = "text/html" };
    }

    [Authorize]
    public ActionResult GetFileOG(string data)
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
      var prefix = @"\\am-uralsib.ru\uralsib\MSK\COMMON\VOL1\ASSETS\CorpEvents\RegDoc\Outgoing";
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

  }
}
