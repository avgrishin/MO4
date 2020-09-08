using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using MO.Helpers;
using MO.Hubs;
using MO.Models;
using MO.ViewModels;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.SS.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using ClosedXML.Excel;
using System.Text;

namespace MO.Controllers
{
  public class ReportController : Controller
  {
    public IReportRepository repRepository;
    protected readonly Lazy<Microsoft.AspNet.SignalR.IHubContext> AHub = new Lazy<Microsoft.AspNet.SignalR.IHubContext>(() => Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<AHub>());

    public ReportController(IReportRepository repRepo)
    {
      repRepository = repRepo;
    }

    [Authorize]
    public ActionResult Index()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);
      return View();
    }

    [Authorize(Roles = "bank, mo, bank1, PIF")]
    public ActionResult RepParam(int id)
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);
      switch (id)
      {
        case 2:
          ViewBag.Title = "Отчет управляющего";
          var r2 = new Rep1Param();
          return View("RepParam2", r2);
        case 3:
          Rep3Param r3 = new Rep3Param() { DateB = DateTime.Today.AddDays(-DateTime.Today.DayOfYear + 1), DateE = DateTime.Today.AddDays(-1) };
          ViewBag.Title = "Отчет";
          return View("RepParam3", r3);
        case 4:
          Rep4Param r4 = new Rep4Param { d = DateTime.Today.AddDays(-DateTime.Today.Day) };
          ViewBag.Title = "Отчет Уралсиб Фонд Первый";
          return View("RepParam4", r4);
        case 5:
          var r5 = new Rep4Param { d = DateTime.Today.AddDays(-DateTime.Today.Day) };
          ViewBag.Title = "Отчет Уралсиб Фонд Отраслевых Инвестиций";
          return View("RepParam4", r5);
        case 6:
          var r6 = new Rep4Param { d = DateTime.Today.AddDays(-DateTime.Today.Day) };
          ViewBag.Title = "Отчет УралСиб Фонд Перспективных вложений";
          return View("RepParam4", r6);
        case 7:
          var r7 = new Rep4Param { d = DateTime.Today.AddDays(-DateTime.Today.Day) };
          ViewBag.Title = "Отчет УралСиб Фонд Консервативный";
          return View("RepParam4", r7);
        case 8:
          var r8 = new Rep4Param { d = DateTime.Today.AddDays(-DateTime.Today.Day) };
          ViewBag.Title = "Отчет УралСиб Фонд Профессиональный";
          return View("RepParam4", r8);
        case 9:
          var r9 = new Rep4Param { d = DateTime.Today.AddDays(-DateTime.Today.Day) };
          ViewBag.Title = "Отчет Уралсиб Энергетическая перспектива";
          return View("RepParam4", r9);
        case 10:
          var r10 = new Rep4Param { d = DateTime.Today.AddDays(-DateTime.Today.Day) };
          ViewBag.Title = "Отчет Уралсиб Металлы России";
          return View("RepParam4", r10);
        case 11:
          var r11 = new Rep4Param { d = DateTime.Today.AddDays(-DateTime.Today.Day) };
          ViewBag.Title = "Отчет Уралсиб Нефть и Газ";
          return View("RepParam4", r11);
        case 12:
          var r12 = new Rep4Param { d = DateTime.Today.AddDays(-DateTime.Today.Day) };
          ViewBag.Title = "Отчет Уралсиб Российские финансы";
          return View("RepParam4", r12);
        case 13:
          var r13 = new Rep4Param { d = DateTime.Today.AddDays(-DateTime.Today.Day) };
          ViewBag.Title = "Отчет Уралсиб Связь и Информационные технологии";
          return View("RepParam4", r13);
        case 14:
          var r14 = new Rep3Param { DateB = DateTime.Today.AddDays(-DateTime.Today.DayOfYear + 1), DateE = DateTime.Today.AddDays(-DateTime.Today.Day) };
          ViewBag.Title = "Отчет Fixed Income";
          return View("RepParam3", r14);
        case 15:
          var r15 = new Rep4Param { d = DateTime.Today.AddDays(-DateTime.Today.Day) };
          ViewBag.Title = "Отчет РСМС";
          return View("RepParam4", r15);
        case 16:
          var r16 = new Rep3Param { DateB = DateTime.Today.AddDays(-DateTime.Today.DayOfYear + 1), DateE = DateTime.Today.AddDays(-DateTime.Today.Day) };
          ViewBag.Title = "Отчет Extra Return";
          return View("RepParam3", r16);
        case 17:
          ViewBag.Title = "Отчет по пайщику";
          var r17 = new RepParam17();
          r17.d1 = DateTime.Today.AddDays(1 - DateTime.Today.Day).AddMonths(-1);
          r17.d2 = DateTime.Today.AddDays(-DateTime.Today.Day);
          r17.cid = 0;
          return View("RepParam5", r17);
      }
      return HttpNotFound();
    }

    [Authorize(Roles = "bank, mo, PIF")]
    public ActionResult GetFI(int? id, string query, int? start, int? limit)
    {
      if (id.HasValue)
        return Json(new { data = repRepository.GetFinInst(id.Value) }, JsonRequestBehavior.AllowGet);
      else
        return Json(new { data = repRepository.GetFinInsts(query ?? "", limit ?? 10, User.IsInRole("bank"), User.IsInRole("PIF")) }, JsonRequestBehavior.AllowGet);
    }

    [Authorize(Roles = "bank, mo, PIF")]
    public ActionResult GetFunds()
    {
      return Json(new { data = from q in repRepository.GetFunds() select new { id = q.SecurityID, brief = q.NameBrief.TrimEnd(), name = q.Name1.TrimEnd() } }, JsonRequestBehavior.AllowGet);
    }

    [Authorize(Roles = "bank, mo, PIF")]
    public ActionResult GetIndexes()
    {
      return Json(new { data = from q in repRepository.GetIndexes() select new { id = q.ExchangeIndexID, brief = q.Brief.TrimEnd(), name = q.IndexName.TrimEnd() } }, JsonRequestBehavior.AllowGet);
    }

    [Authorize(Roles = "bank, mo, PIF")]
    public ActionResult GetStrategies()
    {
      return Json(new { data = from q in repRepository.GetStrategies() select new { id = q.PortfolioID, brief = q.NameBrief.TrimEnd(), name = q.Name.TrimEnd() } }, JsonRequestBehavior.AllowGet);
    }

    [Authorize(Roles = "bank, mo, bank1, PIF")]
    public ActionResult GetParam(int id)
    {
      switch (id)
      {
        case 2:
          ProfileCommon profile = ProfileCommon.Create(User.Identity.Name, true) as ProfileCommon;
          return Json(new
          {
            success = true,
            data = new
            {
              DateB = (profile.db ?? new DateTime(2009, 1, 1)).ToShortDateString(),
              DateE = (profile.de ?? DateTime.Today.AddDays(-1)).ToShortDateString(),
              instID = profile.FinInstID,
              fundID = profile.FundID == null ? 39191 : profile.FundID,
              indexID = profile.IndexID,
              strategyID = profile.StrategyID
            }
          });
      }
      return View("NotFound");
    }

    [Authorize(Roles = "bank, mo, PIF")]
    public ActionResult Chart(int id)
    {
      Rep1Param r1 = new Rep1Param() { repRepository = repRepository, isBank = User.IsInRole("bank"), isPIF = User.IsInRole("PIF") };
      try
      {
        Chart chart = null;
        TryUpdateModel(r1, new string[] { "DateB", "DateE", "finInstID", "fundID", "indexID", "strategyID" });

        if (ModelState.IsValid)
        {
          if (id == 0)
          {
            chart = new Chart()
            {
              Width = new System.Web.UI.WebControls.Unit(349, System.Web.UI.WebControls.UnitType.Pixel),
              Palette = ChartColorPalette.EarthTones,
              BorderlineWidth = 0
            };
            chart.Legends.Add(new Legend { Docking = Docking.Bottom, TableStyle = LegendTableStyle.Wide, BorderWidth = 0 });
            chart.Titles.Add(new Title { Text = "Структура по видам активов", Font = new System.Drawing.Font("Trebuchet MS", 10, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point) });
            chart.Series.Add(new Series
            {
              ChartType = SeriesChartType.Pie,
              XValueMember = "Name",
              YValueMembers = "Qty",
              BorderColor = System.Drawing.Color.Gray,
              CustomProperties = "PieDrawingStyle=Concave",
              Label = "#PERCENT",
              LegendText = "#VALX #VAL{N2}"
            });
            chart.ChartAreas.Add(new ChartArea());
            chart.DataSource = repRepository.GetRepClient0(r1.DateB, r1.DateE, r1.finInstID, r1.fundID, r1.indexID, r1.strategyID);
            chart.DataBind();
          }
          else if (id == 1)
          {
            chart = new Chart()
            {
              Width = new System.Web.UI.WebControls.Unit(349, System.Web.UI.WebControls.UnitType.Pixel),
              Palette = ChartColorPalette.EarthTones,
              BorderlineWidth = 0
            };
            chart.Titles.Add(new Title { Text = "Структура по отраслям", Font = new System.Drawing.Font("Trebuchet MS", 10, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point) });
            chart.Series.Add(new Series
            {
              ChartType = SeriesChartType.Bar,
              XValueMember = "Name",
              YValueMembers = "Qty",
              BorderColor = System.Drawing.Color.Gray,
              CustomProperties = "DrawingStyle=Cylinder",
              IsValueShownAsLabel = true,
              Label = "#PERCENT"
            });
            chart.ChartAreas.Add(new ChartArea
            {
              BorderWidth = 1,
              AxisY = new Axis { MajorGrid = new Grid { LineColor = System.Drawing.Color.LightGray }, Enabled = AxisEnabled.False },
              AxisX = new Axis { MajorGrid = new Grid { Enabled = false }, IntervalAutoMode = IntervalAutoMode.VariableCount }
            });
            chart.DataSource = repRepository.GetRepClient1(r1.DateB, r1.DateE, r1.finInstID, r1.fundID, r1.indexID, r1.strategyID);
            chart.DataBind();
          }
          else if (id == 2)
          {
            chart = new Chart()
            {
              Width = new System.Web.UI.WebControls.Unit(700, System.Web.UI.WebControls.UnitType.Pixel),
              Palette = ChartColorPalette.EarthTones,
              AntiAliasing = AntiAliasingStyles.All,
              TextAntiAliasingQuality = TextAntiAliasingQuality.SystemDefault,
            };
            chart.Legends.Add(new Legend { BorderWidth = 0, Alignment = System.Drawing.StringAlignment.Near });
            chart.Titles.Add(new Title { Text = "Доходность портфеля клиента (TWR), %", Font = new System.Drawing.Font("Trebuchet MS", 10, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point) });
            chart.ChartAreas.Add(new ChartArea
            {
              AxisY = new Axis
              {
                LabelAutoFitMaxFontSize = 8,
                IntervalAutoMode = IntervalAutoMode.VariableCount,
                IsLabelAutoFit = true,
                MajorGrid = new Grid
                {
                  IntervalType = DateTimeIntervalType.Number,
                  LineDashStyle = ChartDashStyle.Dot,
                  LineColor = System.Drawing.Color.LightGray
                },
                LabelStyle = new LabelStyle { Format = "N1" }
              },
              AxisY2 = new Axis
              {
                Enabled = AxisEnabled.True,
                LabelAutoFitMaxFontSize = 8,
                IntervalAutoMode = IntervalAutoMode.VariableCount,
                IsLabelAutoFit = true,
                MajorGrid = new Grid
                {
                  Enabled = false
                },
                LabelStyle = new LabelStyle { Format = "N1" }
              },
              AxisX = new Axis
              {
                IsStartedFromZero = true,
                LabelAutoFitMaxFontSize = 8,
                LabelAutoFitStyle = LabelAutoFitStyles.LabelsAngleStep90,
                //IntervalAutoMode = IntervalAutoMode.VariableCount,
                IntervalType = DateTimeIntervalType.Days,
                IsLabelAutoFit = true,
                IsMarginVisible = false,
                MajorGrid = new Grid
                {
                  LineDashStyle = ChartDashStyle.Dot,
                  //IntervalType = DateTimeIntervalType.Number,
                  LineColor = System.Drawing.Color.LightGray
                },
                LabelStyle = new LabelStyle
                {
                  Format = "dd.MM",
                  IsStaggered = true,
                  Angle = -90
                }
              }
            });
            chart.Series.Add(new Series
            {
              Name = "Клиент",
              ChartType = SeriesChartType.FastLine,
              Color = System.Drawing.Color.BlueViolet,
              XValueMember = "Date2",
              YValueMembers = "coef_all"
            });
            var r = repRepository.GetRepClient7(r1.DateB, r1.DateE, r1.finInstID, r1.fundID, r1.indexID, r1.strategyID);
            chart.Series[0].Points.DataBind(r, "Date2", "coef_all", null);
            chart.Series.Add(new Series
            {
              Name = "Индекс",
              ChartType = SeriesChartType.FastLine,
              Color = System.Drawing.Color.Red,
              XValueMember = "Date2",
              YValueMembers = "coef_allb"
            });
            chart.Series[1].Points.DataBind(r, "Date2", "coef_allb", null);
            //chart.DataSource = repRepository.GetRepClient7(r1.DateB, r1.DateE, r1.finInstID, r1.fundID, r1.indexID);
            //chart.DataBind();
          }
          if (chart != null)
          {
            var imgStream = new MemoryStream();
            chart.SaveImage(imgStream, ChartImageFormat.Png);
            imgStream.Seek(0, SeekOrigin.Begin);
            return File(imgStream, "image/png");
          }
        }
      }
      catch (Exception /*ex*/)
      {
        //ModelState.AddModelError("_FORM", ex.Message);
      }
      return View("NotFound");
    }

    [Authorize(Roles = "bank, mo, PIF")]
    public ActionResult Rep(int id)
    {
      if (id == 1)
      {
        Rep1Param r1 = new Rep1Param() { repRepository = repRepository, isBank = User.IsInRole("bank"), isPIF = User.IsInRole("PIF") };
        try
        {
          TryUpdateModel(r1, new string[] { "DateB", "DateE", "finInstID", "fundID", "indexID", "strategyID" });
          ModelState.AddModelErrors(r1.GetRuleViolations());

          if (ModelState.IsValid)
          {
            ProfileCommon profile = ProfileCommon.Create(User.Identity.Name, true) as ProfileCommon;
            profile.db = r1.DateB;
            profile.de = r1.DateE;
            profile.FundID = r1.fundID;
            profile.FinInstID = r1.finInstID;
            profile.IndexID = r1.indexID;
            profile.StrategyID = r1.strategyID;
            profile.Save();
            return View("Rep1", new Rep1ViewModel(repRepository, r1, repRepository.IsShowAssets(r1.finInstID.Value, User.IsInRole("bank"))));
          }
        }
        catch (Exception ex)
        {
          ModelState.AddModelError("_FORM", ex.Message);
        }
        return View("RepParam2", new Rep1ParamViewModel(repRepository, r1));
      }
      else if (id == 2)
      {
        Rep1Param r1 = new Rep1Param() { repRepository = repRepository, isBank = User.IsInRole("bank"), isPIF = User.IsInRole("PIF") };
        try
        {
          TryUpdateModel(r1, new string[] { "DateB", "DateE", "finInstID", "fundID", "indexID", "strategyID" });
          ModelState.AddModelErrors(r1.GetRuleViolations());

          if (ModelState.IsValid)
          {
            ProfileCommon profile = ProfileCommon.Create(User.Identity.Name, true) as ProfileCommon;
            profile.db = r1.DateB;
            profile.de = r1.DateE;
            profile.FundID = r1.fundID;
            profile.FinInstID = r1.finInstID;
            profile.IndexID = r1.indexID;
            profile.StrategyID = r1.strategyID;
            profile.Save();
            return Json(new { success = true });
          }
        }
        catch
        {
          //ModelState.AddModelError("_FORM", ex.Message);
        }
        return Json(new { success = false, data = new { ModelState } });
      }
      else if (id == 4)
      {
        return View("Rep4");
      }
      return View("NotFound");
    }

    [Authorize(Roles = "bank, mo, bank1, PIF")]
    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult RepParam(int id, FormCollection collection)
    {
      if (id == 3)
      {
        Rep3Param r3 = new Rep3Param();
        try
        {
          TryUpdateModel(r3);
          if (ModelState.IsValid)
          {
            FileStream fs = new FileStream(Server.MapPath(@"\Templates\Rep3.xls"), FileMode.Open, FileAccess.Read);
            HSSFWorkbook workbook = new HSSFWorkbook(fs, true);
            ISheet sheet = workbook.GetSheet("Лист1");
            ISingleResult<up_avgRepDUCBBResult> res = repRepository.up_avgRepDUCBB(r3.DateB, r3.DateE);
            int ir = 1;
            IRow row = sheet.GetRow(0);
            row.GetCell(9).SetCellValue(r3.DateB.Value);
            row.GetCell(10).SetCellValue(r3.DateE.Value);
            foreach (up_avgRepDUCBBResult row3 in res)
            {
              row = sheet.CreateRow(ir++);
              int c = 0;
              row.CreateCell(c++).SetCellValue(row3.FinancialInstitutionPortal ?? "");
              row.CreateCell(c++).SetCellValue(row3.ClnManager);
              row.CreateCell(c++).SetCellValue(row3.Instrument);
              row.CreateCell(c++).SetCellValue(row3.BackBoneGroupName2);
              row.CreateCell(c++).SetCellValue(row3.TreatyTypeNameBrief);
              row.CreateCell(c++).SetCellValue("");
              row.CreateCell(c++).SetCellValue(row3.fiCode);
              row.CreateCell(c++).SetCellValue(row3.Income ?? 0);
              row.CreateCell(c++).SetCellValue(row3.Outcome ?? 0);
              row.CreateCell(c++).SetCellValue(row3.ValueRURb ?? 0);
              row.CreateCell(c++).SetCellValue(row3.ValueRUR ?? 0);
            }
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            return File(ms.ToArray(), "application/vnd.ms-excel", "rep3.xls");
          }
          else
          {
            ViewBag.Title = "Отчет";
            return View("RepParam3", r3);
          }
        }
        catch (Exception ex)
        {
          ModelState.AddModelError("_FORM", ex.Message);
          ViewBag.Title = "Отчет";
          return View("RepParam3", r3);
        }

      }
      else if (id == 4)
      {
        Rep4Param r4 = new Rep4Param();
        try
        {
          UpdateModel(r4);
          if (ModelState.IsValid)
          {
            RepF1 r = new RepF1()
            {
              DateB = new DateTime(2000, 5, 1),
              DateE = r4.d,
              FinInstID = 261,
              FundID = 39191,
              IndexID = 53,
              repRepository = repRepository,
              ImagePath = Server.MapPath(@"\Content\images\pdf\")
            };
            return File(r.GetReport(), "application/pdf", "d4.pdf");
          }
        }
        catch
        {
          //ModelState.AddModelError("_FORM", ex.Message);
          ViewBag.Title = "Отчет Уралсиб Фонд Первый";
          return View("RepParam4", r4);
        }
      }
      else if (id == 5)
      {
        Rep4Param r5 = new Rep4Param();
        try
        {
          UpdateModel(r5);
          if (ModelState.IsValid)
          {
            RepF2 r = new RepF2()
            {
              DateB = new DateTime(2000, 5, 1),
              DateE = r5.d,
              FinInstID = 262,
              FundID = 39191,
              IndexID = 53,
              repRepository = repRepository,
              ImagePath = Server.MapPath(@"\Content\images\pdf\")
            };
            return File(r.GetReport(), "application/pdf", "d5.pdf");
          }
        }
        catch (Exception ex)
        {
          ModelState.AddModelError("_FORM", ex.Message);
          ViewBag.Title = "Отчет Уралсиб Фонд Отраслевых Инвестиций";
          return View("RepParam4", r5);
        }
      }
      else if (id == 6)
      {
        Rep4Param r6 = new Rep4Param();
        try
        {
          UpdateModel(r6);
          if (ModelState.IsValid)
          {
            RepF3 r = new RepF3()
            {
              DateB = new DateTime(2000, 5, 1),
              DateE = r6.d,
              FinInstID = 263,
              FundID = 39191,
              IndexID = 54,
              repRepository = repRepository,
              ImagePath = Server.MapPath(@"\Content\images\pdf\")
            };
            return File(r.GetReport(), "application/pdf", "d6.pdf");
          }
        }
        catch (Exception ex)
        {
          ModelState.AddModelError("_FORM", ex.Message);
          ViewBag.Title = "Отчет Уралсиб Фонд Перспективных вложений";
          return View("RepParam4", r6);
        }
      }
      else if (id == 7)
      {
        Rep4Param r7 = new Rep4Param();
        try
        {
          UpdateModel(r7);
          if (ModelState.IsValid)
          {
            RepK r = new RepK()
            {
              DateB = new DateTime(2001, 6, 21),
              DateE = r7.d,
              FinInstID = 264,
              FundID = 39191,
              IndexID = 16,
              repRepository = repRepository,
              ImagePath = Server.MapPath(@"\Content\images\pdf\")
            };
            return File(r.GetReport(), "application/pdf", "d7.pdf");
          }
        }
        catch (Exception ex)
        {
          ModelState.AddModelError("_FORM", ex.Message);
          ViewBag.Title = "Отчет Уралсиб Фонд Консервативный";
          return View("RepParam4", r7);
        }
      }
      else if (id == 8)
      {
        Rep4Param r8 = new Rep4Param();
        try
        {
          UpdateModel(r8);
          if (ModelState.IsValid)
          {
            RepP r = new RepP()
            {
              DateB = new DateTime(2001, 5, 11),
              DateE = r8.d,
              FinInstID = 265,
              FundID = 39191,
              IndexID = 60,
              repRepository = repRepository,
              ImagePath = Server.MapPath(@"\Content\images\pdf\")
            };
            return File(r.GetReport(), "application/pdf", "d8.pdf");
          }
        }
        catch (Exception ex)
        {
          ModelState.AddModelError("_FORM", ex.Message);
          ViewBag.Title = "Отчет Уралсиб Фонд Профессиональный";
          return View("RepParam4", r8);
        }
      }
      else if (id == 9)
      {
        Rep4Param r9 = new Rep4Param();
        try
        {
          UpdateModel(r9);
          if (ModelState.IsValid)
          {
            RepE r = new RepE()
            {
              DateB = new DateTime(2008, 3, 2),
              DateE = r9.d,
              FinInstID = 6626,
              FundID = 39191,
              IndexID = 12,
              repRepository = repRepository,
              ImagePath = Server.MapPath(@"\Content\images\pdf\")
            };
            return File(r.GetReport(), "application/pdf", "d9.pdf");
          }
        }
        catch (Exception ex)
        {
          ModelState.AddModelError("_FORM", ex.Message);
          ViewBag.Title = "Отчет Уралсиб Энергетическая перспектива";
          return View("RepParam4", r9);
        }
      }
      else if (id == 10)
      {
        Rep4Param r10 = new Rep4Param();
        try
        {
          UpdateModel(r10);
          if (ModelState.IsValid)
          {
            RepMR r = new RepMR()
            {
              DateB = new DateTime(2008, 3, 2),
              DateE = r10.d,
              FinInstID = 6630,
              FundID = 39191,
              IndexID = 47,
              repRepository = repRepository,
              ImagePath = Server.MapPath(@"\Content\images\pdf\")
            };
            return File(r.GetReport(), "application/pdf", "d10.pdf");
          }
        }
        catch (Exception ex)
        {
          ModelState.AddModelError("_FORM", ex.Message);
          ViewBag.Title = "Отчет Уралсиб Металлы России";
          return View("RepParam4", r10);
        }
      }
      else if (id == 11)
      {
        Rep4Param r11 = new Rep4Param();
        try
        {
          UpdateModel(r11);
          if (ModelState.IsValid)
          {
            RepNG r = new RepNG()
            {
              DateB = new DateTime(2008, 3, 2),
              DateE = r11.d,
              FinInstID = 6627,
              FundID = 39191,
              IndexID = 33,
              repRepository = repRepository,
              ImagePath = Server.MapPath(@"\Content\images\pdf\")
            };
            return File(r.GetReport(), "application/pdf", "d11.pdf");
          }
        }
        catch (Exception ex)
        {
          ModelState.AddModelError("_FORM", ex.Message);
          ViewBag.Title = "Отчет Уралсиб Нефть и Газ";
          return View("RepParam4", r11);
        }
      }
      else if (id == 12)
      {
        Rep4Param r12 = new Rep4Param();
        try
        {
          UpdateModel(r12);
          if (ModelState.IsValid)
          {
            RepRF r = new RepRF()
            {
              DateB = new DateTime(2008, 3, 2),
              DateE = r12.d,
              FinInstID = 6628,
              FundID = 39191,
              IndexID = 36,
              repRepository = repRepository,
              ImagePath = Server.MapPath(@"\Content\images\pdf\")
            };
            return File(r.GetReport(), "application/pdf", "d12.pdf");
          }
        }
        catch (Exception ex)
        {
          ModelState.AddModelError("_FORM", ex.Message);
          ViewBag.Title = "Отчет Уралсиб Российские финансы";
          return View("RepParam4", r12);
        }
      }
      else if (id == 13)
      {
        Rep4Param r13 = new Rep4Param();
        try
        {
          UpdateModel(r13);
          if (ModelState.IsValid)
          {
            RepSIT r = new RepSIT()
            {
              DateB = new DateTime(2008, 3, 2),
              DateE = r13.d,
              FinInstID = 6629,
              FundID = 39191,
              IndexID = 13,
              repRepository = repRepository,
              ImagePath = Server.MapPath(@"\Content\images\pdf\")
            };
            return File(r.GetReport(), "application/pdf", "d13.pdf");
          }
        }
        catch (Exception ex)
        {
          ModelState.AddModelError("_FORM", ex.Message);
          ViewBag.Title = "Отчет Связь и Информационные технологии";
          return View("RepParam4", r13);
        }
      }
      else if (id == 14)
      {
        var r14 = new Rep3Param();
        try
        {
          UpdateModel(r14);
          if (ModelState.IsValid)
          {
            RepAstr r = new RepAstr()
            {
              DateStart = new DateTime(2006, 5, 31),
              DateB = r14.DateB,
              DateE = r14.DateE,
              FinInstID = 3396,
              FundID = 39192,
              IndexID = 80,
              repRepository = repRepository,
              ImagePath = Server.MapPath(@"\Content\images\pdf\")
            };
            return File(r.GetReport(), "application/pdf", "d14.pdf");
          }
        }
        catch
        {
          ViewBag.Title = "Отчет Fixed Income";
          return View("RepParam4", r14);
        }
      }
      else if (id == 15)
      {
        Rep4Param r15 = new Rep4Param();
        try
        {
          UpdateModel(r15);
          if (ModelState.IsValid)
          {
            var r = new RepRSMC()
            {
              DateB = new DateTime(2006, 5, 1),
              DateE = r15.d,
              FinInstID = 3647,
              FundID = 39192,
              IndexID = 2,
              repRepository = repRepository,
              ImagePath = Server.MapPath(@"\Content\images\pdf\")
            };
            return File(r.GetReport(), "application/pdf", "d15.pdf");
          }
        }
        catch
        {
          ViewBag.Title = "Отчет РСМС";
          return View("RepParam4", r15);
        }
      }
      else if (id == 16)
      {
        var r16 = new Rep3Param();
        try
        {
          UpdateModel(r16);
          if (ModelState.IsValid)
          {
            var r = new RepER()
            {
              DateStart = new DateTime(2006, 5, 31),
              DateB = r16.DateB,
              DateE = r16.DateE,
              FinInstID = 3394,
              FundID = 39192,
              IndexID = 166,
              repRepository = repRepository,
              ImagePath = Server.MapPath(@"\Content\images\pdf\")
            };
            return File(r.GetReport(), "application/pdf", "d16.pdf");
          }
        }
        catch
        {
          ViewBag.Title = "Отчет Extra Return";
          return View("RepParam4", r16);
        }
      }
      else if (id == 17)
      {
        var r17 = new RepParam17();
        try
        {
          UpdateModel(r17);
          if (ModelState.IsValid)
          {
            var q = repRepository.GetPiferData(r17.cid, r17.d1, r17.d2);
            //FileStream fs = new FileStream(Server.MapPath(@"\Templates\Rep5.xls"), FileMode.Open, FileAccess.Read);
            HSSFWorkbook workbook = new HSSFWorkbook();  //new HSSFWorkbook(fs, true);
            foreach (var qr in q)
            {
              HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet(qr.PIFName);
              sheet.DisplayGridlines = false;

              IRow row;
              ICell cell;
              HSSFCellStyle cellStyleNum = (HSSFCellStyle)workbook.CreateCellStyle();
              cellStyleNum.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0.00");
              HSSFCellStyle cellStylePerc = (HSSFCellStyle)workbook.CreateCellStyle();
              cellStylePerc.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00%");
              cellStylePerc.BorderBottom = BorderStyle.Thin;
              cellStylePerc.BorderTop = BorderStyle.Thin;
              cellStylePerc.BorderLeft = BorderStyle.Thin;
              cellStylePerc.BorderRight = BorderStyle.Thin;
              cellStylePerc.BottomBorderColor = HSSFColor.Black.Index;
              cellStylePerc.TopBorderColor = HSSFColor.Black.Index;
              cellStylePerc.LeftBorderColor = HSSFColor.Black.Index;
              cellStylePerc.RightBorderColor = HSSFColor.Black.Index;

              HSSFCellStyle cellStyle = (HSSFCellStyle)workbook.CreateCellStyle();
              cellStyle.BorderBottom = BorderStyle.Thin;
              cellStyle.BorderTop = BorderStyle.Thin;
              cellStyle.BorderLeft = BorderStyle.Thin;
              cellStyle.BorderRight = BorderStyle.Thin;
              cellStyle.BottomBorderColor = HSSFColor.Black.Index;
              cellStyle.TopBorderColor = HSSFColor.Black.Index;
              cellStyle.LeftBorderColor = HSSFColor.Black.Index;
              cellStyle.RightBorderColor = HSSFColor.Black.Index;
              cellStyle.WrapText = true;

              HSSFCellStyle cellStyleC = (HSSFCellStyle)workbook.CreateCellStyle();
              IFont font = workbook.CreateFont();
              font.Boldweight = 1600;
              font.Color = HSSFColor.Teal.Index;
              cellStyleC.SetFont(font);

              HSSFCellStyle cellStyle3 = (HSSFCellStyle)workbook.CreateCellStyle();
              font = workbook.CreateFont();
              font.Color = HSSFColor.Teal.Index;
              cellStyle3.Alignment = HorizontalAlignment.Justify;
              cellStyle3.WrapText = true;
              cellStyle3.SetFont(font);

              sheet.CreateRow(6).ZeroHeight = true;
              sheet.CreateRow(7).ZeroHeight = true;
              sheet.CreateRow(8).ZeroHeight = true;
              sheet.CreateRow(9).ZeroHeight = true;
              sheet.CreateRow(10).ZeroHeight = true;

              cell = sheet.CreateRow(12).CreateCell(0);
              cell.SetCellValue(string.Format("Клиент: {0}", qr.name));
              cell.CellStyle = cellStyleC;

              sheet.CreateRow(13).CreateCell(0).SetCellValue(string.Format("Период: {0:dd.MM.yyyy}-{1:dd.MM.yyyy}", r17.d1.Value, r17.d2.Value));

              row = sheet.CreateRow(16);
              row.CreateCell(0).SetCellValue("Количество паев на начало периода, шт");
              cell = row.CreateCell(6);
              cell.SetCellValue((double)(qr.RestB ?? 0));
              cell.CellStyle = cellStyleNum;

              row = sheet.CreateRow(17);
              row.CreateCell(0).SetCellValue("Цена одного пая на начало периода, руб");
              cell = row.CreateCell(6);
              cell.SetCellValue(qr.RateB);
              cell.CellStyle = cellStyleNum;

              row = sheet.CreateRow(18);
              row.CreateCell(0).SetCellValue("Стоимость портфеля на начало периода, руб");
              cell = row.CreateCell(6);
              cell.SetCellValue((double)(qr.RestB ?? 0) * qr.RateB ?? 0);
              cell.CellStyle = cellStyleNum;

              row = sheet.CreateRow(19);
              row.CreateCell(0).SetCellValue("Количество паев на конец периода, шт");
              cell = row.CreateCell(6);
              cell.SetCellValue((double)(qr.RestE ?? 0));
              cell.CellStyle = cellStyleNum;

              row = sheet.CreateRow(20);
              row.CreateCell(0).SetCellValue("Цена одного пая на конец периода, руб");
              cell = row.CreateCell(6);
              cell.SetCellValue(qr.RateE ?? 0);
              cell.CellStyle = cellStyleNum;

              row = sheet.CreateRow(21);
              row.CreateCell(0).SetCellValue("Стоимость портфеля на конец периода, руб");
              cell = row.CreateCell(6);
              cell.SetCellValue((double)(qr.RestE ?? 0) * qr.RateE ?? 0);
              cell.CellStyle = cellStyleNum;

              row = sheet.CreateRow(22);
              row.CreateCell(0).SetCellValue("Доход/убыток за период (в том числе нереализованный), руб");
              cell = row.CreateCell(6);
              cell.SetCellValue((double)(qr.RestE ?? 0) * (qr.RateE ?? 0) - (double)(qr.RestB ?? 0) * (qr.RateB ?? 0) - (qr.Turn ?? 0));
              cell.CellStyle = cellStyleNum;

              sheet.AutoSizeColumn(6);

              row = sheet.CreateRow(15);
              row.CreateCell(0).SetCellValue("Наименование ПИФа");
              row.CreateCell(6).SetCellValue(qr.PIFName);

              row = sheet.CreateRow(25);
              row.CreateCell(0).SetCellValue(string.Format("Доходность с {0:dd.MM.yyyy} по {1:dd.MM.yyyy}", r17.d1.Value, r17.d2.Value));

              row = sheet.CreateRow(26);
              cell = row.CreateCell(0);
              cell.SetCellValue("Неделя");
              cell.CellStyle = cellStyle;

              cell = row.CreateCell(1);
              cell.SetCellValue("Месяц");
              cell.CellStyle = cellStyle;

              cell = row.CreateCell(2);
              cell.SetCellValue("Квартал");
              cell.CellStyle = cellStyle;

              cell = row.CreateCell(3);
              cell.SetCellValue("Год");
              cell.CellStyle = cellStyle;

              cell = row.CreateCell(4);
              cell.SetCellValue("3 года");
              cell.CellStyle = cellStyle;

              cell = row.CreateCell(5);
              cell.SetCellValue("С начала года");
              cell.CellStyle = cellStyle;

              cell = row.CreateCell(6);
              cell.SetCellValue("С начала операций");
              cell.CellStyle = cellStyle;

              row = sheet.CreateRow(27);
              cell = row.CreateCell(0);
              cell.SetCellValue((qr.RateE / qr.rtw - 1) ?? 0);
              cell.CellStyle = cellStylePerc;
              cell = row.CreateCell(1);
              cell.SetCellValue((qr.RateE / qr.rtm - 1) ?? 0);
              cell.CellStyle = cellStylePerc;
              cell = row.CreateCell(2);
              cell.SetCellValue((qr.RateE / qr.rtq - 1) ?? 0);
              cell.CellStyle = cellStylePerc;
              cell = row.CreateCell(3);
              cell.SetCellValue((qr.RateE / qr.rty - 1) ?? 0);
              cell.CellStyle = cellStylePerc;
              cell = row.CreateCell(4);
              cell.SetCellValue((qr.RateE / qr.rt3y - 1) ?? 0);
              cell.CellStyle = cellStylePerc;
              cell = row.CreateCell(5);
              cell.SetCellValue((qr.RateE / qr.rtyb - 1) ?? 0);
              cell.CellStyle = cellStylePerc;
              cell = row.CreateCell(6);
              cell.SetCellValue((qr.RateE / qr.rtf - 1) ?? 0);
              cell.CellStyle = cellStylePerc;

              row = sheet.CreateRow(51);
              cell = row.CreateCell(0);
              cell.CellStyle = cellStyleC;
              cell.SetCellValue("Ограничение ответственности");
              row = sheet.CreateRow(52);
              row.HeightInPoints = 78;
              cell = row.CreateCell(0);
              cell.SetCellValue("Информация содержащаяся в настоящих материалах, подготовленных ООО «УРАЛСИБ Стиль жизни», не является оценкой активов, выпиской из реестра владельцев паев, не является подтверждением прав/перехода прав собственности на ценные бумаги. Таким образом, в соответствии с действующим законодательством, указанная информация не может быть использована для целей бухгалтерского/налогового учета и/или составления бухгалиерской/налоговой отчетности и имеет исключительно информационное назначение");
              cell.CellStyle = cellStyle3;
              sheet.AddMergedRegion(new CellRangeAddress(52, 52, 0, 9));

              Chart chart = new Chart()
              {
                Width = new System.Web.UI.WebControls.Unit(700, System.Web.UI.WebControls.UnitType.Pixel),
                Palette = ChartColorPalette.EarthTones,
                AntiAliasing = AntiAliasingStyles.All,
                TextAntiAliasingQuality = TextAntiAliasingQuality.SystemDefault,
              };
              chart.Legends.Add(new Legend { BorderWidth = 0, Alignment = System.Drawing.StringAlignment.Near, Docking = Docking.Bottom });
              //chart.Titles.Add(new Title { Text = "Доходность фонда, %", Font = new System.Drawing.Font("Trebuchet MS", 10, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point) });
              chart.ChartAreas.Add(new ChartArea
              {
                AxisY = new Axis
                {
                  LabelAutoFitMaxFontSize = 8,
                  IntervalAutoMode = IntervalAutoMode.VariableCount,
                  IsLabelAutoFit = true,
                  MajorGrid = new Grid
                  {
                    IntervalType = DateTimeIntervalType.Number,
                    LineDashStyle = ChartDashStyle.Dot,
                    LineColor = System.Drawing.Color.LightGray
                  },
                  LabelStyle = new LabelStyle { Format = "N1" }
                },
                AxisY2 = new Axis
                {
                  Enabled = AxisEnabled.True,
                  LabelAutoFitMaxFontSize = 8,
                  IntervalAutoMode = IntervalAutoMode.VariableCount,
                  IsLabelAutoFit = true,
                  MajorGrid = new Grid
                  {
                    Enabled = false
                  },
                  LabelStyle = new LabelStyle { Format = "N1" }
                },
                AxisX = new Axis
                {
                  IsStartedFromZero = true,
                  LabelAutoFitMaxFontSize = 8,
                  LabelAutoFitStyle = LabelAutoFitStyles.LabelsAngleStep90,
                  IntervalAutoMode = IntervalAutoMode.VariableCount,
                  IntervalType = DateTimeIntervalType.Days,
                  IsLabelAutoFit = true,
                  IsMarginVisible = false,
                  MajorGrid = new Grid
                  {
                    LineDashStyle = ChartDashStyle.Dot,
                    //IntervalType = DateTimeIntervalType.Number,
                    LineColor = System.Drawing.Color.LightGray
                  },
                  LabelStyle = new LabelStyle
                  {
                    Format = "dd.MM.yy",
                    IsStaggered = true,
                    Angle = -90
                  }
                }
              });
              chart.Series.Add(new Series
              {
                Name = qr.PIFName,
                ChartType = SeriesChartType.FastLine,
                Color = System.Drawing.Color.BlueViolet,
                XValueMember = "Date2",
                YValueMembers = "coef_all"
              });
              var r = repRepository.GetRepClient22(qr.DateFirst, r17.d2, qr.id, 39191, 0);
              chart.Series[0].Points.DataBind(r, "Date2", "coef_all", null);
              chart.Series.Add(new Series
              {
                Name = qr.IndexName,
                ChartType = SeriesChartType.FastLine,
                Color = System.Drawing.Color.Red,
                XValueMember = "Date2",
                YValueMembers = "coef_allb"
              });
              chart.Series[1].Points.DataBind(repRepository.GetRepClient24(qr.DateFirst, r17.d2, 0, 39191, qr.iid), "Date2", "coef_all", null);
              var imgStream = new MemoryStream();
              chart.SaveImage(imgStream, ChartImageFormat.Png);
              imgStream.Seek(0, SeekOrigin.Begin);

              HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
              {
                HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, 0, 31, 14, 60);
                anchor.AnchorType = 2;
                int pictIdx = workbook.AddPicture(imgStream.ToArray(), PictureType.PNG);
                HSSFPicture picture = (HSSFPicture)patriarch.CreatePicture(anchor, pictIdx);
                picture.Resize();
              }
              {
                HSSFClientAnchor anchor = new HSSFClientAnchor();
                anchor.AnchorType = 2;
                FileStream file = new FileStream(Server.MapPath(@"\Content\images\sl.jpg"), FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[file.Length];
                file.Read(buffer, 0, (int)file.Length);

                int pictIdx = workbook.AddPicture(buffer, PictureType.JPEG);
                HSSFPicture picture = (HSSFPicture)patriarch.CreatePicture(anchor, pictIdx);
                picture.Resize();
              }
            }
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            return File(ms.ToArray(), "application/vnd.ms-excel", string.Format("oc{0}.xls", DateTime.Now.Second));
          }
        }
        catch (Exception ex)
        {
          ModelState.AddModelError("", ex.Message);
          ViewBag.Title = "Отчет по пайщику";
          return View("RepParam5", r17);
        }
      }
      return View("NotFound");
    }

    public ActionResult GetSecChart()
    {
      var q = repRepository.GetSecChart();
      return Json(new { data = q }, JsonRequestBehavior.AllowGet);
    }

    [Authorize(Roles = "mo")]
    public ActionResult GetFinRep()
    {
      return View();
    }

    [Authorize(Roles = "mo")]
    public ActionResult GetFinRepForm()
    {
      return Json(new { data = repRepository.GetFinRep() });
    }

    [Authorize(Roles = "mo")]
    public ActionResult GetFinRepData(int id)
    {
      return Json(new { success = true, data = repRepository.GetFinRepData(id) });
    }

    [Authorize(Roles = "mo")]
    public ActionResult GetObjClsByParent(int id)
    {
      return Json(new { data = repRepository.GetObjClsByParent(id) });
    }

    [Authorize(Roles = "mo")]
    public ActionResult GetTreaties(string query, int? start, int? limit)
    {
      return Json(new { data = repRepository.GetTreaties(query ?? "", limit ?? 10) }, JsonRequestBehavior.AllowGet);
    }

    [Authorize(Roles = "mo")]
    public ActionResult SaveFinRep(int id, List<SaveFinRep> data)
    {
      try
      {
        foreach (var r in data)
        {
          repRepository.SetFinRepData(id, r.ocp, r.oc);
        }
      }
      catch
      {
        return Json(new { success = false });
      }
      return Json(new { success = true });
    }

    [Authorize(Roles = "mo")]
    public ActionResult GetPlatList()
    {
      var q = repRepository.GetPlatList();
      return Json(new { data = q, totalCount = q.Count() });
    }

    [Authorize(Roles = "mo")]
    public ActionResult GetExecutor()
    {
      var q = repRepository.GetObjClsByParentID(1004);
      return Json(new { data = q, totalCount = q.Count() });
    }

    [Authorize(Roles = "mo")]
    public ActionResult GetDepart()
    {
      var q = repRepository.GetObjClsByParentID(1005);
      return Json(new { data = q, totalCount = q.Count() });
    }

    [Authorize(Roles = "mo")]
    public ActionResult GetPlatType()
    {
      var q = repRepository.GetObjClsByParentID(15185);
      return Json(new { data = q, totalCount = q.Count() });
    }

    [Authorize(Roles = "mo")]
    public ActionResult GetSign1()
    {
      var q = repRepository.GetObjClsByParentID(47920);
      return Json(new { data = q });
    }

    [Authorize(Roles = "mo")]
    public ActionResult FileImport(HttpPostedFileBase file, int? mois, int? an)
    {
      if (file != null && file.ContentLength > 0 && mois.HasValue && an.HasValue)
      {
        if (repRepository.ImpCharges(file, mois, an))
          return new JsonnResult { Data = new { success = true, message = "Сохранено", file = file.FileName }, ContentType = "text/html" };
        else
          return new JsonnResult { Data = new { success = false, message = "Ошибка при импорте файла" }, ContentType = "text/html" };
      }
      return new JsonnResult { Data = new { success = false, message = "Нет файла" }, ContentType = "text/html" };
    }

    [Authorize(Roles = "mo")]
    public ActionResult OrderPayment()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);
      return View();
    }

    [Authorize(Roles = "mo")]
    public ActionResult OrderPaymentList(string sort, string dir, DateTime? db, DateTime? de, int? DateType, Boolean? NotPayed, Boolean? Reserved, int? TypeID, string filter)
    {
      var q = repRepository.GetOrderPayment(sort, dir, db, de, DateType, NotPayed, Reserved, TypeID, filter);
      return new JsonnResult { Data = new { data = q } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult OrderPaymentCreate(List<tOrdPayment1> data, int? CopyID)
    {
      var q = repRepository.OrderPaymentCreate(data);
      return new JsonnResult { Data = new { success = true, message = "Сохранено", data = q } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult OrderPaymentUpdate(List<tOrdPayment> data)
    {
      var q = repRepository.OrderPaymentUpdate(data, User.IsInRole("ordpayma"));
      return new JsonnResult { Data = new { success = true, message = "Сохранено", data = q } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult OrderPaymentDel(List<tOrdPayment> data)
    {
      if (repRepository.OrderPaymentDel(data))
        return Json(new { success = true });
      else
        return Json(new { success = false, msg = "Ошибка при удалении" });
    }

    [Authorize(Roles = "mo")]
    public ActionResult GetPaymFunds()
    {
      return Json(new { data = repRepository.GetPaymFunds() });
    }

    [Authorize(Roles = "mo")]
    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult OrdPaymExcel(DateTime? db, DateTime? de, int? DateType)
    {
      try
      {
        FileStream fs = new FileStream(Server.MapPath(@"\Templates\OrdPaymList.xls"), FileMode.Open, FileAccess.Read);
        HSSFWorkbook workbook = new HSSFWorkbook(fs, true);
        ISheet sheet = workbook.GetSheet("Лист1");
        var q = repRepository.GetOrderPayment(db, de, DateType);

        int ir = 1;
        IRow row = sheet.GetRow(0);
        row.GetCell(0).SetCellValue(db.Value);
        row.GetCell(1).SetCellValue(de.Value);
        ir = 2;
        foreach (var r in q)
        {
          row = sheet.CreateRow(ir++);
          int c = 0;
          row.CreateCell(c++).SetCellValue(r.Number ?? "");
          row.CreateCell(c++).SetCellValue(r.NameBrief ?? "");
          row.CreateCell(c++).SetCellValue(r.Receiver ?? "");
          row.CreateCell(c++).SetCellValue(r.Item ?? "");
          row.CreateCell(c++).SetCellValue(r.ItemName ?? "");
          row.CreateCell(c++).SetCellValue(r.ItemParent ?? "");
          row.CreateCell(c++).SetCellValue(r.PFP ?? "");
          row.CreateCell(c++).SetCellValue(r.Comment ?? "");
          row.CreateCell(c++).SetCellValue((double)(r.Qty ?? 0));
          row.CreateCell(c++).SetCellValue((double)(r.VAT ?? 0));
          row.CreateCell(c++).SetCellValue(r.FundName ?? "");
          row.CreateCell(c++).SetCellValue((double)(r.QtyR ?? 0));
          row.CreateCell(c++).SetCellValue((double)(r.VATR ?? 0));
          row.CreateCell(c++).SetCellValue(r.DateCreate);
          row.CreateCell(c++).SetCellValue(r.DatePay == null ? "" : r.DatePay.Date.ToShortDateString());
          row.CreateCell(c++).SetCellValue(r.ExecutorName ?? "");
          row.CreateCell(c++).SetCellValue(r.PlatNumb ?? "");
          row.CreateCell(c++).SetCellValue(r.IsBudgetS ?? "Нет");
          row.CreateCell(c++).SetCellValue(r.DocTypeName ?? "");
          row.CreateCell(c++).SetCellValue(r.DateDoc == null ? "" : r.DateDoc.Date.ToShortDateString());
          row.CreateCell(c++).SetCellValue(r.DocNumb ?? "");
          row.CreateCell(c++).SetCellValue(r.Dogovor ?? "");
          row.CreateCell(c++).SetCellValue(r.IsPlanS ?? "Нет");
          row.CreateCell(c++).SetCellValue(r.DateReg == null ? "" : r.DateReg.Date.ToShortDateString());
          row.CreateCell(c++).SetCellValue(r.TypeName ?? "");
          row.CreateCell(c++).SetCellValue(r.PeriodicityName ?? "");
          row.CreateCell(c++).SetCellValue(r.State ?? "");
          row.CreateCell(c++).SetCellValue(r.IsReserveS ?? "Нет");
        }
        MemoryStream ms = new MemoryStream();
        workbook.Write(ms);
        return File(ms.ToArray(), "application/vnd.ms-excel", "oc.xls");
      }
      catch
      {
      }
      return View("Error");
    }

    [Authorize(Roles = "mo")]
    public ActionResult ordpaymCourriel(int id)
    {
      if ((repRepository.GetOrderPaym(id).StateID ?? 0) == 0)
      {
        repRepository.ordpaymCourriel(id, null, (HttpContext.Request).Url.Authority, "");
        if (repRepository.confirmOrdPaym(id, User.Identity.Name, action: 1, StateIdCur: 0, descr: "", DatePay: null))
        {
          var res = repRepository.ordpaymCourriel(id, Url.RouteUrl("Default", new { action = "opcs", id = id, s = repRepository.getOrdPaymStateID(id) }, Request.Url.Scheme), (HttpContext.Request).Url.Authority, "");
          return new JsonnResult { Data = new { success = res } };
        }
      }
      return new JsonnResult { Data = new { success = false } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult getOrdPaymLogList(int id)
    {
      return new JsonnResult { Data = new { success = true, data = repRepository.getOrdPaymLogList(id) } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult ordpaymConfirm(int id)
    {
      return Redirect(Url.RouteUrl("Default", new { action = "opcs", id = id, s = repRepository.getOrdPaymStateID(id) }, Request.Url.Scheme));
    }

    [Authorize(Roles = "mo")]
    public ActionResult ordpaymRefuse(int id)
    {
      return Redirect(Url.RouteUrl("Default", new { action = "opcs", id = id, s = repRepository.getOrdPaymStateID(id), a = -1 }, Request.Url.Scheme));
    }

    [Authorize(Roles = "mo, jrpc")]
    public ActionResult opcs(int id, int s = 0, int a = 1)
    {
      var q1 = repRepository.GetOrderPaym(id);
      var NextStateID = repRepository.NextStateID(id, a, q1.StateID ?? 0);
      if (a == 1 && User.IsInRole("rcenter") && (q1.PPTypeID ?? 0) == 0 && NextStateID == 4)
      {
        if (repRepository.confirmOrdPaym(id, User.Identity.Name, action: 1, StateIdCur: s, descr: "", DatePay: DateTime.Today))
        {
          var res = repRepository.ordpaymCourriel(id, Url.RouteUrl("Default", new { action = "opcs", id = id, s = repRepository.getOrdPaymStateID(id) }, Request.Url.Scheme), (HttpContext.Request).Url.Authority, descr: "", refuse: a != 1);
          return View("OrdPaymConfirm", new { action = 1 }.ToExpando());
        }
      }
      var q2 = repRepository.GetOrderPaymDetF(id);
      var ql = repRepository.getOrdPaymLogList(id);
      return View("OrdPaymChgState", new { id = id, s = s, q1 = q1, q2 = q2, ql = ql, NextStateID = NextStateID }.ToExpando());
    }

    [Authorize(Roles = "mo, jrpc")]
    [HttpPost]
    public ActionResult opcs(int id, int s, string descr, string refuse, string confirm, DateTime? DatePay)
    {
      var NextStateID = repRepository.NextStateID(id, refuse == null ? 1 : -1, repRepository.getOrdPaymStateID(id) ?? 0);
      if (NextStateID != 4 || User.IsInRole("rcenter"))
      {
        if (repRepository.confirmOrdPaym(id, User.Identity.Name, action: refuse == null ? 1 : -1, StateIdCur: s, descr: descr, DatePay: DatePay))
        {
          var res = repRepository.ordpaymCourriel(id, Url.RouteUrl("Default", new { action = "opcs", id = id, s = repRepository.getOrdPaymStateID(id) }, Request.Url.Scheme), (HttpContext.Request).Url.Authority, descr, refuse != null);
          return View("OrdPaymConfirm", new { action = refuse == null ? 1 : -1 }.ToExpando());
        }
      }
      ModelState.AddModelError(string.Empty, "Неверное состояние документа.");
      var q1 = repRepository.GetOrderPaym(id);
      var q2 = repRepository.GetOrderPaymDetF(id);
      var ql = repRepository.getOrdPaymLogList(id);
      return View("OrdPaymChgState", new { id = id, s = s, q1 = q1, q2 = q2, ql = ql, NextStateID = NextStateID }.ToExpando());
    }

    [Authorize(Roles = "mo, jrpc")]
    public ActionResult ordPaymRappel()
    {
      var success = repRepository.ordPaymRappel((HttpContext.Request).Url.Authority, Url);
      return new JsonnResult { Data = new { success = success } };
    }

    [Authorize(Roles = "admin")]
    public ActionResult ordPaymRappel2()
    {
      var success = repRepository.ordPaymRappel2((HttpContext.Request).Url.Authority, Url);
      return new JsonnResult { Data = new { success = success } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult GetPoluch1(string query)
    {
      return Json(new { data = repRepository.GetPoluch1(query) });
    }

    [Authorize(Roles = "mo")]
    public ActionResult GetPoluch2(int? id, string query, int? start, int? limit)
    {
      if (id.HasValue)
        return Json(new { data = repRepository.GetContragent(id.Value) }, JsonRequestBehavior.AllowGet);
      else
        return Json(new { data = repRepository.GetContragents(query ?? "", limit ?? 10) }, JsonRequestBehavior.AllowGet);
    }

    [Authorize(Roles = "mo")]
    public ActionResult GetSign(string query)
    {
      return Json(new { data = repRepository.GetSign(query) });
    }

    [Authorize(Roles = "mo")]
    public ActionResult GetDocType()
    {
      var q = repRepository.GetObjClsByParentID(14244);
      return Json(new { data = q, totalCount = q.Count() });
    }

    [Authorize(Roles = "mo")]
    public ActionResult GetPeriodicity()
    {
      var q = repRepository.GetObjClsByParentID(47185);
      return Json(new { data = q, totalCount = q.Count() });
    }

    [Authorize(Roles = "mo")]
    public ActionResult GetTR()
    {
      var q = repRepository.GetObjClsByParentID(58880);
      return Json(new { data = q, totalCount = q.Count() });
    }

    [Authorize(Roles = "mo")]
    public ActionResult GetDogovor(int id)
    {
      var q = repRepository.GetDogovor(id);
      return Json(new { data = q, totalCount = q.Count() });
    }

    [Authorize(Roles = "mo")]
    public ActionResult OrderPaymDetList(int? id)
    {
      return new JsonnResult { Data = new { success = true, data = repRepository.GetOrderPaymDet(id) } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult OrderPaymDetCreate(List<tOrdPaymentDet> data)
    {
      var q = repRepository.OrderPaymDetCreate(data);
      return new JsonnResult { Data = new { success = true, message = "Сохранено", data = q } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult OrderPaymDetUpdate(List<tOrdPaymentDet> data)
    {
      var q = repRepository.OrderPaymDetUpdate(data, User.IsInRole("ordpayma"));
      return new JsonnResult { Data = new { success = true, message = "Сохранено", data = q } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult OrderPaymDetDel(List<tOrdPaymentDet> data)
    {
      if (repRepository.OrderPaymDetDel(data))
        return Json(new { success = true });
      else
        return Json(new { success = false, msg = "Ошибка при удалении" });
    }

    [Authorize(Roles = "mo")]
    public ActionResult ContragentList(string sort, string dir, bool? isNotArch, string name)
    {
      return new JsonnResult { Data = new { success = true, data = repRepository.GetContragentList(sort, dir, isNotArch, name) } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult GetCountries()
    {
      return Json(new { data = repRepository.GetCountries() });
    }

    [Authorize(Roles = "mo")]
    public ActionResult GetContrType()
    {
      var q = repRepository.GetObjClsByParentID(49404);
      return Json(new { data = q, totalCount = q.Count() });
    }

    [Authorize(Roles = "mo")]
    public ActionResult AddContragent(string name)
    {
      var q = repRepository.AddContragent(name);
      return new JsonnResult { Data = new { success = true, message = "Сохранено", data = q } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult ContragentCreate(List<tContragent> data)
    {
      var q = repRepository.ContragentCreate(data);
      return new JsonnResult { Data = new { success = true, message = "Сохранено", data = q } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult ContragentUpdate(List<tContragent> data)
    {
      var q = repRepository.ContragentUpdate(data);
      return new JsonnResult { Data = new { success = true, message = "Сохранено", data = q } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult ContragentDel(List<tContragent> data)
    {
      if (repRepository.ContragentDel(data))
        return Json(new { success = true });
      else
        return Json(new { success = false, msg = "Ошибка при удалении" });
    }

    [Authorize(Roles = "mo")]
    public ActionResult ContragentDogList(int id)
    {
      return new JsonnResult { Data = new { success = true, data = repRepository.GetContragentDogList(id) } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult ContragentDogCreate(List<tContragentDog> data)
    {
      var q = repRepository.ContragentDogCreate(data);
      return new JsonnResult { Data = new { success = true, message = "Сохранено", data = q } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult ContragentDogUpdate(List<tContragentDog> data)
    {
      var q = repRepository.ContragentDogUpdate(data);
      return new JsonnResult { Data = new { success = true, message = "Сохранено", data = q } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult ContragentDogDel(List<tContragentDog> data)
    {
      if (repRepository.ContragentDogDel(data))
        return Json(new { success = true });
      else
        return Json(new { success = false, msg = "Ошибка при удалении" });
    }

    [Authorize(Roles = "mo")]
    public ActionResult OrdPaymForm(int? id)
    {
      try
      {
        FileStream fs = new FileStream(Server.MapPath(@"\Templates\OrdPaym.xls"), FileMode.Open, FileAccess.Read);
        HSSFWorkbook workbook = new HSSFWorkbook(fs, true);
        ISheet sheet = workbook.GetSheet("Лист1");
        HSSFCellStyle cellStyle = (HSSFCellStyle)workbook.CreateCellStyle();
        cellStyle.BorderBottom = BorderStyle.Thin;
        cellStyle.BorderTop = BorderStyle.Thin;
        cellStyle.BorderLeft = BorderStyle.Thin;
        cellStyle.BorderRight = BorderStyle.Thin;
        cellStyle.BottomBorderColor = HSSFColor.Black.Index;
        cellStyle.TopBorderColor = HSSFColor.Black.Index;
        cellStyle.LeftBorderColor = HSSFColor.Black.Index;
        cellStyle.RightBorderColor = HSSFColor.Black.Index;
        cellStyle.WrapText = true;
        HSSFCellStyle cellStyleN = (HSSFCellStyle)workbook.CreateCellStyle();
        cellStyleN.BorderBottom = BorderStyle.Thin;
        cellStyleN.BorderTop = BorderStyle.Thin;
        cellStyleN.BorderLeft = BorderStyle.Thin;
        cellStyleN.BorderRight = BorderStyle.Thin;
        cellStyleN.BottomBorderColor = HSSFColor.Black.Index;
        cellStyleN.TopBorderColor = HSSFColor.Black.Index;
        cellStyleN.LeftBorderColor = HSSFColor.Black.Index;
        cellStyleN.RightBorderColor = HSSFColor.Black.Index;
        cellStyleN.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0.00");
        //IFont defaultFont = workbook.GetFontAt(0);
        //defaultFont.FontHeightInPoints = 14;

        var q = repRepository.GetOrderPaym(id);

        int ir = 1;
        ICell cell;
        IRow row = sheet.GetRow(0);
        row.GetCell(3).SetCellValue(string.IsNullOrWhiteSpace(q.Number) ? q.id.ToString() : q.Number);
        row = sheet.GetRow(1);
        row.GetCell(4).SetCellValue(q.IsBudgetS ?? "");
        row = sheet.GetRow(2);
        row.GetCell(3).SetCellValue(q.NameBrief ?? "");
        row = sheet.GetRow(4);
        row.GetCell(3).SetCellValue(q.DocTypeName ?? "");
        row.GetCell(4).SetCellValue(q.DateDoc == null ? "" : q.DateDoc.Date.ToShortDateString());
        row.GetCell(5).SetCellValue(q.DocNumb ?? "");
        row = sheet.GetRow(5);
        row.GetCell(3).SetCellValue(q.Receiver ?? "");
        row = sheet.GetRow(6);
        row.GetCell(3).SetCellValue(q.Dogovor ?? "");
        row = sheet.GetRow(8);
        row.GetCell(3).SetCellValue(q.FundName ?? "");
        row = sheet.GetRow(10);
        row.GetCell(3).SetCellValue(q.ExecutorName ?? "");
        row.GetCell(5).SetCellValue(q.DateCreate == null ? "" : q.DateCreate.Date.ToShortDateString());
        ir = 14;
        var qd = repRepository.GetOrderPaymDetF(id);
        decimal? Qty = 0m;
        foreach (var r in qd)
        {
          row = sheet.CreateRow(ir++);
          sheet.ShiftRows(ir + 1, sheet.LastRowNum /*ir + 10*/, 1);
          int c = 0;
          cell = row.CreateCell(c++);
          cell.CellStyle = cellStyle;
          cell.SetCellValue(ir - 14);

          cell = row.CreateCell(c++);
          cell.CellStyle = cellStyle;
          cell.SetCellValue(r.Item ?? "");

          cell = row.CreateCell(c++);
          cell.CellStyle = cellStyle;
          cell.SetCellValue(r.PFP ?? "");

          cell = row.CreateCell(c++);
          cell.CellStyle = cellStyleN;
          if (q.FundID == 39191)
            cell.SetCellValue((double)(r.Qty ?? 0));
          else
            cell.SetCellValue((double)(r.QtyR ?? 0));

          //cell = row.CreateCell(c++);
          //cell.CellStyle = cellStyleN;
          //cell.SetCellValue((double)(r.Qty ?? 0));

          cell = row.CreateCell(c++, CellType.String);
          cell.CellStyle = cellStyle;
          cell.SetCellValue(r.Comment ?? "");

          cell = row.CreateCell(c++, CellType.String);
          cell.CellStyle = cellStyle;

          sheet.AddMergedRegion(new CellRangeAddress(ir - 1, ir - 1, c - 2, c - 1));

          Qty += (q.FundID == 39191) ? r.Qty : r.QtyR;
        }
        row = sheet.GetRow(8);
        row.GetCell(4).SetCellValue((double)(Qty ?? 0));

        row = sheet.GetRow(sheet.LastRowNum - 3);
        row.Height = 600;
        row.GetCell(0).SetCellValue(q.SignFIO1 ?? "");
        row.GetCell(1).SetCellValue(q.SignPost1 ?? "");
        sheet.GetRow(sheet.LastRowNum - 2).Height = 600;
        sheet.GetRow(sheet.LastRowNum - 1).Height = 600;

        workbook.SetPrintArea(0, 0, 5, 0, sheet.LastRowNum + 1 /*- 8*/);

        MemoryStream ms = new MemoryStream();
        workbook.Write(ms);
        return File(ms.ToArray(), "application/vnd.ms-excel", string.Format("pf{0:ss}.xls", DateTime.Now));
      }
      catch (Exception ex)
      {
      }
      return View("Error");
    }

    [Authorize(Roles = "mo")]
    public ActionResult CheckOrdPaymByPlatNumb(string Receiver, string DocNumb)
    {
      var dd = repRepository.CheckOrdPaymByPlatNumb(Receiver, DocNumb);
      return Json(new { exist = dd != null, data = dd != null ? (dd.DateDoc as DateTime?).HasValue ? (dd.DateDoc as DateTime?).Value.ToShortDateString() : "" : "" });
    }

    [Authorize(Roles = "mo")]
    public ActionResult CheckOrdPaymByPlatNumb1(int ReceiverID, string DocNumb)
    {
      var dd = repRepository.CheckOrdPaymByPlatNumb1(ReceiverID, DocNumb);
      return Json(new { exist = dd != null, data = dd != null ? (dd.DateDoc as DateTime?).HasValue ? (dd.DateDoc as DateTime?).Value.ToShortDateString() : "" : "" });
    }

    [Authorize(Roles = "mo")]
    public ActionResult GetOrdPaymData(int? id, string item)
    {
      var dd = repRepository.getOrdPaymData(id, item).FirstOrDefault();
      return Json(new { Qty = (dd.Qty ?? 0).ToString("N2"), QtyA = (dd.QtyA ?? 0).ToString("N2") });
    }

    [Authorize(Roles = "mo")]
    public ActionResult Charges()
    {
      return View();
    }

    [Authorize(Roles = "mo")]
    public ActionResult ChargesPFP()
    {
      return Json(new { data = repRepository.GetObjClsNode(1054) });
    }

    [Authorize(Roles = "mo")]
    public ActionResult ChargesList(string sort, string dir, int? y, int? m, int? pfp, bool? np, int? start, int? limit)
    {
      var q1 = repRepository.ChargesList(sort, dir, y, m, pfp, np);
      return Json(new { data = q1.Skip(start ?? 0).Take(limit ?? 500), totalCount = q1.Count() });
    }

    [Authorize(Roles = "mo")]
    public ActionResult ChargesUpdate(List<tCharge> data)
    {
      var q = repRepository.ChargesUpdate(data);
      return Json(new { success = true, message = "Сохранено", data = q });
    }

    [Authorize(Roles = "mo")]
    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult ChargesExcel(int? y, int? m, bool? np)
    {
      try
      {
        FileStream fs = new FileStream(Server.MapPath(@"\Templates\Charges.xls"), FileMode.Open, FileAccess.Read);
        HSSFWorkbook workbook = new HSSFWorkbook(fs, true);
        ISheet sheet = workbook.GetSheet("Лист1");
        var q1 = repRepository.ChargesList("Item3", "asc", y, m, null, np);

        int ir = 1;
        IRow row;
        foreach (var r in q1)
        {
          row = sheet.CreateRow(ir++);
          int c = 0;
          row.CreateCell(c++).SetCellValue(r.Item1 ?? "");
          row.CreateCell(c++).SetCellValue(r.Item2 ?? "");
          row.CreateCell(c++).SetCellValue(r.Item3 ?? "");
          row.CreateCell(c++).SetCellValue(r.Item4 ?? "");
          row.CreateCell(c++).SetCellValue(r.Item5 ?? "");
          row.CreateCell(c++).SetCellValue(r.Pfp ?? "");
          row.CreateCell(c++).SetCellValue(r.Quartier ?? 0);
          row.CreateCell(c++).SetCellValue(r.An ?? 0);
          if (r.QtyP != null) row.CreateCell(c).SetCellValue((double)(r.QtyP ?? 0));
          c++;
          if (r.QtyF != null) row.CreateCell(c).SetCellValue((double)(r.QtyF ?? 0));
          c++;
          if (r.QtyP != null || r.QtyF != null) row.CreateCell(c).SetCellValue((double)((r.QtyP ?? 0) - (r.QtyF ?? 0)));
        }
        MemoryStream ms = new MemoryStream();
        workbook.Write(ms);
        return File(ms.ToArray(), "application/vnd.ms-excel", "ch.xls");
      }
      catch (Exception /*ex*/)
      {
      }
      return View("Error");
    }

    [Authorize(Roles = "mo")]
    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult ImportOrdCtrl(int? y, int? m)
    {
      if (y.HasValue && m.HasValue)
        repRepository.ImportOrdControl(y.Value, m.Value);
      return Json(new { data = 1 });
    }

    [Authorize(Roles = "mo")]
    public ActionResult FinPlanSchema()
    {
      return View();
    }

    [Authorize(Roles = "mo")]
    public ActionResult FPSList(string sort, string dir, int? start, int? limit, string filter)
    {
      var q = repRepository.GetFPS(sort, dir, filter).ToList();
      return Json(new { data = q.Skip(start ?? 0).Take(limit ?? 500), totalCount = q.Count() });
    }

    [Authorize(Roles = "mo")]
    public ActionResult FPSCreate(List<tFinPlanSchema> data)
    {
      var q = repRepository.FPSCreate(data);
      return Json(new { success = true, message = "Сохранено", data = q });
    }

    [Authorize(Roles = "mo")]
    public ActionResult FPSUpdate(List<tFinPlanSchema> data)
    {
      var q = repRepository.FPSUpdate(data);
      return Json(new { success = true, message = "Сохранено", data = q });
    }

    [Authorize(Roles = "mo")]
    public ActionResult FPSDel(List<tFinPlanSchema> data)
    {
      if (repRepository.FPSDel(data))
        return Json(new { success = true });
      else
        return Json(new { success = false, msg = "Ошибка при удалении" });
    }

    [Authorize(Roles = "mo")]
    public ActionResult FinPlan()
    {
      return View();
    }

    [Authorize(Roles = "mo")]
    public ActionResult FinPlanList(string sort, string dir, int? y, int? q)
    {
      var q1 = repRepository.FinPlanList(sort, dir, y, q);
      return Json(new { data = q1 });
    }

    [Authorize(Roles = "mo")]
    public ActionResult FinPlanUpdate(List<tFinPlan> data)
    {
      var q = repRepository.FinPlanUpdate(data);
      return Json(new { success = true, message = "Сохранено", data = q });
    }

    [Authorize(Roles = "mo")]
    public ActionResult FinActive()
    {
      return View();
    }

    [Authorize(Roles = "mo")]
    public ActionResult FinActList(string sort, string dir, int? y, int? q)
    {
      var q1 = repRepository.FinActList(sort, dir, y, q);
      return Json(new { data = q1 });
    }

    [Authorize(Roles = "mo")]
    public ActionResult FinActUpdate(List<tFinActive> data)
    {
      var q = repRepository.FinActUpdate(data);
      return Json(new { success = true, message = "Сохранено", data = q });
    }

    [Authorize(Roles = "mo")]
    public ActionResult FinSales()
    {
      return View();
    }

    [Authorize(Roles = "mo")]
    public ActionResult FinSalesList(string sort, string dir, int? y, int? q)
    {
      var q1 = repRepository.FinSalesList(sort, dir, y, q);
      return Json(new { data = q1 });
    }

    [Authorize(Roles = "mo")]
    public ActionResult FinSalesUpdate(List<tFinSale> data)
    {
      var q = repRepository.FinSalesUpdate(data);
      return Json(new { success = true, message = "Сохранено", data = q });
    }

    [Authorize(Roles = "mo")]
    public ActionResult FinProfit()
    {
      return View();
    }

    [Authorize(Roles = "mo")]
    public ActionResult FinProfitList(string sort, string dir, int? y, int? q)
    {
      var q1 = repRepository.FinProfitList(sort, dir, y, q);
      return Json(new { data = q1 });
    }

    [Authorize(Roles = "mo")]
    public ActionResult FinProfitUpdate(List<tFinProfit> data)
    {
      var q = repRepository.FinProfitUpdate(data);
      return Json(new { success = true, message = "Сохранено", data = q });
    }

    [Authorize(Roles = "mo")]
    public ActionResult FinDohodSchema()
    {
      return View();
    }

    [Authorize(Roles = "mo")]
    public ActionResult FDSList(string sort, string dir, int? start, int? limit, string filter)
    {
      var q = repRepository.GetFDS(sort, dir, filter).ToList();
      return Json(new { data = q.Skip(start ?? 0).Take(limit ?? 500), totalCount = q.Count() });
    }

    [Authorize(Roles = "mo")]
    public ActionResult FDSCreate(List<tFinDohodSchema> data)
    {
      var q = repRepository.FDSCreate(data).ToList();
      return Json(new { success = true, message = "Сохранено", data = q });
    }

    [Authorize(Roles = "mo")]
    public ActionResult FDSUpdate(List<tFinDohodSchema> data)
    {
      var q = repRepository.FDSUpdate(data);
      return Json(new { success = true, message = "Сохранено", data = q });
    }

    [Authorize(Roles = "mo")]
    public ActionResult FDSDel(List<tFinDohodSchema> data)
    {
      if (repRepository.FDSDel(data))
        return Json(new { success = true });
      else
        return Json(new { success = false, msg = "Ошибка при удалении" });
    }

    [Authorize(Roles = "mo")]
    public ActionResult FinDohod()
    {
      return View();
    }

    [Authorize(Roles = "mo")]
    public ActionResult FinDohodList(string sort, string dir, int? y, int? q)
    {
      var q1 = repRepository.FinDohodList(sort, dir, y, q);
      return Json(new { data = q1 });
    }

    [Authorize(Roles = "mo")]
    public ActionResult FinDohodUpdate(List<tFinDohod> data)
    {
      var q = repRepository.FinDohodUpdate(data);
      return Json(new { success = true, message = "Сохранено", data = q });
    }

    [Authorize(Roles = "mo")]
    public ActionResult Kladr()
    {
      return View();
    }

    [Authorize(Roles = "mo")]
    public ActionResult GetKladr(int? id, string query, int? start, int? limit, string Code1)
    {
      return Json(new { data = repRepository.GetKladr(id, query ?? "", limit ?? 10, Code1) }, JsonRequestBehavior.AllowGet);
    }

    [Authorize(Roles = "mo")]
    public ActionResult GetKladr1(int? id, string Code1)
    {
      return Json(new { data = repRepository.GetKladr1(id, Code1) }, JsonRequestBehavior.AllowGet);
    }

    [Authorize(Roles = "mo")]
    public ActionResult FinActProfit()
    {
      return View();
    }

    [Authorize(Roles = "mo")]
    public ActionResult FinActProfitList(string sort, string dir, int? y)
    {
      var q1 = repRepository.FinActProfitList(sort, dir, y);
      return Json(new { data = q1 });
    }

    [Authorize(Roles = "mo")]
    public ActionResult FinActProfitUpdate(List<tFinActProfit> data)
    {
      var q = repRepository.FinActProfitUpdate(data);
      return Json(new { success = true, message = "Сохранено", data = q });
    }

    [Authorize(Roles = "pai")]
    public ActionResult PIFerRest()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);
      ViewBag.RequestURL = "PIFerRestExcel";
      return View("PIFRests");
    }

    [Authorize(Roles = "pai")]
    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult PIFerRestExcel(DateTime? d, int? FinInstID, string clName)
    {
      try
      {
        var q = repRepository.GetPIFerRest(d, FinInstID, clName);

        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Лист");
        worksheet.Cell(2, 1).Value = string.Format("Информация о состоянии лицевых счетов на {0:dd.MM.yyyy}", d);
        worksheet.Column(1).Width = 43;
        worksheet.Column(2).Width = 48;
        worksheet.Column(3).Width = 12;
        worksheet.Column(4).Width = 15;
        worksheet.Column(5).Width = 17;
        worksheet.Column(6).Width = 17;
        worksheet.Column(7).Width = 17;
        worksheet.Cell(4, 1).Value = "Владелец";
        worksheet.Cell(4, 2).Value = "Фонд";
        worksheet.Cell(4, 3).Value = "Дата отчета (отчетная дата)";
        worksheet.Cell(4, 4).Value = "Номер лицевого счета";
        worksheet.Cell(4, 5).Value = "Количество инвестиционных паев на счете на дату отчета";
        worksheet.Cell(4, 6).Value = "Расчетная стоимость 1(одного) инвестиционного пая на дату отчета";
        worksheet.Cell(4, 7).Value = "Общая стоимость инвестиционных паев на отчетную дату  (руб.)*";

        var range = worksheet.Range(4, 1, 4, 7);
        range.Style.Fill.BackgroundColor = XLColor.FromArgb(233, 217, 253);
        range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        range.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        range.Style.Alignment.SetWrapText(true);
        range.Style.Font.Bold = true;
        var i = 5;
        foreach (var r in q)
        {
          worksheet.Cell(i, 1).Value = r.Name;
          worksheet.Cell(i, 2).SetValue(string.Format("ОПИФ {0}", r.Name1));
          worksheet.Cell(i, 2).Style.Alignment.SetWrapText(true);
          worksheet.Cell(i, 3).Value = r.RestDate;
          worksheet.Cell(i, 4).SetValue(r.Acc.Length == 10 ? "000" + r.Acc.Substring(3) : r.Acc);
          worksheet.Cell(i, 5).Value = r.Rest;
          worksheet.Cell(i, 6).Value = r.CourseCurrent;
          worksheet.Cell(i, 7).Value = r.Qty;
          i++;
        }
        worksheet.Cell(i, 1).Value = "Сумма";
        range = worksheet.Range(i, 1, i, 6);
        range.Merge(false);
        range.Style.Font.Bold = true;
        range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

        worksheet.Cell(i, 7).FormulaR1C1 = "=SUM(R1C:R[-1]C)";
        worksheet.Range(5, 7, i, 7).Style.NumberFormat.NumberFormatId = 4;
        worksheet.Range(5, 1, i - 1, 1).Style.Alignment.SetWrapText(true);
        worksheet.Range(5, 3, i - 1, 3).Style.Alignment.SetWrapText(true);

        range = worksheet.Range(4, 1, i, 7);
        range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        worksheet.Cell(i + 1, 1).Value = "* данные в Отчете об общей стоимости рассчитаны без учета налога на доходы и скидки при погашении, исходя из расчетной стоимости инвестиционных паев на дату формирования отчета.Стоимость инвестиционных паев может, как увеличиваться, так и уменьшаться. Результаты инвестирования в инвестиционные паи паевого инвестиционного фонда в прошлом не определяют доходы в будущем.Определение фактического размера дохода/убытка возможно будет произвести только после совершения Вами всех действий, направленных на погашение принадлежащих Вам инвестиционных паев.";
        worksheet.Range(i + 1, 1, i + 1, 7).Merge(false).Style.Alignment.SetWrapText(true).Font.SetFontSize(8).Font.SetItalic(true);
        worksheet.Row(i + 1).Height = 39;

        worksheet.Cell(i + 3, 1).Value = "В соответствии со статьей 47 Федерального закона от 29.11.2001 № 156-ФЗ  «Об инвестиционных фондах»  Управляющая компания не осуществляет ведение реестра владельцев инвестиционных паев паевых инвестиционных фондов, находящихся под управлением АО «УК УРАЛСИБ», а также   не располагает сведениями об операциях с инвестиционными паями, которые были оформлены без участия Управляющей компании или ее агентов по выдаче, погашению и обмену инвестиционных паев. Для получения информации о наличии и количестве инвестиционных паев, в том числе для получения выписки с подтверждением  остатка по лицевому счету,  Вам необходимо направить соответствующий запрос в организацию, которая осуществляет ведение реестра владельцев инвестиционных паев паевых инвестиционных фондов, находящихся под управлением АО «УК УРАЛСИБ», Акционерное общество «Независимая регистраторская компания Р.О.С.Т.», по адресу: 107076, г. Москва, ул. Стромынка, д. 18, корпус 13, тел.: +7 (495) 989 76 50, +7 (495) 780-73-63. Оформить запрос на выписку также возможно путем обращения в пункты приема заявок Управляющей компании либо агента по выдаче, погашению и обмену инвестиционных паев. Информацию о местах приема заявок Вы можете уточнить в консультационном центре по телефону 8-800-200-90-58 (звонки по России бесплатно), а также на Сайте Управляющей компании в информационно-телекоммуникационной сети «Интернет» по адресу:  https://www.uralsib-am.ru";
        worksheet.Range(i + 3, 1, i + 3, 7).Merge(false).Style.Alignment.SetWrapText(true).Font.SetFontSize(8).Font.SetItalic(true);
        worksheet.Row(i + 3).Height = 81;

        worksheet.PageSetup.SetPageOrientation(XLPageOrientation.Landscape).SetPagesWide(1).PrintAreas.Add(1, 1, i + 3, 7);
        MemoryStream ms = new MemoryStream();
        workbook.SaveAs(ms);
        return File(ms.ToArray(), "application/vnd.ms-excel", string.Format("pf{0}.xlsx", DateTime.Now.Second));
      }
      catch (Exception ex)
      {
      }
      return View("Error");

      //  FileStream fs = new FileStream(Server.MapPath(@"\Templates\PIFerList.xls"), FileMode.Open, FileAccess.Read);
      //  HSSFWorkbook workbook = new HSSFWorkbook(fs, true);
      //  ISheet sheet = workbook.GetSheet("422");
      //  var f = repRepository.GetFinInst(FinInstID ?? 0);
      //  IRow row = sheet.GetRow(2);
      //  row.GetCell(0).SetCellValue(string.Format("ПО СОСТОЯНИЮ НА {0:dd.MM.yyyy}", d));
      //  (sheet.GetRow(3) as IRow).GetCell(0).SetCellValue(f.name);
      //  int ir = 6;
      //  //var cellStyle = workbook.CreateCellStyle();
      //  //cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,#0.0");

      //  foreach (var r in q)
      //  {
      //    row = sheet.CreateRow(ir++);
      //    int c = 0;
      //    row.CreateCell(c++).SetCellValue(ir - 6);
      //    row.CreateCell(c++).SetCellValue(r.Brief ?? "");
      //    row.CreateCell(c++).SetCellValue(r.Name ?? "");
      //    row.CreateCell(c++).SetCellValue(r.Sex ?? "");
      //    row.CreateCell(c++).SetCellValue(r.RegSeries ?? "");
      //    row.CreateCell(c++).SetCellValue(r.RegNumber ?? "");
      //    row.CreateCell(c++).SetCellValue((double)(r.Rest ?? 0));
      //    row.CreateCell(c++).SetCellValue((double)(r.CourseCurrent ?? 0));
      //    row.CreateCell(c++).SetCellValue((double)(r.Qty ?? 0));
      //    row.CreateCell(c++).SetCellValue((double)(r.Part ?? 0));
      //    row.CreateCell(c++).SetCellValue(r.Phone ?? "");
      //  }
      //  row = sheet.CreateRow(ir++);
      //  var cell = row.CreateCell(6);
      //  //(cell as HSSFCell).CellStyle = cellStyle;
      //  cell.SetCellType(CellType.Formula);
      //  cell.SetCellFormula(string.Format("SUM(G7:G{0})", ir - 1));
      //  cell = row.CreateCell(8);
      //  cell.SetCellType(CellType.Formula);
      //  cell.SetCellFormula(string.Format("SUM(I7:I{0})", ir - 1));
      //  cell = row.CreateCell(9);
      //  cell.SetCellType(CellType.Formula);
      //  cell.SetCellFormula(string.Format("SUM(J7:J{0})", ir - 1));
      //  MemoryStream ms = new MemoryStream();
      //  workbook.Write(ms);
      //  return File(ms.ToArray(), "application/vnd.ms-excel", "pl.xls");
      //}
      //catch (Exception /*ex*/)
      //{
      //}
      //return View("Error");
    }

    [Authorize(Roles = "pai")]
    public ActionResult PIFRests()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);
      ViewBag.RequestURL = "PIFerRestAllExcel";
      return View();
    }

    private void PIFerRestAllTitle(HSSFSheet sheet)
    {
      HSSFWorkbook workbook = (HSSFWorkbook)sheet.Workbook;
      HSSFCellStyle cellStyleNum = (HSSFCellStyle)workbook.CreateCellStyle();
      cellStyleNum.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0.00");
      HSSFCellStyle cellStyleDat = (HSSFCellStyle)workbook.CreateCellStyle();
      cellStyleDat.DataFormat = workbook.CreateDataFormat().GetFormat("dd.mm.yyyy");
      HSSFCellStyle cellStyleTit = (HSSFCellStyle)workbook.CreateCellStyle();

      cellStyleTit.BorderBottom = BorderStyle.Thin;
      cellStyleTit.BorderTop = BorderStyle.Thin;
      cellStyleTit.BorderLeft = BorderStyle.Thin;
      cellStyleTit.BorderRight = BorderStyle.Thin;
      cellStyleTit.BottomBorderColor = HSSFColor.Black.Index;
      cellStyleTit.TopBorderColor = HSSFColor.Black.Index;
      cellStyleTit.LeftBorderColor = HSSFColor.Black.Index;
      cellStyleTit.RightBorderColor = HSSFColor.Black.Index;
      cellStyleTit.WrapText = true;

      IFont font = workbook.CreateFont();
      font.FontHeightInPoints = 8;
      font.Boldweight = 1600;
      font.Color = HSSFColor.Black.Index;
      cellStyleTit.SetFont(font);
      cellStyleTit.Alignment = HorizontalAlignment.Center;

      IFont defaultFont = workbook.GetFontAt(0);
      defaultFont.FontHeightInPoints = 8;
      sheet.SetDefaultColumnStyle(2, cellStyleDat);
      sheet.SetDefaultColumnStyle(4, cellStyleDat);
      sheet.SetDefaultColumnStyle(7, cellStyleDat);
      sheet.SetDefaultColumnStyle(10, cellStyleNum);
      sheet.SetDefaultColumnStyle(11, cellStyleNum);
      sheet.SetDefaultColumnStyle(22, cellStyleDat);
      sheet.SetDefaultColumnStyle(23, cellStyleDat);
      sheet.SetColumnWidth(0, 256 * 28);
      sheet.SetColumnWidth(1, 256 * 50);
      sheet.SetColumnWidth(2, 256 * 12);
      sheet.SetColumnWidth(4, 256 * 12);
      sheet.SetColumnWidth(5, 256 * 13);
      sheet.SetColumnWidth(7, 256 * 12);
      sheet.SetColumnWidth(10, 256 * 14);
      sheet.SetColumnWidth(11, 256 * 15);
      sheet.SetColumnWidth(12, 256 * 12);
      sheet.SetColumnWidth(13, 256 * 20);
      sheet.SetColumnWidth(14, 256 * 12);
      sheet.SetColumnWidth(18, 256 * 40);
      sheet.SetColumnWidth(20, 256 * 40);
      sheet.SetColumnWidth(22, 256 * 12);
      sheet.SetColumnWidth(23, 256 * 12);

      IRow row = null;
      ICell cell;
      int c;

      row = sheet.CreateRow(5);
      c = 0;
      cell = row.CreateCell(c++);
      cell.CellStyle = cellStyleTit;
      cell.SetCellValue("Код пайщика");

      cell = row.CreateCell(c++);
      cell.CellStyle = cellStyleTit;
      cell.SetCellValue("ФИО пайщика");

      cell = row.CreateCell(c++);
      cell.CellStyle = cellStyleTit;
      cell.SetCellValue("Дата рождения пайщика");

      cell = row.CreateCell(c++);
      cell.CellStyle = cellStyleTit;
      cell.SetCellValue("Фонд");

      cell = row.CreateCell(c++);
      cell.CellStyle = cellStyleTit;
      cell.SetCellValue("Дата последней операции");

      cell = row.CreateCell(c++);
      cell.CellStyle = cellStyleTit;
      cell.SetCellValue("Последняя операция");

      cell = row.CreateCell(c++);
      cell.CellStyle = cellStyleTit;
      cell.SetCellValue("Последняя ОФБУ");

      cell = row.CreateCell(c++);
      cell.CellStyle = cellStyleTit;
      cell.SetCellValue("Дата остатка");

      cell = row.CreateCell(c++);
      cell.CellStyle = cellStyleTit;
      cell.SetCellValue("Количество паев на счете");

      cell = row.CreateCell(c++);
      cell.CellStyle = cellStyleTit;
      cell.SetCellValue("Цена пая");

      cell = row.CreateCell(c++);
      cell.CellStyle = cellStyleTit;
      cell.SetCellValue("Оценка позиции");

      cell = row.CreateCell(c++);
      cell.CellStyle = cellStyleTit;
      cell.SetCellValue("Оценка портфеля");

      cell = row.CreateCell(c++);
      cell.CellStyle = cellStyleTit;
      cell.SetCellValue("Доля от фонда");

      cell = row.CreateCell(c++);
      cell.CellStyle = cellStyleTit;
      cell.SetCellValue("Телефоны");

      cell = row.CreateCell(c++);
      cell.CellStyle = cellStyleTit;
      cell.SetCellValue("Лицевой счет");

      cell = row.CreateCell(c++);
      cell.CellStyle = cellStyleTit;
      cell.SetCellValue("Пол");

      cell = row.CreateCell(c++);
      cell.CellStyle = cellStyleTit;
      cell.SetCellValue("Тип клиента");

      cell = row.CreateCell(c++);
      cell.CellStyle = cellStyleTit;
      cell.SetCellValue("РД");

      cell = row.CreateCell(c++);
      cell.CellStyle = cellStyleTit;
      cell.SetCellValue("Точка продаж");

      cell = row.CreateCell(c++);
      cell.CellStyle = cellStyleTit;
      cell.SetCellValue("Номер ТП");

      cell = row.CreateCell(c++);
      cell.CellStyle = cellStyleTit;
      cell.SetCellValue("ФИО продавца");

      cell = row.CreateCell(c++);
      cell.CellStyle = cellStyleTit;
      cell.SetCellValue("Тип счета");

      cell = row.CreateCell(c++);
      cell.CellStyle = cellStyleTit;
      cell.SetCellValue("Первая покупка");

      cell = row.CreateCell(c++);
      cell.CellStyle = cellStyleTit;
      cell.SetCellValue("Открытие счета");

      cell = row.CreateCell(c++);
      cell.CellStyle = cellStyleTit;
      cell.SetCellValue("ИНН");

      cell = row.CreateCell(c++);
      cell.CellStyle = cellStyleTit;
      cell.SetCellValue("Адрес");

      //cell = row.CreateCell(c++);
      //cell.CellStyle = cellStyleTit;
      //cell.SetCellValue("Серия");

      //cell = row.CreateCell(c++);
      //cell.CellStyle = cellStyleTit;
      //cell.SetCellValue("Номер");
    }

    [Authorize(Roles = "pai")]
    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult PIFerRestAllExcel(DateTime? d, int? FinInstID, string clName)
    {
      try
      {
        var q = repRepository.GetPIFRests(d, FinInstID, clName);
        HSSFWorkbook workbook = new HSSFWorkbook();

        IRow row = null;
        ICell cell;
        int ira = 6;
        int c = 0;
        HSSFSheet sheeta = (HSSFSheet)workbook.CreateSheet("Итоговая");
        PIFerRestAllTitle(sheeta);

        foreach (var r in q)
        {
          //if (sid != r.SecurityID)
          //{
          //  sid = r.SecurityID;

          //  sheet = (HSSFSheet)workbook.CreateSheet(r.PIFBrief);
          //  PIFerRestAllTitle(sheet);
          //  ir = 6;
          //}
          //c = 0;
          //row = sheet.CreateRow(ir++);
          //row.CreateCell(c++).SetCellValue(r.Brief ?? "");
          //row.CreateCell(c++).SetCellValue(r.Name ?? "");
          //cell = row.CreateCell(c++); if (r.BirthDay != null) cell.SetCellValue(r.BirthDay.Value);
          //row.CreateCell(c++).SetCellValue(r.PIFBrief ?? "");
          //cell = row.CreateCell(c++); if (r.ContrDate != null) cell.SetCellValue(r.ContrDate.Value);
          //row.CreateCell(c++).SetCellValue(r.DealType ?? "");
          //row.CreateCell(c++).SetCellValue(r.instrBrief ?? "");
          //row.CreateCell(c++).SetCellValue(r.RestDate);
          //row.CreateCell(c++).SetCellValue((double)(r.Rest ?? 0));
          //row.CreateCell(c++).SetCellValue((double)(r.CourseCurrent ?? 0));
          //row.CreateCell(c++).SetCellValue((double)(r.Qty ?? 0));
          //row.CreateCell(c++).SetCellValue((double)(r.QtyA ?? 0));
          //row.CreateCell(c++).SetCellValue((double)(r.Part ?? 0));
          //row.CreateCell(c++).SetCellValue(r.Phones ?? "");
          //row.CreateCell(c++).SetCellValue(r.ResBrief ?? "");
          //row.CreateCell(c++).SetCellValue(r.Sex ?? "");
          //row.CreateCell(c++).SetCellValue(r.FU ?? "");
          //row.CreateCell(c++).SetCellValue(r.RD ?? "");
          //row.CreateCell(c++).SetCellValue(r.PointOfSale ?? "");
          //row.CreateCell(c++).SetCellValue(r.NomberPS ?? "");
          //row.CreateCell(c++).SetCellValue(r.Seller ?? "");
          //row.CreateCell(c++).SetCellValue(r.AccType ?? "");
          //cell = row.CreateCell(c++); if (r.FirstDate != null) cell.SetCellValue(r.FirstDate.Value);
          //cell = row.CreateCell(c++); if (r.DateStart != null) cell.SetCellValue(r.DateStart.Value);
          //row.CreateCell(c++).SetCellValue(r.INN ?? "");

          row = sheeta.CreateRow(ira++);
          c = 0;
          row.CreateCell(c++).SetCellValue(r.Brief ?? "");
          row.CreateCell(c++).SetCellValue(r.Name ?? "");
          cell = row.CreateCell(c++); if (r.BirthDay != null) cell.SetCellValue(r.BirthDay.Value);
          row.CreateCell(c++).SetCellValue(r.PIFBrief ?? "");
          cell = row.CreateCell(c++); if (r.ContrDate != null) cell.SetCellValue(r.ContrDate.Value);
          row.CreateCell(c++).SetCellValue(r.DealType ?? "");
          row.CreateCell(c++).SetCellValue(r.instrBrief ?? "");
          row.CreateCell(c++).SetCellValue(r.RestDate);
          row.CreateCell(c++).SetCellValue((double)(r.Rest ?? 0));
          row.CreateCell(c++).SetCellValue((double)(r.CourseCurrent ?? 0));
          row.CreateCell(c++).SetCellValue((double)(r.Qty ?? 0));
          row.CreateCell(c++).SetCellValue((double)(r.QtyA ?? 0));
          row.CreateCell(c++).SetCellValue((double)(r.Part ?? 0));
          row.CreateCell(c++).SetCellValue(r.Phones ?? "");
          row.CreateCell(c++).SetCellValue(r.ResBrief ?? "");
          row.CreateCell(c++).SetCellValue(r.Sex ?? "");
          row.CreateCell(c++).SetCellValue(r.FU ?? "");
          row.CreateCell(c++).SetCellValue(r.RD ?? "");
          row.CreateCell(c++).SetCellValue(r.PointOfSale ?? "");
          row.CreateCell(c++).SetCellValue(r.NomberPS ?? "");
          row.CreateCell(c++).SetCellValue(r.Seller ?? "");
          row.CreateCell(c++).SetCellValue(r.AccType ?? "");
          cell = row.CreateCell(c++); if (r.FirstDate != null) cell.SetCellValue(r.FirstDate.Value);
          cell = row.CreateCell(c++); if (r.DateStart != null) cell.SetCellValue(r.DateStart.Value);
          row.CreateCell(c++).SetCellValue(r.INN ?? "");
          row.CreateCell(c++).SetCellValue(r.AddressLegal ?? "");
          //row.CreateCell(c++).SetCellValue(r.RegSeries ?? "");
          //row.CreateCell(c++).SetCellValue(r.RegNumber ?? "");
        }
        MemoryStream ms = new MemoryStream();
        workbook.Write(ms);
        return File(ms.ToArray(), "application/vnd.ms-excel", "pla.xls");
      }
      catch (Exception ex)
      {
      }
      return View("Error");
    }

    [Authorize(Roles = "pai")]
    public ActionResult PIFRestStat()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);
      return View();
    }

    [Authorize(Roles = "pai")]
    [ActionName("PIFRestStat")]
    [HttpPost]
    public ActionResult PIFerRestStatExcel(DateTime? d, int? FinInstID, string clName)
    {
      try
      {
        var q = repRepository.GetPIFRestStat(d);
        FileStream fs = new FileStream(Server.MapPath(@"\Templates\PIFRestStat.xls"), FileMode.Open, FileAccess.Read);
        HSSFWorkbook workbook = new HSSFWorkbook(fs, true);
        ISheet sheet = workbook.GetSheet("Лист1");
        IRow row = null;
        int ir = 4;
        int c = 0;
        foreach (var r in q)
        {
          c = 0;
          row = sheet.CreateRow(ir++);
          row.CreateCell(c++).SetCellValue(r.Name ?? "");
          row.CreateCell(c++).SetCellValue(r.QT ?? "");
          row.CreateCell(c++).SetCellValue((double)(r.f1 ?? 0));
          row.CreateCell(c++).SetCellValue((double)(r.f2 ?? 0));
          row.CreateCell(c++).SetCellValue((double)(r.f5 ?? 0));
          row.CreateCell(c++).SetCellValue((double)(r.f3 ?? 0));
          row.CreateCell(c++).SetCellValue((double)(r.f4 ?? 0));
          row.CreateCell(c++).SetCellValue((double)(r.f6 ?? 0));
          row.CreateCell(c++).SetCellValue((double)(r.u1 ?? 0));
          row.CreateCell(c++).SetCellValue((double)(r.u2 ?? 0));
          row.CreateCell(c++).SetCellValue((double)(r.u5 ?? 0));
          row.CreateCell(c++).SetCellValue((double)(r.u3 ?? 0));
          row.CreateCell(c++).SetCellValue((double)(r.u4 ?? 0));
          row.CreateCell(c++).SetCellValue((double)(r.u6 ?? 0));
          row.CreateCell(c++).SetCellValue((double)(r.cnt ?? 0));
          row.CreateCell(c++).SetCellValue((double)(r.cnt1 ?? 0));
          row.CreateCell(c++).SetCellValue((double)(r.q ?? 0));
          row.CreateCell(c++).SetCellValue((double)(r.v1 ?? 0));
          row.CreateCell(c++).SetCellValue((double)(r.v2 ?? 0));
          row.CreateCell(c++).SetCellValue((double)(r.v3 ?? 0));
          row.CreateCell(c++).SetCellValue((double)(r.v4 ?? 0));
        }
        MemoryStream ms = new MemoryStream();
        workbook.Write(ms);
        return File(ms.ToArray(), "application/vnd.ms-excel", "prs.xls");
      }
      catch (Exception /*ex*/)
      {
      }
      return View("Error");
    }

    [Authorize(Roles = "pai")]
    public ActionResult RepPIFOrderStat()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);
      ViewBag.Title = "Статистика по заявкам на покупку";
      return View();
    }

    [Authorize(Roles = "pai")]
    [ActionName("RepPIFOrderStat")]
    [HttpPost]
    public ActionResult RepPIFOrderStatExcel(DateTime? d1, DateTime? d2)
    {
      //try
      //{
      var q = repRepository.RepPIFOrderStat(d1, d2);
      FileStream fs = new FileStream(Server.MapPath(@"\Templates\PIFOrderStat.xls"), FileMode.Open, FileAccess.Read);
      HSSFWorkbook workbook = new HSSFWorkbook(fs, true);
      ISheet sheet = workbook.GetSheet("Лист1");
      IRow row = null;
      row = sheet.GetOrCreateRow(0);
      row.GetOrCreateCell(1).SetCellValue(d1.Value);
      row.GetOrCreateCell(2).SetCellValue(d2.Value);
      int ir = 2;
      int c = 0;
      foreach (var r in q)
      {
        c = 0;
        row = sheet.GetOrCreateRow(ir++);
        row.GetOrCreateCell(c++).SetCellValue(r.QtyType ?? "");
        row.GetOrCreateCell(c++).SetCellValue((double)(r.Cnt ?? 0));
        row.GetOrCreateCell(c++).SetCellValue((double)(r.CntP ?? 0));
        row.GetOrCreateCell(c++).SetCellValue((double)(r.CntM ?? 0));
        row.GetOrCreateCell(c++).SetCellValue((double)(r.CntMP ?? 0));
        row.GetOrCreateCell(c++).SetCellValue((double)(r.Qty ?? 0));
        row.GetOrCreateCell(c++).SetCellValue((double)(r.QtyP ?? 0));
        row.GetOrCreateCell(c++).SetCellValue((double)(r.QtyM ?? 0));
        row.GetOrCreateCell(c++).SetCellValue((double)(r.QtyMP ?? 0));
        row.GetOrCreateCell(c++).SetCellValue((double)(r.AQty ?? 0));
        row.GetOrCreateCell(c++).SetCellValue((double)(r.AQtyM ?? 0));
      }
      MemoryStream ms = new MemoryStream();
      workbook.Write(ms);
      return File(ms.ToArray(), "application/vnd.ms-excel", string.Format("os{0}.xls", DateTime.Now.Second));
      //}
      //catch (Exception /*ex*/)
      //{
      //}
      //return View("Error");
    }

    [Authorize(Roles = "pai")]
    public ActionResult RepPIFOrderOutStat()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);
      ViewBag.Title = "Статистика по заявкам на вывод из ПИФов";
      return View("RepPIFOrderStat");
    }

    [Authorize(Roles = "pai")]
    [ActionName("RepPIFOrderOutStat")]
    [HttpPost]
    public ActionResult RepPIFOrderOutStatExcel(DateTime? d1, DateTime? d2)
    {
      //try
      //{
      var q = repRepository.RepPIFOrderOutStat(d1, d2);
      FileStream fs = new FileStream(Server.MapPath(@"\Templates\PIFOrderOutStat.xls"), FileMode.Open, FileAccess.Read);
      HSSFWorkbook workbook = new HSSFWorkbook(fs, true);
      ISheet sheet = workbook.GetSheet("Лист1");
      IRow row = null;
      row = sheet.GetOrCreateRow(0);
      row.GetOrCreateCell(1).SetCellValue(d1.Value);
      row.GetOrCreateCell(2).SetCellValue(d2.Value);
      int ir = 2;
      int c = 0;
      foreach (var r in q)
      {
        c = 0;
        row = sheet.GetOrCreateRow(ir++);
        row.GetOrCreateCell(c++).SetCellValue(r.QtyType ?? "");
        row.GetOrCreateCell(c++).SetCellValue((double)(r.Cnt ?? 0));
        row.GetOrCreateCell(c++).SetCellValue((double)(r.CntP ?? 0));
        row.GetOrCreateCell(c++).SetCellValue((double)(r.Qty ?? 0));
        row.GetOrCreateCell(c++).SetCellValue((double)(r.QtyP ?? 0));
        row.GetOrCreateCell(c++).SetCellValue((double)(r.AQty ?? 0));
      }
      MemoryStream ms = new MemoryStream();
      workbook.Write(ms);
      return File(ms.ToArray(), "application/vnd.ms-excel", string.Format("ost{0}.xls", DateTime.Now.Second));
      //}
      //catch (Exception /*ex*/)
      //{
      //}
      //return View("Error");
    }

    [Authorize(Roles = "paib")]
    public ActionResult RepPIFOrders()
    {
      ViewBag.Title = "Данные по портфелю РББ УралСиб";
      return View();
    }

    [Authorize(Roles = "paib")]
    [ActionName("RepPIFOrders")]
    [HttpPost]
    public ActionResult RepPIFOrdersExcel(DateTime? d)
    {
      try
      {
        FileStream fs = new FileStream(Server.MapPath(@"\Templates\RepPIFOrders.xls"), FileMode.Open, FileAccess.Read);
        HSSFWorkbook workbook = new HSSFWorkbook(fs, true);
        ISheet sheet = workbook.GetSheet("Лист1");
        ICell cell;
        var q = repRepository.RepPIFOrders(d);
        IRow row = null;
        int ir = 1;
        foreach (var r in q)
        {
          row = sheet.CreateRow(ir++);
          int c = 0;
          row.CreateCell(c++).SetCellValue(r.РД ?? "");
          row.CreateCell(c++).SetCellValue(r.Точка_продаж ?? "");
          row.CreateCell(c++).SetCellValue(r.Номер_ТП ?? "");
          row.CreateCell(c++).SetCellValue(r.ФИО_продавца ?? "");
          row.CreateCell(c++).SetCellValue(r.Табельный_номер ?? "");
          row.CreateCell(c++).SetCellValue(r.Код_пайщика ?? "");
          row.CreateCell(c++).SetCellValue(r.ФИО_пайщика__полностью_ ?? "");
          cell = row.CreateCell(c++); if (r.Дата_рождения_пайщика != null) cell.SetCellValue(r.Дата_рождения_пайщика.Value);
          row.CreateCell(c++).SetCellValue(r.Фонд ?? "");
          cell = row.CreateCell(c++); if (r.Дата_первой_покупки != null) cell.SetCellValue(r.Дата_первой_покупки.Value);
          row.CreateCell(c++).SetCellValue(r.Дата_остатка.Value);
          row.CreateCell(c++).SetCellValue((double)(r.Количество_паев_на_счете ?? 0));
          row.CreateCell(c++).SetCellValue((double)(r.Цена_пая ?? 0));
          row.CreateCell(c++).SetCellValue((double)(r.Оценка_позиции ?? 0));
          row.CreateCell(c++).SetCellValue((double)(r.Оценка_портфеля ?? 0));
          row.CreateCell(c++).SetCellValue((double)(r.ClientID ?? 0));
          row.CreateCell(c++).SetCellValue((double)(r.ExternalID ?? 0));
          row.CreateCell(c++).SetCellValue(r.Пол ?? "");
          row.CreateCell(c++).SetCellValue(r.Тип ?? "");
          row.CreateCell(c++).SetCellValue(r.Phones ?? "");
        }
        MemoryStream ms = new MemoryStream();
        workbook.Write(ms);
        return File(ms.ToArray(), "application/vnd.ms-excel", "dpl.xls");
      }
      catch (Exception /*ex*/)
      {
      }
      return View("Error");
    }

    [Authorize(Roles = "pai")]
    public ActionResult GetPIFList()
    {
      return new JsonnResult { Data = new { success = true, data = repRepository.GetPIFList() } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult FinInOutCome()
    {
      return View();
    }

    [Authorize(Roles = "mo")]
    public ActionResult FinInOutComeList(string sort, string dir, int? an)
    {
      var q1 = repRepository.FinInOutComeList(sort, dir, an);
      return Json(new { data = q1 });
    }

    [Authorize(Roles = "mo")]
    public ActionResult FinInOutComeUpdate(List<tFinInOutCome> data)
    {
      var q = repRepository.FinInOutComeUpdate(data);
      return Json(new { success = true, message = "Сохранено", data = q });
    }

    [Authorize(Roles = "mo")]
    public ActionResult FinPart()
    {
      return View();
    }

    [Authorize(Roles = "mo")]
    public ActionResult FinPartList(string sort, string dir, DateTime? dt)
    {
      var q1 = repRepository.FinPartList(sort, dir, dt);
      return new JsonnResult { Data = new { data = q1 } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult FinPartUpdate(List<FinPartViewModel> data)
    {
      var q1 = repRepository.FinPartUpdate(data);
      return new JsonnResult { Data = new { data = q1 } };
    }

    [Authorize(Roles = "mo, bank")]
    public ActionResult GetCbBPifer()
    {
      return new JsonnResult { Data = new { success = true, data = repRepository.GetCbBPifer() } };
    }

    [Authorize(Roles = "mo, bank")]
    public ActionResult GetChartData()
    {
      return new JsonnResult { Data = new { success = true, data = repRepository.GetChartData() } };
    }

    public ActionResult GetPDF()
    {
      var text = new System.Text.StringBuilder();
      using (var pdfReader = new iTextSharp.text.pdf.PdfReader(Server.MapPath(@"\Templates\Open.pdf")))
      {
        for (var page = 1; page <= pdfReader.NumberOfPages; page++)
        {
          iTextSharp.text.pdf.parser.ITextExtractionStrategy strategy = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();

          var currentText = iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

          currentText = System.Text.Encoding.UTF8.GetString(
            System.Text.Encoding.Convert(
              System.Text.Encoding.Default,
              System.Text.Encoding.GetEncoding(1251),
              System.Text.Encoding.Default.GetBytes(currentText)
            )
          );

          text.Append(currentText);
        }
      }
      return new JsonnResult { Data = new { success = true } };
    }

    public ActionResult ClientIK()
    {
      return View();
    }

    public ActionResult GetClientIKList(string sort, string dir)
    {
      return new JsonnResult { Data = new { success = true, data = repRepository.GetClientIKList(sort, dir) } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult addClientIK(List<taNab> data)
    {
      return new JsonnResult { Data = new { success = true, data = repRepository.addClientIK(data) } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult updClientIK(List<taNab> data)
    {
      return new JsonnResult { Data = new { success = true, data = repRepository.updClientIK(data) } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult delClientIK(List<taNab> data)
    {
      return new JsonnResult { Data = new { success = true, data = repRepository.delClientIK(data) } };
    }

    public ActionResult GetIKList()
    {
      return new JsonnResult { Data = new { success = true, data = repRepository.GetIKList() } };
    }

    public ActionResult GetClIKList()
    {
      return new JsonnResult { Data = new { success = true, data = repRepository.GetClIKList() } };
    }

    [Authorize(Roles = "pai")]
    public ActionResult PIFDepStat()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);
      ViewBag.Title = "Статистика ПИФ+Вклад";
      return View("RepPIFOrderStat");
    }

    [Authorize(Roles = "pai")]
    [ActionName("PIFDepStat")]
    [HttpPost]
    public ActionResult PIFDepStatExcel(DateTime? d1, DateTime? d2)
    {
      try
      {
        FileStream fs = new FileStream(Server.MapPath(@"\Templates\PIFDepStat.xlsx"), FileMode.Open, FileAccess.Read);
        XSSFWorkbook workbook = new XSSFWorkbook(fs);
        ISheet sheet = workbook.GetSheet("Лист1");
        IRow row = null;
        int ir = 4;
        int c = 0;
        sheet.CreateRow(0).CreateCell(0).SetCellValue(string.Format("период {0:dd.MM.yyyy}-{1:dd.MM.yyyy}", d1, d2));
        var q = repRepository.RepPIFDepStat(d1, d2);
        foreach (var r in q)
        {
          c = 0;
          row = sheet.CreateRow(ir++);
          row.CreateCell(c++).SetCellValue(r.Srok ?? 0);
          row.CreateCell(c++).SetCellValue(r.Cnt ?? 0);
          row.CreateCell(c++).SetCellValue(r.AvgSum ?? 0);
          row.CreateCell(c++).SetCellValue(r.p1 ?? 0);
          row.CreateCell(c++).SetCellValue(r.p2 ?? 0);
          row.CreateCell(c++).SetCellValue(r.p3 ?? 0);
          row.CreateCell(c++).SetCellValue(r.p4 ?? 0);
          row.CreateCell(c++).SetCellValue(r.p5 ?? 0);
          row.CreateCell(c++).SetCellValue(r.p6 ?? 0);
          row.CreateCell(c++).SetCellValue(r.p7 ?? 0);
          row.CreateCell(c++).SetCellValue(r.p8 ?? 0);
          row.CreateCell(c++).SetCellValue(r.p9 ?? 0);
          row.CreateCell(c++).SetCellValue(r.p10 ?? 0);
          row.CreateCell(c++).SetCellValue(r.v1 ?? 0);
          row.CreateCell(c++).SetCellValue(r.avgv1 ?? 0);
          row.CreateCell(c++).SetCellValue(r.avg1 ?? 0);
        }
        sheet = workbook.GetSheet("Лист2");
        ir = 1;
        var q1 = repRepository.RepPIFDepStat2(d1, d2);
        foreach (var r in q1)
        {
          c = 0;
          row = sheet.CreateRow(ir++);
          row.CreateCell(c++).SetCellValue(r.Srok ?? 0);
          row.CreateCell(c++).SetCellValue(r.Дата_заявки == null ? "" : r.Дата_заявки.Value.Date.ToShortDateString());
          row.CreateCell(c++).SetCellValue(r.Клиент);
          row.CreateCell(c++).SetCellValue(r.NameBrief);
          row.CreateCell(c++).SetCellValue(r.Сумма ?? 0);
          row.CreateCell(c++).SetCellValue(r.Кол_во ?? 0);
          row.CreateCell(c++).SetCellValue(r.Остаток ?? 0);
          row.CreateCell(c++).SetCellValue(r.Дней ?? 0);
          row.CreateCell(c++).SetCellValue(r.Обмены ?? 0);
        }
        MemoryStream ms = new MemoryStream();
        workbook.Write(ms);
        return File(ms.ToArray(), "application/vnd.ms-excel", "pds.xlsx");
      }
      catch (Exception ex)
      {
      }
      return View("Error");
    }

    [Authorize(Roles = "paib")]
    public ActionResult BankPIFIncome()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);
      ViewBag.Title = "Отчет по доходам MF банка";
      return View("RepPIFOrderStat");
    }

    [Authorize(Roles = "paib")]
    [ActionName("BankPIFIncome")]
    [HttpPost]
    public ActionResult BankPIFIncomeExcel(DateTime? d1, DateTime? d2)
    {
      try
      {
        var workbook = new XLWorkbook(Server.MapPath(@"\Templates\BankPIFIncome.xlsx"));
        var worksheet = workbook.Worksheet(1);
        worksheet.Column(17).Width = 24;
        var q = repRepository.RepBankPIFIncome(d1, d2);
        var i = 4;
        foreach (var r in q)
        {
          worksheet.Cell(i, 1).SetValue(r.Начало_периода);
          worksheet.Cell(i, 2).SetValue(r.Конец_периода);
          worksheet.Cell(i, 3).SetValue(r.ФИО_пайщика);
          worksheet.Cell(i, 4).SetValue(r.Дата_рождения_пайщика);
          worksheet.Cell(i, 5).SetValue(r.Пол);
          worksheet.Cell(i, 6).SetValue(r.ID_IR);
          worksheet.Cell(i, 7).SetValue(r.ID_Diasoft);
          worksheet.Cell(i, 8).SetValue(r.Кол_во_паёв);
          worksheet.Cell(i, 9).SetValue(r.Сумма_портфеля);
          worksheet.Cell(i, 10).SetValue(r.Management_Fee);
          worksheet.Cell(i, 11).SetValue(r.Код_ППЗ);
          worksheet.Cell(i, 12).SetValue(r.Наименование_ППЗ);
          worksheet.Cell(i, 13).SetValue(r.Продавец);
          worksheet.Cell(i, 14).SetValue(r.Табномер);
          worksheet.Cell(i, 15).SetValue(r.Группа);
          worksheet.Cell(i, 16).SetValue(r.ПИФ);
          worksheet.Cell(i, 17).SetValue(r.Brief);
          worksheet.Cell(i, 18).SetValue(r.Дата_первой_покупки);
          worksheet.Cell(i, 19).SetValue(r.Сумма_первой_покупки);
          i++;
        }
        worksheet.Range(4, 9, i - 1, 10).Style.NumberFormat.NumberFormatId = 4;
        using (MemoryStream ms = new MemoryStream())
        {
          workbook.SaveAs(ms);
          return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", string.Format("bpi{0}.xlsx", DateTime.Now.Second));
        }
      }
      catch (Exception ex)
      {
      }
      return View("Error");
    }

    [Authorize(Roles = "paib")]
    public ActionResult BankPIFIncomeLK()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);
      ViewBag.Title = "Отчет по доходам MF банка ЛК";
      return View("RepPIFOrderStat");
    }

    [Authorize(Roles = "paib")]
    [ActionName("BankPIFIncomeLK")]
    [HttpPost]
    public ActionResult BankPIFIncomeLKExcel(DateTime? d1, DateTime? d2)
    {
      try
      {
        var workbook = new XLWorkbook(Server.MapPath(@"\Templates\BankPIFIncome.xlsx"));
        var worksheet = workbook.Worksheet(1);
        worksheet.Column(17).Width = 24;
        var q = repRepository.RepBankPIFIncomeLK(d1, d2);
        var i = 4;
        foreach (var r in q)
        {
          worksheet.Cell(i, 1).SetValue(r.Начало_периода);
          worksheet.Cell(i, 2).SetValue(r.Конец_периода);
          worksheet.Cell(i, 3).SetValue(r.ФИО_пайщика);
          worksheet.Cell(i, 4).SetValue(r.Дата_рождения_пайщика);
          worksheet.Cell(i, 5).SetValue(r.Пол);
          worksheet.Cell(i, 6).SetValue(r.ID_IR);
          worksheet.Cell(i, 7).SetValue(r.ID_Diasoft);
          worksheet.Cell(i, 8).SetValue(r.Кол_во_паёв);
          worksheet.Cell(i, 9).SetValue(r.Сумма_портфеля);
          worksheet.Cell(i, 10).SetValue(r.Management_Fee);
          worksheet.Cell(i, 11).SetValue(r.Код_ППЗ);
          worksheet.Cell(i, 12).SetValue(r.Наименование_ППЗ);
          worksheet.Cell(i, 13).SetValue(r.Продавец);
          worksheet.Cell(i, 14).SetValue(r.Табномер);
          worksheet.Cell(i, 15).SetValue(r.Группа);
          worksheet.Cell(i, 16).SetValue(r.ПИФ);
          worksheet.Cell(i, 17).SetValue(r.Brief);
          worksheet.Cell(i, 18).SetValue(r.Дата_первой_покупки);
          worksheet.Cell(i, 19).SetValue(r.Сумма_первой_покупки);
          i++;
        }
        worksheet.Range(4, 9, i - 1, 10).Style.NumberFormat.NumberFormatId = 4;
        using (MemoryStream ms = new MemoryStream())
        {
          workbook.SaveAs(ms);
          return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", string.Format("blk{0}.xlsx", DateTime.Now.Second));
        }
      }
      catch (Exception ex)
      {
      }
      return View("Error");
    }

    [Authorize(Roles = "pai")]
    public ActionResult PIFStat()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);
      ViewBag.Title = "Отчет размещение";
      return View("RepPIFOrderStat");
    }

    [Authorize(Roles = "pai")]
    [ActionName("PIFStat")]
    [HttpPost]
    public ActionResult PIFStateExcel(DateTime? d1, DateTime? d2)
    {
      try
      {
        FileStream fs = new FileStream(Server.MapPath(@"\Templates\PIFStat.xlsx"), FileMode.Open, FileAccess.Read);
        XSSFWorkbook workbook = new XSSFWorkbook(fs);
        ISheet sheet = workbook.GetSheet("Лист1");
        IRow row = null;
        ICell cell;
        int ir = 4;
        int c = 0;

        XSSFCellStyle cellStyleNum = (XSSFCellStyle)workbook.CreateCellStyle();
        cellStyleNum.DataFormat = workbook.CreateDataFormat().GetFormat("#,##0.00");
        XSSFCellStyle cellStyleDat = (XSSFCellStyle)workbook.CreateCellStyle();
        cellStyleDat.DataFormat = workbook.CreateDataFormat().GetFormat("dd.mm.yyyy");

        row = sheet.CreateRow(1);
        cell = row.CreateCell(0);
        cell.SetCellValue(d1 == null ? "" : d1.Value.Date.ToShortDateString());
        cell.CellStyle = cellStyleDat;

        cell = row.CreateCell(1);
        cell.SetCellValue(d2 == null ? "" : d2.Value.Date.ToShortDateString());
        cell.CellStyle = cellStyleDat;

        var q = repRepository.RepPIFStat(d1, d2);
        foreach (var r in q)
        {
          c = 0;
          row = sheet.CreateRow(ir++);
          cell = row.CreateCell(c++);
          cell.SetCellValue(r.PIF);

          row.CreateCell(c++).SetCellValue(r.AccCnt ?? 0);
          row.CreateCell(c++).SetCellValue(r.AccCntDep ?? 0);
          row.CreateCell(c++).SetCellValue(r.p0 ?? 0);
          row.CreateCell(c++).SetCellValue(r.p1 ?? 0);
          row.CreateCell(c++).SetCellValue(r.p2 ?? 0);
          row.CreateCell(c++).SetCellValue(r.p3 ?? 0);
          row.CreateCell(c++).SetCellValue(r.p4 ?? 0);
          row.CreateCell(c++).SetCellValue(r.p5 ?? 0);
          row.CreateCell(c++).SetCellValue(r.r0 ?? 0);
          row.CreateCell(c++).SetCellValue(r.r00 ?? 0);
          row.CreateCell(c++).SetCellValue(r.r01 ?? 0);
          row.CreateCell(c++).SetCellValue(r.r1 ?? 0);
          row.CreateCell(c++).SetCellValue(r.r11 ?? 0);
          row.CreateCell(c++).SetCellValue(r.s0 ?? 0);
          row.CreateCell(c++).SetCellValue(r.s01 ?? 0);
          row.CreateCell(c++).SetCellValue(r.d0 ?? 0);
          row.CreateCell(c++).SetCellValue(r.d00 ?? 0);

        }
        sheet = workbook.GetSheet("Лист2");
        ir = 1;
        var q1 = repRepository.RepPIFStat1(d1, d2);
        foreach (var r in q1)
        {
          c = 0;
          row = sheet.CreateRow(ir++);
          row.CreateCell(c++).SetCellValue(r.PIF);
          row.CreateCell(c++).SetCellValue(r.Client);
          row.CreateCell(c++).SetCellValue(r.Days ?? 0);
          row.CreateCell(c++).SetCellValue((double)(r.Rest ?? 0));
          row.CreateCell(c++).SetCellValue(r.Dep ?? 0);
          row.CreateCell(c++).SetCellValue(r.Type1);
          row.CreateCell(c++).SetCellValue(r.Type2);
          row.CreateCell(c++).SetCellValue(r.Type3);
        }
        MemoryStream ms = new MemoryStream();
        workbook.Write(ms);
        return File(ms.ToArray(), "application/vnd.ms-excel", string.Format("ps{0}.xlsx", DateTime.Now.Second));
      }
      catch (Exception ex)
      {
      }
      return View("Error");
    }

    private void RepFinHeader(DateTime? de, bool? withDog, IXLWorksheet worksheet)
    {
      var m = de.Value.Month;
      worksheet.ShowGridLines = false;
      worksheet.Style.Font.FontName = "Arial";
      worksheet.Style.Font.SetFontSize(8);
      worksheet.Row(3).Height = 45;
      worksheet.Row(4).Height = 45;

      worksheet.Column("B").Width = 5.5;
      worksheet.Column("C").Width = 18.5;
      worksheet.Column("D").Width = 47;
      worksheet.Column("E").Width = 9;
      worksheet.Column("F").Width = withDog == true ? 39 : 0;
      if (withDog == false) worksheet.Column("F").Collapse();
      worksheet.Column("G").Width = 15;
      worksheet.Column("H").Width = 8.57;
      worksheet.Columns("I:AI").Width = 12.86;
      worksheet.Column("AJ").Width = 50;

      var range = worksheet.Range("B3:H4");
      range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
      range.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
      range.Style.Alignment.SetWrapText(true);

      worksheet.Cell("B3").Value = "статья верхнего уровня (3 значения)/ \nСтатья второго уровня (4 значения)";
      range = worksheet.Range("B3:C4").Merge();

      worksheet.Cell("D3").Value = "контрагент (из поля получатель)";
      range = worksheet.Range("D3:D4").Merge();

      worksheet.Cell("AI3").Value = "Выполнение плана, %";
      range = worksheet.Range("AI3:AI4");
      range.Merge();
      range.Style.Alignment.SetWrapText(true);

      worksheet.Cell("AJ3").Value = "услуга (экономическое содержание)";
      range = worksheet.Range("AJ3:AJ4").Merge();

      worksheet.Cell("e3").Value = "тип платежа";
      range = worksheet.Range("e3:e4").Merge();

      worksheet.Cell("F3").Value = "договор";
      range = worksheet.Range("F3:F4").Merge();

      worksheet.Cell("G3").Value = "Периодичность";
      range = worksheet.Range("G3:G4").Merge();

      worksheet.Cell("H3").Value = "Т/Р";
      range = worksheet.Range("H3:H4").Merge();

      var mnth = new string[] { "январь", "февраль", "март", "апрель", "май", "июнь", "июль", "август", "сентябрь", "октябрь", "ноябрь", "декабрь", de.Value.Year.ToString() };
      for (var im = 0; im <= 12; im++)
      {
        worksheet.Range(worksheet.Cell(3, 9 + im * 2), worksheet.Cell(3, 9 + im * 2 + 1)).Merge();
        worksheet.Cell(3, 9 + im * 2).Value = mnth[im];
        worksheet.Cell(4, 9 + im * 2).Value = "План";
        worksheet.Cell(4, 9 + im * 2 + 1).Value = "Факт";
      }

      range = worksheet.Range("i3:AJ4");
      range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
      range.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

      range = worksheet.Range("B3:AJ5");
      range.Style.Fill.BackgroundColor = XLColor.FromArgb(216, 228, 188);
      var border = range.Style.Border;
      border.OutsideBorder = XLBorderStyleValues.Thin;
      border.OutsideBorderColor = XLColor.Black;
      border.InsideBorder = XLBorderStyleValues.Thin;

    }

    private int RepFinBody5(DateTime? de, bool? withDog, string FinInstID, IXLWorksheet worksheet)
    {
      var m = de.Value.Month;
      var q = repRepository.RepFin5(de, withDog, FinInstID);
      RepFinHeader(de, withDog, worksheet);
      int i = 5;
      int g1 = 0;
      foreach (var r in q)
      {
        if (r.gItemID1 == 1)
        {
          worksheet.Range(i, 2, i, 8).Merge();
          worksheet.Cell(i, 2).Value = "Всего";
        }
        else if (r.gItemID2 == 1)
        {
          worksheet.Cell(i, 3).Value = r.Item1;
          worksheet.Range(i, 3, i, 8).Merge();
          worksheet.Range(i, 2, i, 36).Style.Fill.BackgroundColor = XLColor.FromArgb(255, 255, 153);
          if (g1 != 0 && g1 <= i - 1)
            worksheet.Rows(g1, i - 1).Group();
          g1 = i + 1;
        }
        else if (r.gItemID3 == 1)
        {
          worksheet.Cell(i, 2).Value = r.Item2b;
          worksheet.Cell(i, 3).Value = r.Item2;
          worksheet.Range(i, 3, i, 8).Merge();
          worksheet.Range(i, 2, i, 36).Style.Fill.BackgroundColor = XLColor.FromArgb(253, 233, 217);
        }
        else if (r.gPeriodicityID == 1)
        {
          worksheet.Cell(i, 2).Value = r.Item3b;
          worksheet.Cell(i, 3).Value = r.Item3;
          worksheet.Range(i, 3, i, 8).Merge();
          worksheet.Range(i, 2, i, 36).Style.Fill.BackgroundColor = XLColor.FromArgb(233, 217, 253);
        }
        else
        {
          worksheet.Cell(i, 4).Value = r.Receiver;
          //worksheet.Cell(i, 5).Value = r.Name;
          worksheet.Cell(i, 36).Value = r.Comment;
          worksheet.Cell(i, 6).Value = r.Dogovor;
          worksheet.Cell(i, 7).Value = r.Name;
          worksheet.Cell(i, 8).Value = r.tr;
        }
        if (r.gItemID1 == 1 || r.gItemID2 == 1 || r.gItemID3 == 1 || r.gPeriodicityID == 1)
        {
          worksheet.Cell(i, 35).FormulaR1C1 = "=RC34/RC33*100";
        }
        worksheet.Cell(i, 9).Value = (double?)r.p1;
        worksheet.Cell(i, 10).Value = r.f1;
        worksheet.Cell(i, 11).Value = (double?)r.p2;
        worksheet.Cell(i, 12).Value = r.f2;
        worksheet.Cell(i, 13).Value = (double?)r.p3;
        worksheet.Cell(i, 14).Value = r.f3;
        worksheet.Cell(i, 15).Value = (double?)r.p4;
        worksheet.Cell(i, 16).Value = r.f4;
        worksheet.Cell(i, 17).Value = (double?)r.p5;
        worksheet.Cell(i, 18).Value = r.f5;
        worksheet.Cell(i, 19).Value = (double?)r.p6;
        worksheet.Cell(i, 20).Value = r.f6;
        worksheet.Cell(i, 21).Value = (double?)r.p7;
        worksheet.Cell(i, 22).Value = r.f7;
        worksheet.Cell(i, 23).Value = (double?)r.p8;
        worksheet.Cell(i, 24).Value = r.f8;
        worksheet.Cell(i, 25).Value = (double?)r.p9;
        worksheet.Cell(i, 26).Value = r.f9;
        worksheet.Cell(i, 27).Value = (double?)r.p10;
        worksheet.Cell(i, 28).Value = r.f10;
        worksheet.Cell(i, 29).Value = (double?)r.p11;
        worksheet.Cell(i, 30).Value = r.f11;
        worksheet.Cell(i, 31).Value = (double?)r.p12;
        worksheet.Cell(i, 32).Value = r.f12;
        i++;
      }
      if (g1 != 0 && g1 <= i - 1)
        worksheet.Rows(g1, i - 1).Group();
      worksheet.CollapseRows();
      return i;
    }

    private int RepFinBody6(DateTime? de, bool? withDog, string FinInstID, IXLWorksheet worksheet)
    {
      var m = de.Value.Month;
      var q = repRepository.RepFin6(de, withDog, FinInstID);

      RepFinHeader(de, withDog, worksheet);
      int i = 5;
      int g = 0;
      foreach (var r in q)
      {
        if (r.gItemID1 == 1)
        {
          worksheet.Range(i, 2, i, 8).Merge();
          worksheet.Cell(i, 2).Value = "Всего";
        }
        else if (r.gItemID2 == 1)
        {
          worksheet.Cell(i, 3).Value = r.Item1;
          worksheet.Range(i, 3, i, 8).Merge();
          worksheet.Range(i, 2, i, 36).Style.Fill.BackgroundColor = XLColor.FromArgb(255, 255, 153);
          if (g != 0 && g <= i - 1)
          {
            worksheet.Rows(g, i - 1).Group();
            worksheet.Rows(g, i - 1).Collapse();
          }
          g = i + 1;
        }
        else if (r.gItemID3 == 1)
        {
          worksheet.Cell(i, 2).Value = r.Item2b;
          worksheet.Cell(i, 3).Value = r.Item2;
          worksheet.Range(i, 3, i, 8).Merge();
          worksheet.Range(i, 2, i, 36).Style.Fill.BackgroundColor = XLColor.FromArgb(253, 233, 217);
        }
        else if (r.gPeriodicityID == 1)
        {
          worksheet.Cell(i, 2).Value = r.Item3b;
          worksheet.Cell(i, 3).Value = r.Item3;
          worksheet.Range(i, 3, i, 8).Merge();
          worksheet.Range(i, 2, i, 36).Style.Fill.BackgroundColor = XLColor.FromArgb(233, 217, 253);
        }
        else
        {
          worksheet.Cell(i, 4).Value = r.Receiver;
          //worksheet.Cell(i, 5).Value = r.Name;
          worksheet.Cell(i, 36).Value = r.Comment;
          worksheet.Cell(i, 6).Value = r.Dogovor;
          worksheet.Cell(i, 7).Value = r.Name;
          worksheet.Cell(i, 8).Value = r.tr;
        }
        if (r.gItemID1 == 1 || r.gItemID2 == 1 || r.gItemID3 == 1 || r.gPeriodicityID == 1)
        {
          worksheet.Cell(i, 35).FormulaR1C1 = "=RC34/RC33*100";
        }
        worksheet.Cell(i, 9).Value = (double?)r.p1;
        worksheet.Cell(i, 10).Value = r.f1;
        worksheet.Cell(i, 11).Value = (double?)r.p2;
        worksheet.Cell(i, 12).Value = r.f2;
        worksheet.Cell(i, 13).Value = (double?)r.p3;
        worksheet.Cell(i, 14).Value = r.f3;
        worksheet.Cell(i, 15).Value = (double?)r.p4;
        worksheet.Cell(i, 16).Value = r.f4;
        worksheet.Cell(i, 17).Value = (double?)r.p5;
        worksheet.Cell(i, 18).Value = r.f5;
        worksheet.Cell(i, 19).Value = (double?)r.p6;
        worksheet.Cell(i, 20).Value = r.f6;
        worksheet.Cell(i, 21).Value = (double?)r.p7;
        worksheet.Cell(i, 22).Value = r.f7;
        worksheet.Cell(i, 23).Value = (double?)r.p8;
        worksheet.Cell(i, 24).Value = r.f8;
        worksheet.Cell(i, 25).Value = (double?)r.p9;
        worksheet.Cell(i, 26).Value = r.f9;
        worksheet.Cell(i, 27).Value = (double?)r.p10;
        worksheet.Cell(i, 28).Value = r.f10;
        worksheet.Cell(i, 29).Value = (double?)r.p11;
        worksheet.Cell(i, 30).Value = r.f11;
        worksheet.Cell(i, 31).Value = (double?)r.p12;
        worksheet.Cell(i, 32).Value = r.f12;
        i++;
      }
      if (g != 0 && g <= i - 1)
      {
        worksheet.Rows(g, i - 1).Group();
        worksheet.Rows(g, i - 1).Collapse();
      }
      return i;
    }

    [Authorize(Roles = "mo")]
    public ActionResult RepFin()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);
      ViewBag.Title = "Отчет об исполнении бюджета по контрагентам";
      return View();
    }

    [ActionName("RepFin")]
    [Authorize(Roles = "mo")]
    [HttpPost]
    public ActionResult RepFinExcel(DateTime? de, bool? withDog, string FinInstID)
    {
      if (!Regex.IsMatch(FinInstID, "^(((\\d+)(,\\d+)*)|(\\d*))$"))
        return View("Error");

      if (!de.HasValue)
        return View("Error");
      var m = de.Value.Month;
      try
      {
        var workbook = new XLWorkbook();
        for (var sh = 1; sh <= 2; sh++)
        {
          var worksheet = workbook.Worksheets.Add(sh == 1 ? "по дате оплаты" : "по дате учета");

          int i;
          if (sh == 1)
            i = RepFinBody5(de, withDog, FinInstID, worksheet);
          else
            i = RepFinBody6(de, withDog, FinInstID, worksheet);
          StringBuilder sb = new StringBuilder();
          sb.Append("=RC10");
          for (var im = 2; im <= m; im++)
          {
            sb.AppendFormat("+RC{0}", im * 2 + 8);
          }
          worksheet.Range(5, 33, i - 1, 33).FormulaR1C1 = "=RC9+RC11+RC13+RC15+RC17+RC19+RC21+RC23+RC25+RC27+RC29+RC31";
          worksheet.Range(5, 34, i - 1, 34).FormulaR1C1 = sb.ToString();

          var border = worksheet.Range(5, 2, i - 1, 36).Style.Border;
          border.OutsideBorder = XLBorderStyleValues.Thin;
          border.InsideBorder = XLBorderStyleValues.Thin;

          worksheet.Range(5, 9, i - 1, 34).Style.NumberFormat.NumberFormatId = 4;
          var range = worksheet.Range(5, 35, i - 1, 35);
          range.Style.NumberFormat.NumberFormatId = 3;
          var style = range.AddConditionalFormat().WhenGreaterThan(100);
          //style.Font.FontColor = XLColor.Red;
          style.Font.FontColor = XLColor.FromArgb(255, 255, 0, 0);
          style.Fill.PatternType = XLFillPatternValues.None;
          worksheet.Range(5, 2, i - 1, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
          worksheet.Range(5, 4, i - 1, 7).Style.Alignment.WrapText = true;
          worksheet.Range(5, 36, i - 1, 36).Style.Alignment.WrapText = true;
          worksheet.Columns(m * 2 + 9, 32).Group();
          worksheet.Columns(m * 2 + 9, 32).Collapse();
        }
        MemoryStream ms = new MemoryStream();
        workbook.SaveAs(ms);
        return File(ms.ToArray(), "application/vnd.ms-excel", string.Format("rf{0}.xlsx", DateTime.Now.Second));
      }
      catch (Exception ex)
      {
      }
      return View("Error");
    }

    [Authorize(Roles = "admin,mo")]
    public ActionResult FileUpload(int? id, HttpPostedFileBase fn)
    {
      return FileUploadBase(0, id, fn);
    }

    [Authorize(Roles = "admin,mo")]
    public ActionResult FileUploadPP(int id, HttpPostedFileBase fn)
    {
      var res = FileUploadBase(1, id, fn);
      repRepository.OrdPaymUpdFilePP(id, Path.Combine(DateTime.Today.ToString("yy"), string.Format("{0}_{1}", id, Path.GetFileName(fn.FileName))));
      return res;
    }

    [Authorize(Roles = "admin,mo")]
    public ActionResult FileUploadPP1(int id, HttpPostedFileBase fn)
    {
      return FileUploadBase(1, id, fn);
    }

    [Authorize(Roles = "admin,mo")]
    public ActionResult FileUploadS(int? id, HttpPostedFileBase fn)
    {
      return FileUploadBase(2, id, fn);
    }

    [Authorize(Roles = "admin,mo")]
    public ActionResult FileUploadCD(int? id, HttpPostedFileBase fn)
    {
      return FileUploadBase(3, id, fn);
    }

    private ActionResult FileUploadBase(int type, int? id, HttpPostedFileBase fn)
    {
      //var path1 = @"\\fc.uralsibbank.ru\uralsib\MSK\COMMON\VOL1\ASSETS\";
      var path1 = @"\\am-uralsib.ru\uralsib\MSK\COMMON\VOL1\ASSETS\";
      if (fn != null && fn.ContentLength > 0)
      {
        var prefix =
          type == 0 ? Path.Combine(path1, @"CorpEvents\OrdPaym") :
          type == 1 ? Path.Combine(path1, @"CorpEvents\OrdPaymPP") :
          type == 2 ? Path.Combine(path1, @"CorpEvents\OrdPaymS") :
          type == 3 ? Path.Combine(path1, @"CorpEvents\OrdPaymCD") :
          type == 4 ? Path.Combine(path1, @"CorpEvents\OrdPaymCh") :
          "";
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

    [Authorize]
    public ActionResult GetFile(string data)
    {
      return GetFileBase(0, data);
    }

    public ActionResult GetFilePP(string data)
    {
      return GetFileBase(1, data);
    }

    public ActionResult GetFileS(string data)
    {
      return GetFileBase(2, data);
    }

    public ActionResult GetFileCD(string data)
    {
      return GetFileBase(3, data);
    }

    private ActionResult GetFileBase(int type, string data)
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
      var path1 = @"\\am-uralsib.ru\uralsib\MSK\COMMON\VOL1\ASSETS\";
      var prefix =
        type == 0 ? Path.Combine(path1, @"CorpEvents\OrdPaym") :
        type == 1 ? Path.Combine(path1, @"CorpEvents\OrdPaymPP") :
        type == 2 ? Path.Combine(path1, @"CorpEvents\OrdPaymS") :
        type == 3 ? Path.Combine(path1, @"CorpEvents\OrdPaymCD") :
        "";
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

    [Authorize(Roles = "mo")]
    public ActionResult OrderPaymConfList(int? id)
    {
      return new JsonnResult { Data = new { success = true, data = repRepository.GetOrderPaymConf(id) } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult OrderPaymConfCreate(List<tOrdPaymentConf> data)
    {
      var q = repRepository.OrderPaymConfCreate(data);
      return new JsonnResult { Data = new { success = true, message = "Сохранено", data = q } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult OrderPaymConfUpdate(List<tOrdPaymentConf> data)
    {
      var q = repRepository.OrderPaymConfUpdate(data);
      return new JsonnResult { Data = new { success = true, message = "Сохранено", data = q } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult OrderPaymConfDel(List<tOrdPaymentConf> data)
    {
      if (repRepository.OrderPaymConfDel(data))
        return Json(new { success = true });
      else
        return Json(new { success = false, msg = "Ошибка при удалении" });
    }

    [Authorize(Roles = "mo")]
    public ActionResult ordPaymState()
    {
      return new JsonnResult { Data = new { success = true, data = repRepository.GetObjClsByParentID(48759).Select(p => new { id = p.NameBrief, p.Text }) } };
    }

    [Authorize(Roles = "pai")]
    public ActionResult RepKM1()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);
      ViewBag.Title = "Отчет \"КМ Статус/Приоритет\")";
      return View("RepPIFOrderStat");
    }

    [Authorize(Roles = "pai")]
    [ActionName("RepKM1")]
    [HttpPost]
    public ActionResult RepKM1Excel(DateTime? d1, DateTime? d2)
    {
      try
      {
        var workbook = new XLWorkbook();
        var q = repRepository.getRepKM1(d1, d2);
        var worksheet = workbook.Worksheets.Add("КМ");

        worksheet.Column(1).Width = 40;
        worksheet.Column(2).Width = 15;
        worksheet.Column(3).Width = 15;
        worksheet.Column(4).Width = 15;
        worksheet.Column(5).Width = 15;
        worksheet.Cell(1, 1).Value = "Период";
        worksheet.Cell(1, 2).Value = d1;
        worksheet.Cell(1, 3).Value = d2;
        worksheet.Cell(1, 4).Value = "по дате заявки";
        worksheet.Cell(2, 1).Value = "ФИО продавца";
        worksheet.Cell(2, 2).Value = "Брутто-приток";
        worksheet.Cell(2, 3).Value = "Брутто-отток";
        worksheet.Cell(2, 4).Value = "Сальдо";
        worksheet.Cell(2, 5).Value = "Статус/Приоритет";

        var range = worksheet.Range(2, 1, 2, 5);
        range.Style.Fill.BackgroundColor = XLColor.FromArgb(233, 217, 253);
        range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        range.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        range.Style.Alignment.SetWrapText(true);
        range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        var i = 3;
        foreach (var r in q)
        {
          worksheet.Cell(i, 1).Value = r.Seller;
          worksheet.Cell(i, 2).Value = r.QtyP;
          worksheet.Cell(i, 3).Value = r.QtyR;
          worksheet.Cell(i, 4).FormulaR1C1 = "=RC[-2]-RC[-1]";
          worksheet.Cell(i, 5).Value = r.Act;
          i++;
        }
        worksheet.Range(3, 2, i - 1, 4).Style.NumberFormat.NumberFormatId = 4;
        MemoryStream ms = new MemoryStream();
        workbook.SaveAs(ms);
        return File(ms.ToArray(), "application/vnd.ms-excel", string.Format("rf{0}.xlsx", DateTime.Now.Second));
      }
      catch (Exception ex)
      {
      }
      return View("Error");
    }

    [Authorize(Roles = "paib")]
    public ActionResult RepUralsibCommis()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);
      ViewBag.Title = "Отчет по скидкам-надбавкам банка";
      return View("RepPIFOrderStat");
    }

    [Authorize(Roles = "paib")]
    [ActionName("RepUralsibCommis")]
    [HttpPost]
    public ActionResult RepUralsibCommisExcel(DateTime d1, DateTime d2)
    {
      try
      {
        var q = repRepository.RepUralsibCommis(d1, d2);
        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Банк");
        worksheet.Style.Font.FontName = "Arial";
        worksheet.Style.Font.SetFontSize(8);

        worksheet.Column(1).Width = 12;
        worksheet.Column(2).Width = 14.14;
        worksheet.Column(3).Width = 22.86;
        worksheet.Column(4).Width = 48.29;
        worksheet.Column(5).Width = 16.57;
        worksheet.Column(1).Width = 12;
        worksheet.Column(6).Width = 9;
        worksheet.Column(7).Width = 32.43;
        worksheet.Column(8).Width = 10;
        worksheet.Column(9).Width = 30.43;
        worksheet.Column(10).Width = 11;
        worksheet.Column(11).Width = 11;
        worksheet.Column(12).Width = 12.86;
        worksheet.Column(13).Width = 10;
        worksheet.Column(14).Width = 10;
        worksheet.Column(15).Width = 61.43;
        worksheet.Column(16).Width = 8;

        worksheet.Column(18).Width = 8;
        worksheet.Column(19).Width = 5;
        worksheet.Column(20).Width = 11.43;
        worksheet.Column(21).Width = 3;
        worksheet.Column(22).Width = 24;
        worksheet.Cell(1, 1).Value = "Период";
        worksheet.Cell(1, 2).Value = d1;
        worksheet.Cell(1, 3).Value = d2;
        worksheet.Cell(2, 1).Value = "Дата заявки";
        worksheet.Cell(2, 2).Value = "Код отделения";
        worksheet.Cell(2, 3).Value = "Дирекция";
        worksheet.Cell(2, 4).Value = "Точка продаж";
        worksheet.Cell(2, 5).Value = "Управляющий";
        worksheet.Cell(2, 6).Value = "ТабНомер продавца";
        worksheet.Cell(2, 7).Value = "Продавец";
        worksheet.Cell(2, 8).Value = "Группа";
        worksheet.Cell(2, 9).Value = "ПИФ";
        worksheet.Cell(2, 10).Value = "Сумма Привлечения";
        worksheet.Cell(2, 11).Value = "Сумма Оттока";
        worksheet.Cell(2, 12).Value = "Скидка/Надбавка";
        worksheet.Cell(2, 13).Value = "ID Diasoft";
        worksheet.Cell(2, 14).Value = "ID IR";
        worksheet.Cell(2, 15).Value = "ФИО пайщика";
        worksheet.Cell(2, 16).Value = "Дата рождения";
        worksheet.Cell(2, 18).Value = "Комиссия";
        worksheet.Cell(2, 19).Value = "Серия";
        worksheet.Cell(2, 20).Value = "Номер";
        worksheet.Cell(2, 21).Value = "Пол";
        worksheet.Cell(2, 22).Value = "Сокращение";
        worksheet.Cell(2, 23).Value = "Телефон";
        worksheet.Cell(2, 24).Value = "Номер заявки";
        var range = worksheet.Range(2, 1, 2, 24);
        range.Style.Fill.BackgroundColor = XLColor.FromArgb(233, 217, 253);
        range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        range.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        range.Style.Alignment.SetWrapText(true);
        range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        var i = 3;
        foreach (var r in q)
        {
          worksheet.Cell(i, 1).Value = r.Дата_заявки;
          worksheet.Cell(i, 2).Value = r.Код_отделения;
          worksheet.Cell(i, 3).Value = r.Дирекция;
          worksheet.Cell(i, 4).Value = r.Точка_продаж;
          worksheet.Cell(i, 5).Value = r.Управляющий;
          worksheet.Cell(i, 6).SetValue(r.ТабНомер);
          worksheet.Cell(i, 7).Value = r.Продавец;
          worksheet.Cell(i, 8).Value = r.Группа;
          worksheet.Cell(i, 9).Value = r.ПИФ;
          worksheet.Cell(i, 10).Value = r.Сумма_Привлечения;
          worksheet.Cell(i, 11).Value = r.Сумма_Оттока;

          worksheet.Cell(i, 12).Value = r.СкидкаНадбавка;
          worksheet.Cell(i, 13).Value = r.ID_Diasoft;
          worksheet.Cell(i, 14).Value = r.ID_IR;
          worksheet.Cell(i, 15).Value = r.ФИО_пайщика;
          worksheet.Cell(i, 16).SetValue(r.BirthDay);

          worksheet.Cell(i, 18).Value = r.Комиссия;
          worksheet.Cell(i, 19).SetValue(r.RegSeries);
          worksheet.Cell(i, 20).SetValue(r.RegNumber);
          worksheet.Cell(i, 21).SetValue(r.Sex);
          worksheet.Cell(i, 22).SetValue(r.Brief);
          worksheet.Cell(i, 23).SetValue(r.Phone1);
          worksheet.Cell(i, 24).SetValue(r.Number);
          i++;
        }
        worksheet.Range(3, 10, i - 1, 12).Style.NumberFormat.NumberFormatId = 4;
        worksheet.Range(3, 18, i - 1, 18).Style.NumberFormat.NumberFormatId = 4;
        worksheet.Columns(17, 23).Group(false);
        worksheet.CollapseColumns();
        MemoryStream ms = new MemoryStream();
        workbook.SaveAs(ms);
        return File(ms.ToArray(), "application/vnd.ms-excel", string.Format("rf{0}.xlsx", DateTime.Now.Second));
      }
      catch (Exception ex)
      {
      }
      return View("Error");
    }

    [Authorize(Roles = "paib")]
    public ActionResult RepUralsibCommisLK()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);
      ViewBag.Title = "Отчет по скидкам-надбавкам банка ЛК";
      return View("RepPIFOrderStat");
    }

    [Authorize(Roles = "paib")]
    [ActionName("RepUralsibCommisLK")]
    [HttpPost]
    public ActionResult RepUralsibCommisLKExcel(DateTime d1, DateTime d2)
    {
      try
      {
      var q = repRepository.RepUralsibCommisLK(d1, d2);
        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Банк");
        worksheet.Style.Font.FontName = "Arial";
        worksheet.Style.Font.SetFontSize(8);

        worksheet.Column(1).Width = 12;
        worksheet.Column(2).Width = 14.14;
        worksheet.Column(3).Width = 22.86;
        worksheet.Column(4).Width = 48.29;
        worksheet.Column(5).Width = 16.57;
        worksheet.Column(1).Width = 12;
        worksheet.Column(6).Width = 9;
        worksheet.Column(7).Width = 32.43;
        worksheet.Column(8).Width = 10;
        worksheet.Column(9).Width = 30.43;
        worksheet.Column(10).Width = 11;
        worksheet.Column(11).Width = 11;
        worksheet.Column(12).Width = 12.86;
        worksheet.Column(13).Width = 10;
        worksheet.Column(14).Width = 10;
        worksheet.Column(15).Width = 61.43;
        worksheet.Column(16).Width = 8;

        worksheet.Column(18).Width = 8;
        worksheet.Column(19).Width = 5;
        worksheet.Column(20).Width = 11.43;
        worksheet.Column(21).Width = 3;
        worksheet.Column(22).Width = 24;
        worksheet.Cell(1, 1).Value = "Период";
        worksheet.Cell(1, 2).Value = d1;
        worksheet.Cell(1, 3).Value = d2;
        worksheet.Cell(2, 1).Value = "Дата заявки";
        worksheet.Cell(2, 2).Value = "Код отделения";
        worksheet.Cell(2, 3).Value = "Дирекция";
        worksheet.Cell(2, 4).Value = "Точка продаж";
        worksheet.Cell(2, 5).Value = "Управляющий";
        worksheet.Cell(2, 6).Value = "ТабНомер продавца";
        worksheet.Cell(2, 7).Value = "Продавец";
        worksheet.Cell(2, 8).Value = "Группа";
        worksheet.Cell(2, 9).Value = "ПИФ";
        worksheet.Cell(2, 10).Value = "Сумма Привлечения";
        worksheet.Cell(2, 11).Value = "Сумма Оттока";
        worksheet.Cell(2, 12).Value = "Скидка/Надбавка";
        worksheet.Cell(2, 13).Value = "ID Diasoft";
        worksheet.Cell(2, 14).Value = "ID IR";
        worksheet.Cell(2, 15).Value = "ФИО пайщика";
        worksheet.Cell(2, 16).Value = "Дата рождения";
        worksheet.Cell(2, 18).Value = "Комиссия";
        worksheet.Cell(2, 19).Value = "Серия";
        worksheet.Cell(2, 20).Value = "Номер";
        worksheet.Cell(2, 21).Value = "Пол";
        worksheet.Cell(2, 22).Value = "Сокращение";
        worksheet.Cell(2, 23).Value = "Телефон";
        worksheet.Cell(2, 24).Value = "Номер заявки";
        var range = worksheet.Range(2, 1, 2, 24);
        range.Style.Fill.BackgroundColor = XLColor.FromArgb(233, 217, 253);
        range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        range.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        range.Style.Alignment.SetWrapText(true);
        range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        var i = 3;
        foreach (var r in q)
        {
          worksheet.Cell(i, 1).Value = r.Дата_заявки;
          worksheet.Cell(i, 2).Value = r.Код_отделения;
          worksheet.Cell(i, 3).Value = r.Дирекция;
          worksheet.Cell(i, 4).Value = r.Точка_продаж;
          worksheet.Cell(i, 5).Value = r.Управляющий;
          worksheet.Cell(i, 6).SetValue(r.ТабНомер);
          worksheet.Cell(i, 7).Value = r.Продавец;
          worksheet.Cell(i, 8).Value = r.Группа;
          worksheet.Cell(i, 9).Value = r.ПИФ;
          worksheet.Cell(i, 10).Value = r.Сумма_Привлечения;
          worksheet.Cell(i, 11).Value = r.Сумма_Оттока;

          worksheet.Cell(i, 12).Value = r.СкидкаНадбавка;
          worksheet.Cell(i, 13).Value = r.ID_Diasoft;
          worksheet.Cell(i, 14).Value = r.ID_IR;
          worksheet.Cell(i, 15).Value = r.ФИО_пайщика;
          worksheet.Cell(i, 16).SetValue(r.BirthDay);

          worksheet.Cell(i, 18).Value = r.Комиссия;
          worksheet.Cell(i, 19).SetValue(r.RegSeries);
          worksheet.Cell(i, 20).SetValue(r.RegNumber);
          worksheet.Cell(i, 21).SetValue(r.Sex);
          worksheet.Cell(i, 22).SetValue(r.Brief);
          worksheet.Cell(i, 23).SetValue(r.Phone1);
          worksheet.Cell(i, 24).SetValue(r.Number);
          i++;
        }
        worksheet.Range(3, 10, i - 1, 12).Style.NumberFormat.NumberFormatId = 4;
        worksheet.Range(3, 18, i - 1, 18).Style.NumberFormat.NumberFormatId = 4;
        worksheet.Columns(17, 23).Group(false);
        worksheet.CollapseColumns();
        MemoryStream ms = new MemoryStream();
        workbook.SaveAs(ms);
        return File(ms.ToArray(), "application/vnd.ms-excel", string.Format("rf{0}.xlsx", DateTime.Now.Second));
      }
      catch (Exception ex)
      {
      }
      return View("Error");
    }

    [Authorize(Roles = "paib")]
    public ActionResult RepUralsibInOut()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);
      ViewBag.Title = "Отчет по притокам-оттокам  банка";
      return View("RepPIFOrderStat");
    }

    [Authorize(Roles = "paib")]
    [ActionName("RepUralsibInOut")]
    [HttpPost]
    public ActionResult RepUralsibInOutExcel(DateTime d1, DateTime d2)
    {
      try
      {
        var q = repRepository.RepUralsibInOut(d1, d2);
        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Банк");
        worksheet.Style.Font.FontName = "Arial";
        worksheet.Style.Font.SetFontSize(8);

        worksheet.Column(1).Width = 12;
        worksheet.Column(2).Width = 14.14;
        worksheet.Column(3).Width = 22.86;
        worksheet.Column(4).Width = 48.29;
        worksheet.Column(5).Width = 16.57;
        worksheet.Column(1).Width = 12;
        worksheet.Column(6).Width = 9;
        worksheet.Column(7).Width = 32.43;
        worksheet.Column(8).Width = 10;
        worksheet.Column(9).Width = 30.43;
        worksheet.Column(10).Width = 11;
        worksheet.Column(11).Width = 11;
        worksheet.Column(12).Width = 12.86;
        worksheet.Column(13).Width = 10;
        worksheet.Column(14).Width = 10;
        worksheet.Column(15).Width = 61.43;
        worksheet.Column(16).Width = 8;

        //worksheet.Column(18).Width = 8;
        worksheet.Column(18).Width = 5;
        worksheet.Column(19).Width = 11.43;
        worksheet.Column(20).Width = 3;
        worksheet.Column(21).Width = 24;
        worksheet.Cell(1, 1).Value = "Период";
        worksheet.Cell(1, 2).Value = d1;
        worksheet.Cell(1, 3).Value = d2;
        worksheet.Cell(2, 1).Value = "Дата заявки";
        worksheet.Cell(2, 2).Value = "Код отделения";
        worksheet.Cell(2, 3).Value = "Дирекция";
        worksheet.Cell(2, 4).Value = "Точка продаж";
        worksheet.Cell(2, 5).Value = "Управляющий";
        worksheet.Cell(2, 6).Value = "ТабНомер продавца";
        worksheet.Cell(2, 7).Value = "Продавец";
        worksheet.Cell(2, 8).Value = "Группа";
        worksheet.Cell(2, 9).Value = "ПИФ";
        worksheet.Cell(2, 10).Value = "Сумма Привлечения";
        worksheet.Cell(2, 11).Value = "Сумма Оттока";
        worksheet.Cell(2, 12).Value = "Скидка/Надбавка";
        worksheet.Cell(2, 13).Value = "ID Diasoft";
        worksheet.Cell(2, 14).Value = "ID IR";
        worksheet.Cell(2, 15).Value = "ФИО пайщика";
        worksheet.Cell(2, 16).Value = "Дата рождения";
        //worksheet.Cell(2, 18).Value = "Комиссия";
        worksheet.Cell(2, 18).Value = "Серия";
        worksheet.Cell(2, 19).Value = "Номер";
        worksheet.Cell(2, 20).Value = "Пол";
        worksheet.Cell(2, 21).Value = "Сокращение";
        worksheet.Cell(2, 22).Value = "Номер заявки";
        var range = worksheet.Range(2, 1, 2, 22);
        range.Style.Fill.BackgroundColor = XLColor.FromArgb(233, 217, 253);
        range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        range.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        range.Style.Alignment.SetWrapText(true);
        range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        var i = 3;
        foreach (var r in q)
        {
          worksheet.Cell(i, 1).Value = r.Дата_заявки;
          worksheet.Cell(i, 2).Value = r.Код_отделения;
          worksheet.Cell(i, 3).Value = r.Дирекция;
          worksheet.Cell(i, 4).Value = r.Точка_продаж;
          worksheet.Cell(i, 5).Value = r.Управляющий;
          worksheet.Cell(i, 6).SetValue(r.ТабНомер);
          worksheet.Cell(i, 7).Value = r.Продавец;
          worksheet.Cell(i, 8).Value = r.Группа;
          worksheet.Cell(i, 9).Value = r.ПИФ;
          worksheet.Cell(i, 10).Value = r.Сумма_Привлечения;
          worksheet.Cell(i, 11).Value = r.Сумма_Оттока;

          worksheet.Cell(i, 12).Value = r.СкидкаНадбавка;
          worksheet.Cell(i, 13).Value = r.ID_Diasoft;
          worksheet.Cell(i, 14).Value = r.ID_IR;
          worksheet.Cell(i, 15).Value = r.ФИО_пайщика;
          worksheet.Cell(i, 16).SetValue(r.BirthDay);

          //worksheet.Cell(i, 18).Value = r.Комиссия;
          worksheet.Cell(i, 18).SetValue(r.RegSeries);
          worksheet.Cell(i, 19).SetValue(r.RegNumber);
          worksheet.Cell(i, 20).SetValue(r.Sex);
          worksheet.Cell(i, 21).SetValue(r.Brief);
          worksheet.Cell(i, 22).SetValue(r.Number);
          i++;
        }
        worksheet.Range(3, 10, i - 1, 11).Style.NumberFormat.NumberFormatId = 4;
        //worksheet.Range(3, 18, i - 1, 18).Style.NumberFormat.NumberFormatId = 4;
        worksheet.Columns(17, 21).Group(false);
        worksheet.CollapseColumns();
        MemoryStream ms = new MemoryStream();
        workbook.SaveAs(ms);
        return File(ms.ToArray(), "application/vnd.ms-excel", string.Format("rf{0}.xlsx", DateTime.Now.Second));
      }
      catch (Exception ex)
      {
      }
      return View("Error");
    }

    [Authorize(Roles = "pai")]
    public ActionResult RepDURests()
    {
      ViewBag.Title = "Отчет по остаткам на договорах ДУ";
      return View("RepPIFOrders");
    }

    [Authorize(Roles = "pai")]
    [ActionName("RepDURests")]
    [HttpPost]
    public ActionResult RepDURestsExcel(DateTime d)
    {
      try
      {
        var q = repRepository.getRepDURests(d);
        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("ДУ");
        worksheet.Style.Font.FontName = "Arial";
        worksheet.Style.Font.SetFontSize(8);

        worksheet.Column(1).Width = 34;
        worksheet.Column(2).Width = 12;
        worksheet.Column(3).Width = 18.29;
        worksheet.Column(4).Width = 15;
        worksheet.Column(5).Width = 11.14;
        worksheet.Column(6).Width = 21;
        worksheet.Column(7).Width = 36;
        worksheet.Column(8).Width = 4;
        worksheet.Column(9).Width = 4;
        worksheet.Column(10).Width = 4;
        worksheet.Column(11).Width = 36;
        worksheet.Column(12).Width = 12;
        worksheet.Column(13).Width = 4;
        worksheet.Column(14).Width = 60;
        worksheet.Column(15).Width = 18.71;
        worksheet.Column(16).Width = 13;
        worksheet.Column(18).Width = 12;
        worksheet.Column(19).Width = 18.29;

        worksheet.Cell(1, 1).Value = "На дату";
        worksheet.Cell(1, 2).SetValue(d);
        worksheet.Cell(2, 1).Value = "Клиент";
        worksheet.Cell(2, 2).Value = "Сумма";
        worksheet.Cell(2, 3).Value = "Номер";
        worksheet.Cell(2, 4).Value = "Телефон";
        worksheet.Cell(2, 5).Value = "Дата договора";
        worksheet.Cell(2, 6).Value = "Почта";
        worksheet.Cell(2, 7).Value = "Стратегия";
        worksheet.Cell(2, 8).Value = "MF";
        worksheet.Cell(2, 9).Value = "SF";
        worksheet.Cell(2, 10).Value = "OF";
        worksheet.Cell(2, 11).Value = "Клиентский менеджер";
        worksheet.Cell(2, 12).Value = "Ввод ДС при заключении договора ДУ";
        worksheet.Cell(2, 13).Value = "ППЗ";
        worksheet.Cell(2, 14).Value = "ДО";
        worksheet.Cell(2, 15).Value = "ТД";
        worksheet.Cell(2, 16).Value = "Тип менеджера";
        worksheet.Cell(2, 17).Value = "Кластер";
        worksheet.Cell(2, 18).Value = "Дата окончания";
        worksheet.Cell(2, 19).Value = "Номер договора";
        worksheet.Cell(2, 20).Value = "Дата рождения";
        worksheet.Cell(2, 21).Value = "Ввод ДС после заключении договора ДУ";
        var range = worksheet.Range(2, 1, 2, 21);
        range.Style.Fill.BackgroundColor = XLColor.FromArgb(233, 217, 253);
        range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        range.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        range.Style.Alignment.SetWrapText(true);
        range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        var i = 3;
        foreach (var r in q)
        {
          worksheet.Cell(i, 1).SetValue(r.Client ?? "");
          worksheet.Cell(i, 2).SetValue(r.Qty);
          worksheet.Cell(i, 3).SetValue(r.Number ?? "");
          worksheet.Cell(i, 4).SetValue(r.Phone1 ?? "");
          worksheet.Cell(i, 5).SetValue(r.DateStart);
          worksheet.Cell(i, 6).SetValue(r.Email ?? "");
          worksheet.Cell(i, 7).Value = r.Strategy;
          worksheet.Cell(i, 8).Value = r.MF;
          worksheet.Cell(i, 9).Value = r.SF;
          worksheet.Cell(i, 10).Value = r.OF;
          worksheet.Cell(i, 11).Value = r.ClManager ?? "";
          worksheet.Cell(i, 11).Style.Alignment.SetWrapText(true);
          worksheet.Cell(i, 12).SetValue(r.QtyB);
          worksheet.Cell(i, 13).Value = r.PPZ ?? "";
          worksheet.Cell(i, 14).Value = r.DO ?? "";
          worksheet.Cell(i, 14).Style.Alignment.SetWrapText(true);
          worksheet.Cell(i, 15).Value = r.TD ?? "";
          worksheet.Cell(i, 15).Style.Alignment.SetWrapText(true);
          worksheet.Cell(i, 16).Value = r.StPr;
          worksheet.Cell(i, 17).Value = r.Cluster2;
          worksheet.Cell(i, 18).Value = (r.DateFinish == new DateTime(1900, 1, 1) ? "" : string.Format("{0:dd.MM.yyyy}", r.DateFinish));
          worksheet.Cell(i, 19).SetValue(r.trNumber ?? "");
          worksheet.Cell(i, 20).Value = (r.BirthDay == null ? "" : string.Format("{0:dd.MM.yyyy}", r.BirthDay));
          worksheet.Cell(i, 21).Value = r.QtyN;

          i++;
        }
        worksheet.Range(3, 2, i - 1, 2).Style.NumberFormat.NumberFormatId = 4;
        worksheet.Range(3, 12, i - 1, 12).Style.NumberFormat.NumberFormatId = 4;
        worksheet.Range(3, 21, i - 1, 21).Style.NumberFormat.NumberFormatId = 4;
        MemoryStream ms = new MemoryStream();
        workbook.SaveAs(ms);
        return File(ms.ToArray(), "application/vnd.ms-excel", string.Format("du{0}.xlsx", DateTime.Now.Second));
      }
      catch (Exception ex)
      {
      }
      return View("Error");
    }

    [Authorize(Roles = "pai,bank")]
    public ActionResult RepDUBank()
    {
      ViewBag.Title = "Отчет \"Акт сверки ДУ\"";
      return View("RepPIFOrderStat");
    }

    [Authorize(Roles = "pai,bank")]
    [ActionName("RepDUBank")]
    [HttpPost]
    public ActionResult RepDUBankExcel(DateTime d1, DateTime d2)
    {
      return RepDUExcel(d1, d2, new int[] { 1, 2 }, new int[] { 0 });
    }

    [Authorize(Roles = "pai")]
    public ActionResult RepDU()
    {
      ViewBag.Title = "Отчет \"Акт сверки ДУ\"";
      return View("RepPIFOrderStat");
    }

    [Authorize(Roles = "pai")]
    [ActionName("RepDU")]
    [HttpPost]
    public ActionResult RepDUExcel(DateTime d1, DateTime d2)
    {
      return RepDUExcel(d1, d2, new int[] { 3 }, new int[] { 1 });
    }

    [Authorize(Roles = "pai,bank")]
    public ActionResult RepDUBankCh()
    {
      ViewBag.Title = "Отчет \"Акт сверки ДУ\"";
      return View("RepPIFOrderStat");
    }

    [Authorize(Roles = "pai,bank")]
    [ActionName("RepDUBankCh")]
    [HttpPost]
    public ActionResult RepDUBankChExcel(DateTime d1, DateTime d2)
    {
      return RepDUExcel(d1, d2, new int[] { 4, 5 }, new int[] { 0 });
    }

    [Authorize(Roles = "pai,bank")]
    [ActionName("RepDUBankChRep")]
    [HttpPost]
    public ActionResult RepDUBankChRepExcel(DateTime d1, DateTime d2)
    {
      return RepDUExcel(d1, d2, new int[] { 4 }, new int[] { 0 });
    }

    private ActionResult RepDUExcel(DateTime d1, DateTime d2, int[] reptype, int[] iotype)
    {
      try
      {
        var workbook = new XLWorkbook();
        foreach (var type in reptype /*new int[] { 1, 2 }*/)
        {
          var q = repRepository.getRepDUBank(d1, d2, type);
          var worksheet = workbook.Worksheets.Add("Акт сверки ДУ" + (type == 2 || type == 5 ? " ИИС" : ""));
          worksheet.Style.Font.FontName = "Arial";
          worksheet.Style.Font.SetFontSize(8);

          worksheet.Column(1).Width = 12;
          worksheet.Column(2).Width = 12;
          worksheet.Column(3).Width = 32;
          worksheet.Column(4).Width = 11.6;
          worksheet.Column(5).Width = 11.14;
          worksheet.Column(6).Width = 13;
          worksheet.Column(7).Width = 36;
          worksheet.Column(8).Width = 4;
          worksheet.Column(9).Width = 9;
          worksheet.Column(10).Width = 4;
          worksheet.Column(11).Width = 9;
          worksheet.Column(12).Width = 4;
          worksheet.Column(13).Width = 9;
          worksheet.Column(14).Width = 30;
          worksheet.Column(15).Width = 4;
          worksheet.Column(16).Width = 60;
          worksheet.Column(17).Width = 18.71;
          worksheet.Column(18).Width = 13;
          worksheet.Column(19).Width = 11;
          worksheet.Column(20).Width = 11;
          worksheet.Column(21).Width = 13;
          worksheet.Column(22).Width = 13;
          worksheet.Column(23).Width = 11;
          worksheet.Column(24).Width = 11;
          worksheet.Column(25).Width = 13;
          worksheet.Column(26).Width = 13;
          worksheet.Column(27).Width = 6;
          worksheet.Column(28).Width = 25;
          worksheet.Column(29).Width = 15;
          worksheet.Column(31).Width = 12;
          worksheet.Column(34).Width = 11;
          worksheet.Column(35).Width = 11;
          worksheet.Cell(1, 1).Value = "Начало периода";
          worksheet.Cell(1, 2).Value = "Конец периода";
          worksheet.Cell(1, 3).Value = "Клиент";
          worksheet.Cell(1, 4).Value = "Дата рождения";
          worksheet.Cell(1, 5).Value = "Дата договора";
          worksheet.Cell(1, 6).Value = "Номер";
          worksheet.Cell(1, 7).Value = "Стратегия";
          worksheet.Cell(1, 8).Value = "MF,%";
          worksheet.Cell(1, 9).Value = "MF";
          worksheet.Cell(1, 10).Value = "SF,%";
          worksheet.Cell(1, 11).Value = "SF";
          worksheet.Cell(1, 12).Value = "OF,%";
          worksheet.Cell(1, 13).Value = "OF";
          worksheet.Cell(1, 14).Value = "Клиентский менеджер";
          worksheet.Cell(1, 15).Value = "ППЗ";
          worksheet.Cell(1, 16).Value = "ДО";
          worksheet.Cell(1, 17).Value = "ТД";
          worksheet.Cell(1, 18).Value = "Входящая стоимость";
          worksheet.Cell(1, 19).Value = "Притоки";
          worksheet.Cell(1, 20).Value = "Оттоки";
          worksheet.Cell(1, 21).Value = "Исходящая стоимость";
          worksheet.Cell(1, 22).Value = "Средняя стоимость";
          worksheet.Cell(1, 23).Value = "ТабНомер";
          worksheet.Cell(1, 24).Value = "ID IR";
          worksheet.Cell(1, 25).Value = "Тип менеджера";
          worksheet.Cell(1, 26).Value = "ФинРез";
          worksheet.Cell(1, 27).Value = "Портал";
          worksheet.Cell(1, 28).Value = "Код клиента";
          worksheet.Cell(1, 29).Value = "Управляющий";
          worksheet.Cell(1, 30).Value = "Кластер";
          worksheet.Cell(1, 31).Value = "Дата окончания";
          worksheet.Cell(1, 32).Value = "Курс валюты договора";
          worksheet.Cell(1, 33).Value = "Валюта договора";
          worksheet.Cell(1, 34).Value = "Приток в руб.";
          worksheet.Cell(1, 35).Value = "Отток в руб.";

          var range = worksheet.Range(1, 1, 1, 35);
          range.Style.Fill.BackgroundColor = XLColor.FromArgb(233, 217, 253);
          range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
          range.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
          range.Style.Alignment.SetWrapText(true);
          range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
          range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
          var i = 2;
          foreach (var r in q)
          {
            worksheet.Cell(i, 1).SetValue(r.d1);
            worksheet.Cell(i, 2).SetValue(r.d2);
            worksheet.Cell(i, 3).SetValue(r.Client ?? "");
            worksheet.Cell(i, 4).Value = (r.BirthDay);
            worksheet.Cell(i, 5).SetValue(r.DateStart);
            worksheet.Cell(i, 6).Value = r.Number ?? "";
            worksheet.Cell(i, 7).Value = r.Strategy ?? "";
            worksheet.Cell(i, 8).Value = r.MF;
            worksheet.Cell(i, 9).Value = r.MFV;
            worksheet.Cell(i, 10).Value = r.SF;
            worksheet.Cell(i, 11).FormulaR1C1 = "=MAX(RC[15],0)*RC[-1]/100";
            worksheet.Cell(i, 12).Value = r.OF;
            worksheet.Cell(i, 13).Value = r.OFV;
            worksheet.Cell(i, 14).Value = r.ClManager ?? "";
            worksheet.Cell(i, 14).Style.Alignment.SetWrapText(true);
            worksheet.Cell(i, 15).Value = r.PPZ ?? "";
            worksheet.Cell(i, 16).Value = r.DO ?? "";
            worksheet.Cell(i, 16).Style.Alignment.SetWrapText(true);
            worksheet.Cell(i, 17).Value = r.TD ?? "";
            worksheet.Cell(i, 17).Style.Alignment.SetWrapText(true);
            worksheet.Cell(i, 18).Value = r.Qtyp * r.FundCourse;
            worksheet.Cell(i, 19).Value = r.Income * r.FundCourse;
            worksheet.Cell(i, 20).Value = r.Outcome * r.FundCourse;
            worksheet.Cell(i, 21).Value = r.Qty * r.FundCourse;
            worksheet.Cell(i, 22).Value = r.QtyAvg * r.FundCourse;
            worksheet.Cell(i, 23).SetValue(r.TabNum ?? "");
            worksheet.Cell(i, 24).Value = r.ExternalID;
            worksheet.Cell(i, 25).Value = r.StPr;
            worksheet.Cell(i, 26).FormulaR1C1 = "=(-RC[-8]-RC[-7]-RC[-6]+RC[-5])";
            worksheet.Cell(i, 27).Value = r.Portal;
            worksheet.Cell(i, 28).Value = r.ClientCode;
            worksheet.Cell(i, 29).Value = r.UPR;
            worksheet.Cell(i, 30).Value = r.Cluster2;
            worksheet.Cell(i, 31).Value = (r.DateFinish == new DateTime(1900, 1, 1) ? "" : string.Format("{0:dd.MM.yyyy}", r.DateFinish));
            worksheet.Cell(i, 32).Value = r.FundCourse;
            worksheet.Cell(i, 33).Value = r.Fund;
            worksheet.Cell(i, 34).Value = r.IncomeR;
            worksheet.Cell(i, 35).Value = r.OutcomeR;
            i++;
          }
          worksheet.Range(2, 9, i - 1, 9).Style.NumberFormat.NumberFormatId = 4;
          worksheet.Range(2, 11, i - 1, 11).Style.NumberFormat.NumberFormatId = 4;
          worksheet.Range(2, 13, i - 1, 13).Style.NumberFormat.NumberFormatId = 4;
          worksheet.Range(2, 18, i - 1, 22).Style.NumberFormat.NumberFormatId = 4;
          worksheet.Range(2, 26, i - 1, 26).Style.NumberFormat.NumberFormatId = 4;
          worksheet.Range(2, 34, i - 1, 35).Style.NumberFormat.NumberFormatId = 4;
        }
        foreach (var type in iotype)
        {
          var q = repRepository.getRepDUInOut(d1, d2, type);
          var worksheet = workbook.Worksheets.Add("Ввод вывод ДУ");
          worksheet.Style.Font.FontName = "Arial";
          worksheet.Style.Font.SetFontSize(8);

          worksheet.Column(1).Width = 9;
          worksheet.Column(2).Width = 39;
          worksheet.Column(3).Width = 9;
          worksheet.Column(4).Width = 9;
          worksheet.Column(5).Width = 13.29;
          worksheet.Column(6).Width = 33;
          worksheet.Column(7).Width = 36;
          worksheet.Column(8).Width = 4;
          worksheet.Column(9).Width = 38;
          worksheet.Column(10).Width = 19;
          worksheet.Column(11).Width = 9.3;
          worksheet.Column(12).Width = 10.14;
          worksheet.Column(13).Width = 7.57;
          worksheet.Column(14).Width = 9.71;
          worksheet.Column(15).Width = 8.3;
          worksheet.Column(16).Width = 5.57;
          worksheet.Column(17).Width = 15.14;
          worksheet.Column(18).Width = 9;
          worksheet.Column(19).Width = 7;
          worksheet.Column(20).Width = 10.14;
          worksheet.Column(21).Width = 9.71;
          worksheet.Column(22).Width = 9.71;
          worksheet.Cell(1, 1).Value = "Дата";
          worksheet.Cell(1, 2).Value = "Клиент";
          worksheet.Cell(1, 3).Value = "Дата рождения";
          worksheet.Cell(1, 4).Value = "Дата договора";
          worksheet.Cell(1, 5).Value = "Номер";
          worksheet.Cell(1, 6).Value = "Стратегия";
          worksheet.Cell(1, 7).Value = "Клиентский менеджер";
          worksheet.Cell(1, 8).Value = "ППЗ";
          worksheet.Cell(1, 9).Value = "ДО";
          worksheet.Cell(1, 10).Value = "ТД";
          worksheet.Cell(1, 11).Value = "Ввод Вывод";
          worksheet.Cell(1, 12).Value = "Сумма в руб.";
          worksheet.Cell(1, 13).Value = "ТабНомер";
          worksheet.Cell(1, 14).Value = "ID IR";
          worksheet.Cell(1, 15).Value = "Тип менеджера";
          worksheet.Cell(1, 16).Value = "Портал";
          worksheet.Cell(1, 17).Value = "Код клиента";
          worksheet.Cell(1, 18).Value = "Дата окончания";
          worksheet.Cell(1, 19).Value = "Курс валюты";
          worksheet.Cell(1, 20).Value = "Сумма";
          worksheet.Cell(1, 21).Value = "Валюта";
          worksheet.Cell(1, 22).Value = "ClientID";

          var range = worksheet.Range(1, 1, 1, 22);
          range.Style.Fill.BackgroundColor = XLColor.FromArgb(233, 217, 253);
          range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
          range.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
          range.Style.Alignment.SetWrapText(true);
          range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
          range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
          var i = 2;
          foreach (var r in q)
          {
            worksheet.Cell(i, 1).SetValue(r.DealDate);
            worksheet.Cell(i, 2).SetValue(r.Client ?? "");
            worksheet.Cell(i, 3).Value = (r.BirthDay);
            worksheet.Cell(i, 4).SetValue(r.DateStart);
            worksheet.Cell(i, 5).Value = r.Number ?? "";
            worksheet.Cell(i, 6).Value = r.Strategy ?? "";
            worksheet.Cell(i, 7).Value = r.ClManager ?? "";
            worksheet.Cell(i, 7).Style.Alignment.SetWrapText(true);
            worksheet.Cell(i, 8).Value = r.PPZ ?? "";
            worksheet.Cell(i, 9).Value = r.DO ?? "";
            worksheet.Cell(i, 9).Style.Alignment.SetWrapText(true);
            worksheet.Cell(i, 10).Value = r.TD ?? "";
            worksheet.Cell(i, 10).Style.Alignment.SetWrapText(true);
            worksheet.Cell(i, 11).SetValue(r.BS ?? "");
            worksheet.Cell(i, 12).Value = r.Qty * r.FundCourse;
            worksheet.Cell(i, 13).SetValue(r.TabNum ?? "");
            worksheet.Cell(i, 14).Value = r.ExternalID;
            worksheet.Cell(i, 15).Value = r.StPr;
            worksheet.Cell(i, 16).Value = r.Portal;
            worksheet.Cell(i, 17).Value = r.ClientCode;
            worksheet.Cell(i, 18).Value = (r.DateFinish == new DateTime(1900, 1, 1) ? "" : string.Format("{0:dd.MM.yyyy}", r.DateFinish));
            worksheet.Cell(i, 19).Value = r.FundCourse;
            worksheet.Cell(i, 20).Value = r.Qty;
            worksheet.Cell(i, 21).Value = ((string)r.Fund).TrimEnd();
            worksheet.Cell(i, 22).Value = r.ClientID;
            i++;
          }
          worksheet.Range(2, 12, i - 1, 12).Style.NumberFormat.NumberFormatId = 4;
          worksheet.Range(2, 20, i - 1, 20).Style.NumberFormat.NumberFormatId = 4;
        }
        MemoryStream ms = new MemoryStream();
        workbook.SaveAs(ms);
        return File(ms.ToArray(), "application/vnd.ms-excel", string.Format("actdu{0}.xlsx", DateTime.Now.Second));
      }
      catch (Exception ex)
      {
      }
      return View("Error");
    }

    private ActionResult RepDUInOutExcel(DateTime d1, DateTime d2, int[] reptype)
    {
      try
      {
        var workbook = new XLWorkbook();
        foreach (var type in reptype /*new int[] { 1, 2 }*/)
        {
        }
        MemoryStream ms = new MemoryStream();
        workbook.SaveAs(ms);
        return File(ms.ToArray(), "application/vnd.ms-excel", string.Format("actdu{0}.xlsx", DateTime.Now.Second));
      }
      catch (Exception ex)
      {
      }
      return View("Error");
    }

    [Authorize(Roles = "pai")]
    public ActionResult RepPifBankBuy()
    {
      ViewBag.Title = "Отчет \"Покупки ПИФ Банк\"";
      return View("RepPIFOrderStat");
    }

    [Authorize(Roles = "pai")]
    [ActionName("RepPifBankBuy")]
    [HttpPost]
    public ActionResult RepPifBankBuyExcel(DateTime d1, DateTime d2)
    {
      try
      {
        var workbook = new XLWorkbook();
        var q = repRepository.getPifBankBuy(d1, d2);
        var worksheet = workbook.Worksheets.Add("Покупки ПИФ Банк");
        worksheet.Style.Font.FontName = "Arial";
        worksheet.Style.Font.SetFontSize(8);

        worksheet.Column(1).Width = 25;
        worksheet.Column(2).Width = 41;
        worksheet.Column(3).Width = 11;
        worksheet.Column(4).Width = 7;
        worksheet.Column(5).Width = 4.5;
        worksheet.Column(6).Width = 19;
        worksheet.Column(7).Width = 31;
        worksheet.Column(8).Width = 14;
        worksheet.Column(9).Width = 8.5;
        worksheet.Column(10).Width = 9.6;
        worksheet.Column(11).Width = 9;
        worksheet.Cell(1, 1).SetValue(d1);
        worksheet.Cell(1, 2).SetValue(d2);
        worksheet.Cell(2, 1).Value = "Сокращение";
        worksheet.Cell(2, 2).Value = "Пайщик";
        worksheet.Cell(2, 3).Value = "Сумма";
        worksheet.Cell(2, 4).Value = "ПИФ";
        worksheet.Cell(2, 5).Value = "ППЗ";
        worksheet.Cell(2, 6).Value = "ТД";
        worksheet.Cell(2, 7).Value = "Продавец";
        worksheet.Cell(2, 8).Value = "Телефон";
        worksheet.Cell(2, 9).Value = "Заявка";
        worksheet.Cell(2, 10).Value = "Номер";
        worksheet.Cell(2, 11).Value = "Дата рождения";

        var range = worksheet.Range(2, 1, 2, 11);
        range.Style.Fill.BackgroundColor = XLColor.FromArgb(233, 217, 253);
        range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        range.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        range.Style.Alignment.SetWrapText(true);
        range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        var i = 3;
        foreach (var r in q)
        {
          worksheet.Cell(i, 1).Value = r.NameBrief ?? "";
          worksheet.Cell(i, 2).Value = r.Name ?? "";
          worksheet.Cell(i, 3).SetValue(r.Qty);
          worksheet.Cell(i, 4).Value = r.PIF;
          worksheet.Cell(i, 5).SetValue(r.PPZCode);
          worksheet.Cell(i, 6).Value = r.TD ?? "";
          worksheet.Cell(i, 7).Value = r.Seller ?? "";
          worksheet.Cell(i, 8).SetValue(r.Phone1 ?? "");
          worksheet.Cell(i, 9).Value = r.DealDate;
          worksheet.Cell(i, 10).Value = r.Number ?? "";
          if (r.BirthDay != null)
            worksheet.Cell(i, 11).SetValue(r.BirthDay);
          i++;
        }
        worksheet.Range(3, 3, i - 1, 3).Style.NumberFormat.NumberFormatId = 4;
        MemoryStream ms = new MemoryStream();
        workbook.SaveAs(ms);
        return File(ms.ToArray(), "application/vnd.ms-excel", string.Format("pifbuy{0}.xlsx", DateTime.Now.Second));
      }
      catch (Exception ex)
      {
      }
      return View("Error");
    }

    [Authorize(Roles = "pai")]
    public ActionResult RepPifRepmnt()
    {
      ViewBag.Title = "Отчет \"Погашения ПИФ\"";
      return View("RepPIFOrderStat");
    }

    [Authorize(Roles = "pai")]
    [ActionName("RepPifRepmnt")]
    [HttpPost]
    public ActionResult RepPifRepmntExcel(DateTime d1, DateTime d2)
    {
      try
      {
        var workbook = new XLWorkbook();
        var q = repRepository.getPifRepmt(d1, d2);
        var worksheet = workbook.Worksheets.Add("Погашения ПИФ");
        worksheet.Style.Font.FontName = "Arial";
        worksheet.Style.Font.SetFontSize(8);

        worksheet.Column(1).Width = 25;
        worksheet.Column(2).Width = 41;
        worksheet.Column(3).Width = 11;
        worksheet.Column(4).Width = 7;
        worksheet.Column(5).Width = 5.14;
        worksheet.Column(6).Width = 12.5;
        worksheet.Column(7).Width = 8.5;
        worksheet.Column(8).Width = 9.6;
        worksheet.Column(9).Width = 9;
        worksheet.Cell(1, 1).SetValue(d1);
        worksheet.Cell(1, 2).SetValue(d2);
        worksheet.Cell(2, 1).Value = "Сокращение";
        worksheet.Cell(2, 2).Value = "Пайщик";
        worksheet.Cell(2, 3).Value = "Сумма";
        worksheet.Cell(2, 4).Value = "ПИФ";
        worksheet.Cell(2, 5).Value = "Канал";
        worksheet.Cell(2, 6).Value = "Телефон";
        worksheet.Cell(2, 7).Value = "Заявка";
        worksheet.Cell(2, 8).Value = "Номер";
        worksheet.Cell(2, 9).Value = "Дата рождения";

        var range = worksheet.Range(2, 1, 2, 9);
        range.Style.Fill.BackgroundColor = XLColor.FromArgb(233, 217, 253);
        range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        range.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        range.Style.Alignment.SetWrapText(true);
        range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        var i = 3;
        foreach (var r in q)
        {
          worksheet.Cell(i, 1).Value = r.NameBrief ?? "";
          worksheet.Cell(i, 2).Value = r.Name ?? "";
          worksheet.Cell(i, 3).SetValue(r.Qty);
          worksheet.Cell(i, 4).Value = r.PIF;
          worksheet.Cell(i, 5).Value = r.Chanel;
          worksheet.Cell(i, 6).SetValue(r.Phone1 ?? "");
          worksheet.Cell(i, 7).Value = r.DealDate;
          worksheet.Cell(i, 8).Value = r.Number ?? "";
          if (r.BirthDay != null)
            worksheet.Cell(i, 9).SetValue(r.BirthDay);
          i++;
        }
        worksheet.Range(3, 3, i - 1, 3).Style.NumberFormat.NumberFormatId = 4;
        MemoryStream ms = new MemoryStream();
        workbook.SaveAs(ms);
        return File(ms.ToArray(), "application/vnd.ms-excel", string.Format("pifs{0}.xlsx", DateTime.Now.Second));
      }
      catch (Exception ex)
      {
      }
      return View("Error");
    }

    public ActionResult PostPifers(string q)
    {
      return new JsonnResult { Data = new { data = repRepository.GetPifers(q) } };
    }

    public ActionResult getValRate(int Id, decimal q)
    {
      return new JsonnResult { Data = new { data = repRepository.GetValRate(Id, q) } };
    }

    [Authorize(Roles = "mo")]
    public ActionResult Rep4BCS1()
    {
      ViewBag.Title = "Отчет ЕНО БКС Реестр";
      return View("RepPIFOrderStat");
    }

    [Authorize(Roles = "mo")]
    [ActionName("Rep4BCS1")]
    [HttpPost]
    public ActionResult Rep4BCS1Excel(DateTime d1, DateTime d2)
    {
      try
      {
        var q = repRepository.Rep4BCS1(d1, d2);
        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Реестр");
        worksheet.Style.Font.FontName = "Arial";
        worksheet.Style.Font.SetFontSize(8);

        worksheet.Column(1).Width = 10;
        worksheet.Column(2).Width = 55;
        worksheet.Column(3).Width = 44;

        worksheet.Cell(1, 1).Value = string.Format("Классификатор клиентов на {0:dd.MM.yyyy} года", d2);
        worksheet.Cell(2, 1).Value = "ID Клиента";
        worksheet.Cell(2, 2).Value = "ФИО клиента";
        worksheet.Cell(2, 3).Value = "Продукт";
        worksheet.Cell(2, 4).Value = "Тип";
        var range = worksheet.Range(2, 1, 2, 4);
        range.Style.Fill.BackgroundColor = XLColor.FromArgb(233, 217, 253);
        range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        range.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        range.Style.Alignment.SetWrapText(true);
        range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        var i = 3;
        foreach (var r in q)
        {
          worksheet.Cell(i, 1).SetValue(r.ID);
          worksheet.Cell(i, 2).SetValue(r.Name);
          worksheet.Cell(i, 3).SetValue(r.Product);
          worksheet.Cell(i, 4).SetValue(r.IsJuridicalPerson == 1 ? "ЮЛ" : "ФЛ");
          i++;
        }
        MemoryStream ms = new MemoryStream();
        workbook.SaveAs(ms);
        return File(ms.ToArray(), "application/vnd.ms-excel", string.Format("bcs1_{0}.xlsx", DateTime.Now.Second));
      }
      catch (Exception ex)
      {
      }
      return View("Error");
    }

    [Authorize(Roles = "mo")]
    public ActionResult Rep4BCS2()
    {
      ViewBag.Title = "Отчет ЕНО БКС Остатки";
      return View("RepPIFOrders");
    }

    [Authorize(Roles = "mo")]
    [ActionName("Rep4BCS2")]
    [HttpPost]
    public ActionResult Rep4BCS2Excel(DateTime d)
    {
      try
      {
        var q = repRepository.Rep4BCS2(d);
        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Остатки");
        worksheet.Style.Font.FontName = "Arial";
        worksheet.Style.Font.SetFontSize(8);

        worksheet.Column(1).Width = 10;
        worksheet.Column(2).Width = 55;
        worksheet.Column(3).Width = 44;
        worksheet.Column(4).Width = 12.57;
        worksheet.Column(5).Width = 9;
        worksheet.Column(6).Width = 9;
        worksheet.Column(7).Width = 12;
        worksheet.Column(8).Width = 6.3;
        worksheet.Column(9).Width = 12;

        worksheet.Cell(1, 1).Value = string.Format("АпУ в расшифровке по активам на {0:dd.MM.yyyy} года", d);
        worksheet.Cell(2, 1).Value = "ID Клиента";
        worksheet.Cell(2, 2).Value = "Продукт";
        worksheet.Cell(2, 3).Value = "Актив";
        worksheet.Cell(2, 4).Value = "ISIN";
        worksheet.Cell(2, 5).Value = "Остаток";
        worksheet.Cell(2, 6).Value = "Котировка";
        worksheet.Cell(2, 7).Value = "Стоимость в валюте";
        worksheet.Cell(2, 8).Value = "Курс";
        worksheet.Cell(2, 9).Value = "Остаток в рублях";
        worksheet.Cell(2, 10).Value = "Портал";
        var range = worksheet.Range(2, 1, 2, 10);
        range.Style.Fill.BackgroundColor = XLColor.FromArgb(233, 217, 253);
        range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        range.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        range.Style.Alignment.SetWrapText(true);
        range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        var i = 3;
        foreach (var r in q)
        {
          worksheet.Cell(i, 1).SetValue(r.ID);
          worksheet.Cell(i, 2).SetValue(r.Product);
          worksheet.Cell(i, 3).SetValue(r.Active);
          worksheet.Cell(i, 4).SetValue(r.ISIN);
          worksheet.Cell(i, 5).SetValue(r.Rest);
          worksheet.Cell(i, 6).SetValue(r.Course);
          worksheet.Cell(i, 7).SetValue(r.Qty);
          worksheet.Cell(i, 8).SetValue(r.Rate);
          worksheet.Cell(i, 9).SetValue(r.QtyRur);
          worksheet.Cell(i, 10).SetValue(r.Portal);
          i++;
        }
        worksheet.Range(3, 7, i - 1, 7).Style.NumberFormat.NumberFormatId = 4;
        worksheet.Range(3, 9, i - 1, 9).Style.NumberFormat.NumberFormatId = 4;
        MemoryStream ms = new MemoryStream();
        workbook.SaveAs(ms);
        return File(ms.ToArray(), "application/vnd.ms-excel", string.Format("bcs2_{0}.xlsx", DateTime.Now.Second));
      }
      catch (Exception ex)
      {
      }
      return View("Error");
    }

    [Authorize(Roles = "mo")]
    public ActionResult Rep4BCS3()
    {
      ViewBag.Title = "Отчет ЕНО БКС Притоки и оттоки";
      return View("RepPIFOrderStat");
    }

    [Authorize(Roles = "mo")]
    [ActionName("Rep4BCS3")]
    [HttpPost]
    public ActionResult Rep4BCS3Excel(DateTime d1, DateTime d2)
    {
      try
      {
        var q = repRepository.Rep4BCS3(d1, d2);
        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Притоки и оттоки");
        worksheet.Style.Font.FontName = "Arial";
        worksheet.Style.Font.SetFontSize(8);

        worksheet.Column(1).Width = 10;
        worksheet.Column(2).Width = 43.43;
        worksheet.Column(3).Width = 31.43;
        worksheet.Column(4).Width = 11.57;
        worksheet.Column(5).Width = 12.57;
        worksheet.Column(6).Width = 9;
        worksheet.Column(7).Width = 7.14;
        worksheet.Column(8).Width = 12;
        worksheet.Column(9).Width = 6.3;
        worksheet.Column(10).Width = 12;

        worksheet.Cell(1, 1).Value = string.Format("Притоки и оттоки за период с {0:dd.MM.yyyy} по {1:dd.MM.yyyy} года", d1, d2);
        worksheet.Cell(2, 1).Value = "ID Клиента";
        worksheet.Cell(2, 2).Value = "Продукт";
        worksheet.Cell(2, 3).Value = "Актив";
        worksheet.Cell(2, 4).Value = "ISIN";
        worksheet.Cell(2, 5).Value = "Количество";
        worksheet.Cell(2, 6).Value = "Дата операции";
        worksheet.Cell(2, 7).Value = "Валюта операции";
        worksheet.Cell(2, 8).Value = "Сумма в валюте операции";
        worksheet.Cell(2, 9).Value = "Курс";
        worksheet.Cell(2, 10).Value = "Сумма в рублях";
        var range = worksheet.Range(2, 1, 2, 10);
        range.Style.Fill.BackgroundColor = XLColor.FromArgb(233, 217, 253);
        range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        range.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        range.Style.Alignment.SetWrapText(true);
        range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        var i = 3;
        foreach (var r in q)
        {
          worksheet.Cell(i, 1).SetValue(r.ID);
          worksheet.Cell(i, 2).SetValue(r.Product);
          worksheet.Cell(i, 3).SetValue(r.Active);
          worksheet.Cell(i, 4).SetValue(r.ISIN);
          worksheet.Cell(i, 5).SetValue(r.Num);
          worksheet.Cell(i, 6).SetValue(r.Date);
          worksheet.Cell(i, 7).SetValue(r.Fund);
          worksheet.Cell(i, 8).SetValue(r.Qty);
          worksheet.Cell(i, 9).SetValue(r.Rate);
          worksheet.Cell(i, 10).SetValue(r.QtyRur);
          i++;
        }
        worksheet.Range(3, 8, i - 1, 8).Style.NumberFormat.NumberFormatId = 4;
        worksheet.Range(3, 10, i - 1, 10).Style.NumberFormat.NumberFormatId = 4;
        MemoryStream ms = new MemoryStream();
        workbook.SaveAs(ms);
        return File(ms.ToArray(), "application/vnd.ms-excel", string.Format("bcs3_{0}.xlsx", DateTime.Now.Second));
      }
      catch (Exception ex)
      {
      }
      return View("Error");
    }

    [Authorize(Roles = "mo")]
    public ActionResult Rep4BCS4()
    {
      ViewBag.Title = "Отчет ЕНО БКС Начисленные комиссии";
      return View("RepPIFOrderStat");
    }

    [Authorize(Roles = "mo")]
    [ActionName("Rep4BCS4")]
    [HttpPost]
    public ActionResult Rep4BCS4Excel(DateTime d1, DateTime d2)
    {
      try
      {
        var q = repRepository.Rep4BCS4(d1, d2);
        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Начисленные комиссии");
        worksheet.Style.Font.FontName = "Arial";
        worksheet.Style.Font.SetFontSize(8);

        worksheet.Column(1).Width = 10;
        worksheet.Column(2).Width = 40;
        worksheet.Column(3).Width = 28;
        worksheet.Column(4).Width = 12.57;
        worksheet.Column(5).Width = 9;

        worksheet.Cell(1, 1).Value = string.Format("Начисленные комиссии за период с {0:dd.MM.yyyy} по {1:dd.MM.yyyy} года", d1, d2);
        worksheet.Cell(2, 1).Value = "ID Клиента";
        worksheet.Cell(2, 2).Value = "Продукт";
        worksheet.Cell(2, 3).Value = "Тип вознаграждения";
        worksheet.Cell(2, 4).Value = "Сумма комиссии, руб.";
        worksheet.Cell(2, 5).Value = "Дата начисления";
        worksheet.Cell(2, 6).Value = "Портал"; 
        var range = worksheet.Range(2, 1, 2, 6);
        range.Style.Fill.BackgroundColor = XLColor.FromArgb(233, 217, 253);
        range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        range.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        range.Style.Alignment.SetWrapText(true);
        range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        var i = 3;
        foreach (var r in q)
        {
          worksheet.Cell(i, 1).SetValue(r.ID);
          worksheet.Cell(i, 2).SetValue(r.Product);
          worksheet.Cell(i, 3).SetValue(r.Type);
          worksheet.Cell(i, 4).SetValue(r.Qty);
          worksheet.Cell(i, 5).SetValue(r.Date);
          worksheet.Cell(i, 6).SetValue(r.Portal);
          i++;
        }
        worksheet.Range(3, 4, i - 1, 4).Style.NumberFormat.NumberFormatId = 4;
        MemoryStream ms = new MemoryStream();
        workbook.SaveAs(ms);
        return File(ms.ToArray(), "application/vnd.ms-excel", string.Format("bcs4_{0}.xlsx", DateTime.Now.Second));
      }
      catch (Exception ex)
      {
      }
      return View("Error");
    }

    [Authorize(Roles = "mo")]
    public ActionResult Rep4BCS5()
    {
      ViewBag.Title = "Отчет ЕНО БКС по продажам";
      return View("RepPIFOrderStat");
    }

    [Authorize(Roles = "mo")]
    [ActionName("Rep4BCS5")]
    [HttpPost]
    public ActionResult Rep4BCS5Excel(DateTime d1, DateTime d2)
    {
      try
      {
        var q = repRepository.Rep4BCS5(d1, d2);
        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Продажи");
        worksheet.Style.Font.FontName = "Arial";
        worksheet.Style.Font.SetFontSize(8);
        var ii = 1;
        foreach (var c in new double[] { 10, 40, 12.86, 18.71, 8.29, 12, 12 })
          worksheet.Column(ii++).Width = c;

        worksheet.Cell(1, 1).Value = string.Format("Отчет по продажам за период с {0:dd.MM.yyyy} по {1:dd.MM.yyyy} года", d1, d2);
        worksheet.Cell(2, 1).Value = "ID Клиента";
        worksheet.Cell(2, 2).Value = "Продукт";
        worksheet.Cell(2, 3).Value = "Принадлежность клиента";
        worksheet.Cell(2, 4).Value = "Тер. дирекция";
        worksheet.Cell(2, 5).Value = "Дата операции";
        worksheet.Cell(2, 6).Value = "Приток, руб.";
        worksheet.Cell(2, 7).Value = "Отток, руб.";
        var range = worksheet.Range(2, 1, 2, 7);
        range.Style.Fill.BackgroundColor = XLColor.FromArgb(233, 217, 253);
        range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        range.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        range.Style.Alignment.SetWrapText(true);
        range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        var i = 3;
        foreach (var r in q)
        {
          worksheet.Cell(i, 1).SetValue(r.ID);
          worksheet.Cell(i, 2).SetValue(r.Product);
          worksheet.Cell(i, 3).SetValue(r.ClientType);
          worksheet.Cell(i, 4).SetValue(r.TD);
          worksheet.Cell(i, 5).SetValue(r.Date);
          worksheet.Cell(i, 6).SetValue(r.Income);
          worksheet.Cell(i, 7).SetValue(r.Outcome);
          i++;
        }
        worksheet.Range(3, 6, i - 1, 7).Style.NumberFormat.NumberFormatId = 4;
        MemoryStream ms = new MemoryStream();
        workbook.SaveAs(ms);
        return File(ms.ToArray(), "application/vnd.ms-excel", string.Format("bcs5_{0}.xlsx", DateTime.Now.Second));
      }
      catch (Exception ex)
      {
      }
      return View("Error");
    }

    [Authorize(Roles = "mo")]
    public ActionResult Rep4BCS_2()
    {
      ViewBag.Title = "Отчет ЕНО БКС Продажи 2";
      return View("RepPIFOrderStat");
    }

    [Authorize(Roles = "mo")]
    [ActionName("Rep4BCS_2")]
    [HttpPost]
    public ActionResult Rep4BCS_2Excel(DateTime? d1, DateTime? d2)
    {
      try
      {
        var q = repRepository.Rep4BCS_2(d1, d2);
        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Отчет");
        worksheet.Style.Font.FontName = "Arial";
        worksheet.Style.Font.SetFontSize(8);

        var ii = 1;
        foreach (var c in new double[] { 10, 6.29, 53, 4, 25, 19, 15, 8, 5.6, 6.57, 54.57, 10.57, 8.3, 12, 10, 12, 12, 12 })
          worksheet.Column(ii++).Width = c;

        worksheet.Cell(1, 1).Value = string.Format("Отчет с {0:dd.MM.yyyy} по {1:dd.MM.yyyy} года", d1, d2);
        worksheet.Cell(2, 1).Value = "Тип";
        worksheet.Cell(2, 2).Value = "ДУ/ПИФ";
        worksheet.Cell(2, 3).Value = "Продукт";
        worksheet.Cell(2, 4).Value = "ППЗ";
        worksheet.Cell(2, 5).Value = "ДО";
        worksheet.Cell(2, 6).Value = "ТД";
        worksheet.Cell(2, 7).Value = "РД";
        worksheet.Cell(2, 8).Value = "Дата";
        worksheet.Cell(2, 9).Value = "Неделя";
        worksheet.Cell(2, 10).Value = "ГГГГ-ММ";
        worksheet.Cell(2, 11).Value = "Продавец";
        worksheet.Cell(2, 12).Value = "Тип Продавца";
        worksheet.Cell(2, 13).Value = "Вх.кол.клн.";
        worksheet.Cell(2, 14).Value = "Вх.стоимость";
        worksheet.Cell(2, 15).Value = "Исх.кол.клн.";
        worksheet.Cell(2, 16).Value = "Исх.стоимость";
        worksheet.Cell(2, 17).Value = "Приток";
        worksheet.Cell(2, 18).Value = "Отток";

        var range = worksheet.Range(2, 1, 2, 18);
        range.Style.Fill.BackgroundColor = XLColor.FromArgb(233, 217, 253);
        range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        range.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        range.Style.Alignment.SetWrapText(true);
        range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        var i = 3;
        foreach (var r in q)
        {
          worksheet.Cell(i, 1).SetValue(r.Тип);
          worksheet.Cell(i, 2).SetValue(r.ДУ_ПИФ);
          worksheet.Cell(i, 3).SetValue(r.Продукт);
          worksheet.Cell(i, 4).SetValue(r.ППЗ ?? "");
          worksheet.Cell(i, 5).SetValue(r.ДО);
          worksheet.Cell(i, 6).SetValue(r.ТД);
          worksheet.Cell(i, 7).SetValue(r.РД);
          worksheet.Cell(i, 8).SetValue(r.Дата);
          worksheet.Cell(i, 9).SetValue(r.Неделя);
          worksheet.Cell(i, 10).SetValue(r.ГГГГ_ММ);
          worksheet.Cell(i, 11).SetValue(r.Продавец);
          worksheet.Cell(i, 12).SetValue(r.ТипПродавца);
          worksheet.Cell(i, 13).SetValue(r.InNumCln);
          worksheet.Cell(i, 14).SetValue(r.InQty);
          worksheet.Cell(i, 15).SetValue(r.OutNumCln);
          worksheet.Cell(i, 16).SetValue(r.OutQty);
          worksheet.Cell(i, 17).SetValue(r.Приток);
          worksheet.Cell(i, 18).SetValue(r.Отток);
          i++;
        }
        MemoryStream ms = new MemoryStream();
        workbook.SaveAs(ms);
        return File(ms.ToArray(), "application/vnd.ms-excel", "bcs_2.xlsx");
      }
      catch (Exception ex)
      {
      }
      return View("Error");
    }

    public ActionResult Bot()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Bot(string chart_id, HttpPostedFileBase document)
    {
      return View();
    }

    [AllowAnonymous]
    public ActionResult BotChart(string id)
    {
      var de = repRepository.GetLastPifDate();
      var fid = repRepository.GetFinInstID(id);
      var chart = new Chart()
      {
        Width = new System.Web.UI.WebControls.Unit(700, System.Web.UI.WebControls.UnitType.Pixel),
        Palette = ChartColorPalette.EarthTones,
        AntiAliasing = AntiAliasingStyles.All,
        TextAntiAliasingQuality = TextAntiAliasingQuality.SystemDefault,
      };
      //chart.Legends.Add(new Legend { BorderWidth = 0, Alignment = System.Drawing.StringAlignment.Near });
      chart.Titles.Add(new Title { Text = "Динамика цены пая, %", Font = new System.Drawing.Font("Trebuchet MS", 10, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point) });
      chart.ChartAreas.Add(new ChartArea
      {
        AxisY = new Axis
        {
          LabelAutoFitMaxFontSize = 8,
          IntervalAutoMode = IntervalAutoMode.VariableCount,
          IsLabelAutoFit = true,
          MajorGrid = new Grid
          {
            IntervalType = DateTimeIntervalType.Number,
            LineDashStyle = ChartDashStyle.Dot,
            LineColor = System.Drawing.Color.LightGray
          },
          LabelStyle = new LabelStyle { Format = "N1" }
        },
        //AxisY2 = new Axis
        //{
        //  Enabled = AxisEnabled.True,
        //  LabelAutoFitMaxFontSize = 8,
        //  IntervalAutoMode = IntervalAutoMode.VariableCount,
        //  IsLabelAutoFit = true,
        //  MajorGrid = new Grid
        //  {
        //    Enabled = false
        //  },
        //  LabelStyle = new LabelStyle { Format = "N1" }
        //},
        AxisX = new Axis
        {
          IsStartedFromZero = true,
          LabelAutoFitMaxFontSize = 8,
          LabelAutoFitStyle = LabelAutoFitStyles.LabelsAngleStep90,
          IntervalAutoMode = IntervalAutoMode.FixedCount,
          Interval = 1,
          IntervalType = DateTimeIntervalType.Months,
          IsLabelAutoFit = true,
          IsMarginVisible = false,
          MajorGrid = new Grid
          {
            LineDashStyle = ChartDashStyle.Dot,
            //IntervalType = DateTimeIntervalType.Number,
            LineColor = System.Drawing.Color.LightGray
          },
          LabelStyle = new LabelStyle
          {
            Format = "MMM.yy",
            IsStaggered = true,
            Angle = -45
          }
        }
      });
      chart.Series.Add(new Series
      {
        Name = "Фонд",
        ChartType = SeriesChartType.FastLine,
        Color = System.Drawing.Color.BlueViolet,
        XValueMember = "Date2",
        YValueMembers = "coef_all"
      });
      var r = repRepository.GetRepClient22(de.Value.AddDays(1).AddYears(-1), de, fid, 39191, 0);
      chart.Series[0].Points.DataBind(r, "Date2", "coef_all", null);
      var imgStream = new MemoryStream();
      chart.SaveImage(imgStream, ChartImageFormat.Png);
      imgStream.Seek(0, SeekOrigin.Begin);
      return File(imgStream, "image/png");
    }

    [AllowAnonymous]
    public ActionResult BotChartDU(int id)
    {
      var chart = new Chart()
      {
        Width = new System.Web.UI.WebControls.Unit(700, System.Web.UI.WebControls.UnitType.Pixel),
        Palette = ChartColorPalette.EarthTones,
        AntiAliasing = AntiAliasingStyles.All,
        TextAntiAliasingQuality = TextAntiAliasingQuality.SystemDefault,
      };
      chart.Titles.Add(new Title { Text = "Динамика цены, %", Font = new System.Drawing.Font("Trebuchet MS", 10, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point) });
      chart.ChartAreas.Add(new ChartArea
      {
        AxisY = new Axis
        {
          LabelAutoFitMaxFontSize = 8,
          IntervalAutoMode = IntervalAutoMode.VariableCount,
          IsLabelAutoFit = true,
          MajorGrid = new Grid
          {
            IntervalType = DateTimeIntervalType.Number,
            LineDashStyle = ChartDashStyle.Dot,
            LineColor = System.Drawing.Color.LightGray
          },
          LabelStyle = new LabelStyle { Format = "N1" }
        },
        AxisX = new Axis
        {
          IsStartedFromZero = true,
          LabelAutoFitMaxFontSize = 8,
          LabelAutoFitStyle = LabelAutoFitStyles.LabelsAngleStep90,
          IntervalAutoMode = IntervalAutoMode.FixedCount,
          Interval = 1,
          IntervalType = DateTimeIntervalType.Months,
          IsLabelAutoFit = true,
          IsMarginVisible = false,
          MajorGrid = new Grid
          {
            LineDashStyle = ChartDashStyle.Dot,
            //IntervalType = DateTimeIntervalType.Number,
            LineColor = System.Drawing.Color.LightGray
          },
          LabelStyle = new LabelStyle
          {
            Format = "MMM.yy",
            IsStaggered = true,
            Angle = -45
          }
        }
      });
      chart.Series.Add(new Series
      {
        Name = "Фонд",
        ChartType = SeriesChartType.FastLine,
        Color = System.Drawing.Color.BlueViolet,
        XValueMember = "Date2",
        YValueMembers = "coef_all"
      });
      var r = repRepository.StrategyChartData(id);
      chart.Series[0].Points.DataBind(r, "Date2", "coef_all", null);
      var imgStream = new MemoryStream();
      chart.SaveImage(imgStream, ChartImageFormat.Png);
      imgStream.Seek(0, SeekOrigin.Begin);
      return File(imgStream, "image/png");
    }

  }
}
