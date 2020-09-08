using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using MO.Helpers;
using MO.Models;
using MvcContrib.Sorting;
using System.Text.RegularExpressions;

namespace MO.Areas.Code.Models
{
  public interface IRegDocRepository
  {
    IEnumerable<dynamic> getRegDocList(string sort, string dir, DateTime? d1, DateTime? d2, int? type, Boolean? sd);
    IEnumerable<dynamic> addRegDoc(List<tRegDoc> data, string UserName);
    IEnumerable<dynamic> updRegDoc(List<tRegDoc> data, string UserName);
    bool delRegDoc(List<tRegDoc> data);
    bool regdocCourriel(int? id, string host);
    tRegDoc getRegDoc(int? ID);
    IEnumerable<dynamic> getEMailList(string sort, string dir);
    IEnumerable<dynamic> GetObjClsByParent(int id);
    string getNextRegNum1();
    //string getNextRegNum();

    IEnumerable<dynamic> getOutgoingList(string sort, string dir, DateTime? d1, DateTime? d2);
    IEnumerable<dynamic> addOutgoing(List<tOutgoing> data, string UserName);
    IEnumerable<dynamic> updOutgoing(List<tOutgoing> data, string UserName);
    bool delOutgoing(List<tOutgoing> data);
    string getNextOutNum();
  }

  public class RegDocRepository : IRegDocRepository
  {
    private MiddleOfficeDataContext db = new MiddleOfficeDataContext() { CommandTimeout = 600 };

    public IEnumerable<dynamic> getRegDocList(string sort, string dir, DateTime? d1, DateTime? d2, int? type, Boolean? sd)
    {
      var q = from i in db.tRegDocs
              join oc in db.tObjClassifiers on i.TypeID equals oc.ObjClassifierID into oc_
              from oc in oc_.DefaultIfEmpty()
              join ocl in db.tObjClassifiers on i.LicenseID equals ocl.ObjClassifierID into ocl_
              from ocl in ocl_.DefaultIfEmpty()
              select new
              {
                i.Id,
                i.ADate,
                i.ANum,
                i.Directed,
                i.EmailTo,
                //EmailToName = string.Join(", ", db.taLibs.Where(p => p.LConcept == 458622 && p.LParent == 458622 && i.EmailTo.IndexOf(p.LName1) > -1).OrderBy(p => p.LName).Select(o => o.LName.Trim()).ToArray()),
                i.EmailCc,
                //EmailCcName = string.Join(", ", db.taLibs.Where(p => p.LConcept == 458622 && p.LParent == 458622 && i.EmailCc.IndexOf(p.LName1) > -1).OrderBy(p => p.LName).Select(o => o.LName.Trim()).ToArray()),
                i.InDateTime,
                i.ODate,
                i.RegNum,
                i.TDate,
                i.Theme,
                i.TypeID,
                TypeName = oc.Name,
                i.FileNameI,
                i.FileNameO,
                i.LotusLinkI,
                i.UserName,
                i.Comment,
                i.Resolution,
                i.DocDate,
                i.DocNum,
                i.IsAcquaintance,
                i.IsComplaint,
                i.LicenseID,
                LicenseName = ocl.Name
              };
      if (type == 0)
      {
        if (d1.HasValue)
        {
          q = q.Where(a => a.ODate >= d1);
        }
        if (d2.HasValue)
        {
          q = q.Where(a => a.ODate <= d2);
        }
      }
      else if (type == 1)
      {
        if (d1.HasValue)
        {
          q = q.Where(a => a.TDate >= d1);
        }
        if (d2.HasValue)
        {
          q = q.Where(a => a.TDate <= d2);
        }
      }
      else if (type == 2)
      {
        if (d1.HasValue)
        {
          q = q.Where(a => a.ADate >= d1);
        }
        if (d2.HasValue)
        {
          q = q.Where(a => a.ADate <= d2);
        }
      }
      if (sd == true)
        q = q.Where(a => a.ADate == null);
      if (sort != null) q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);

      var ql = db.taLibs.Where(p => p.LConcept == 458622 && p.LParent == 458622).Select(p => new { p.LName, p.LName1 });

      return q.ToList().Select(i => new
      {
        i.Id,
        i.ADate,
        i.ANum,
        i.Directed,
        i.EmailTo,
        i.EmailCc,
        i.InDateTime,
        i.ODate,
        i.RegNum,
        i.TDate,
        i.Theme,
        i.TypeID,
        i.TypeName,
        i.FileNameI,
        i.FileNameO,
        i.LotusLinkI,
        i.UserName,
        i.Comment,
        i.Resolution,
        i.DocDate,
        i.DocNum,
        i.IsAcquaintance,
        i.IsComplaint,
        i.LicenseID,
        i.LicenseName,
        EmailToName = string.Join(", ", ql.Where(f => i.EmailTo.IndexOf(f.LName1) > -1).OrderBy(f => f.LName).Select(f => f.LName.Trim()).ToArray()),
        EmailCcName = string.Join(", ", ql.Where(f => i.EmailCc.IndexOf(f.LName1) > -1).OrderBy(f => f.LName).Select(f => f.LName.Trim()).ToArray())
      });
    }

