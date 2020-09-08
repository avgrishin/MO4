using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace MO.Models
{
  public class RepRSMC
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
      Image im = Image.GetInstance(string.Format(@"{0}RSMC.png", ImagePath));
      im.ScaleToFit(doc.Right - doc.Left, 200);
      doc.Add(im);

      FontFactory.RegisterDirectory(string.Format(@"{0}\Fonts", Environment.GetEnvironmentVariable("windir")));
      var arial8 = FontFactory.GetFont("arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 8, Font.NORMAL, new BaseColor(0, 0, 0));
      var arial8b = FontFactory.GetFont("arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 8, Font.BOLD, new BaseColor(0, 51, 102));
      var arial10 = FontFactory.GetFont("arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 10);
      var arial10b = FontFactory.GetFont("arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 10, Font.BOLD, new BaseColor(0, 51, 102));
      var arial12b = FontFactory.GetFont("arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 12, Font.BOLD, new BaseColor(0, 51, 102));

      var cb = writer.DirectContent;
      var r23 = repRepository.GetRepClient25(DateB, DateE, FinInstID, FundID, IndexID).First();
      ColumnText.ShowTextAligned(cb, Element.ALIGN_RIGHT, new Phrase(string.Format("Данные по состоянию на {0:dd.MM.yyyy}", r23.de), arial10b), PageSize.A4.Width - 22, PageSize.A4.Height - 36 - 57, 0);

      var mtable = new PdfPTable(new float[] { 45f, 55f }) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT, ExtendLastRow = false };
      var table = new PdfPTable(1);
      table.AddCell(new PdfPCell(new Paragraph("Инвестиционная цель", arial12b)) { Border = 2, BorderWidth = 0.8f, PaddingTop = 5f, PaddingBottom = 5f });
      var c = new PdfPCell(new Paragraph("Долгосрочный прирост капитала путем инвестирования в диверсифицированный портфель акций российских компаний малой и средней капитализации, составляющих 30 % капитализации рынка.", arial10)) { HorizontalAlignment = Element.ALIGN_JUSTIFIED, Border = 0 };
      c.SetLeading(0f, 1.1f);
      table.AddCell(c);
      table.AddCell(new PdfPCell(new Paragraph("Инвестиционная стратегия", arial12b)) { Border = 2, BorderWidth = 0.8f, PaddingTop = 5f, PaddingBottom = 5f });
      c = new PdfPCell(new Phrase(100f, "Управляющий использует макро и микро экономический анализ при управлении портфелем и оценке инвестиций, анализирует сектора рынка, а также отдельные недооцененные бумаги. Основной метод оценки - метод дисконтированного потока денежных средств, периодически используются и иные методы оценки. Управляющий также стремится учитывать и  использовать особые условия и события, которые могут способствовать созданию и реализации внутренней стоимости акций, такие как сделки слияния и поглощения, реструктуризация/реорганизация, значительные изменения в регулировании рынка, ценообразовании и т.д. Тесные контакты с эмитентами и рыночными экспертами используются в качестве источника информации о перспективных инвестиционных возможностях. Оборачиваемость портфеля достаточно низкая, с учетом низкой ликвидности основных  объектов инвестирования.", arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_JUSTIFIED };
      c.SetLeading(0f, 1.1f);
      table.AddCell(c);
      table.AddCell(new PdfPCell(new Phrase("Информация о фонде", arial12b)) { Border = 2, BorderWidth = 0.8f, PaddingTop = 5f, PaddingBottom = 5f });
      var t = new PdfPTable(new float[] { 3.3f, 5.3f });
      t.AddCell(new PdfPCell(new Phrase("Наименование", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("Фонд российских акций средней и малой капитализации", arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
      t.AddCell(new PdfPCell(new Phrase("Тип", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("Инвестиционная компания открытого типа", arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
      t.AddCell(new PdfPCell(new Phrase("Место регистрации", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("Британо-Виргинские острова", arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
      t.AddCell(new PdfPCell(new Phrase("Управляющий", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("USB Investment Solutions, Cayman Ltd.", arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
      t.AddCell(new PdfPCell(new Phrase("Инвестиционный советник", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("USB Solutions, Cayman Ltd.", arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });

      t.AddCell(new PdfPCell(new Phrase("Банк-кастодиан", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("Pictet & Cie (Europe) S. A.", arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
      t.AddCell(new PdfPCell(new Phrase("Окончание финансового года", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("31 декабря каждого года", arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
      table.AddCell(new PdfPCell(t) { Border = 0 });

      t = new PdfPTable(new float[] { 4f, 4.6f });
      t.AddCell(new PdfPCell(new Phrase("Рекомендуемый срок инвестирования", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("12 месяцев или более", arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
      table.AddCell(new PdfPCell(t) { Border = 0 });

      t = new PdfPTable(new float[] { 2.6f, 6.0f });
      t.AddCell(new PdfPCell(new Phrase("Бенчмарк", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("RTS2 Index Russia", arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
      table.AddCell(new PdfPCell(t) { Border = 0 });

      t = new PdfPTable(new float[] { 4.6f, 4.0f });
      t.AddCell(new PdfPCell(new Phrase("Валюта фонда", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("USD", arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
      t.AddCell(new PdfPCell(new Phrase("Дата основания фонда", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("23.05.2006", arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
      table.AddCell(new PdfPCell(t) { Border = 0 });

      t = new PdfPTable(new float[] { 5.6f, 3.0f });
      t.AddCell(new PdfPCell(new Phrase("Комиссия при покупке", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("0%", arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
      t.AddCell(new PdfPCell(new Phrase("Комиссия при погашении", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("0%", arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
      table.AddCell(new PdfPCell(t) { Border = 0 });

      t = new PdfPTable(new float[] { 5.6f, 3.0f });
      t.AddCell(new PdfPCell(new Phrase("Вознаграждение за управление", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("2% СЧА", arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
      t.AddCell(new PdfPCell(new Phrase("Вознаграждение за успех", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("10%", arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
      table.AddCell(new PdfPCell(t) { Border = 0 });

      t = new PdfPTable(new float[] { 3.6f, 5.0f });
      t.AddCell(new PdfPCell(new Phrase("Минимальная сумма инвестирования", arial10)) { Border = 0 });
      t.AddCell(new PdfPCell(new Phrase("первоначальная – 100 000 USD, последующая 10 000 USD.", arial10)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
      table.AddCell(new PdfPCell(t) { Border = 0 });
      t = new PdfPTable(new float[] { 1f, 13f });
      var par1 = new Paragraph() { };
      par1.Add(new Phrase("Контакты: ", arial8b));
      c = new PdfPCell(new Phrase("Перед инвестированием в Фонд необходимо внимательно ознакомиться с Проспектом Фонда и иными соответствующими правовыми документами, а также с финансовой   отчетностью. Стоимость акций Фонда может, как расти, так и падать. Последние показатели доходность Фонда не являются  индикатором будущих показателей. Инвестиционные паи Фонда в настоящее время не предназначены  для публичного обращения.", arial8)) { Colspan = 2, Border = 0, HorizontalAlignment = Element.ALIGN_JUSTIFIED };
      c.SetLeading(0f, 1.1f);
      t.AddCell(c);
      table.AddCell(new PdfPCell(t) { Border = 0 });
      mtable.AddCell(new PdfPCell(table) { Border = 0 });

      table = new PdfPTable(1);
      par1 = new Paragraph();
      par1.Add(new Phrase("Крупнейшие вложения, % от СЧА", arial12b));
      par1.Add(new Phrase(string.Format(" (На {0:dd.MM.yyyy})", DateE.Value.AddDays(-30)), arial10b));
      table.AddCell(new PdfPCell(par1) { Border = 2, BorderWidth = 0.8f, PaddingTop = 5f, PaddingBottom = 5f });
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
      table.AddCell(new PdfPCell(new Paragraph("История доходности фонда, в USD", arial12b)) { Border = 2, BorderWidth = 0.8f, PaddingTop = 10f, PaddingBottom = 5f });

      table.AddCell(new PdfPCell(new Phrase(string.Format("Размер чистых активов: {0:0,0.00} млн. USD", r23.c / 1000000f), arial10)) { Border = 0 });

      t = new PdfPTable(new float[] { 2.9f, 1.5f, 1.5f, 1.5f, 1.5f, 1.6f }) { ExtendLastRow = false };
      t.DefaultCell.Border = 2;
      t.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
      t.DefaultCell.PaddingBottom = 5f;
      t.AddCell(new PdfPCell(new Phrase("Доходность,%", arial10b)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 2, PaddingBottom = 5f, VerticalAlignment = Element.ALIGN_BOTTOM });
      t.AddCell(new PdfPCell(new Phrase("1\nмесяц", arial10b)) { HorizontalAlignment = Element.ALIGN_RIGHT, Border = 2, PaddingBottom = 5f, VerticalAlignment = Element.ALIGN_BOTTOM });
      t.AddCell(new PdfPCell(new Phrase("1\nгод", arial10b)) { HorizontalAlignment = Element.ALIGN_RIGHT, Border = 2, PaddingBottom = 5f, VerticalAlignment = Element.ALIGN_BOTTOM });
      t.AddCell(new PdfPCell(new Phrase("3\nгода", arial10b)) { HorizontalAlignment = Element.ALIGN_RIGHT, Border = 2, PaddingBottom = 5f, VerticalAlignment = Element.ALIGN_BOTTOM });
      t.AddCell(new PdfPCell(new Phrase("5\nлет", arial10b)) { HorizontalAlignment = Element.ALIGN_RIGHT, Border = 2, PaddingBottom = 5f, VerticalAlignment = Element.ALIGN_BOTTOM });
      t.AddCell(new PdfPCell(new Phrase("с даты образо- вания", arial10b)) { HorizontalAlignment = Element.ALIGN_RIGHT, Border = 2, PaddingBottom = 5f, VerticalAlignment = Element.ALIGN_BOTTOM });
      t.DefaultCell.Border = 0;
      t.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
      t.AddCell(new PdfPCell(new Phrase("Фонд", arial10)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 });
      t.AddCell(new Phrase(string.Format("{0:0.0}", r23.fm1), arial10));
      t.AddCell(new Phrase(string.Format("{0:0.0}", r23.fy1), arial10));
      t.AddCell(new Phrase(string.Format("{0:0.0}", r23.fy3), arial10));
      t.AddCell(new Phrase(string.Format("{0:0.0}", r23.fy5), arial10));
      t.AddCell(new Phrase(string.Format("{0:0.0}", r23.fy10), arial10));
      t.DefaultCell.Border = 2;
      t.DefaultCell.PaddingBottom = 5f;
      t.AddCell(new PdfPCell(new Phrase("Бенчмарк", arial10)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 2 });
      t.AddCell(new Phrase(string.Format("{0:0.0}", r23.im1), arial10));
      t.AddCell(new Phrase(string.Format("{0:0.0}", r23.iy1), arial10));
      t.AddCell(new Phrase(string.Format("{0:0.0}", r23.iy3), arial10));
      t.AddCell(new Phrase(string.Format("{0:0.0}", r23.iy5), arial10));
      t.AddCell(new Phrase(string.Format("{0:0.0}", r23.iy10), arial10));
      table.AddCell(new PdfPCell(t) { Border = 0 });
      table.AddCell(new PdfPCell(new Phrase("")) { Border = 0 });
      Image im22 = Image.GetInstance(getChart(22));
      table.AddCell(new PdfPCell(im22, true) { Border = 0 });

      var r26 = repRepository.GetRepClient26(null, DateE, FinInstID, FundID, IndexID).First();
      t = new PdfPTable(new float[] { 8f, 2.5f });
      t.AddCell(new PdfPCell(new Phrase("2 года", arial10b)) { HorizontalAlignment = Element.ALIGN_RIGHT, Border = 2, Colspan = 2, PaddingBottom = 5f });
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
          //AntiAliasing = AntiAliasingStyles.All,
          //TextAntiAliasingQuality = TextAntiAliasingQuality.SystemDefault
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
          AxisY = new Axis { MajorGrid = new Grid { LineColor = System.Drawing.Color.LightGray }, Enabled = AxisEnabled.False },
          AxisX = new Axis { MajorGrid = new Grid { Enabled = false }, IsMarginVisible = false, LineColor = System.Drawing.Color.FromArgb(0, 51, 102), IntervalAutoMode = IntervalAutoMode.FixedCount, Interval = 1 }
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
          BackColor = System.Drawing.Color.FromArgb(120, 255, 255, 255),
          Alignment = System.Drawing.StringAlignment.Near,
          Position = new ElementPosition(0, 5, 60, 15),
          LegendStyle = LegendStyle.Column,
          Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point),
          Docking = Docking.Left,
          TableStyle = LegendTableStyle.Tall,
          LegendItemOrder = LegendItemOrder.SameAsSeriesOrder,
          IsTextAutoFit = true,
          IsDockedInsideChartArea = true
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
          Name = "RTS2 Index",
          ChartType = SeriesChartType.FastLine,
          Color = System.Drawing.Color.Gray,
          XValueMember = "Date2",
          YValueMembers = "coef_allb",
          Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        });
        chart.Series[0].Points.DataBind(repRepository.GetRepClient24(DateB, DateE, FinInstID, FundID, IndexID), "Date2", "coef_all", null);
        chart.Series.Add(new Series
        {
          Name = "Russian Small/Mid Cap Fund",
          ChartType = SeriesChartType.FastLine,
          Color = System.Drawing.Color.FromArgb(0, 51, 102),
          XValueMember = "Date2",
          YValueMembers = "coef_all",
          Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        });
        chart.Series[1].Points.DataBind(repRepository.GetRepClient22(DateB, DateE, FinInstID, FundID, IndexID), "Date2", "coef_all", null);
      }
      //var fstr = new FileStream(string.Format("c:\\tmp\\s{0}.bmp", id), FileMode.Create);
      //chart.SaveImage(fstr, ChartImageFormat.Bmp);
      //fstr.Close();
      var imgStream = new MemoryStream();
      chart.SaveImage(imgStream, ChartImageFormat.Bmp);
      imgStream.Seek(0, SeekOrigin.Begin);
      return imgStream;
    }

  }
}