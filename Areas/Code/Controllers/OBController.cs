using ClosedXML.Excel;
using MO.Areas.Code.Models;
using MO.Helpers;
using MO.Hubs;
using MO.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Web.Mvc;

namespace MO.Areas.Code.Controllers
{
  [Authorize]
  public class OBController : Controller
  {
    public IOBRepository obRepository;
    protected readonly Lazy<Microsoft.AspNet.SignalR.IHubContext> AHub = new Lazy<Microsoft.AspNet.SignalR.IHubContext>(() => Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<AHub>());

    public OBController(IOBRepository _obRepository)
    {
      obRepository = _obRepository;
    }

    [Authorize(Roles = "admin,risk")]
    public ActionResult pu()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);
      return View();
    }
    [Authorize(Roles = "admin,risk")]
    public ActionResult getPoolList(int TypeID, string sort, string dir)
    {
      return new JsonnResult { Data = new { success = true, data = obRepository.getPoolList(TypeID, sort, dir) } };
    }
    [Authorize(Roles = "admin,risk")]
    public ActionResult getPoolUser(int PoolID, string sort, string dir)
    {
      return new JsonnResult { Data = new { success = true, data = obRepository.getPoolUser(PoolID, sort, dir) } };
    }
    [Authorize(Roles = "admin,risk")]
    public ActionResult getUser(string sort, string dir)
    {
      return new JsonnResult { Data = new { success = true, data = obRepository.getUserByGroup(1, sort, dir) } };
    }
    [Authorize(Roles = "admin,risk")]
    public ActionResult AddPoolUser(int PoolID, int UserID, DateTime StartDate)
    {
      return new JsonnResult { Data = new { success = obRepository.addPoolUser(PoolID, UserID, StartDate) } };
    }
    [Authorize(Roles = "admin,risk")]
    public ActionResult DelPoolUser(int id)
    {
      return new JsonnResult { Data = new { success = obRepository.delPoolUser(id) } };
    }

    public ActionResult TestClosedXML()
    {
      var workbook = new XLWorkbook();
      var ws = workbook.Worksheets.Add("Sample Sheet");
      ws.Cell(2, 2).Value = "Hello World!";
      var col1 = ws.Column(2);
      col1.Style.Fill.BackgroundColor = XLColor.Amethyst;
      col1.Width = 20;

      ws.Range(ws.Cell(3, 3), ws.Cell(4, 4)).Style.Fill.BackgroundColor = XLColor.Red;

      var ms = new System.IO.MemoryStream();
      workbook.SaveAs(ms);
      return File(ms.ToArray(), "application/vnd.ms-excel", "pl.xlsx");
    }

    [Authorize(Roles = "admin,client,clienta")]
    public ActionResult cl()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);

      var principal = UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Domain), User.Identity.Name);
      ViewBag.KM = string.Format("{0} {1}", string.IsNullOrEmpty(principal.Surname) ? "" : principal.Surname, string.IsNullOrEmpty(principal.GivenName) ? "" : ((principal.GivenName.Left(1) + ".") + (string.IsNullOrEmpty(principal.MiddleName) ? "" : (principal.MiddleName.Left(1) + "."))));
      return View();
    }
    [Authorize(Roles = "admin,client,clienta")]
    public ActionResult getClientList(string filter, string sort, string dir)
    {
      return new JsonnResult { Data = new { success = true, data = obRepository.GetClientList(filter, User.Identity.Name, User.IsInRole("clienta"), sort, dir) } };
    }
    [Authorize(Roles = "admin,client,clienta")]
    public ActionResult addClient(List<tClient> data)
    {
      return new JsonnResult { Data = new { success = true, data = obRepository.AddClient(data, User.Identity.Name) } };
    }
    [Authorize(Roles = "admin,client,clienta")]
    public ActionResult updClient(List<tClient> data)
    {
      return new JsonnResult { Data = new { success = true, data = obRepository.UpdClient(data, User.Identity.Name, User.IsInRole("clienta")) } };
    }

    [Authorize(Roles = "admin,client,clienta")]
    public ActionResult getClientActList(int id, DateTime? d1, DateTime? d2, string sort, string dir)
    {
      return new JsonnResult { Data = new { success = true, data = obRepository.GetClientActList(id, d1, d2, sort, dir) } };
    }
    [Authorize(Roles = "admin,client,clienta")]
    public ActionResult addClientAct(List<tClientAct> data)
    {
      return new JsonnResult { Data = new { success = true, data = obRepository.AddClientAct(data, User.Identity.Name) } };
    }
    [Authorize(Roles = "admin,client,clienta")]
    public ActionResult updClientAct(List<tClientAct> data)
    {
      return new JsonnResult { Data = new { success = true, data = obRepository.UpdClientAct(data, User.Identity.Name) } };
    }

    [Authorize(Roles = "admin,client,clienta")]
    public ActionResult delClientAct(List<tClientAct> data)
    {
      return new JsonnResult { Data = new { success = obRepository.DelClientAct(data) } };
    }

    [Authorize(Roles = "admin,client,clienta")]
    public ActionResult GetKind()
    {
      var q = obRepository.GetObjClsByParentID(63954);
      return new JsonnResult { Data = new { success = true, data = q } };
    }
    [HttpPost]
    [Authorize(Roles = "admin,client,clienta")]
    public ActionResult repClient(DateTime? d1, DateTime? d2)
    {
      var workbook = new XLWorkbook();
      var q = obRepository.RepClient(d1, d2, User.Identity.Name, User.IsInRole("clienta"));
      var worksheet = workbook.Worksheets.Add("Отчёт");
      worksheet.Column(1).Width = 32;
      worksheet.Column(2).Width = 8;
      worksheet.Column(3).Width = 9.5;
      worksheet.Column(4).Width = 20.5;
      worksheet.Column(5).Width = 9;
      worksheet.Column(6).Width = 22;
      worksheet.Column(7).Width = 15;
      worksheet.Column(8).Width = 15;
      worksheet.Column(9).Width = 22;
      worksheet.Column(10).Width = 11;
      worksheet.Column(11).Width = 11;
      worksheet.Column(12).Width = 9.5;
      worksheet.Column(13).Width = 18;
      worksheet.Column(14).Width = 140 / 2;
      worksheet.Column(15).Width = 9.5;
      worksheet.Column(16).Width = 8;
      worksheet.Column(17).Width = 9;
      worksheet.Column(18).Width = 22;
      worksheet.Cell(1, 1).Value = "ФИО клиента";
      worksheet.Cell(1, 2).Value = "Ф/Ю";
      worksheet.Cell(1, 3).Value = "ДР";
      worksheet.Cell(1, 4).Value = "Email";
      worksheet.Cell(1, 5).Value = "Город";
      worksheet.Cell(1, 6).Value = "Род деятельности";
      worksheet.Cell(1, 7).Value = "КМ";
      worksheet.Cell(1, 8).Value = "Моб телефон";
      worksheet.Cell(1, 9).Value = "Должность";
      worksheet.Cell(1, 10).Value = "Потенциал";
      worksheet.Cell(1, 11).Value = "Риск-профиль";
      worksheet.Cell(1, 12).Value = "Дата контакта";
      worksheet.Cell(1, 13).Value = "Вид контакта";
      worksheet.Cell(1, 14).Value = "Результат";
      worksheet.Cell(1, 15).Value = "Дата следующего контакта";
      worksheet.Cell(1, 16).Value = "Сделка?";
      worksheet.Cell(1, 17).Value = "Сумма сделки";
      worksheet.Cell(1, 18).Value = "Продукт";
      worksheet.Cell(1, 19).Value = "Баллы";

      var i = 2;
      var UserName = "";
      var Ball = 0;
      foreach (var r in q)
      {
        if (UserName != r.UserName)
        {
          if (UserName != "")
          {
            worksheet.Cell(i, 18).Value = "Итого";
            worksheet.Cell(i, 19).Value = Ball;
            Ball = 0;
            i += 2;
          }
          UserName = r.UserName;
        }
        worksheet.Cell(i, 1).Value = r.FIO;
        worksheet.Cell(i, 2).Value = r.IsUL == true ? "ЮЛ" : r.IsUL == false ? "ФЛ" : "";
        worksheet.Cell(i, 3).Value = r.BirthDay;
        worksheet.Cell(i, 4).Value = r.Email;
        worksheet.Cell(i, 5).Value = r.AddressTown;
        worksheet.Cell(i, 6).Value = r.KindActivity;
        worksheet.Cell(i, 7).Value = r.KM;
        worksheet.Cell(i, 8).Value = r.Phone1;
        worksheet.Cell(i, 9).Value = r.Position;
        worksheet.Cell(i, 10).Value = r.Potential;
        worksheet.Cell(i, 11).Value = r.RiskProfil;
        worksheet.Cell(i, 12).Value = r.DateC;
        worksheet.Cell(i, 13).Value = r.KindS;
        worksheet.Cell(i, 14).Value = r.Result;
        worksheet.Cell(i, 15).Value = r.DateN;
        worksheet.Cell(i, 16).Value = r.IsDeal == true ? "Да" : r.IsDeal == false ? "Нет" : "";
        worksheet.Cell(i, 17).Value = r.Qty;
        worksheet.Cell(i, 18).Value = r.Product;
        worksheet.Cell(i, 19).Value = r.Ball > 0 ? r.Ball : null;
        worksheet.Range(i, 1, i, 19).Style.Fill.BackgroundColor = XLColor.FromArgb(253, 233, 217);

        Ball += r.Ball;
        i++;
      }
      if (UserName != "")
      {
        worksheet.Cell(i, 18).Value = "Итого";
        worksheet.Cell(i, 19).Value = Ball;
      }
      var border = worksheet.Range(2, 1, i, 19).Style.Border;
      border.OutsideBorder = XLBorderStyleValues.Thin;
      border.InsideBorder = XLBorderStyleValues.Thin;
      worksheet.Range(2, 14, i, 14).Style.Alignment.WrapText = true;
      MemoryStream ms = new MemoryStream();
      workbook.SaveAs(ms);
      return File(ms.ToArray(), "application/vnd.ms-excel", string.Format("cl{0}.xlsx", DateTime.Now.Second));
    }

    [HttpPost]
    [Authorize(Roles = "admin,client,clienta")]
    public ActionResult RepClientBalls(DateTime d2)
    {
      var workbook = new XLWorkbook();
      var q = obRepository.RepClientBalls(d2, User.Identity.Name, User.IsInRole("clienta"));
      var worksheet = workbook.Worksheets.Add("Отчёт");
      worksheet.Cell(1, 1).Value = d2;
      worksheet.Column(1).Width = 32;
      worksheet.Column(2).Width = 8;
      worksheet.Column(3).Width = 9.5;
      worksheet.Column(4).Width = 20.5;
      worksheet.Column(5).Width = 9;
      worksheet.Column(6).Width = 22;
      worksheet.Column(7).Width = 22;
      worksheet.Column(8).Width = 22;
      worksheet.Column(9).Width = 22;
      worksheet.Column(10).Width = 22;
      worksheet.Cell(2, 1).Value = "ФИО КМ";
      worksheet.Cell(2, 2).Value = "Выполнение плана (%)";
      worksheet.Cell(2, 3).Value = "% от выполненного плана";
      worksheet.Cell(2, 4).Value = "Звонок";
      worksheet.Cell(2, 5).Value = "Встреча";
      worksheet.Cell(2, 6).Value = "Cделка";
      worksheet.Cell(2, 7).Value = "Покупка паёв";
      worksheet.Cell(2, 8).Value = "Приток ДУ";
      worksheet.Cell(2, 9).Value = "Отток ДУ";
      worksheet.Cell(2, 10).Value = "Портфель ДУ";
      worksheet.Range(2, 1, 2, 10).Style.Fill.BackgroundColor = XLColor.FromArgb(255, 255, 153);

      var i = 3;
      foreach (var r in q)
      {
        worksheet.Cell(i, 1).Value = r.UN;
        worksheet.Cell(i, 2).Value = 100;
        worksheet.Cell(i, 3).Value = r.Ball;
        worksheet.Cell(i, 4).Value = r.Call;
        worksheet.Cell(i, 5).Value = r.Meet;
        worksheet.Cell(i, 6).Value = r.Deal;
        worksheet.Cell(i, 7).Value = r.paib;
        worksheet.Cell(i, 8).Value = r.QtyI;
        worksheet.Cell(i, 9).Value = r.QtyO;
        worksheet.Cell(i, 10).Value = r.Qty;
        i++;
      }
      worksheet.Range(3, 7, i - 1, 10).Style.NumberFormat.NumberFormatId = 4;
      var border = worksheet.Range(2, 1, i - 1, 10).Style.Border;
      border.OutsideBorder = XLBorderStyleValues.Thin;
      border.InsideBorder = XLBorderStyleValues.Thin;

      MemoryStream ms = new MemoryStream();
      workbook.SaveAs(ms);
      return File(ms.ToArray(), "application/vnd.ms-excel", string.Format("bl{0}.xlsx", DateTime.Now.Second));
    }
    [Authorize(Roles = "jrpc")]
    public ActionResult rc()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);
      return View();
    }
    public ActionResult getDogNumber(string q)
    {
      return new JsonnResult { Data = new { data = obRepository.getDogNumber(q) } };
    }
    public ActionResult getOrdNumber(string q)
    {
      return new JsonnResult { Data = new { data = obRepository.getOrdNumber(q) } };
    }
    public ActionResult getKM(string q)
    {
      return new JsonnResult { Data = new { data = obRepository.GetKM(q) } };
    }
    public ActionResult getPPZ()
    {
      return new JsonnResult { Data = new { data = obRepository.getPPZ() } };
    }
    [Authorize(Roles = "jrpc")]
    public ActionResult addReqChange1(tReqChange data)
    {
      return new JsonnResult { Data = new { success = obRepository.AddReqChange1(data, User.Identity.Name) } };
    }
    [Authorize(Roles = "jrpc")]
    public ActionResult addReqChange2(tReqChange data)
    {
      return new JsonnResult { Data = new { success = obRepository.AddReqChange2(data, User.Identity.Name) } };
    }

    public ActionResult cp() {
      return View();
    }
    [HttpPost]
    [ActionName("cp")]
    public ActionResult RepCouponPays(int TreatyID, DateTime d)
    {
      var workbook = new XLWorkbook();
      var q = obRepository.getCouponPays(TreatyID, d);
      var worksheet = workbook.Worksheets.Add("Отчёт");
      worksheet.Style.Font.FontName = "Arial";
      worksheet.Style.Font.SetFontSize(8);
      var table = worksheet.Cell(1, 1).InsertTable(q, "Data", true);
      if (q.Length > 0)
      {
        var pt = worksheet.PivotTables.AddNew("PivotTable", worksheet.Cell(2, 7), table.AsRange());
        pt.AutofitColumns = true;
        pt.RowLabels.Add("Brief").SortType = XLPivotSortType.Ascending;
        pt.ColumnLabels.Add("ym").SortType = XLPivotSortType.Ascending;
        pt.Values.Add("val");
      }
      MemoryStream ms = new MemoryStream();
      workbook.SaveAs(ms);
      return File(ms.ToArray(), "application/vnd.ms-excel", string.Format("bl{0}.xlsx", DateTime.Now.Second));
    }

    public ActionResult sp()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);
      return View();
    }

    public ActionResult getTreatyList(string filter, int TreatyTypeID, string sort, string dir)
    {
      return new JsonnResult { Data = new { success = true, data = obRepository.GetTreatyList(filter, TreatyTypeID, sort, dir) } };
    }

    public ActionResult getPortfolioTreatyList(int TreatyID, int PortfolioTypeID)
    {
      return new JsonnResult { Data = new { success = true, data = obRepository.GetPortfolioTreatyList(TreatyID, PortfolioTypeID) } };
    }

    public ActionResult getPortfolioList(string filter, int? TypeID, string sort, string dir)
    {
      return new JsonnResult { Data = new { success = true, data = obRepository.GetPortfolioList(filter, TypeID, sort ?? "Name", dir) } };
    }

    public ActionResult addPortfolioTreaty(int TreatyID, int PortfolioID, DateTime DateStart)
    {
      return new JsonnResult { Data = new { success = obRepository.AddPortfolioTreaty(TreatyID, PortfolioID, DateStart) } };
    }

    public class PotfolioTreaty { public int ID { get; set; } public int TreatyID { get; set; } public int PortfolioID { get; set; } }
    public ActionResult delTreatyPortfolio(List<PotfolioTreaty> data)
    {
      if (data != null && data.Count > 0)
        return new JsonnResult { Data = new { success = obRepository.DelPortfolioTreaty(data[0].ID) } };
      else
        return new JsonnResult { Data = new { success = false } };
    }

  }
}