    public IEnumerable<dynamic> addRegDoc(List<tRegDoc> data, string UserName)
    {
      db.tRegDocs.InsertAllOnSubmit(data);
      data[0].InDateTime = DateTime.Now;
      data[0].UserName = UserName;
      db.SubmitChanges();

      var q = from i in db.tRegDocs.Where(p => data.Select(n => n.Id).Contains(p.Id))
              join oc in db.tObjClassifiers on i.TypeID equals oc.ObjClassifierID into oc_
              from oc in oc_.DefaultIfEmpty()
              join ocl in db.tObjClassifiers on i.LicenseID equals ocl.ObjClassifierID into ocl_
              from ocl in ocl_.DefaultIfEmpty()
              select new
              {
                i.Id,
                i.ADate,
                i.ANum,
                i.Directed,
                i.EmailTo,
                i.EmailCc,
                i.InDateTime,
                i.ODate,
                i.RegNum,
                i.TDate,
                i.Theme,
                i.TypeID,
                TypeName = oc.Name,
                i.FileNameI,
                i.FileNameO,
                i.LotusLinkI,
                i.UserName,
                i.Comment,
                i.Resolution,
                i.DocDate,
                i.DocNum,
                i.IsAcquaintance,
                i.IsComplaint,
                i.LicenseID,
                LicenseName = ocl.Name
              };
      var ql = db.taLibs.Where(p => p.LConcept == 458622 && p.LParent == 458622).Select(p => new { p.LName, p.LName1 });

      return q.ToList().Select(i => new
      {
        i.Id,
        i.ADate,
        i.ANum,
        i.Directed,
        i.EmailTo,
        i.EmailCc,
        i.InDateTime,
        i.ODate,
        i.RegNum,
        i.TDate,
        i.Theme,
        i.TypeID,
        i.TypeName,
        i.FileNameI,
        i.FileNameO,
        i.LotusLinkI,
        i.UserName,
        i.Comment,
        i.Resolution,
        i.DocDate,
        i.DocNum,
        i.IsAcquaintance,
        i.IsComplaint,
        i.LicenseID,
        i.LicenseName,

        EmailToName = string.Join(", ", ql.Where(f => i.EmailTo.IndexOf(f.LName1) > -1).OrderBy(f => f.LName).Select(f => f.LName.Trim()).ToArray()),
        EmailCcName = string.Join(", ", ql.Where(f => i.EmailCc.IndexOf(f.LName1) > -1).OrderBy(f => f.LName).Select(f => f.LName.Trim()).ToArray())
      });
    }

    public IEnumerable<dynamic> updRegDoc(List<tRegDoc> data, string UserName)
    {
      foreach (var e in data.Where(p => p.Id > 0))
      {
        var q1 = db.tRegDocs.Where(p => p.Id == e.Id).First();
        if (q1 != null)
        {
          AutoMapper.Mapper.Map<tRegDoc, tRegDoc>(e, q1);
          q1.InDateTime = DateTime.Now;
          q1.UserName = UserName;
        }
      }
      db.SubmitChanges();

      var q = from i in db.tRegDocs.Where(p => data.Select(n => n.Id).Contains(p.Id))
              join oc in db.tObjClassifiers on i.TypeID equals oc.ObjClassifierID into oc_
              from oc in oc_.DefaultIfEmpty()
              join ocl in db.tObjClassifiers on i.LicenseID equals ocl.ObjClassifierID into ocl_
              from ocl in ocl_.DefaultIfEmpty()
              select new
              {
                i.Id,
                i.ADate,
                i.ANum,
                i.Directed,
                i.EmailTo,
                i.EmailCc,
                i.InDateTime,
                i.ODate,
                i.RegNum,
                i.TDate,
                i.Theme,
                i.TypeID,
                TypeName = oc.Name,
                i.FileNameI,
                i.FileNameO,
                i.LotusLinkI,
                i.UserName,
                i.Comment,
                i.Resolution,
                i.DocDate,
                i.DocNum,
                i.IsAcquaintance,
                i.IsComplaint,
                i.LicenseID,
                LicenseName = ocl.Name
              };

      var ql = db.taLibs.Where(p => p.LConcept == 458622 && p.LParent == 458622).Select(p => new { p.LName, p.LName1 });

      return q.ToList().Select(i => new
      {
        i.Id,
        i.ADate,
        i.ANum,
        i.Directed,
        i.EmailTo,
        i.EmailCc,
        i.InDateTime,
        i.ODate,
        i.RegNum,
        i.TDate,
        i.Theme,
        i.TypeID,
        i.TypeName,
        i.FileNameI,
        i.FileNameO,
        i.LotusLinkI,
        i.UserName,
        i.Comment,
        i.Resolution,
        i.DocDate,
        i.DocNum,
        i.IsAcquaintance,
        i.IsComplaint,
        i.LicenseID,
        i.LicenseName,

        EmailToName = string.Join(", ", ql.Where(f => i.EmailTo.IndexOf(f.LName1) > -1).OrderBy(f => f.LName).Select(f => f.LName.Trim()).ToArray()),
        EmailCcName = string.Join(", ", ql.Where(f => i.EmailCc.IndexOf(f.LName1) > -1).OrderBy(f => f.LName).Select(f => f.LName.Trim()).ToArray())
      });
    }

