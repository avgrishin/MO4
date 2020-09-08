using ClosedXML.Excel;
using MO.Areas.Code.Models;
using MO.Helpers;
using MO.Hubs;
using MO.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MO.Areas.Code.Controllers
{
  [Authorize(Roles = "mo")]
  public class FinController : Controller
  {
    public IFinRepository finRepository;
    protected readonly Lazy<Microsoft.AspNet.SignalR.IHubContext> AHub = new Lazy<Microsoft.AspNet.SignalR.IHubContext>(() => Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<AHub>());

    public FinController(IFinRepository _finRepository)
    {
      finRepository = _finRepository;
    }

    public ActionResult Charges()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);
      return View();
    }

    public ActionResult ChargesPFP()
    {
      return Json(new { data = finRepository.GetObjClsNode(1054) });
    }

    public ActionResult GetPlatList()
    {
      var q = finRepository.GetPlatList();
      return new JsonnResult { Data = new { data = q } };
    }

    public ActionResult ChargesList(DateTime? d1, DateTime? d2, int? DateType, int? FinInstID, int? pfp, bool? np, string sort, string dir)
    {
      var q1 = finRepository.ChargesList(d1, d2, DateType, FinInstID, pfp, np, sort, dir);
      return new JsonnResult { Data = new { data = q1 } };
    }

    public ActionResult ChargesDetList(DateTime? d1, DateTime? d2, int? DateType, int? FinInstID, int? pfpID, int? itemID, string sort, string dir)
    {
      var q1 = finRepository.ChargesDet(d1, d2, DateType, FinInstID, pfpID, itemID, sort, dir);
      return new JsonnResult { Data = new { data = q1 } };
    }

    public ActionResult GetPlatType()
    {
      var q = finRepository.GetObjClsByParentID(15185);
      return Json(new { data = q, totalCount = q.Count() });
    }

    public ActionResult ChargesDetCreate(List<tCharges2> data)
    {
      var q = finRepository.ChargesCreate(data);
      return new JsonnResult { Data = new { success = true, message = "Сохранено", data = q } };
    }

    public ActionResult ChargesDetUpdate(List<tCharges2> data)
    {
      var q = finRepository.ChargesUpdate(data);
      return new JsonnResult { Data = new { success = true, message = "Сохранено", data = q } };
    }

    public ActionResult ChargesDetDel(List<tCharges2> data)
    {
      if (finRepository.ChargesDel(data))
        return Json(new { success = true });
      else
        return Json(new { success = false, msg = "Ошибка при удалении" });
    }

    public ActionResult GetPoluch2(int? id, string query, int? start, int? limit)
    {
      if (id.HasValue)
        return Json(new { data = finRepository.GetContragent(id.Value) }, JsonRequestBehavior.AllowGet);
      else
        return Json(new { data = finRepository.GetContragents(query ?? "", limit ?? 10) }, JsonRequestBehavior.AllowGet);
    }

    public ActionResult AddContragent(string name)
    {
      var q = finRepository.AddContragent(name);
      return new JsonnResult { Data = new { success = true, message = "Сохранено", data = q } };
    }

    [Authorize(Roles = "admin,mo")]
    public ActionResult FileUploadCh(int id, HttpPostedFileBase fn)
    {
      if (fn != null && fn.ContentLength > 0)
      {
        var prefix = @"\\am-uralsib.ru\uralsib\MSK\COMMON\VOL1\ASSETS\CorpEvents\Charges";
        var dir = Path.Combine(prefix, DateTime.Today.ToString("yy"));
        if (!Directory.Exists(dir))
          Directory.CreateDirectory(dir);
        var file = Path.Combine(DateTime.Today.ToString("yy"), string.Format("{0}_{1}", id, Path.GetFileName(fn.FileName)));
        var path = Path.Combine(prefix, file);
        if (System.IO.File.Exists(path))
          System.IO.File.Delete(path);
        fn.SaveAs(path);
        return new JsonnResult { Data = new { success = true, message = "Сохранено", file = file }, ContentType = "text/html" };
      }
      return new JsonnResult { Data = new { success = false, message = "Нет файла" }, ContentType = "text/html" };
    }

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
      var prefix = @"\\am-uralsib.ru\uralsib\MSK\COMMON\VOL1\ASSETS\CorpEvents\Charges";
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

    public ActionResult GetPeriodicity()
    {
      var q = finRepository.GetObjClsByParentID(47185);
      return Json(new { data = q, totalCount = q.Count() });
    }

    public ActionResult GetTR()
    {
      var q = finRepository.GetObjClsByParentID(58880);
      return Json(new { data = q, totalCount = q.Count() });
    }

    public ActionResult ChargesExport(DateTime? d1, DateTime? d2, int? DateType, int? FinInstID)
    {
      var q = finRepository.ChargesExport(d1, d2, DateType, FinInstID);
      var workbook = new XLWorkbook();
      var worksheet = workbook.Worksheets.Add("Расходы");
      worksheet.Column(1).Width = 5.5;
      worksheet.Column(2).Width = 10;
      worksheet.Column(3).Width = 11.74;
      worksheet.Column(4).Width = 23;
      worksheet.Column(5).Width = 57;
      worksheet.Column(6).Width = 6.14;
      worksheet.Column(7).Width = 6.29;
      worksheet.Column(8).Width = 9.86;
      worksheet.Column(9).Width = 10;
      worksheet.Column(10).Width = 14.86;
      worksheet.Column(11).Width = 8.57;
      worksheet.Column(12).Width = 7.29;
      worksheet.Column(13).Width = 12.71;
      worksheet.Column(14).Width = 52;
      worksheet.Cell(1, 1).Value = "ID";
      worksheet.Cell(1, 2).Value = "Дата учёта";
      worksheet.Cell(1, 3).Value = "Дата оплаты";
      worksheet.Cell(1, 4).Value = "Плательщик";
      worksheet.Cell(1, 5).Value = "Получатель";
      worksheet.Cell(1, 6).Value = "Статья";
      worksheet.Cell(1, 7).Value = "ПФП";
      worksheet.Cell(1, 8).Value = "Тип платежа";
      worksheet.Cell(1, 9).Value = "Конец периода";
      worksheet.Cell(1, 10).Value = "Периодичность";
      worksheet.Cell(1, 11).Value = "Т/Р";
      worksheet.Cell(1, 12).Value = "Валюта";
      worksheet.Cell(1, 13).Value = "План";
      worksheet.Cell(1, 14).Value = "Примечание";
      var range = worksheet.Range(1, 1, 1, 14);
      range.Style.Fill.BackgroundColor = XLColor.FromArgb(233, 217, 253);
      range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
      range.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
      range.Style.Alignment.SetWrapText(true);
      range.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin).Border.SetInsideBorder(XLBorderStyleValues.Thin);
      var i = 2;
      foreach (var r in q)
      {
        worksheet.Cell(i, 1).SetValue(r.id);
        worksheet.Cell(i, 2).SetValue(r.DateReg);
        worksheet.Cell(i, 3).SetValue(r.DatePay);
        worksheet.Cell(i, 4).SetValue(r.FinInst);
        worksheet.Cell(i, 5).SetValue(r.Receiver);
        worksheet.Cell(i, 6).SetValue(r.Item);
        worksheet.Cell(i, 7).SetValue(r.Pfp);
        worksheet.Cell(i, 8).SetValue(r.TypeName);
        worksheet.Cell(i, 9).SetValue((DateTime?)r.DateRegEnd);
        worksheet.Cell(i, 10).SetValue(r.PeriodicityName);
        worksheet.Cell(i, 11).SetValue(r.TRName);
        worksheet.Cell(i, 12).SetValue(r.Fund);
        worksheet.Cell(i, 13).SetValue(r.QtyP);
        worksheet.Cell(i, 14).SetValue(r.Comment);
        i++;
      }
      range.Style.Alignment.SetWrapText(true);
      worksheet.Range(2, 13, i - 1, 13).Style.NumberFormat.NumberFormatId = 4;
      worksheet.Range(2, 14, i - 1, 14).Style.Alignment.SetWrapText(true);
      using (MemoryStream ms = new MemoryStream())
      {
        workbook.SaveAs(ms);
        return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", string.Format("chrg{0}.xlsx", DateTime.Now.Second));
      }
    }

    public ActionResult ChargesImport(HttpPostedFileBase file)
    {
      var i = 0;
      var s = 0;
      var f = 0;
      if (file != null && file.ContentLength > 0)
      {
        var fn = Path.Combine("c:\\tmp", Path.GetFileName(file.FileName));
        file.SaveAs(fn);
        var workbook = new XLWorkbook(fn);
        var worksheet = workbook.Worksheet(1);
        var rows = worksheet.RangeUsed().RowsUsed();
        foreach (var row in rows)
        {
          if (i++ > 0)
          {
            try
            {
              var c = new ChargesExport();
              c.id = row.Cell(1).Value is string ? null : (int?)(double?)row.Cell(1).Value;
              c.DateReg = row.Cell(2).Value is string ? null : (DateTime?)row.Cell(2).Value;
              c.DatePay = row.Cell(3).Value is string ? null : (DateTime?)row.Cell(3).Value;
              c.FinInst = row.Cell(4).Value.ToString();
              c.Receiver = row.Cell(5).Value.ToString();
              c.Item = row.Cell(6).Value.ToString();
              c.Pfp = row.Cell(7).Value.ToString();
              c.TypeName = (string)row.Cell(8).Value;
              c.DateRegEnd = row.Cell(9).Value is string ? null : (DateTime?)row.Cell(9).Value;
              c.PeriodicityName = (string)row.Cell(10).Value;
              c.TRName = (string)row.Cell(11).Value;
              c.Fund = (string)row.Cell(12).Value;
              c.QtyP = row.Cell(13).Value is double? ? (decimal?)(double?)row.Cell(13).Value : null;
              c.Comment = (string)row.Cell(14).Value;
              if (finRepository.ImpCharges(c))
              {
                row.Delete(XLShiftDeletedCells.ShiftCellsUp);
                s++;
              }
              else f++;
            }
            catch (Exception /*ex*/)
            {
              f++;
            }
          }
        }
        using (MemoryStream ms = new MemoryStream())
        {
          workbook.SaveAs(ms);
          return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", string.Format("chrg{0:HHmm}.xlsx", DateTime.Now));
        }
        //          return new JsonnResult { Data = new { success = true, message = string.Format("Сохранено {0}, не удалось сохранить {1}", s, f), file = file.FileName }, ContentType = "text/html" };
      }
      return new JsonnResult { Data = new { success = false, message = "Нет файла" }, ContentType = "text/html" };
    }

    public ActionResult ChargesBCS()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);
      return View();
    }
    
    public ActionResult ChargesBCSList(DateTime? d1, DateTime? d2, int? DateType, bool? np, string sort, string dir)
    {
      var q1 = finRepository.ChargesBCSList(d1, d2, DateType, np, sort, dir);
      return new JsonnResult { Data = new { data = q1 } };
    }

  }
}