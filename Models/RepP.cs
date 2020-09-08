using System;
using System.IO;
using System.Linq;
using System.Web.UI.DataVisualization.Charting;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace MO.Models
{
  public class RepP
  {
    public DateTime? DateB { get; set; }
    public DateTime? DateE { get; set; }
    public int FinInstID { get; set; }
    public int FundID { get; set; }
    public int IndexID { get; set; }
    public string ImagePath { get; set; }
    public IReportRepository repRepository { get; set; }

    public Stream GetReport()
    {
      var doc = new Document();
      doc.SetMargins(15f, 15f, 15f, 15f);
      var stream = new MemoryStream();
      var writer = PdfWriter.GetInstance(doc, stream);
      writer.CloseStream = false;

      doc.Open();
      Image im = Image.GetInstance(string.Format(@"{0}P.png", ImagePath));
      im.ScaleToFit(doc.Right - doc.Left, 200);
      doc.Add(im);

      FontFactory.RegisterDirectory(string.Format(@"{0}\Fonts", Environment.GetEnvironmentVariable("windir")));
      var arial8 = FontFactory.GetFont("arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 8, Font.NORMAL, new BaseColor(0, 0, 0));
      var arial8b = FontFactory.GetFont("arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 8, Font.BOLD, new BaseColor(0, 51, 102));
      var arial10 = FontFactory.GetFont("arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 10);
      var arial10b = FontFactory.GetFont("arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 10, Font.BOLD, new BaseColor(0, 51, 102));
      var arial12b = FontFactory.GetFont("arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 12, Font.BOLD, new BaseColor(0, 51, 102));

      var cb = writer.DirectContent;
      var r23 = repRepository.GetRepClient23(new DateTime(2000, 5, 1), DateE, FinInstID, FundID, IndexID).First();
      ColumnText.ShowTextAligned(cb, Element.ALIGN_RIGHT, new Phrase(string.Format("Данные по состоянию на {0}", r23.des), arial10b), PageSize.A4.Width - 22, PageSize.A4.Height - 36 - 57, 0);

      var mtable = new PdfPTable(new float[] { 45f, 55f }) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT, ExtendLastRow = false };
      var table = new PdfPTable(1);
      table.AddCell(new PdfPCell(new Paragraph("Инвестиционная цель", arial12b)) { Border = 2, BorderWidth = 0.8f, PaddingTop = 10f, PaddingBottom = 5f });
      var c = new PdfPCell(new Paragraph("Доходность, превышающая средний уровень между рынками акций и облигаций в долгосрочной перспективе, при высокой промежуточной ликвидности.", arial10)) { HorizontalAlignment = Element.ALIGN_JUSTIFIED, Border = 0 };
      c.SetLeading(0f, 1.5f);
      table.AddCell(c);
      table.AddCell(new PdfPCell(new Paragraph("Инвестиционная стратегия", arial12b)) { Border = 2, BorderWidth = 0.8f, PaddingTop = 20f, PaddingBottom = 5f });
      c = new PdfPCell(new Phrase(100f, "Баланс между вложениями в акции и облигации регулярно пересматривается на основе сопоставления рисков и ожидаемой доходности на ближайшие 3-6 месяцев. Выбор конкретных эмитентов и выпусков ценных бумаг осуществляется по критерию фундаментального потенциала. Предпочтение отдаётся высоколиквидным инструментам.", arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_JUSTIFIED };
      c.SetLeading(0f, 1.5f);
      table.AddCell(c);
      table.AddCell(new PdfPCell(new Phrase("Основные параметры фонда", arial12b)) { Border = 2, BorderWidth = 0.8f, PaddingTop = 20f, PaddingBottom = 5f });
      var t = new PdfPTable(new float[] { 3.1f, 5.5f });
      t.AddCell(new PdfPCell(new Phrase("Наименование", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("УралСиб Фонд Профессиональный", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("Тип", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("Открытый паевой инвестиционный фонд смешанных инвестиций", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("Юрисдикция", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("Российская Федерация", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("Управляющая компания", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("ЗАО «Управляющая компания УралСиб»", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("Бенчмарк", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("50% РТС/ 50% RUX-Cbonds", arial10)) { Border = 0 });
      table.AddCell(new PdfPCell(t) { Border = 0 });
      t = new PdfPTable(new float[] { 4f, 4.6f });
      t.AddCell(new PdfPCell(new Phrase("Валюта фонда", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("RUR", arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
      t.AddCell(new PdfPCell(new Phrase("Дата начала работы фонда", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("21.06.2001", arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
      t.AddCell(new PdfPCell(new Phrase("Комиссия за управление, %", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("3,5", arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
      t.AddCell(new PdfPCell(new Phrase("Комиссия за успех, %", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("0", arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
      t.AddCell(new PdfPCell(new Phrase("Скидки, %", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("0,5 - 1,5", arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
      t.AddCell(new PdfPCell(new Phrase("Надбавки, %", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("0 - 0,5", arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
      t.AddCell(new PdfPCell(new Phrase("Прочие издержки, %", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("не более 0,7", arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
      table.AddCell(new PdfPCell(t) { Border = 0 });
      t = new PdfPTable(new float[] { 1f, 13f });
      var par1 = new Paragraph() { };
      par1.Add(new Phrase("Контакты: ", arial8b));
      par1.Add(new Phrase("ул. Ефремова, д.8, Москва, 119048, Россия", arial8));
      t.AddCell(new PdfPCell(par1) { Colspan = 2, Border = 0, PaddingTop = 20f });
      t.AddCell(new PdfPCell(Image.GetInstance(string.Format(@"{0}Phone.png", ImagePath)), false) { Border = 0, HorizontalAlignment = Element.ALIGN_MIDDLE });
      t.AddCell(new PdfPCell(new Phrase("+7 495 788-66-42,", arial8)) { Border = 0 });
      t.AddCell(new PdfPCell(Image.GetInstance(string.Format(@"{0}Contacts.png", ImagePath)), false) { Border = 0, HorizontalAlignment = Element.ALIGN_MIDDLE });
      t.AddCell(new PdfPCell(new Phrase("info@management.uralsib.ru, www.uralsib.com", arial8)) { Border = 0 });
      c = new PdfPCell(new Phrase("Лицензия ЗАО «УК УралСиб» № 21-000-1-00037 выдана ФСФР России 14.07.2000г.\nПравила фонда зарегистрированы ФСФР России № 0029-18610555 от 14.09.1998г.\nСтоимость пая может как увеличиваться, так и уменьшаться. Результаты инвестирования в прошлом не определяют доходы в будущем, государство не гарантирует доходность инвестиций в фонды. Прежде чем приобрести пай, следует внимательно ознакомиться с правилами фондов.", arial8)) { Colspan = 2, Border = 0, HorizontalAlignment = Element.ALIGN_JUSTIFIED };
      c.SetLeading(0f, 1.5f);
      t.AddCell(c);
      table.AddCell(new PdfPCell(t) { Border = 0 });
      mtable.AddCell(new PdfPCell(table) { Border = 0 });

      table = new PdfPTable(1);
      par1 = new Paragraph();
      par1.Add(new Phrase("Основная структура и вложения, %", arial12b));
      par1.Add(new Phrase(string.Format(" (На {0:dd.MM.yyyy})", DateE.Value.AddDays(-30)), arial10b));
      table.AddCell(new PdfPCell(par1) { Border = 2, BorderWidth = 0.8f, PaddingTop = 10f, PaddingBottom = 5f });
      var r = repRepository.GetRepClient21(DateB, DateE.Value.AddDays(-30), FinInstID, FundID, IndexID);
      t = new PdfPTable(new float[] { 8.4f, 2f });
      foreach (var row in r)
      {
        t.AddCell(new PdfPCell(new Phrase(row.NameRus, arial10)) { Border = 0 });
        t.AddCell(new PdfPCell(new Phrase(string.Format("{0:##.0}", row.Perc), arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
      }
      table.AddCell(new PdfPCell(t) { Border = 2, PaddingBottom = 5f });
      table.AddCell(new PdfPCell(new Phrase("")) { Border = 0 });
      Image im2 = Image.GetInstance(getChart(20));
      table.AddCell(new PdfPCell(im2, true) { Border = 0 });
      table.AddCell(new PdfPCell(new Paragraph("Статистика фонда, руб.", arial12b)) { Border = 2, BorderWidth = 0.8f, PaddingTop = 10f, PaddingBottom = 5f });

      table.AddCell(new PdfPCell(new Phrase(string.Format("СЧА: {0:0,0.00} млн. руб", r23.c / 1000000f), arial10)) { Border = 0 });

      t = new PdfPTable(new float[] { 2.9f, 1.5f, 1.5f, 1.5f, 1.5f, 1.6f }) { ExtendLastRow = false };
      t.DefaultCell.Border = 2;
      t.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
      t.DefaultCell.PaddingBottom = 5f;
      t.AddCell(new PdfPCell(new Phrase("Доходность,%", arial10b)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 2, PaddingBottom = 5f, VerticalAlignment = Element.ALIGN_BOTTOM });
      t.AddCell(new Phrase("1\nмесяц", arial10b));
      t.AddCell(new Phrase("1\nгод", arial10b));
      t.AddCell(new Phrase("3\nгода", arial10b));
      t.AddCell(new Phrase("5\nлет", arial10b));
      t.AddCell(new Phrase("10\nлет", arial10b));
      t.DefaultCell.Border = 0;
      t.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
      t.AddCell(new PdfPCell(new Phrase("Фонд", arial10)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 });
      t.AddCell(new Phrase(r23.fm1, arial10));
      t.AddCell(new Phrase(r23.fy1, arial10));
      t.AddCell(new Phrase(r23.fy3, arial10));
      t.AddCell(new Phrase(r23.fy5, arial10));
      t.AddCell(new Phrase(r23.fy10, arial10));
      t.DefaultCell.Border = 2;
      t.DefaultCell.PaddingBottom = 5f;
      t.AddCell(new PdfPCell(new Phrase("Бенчмарк", arial10)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 2 });
      t.AddCell(new Phrase(r23.im1, arial10));
      t.AddCell(new Phrase(r23.iy1, arial10));
      t.AddCell(new Phrase(r23.iy3, arial10));
      t.AddCell(new Phrase(r23.iy5, arial10));
      t.AddCell(new Phrase(r23.iy10, arial10));
      table.AddCell(new PdfPCell(t) { Border = 0 });
      table.AddCell(new PdfPCell(new Phrase("")) { Border = 0 });
      Image im22 = Image.GetInstance(getChart(22));
      table.AddCell(new PdfPCell(im22, true) { Border = 0 });

      var r26 = repRepository.GetRepClient26(null, DateE, FinInstID, FundID, IndexID).First();
      t = new PdfPTable(new float[] { 8f, 2.5f });
      t.AddCell(new PdfPCell(new Phrase("3 года", arial10b)) { HorizontalAlignment = Element.ALIGN_RIGHT, Border = 2, Colspan = 2, PaddingBottom = 5f });
      t.AddCell(new PdfPCell(new Phrase("Волатильность фонда годовая,%", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase(string.Format("{0:0.00}", r26.Vol), arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
      t.AddCell(new PdfPCell(new Phrase("Коэффициент Шарпа фонда", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase(string.Format("{0:0.00}", r26.Sharpe), arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
      t.AddCell(new PdfPCell(new Phrase("Корреляция", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase(string.Format("{0:0.00}", r26.Correl), arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
      t.AddCell(new PdfPCell(new Phrase("Ошибка слежения,%", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase(string.Format("{0:0.00}", r26.TrackErr), arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
      t.AddCell(new PdfPCell(new Phrase("Коэффициент Альфа", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase(string.Format("{0:0.00}", r26.Alpha), arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
      t.AddCell(new PdfPCell(new Phrase("Коэффициент Бета", arial10)) { Border = 2, PaddingBottom = 5f });
      t.AddCell(new PdfPCell(new Phrase(string.Format("{0:0.00}", r26.Beta), arial10)) { Border = 2, HorizontalAlignment = Element.ALIGN_RIGHT, PaddingBottom = 5f });
      table.AddCell(new PdfPCell(t) { Border = 0 });

      table.AddCell(new PdfPCell(new Paragraph("")) { Border = 0 });
      mtable.AddCell(new PdfPCell(table) { Border = 0, PaddingLeft = 10f });
      doc.Add(mtable);

      doc.Close();
      stream.Seek(0, SeekOrigin.Begin);
      return stream;
    }

    private Stream getChart(int id)
    {
      Chart chart = null;
      if (id == 20)
      {
        chart = new Chart()
        {
          Width = new System.Web.UI.WebControls.Unit(400, System.Web.UI.WebControls.UnitType.Pixel),
          Height = new System.Web.UI.WebControls.Unit(200, System.Web.UI.WebControls.UnitType.Pixel),
          //Palette = ChartColorPalette.EarthTones,
          BorderlineWidth = 0,
          AntiAliasing = AntiAliasingStyles.All,
          TextAntiAliasingQuality = TextAntiAliasingQuality.SystemDefault
        };
        chart.Series.Add(new Series
        {
          Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point),
          ChartType = SeriesChartType.Bar,
          XValueMember = "NameRus",
          YValueMembers = "Qty",
          Color = System.Drawing.Color.FromArgb(255, 255, 255),
          BorderColor = System.Drawing.Color.FromArgb(0, 51, 102),
          BorderWidth = 1,
          //CustomProperties = "DrawingStyle=Emboss, BarLabelStyle=Outside, DrawSideBySide=False",
          IsValueShownAsLabel = true,

          Label = "#PERCENT{0%}"

        });
        chart.ChartAreas.Add(new ChartArea
        {
          BorderWidth = 1,
          AxisY = new Axis { MajorGrid = new Grid { Enabled = false }, Enabled = AxisEnabled.False },
          AxisX = new Axis
          {
            MajorGrid = new Grid { Enabled = false },
            IsMarginVisible = false,
            LabelStyle = new LabelStyle() { TruncatedLabels = true },
            LineColor = System.Drawing.Color.FromArgb(0, 51, 102),
            IntervalAutoMode = IntervalAutoMode.FixedCount,
            Interval = 1,
            IsLabelAutoFit = true
          }
        });
        chart.DataSource = repRepository.GetRepClient20(DateB, DateE, FinInstID, FundID, IndexID);
        chart.DataBind();
      }
      else if (id == 22)
      {
        chart = new Chart()
        {
          Width = new System.Web.UI.WebControls.Unit(400, System.Web.UI.WebControls.UnitType.Pixel),
          Height = new System.Web.UI.WebControls.Unit(300, System.Web.UI.WebControls.UnitType.Pixel),
          AntiAliasing = AntiAliasingStyles.All,
          TextAntiAliasingQuality = TextAntiAliasingQuality.SystemDefault
        };
        chart.Legends.Add(new Legend
        {
          BorderWidth = 0,
          Alignment = System.Drawing.StringAlignment.Near,
          BackColor = System.Drawing.Color.FromArgb(120, 255, 255, 255),
          Position = new ElementPosition(0, 5, 75, 15),
          LegendStyle = LegendStyle.Column,
          Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point),
          Docking = Docking.Left,
          TableStyle = LegendTableStyle.Tall,
          LegendItemOrder = LegendItemOrder.SameAsSeriesOrder
        });
        chart.ChartAreas.Add(new ChartArea
        {
          AxisY = new Axis
          {
            Enabled = AxisEnabled.False,
          },
          AxisY2 = new Axis
          {
            IsStartedFromZero = true,
            Enabled = AxisEnabled.True,
            LabelAutoFitMaxFontSize = 10,
            LabelAutoFitMinFontSize = 8,
            IntervalAutoMode = IntervalAutoMode.VariableCount,
            IsLabelAutoFit = true,
            MajorGrid = new Grid
            {
              Enabled = true,
              LineDashStyle = ChartDashStyle.Dash,
              LineColor = System.Drawing.Color.Gray,
              LineWidth = 1
            },
            LabelStyle = new LabelStyle { Format = "N0" }
          },
          AxisX = new Axis
          {
            LabelAutoFitMaxFontSize = 10,
            LabelAutoFitMinFontSize = 10,
            IntervalAutoMode = IntervalAutoMode.VariableCount,
            IntervalType = DateTimeIntervalType.Days,
            IsLabelAutoFit = true,
            IsMarginVisible = false,

            MajorGrid = new Grid
            {
              Enabled = false
            },
            LabelStyle = new LabelStyle
            {
              Format = "MMM yy",
              IsStaggered = true,
              Angle = -90
            }
          },
          BorderWidth = 0
        });
        chart.Series.Add(new Series
        {
          Name = "50% РТС/50% RUX-Cbonds,%",
          ChartType = SeriesChartType.FastLine,
          Color = System.Drawing.Color.Gray,
          XValueMember = "Date2",
          YValueMembers = "coef_allb",
          Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        });
        chart.Series[0].Points.DataBind(repRepository.GetRepClient24(DateE.Value.AddYears(-10), DateE, FinInstID, FundID, IndexID), "Date2", "coef_all", null);
        chart.Series.Add(new Series
        {
          Name = "УралСиб Профессиональный,%",
          ChartType = SeriesChartType.FastLine,
          Color = System.Drawing.Color.FromArgb(0, 51, 102),
          XValueMember = "Date2",
          YValueMembers = "coef_all",
          Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        });
        chart.Series[1].Points.DataBind(repRepository.GetRepClient22(DateE.Value.AddYears(-10), DateE, FinInstID, FundID, IndexID), "Date2", "coef_all", null);
      }
      var imgStream = new MemoryStream();
      chart.SaveImage(imgStream, ChartImageFormat.Bmp);
      imgStream.Seek(0, SeekOrigin.Begin);
      return imgStream;
    }
  }
}