    public bool delRegDoc(List<tRegDoc> data)
    {
      try
      {
        IEnumerable<tRegDoc> e = db.tRegDocs.Where(p => data.Select(n => n.Id).Contains(p.Id));
        db.tRegDocs.DeleteAllOnSubmit(e);
        db.SubmitChanges();
        return true;
      }
      catch (Exception /*ex*/)
      {
        return false;
      }
    }

    public bool regdocCourriel(int? id, string host)
    {
      var q = (from i in
                 ((from i in db.tRegDocs.Where(p => (id == null && p.ADate == null && p.IsAcquaintance == false) || p.Id == id)
                   join oc in db.tObjClassifiers on i.TypeID equals oc.ObjClassifierID into oc_
                   from oc in oc_.DefaultIfEmpty()
                   select new
                   {
                     i.Id,
                     i.ADate,
                     i.ANum,
                     i.Directed,
                     i.EmailTo,
                     EmailToName = string.Join(", ", db.taLibs.Where(p => p.LConcept == 458622 && p.LParent == 458622 && i.EmailTo.IndexOf(p.LName1) > -1).OrderBy(p => p.LName).Select(o => o.LName.Trim()).ToArray()),
                     i.EmailCc,
                     EmailCcName = string.Join(", ", db.taLibs.Where(p => p.LConcept == 458622 && p.LParent == 458622 && i.EmailCc.IndexOf(p.LName1) > -1).OrderBy(p => p.LName).Select(o => o.LName.Trim()).ToArray()),
                     i.InDateTime,
                     i.ODate,
                     i.RegNum,
                     i.TDate,
                     i.Theme,
                     i.TypeID,
                     i.FileNameI,
                     i.LotusLinkI,
                     TypeName = oc.Name,
                     i.UserName,
                     i.Comment,
                     i.Resolution,
                     i.DocDate,
                     i.DocNum,
                     i.IsAcquaintance,
                     i.IsComplaint
                   }).ToList())
               from e1 in ((i.EmailTo + "," + i.EmailCc + ",LikholetovaES@am-uralsib.ru,IvlievaTA@am-uralsib.ru").Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).AsQueryable()).Distinct()
               orderby i.TDate
               select new
               {
                 i.Id,
                 i.ADate,
                 i.ANum,
                 i.Directed,
                 EmailTo = e1,
                 i.EmailToName,
                 i.EmailCcName,
                 i.InDateTime,
                 i.ODate,
                 i.RegNum,
                 i.TDate,
                 i.Theme,
                 i.FileNameI,
                 i.LotusLinkI,
                 i.TypeID,
                 i.TypeName,
                 i.UserName,
                 i.Comment,
                 i.Resolution,
                 i.DocDate,
                 i.DocNum,
                 i.IsAcquaintance,
                 i.IsComplaint
               }).GroupBy(l => new { l.EmailTo });
      if (q != null)
      {
        SmtpClient sc = new SmtpClient("m.am-uralsib.ru");
        sc.UseDefaultCredentials = true;
        foreach (var i in q)
        {
          MailMessage message = new MailMessage();
          message.From = new MailAddress("MiddleOffice <assets_msg@am-uralsib.ru>");
          if (host.Contains("localhost") || host.Contains("10.158.32.10") || i.Key == null)
          {
            message.To.Add("GrishinAV@am-uralsib.ru");
          }
          else
          {
            message.To.Add(i.Key.EmailTo);
          }
          var template = new MO.Areas.Code.Views.RegDoc.regdocCourriel { q = i, host = host, email = i.Key.EmailTo };
          message.Body = template.TransformText();
          message.IsBodyHtml = true;
          message.Priority = MailPriority.High;
          message.Headers.Add("Importance", "High");
          message.IsBodyHtml = true;
          message.Subject = "Поручения к исполнению";
          sc.Send(message);
        }
        return true;
      }
      return false;
    }

    public tRegDoc getRegDoc(int? Id)
    {
      var q = (
        from i in db.tRegDocs
        where i.Id == Id
        select i
      ).FirstOrDefault();
      return q;
    }

    public string getNextRegNum1()
    {
      var rg = new Regex("(.*[^\\d])(\\d+)");
      var q = (from tr in db.tRegDocs
               group tr by 1 into g
               select new
               {
                 RegNum = g.Max(t => t.RegNum)
               }).FirstOrDefault();
      var rn = string.Format("УК{0}-001", DateTime.Today.Year % 100);
      if (q != null)
      {
        var m = rg.Match(q.RegNum);
        if (m.Success)
          rn = m.Groups[1].Value + (int.Parse(m.Groups[2].Value) + 1001).ToString().Right(3);
      }
      return rn;
    }

    public IEnumerable<dynamic> getEMailList(string sort, string dir)
    {
      var q = from l in db.taLibs.Where(p => p.LConcept == 458622 && p.LParent == 458622)
              select new { id = l.LID, name = l.LName, email = l.LName1 };

      if (sort != null) q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
      return q;
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

    //public string getNextRegNum()
    //{
    //  var rg = new Regex("(.*[^\\d])(\\d+)");
    //  var q = (from tr in db.tRegInDocs
    //           group tr by 1 into g
    //           select new
    //           {
    //             RegNum = g.Max(t => t.RegNum)
    //           }).FirstOrDefault();
    //  var rn = string.Format("УК{0}-001", DateTime.Today.Year % 100);
    //  if (q != null)
    //  {
    //    var m = rg.Match(q.RegNum);
    //    if (m.Success)
    //      rn = m.Groups[1].Value + (int.Parse(m.Groups[2].Value) + 1001).ToString().Right(3);
    //  }
    //  return rn;
    //}

    public IEnumerable<dynamic> getOutgoingList(string sort, string dir, DateTime? d1, DateTime? d2)
    {
      var q = from i in db.tOutgoings
              select new
              {
                i.Id,
                i.ODate,
                i.FileName,
                i.Number,
                i.Recipient,
                i.Sender,
                i.InDateTime,
                i.Signer,
                i.Theme,
                i.UserName
              };
      if (d1.HasValue)
        q = q.Where(a => a.ODate >= d1);
      if (d2.HasValue)
        q = q.Where(a => a.ODate <= d2);
      if (sort != null) q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
      return q;
    }

    public IEnumerable<dynamic> addOutgoing(List<tOutgoing> data, string UserName)
    {
      db.tOutgoings.InsertAllOnSubmit(data);
      data[0].InDateTime = DateTime.Now;
      data[0].UserName = UserName;
      db.SubmitChanges();

      return from i in db.tOutgoings.Where(p => data.Select(n => n.Id).Contains(p.Id))
             select new
             {
               i.Id,
               i.ODate,
               i.FileName,
               i.Number,
               i.Recipient,
               i.Sender,
               i.InDateTime,
               i.Signer,
               i.Theme,
               i.UserName
             };
    }

    public IEnumerable<dynamic> updOutgoing(List<tOutgoing> data, string UserName)
    {
      foreach (var e in data.Where(p => p.Id > 0))
      {
        var q1 = db.tOutgoings.Where(p => p.Id == e.Id).First();
        if (q1 != null)
        {
          AutoMapper.Mapper.Map<tOutgoing, tOutgoing>(e, q1);
          q1.InDateTime = DateTime.Now;
          q1.UserName = UserName;
        }
      }
      db.SubmitChanges();
      return from i in db.tOutgoings.Where(p => data.Select(n => n.Id).Contains(p.Id))
             select new
             {
               i.Id,
               i.ODate,
               i.FileName,
               i.Number,
               i.Recipient,
               i.Sender,
               i.InDateTime,
               i.Signer,
               i.Theme,
               i.UserName
             };
    }

    public bool delOutgoing(List<tOutgoing> data)
    {
      try
      {
        IEnumerable<tOutgoing> e = db.tOutgoings.Where(p => data.Select(n => n.Id).Contains(p.Id));
        db.tOutgoings.DeleteAllOnSubmit(e);
        db.SubmitChanges();
        return true;
      }
      catch (Exception /*ex*/)
      {
        return false;
      }
    }

    public string getNextOutNum()
    {
      var rg = new Regex("(\\d+)");
      var q = db.tOutgoings.Max(p => p.Number);
      //(from tr in db.tOutgoings
      //         group tr by 1 into g
      //         select new
      //         {
      //           Number = g.Max(t => t.Number)
      //         }).FirstOrDefault();
      var rn = "1-УК";
      if (q != null)
      {
        var m = rg.Match(q);
        if (m.Success)
          rn = (int.Parse(m.Groups[1].Value) + 1).ToString();
      }
      return rn;
    }
  }
}