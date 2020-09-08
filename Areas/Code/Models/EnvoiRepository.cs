using MO.Models;
using MvcContrib.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace MO.Areas.Code.Models
{
  public interface IEnvoiRepository
  {
    IEnumerable<dynamic> getEnvoiList(int TypeID, bool? isAuto, bool? IsActive, string sort, string dir);
    IEnumerable<dynamic> addEnvoi(List<tEnvoi> data);
    IEnumerable<dynamic> updEnvoi(List<tEnvoi> data);
    bool delEnvoi(List<tEnvoi> data);
    IEnumerable<dynamic> getEMailList(string sort, string dir);
    IEnumerable<dynamic> addEMail(List<EMailItem> data);
    IEnumerable<dynamic> updEMail(List<EMailItem> data);
    bool delEMail(List<EMailItem> data);
    bool envoyerCourriel(int? id, string Comment, string host);
    IEnumerable<dynamic> getEnvoiHoraire(int? id);
    IEnumerable<dynamic> addEnvoiHoraire(List<tEnvoiHoraire> data);
    IEnumerable<dynamic> updEnvoiHoraire(List<tEnvoiHoraire> data);
    bool delEnvoiHoraire(List<tEnvoiHoraire> data);
    IEnumerable<dynamic> getEnvoiHoraireType();

    IEnumerable<dynamic> getEnvoiExecList(int TypeID, DateTime? d1, DateTime? d2, bool? IsExec, string sort, string dir);
    IEnumerable<dynamic> addEnvoiExec(List<tEnvoiExec> data);
    IEnumerable<dynamic> updEnvoiExec(List<tEnvoiExec> data);
    bool delEnvoiExec(List<tEnvoiExec> data);
    bool envoiExecCourriel(string host);
    bool envoiExecRiCourriel(string host);

    IEnumerable<dynamic> getConseilList(DateTime? d1, DateTime? d2, int? type, Boolean? nopen, string sort, string dir);
    IEnumerable<dynamic> addConseil(List<tConseil> data);
    IEnumerable<dynamic> updConseil(List<tConseil> data);
    bool delConseil(List<tConseil> data);
    bool conseilCourriel(int? id);
    bool conseilEnabledCourriel();
    IEnumerable<dynamic> getConseilHoraire(int? id);
    IEnumerable<dynamic> addConseilHoraire(List<tConseilHoraire> data);
    IEnumerable<dynamic> updConseilHoraire(List<tConseilHoraire> data);
    bool delConseilHoraire(List<tConseilHoraire> data);
    IEnumerable<dynamic> getCPriorite();
    bool conseilCourrielAll();

    IEnumerable<dynamic> getRiskMapList(string sort, string dir);
    IEnumerable<dynamic> addRiskMap(List<tRiskMap> data);
    IEnumerable<dynamic> updRiskMap(List<tRiskMap> data);
    bool delRiskMap(List<tRiskMap> data);
    IEnumerable<dynamic> getRMLevel();
    IEnumerable<dynamic> getRiskMapHoraire(int? id);
    IEnumerable<dynamic> addRiskMapHoraire(List<tRiskMapHoraire> data);
    IEnumerable<dynamic> updRiskMapHoraire(List<tRiskMapHoraire> data);
    bool delRiskMapHoraire(List<tRiskMapHoraire> data);
    bool riskMapCourriel(List<int> id, string host);

    IEnumerable<dynamic> getDeclViol(DateTime? d1, DateTime? d2, bool? op, string sort, string dir);
    IEnumerable<dynamic> addDeclViol(List<tInvDeclErrorJournal> data);
    IEnumerable<dynamic> updDeclViol(List<tInvDeclErrorJournal> data);
    bool delDeclViol(List<tInvDeclErrorJournal> data);
    IQueryable<dynamic> GetFinInsts(string q, int limit);
    dynamic GetFinInst(int FinInstID);
    dynamic GetDeclByInstID(int? id);
    dynamic GetDeclWhereByDeclID(int? id, int? wid);
    IEnumerable<dynamic> GetObjClsByParent(int id);
    IEnumerable<dynamic> GetIssuers(int? id, DateTime? d);
    bool declViolCourriel(string host);
    DateTime? getTermDate(DateTime? d, int? prid);

    IEnumerable<dynamic> getEnvoiExecByDepList(DateTime? d1, DateTime? d2, bool? IsExec, string sort, string dir);
    IEnumerable<dynamic> updEnvoiExecByDep(List<EnvoiDep> data);

  }

  public class EnvoiRepository : IEnvoiRepository
  {
    private MiddleOfficeDataContext db = new MiddleOfficeDataContext() { CommandTimeout = 600 };

    public IEnumerable<dynamic> getEnvoiList(int TypeID, bool? isAuto, bool? IsActive, string sort, string dir)
    {
      var q1 = db.tEnvois.Where(p => p.IsAuto == isAuto && p.TypeID == TypeID);
      if (IsActive == true)
        q1 = q1.Where(p => p.IsEnabled == true);
      var ql = db.taLibs.Where(p => p.LConcept == 458622 && p.LParent == 458622).Select(p => new { p.LName, p.LName1 });
      var q = (from e in q1
               join c1 in db.tObjClassifiers on e.PeriodichID equals c1.ObjClassifierID into _c1
               from c1 in _c1.DefaultIfEmpty()
               select new
               {
                 id = e.ID,
                 e.EmailCc,
                 e.EmailTo,
                 e.IsAuto,
                 e.Mesto,
                 e.Osnovan,
                 e.PoryadPredst,
                 e.PeriodichID,
                 Periodich = c1.Name,
                 e.SrokRask,
                 e.SrokRass,
                 e.TypeInf,
                 e.VidAktiv,
                 e.IsEnabled
               })
      .ToList().Select(e => new
      {
        e.id,
        e.EmailCc,
        e.EmailTo,
        e.IsAuto,
        e.Mesto,
        e.Osnovan,
        e.PoryadPredst,
        e.PeriodichID,
        e.Periodich,
        e.SrokRask,
        e.SrokRass,
        e.TypeInf,
        e.VidAktiv,
        e.IsEnabled,

        EmailToName = string.Join(", ", ql.Where(f => e.EmailTo.IndexOf(f.LName1) > -1).OrderBy(f => f.LName).Select(f => f.LName.Trim()).ToArray()),
        EmailCcName = string.Join(", ", ql.Where(f => e.EmailCc.IndexOf(f.LName1) > -1).OrderBy(f => f.LName).Select(f => f.LName.Trim()).ToArray())
      });

      if (sort != null) q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
      return q;
    }

    public IEnumerable<dynamic> addEnvoi(List<tEnvoi> data)
    {
      db.tEnvois.InsertAllOnSubmit(data);
      db.SubmitChanges();

      var q = from e in db.tEnvois.Where(p => data.Select(n => n.ID).Contains(p.ID))
              join c1 in db.tObjClassifiers on e.PeriodichID equals c1.ObjClassifierID into _c1
              from c1 in _c1.DefaultIfEmpty()
              select new
              {
                id = e.ID,
                e.EmailCc,
                e.EmailTo,
                EmailToName = string.Join(", ", db.taLibs.Where(p => p.LConcept == 458622 && p.LParent == 458622 && e.EmailTo.IndexOf(p.LName1) > -1).OrderBy(p => p.LName).Select(o => o.LName.Trim()).ToArray()),
                EmailCcName = string.Join(", ", db.taLibs.Where(p => p.LConcept == 458622 && p.LParent == 458622 && e.EmailCc.IndexOf(p.LName1) > -1).OrderBy(p => p.LName).Select(o => o.LName.Trim()).ToArray()),
                e.IsAuto,
                e.Mesto,
                e.Osnovan,
                e.PoryadPredst,
                e.PeriodichID,
                Periodich = c1.Name,
                e.SrokRask,
                e.SrokRass,
                e.TypeInf,
                e.VidAktiv,
                e.IsEnabled
              };

      return q;
    }

    public IEnumerable<dynamic> updEnvoi(List<tEnvoi> data)
    {
      foreach (var e in data.Where(p => p.ID > 0))
      {
        var q1 = db.tEnvois.Where(p => p.ID == e.ID).FirstOrDefault();
        if (q1 != null)
        {
          q1.IsAuto = e.IsAuto;
          q1.Mesto = e.Mesto;
          q1.Osnovan = e.Osnovan;
          q1.PoryadPredst = e.PoryadPredst;
          q1.Periodich = e.Periodich;
          q1.PeriodichID = e.PeriodichID;
          q1.SrokRask = e.SrokRask;
          q1.TypeInf = e.TypeInf;
          q1.VidAktiv = e.VidAktiv;
          q1.SrokRass = e.SrokRass;
          q1.EmailTo = e.EmailTo;
          q1.EmailCc = e.EmailCc;
          q1.IsEnabled = e.IsEnabled;
          db.SubmitChanges();
        }
      }
      db.SubmitChanges();

      var q = from en in db.tEnvois.Where(p => data.Select(n => n.ID).Contains(p.ID))
              join c1 in db.tObjClassifiers on en.PeriodichID equals c1.ObjClassifierID into _c1
              from c1 in _c1.DefaultIfEmpty()
              select new
              {
                id = en.ID,
                en.EmailCc,
                en.EmailTo,
                EmailToName = string.Join(", ", db.taLibs.Where(p => p.LConcept == 458622 && p.LParent == 458622 && en.EmailTo.IndexOf(p.LName1) > -1).OrderBy(p => p.LName).Select(o => o.LName.Trim()).ToArray()),
                EmailCcName = string.Join(", ", db.taLibs.Where(p => p.LConcept == 458622 && p.LParent == 458622 && en.EmailCc.IndexOf(p.LName1) > -1).OrderBy(p => p.LName).Select(o => o.LName.Trim()).ToArray()),
                en.IsAuto,
                en.Mesto,
                en.Osnovan,
                en.PoryadPredst,
                en.PeriodichID,
                Periodich = c1.Name,
                en.SrokRask,
                en.SrokRass,
                en.TypeInf,
                en.VidAktiv,
                en.IsEnabled
              };

      return q;
    }

    public bool delEnvoi(List<tEnvoi> data)
    {
      try
      {
        IEnumerable<tEnvoi> e = db.tEnvois.Where(p => data.Select(n => n.ID).Contains(p.ID));
        db.tEnvois.DeleteAllOnSubmit(e);
        db.SubmitChanges();
        return true;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public IEnumerable<dynamic> getEMailList(string sort, string dir)
    {
      var q = from l in db.taLibs.Where(p => p.LConcept == 458622 && p.LParent == 458622)
              select new { id = l.LID, name = l.LName, email = l.LName1 };

      if (sort != null) q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
      return q;
    }

    public IEnumerable<dynamic> addEMail(List<EMailItem> data)
    {
      List<taLib> l = new List<taLib>();
      foreach (var e in data.Where(p => (p.ID ?? 0) == 0))
      {
        l.Add(new taLib { LConcept = 458622, LParent = 458622, LName = e.Name, LName1 = e.Email, InDateTime = DateTime.Now });
      }
      db.taLibs.InsertAllOnSubmit(l);
      db.SubmitChanges();

      var q = from lb in db.taLibs.Where(p => l.Select(n => n.LID).Contains(p.LID))
              select new { id = lb.LID, name = lb.LName, email = lb.LName1 };

      return q;
    }

    public IEnumerable<dynamic> updEMail(List<EMailItem> data)
    {
      List<taLib> l = new List<taLib>();
      foreach (var e in data.Where(p => (p.ID ?? 0) > 0))
      {
        var q1 = db.taLibs.Where(p => p.LID == e.ID && p.LConcept == 458622).First();
        if (q1 != null)
        {
          q1.LName = e.Name;
          q1.LName1 = e.Email;
          q1.InDateTime = DateTime.Now;
          db.SubmitChanges();
          l.Add(new taLib { LID = e.ID.Value });
        }
      }
      db.SubmitChanges();

      var q = from lb in db.taLibs.Where(p => l.Select(n => n.LID).Contains(p.LID))
              select new { id = lb.LID, name = lb.LName, email = lb.LName1 };

      return q;
    }

    public bool delEMail(List<EMailItem> data)
    {
      try
      {
        var q = db.taLibs.Where(o => o.LID == data[0].ID && o.LConcept == 458622);
        db.taLibs.DeleteAllOnSubmit(q);
        db.SubmitChanges();
        return true;
      }
      catch
      {
        return false;
      }
    }

    public bool envoyerCourriel(int? id, string Comment, string host)
    {
      var q = db.tEnvois.FirstOrDefault(p => p.ID == id);
      if (q != null)
      {
        var q1 = db.tEnvoiHoraires.Where(p => p.EnvoiID == id && p.ModeID == 1).FirstOrDefault();
        DateTime? dt = null;
        if (q1 != null)
        {
          switch (q1.EnvoiHoraireTypeID)
          {
            case 1:
              if (q1.Month.HasValue && q1.Day.HasValue)
                dt = new DateTime(DateTime.Today.Year, q1.Month.Value, q1.Day.Value);
              break;
            case 2:
              if ((q1.Day ?? 1) > 0)
                dt = db.tWorkDates.Where(p => p.WorkDate >= new DateTime(DateTime.Today.Year, 1, 1)).OrderBy(p => p.WorkDate).Take(q1.Day ?? 1).OrderByDescending(p => p.WorkDate).FirstOrDefault().WorkDate;
              break;
            case 3:
              if ((q1.Day ?? 1) > 0)
                dt = db.tWorkDates.Where(p => p.WorkDate >= new DateTime(DateTime.Today.Year, (DateTime.Today.Month - 1) / 3 * 3 + 1, 1)).OrderBy(p => p.WorkDate).Take(q1.Day ?? 1).OrderByDescending(p => p.WorkDate).FirstOrDefault().WorkDate;
              break;
            case 4:
              if ((q1.Day ?? 1) > 0)
                dt = db.tWorkDates.Where(p => p.WorkDate >= new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)).OrderBy(p => p.WorkDate).Take(q1.Day ?? 1).OrderByDescending(p => p.WorkDate).FirstOrDefault().WorkDate;
              break;
            case 11:
              if ((q1.Day ?? 1) > 0)
                dt = db.tWorkDates.Where(p => p.WorkDate >= new DateTime(DateTime.Today.Year, 1, 1).AddDays((q1.Day ?? 1) - 1)).OrderBy(p => p.WorkDate).FirstOrDefault().WorkDate;
              break;
            case 12:
              if ((q1.Day ?? 1) > 0)
                dt = db.tWorkDates.Where(p => p.WorkDate >= new DateTime(DateTime.Today.Year, (DateTime.Today.Month - 1) / 3 * 3 + 1, 1).AddDays((q1.Day ?? 1) - 1)).OrderBy(p => p.WorkDate).FirstOrDefault().WorkDate;
              break;
            case 13:
              if ((q1.Day ?? 1) > 0)
                dt = db.tWorkDates.Where(p => p.WorkDate >= new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddDays((q1.Day ?? 1) - 1)).OrderBy(p => p.WorkDate).FirstOrDefault().WorkDate;
              break;
          }
        }
        SmtpClient sc = new SmtpClient("m.am-uralsib.ru");
        sc.UseDefaultCredentials = true;
        MailMessage message = new MailMessage();
        message.From = new MailAddress("Внутренний контроль <assets_msg@am-uralsib.ru>");
        if (!string.IsNullOrWhiteSpace(q.EmailTo))
          message.To.Add((host.Contains("localhost") || host.Contains("10.153.157.66")) ? "GrishinAV@am-uralsib.ru" : q.EmailTo);
        if (!string.IsNullOrWhiteSpace(q.EmailCc))
          message.CC.Add((host.Contains("localhost") || host.Contains("10.153.157.66")) ? "GrishinAV@am-uralsib.ru" : q.EmailCc);
        StringBuilder sb = new StringBuilder();
        sb.Append("<style>td, span, th {font-size:.8em;font-family: \"Segoe UI\", Verdana, Helvetica, Sans-Serif;} span {font-style:italic} th{font-size:.7em}</style>");
        sb.Append("<span>Уважаемые коллеги,<br>напоминаем о сроках предоставления следующей отчетности/информации:<br><br>");
        sb.AppendFormat("<table border='1' cellspacing='0'><tr><th>Тип раскрываемой информации</th><th>Срок представления</th><th>Место представления</th><th>Основание</th></tr><tr><td>{0}</td><td>{1}{4}</td><td>{2}</td><td>{3}</td></tr></table>", q.TypeInf, q.SrokRask, q.Mesto, q.Osnovan, dt.HasValue ? "(" + dt.Value.ToShortDateString() + ")" : "");
        sb.Append("<br>Подпись: Внутренний контроль</span>");
        message.Body = sb.ToString();
        message.IsBodyHtml = true;
        message.Priority = MailPriority.High;
        message.Headers.Add("Importance", "High");
        message.IsBodyHtml = true;
        message.Subject = string.Format("Напоминание о сроках предоставления отчетности/информации {0}", Comment);
        sc.Send(message);

        return true;
      }
      return false;
    }

    public bool conseilCourriel(int? id)
    {
      var q = db.tConseils.FirstOrDefault(p => p.ID == id);
      if (q != null)
      {
        SmtpClient sc = new SmtpClient("m.am-uralsib.ru");
        sc.UseDefaultCredentials = true;
        MailMessage message = new MailMessage();
        message.From = new MailAddress("Внутренний контроль <assets_msg@am-uralsib.ru>");
        if (!string.IsNullOrWhiteSpace(q.EmailTo))
          message.To.Add(q.EmailTo);
        if (!string.IsNullOrWhiteSpace(q.EmailCc))
          message.CC.Add(q.EmailCc);
        StringBuilder sb = new StringBuilder();
        sb.Append("<style>td, span, th {font-size:.8em;font-family: \"Segoe UI\", Verdana, Helvetica, Sans-Serif;vertical-align:top} span {font-style:italic} th{font-size:.7em}</style>");
        sb.Append("<span>Уважаемые коллеги,<br>напоминаю Вам о сроках исполнения рекомендаций, выпущенных заместителем генерального директора-контролером АО \"УК УРАЛСИБ\".<br>Прошу Вас в срок не позднее одного рабочего дня, следующего за днем получения настоящего напоминания, предоставить информацию о статусе и сроках исполнения рекомендации.<br>Дополнительно сообщаю, что информация о неисполненных в срок рекомендациях будет рассматриваться на заседаниях Правления, а также Совета директоров АО \"УК УРАЛСИБ\".<br><br>Спасибо!<br><br>");
        sb.AppendFormat("<table border='1' cellspacing='0'><tr><th width='40%'>Содержание недостатка/нарушения</th><th>Содержание рекомендации</th><th>Срок выполнения рекомендации</th><th>Статус выполнения</th><th>Срок неисполнения рекомендаций</th><th>Срок для продления выполнения рекомендации</th></tr><tr><td>{0}</td><td>{1}</td><td>{2:dd.MM.yyyy}</td><td>{3}</td><td>{4}</td><td>{5:dd.MM.yyyy}</td></tr></table>", q.Violation.Replace("\n", "<br>"), q.Conseil, q.Terme, q.Terme < DateTime.Today ? "Просрочено" : "&nbsp;", q.Terme.HasValue && q.Terme < DateTime.Today ? DateTime.Today.Subtract(q.Terme.Value).TotalDays.ToString() + " дней" : "&nbsp;", q.Prolongation);
        sb.Append("<br>С уважением, Лихолетова Елена");
        sb.Append("<br>Заместитель генерального директора - контролер");
        sb.Append("<br>АО \"УК УРАЛСИБ\"");
        sb.Append("<br>тел.: +7 (495) 788-66-42, доб.7404");
        sb.Append("<br>E-mail: LikholetovaES@am-uralsib.ru</span>");
        message.Body = sb.ToString();
        message.IsBodyHtml = true;
        message.Priority = MailPriority.High;
        message.Headers.Add("Importance", "High");
        message.IsBodyHtml = true;
        message.Subject = "Напоминание по исполнению рекомендаций контролера УК УРАЛСИБ";
        sc.Send(message);

        return true;
      }
      return false;
    }

    public bool conseilCourrielAll()
    {
      SmtpClient sc = new SmtpClient("m.am-uralsib.ru");
      sc.UseDefaultCredentials = true;
      MailMessage message = new MailMessage();
      message.From = new MailAddress("Внутренний контроль <assets_msg@am-uralsib.ru>");
      message.To.Add("LikholetovaES@am-uralsib.ru");
      message.To.Add("IvlievaTA@am-uralsib.ru");
      message.To.Add("NikonenkoKV@am-uralsib.ru");
      //message.Bcc.Add("GrishinAV@am-uralsib.ru");
      StringBuilder sb = new StringBuilder();
      sb.Append("<style>td, span, th {font-size:.8em;font-family: \"Segoe UI\", Verdana, Helvetica, Sans-Serif;vertical-align:top} span {font-style:italic} th{font-size:.7em}</style>");
      sb.Append("<table border='1' cellspacing='0'><tr><th width='40%'>Содержание недостатка/нарушения</th><th>Содержание рекомендации</th><th>Срок выполнения рекомендации</th><th>Статус выполнения</th><th>Срок неисполнения рекомендаций</th><th>Срок для продления выполнения рекомендации</th><th>Владельцы</th></tr>");
      var q1 = from c in db.tConseils.Where(p => p.IsEnabled == true)
               orderby c.Terme
               select new
               {
                 c.Violation,
                 c.Conseil,
                 c.Terme,
                 c.Prolongation,
                 EmailToName = string.Join(", ", db.taLibs.Where(p => p.LConcept == 458622 && p.LParent == 458622 && c.EmailTo.IndexOf(p.LName1) > -1).OrderBy(p => p.LName).Select(o => o.LName.Trim()).ToArray())
               };
      foreach (var q in q1)
      {
        sb.AppendFormat("<tr style=\"color:{6}\"><td>{0}</td><td>{1}</td><td>{2:dd.MM.yyyy}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{7}</td></tr>",
          q.Violation.Replace("\n", "<br>"),
          q.Conseil,
          q.Terme,
          q.Terme < DateTime.Today ? "Просрочено" : "&nbsp;",
          q.Terme.HasValue && q.Terme < DateTime.Today ? DateTime.Today.Subtract(q.Terme.Value).TotalDays.ToString() + " дней" : "&nbsp;",
          q.Prolongation.HasValue ? q.Prolongation.Value.ToShortDateString() : "&nbsp;",
          q.Terme < DateTime.Today ? "red" : q.Terme < DateTime.Today.AddDays(7) ? "maroon" : "black",
          q.EmailToName
        );
      }
      sb.Append("</table>");
      sb.Append("<br>С уважением,");
      sb.Append("<br>Управление внутреннего контроля");
      message.Body = sb.ToString();
      message.IsBodyHtml = true;
      message.Priority = MailPriority.High;
      message.Headers.Add("Importance", "High");
      message.IsBodyHtml = true;
      message.Subject = "Журнал рекомендаций контролера УК УРАЛСИБ";
      sc.Send(message);

      return true;
    }

    public bool conseilEnabledCourriel()
    {
      var q1 = db.tConseils.Where(p => p.IsEnabled == true).OrderBy(p => p.Terme).ToList();
      if (q1.Count > 0)
      {
        SmtpClient sc = new SmtpClient("m.am-uralsib.ru");
        sc.UseDefaultCredentials = true;
        MailMessage message = new MailMessage();
        message.From = new MailAddress("Внутренний контроль <assets_msg@am-uralsib.ru>");
        message.To.Add("LikholetovaES@am-uralsib.ru");
        message.To.Add("IvlievaTA@am-uralsib.ru");
        message.To.Add("NikonenkoKV@am-uralsib.ru");
        StringBuilder sb = new StringBuilder();
        sb.Append("<style>td, span, th {font-size:.8em;font-family: \"Segoe UI\", Verdana, Helvetica, Sans-Serif;vertical-align:top} span {font-style:italic} th{font-size:.7em}</style>");
        sb.Append("<table border='1' cellspacing='0'>");
        sb.Append("<tr><th width='40%'>Содержание недостатка/нарушения</th><th>Содержание рекомендации</th><th>Дата вынесения</th><th>Срок выполнения рекомендации</th><th>Срок неисполнения рекомендаций</th><th>Срок для продления выполнения рекомендации</th><th>Владелец</th></tr>");
        foreach (var q in q1)
        {
          sb.AppendFormat("<tr style='color:{7}'><td>{0}</td><td>{1}</td><td>{5:dd.MM.yy}</td><td>{2:dd.MM.yy}</td><td>{3}</td><td>{4}</td><td>{6}</td></tr>",
            q.Violation.Replace("\n", "<br>"),
            q.Conseil,
            q.Terme,
            q.Terme.HasValue && q.Terme < DateTime.Today ? DateTime.Today.Subtract(q.Terme.Value).TotalDays.ToString() + " дней" : "&nbsp;",
            q.Prolongation.HasValue ? q.Prolongation.Value.ToShortDateString() : "&nbsp;",
            q.PrononceDate,
            q.Possesseur,
            q.Terme < DateTime.Today ? "red" : "black;"
          );
        }
        sb.Append("</table>");
        message.Body = sb.ToString();
        message.IsBodyHtml = true;
        message.Priority = MailPriority.High;
        message.Headers.Add("Importance", "High");
        message.IsBodyHtml = true;
        message.Subject = "Незакрытые напоминания по исполнению рекомендаций контролера УК УРАЛСИБ";
        sc.Send(message);
        return true;
      }
      return false;
    }

    public IEnumerable<dynamic> getEnvoiHoraire(int? id)
    {
      var q = from eh in db.tEnvoiHoraires.Where(p => p.EnvoiID == id)
              select new
              {
                id = eh.ID,
                eh.EnvoiID,
                eh.EnvoiHoraireTypeID,
                eh.ModeID,
                Mode = eh.ModeID == 0 ? "Напоминание" : "Срок направления",
                EnvoiHoraireType = eh.tEnvoiHoraireType.Name,
                eh.Day,
                eh.Month,
                eh.Comment
              };
      return q;
    }

    public IEnumerable<dynamic> addEnvoiHoraire(List<tEnvoiHoraire> data)
    {
      db.tEnvoiHoraires.InsertAllOnSubmit(data);
      db.SubmitChanges();

      var q = from eh in db.tEnvoiHoraires.Where(p => data.Select(n => n.ID).Contains(p.ID))
              select new
              {
                id = eh.ID,
                eh.EnvoiID,
                eh.EnvoiHoraireTypeID,
                eh.ModeID,
                Mode = eh.ModeID == 0 ? "Напоминание" : "Срок направления",
                EnvoiHoraireType = eh.tEnvoiHoraireType.Name,
                eh.Day,
                eh.Month,
                eh.Comment
              };
      return q;
    }

    public IEnumerable<dynamic> updEnvoiHoraire(List<tEnvoiHoraire> data)
    {
      foreach (var e in data.Where(p => p.ID > 0))
      {
        var q1 = db.tEnvoiHoraires.Where(p => p.ID == e.ID).First();
        if (q1 != null)
        {
          q1.EnvoiHoraireTypeID = e.EnvoiHoraireTypeID;
          q1.ModeID = e.ModeID;
          q1.Day = e.Day;
          q1.Month = e.Month;
          q1.Comment = e.Comment;
          db.SubmitChanges();
        }
      }

      var q = from eh in db.tEnvoiHoraires.Where(p => data.Select(n => n.ID).Contains(p.ID))
              select new
              {
                id = eh.ID,
                eh.EnvoiID,
                eh.ModeID,
                Mode = eh.ModeID == 0 ? "Напоминание" : "Срок направления",
                eh.EnvoiHoraireTypeID,
                EnvoiHoraireType = eh.tEnvoiHoraireType.Name,
                eh.Day,
                eh.Month,
                eh.Comment
              };

      return q;
    }

    public bool delEnvoiHoraire(List<tEnvoiHoraire> data)
    {
      try
      {
        IEnumerable<tEnvoiHoraire> e = db.tEnvoiHoraires.Where(p => data.Select(n => n.ID).Contains(p.ID));
        db.tEnvoiHoraires.DeleteAllOnSubmit(e);
        db.SubmitChanges();
        return true;
      }
      catch (Exception /*ex*/)
      {
        return false;
      }
    }

    public IEnumerable<dynamic> getEnvoiHoraireType()
    {
      var q = db.tEnvoiHoraireTypes.Select(p => new { id = p.ID, p.Name });
      return q;
    }

    public IEnumerable<dynamic> getEnvoiExecList(int TypeID, DateTime? d1, DateTime? d2, bool? IsExec, string sort, string dir)
    {
      var q1 = db.tEnvoiExecs.Where(p => 1 == 1);
      if (d1.HasValue)
        q1 = q1.Where(p => p.Date1 >= d1);
      if (d2.HasValue)
        q1 = q1.Where(p => p.Date1 <= d2);
      if (IsExec == true)
        q1 = q1.Where(p => !p.Date2.HasValue && p.tEnvoi.IsEnabled == true);
      var q = from t in q1
              join en in db.tEnvois.Where(p => p.TypeID == TypeID) on t.EnvoiID equals en.ID
              join c1 in db.tObjClassifiers on en.PeriodichID equals c1.ObjClassifierID into _c1
              from c1 in _c1.DefaultIfEmpty()
              select new
              {
                id = t.ID,
                t.EnvoiID,
                t.Date1,
                t.Date2,
                t.Comment,
                en.TypeInf,
                en.Osnovan,
                en.Mesto,
                en.PoryadPredst,
                en.PeriodichID,
                Periodich = c1.Name,
                en.SrokRask,
                EmailTo = string.Join(", ", db.taLibs.Where(p => p.LConcept == 458622 && p.LParent == 458622 && en.EmailTo.IndexOf(p.LName1) > -1).OrderBy(p => p.LName).Select(o => o.LName.Trim()).ToArray()),
                EmailCc = string.Join(", ", db.taLibs.Where(p => p.LConcept == 458622 && p.LParent == 458622 && en.EmailCc.IndexOf(p.LName1) > -1).OrderBy(p => p.LName).Select(o => o.LName.Trim()).ToArray()),
                en.IsAuto,
                en.IsEnabled
              };
      q = q.OrderBy(sort ?? "Date1", dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
      return q;
    }

    public IEnumerable<dynamic> addEnvoiExec(List<tEnvoiExec> data)
    {
      foreach (var d in data)
        d.InDateTime = DateTime.Now;
      db.tEnvoiExecs.InsertAllOnSubmit(data);
      db.SubmitChanges();
      var q = from t in db.tEnvoiExecs.Where(p => data.Select(n => n.ID).Contains(p.ID))
              join en in db.tEnvois on t.EnvoiID equals en.ID
              join c1 in db.tObjClassifiers on en.PeriodichID equals c1.ObjClassifierID into _c1
              from c1 in _c1.DefaultIfEmpty()
              select new
              {
                id = t.ID,
                t.EnvoiID,
                t.Date1,
                t.Date2,
                t.Comment,
                en.TypeInf,
                en.Osnovan,
                en.Mesto,
                en.PoryadPredst,
                en.PeriodichID,
                Periodich = c1.Name,
                en.SrokRask,
                EmailTo = string.Join(", ", db.taLibs.Where(p => p.LConcept == 458622 && p.LParent == 458622 && en.EmailTo.IndexOf(p.LName1) > -1).OrderBy(p => p.LName).Select(o => o.LName.Trim()).ToArray()),
                EmailCc = string.Join(", ", db.taLibs.Where(p => p.LConcept == 458622 && p.LParent == 458622 && en.EmailCc.IndexOf(p.LName1) > -1).OrderBy(p => p.LName).Select(o => o.LName.Trim()).ToArray()),
                en.IsAuto,
                en.IsEnabled
              };

      return q;
    }

    public IEnumerable<dynamic> updEnvoiExec(List<tEnvoiExec> data)
    {
      foreach (var e in data.Where(p => p.ID > 0))
      {
        var q1 = db.tEnvoiExecs.Where(p => p.ID == e.ID).First();
        if (q1 != null)
        {
          q1.Date1 = e.Date1;
          q1.Date2 = e.Date2;
          q1.Comment = e.Comment;
          q1.InDateTime = DateTime.Now;
          db.SubmitChanges();
        }
      }
      db.SubmitChanges();

      var q = from t in db.tEnvoiExecs.Where(p => data.Select(n => n.ID).Contains(p.ID))
              join en in db.tEnvois on t.EnvoiID equals en.ID
              join c1 in db.tObjClassifiers on en.PeriodichID equals c1.ObjClassifierID into _c1
              from c1 in _c1.DefaultIfEmpty()
              select new
              {
                id = t.ID,
                t.EnvoiID,
                t.Date1,
                t.Date2,
                t.Comment,
                en.TypeInf,
                en.Osnovan,
                en.Mesto,
                en.PoryadPredst,
                en.PeriodichID,
                Periodich = c1.Name,
                en.SrokRask,
                EmailTo = string.Join(", ", db.taLibs.Where(p => p.LConcept == 458622 && p.LParent == 458622 && en.EmailTo.IndexOf(p.LName1) > -1).OrderBy(p => p.LName).Select(o => o.LName.Trim()).ToArray()),
                EmailCc = string.Join(", ", db.taLibs.Where(p => p.LConcept == 458622 && p.LParent == 458622 && en.EmailCc.IndexOf(p.LName1) > -1).OrderBy(p => p.LName).Select(o => o.LName.Trim()).ToArray()),
                en.IsAuto,
                en.IsEnabled
              };

      return q;
    }

    public bool delEnvoiExec(List<tEnvoiExec> data)
    {
      try
      {
        IEnumerable<tEnvoiExec> e = db.tEnvoiExecs.Where(p => data.Select(n => n.ID).Contains(p.ID));
        db.tEnvoiExecs.DeleteAllOnSubmit(e);
        db.SubmitChanges();
        return true;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public bool envoiExecCourriel(string host)
    {
      var q = from t in db.tEnvoiExecs.Where(p => p.Date2 == null || p.Date2 > DateTime.Today.AddDays(-DateTime.Today.DayOfYear))
              join en in db.tEnvois.Where(p => p.IsEnabled == true && p.TypeID == 1) on t.EnvoiID equals en.ID
              join c1 in db.tObjClassifiers on en.PeriodichID equals c1.ObjClassifierID into _c1
              from c1 in _c1.DefaultIfEmpty()
              orderby t.Date1
              select new
              {
                id = t.ID,
                t.EnvoiID,
                t.Date1,
                t.Date2,
                en.TypeInf,
                en.Osnovan,
                en.Mesto,
                en.PoryadPredst,
                en.PeriodichID,
                Periodich = c1.Name,
                en.SrokRask,
                Email = string.Join(", ", db.taLibs.Where(p => p.LConcept == 458622 && p.LParent == 458622 && en.EmailTo.IndexOf(p.LName1) > -1).OrderBy(p => p.LName).Select(o => o.LName.Trim()).ToArray())
              };

      SmtpClient sc = new SmtpClient("m.am-uralsib.ru");
      sc.UseDefaultCredentials = true;
      MailMessage message = new MailMessage();
      message.From = new MailAddress("Дирекция комплаенс <assets_msg@am-uralsib.ru>");
      message.To.Add((host.Contains("localhost") || host.Contains("10.158.32.10")) ? "GrishinAV@am-uralsib.ru" : "LikholetovaES@am-uralsib.ru,IvlievaTA@am-uralsib.ru,NikonenkoKV@am-uralsib.ru");
      StringBuilder sb = new StringBuilder();
      sb.Append("<style>td, span, th {font-size:.8em;font-family: \"Segoe UI\", Verdana, Helvetica, Sans-Serif;} span {font-style:italic} th{font-size:.7em}</style>");
      sb.Append("<span>Уважаемые коллеги,<br>напоминаем о сроках предоставления следующей отчетности/информации:<br><br>");
      sb.Append("<table border='1' cellspacing='0'><tr><th>Дата, не позднее</th><th>Дата факт. раскрытия</th><th>Вид отчетности</th><th>Основание</th><th>Место предоставления</th><th>Порядок предоставления</th><th>Периодичность</th><th>Срок направления</th><th>Исполнители</th></tr>");
      foreach (var qd in q)
      {
        sb.AppendFormat("<tr style=\"color:{8}\"><td>{0:dd.MM.yy}</td><td>{9}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td></tr>",
          qd.Date1,
          qd.TypeInf,
          qd.Osnovan,
          qd.Mesto,
          qd.PoryadPredst,
          qd.Periodich,
          qd.SrokRask,
          qd.Email,
          qd.Date2.HasValue ? "gray" : qd.Date1 < DateTime.Today ? "burlywood" : qd.Date1 < DateTime.Today.AddDays(14) ? "red" : qd.Date1 < DateTime.Today.AddDays(28) ? "maroon" : "black",
          qd.Date2.HasValue ? qd.Date2.Value.ToString("dd.MM.yy") : "&nbsp;");
      }
      sb.Append("</table>");

      sb.Append("<br>Подпись: Дирекция комплаенс</span>");
      message.Body = sb.ToString();
      message.IsBodyHtml = true;
      message.Priority = MailPriority.High;
      message.Headers.Add("Importance", "High");
      message.IsBodyHtml = true;
      message.Subject = "Контроль за предоставлением отчетности";
      sc.Send(message);
      return true;
    }

    public bool envoiExecRiCourriel(string host)
    {
      var q = from t in db.tEnvoiExecs.Where(p => p.Date2 == null || p.Date2 > DateTime.Today.AddDays(-DateTime.Today.DayOfYear))
              join en in db.tEnvois.Where(p => p.IsEnabled == true && p.TypeID == 2) on t.EnvoiID equals en.ID
              join c1 in db.tObjClassifiers on en.PeriodichID equals c1.ObjClassifierID into _c1
              from c1 in _c1.DefaultIfEmpty()
              orderby t.Date1
              select new
              {
                id = t.ID,
                t.EnvoiID,
                t.Date1,
                t.Date2,
                en.TypeInf,
                en.Osnovan,
                en.PeriodichID,
                Periodich = c1.Name,
                en.SrokRask,
                EmailTo = string.Join(", ", db.taLibs.Where(p => p.LConcept == 458622 && p.LParent == 458622 && en.EmailTo.IndexOf(p.LName1) > -1).OrderBy(p => p.LName).Select(o => o.LName.Trim()).ToArray()),
                EmailCc = string.Join(", ", db.taLibs.Where(p => p.LConcept == 458622 && p.LParent == 458622 && en.EmailCc.IndexOf(p.LName1) > -1).OrderBy(p => p.LName).Select(o => o.LName.Trim()).ToArray())
              };

      SmtpClient sc = new SmtpClient("m.am-uralsib.ru");
      sc.UseDefaultCredentials = true;
      MailMessage message = new MailMessage();
      message.From = new MailAddress("Дирекция комплаенс <assets_msg@am-uralsib.ru>");
      message.To.Add((host.Contains("localhost") || host.Contains("10.158.32.10")) ? "GrishinAV@am-uralsib.ru" : "LikholetovaES@am-uralsib.ru,IvlievaTA@am-uralsib.ru,NikonenkoKV@am-uralsib.ru");
      StringBuilder sb = new StringBuilder();
      sb.Append("<style>td, span, th {font-size:.8em;font-family: \"Segoe UI\", Verdana, Helvetica, Sans-Serif;} span {font-style:italic} th{font-size:.7em}</style>");
      sb.Append("<span>Уважаемые коллеги,<br>напоминаем о сроках предоставления следующей отчетности/информации:<br><br>");
      sb.Append("<table border='1' cellspacing='0'><tr><th>Дата, не позднее</th><th>Дата факт. раскрытия</th><th>Вид раскрываемой информации</th><th>Основание</th><th>Периодичность</th><th>Срок раскрытия</th><th>Ответственные лица за предоставление информации</th><th>Ответственные лица за раскрытие информации</th></tr>");
      foreach (var qd in q)
      {
        sb.AppendFormat("<tr style=\"color:{7}\"><td>{0:dd.MM.yy}</td><td>{8}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td></tr>",
          qd.Date1,
          qd.TypeInf,
          qd.Osnovan,
          qd.Periodich,
          qd.SrokRask,
          qd.EmailTo,
          qd.EmailCc,
          qd.Date2.HasValue ? "gray" : qd.Date1 < DateTime.Today ? "burlywood" : qd.Date1 < DateTime.Today.AddDays(14) ? "red" : qd.Date1 < DateTime.Today.AddDays(28) ? "maroon" : "black",
          qd.Date2.HasValue ? qd.Date2.Value.ToString("dd.MM.yy") : "&nbsp;");
      }
      sb.Append("</table>");

      sb.Append("<br>Подпись: Дирекция комплаенс</span>");
      message.Body = sb.ToString();
      message.IsBodyHtml = true;
      message.Priority = MailPriority.High;
      message.Headers.Add("Importance", "High");
      message.IsBodyHtml = true;
      message.Subject = "Контроль за сроками раскрытия информации";
      sc.Send(message);
      return true;
    }

    public IEnumerable<dynamic> getConseilList(DateTime? d1, DateTime? d2, int? type, Boolean? nopen, string sort, string dir)
    {
      var q = from e in db.tConseils
              join l1 in db.taLibs.Where(p => p.LConcept == 483545 && p.LParent == 483545) on e.Priorite equals l1.LID1 into l1_
              from l1 in l1_.DefaultIfEmpty()
              select new
              {
                id = e.ID,
                e.EmailCc,
                e.EmailTo,
                e.Violation,
                e.Conseil,
                e.Terme,
                e.Prolongation,
                e.ExecDate,
                e.Possesseur,
                e.Commentaire,
                e.IsEnabled,
                e.PrononceDate,
                e.Priorite,
                PrioriteNom = l1.LName,
                e.MinNomRiskPrice,
                e.MaxNomRiskPrice
              };
      if (type == 0)
      {
        if (d1.HasValue)
        {
          q = q.Where(a => a.PrononceDate >= d1);
        }
        if (d2.HasValue)
        {
          q = q.Where(a => a.PrononceDate <= d2);
        }
      }
      else if (type == 1)
      {
        if (d1.HasValue)
        {
          q = q.Where(a => a.Terme >= d1);
        }
        if (d2.HasValue)
        {
          q = q.Where(a => a.Terme <= d2);
        }
      }
      else if (type == 2)
      {
        if (d1.HasValue)
        {
          q = q.Where(a => a.ExecDate >= d1);
        }
        if (d2.HasValue)
        {
          q = q.Where(a => a.ExecDate <= d2);
        }
      }
      if (nopen == true)
        q = q.Where(a => a.IsEnabled == true);
      if (sort != null) q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
      return q;
    }

    public IEnumerable<dynamic> addConseil(List<tConseil> data)
    {
      data[0].InDateTime = DateTime.Now;
      db.tConseils.InsertAllOnSubmit(data);
      db.SubmitChanges();

      var q = from e in db.tConseils.Where(p => data.Select(n => n.ID).Contains(p.ID))
              join l1 in db.taLibs.Where(p => p.LConcept == 483545 && p.LParent == 483545) on e.Priorite equals l1.LID1 into l1_
              from l1 in l1_.DefaultIfEmpty()
              select new
              {
                id = e.ID,
                e.EmailCc,
                e.EmailTo,
                e.Violation,
                e.Conseil,
                e.Terme,
                e.Prolongation,
                e.ExecDate,
                e.Possesseur,
                e.Commentaire,
                e.IsEnabled,
                e.Priorite,
                PrioriteNom = l1.LName,
                e.MinNomRiskPrice,
                e.MaxNomRiskPrice
              };

      return q;
    }

    public IEnumerable<dynamic> updConseil(List<tConseil> data)
    {
      foreach (var e in data.Where(p => p.ID > 0))
      {
        var q1 = db.tConseils.Where(p => p.ID == e.ID).First();
        if (q1 != null)
        {
          q1.Commentaire = e.Commentaire;
          q1.Conseil = e.Conseil;
          q1.EmailCc = e.EmailCc;
          q1.EmailTo = e.EmailTo;
          q1.InDateTime = DateTime.Now;
          q1.IsEnabled = e.IsEnabled;
          q1.Possesseur = e.Possesseur;
          q1.Prolongation = e.Prolongation;
          q1.Terme = e.Terme;
          q1.Violation = e.Violation;
          q1.Priorite = e.Priorite;
          q1.PrononceDate = e.PrononceDate;
          q1.ExecDate = e.ExecDate;
          q1.MinNomRiskPrice = e.MinNomRiskPrice;
          q1.MaxNomRiskPrice = e.MaxNomRiskPrice;
          db.SubmitChanges();
        }
      }
      db.SubmitChanges();

      var q = from e in db.tConseils.Where(p => data.Select(n => n.ID).Contains(p.ID))
              join l1 in db.taLibs.Where(p => p.LConcept == 483545 && p.LParent == 483545) on e.Priorite equals l1.LID1 into l1_
              from l1 in l1_.DefaultIfEmpty()
              select new
              {
                id = e.ID,
                e.EmailCc,
                e.EmailTo,
                e.Violation,
                e.Conseil,
                e.Terme,
                e.Prolongation,
                e.ExecDate,
                e.Possesseur,
                e.Commentaire,
                e.IsEnabled,
                e.Priorite,
                PrioriteNom = l1.LName,
                e.MinNomRiskPrice,
                e.MaxNomRiskPrice
              };

      return q;
    }

    public bool delConseil(List<tConseil> data)
    {
      try
      {
        IEnumerable<tConseil> e = db.tConseils.Where(p => data.Select(n => n.ID).Contains(p.ID));
        db.tConseils.DeleteAllOnSubmit(e);
        db.SubmitChanges();
        return true;
      }
      catch (Exception /*ex*/)
      {
        return false;
      }
    }

    public IEnumerable<dynamic> getConseilHoraire(int? id)
    {
      var q = from eh in db.tConseilHoraires.Where(p => p.ConseilID == id)
              select new { id = eh.ID, eh.ConseilID, eh.EnvoiHoraireTypeID, EnvoiHoraireType = eh.tEnvoiHoraireType.Name, eh.Day, eh.Month };
      return q;
    }

    public IEnumerable<dynamic> addConseilHoraire(List<tConseilHoraire> data)
    {
      db.tConseilHoraires.InsertAllOnSubmit(data);
      db.SubmitChanges();

      var q = from eh in db.tConseilHoraires.Where(p => data.Select(n => n.ID).Contains(p.ID))
              select new { id = eh.ID, eh.ConseilID, eh.EnvoiHoraireTypeID, EnvoiHoraireType = eh.tEnvoiHoraireType.Name, eh.Day, eh.Month };
      return q;
    }

    public IEnumerable<dynamic> updConseilHoraire(List<tConseilHoraire> data)
    {
      foreach (var e in data.Where(p => p.ID > 0))
      {
        var q1 = db.tConseilHoraires.Where(p => p.ID == e.ID).First();
        if (q1 != null)
        {
          q1.EnvoiHoraireTypeID = e.EnvoiHoraireTypeID;
          q1.Day = e.Day;
          q1.Month = e.Month;
          db.SubmitChanges();
        }
      }
      db.SubmitChanges();

      var q = from eh in db.tConseilHoraires.Where(p => data.Select(n => n.ID).Contains(p.ID))
              select new { id = eh.ID, eh.ConseilID, eh.EnvoiHoraireTypeID, EnvoiHoraireType = eh.tEnvoiHoraireType.Name, eh.Day, eh.Month };

      return q;
    }

    public bool delConseilHoraire(List<tConseilHoraire> data)
    {
      try
      {
        IEnumerable<tConseilHoraire> e = db.tConseilHoraires.Where(p => data.Select(n => n.ID).Contains(p.ID));
        db.tConseilHoraires.DeleteAllOnSubmit(e);
        db.SubmitChanges();
        return true;
      }
      catch (Exception /*ex*/)
      {
        return false;
      }
    }

    public IEnumerable<dynamic> getCPriorite()
    {
      var q = from l in db.taLibs.Where(p => p.LConcept == 483545 && p.LParent == 483545)
              select new { Text = l.LName, Value = l.LID1 };
      return q;
    }

    public IEnumerable<dynamic> getRiskMapList(string sort, string dir)
    {
      var q = from e in db.tRiskMaps
              join li in db.taLibs.Where(p => p.LConcept == 482639 && p.LParent == 482639) on e.Influence equals li.LBInt1 into l1_
              from li in l1_.DefaultIfEmpty()
              join lp in db.taLibs.Where(p => p.LConcept == 482639 && p.LParent == 482639) on e.Probabilite equals lp.LBInt1 into l2_
              from lp in l2_.DefaultIfEmpty()
              join lf in db.taLibs.Where(p => p.LConcept == 482639 && p.LParent == 482639) on e.ControlForce equals lf.LBInt1 into l3_
              from lf in l3_.DefaultIfEmpty()
              join la in db.taLibs.Where(p => p.LConcept == 482644 && p.LParent == 482644) on li.LName + lp.LName + lf.LName equals la.LName into l4_
              from la in l4_.DefaultIfEmpty()
              select new { id = e.ID, e.EmailCc, e.EmailTo, e.BisProc, e.But, e.Control, e.ControlForce, e.Dep, e.IsEnabled, e.EssentielRisk, e.Influence, e.JurPersonne, e.NumRisk, e.PossesseurBut, e.PossesseurControl, e.Probabilite, e.RiskName, InfluenceName = li.LName, ProbabiliteName = lp.LName, ControlForceName = lf.LName, TotalName = la.LName1 };

      if (sort != null) q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
      return q;

    }

    public IEnumerable<dynamic> addRiskMap(List<tRiskMap> data)
    {
      data[0].InDateTime = DateTime.Now;
      db.tRiskMaps.InsertAllOnSubmit(data);
      db.SubmitChanges();

      var q = from e in db.tRiskMaps.Where(p => data.Select(n => n.ID).Contains(p.ID))
              join li in db.taLibs.Where(p => p.LConcept == 482639 && p.LParent == 482639) on e.Influence equals li.LBInt1 into l1_
              from li in l1_.DefaultIfEmpty()
              join lp in db.taLibs.Where(p => p.LConcept == 482639 && p.LParent == 482639) on e.Probabilite equals lp.LBInt1 into l2_
              from lp in l2_.DefaultIfEmpty()
              join lf in db.taLibs.Where(p => p.LConcept == 482639 && p.LParent == 482639) on e.ControlForce equals lf.LBInt1 into l3_
              from lf in l3_.DefaultIfEmpty()
              join la in db.taLibs.Where(p => p.LConcept == 482644 && p.LParent == 482644) on li.LName + lp.LName + lf.LName equals la.LName into l4_
              from la in l4_.DefaultIfEmpty()
              select new { id = e.ID, e.EmailCc, e.EmailTo, e.BisProc, e.But, e.Control, e.ControlForce, e.Dep, e.IsEnabled, e.EssentielRisk, e.Influence, e.JurPersonne, e.NumRisk, e.PossesseurBut, e.PossesseurControl, e.Probabilite, e.RiskName, InfluenceName = li.LName, ProbabiliteName = lp.LName, ControlForceName = lf.LName, TotalName = la.LName1 };

      return q;
    }

    public IEnumerable<dynamic> updRiskMap(List<tRiskMap> data)
    {
      foreach (var e in data.Where(p => p.ID > 0))
      {
        var q1 = db.tRiskMaps.Where(p => p.ID == e.ID).First();
        if (q1 != null)
        {
          q1.BisProc = e.BisProc;
          q1.But = e.But;
          q1.Control = e.Control;
          q1.ControlForce = e.ControlForce;
          q1.Dep = e.Dep;
          q1.EmailCc = e.EmailCc;
          q1.EmailTo = e.EmailTo;
          q1.InDateTime = DateTime.Now;
          q1.IsEnabled = e.IsEnabled;
          q1.EssentielRisk = e.EssentielRisk;
          q1.Influence = e.Influence;
          q1.JurPersonne = e.JurPersonne;
          q1.NumRisk = e.NumRisk;
          q1.PossesseurBut = e.PossesseurBut;
          q1.PossesseurControl = e.PossesseurControl;
          q1.Probabilite = e.Probabilite;
          q1.RiskName = e.RiskName;
          db.SubmitChanges();
        }
      }
      db.SubmitChanges();

      var q = from e in db.tRiskMaps.Where(p => data.Select(n => n.ID).Contains(p.ID))
              join li in db.taLibs.Where(p => p.LConcept == 482639 && p.LParent == 482639) on e.Influence equals li.LBInt1 into l1_
              from li in l1_.DefaultIfEmpty()
              join lp in db.taLibs.Where(p => p.LConcept == 482639 && p.LParent == 482639) on e.Probabilite equals lp.LBInt1 into l2_
              from lp in l2_.DefaultIfEmpty()
              join lf in db.taLibs.Where(p => p.LConcept == 482639 && p.LParent == 482639) on e.ControlForce equals lf.LBInt1 into l3_
              from lf in l3_.DefaultIfEmpty()
              join la in db.taLibs.Where(p => p.LConcept == 482644 && p.LParent == 482644) on li.LName + lp.LName + lf.LName equals la.LName into l4_
              from la in l4_.DefaultIfEmpty()
              select new { id = e.ID, e.EmailCc, e.EmailTo, e.BisProc, e.But, e.Control, e.ControlForce, e.Dep, e.IsEnabled, e.EssentielRisk, e.Influence, e.JurPersonne, e.NumRisk, e.PossesseurBut, e.PossesseurControl, e.Probabilite, e.RiskName, InfluenceName = li.LName, ProbabiliteName = lp.LName, ControlForceName = lf.LName, TotalName = la.LName1 };
      return q;
    }

    public bool delRiskMap(List<tRiskMap> data)
    {
      try
      {
        IEnumerable<tRiskMap> e = db.tRiskMaps.Where(p => data.Select(n => n.ID).Contains(p.ID));
        db.tRiskMaps.DeleteAllOnSubmit(e);
        db.SubmitChanges();
        return true;
      }
      catch (Exception /*ex*/)
      {
        return false;
      }
    }

    public IEnumerable<dynamic> getRMLevel()
    {
      var q = from l in db.taLibs.Where(p => p.LConcept == 482639 && p.LParent == 482639)
              select new { Text = l.LName, Value = l.LBInt1 };
      return q;
    }

    public IEnumerable<dynamic> getRiskMapHoraire(int? id)
    {
      var q = from eh in db.tRiskMapHoraires.Where(p => p.RiskMapID == id)
              select new { id = eh.ID, eh.RiskMapID, eh.EnvoiHoraireTypeID, EnvoiHoraireType = eh.tEnvoiHoraireType.Name, eh.Day, eh.Month };
      return q;
    }

    public IEnumerable<dynamic> addRiskMapHoraire(List<tRiskMapHoraire> data)
    {
      db.tRiskMapHoraires.InsertAllOnSubmit(data);
      db.SubmitChanges();

      var q = from eh in db.tRiskMapHoraires.Where(p => data.Select(n => n.ID).Contains(p.ID))
              select new { id = eh.ID, eh.RiskMapID, eh.EnvoiHoraireTypeID, EnvoiHoraireType = eh.tEnvoiHoraireType.Name, eh.Day, eh.Month };
      return q;
    }

    public IEnumerable<dynamic> updRiskMapHoraire(List<tRiskMapHoraire> data)
    {
      foreach (var e in data.Where(p => p.ID > 0))
      {
        var q1 = db.tRiskMapHoraires.Where(p => p.ID == e.ID).First();
        if (q1 != null)
        {
          q1.EnvoiHoraireTypeID = e.EnvoiHoraireTypeID;
          q1.Day = e.Day;
          q1.Month = e.Month;
          db.SubmitChanges();
        }
      }
      db.SubmitChanges();

      var q = from eh in db.tRiskMapHoraires.Where(p => data.Select(n => n.ID).Contains(p.ID))
              select new { id = eh.ID, eh.RiskMapID, eh.EnvoiHoraireTypeID, EnvoiHoraireType = eh.tEnvoiHoraireType.Name, eh.Day, eh.Month };

      return q;
    }

    public bool delRiskMapHoraire(List<tRiskMapHoraire> data)
    {
      try
      {
        IEnumerable<tRiskMapHoraire> e = db.tRiskMapHoraires.Where(p => data.Select(n => n.ID).Contains(p.ID));
        db.tRiskMapHoraires.DeleteAllOnSubmit(e);
        db.SubmitChanges();
        return true;
      }
      catch (Exception /*ex*/)
      {
        return false;
      }
    }

    public bool riskMapCourriel(List<int> id, string host)
    {

      var q = (from r1 in
                 (
                   (from r in db.tRiskMaps.Where(r => id.Contains(r.ID) && r.EmailTo != null).ToList()
                    from Email in db.tRiskMaps.Where(r1 => r1.ID == r.ID).ToList().Select(p => p.EmailTo).SingleOrDefault().Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).AsQueryable()
                    select new { i = 0, r.ID, Email })
                   .Union(from r in db.tRiskMaps.Where(r => id.Contains(r.ID) && r.EmailCc != null).ToList()
                          from Email in db.tRiskMaps.Where(r1 => r1.ID == r.ID).ToList().Select(p => p.EmailCc).SingleOrDefault().Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).AsQueryable()
                          select new { i = 1, r.ID, Email }))
               join e in db.tRiskMaps on r1.ID equals e.ID
               join li in db.taLibs.Where(p => p.LConcept == 482639 && p.LParent == 482639) on e.Influence equals li.LBInt1 into l1_
               from li in l1_.DefaultIfEmpty()
               join lp in db.taLibs.Where(p => p.LConcept == 482639 && p.LParent == 482639) on e.Probabilite equals lp.LBInt1 into l2_
               from lp in l2_.DefaultIfEmpty()
               join lf in db.taLibs.Where(p => p.LConcept == 482639 && p.LParent == 482639) on e.ControlForce equals lf.LBInt1 into l3_
               from lf in l3_.DefaultIfEmpty()
               join la in db.taLibs.Where(p => p.LConcept == 482644 && p.LParent == 482644) on li.LName + lp.LName + lf.LName equals la.LName into l4_
               from la in l4_.DefaultIfEmpty()
               select new
               {
                 r1.i,
                 r1.Email,
                 e.Dep,
                 e.BisProc,
                 e.NumRisk,
                 e.RiskName,
                 InfluenceName = li.LName,
                 ProbabiliteName = lp.LName,
                 ControlForceName = lf.LName,
                 TotalName = la.LName1,
                 e.EssentielRisk,
                 e.IsEnabled,
                 c1 = (r1.i == 0 ? e.But : e.Control),
                 c2 = (r1.i == 0 ? e.PossesseurBut : e.PossesseurControl),
                 e.PossesseurBut,
                 e.PossesseurControl
               })
                .GroupBy(l => new { l.Email, l.i });

      if (q != null)
      {
        SmtpClient sc = new SmtpClient("m.am-uralsib.ru");
        sc.UseDefaultCredentials = true;
        foreach (var r in q)
        {
          MailMessage message = new MailMessage();
          message.To.Add((host.Contains("localhost") || host.Contains("10.158.32.10")) || r.Key == null ? "GrishinAV@am-uralsib.ru" : r.Key.Email);
          message.From = new MailAddress("Дирекция риск-менеджмента <assets_msg@am-uralsib.ru>");
          var template = new MO.Areas.Code.Views.Envoi.riskMapCourriel { q = r, i = r.Key.i };
          message.Body = template.TransformText();
          message.IsBodyHtml = true;
          message.Priority = MailPriority.High;
          message.Headers.Add("Importance", "High");
          message.Subject = "Операционные контроли";
          sc.Send(message);
        }
        return true;
      }
      return false;
    }

    public IEnumerable<dynamic> getDeclViol(DateTime? d1, DateTime? d2, bool? op, string sort, string dir)
    {
      var qde = db.tInvDeclErrorJournals.Where(p => 1 == 1);
      if (op == true)
        qde = db.tInvDeclErrorJournals.Where(p => p.EndDate == null);
      else
        qde = db.tInvDeclErrorJournals.Where(p => p.DDate >= d1 && p.DDate <= d2);

      var q = from de in qde
              join fi in db.tFinancialInstitutions on de.FinInstID equals fi.FinancialInstitutionID into _fi
              from fi in _fi.DefaultIfEmpty()
              from t in
                (from tr in db.tTreaties.Where(t1 => t1.FinancialInstitutionID == de.FinInstID && t1.IsDisabled == 0)
                 join ttt in db.tTreatyTreatyTypes.Where(i => i.TreatyTypeID == 1 || i.TreatyTypeID == 2) on tr.TreatyID equals ttt.TreatyID
                 join ocr in db.tObjClsRelations.Where(s => s.ObjType == 1631275800) on tr.TreatyID equals ocr.ObjectID
                 join oc in db.tObjClassifiers.Where(s => s.ParentID == 180) on ocr.ObjClassifierID equals oc.ObjClassifierID
                 select new { oc.Name }).Take(1).DefaultIfEmpty()
              join idw in db.sInvestmentDeclarationWheres on de.InvestDeclWhereID equals idw.InvestmentDeclarationWhereID into _idw
              from idw in _idw.DefaultIfEmpty()
              join c1 in db.tObjClassifiers on de.KindViolation equals c1.ObjClassifierID into _c1
              from c1 in _c1.DefaultIfEmpty()
              join c2 in db.tObjClassifiers on de.MethodRemoval equals c2.ObjClassifierID into _c2
              from c2 in _c2.DefaultIfEmpty()
              join c3 in db.tObjClassifiers on de.PeriodRemoval equals c3.ObjClassifierID into _c3
              from c3 in _c3.DefaultIfEmpty()
              from fis in db.tFinancialInstitutions.Where(p => idw.FLAG_Group == 4 && de.IssuerID == p.FinancialInstitutionID).DefaultIfEmpty()
              select new
              {
                de.DDate,
                de.Descr,
                de.EndDate,
                de.FinInstID,
                de.ID,
                InvestDeclID = idw.InvestmentDeclarationID,
                idw.FLAG_Group,
                de.InvestDeclWhereID,
                de.IssuerID,
                de.KindViolation,
                de.MaxValue,
                de.MethodRemoval,
                de.MinValue,
                de.PeriodRemoval,
                de.Reason,
                de.Value,
                kvName = c1.Name,
                mrName = c2.Name,
                prName = c3.Name,
                fiBrief = (fi.NameBrief ?? "").TrimEnd(),
                idw.NameWhere,
                fisBrief = (fis.NameBrief ?? "").TrimEnd(),
                ocName = t.Name,
                de.TermDate
              };
      if (sort != null)
      {
        q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
      }
      return q.ToList();
    }

    public IEnumerable<dynamic> addDeclViol(List<tInvDeclErrorJournal> data)
    {
      foreach (var d in data)
        d.InDateTime = DateTime.Now;

      db.tInvDeclErrorJournals.InsertAllOnSubmit(data);
      db.SubmitChanges();

      var q = from de in db.tInvDeclErrorJournals.Where(p => data.Select(n => n.ID).Contains(p.ID))
              join fi in db.tFinancialInstitutions on de.FinInstID equals fi.FinancialInstitutionID into _fi
              from fi in _fi.DefaultIfEmpty()
              from t in
                (from tr in db.tTreaties.Where(t1 => t1.FinancialInstitutionID == de.FinInstID && t1.IsDisabled == 0)
                 join ttt in db.tTreatyTreatyTypes.Where(i => i.TreatyTypeID == 1 || i.TreatyTypeID == 2) on tr.TreatyID equals ttt.TreatyID
                 join ocr in db.tObjClsRelations.Where(s => s.ObjType == 1631275800) on tr.TreatyID equals ocr.ObjectID
                 join oc in db.tObjClassifiers.Where(s => s.ParentID == 180) on ocr.ObjClassifierID equals oc.ObjClassifierID
                 select new { oc.Name }).Take(1).DefaultIfEmpty()
              join idw in db.sInvestmentDeclarationWheres on de.InvestDeclWhereID equals idw.InvestmentDeclarationWhereID into _idw
              from idw in _idw.DefaultIfEmpty()
              join c1 in db.tObjClassifiers on de.KindViolation equals c1.ObjClassifierID into _c1
              from c1 in _c1.DefaultIfEmpty()
              join c2 in db.tObjClassifiers on de.MethodRemoval equals c2.ObjClassifierID into _c2
              from c2 in _c2.DefaultIfEmpty()
              join c3 in db.tObjClassifiers on de.PeriodRemoval equals c3.ObjClassifierID into _c3
              from c3 in _c3.DefaultIfEmpty()
              join fis in db.tFinancialInstitutions on de.IssuerID equals fis.FinancialInstitutionID into _fis
              from fis in _fis.DefaultIfEmpty()
              select new
              {
                de.DDate,
                de.Descr,
                de.EndDate,
                de.FinInstID,
                de.ID,
                InvestDeclID = idw.InvestmentDeclarationID,
                idw.FLAG_Group,
                de.InvestDeclWhereID,
                de.IssuerID,
                de.KindViolation,
                de.MaxValue,
                de.MethodRemoval,
                de.MinValue,
                de.PeriodRemoval,
                de.Reason,
                de.Value,
                kvName = c1.Name,
                mrName = c2.Name,
                prName = c3.Name,
                fiBrief = (fi.NameBrief ?? "").TrimEnd(),
                idw.NameWhere,
                fisBrief = (fis.NameBrief ?? "").TrimEnd(),
                ocName = t.Name,
                de.TermDate
              };
      return q;
    }

    public IEnumerable<dynamic> updDeclViol(List<tInvDeclErrorJournal> data)
    {
      foreach (var e in data.Where(p => p.ID > 0))
      {
        var q1 = db.tInvDeclErrorJournals.Where(p => p.ID == e.ID).First();
        if (q1 != null)
        {
          q1.DDate = e.DDate;
          q1.Descr = e.Descr;
          q1.EndDate = e.EndDate;
          q1.FinInstID = e.FinInstID;
          q1.InDateTime = DateTime.Now;
          q1.InvestDeclWhereID = e.InvestDeclWhereID;
          q1.IssuerID = e.IssuerID;
          q1.KindViolation = e.KindViolation;
          q1.LastValue = e.LastValue;
          q1.MaxValue = e.MaxValue;
          q1.MethodRemoval = e.MethodRemoval;
          q1.MinValue = e.MinValue;
          q1.PeriodRemoval = e.PeriodRemoval;
          q1.Reason = e.Reason;
          q1.TermDate = e.TermDate;
          q1.Value = e.Value;
          db.SubmitChanges();
        }
      }
      db.SubmitChanges();

      var q = from de in db.tInvDeclErrorJournals.Where(p => data.Select(n => n.ID).Contains(p.ID))
              join fi in db.tFinancialInstitutions on de.FinInstID equals fi.FinancialInstitutionID into _fi
              from fi in _fi.DefaultIfEmpty()
              from t in
                (from tr in db.tTreaties.Where(t1 => t1.FinancialInstitutionID == de.FinInstID && t1.IsDisabled == 0)
                 join ttt in db.tTreatyTreatyTypes.Where(i => i.TreatyTypeID == 1 || i.TreatyTypeID == 2) on tr.TreatyID equals ttt.TreatyID
                 join ocr in db.tObjClsRelations.Where(s => s.ObjType == 1631275800) on tr.TreatyID equals ocr.ObjectID
                 join oc in db.tObjClassifiers.Where(s => s.ParentID == 180) on ocr.ObjClassifierID equals oc.ObjClassifierID
                 select new { oc.Name }).Take(1).DefaultIfEmpty()
              join idw in db.sInvestmentDeclarationWheres on de.InvestDeclWhereID equals idw.InvestmentDeclarationWhereID into _idw
              from idw in _idw.DefaultIfEmpty()
              join c1 in db.tObjClassifiers on de.KindViolation equals c1.ObjClassifierID into _c1
              from c1 in _c1.DefaultIfEmpty()
              join c2 in db.tObjClassifiers on de.MethodRemoval equals c2.ObjClassifierID into _c2
              from c2 in _c2.DefaultIfEmpty()
              join c3 in db.tObjClassifiers on de.PeriodRemoval equals c3.ObjClassifierID into _c3
              from c3 in _c3.DefaultIfEmpty()
              join fis in db.tFinancialInstitutions on de.IssuerID equals fis.FinancialInstitutionID into _fis
              from fis in _fis.DefaultIfEmpty()
              select new
              {
                de.DDate,
                de.Descr,
                de.EndDate,
                de.FinInstID,
                de.ID,
                InvestDeclID = idw.InvestmentDeclarationID,
                idw.FLAG_Group,
                de.InvestDeclWhereID,
                de.IssuerID,
                de.KindViolation,
                de.MaxValue,
                de.MethodRemoval,
                de.MinValue,
                de.PeriodRemoval,
                de.Reason,
                de.Value,
                kvName = c1.Name,
                mrName = c2.Name,
                prName = c3.Name,
                fiBrief = (fi.NameBrief ?? "").TrimEnd(),
                idw.NameWhere,
                fisBrief = (fis.NameBrief ?? "").TrimEnd(),
                ocName = t.Name,
                de.TermDate
              };
      return q;

    }

    public bool delDeclViol(List<tInvDeclErrorJournal> data)
    {
      try
      {
        IEnumerable<tInvDeclErrorJournal> e = db.tInvDeclErrorJournals.Where(p => data.Select(n => n.ID).Contains(p.ID));
        db.tInvDeclErrorJournals.DeleteAllOnSubmit(e);
        db.SubmitChanges();
        return true;
      }
      catch (Exception /*ex*/)
      {
        return false;
      }
    }

    public IQueryable<dynamic> GetFinInsts(string q, int limit)
    {
      return (from f in db.tFinancialInstitutions.Where(fi => fi.NameBrief.Contains(q) || fi.Name.Contains(q))
              join c in
                ((from t in db.tTreaties.Where(tr => tr.IsDisabled == 0 && (tr.DateFinish == new DateTime(1900, 1, 1) || tr.DateFinish >= DateTime.Today))
                  join ttt in db.tTreatyTreatyTypes.Where(ttt => new int[] { 1, 2 }.Contains(ttt.TreatyTypeID)) on t.TreatyID equals ttt.TreatyID
                  select new { t.FinancialInstitutionID }).Distinct()) on f.FinancialInstitutionID equals c.FinancialInstitutionID
              orderby f.NameBrief
              select new
              {
                id = f.FinancialInstitutionID,
                brief = f.NameBrief.Trim(),
                name = f.Name.Trim()
              })
              .Take(limit);
    }

    public dynamic GetFinInst(int FinInstID)
    {
      var q = (from f in db.tFinancialInstitutions
               where f.FinancialInstitutionID == FinInstID
               select new
               {
                 id = f.FinancialInstitutionID,
                 brief = f.NameBrief.Trim(),
                 name = f.Name.Trim()
               })
             .FirstOrDefault();
      return q;
    }

    public dynamic GetDeclByInstID(int? id)
    {
      var q = from d in db.sInvestmentDeclarationLinks.Where(f => f.FinancialInstitutionID == id && f.Enb == 'T')
              join i in db.sInvestmentDeclarations.Where(f => f.Enb == 'T') on d.InvestmentDeclarationID equals i.InvestmentDeclarationID
              select new { InvestDeclID = d.InvestmentDeclarationID, i.Name };
      return q;
    }

    public dynamic GetDeclWhereByDeclID(int? id, int? wid)
    {
      if (id == null)
      {
        var w = db.sInvestmentDeclarationWheres.FirstOrDefault(f => f.InvestmentDeclarationWhereID == wid);
        if (w != null)
          id = w.InvestmentDeclarationID;
      }
      var q1 = from d in db.sInvestmentDeclarationWheres.Where(f => f.InvestmentDeclarationID == id && f.Enb == 'T')
               orderby d.NameWhere
               select new { id = d.InvestmentDeclarationWhereID, Name = d.NameWhere, d.StartValue, d.StopValue, d.FLAG_Group };
      return q1;
    }

    public IEnumerable<dynamic> GetObjClsByParent(int id)
    {
      return (from oc in db.tObjClassifiers
              where oc.ParentID == id
              select new
              {
                id = oc.ObjClassifierID,
                name = oc.Name
              });
    }

    public IEnumerable<dynamic> GetIssuers(int? id, DateTime? d)
    {
      var q = (from t in db.tTreaties.Where(tr => tr.IsDisabled == 0 && tr.FinancialInstitutionID == id)
               join ttt in db.tTreatyTreatyTypes.Where(ttt => new int[] { 1, 2 }.Contains(ttt.TreatyTypeID)) on t.TreatyID equals ttt.TreatyID
               join a in db.tAccounts on t.TreatyID equals a.TreatyID
               join ab in db.tAccountBalances.Where(f => f.BalanceDate == d) on a.AccountID equals ab.AccountID
               join s in db.tSecurities.Where(f => f.IssuerID != null) on a.SecIssuerID equals s.SecurityID
               join sb in db.tSecurities.Where(f => f.IssuerID != null) on s.BaseSecurityID equals sb.SecurityID into _sb
               from sb in _sb.DefaultIfEmpty()
               join fi in db.tFinancialInstitutions on sb.IssuerID == null ? s.IssuerID : sb.IssuerID equals fi.FinancialInstitutionID
               orderby fi.NameBrief
               select new { id = fi.FinancialInstitutionID, name = fi.NameBrief.TrimEnd() }).Distinct();
      return q;
    }

    public bool declViolCourriel(string host)
    {
      var q = from de in db.tInvDeclErrorJournals.Where(p => p.EndDate == null)
              join fi in db.tFinancialInstitutions on de.FinInstID equals fi.FinancialInstitutionID
              join idw in db.sInvestmentDeclarationWheres on de.InvestDeclWhereID equals idw.InvestmentDeclarationWhereID into _idw
              from idw in _idw.DefaultIfEmpty()
              orderby de.TermDate
              select new
              {
                fiName = fi.NameBrief,
                de.DDate,
                idw.NameWhere,
                de.Descr,
                de.Value,
                de.MaxValue,
                de.Reason,
                de.TermDate
              };

      SmtpClient sc = new SmtpClient("m.am-uralsib.ru");
      sc.UseDefaultCredentials = true;
      MailMessage message = new MailMessage();
      message.From = new MailAddress("MiddleOffice <assets_msg@am-uralsib.ru>");
      message.To.Add((host.Contains("localhost") || host.Contains("10.158.32.10")) ? "GrishinAV@am-uralsib.ru" : "TemichevIV@am-uralsib.ru,SuminaOI@am-uralsib.ru,VlasenkoAS@am-uralsib.ru,LikholetovaES@am-uralsib.ru,IvlievaTA@am-uralsib.ru,FomenkoIV@am-uralsib.ru,AlekseevAV@am-uralsib.ru,OsipaDG@am-uralsib.ru,NikonenkoKV@am-uralsib.ru,NikolenkoRYu@am-uralsib.ru");
      var template = new MO.Areas.Code.Views.Envoi.declViolCourriel { q = q, dt = db.tWorkDates.Where(w => w.WorkDate < DateTime.Today).OrderByDescending(w => w.WorkDate).First().WorkDate };
      message.Body = template.TransformText();
      message.IsBodyHtml = true;
      message.Priority = MailPriority.High;
      message.Headers.Add("Importance", "High");
      message.IsBodyHtml = true;
      message.Subject = "Нарушения в портфелях клиентов";
      sc.Send(message);
      return true;
    }

    public DateTime? getTermDate(DateTime? d, int? prid)
    {
      DateTime? ret = null;
      if (d.HasValue)
      {
        if (prid == 6251)
          ret = d.Value.AddDays(30);
        else if (prid == 6252)
          ret = db.tWorkDates.Where(w => w.WorkDate > d).OrderBy(w => w.WorkDate).Take(3).Max(w => w.WorkDate);
        else if (prid == 6253)
          ret = d.Value.AddMonths(6);
        else if (prid == 6254)
          ret = d.Value.AddYears(1);
        else if (prid == 66111)
          ret = d.Value.AddDays(7);
        else if (prid == 66112)
          ret = d.Value.AddDays(14);
      }
      return ret;
    }

    public IEnumerable<dynamic> getEnvoiExecByDepList(DateTime? d1, DateTime? d2, bool? IsExec, string sort, string dir)
    {
      var q1 = db.tEnvoiExecs.Where(p => 1 == 1);
      if (d1.HasValue)
        q1 = q1.Where(p => p.Date1 >= d1);
      if (d2.HasValue)
        q1 = q1.Where(p => p.Date1 <= d2);
      if (IsExec == true)
        q1 = q1.Where(p => !p.Date2.HasValue && p.tEnvoi.IsEnabled == true);
      var q = from t in q1
              join en in db.tEnvois.Where(p => p.TypeID == 1 && p.EmailTo.Contains("KalenskyAA@uralsib.ru")) on t.EnvoiID equals en.ID
              join c1 in db.tObjClassifiers on en.PeriodichID equals c1.ObjClassifierID into _c1
              from c1 in _c1.DefaultIfEmpty()
              select new
              {
                id = t.ID,
                t.EnvoiID,
                t.Date1,
                t.Date2,
                en.TypeInf,
                en.Osnovan,
                en.Mesto,
                en.PoryadPredst,
                en.PeriodichID,
                Periodich = c1.Name,
                en.SrokRask,
                EmailTo = string.Join(", ", db.taLibs.Where(p => p.LConcept == 458622 && p.LParent == 458622 && en.EmailTo.IndexOf(p.LName1) > -1).OrderBy(p => p.LName).Select(o => o.LName.Trim()).ToArray()),
                EmailCc = string.Join(", ", db.taLibs.Where(p => p.LConcept == 458622 && p.LParent == 458622 && en.EmailCc.IndexOf(p.LName1) > -1).OrderBy(p => p.LName).Select(o => o.LName.Trim()).ToArray()),
                en.IsAuto,
                en.IsEnabled,
                t.Comment,
                en.Responsible,
                t.Date3
              };
      q = q.OrderBy(sort ?? "Date1", dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
      return q;
    }

    public IEnumerable<dynamic> updEnvoiExecByDep(List<EnvoiDep> data)
    {
      foreach (var e in data.Where(p => p.ID > 0))
      {
        var q1 = db.tEnvoiExecs.Where(p => p.ID == e.ID).FirstOrDefault();
        if (q1 != null)
        {
          q1.Comment = e.Comment;
          q1.Date3 = e.Date3;
          q1.InDateTime = DateTime.Now;
        }
        var q2 = db.tEnvois.Where(p => p.ID == e.EnvoiID).FirstOrDefault();
        if (q2 != null)
        {
          if (q2.Responsible != e.Responsible)
            q2.Responsible = e.Responsible;
        }
      }
      db.SubmitChanges();

      var q = from t in db.tEnvoiExecs.Where(p => data.Select(n => n.ID).Contains(p.ID))
              join en in db.tEnvois on t.EnvoiID equals en.ID
              join c1 in db.tObjClassifiers on en.PeriodichID equals c1.ObjClassifierID into _c1
              from c1 in _c1.DefaultIfEmpty()
              select new
              {
                id = t.ID,
                t.EnvoiID,
                t.Date1,
                t.Date2,
                en.TypeInf,
                en.Osnovan,
                en.Mesto,
                en.PoryadPredst,
                en.PeriodichID,
                Periodich = c1.Name,
                en.SrokRask,
                EmailTo = string.Join(", ", db.taLibs.Where(p => p.LConcept == 458622 && p.LParent == 458622 && en.EmailTo.IndexOf(p.LName1) > -1).OrderBy(p => p.LName).Select(o => o.LName.Trim()).ToArray()),
                EmailCc = string.Join(", ", db.taLibs.Where(p => p.LConcept == 458622 && p.LParent == 458622 && en.EmailCc.IndexOf(p.LName1) > -1).OrderBy(p => p.LName).Select(o => o.LName.Trim()).ToArray()),
                en.IsAuto,
                en.IsEnabled,
                t.Comment,
                en.Responsible,
                t.Date3
              };

      return q;
    }

  }
}