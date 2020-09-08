using MO.Helpers;
using MO.Models;
using MvcContrib.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace MO.Areas.Code.Models
{
  public interface ILeaveRepository
  {
    IEnumerable<SelectListItem> GetType1();

    IEnumerable<UserViewModel> getUserList(string sort, string dir, bool? IsActive);
    IEnumerable<UserViewModel> addUser(List<UserViewModel> data);
    IEnumerable<UserViewModel> updUser(List<UserViewModel> data);
    bool delUser(List<UserViewModel> data);
    IEnumerable<dynamic> FindU(string q);
    bool CreateLeave(LeaveViewModel vm);
    LeaveViewModel GetLeave(int id);
    bool SendConfirm(int id, string host);
    int CheckConfirmLeave(int id, string UserName, string host);
    int ConfirmLeave(ConfirmViewModel data, string UserName, string host);
    IEnumerable<LeaveViewModel> GetHistory(HistoryParamVM histParam, string UserName);
    UserViewModel GetUserByUserName(string UserName);
    IEnumerable<dynamic> GetKM();
  }

  public class LeaveRepository : ILeaveRepository
  {
    private MiddleOfficeDataContext db = new MiddleOfficeDataContext() { CommandTimeout = 600 };

    public IEnumerable<SelectListItem> GetType1()
    {
      var q = from o in db.tObjClassifiers
              where o.ParentID == 31647
              select new SelectListItem { Value = o.ObjClassifierID.ToString(), Text = o.Name };
      return q;
    }

    public IEnumerable<UserViewModel> getUserList(string sort, string dir, bool? IsActive)
    {
      var q1 = db.taLibs.Where(p => p.LConcept == 503782 && p.LParent == 503782);
      if (IsActive.HasValue)
        q1 = q1.Where(p => p.LInt1 == (IsActive == true ? 1 : 0));
      var q = from l in q1
              join lc in db.taLibs.Where(p => p.LConcept == 503785) on l.LID equals lc.LParent into _lc
              from lc in _lc.DefaultIfEmpty()
              join oc in db.tObjClassifiers.Where(p => p.ParentID == 1949) on l.LID1 equals oc.ObjClassifierID into _oc
              from oc in _oc.DefaultIfEmpty()
              select new UserViewModel
              {
                ID = l.LID,
                Name = l.LName,
                Email = l.LName1,
                UserName = l.LName2,
                Name1 = lc.LName,
                Name2 = lc.LName1,
                Name3 = lc.LName2,
                IsActive = (l.LInt1 ?? 1) == 1,
                TabNomer = l.LInt2,
                ObjClsID = l.LID1,
                ObjClsS = oc.NameBrief
              };

      if (sort != null) q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
      return q;
    }

    public IEnumerable<UserViewModel> addUser(List<UserViewModel> data)
    {
      List<taLib> l = new List<taLib>();
      foreach (var e in data.Where(p => (p.ID ?? 0) == 0))
      {
        taLib lp = new taLib { LConcept = 503782, LParent = 503782, LName = e.Name, LName1 = e.Email, LName2 = e.UserName, InDateTime = DateTime.Now, LInt1 = e.IsActive ? 1 : 0, LInt2 = e.TabNomer, LID1 = e.ObjClsID };
        db.taLibs.InsertOnSubmit(lp);
        db.SubmitChanges();
        taLib lc = new taLib { LConcept = 503785, LParent = lp.LID, LName = e.Name1, LName1 = e.Name2, LName2 = e.Name3, InDateTime = DateTime.Now };
        db.taLibs.InsertOnSubmit(lc);
        db.SubmitChanges();
        l.Add(new taLib { LID = lp.LID });
      }

      var q = from lp in db.taLibs.Where(p => l.Select(n => n.LID).Contains(p.LID))
              join lc in db.taLibs.Where(p => p.LConcept == 503785) on lp.LID equals lc.LParent into _lc
              from lc in _lc.DefaultIfEmpty()
              join oc in db.tObjClassifiers.Where(p => p.ParentID == 1949) on lp.LID1 equals oc.ObjClassifierID into _oc
              from oc in _oc.DefaultIfEmpty()
              select new UserViewModel
              {
                ID = lp.LID,
                Name = lp.LName,
                Email = lp.LName1,
                UserName = lp.LName2,
                Name1 = lc.LName,
                Name2 = lc.LName1,
                Name3 = lc.LName2,
                IsActive = (lp.LInt1 ?? 1) == 1,
                TabNomer = lp.LInt2,
                ObjClsID = lp.LID1,
                ObjClsS = oc.NameBrief
              };

      return q;
    }

    public IEnumerable<UserViewModel> updUser(List<UserViewModel> data)
    {
      List<taLib> l = new List<taLib>();
      foreach (var e in data.Where(p => (p.ID ?? 0) > 0))
      {
        var q1 = db.taLibs.Where(p => p.LID == e.ID && p.LConcept == 503782 && p.LParent == 503782).FirstOrDefault();
        if (q1 != null)
        {
          q1.LName = e.Name;
          q1.LName1 = e.Email;
          q1.LName2 = e.UserName;
          q1.LInt1 = e.IsActive ? 1 : 0;
          q1.LInt2 = e.TabNomer;
          q1.LID1 = e.ObjClsID;
          q1.InDateTime = DateTime.Now;
          var q2 = db.taLibs.Where(p => p.LParent == q1.LID && p.LConcept == 503785).FirstOrDefault();
          if (q2 != null)
          {
            q2.LName = e.Name1;
            q2.LName1 = e.Name2;
            q2.LName2 = e.Name3;
          }
          else
          {
            taLib lc = new taLib { LConcept = 503785, LParent = q1.LID, LName = e.Name1, LName1 = e.Name2, LName2 = e.Name3, InDateTime = DateTime.Now };
            db.taLibs.InsertOnSubmit(lc);
          }
          db.SubmitChanges();
          l.Add(new taLib { LID = e.ID.Value });
        }
      }
      db.SubmitChanges();

      var q = from lp in db.taLibs.Where(p => l.Select(n => n.LID).Contains(p.LID))
              join lc in db.taLibs.Where(p => p.LConcept == 503785) on lp.LID equals lc.LParent into _lc
              from lc in _lc.DefaultIfEmpty()
              join oc in db.tObjClassifiers.Where(p => p.ParentID == 1949) on lp.LID1 equals oc.ObjClassifierID into _oc
              from oc in _oc.DefaultIfEmpty()
              select new UserViewModel
              {
                ID = lp.LID,
                Name = lp.LName,
                Email = lp.LName1,
                UserName = lp.LName2,
                Name1 = lc.LName,
                Name2 = lc.LName1,
                Name3 = lc.LName2,
                IsActive = (lp.LInt1 ?? 1) == 1,
                TabNomer = lp.LInt2,
                ObjClsID = lp.LID1,
                ObjClsS = oc.NameBrief
              };

      return q;
    }

    public bool delUser(List<UserViewModel> data)
    {
      try
      {
        var q = db.taLibs.Where(o => o.LParent == data[0].ID && o.LConcept == 503785).FirstOrDefault();
        if (q != null)
          db.taLibs.DeleteOnSubmit(q);
        var q1 = db.taLibs.Where(o => o.LID == data[0].ID && o.LConcept == 503782).FirstOrDefault();
        if (q1 != null)
          db.taLibs.DeleteOnSubmit(q1);
        db.SubmitChanges();
        return true;
      }
      catch
      {
        return false;
      }
    }

    public IEnumerable<dynamic> GetKM()
    {
      var q = from oc in db.tObjClassifiers
              where oc.ParentID == 1949
              select new
              {
                id = oc.ObjClassifierID,
                text = oc.NameBrief.TrimEnd()
              };
      return q;
    }

    public IEnumerable<dynamic> FindU(string id)
    {
      var q = from l in db.taLibs.Where(p => p.LConcept == 503782 && p.LParent == 503782 && p.LInt1 == 1)
              where l.LName.StartsWith(id)
              orderby l.LName
              select new
              {
                name = l.LName,
                Email = l.LName1,
                UserName = l.LName2
              };

      return q;
    }

    public UserViewModel GetUserByUserName(string UserName)
    {
      var q = db.taLibs.Where(p => p.LConcept == 503782 && p.LParent == 503782 && p.LName2 == UserName)
        .Select(p => new UserViewModel
        {
          Name = p.LName,
          Email = p.LName1,
          UserName = p.LName2
        }).FirstOrDefault();

      return q;
    }

    public bool CreateLeave(LeaveViewModel vm)
    {
      var leave = new tLeave();
      leave.DateB = vm.DateB;
      leave.DateE = vm.DateE;
      leave.Days = vm.Days;
      leave.Email1 = vm.Email1;
      leave.Email2 = vm.Email2;
      leave.Email4 = vm.Email4;
      leave.Email5 = vm.Email5;
      leave.InDateTime = DateTime.Now;
      leave.Name1 = vm.Name1;
      leave.Name2 = vm.Name2;
      leave.Name4 = vm.Name4;
      leave.Name5 = vm.Name5;
      leave.Sign1 = DateTime.Now;
      leave.TypeId = vm.TypeId;
      leave.UserName1 = vm.UserName1;
      leave.UserName4 = vm.UserName4;
      leave.UserName5 = vm.UserName5;
      leave.Comment1 = vm.Comment1;
      db.tLeaves.InsertOnSubmit(leave);
      db.SubmitChanges();
      vm.ID = leave.Id;
      return true;
    }

    public LeaveViewModel GetLeave(int id)
    {
      return (from l in db.tLeaves.Where(p => p.Id == id)
              join o in db.tObjClassifiers.Where(p => p.ParentID == 31647) on l.TypeId equals o.ObjClassifierID into _o
              from o in _o.DefaultIfEmpty()
              select new LeaveViewModel
              {
                ID = l.Id,
                UserName1 = l.UserName1,
                UserName4 = l.UserName4,
                UserName5 = l.UserName5,
                Email1 = l.Email1,
                Email2 = l.Email2,
                Email4 = l.Email4,
                Email5 = l.Email5,
                Name1 = l.Name1,
                Name2 = l.Name2,
                Name3 = l.Name3,
                Name4 = l.Name4,
                Name5 = l.Name5,
                TypeId = l.TypeId,
                TypeName = o.Name,
                DateB = l.DateB,
                DateE = l.DateE,
                Days = l.Days,
                Sign1 = l.Sign1,
                Sign4 = l.Sign4,
                Sign5 = l.Sign5,
                Comment1 = l.Comment1,
                Comment4 = l.Comment4,
                Comment5 = l.Comment5
              }).FirstOrDefault();
    }

    public bool SendConfirm(int id, string host)
    {
      var q = GetLeave(id);
      if (q != null)
      {
        var Email = "";
        if (q.Sign4 == null)
        {
          Email = q.Email4;
        }
        else if (q.Sign5 == null && !string.IsNullOrEmpty(q.UserName5))
        {
          Email = q.Email5;
        }
        if (!string.IsNullOrEmpty(Email))
        {
          SmtpClient sc = new SmtpClient("m.am-uralsib.ru");
          sc.UseDefaultCredentials = true;
          MailMessage message = new MailMessage();
          message.From = new MailAddress("MiddleOffice <assets_msg@am-uralsib.ru>");
          if (host.Contains("localhost") || host.Contains("10.153.157.66"))
          {
            message.To.Add("GrishinAV@am-uralsib.ru");
          }
          else
          {
            message.To.Add(Email);
          }
          var template = new MO.Areas.Code.Views.Leave.SendConfirm { q = q, host = host, email = Email };
          message.Body = template.TransformText();
          message.IsBodyHtml = true;
          message.Priority = MailPriority.High;
          message.Headers.Add("Importance", "High");
          message.IsBodyHtml = true;
          message.Subject = string.Format("Заявление на отпуск {0}", q.Name1);
          sc.Send(message);
        }
        else if (q.Sign4 != null && (q.Sign5 != null || q.UserName5 == null))
        {
          SmtpClient sc = new SmtpClient("m.am-uralsib.ru");
          sc.UseDefaultCredentials = true;
          MailMessage message = new MailMessage();
          message.From = new MailAddress("MiddleOffice <assets_msg@am-uralsib.ru>");
          if (host.Contains("localhost") || host.Contains("10.153.157.66"))
          {
            message.To.Add("GrishinAV@am-uralsib.ru");
          }
          else
          {
            message.To.Add("MartynenkoOA@uralsib.ru");
            //message.To.Add("RyabokonMI@uralsib.ru");
          }
          var template = new MO.Areas.Code.Views.Leave.SendFinal { q = q, host = host, email = "MartynenkoOA@uralsib.ru" };
          message.Body = template.TransformText();
          message.IsBodyHtml = true;
          message.Priority = MailPriority.High;
          message.Headers.Add("Importance", "High");
          message.IsBodyHtml = true;
          message.Subject = string.Format("Заявление на отпуск {0}", q.Name1);
          sc.Send(message);

          message = new MailMessage();
          message.From = new MailAddress("MiddleOffice <assets_msg@am-uralsib.ru>");
          if (host.Contains("localhost") || host.Contains("10.153.157.66"))
          {
            message.To.Add("GrishinAV@am-uralsib.ru");
          }
          else
          {
            message.To.Add(q.Email1);
            message.CC.Add(q.Email2);
          }
          message.Body = (new MO.Areas.Code.Views.Leave.SendConfirmed { q = q, host = host, email = string.Format("{0} {1}", q.Email1, q.Email2) }).TransformText();
          message.IsBodyHtml = true;
          message.Priority = MailPriority.High;
          message.Headers.Add("Importance", "High");
          message.IsBodyHtml = true;
          message.Subject = string.Format("Заявление на отпуск {0}", q.Name1);
          sc.Send(message);
        }
        return true;
      }
      return false;
    }

    public int CheckConfirmLeave(int id, string UserName, string host)
    {
      var q = GetLeave(id);
      if (q != null)
      {
        if (q.UserName4 == UserName && q.Sign4 == null)
        {
          return 1;
        }
        else if (q.Sign4 != null && q.UserName5 == UserName && q.Sign5 == null)
        {
          return 1;
        }
        else if (q.UserName4 != UserName && q.UserName5 != UserName)
          return 2;
        else if (q.Sign4 == null && q.UserName5 == UserName && q.Sign5 == null)
          return 3;
        else if (q.UserName4 == UserName && q.Sign4 != null || q.UserName5 == UserName && q.Sign5 != null)
          return 4;
        else
          return 5;
      }
      return 0;
    }

    public int ConfirmLeave(ConfirmViewModel data, string UserName, string host)
    {
      if (data == null)
        return 0;
      var q = GetLeave(data.ID);
      if (q != null)
      {
        if (q.UserName4 == UserName && q.Sign4 == null)
        {
          var q1 = db.tLeaves.Where(p => p.Id == data.ID).Single();
          q1.Comment4 = data.Comment.Left(255);
          q1.Sign4 = DateTime.Now;
          db.SubmitChanges();
          return 1;
        }
        else if (q.Sign4 != null && q.UserName5 == UserName && q.Sign5 == null)
        {
          var q1 = db.tLeaves.Where(p => p.Id == data.ID).Single();
          q1.Sign5 = DateTime.Now;
          q1.Comment5 = data.Comment.Left(255);
          db.SubmitChanges();
          return 1;
        }
        else if (q.UserName4 != UserName && q.UserName5 != UserName)
          return 2;
        else if (q.Sign4 == null && q.UserName5 == UserName && q.Sign5 == null)
          return 3;
        else if (q.UserName4 == UserName && q.Sign4 != null || q.UserName5 == UserName && q.Sign5 != null)
          return 4;
        else
          return 5;
      }
      return 0;
    }

    public IEnumerable<LeaveViewModel> GetHistory(HistoryParamVM histParam, string UserName)
    {
      var q = from l in db.tLeaves.Where(p => p.UserName1 == UserName)
              join o in db.tObjClassifiers.Where(p => p.ParentID == 31647) on l.TypeId equals o.ObjClassifierID into _o
              from o in _o.DefaultIfEmpty()
              select new LeaveViewModel
              {
                ID = l.Id,
                UserName1 = l.UserName1,
                UserName4 = l.UserName4,
                UserName5 = l.UserName5,
                Email1 = l.Email1,
                Email2 = l.Email2,
                Email4 = l.Email4,
                Email5 = l.Email5,
                Name1 = l.Name1,
                Name2 = l.Name2,
                Name3 = l.Name3,
                Name4 = l.Name4,
                Name5 = l.Name5,
                TypeId = l.TypeId,
                TypeName = o.Name,
                DateB = l.DateB,
                DateE = l.DateE,
                Days = l.Days,
                Sign1 = l.Sign1,
                Sign4 = l.Sign4,
                Sign5 = l.Sign5,
                Comment1 = l.Comment1,
                Comment4 = l.Comment4,
                Comment5 = l.Comment5
              };
      q = q.OrderBy(histParam.Sort + " " + histParam.Dir);
      return q;
    }

  }
}