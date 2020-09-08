using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using MO.Models;
using MvcContrib.Sorting;

namespace MO.Areas.Code.Models
{
  public interface IJurRepository
  {
    IEnumerable<dynamic> getWarrantList(string sort, string dir, DateTime? d1, DateTime? d2, int? type, bool? all);
    IEnumerable<dynamic> addWarrant(List<tWarrant> data, string UserName);
    IEnumerable<dynamic> updWarrant(List<tWarrant> data, string UserName);
    bool delWarrant(List<tWarrant> data);
    bool warrantCourriel(string host);
    tWarrant getWarrant(int? ID);
    bool closeWarrant(int? ID, string UserName);
  }

  public class JurRepository : IJurRepository
  {
    private MiddleOfficeDataContext db = new MiddleOfficeDataContext() { CommandTimeout = 600 };

    public IEnumerable<dynamic> getWarrantList(string sort, string dir, DateTime? d1, DateTime? d2, int? type, bool? all)
    {
      var q = from i in db.tWarrants
              select new
              {
                i.ID,
                i.Nomer,
                i.Principal,
                i.Confidant,
                i.Place,
                i.DateB,
                i.DateE,
                i.Functions,
                i.Initiator,
                i.DateCancel,
                i.UserName
              };
      if (d1.HasValue && type == 0)
      {
        q = q.Where(a => a.DateB >= d1);
      }
      if (d2.HasValue && type == 0)
      {
        q = q.Where(a => a.DateB <= d2);
      }
      if (d1.HasValue && type == 1)
      {
        q = q.Where(a => a.DateE >= d1);
      }
      if (d2.HasValue && type == 1)
      {
        q = q.Where(a => a.DateE <= d2);
      }
      if (all == false)
      {
        q = q.Where(a => a.DateCancel == null);
      }
      if (sort != null) q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
      return q;
    }

    public IEnumerable<dynamic> addWarrant(List<tWarrant> data, string UserName)
    {
      db.tWarrants.InsertAllOnSubmit(data);
      data[0].InDateTime = DateTime.Now;
      data[0].UserName = UserName;
      db.SubmitChanges();

      var q = from i in db.tWarrants.Where(p => data.Select(n => n.ID).Contains(p.ID))
              select new
              {
                i.ID,
                i.Nomer,
                i.Principal,
                i.Confidant,
                i.Place,
                i.DateB,
                i.DateE,
                i.Functions,
                i.Initiator,
                i.DateCancel,
                i.UserName
              };
      return q;
    }

    public IEnumerable<dynamic> updWarrant(List<tWarrant> data, string UserName)
    {
      foreach (var e in data.Where(p => p.ID > 0))
      {
        var q1 = db.tWarrants.Where(p => p.ID == e.ID).First();
        if (q1 != null)
        {
          q1.Confidant = e.Confidant;
          q1.DateB = e.DateB;
          q1.DateCancel = e.DateCancel;
          q1.DateE = e.DateE;
          q1.Functions = e.Functions;
          q1.InDateTime = DateTime.Now;
          q1.Initiator = e.Initiator;
          q1.Nomer = e.Nomer;
          q1.Place = e.Place;
          q1.Principal = e.Principal;
          q1.UserName = UserName;
        }
      }
      db.SubmitChanges();

      var q = from i in db.tWarrants.Where(p => data.Select(n => n.ID).Contains(p.ID))
              select new
              {
                i.ID,
                i.Nomer,
                i.Principal,
                i.Confidant,
                i.Place,
                i.DateB,
                i.DateE,
                i.Functions,
                i.Initiator,
                i.DateCancel,
                i.UserName
              };
      return q;
    }

    public bool delWarrant(List<tWarrant> data)
    {
      try
      {
        IEnumerable<tWarrant> e = db.tWarrants.Where(p => data.Select(n => n.ID).Contains(p.ID));
        db.tWarrants.DeleteAllOnSubmit(e);
        db.SubmitChanges();
        return true;
      }
      catch (Exception /*ex*/)
      {
        return false;
      }
    }

    public bool warrantCourriel(string host)
    {
      var q = (from i in db.tWarrants.Where(p => p.DateCancel == null && p.DateE <= DateTime.Today.AddDays(7))
               select new
               {
                 i.ID,
                 i.Nomer,
                 i.Principal,
                 i.Confidant,
                 i.Place,
                 i.DateB,
                 i.DateE,
                 i.Functions,
                 i.Initiator,
                 i.DateCancel
               }).ToList();
      if (q.Count > 0)
      {
        SmtpClient sc = new SmtpClient("m.am-uralsib.ru");
        sc.UseDefaultCredentials = true;
        MailMessage message = new MailMessage();
        message.From = new MailAddress("MiddleOffice <assets_msg@am-uralsib.ru>");
        message.To.Add((host.Contains("localhost") || host.Contains("10.158.32.10")) ? "GrishinAV@am-uralsib.ru" : "NikonenkoKV@am-uralsib.ru,FedorinovVE@am-uralsib.ru,LikholetovaES@am-uralsib.ru,IvlievaTA@am-uralsib.ru");
        var template = new MO.Areas.Code.Views.Jur.warrantCourriel { q = q, host = host };
        message.Body = template.TransformText();
        message.IsBodyHtml = true;
        message.Priority = MailPriority.High;
        message.Headers.Add("Importance", "High");
        message.IsBodyHtml = true;
        message.Subject = "Доверенности, истекающие через неделю и менее";
        sc.Send(message);
        return true;
      }
      return false;
    }


    public tWarrant getWarrant(int? ID)
    {
      var q = (from i in db.tWarrants
               where i.ID == ID
               select i
               ).FirstOrDefault();
      return q;
    }

    public bool closeWarrant(int? ID, string UserName)
    {
      if (ID.HasValue)
      {
        try
        {
          var q1 = db.tWarrants.Where(p => p.ID == ID).First();
          if (q1 != null)
          {
            if (q1.DateCancel == null)
            {
              q1.DateCancel = DateTime.Today;
              q1.InDateTime = DateTime.Now;
              q1.UserName = UserName;
              db.SubmitChanges();
            }
          }
          return true;
        }
        catch (Exception /*ex*/)
        {
          return false;
        }
      }
      return false;
    }
  }
}