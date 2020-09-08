using MO.Models;
using MvcContrib.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MO.Areas.Code.Models
{
  public interface IFinRepository
  {
    IEnumerable<ObjClsNode> GetObjClsNode(int ParentID);
    IEnumerable<dynamic> GetPlatList();
    IEnumerable<dynamic> ChargesList(DateTime? d1, DateTime? d2, int? DateType, int? FinInstID, int? pfp, bool? np, string sort, string dir);
    IEnumerable<dynamic> ChargesDet(DateTime? d1, DateTime? d2, int? DateType, int? FinInstID, int? pfpID, int? itemID, string sort, string dir);
    IEnumerable<dynamic> GetObjClsByParentID(int ParentID);
    dynamic GetContragent(int Id);
    IQueryable<dynamic> GetContragents(string q, int limit);
    dynamic AddContragent(string name);

    IEnumerable<dynamic> ChargesCreate(List<tCharges2> data);
    IEnumerable<dynamic> ChargesUpdate(List<tCharges2> data);
    bool ChargesDel(List<tCharges2> data);
    IEnumerable<ChargesExport> ChargesExport(DateTime? d1, DateTime? d2, int? DateType, int? FinInstID);
    bool ImpCharges(ChargesExport ce);
    bool ImpChargesBCS(ChargesExport ce);

    IEnumerable<dynamic> ChargesBCSList(DateTime? d1, DateTime? d2, int? DateType, bool? np, string sort, string dir);
  }

  public class FinRepository : IFinRepository
  {
    private MiddleOfficeDataContext db = new MiddleOfficeDataContext() { CommandTimeout = 600 };

    public IEnumerable<ObjClsNode> GetObjClsNode(int ParentID)
    {
      return from oc in db.tObjClassifiers
             where oc.ParentID == ParentID
             orderby oc.ObjClassifierID
             select new ObjClsNode
             {
               id = oc.ObjClassifierID,
               text = oc.Name,
               hasChildren = (from oc1 in db.tObjClassifiers
                              where oc1.ParentID == oc.ObjClassifierID
                              select 1).Count() > 0
             };
    }

    public IEnumerable<dynamic> GetPlatList()
    {
      return from o in db.tObjClsRelations
             join f in db.tFinancialInstitutions on o.ObjectID equals f.FinancialInstitutionID
             where o.ObjClassifierID == 1003
             select new
             {
               id = f.FinancialInstitutionID,
               text = o.Comment ?? f.NameBrief
             };
    }

    public IEnumerable<dynamic> ChargesList(DateTime? d1, DateTime? d2, int? DateType, int? FinInstID, int? pfp, bool? np, string sort, string dir)
    {
      var q1 = db.tCharges2s as IQueryable<tCharges2>;
      if (DateType == 0)
      {
        if (d1.HasValue)
        {
          q1 = q1.Where(a => a.DatePay >= d1);
        }
        if (d2.HasValue)
        {
          q1 = q1.Where(a => a.DatePay <= d2);
        }
      }
      else if (DateType == 1)
      {
        if (d1.HasValue)
        {
          q1 = q1.Where(a => a.DateReg >= d1);
        }
        if (d2.HasValue)
        {
          q1 = q1.Where(a => a.DateReg <= d2);
        }
      }

      var q = from o in db.tObjClassifiers.Where(p => p.ParentID == 1019)
              join oc in db.tObjClassifiers on o.ObjClassifierID equals oc.ParentID
              join oc2 in db.tObjClassifiers on oc.ObjClassifierID equals oc2.ParentID
              from ocp in db.tObjClassifiers.Where(p => p.ObjClassifierID == pfp || (pfp == null && p.ParentID == 1054))
              from c in
                (from od in q1.Where(p => p.ItemID == oc2.ObjClassifierID && p.PfpID == ocp.ObjClassifierID && (!FinInstID.HasValue || p.FinInstID == FinInstID))
                 from rc in db.tObjClsRelations.Where(r1 => r1.ObjClassifierID == 60230 && r1.ObjType == 1104993180 && r1.ObjectID == od.FundID && r1.OnDate <= od.DatePay).OrderByDescending(r1 => r1.OnDate).Take(1).DefaultIfEmpty()
                 group new { od.QtyP, rc.Value } by 1 into g
                 select new
                 {
                   QtyP = g.Sum(p => p.QtyP * (decimal?)p.Value)
                 }).DefaultIfEmpty()
              select new
              {
                ItemID = oc2.ObjClassifierID,
                Item1 = o.Name,
                Item2 = oc.Name,
                Item3 = oc.NameBrief,
                Item4 = oc2.Name,
                Item5 = oc2.NameBrief,
                PfpID = ocp.ObjClassifierID,
                Pfp = ocp.Name,
                c.QtyP
              };
      if (np == true)
        q = q.Where(p => p.QtyP.HasValue);
      if (string.IsNullOrEmpty(sort))
        q = q.OrderBy(p => p.Item1).ThenBy(p => p.Item3).ThenBy(p => p.Item5);
      else
        q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
      return q;
    }

    public IEnumerable<dynamic> ChargesDet(DateTime? d1, DateTime? d2, int? DateType, int? FinInstID, int? pfpID, int? itemID, string sort, string dir)
    {
      var q1 = db.tCharges2s as IQueryable<tCharges2>;
      if (DateType == 0)
      {
        if (d1.HasValue)
        {
          q1 = q1.Where(a => a.DatePay >= d1);
        }
        if (d2.HasValue)
        {
          q1 = q1.Where(a => a.DatePay <= d2);
        }
      }
      else if (DateType == 1)
      {
        if (d1.HasValue)
        {
          q1 = q1.Where(a => a.DateReg >= d1);
        }
        if (d2.HasValue)
        {
          q1 = q1.Where(a => a.DateReg <= d2);
        }
      }
      if (pfpID.HasValue)
        q1 = q1.Where(a => a.PfpID == pfpID);
      if (FinInstID.HasValue)
        q1 = q1.Where(a => a.FinInstID == FinInstID);

      var q = from c in q1
              where c.ItemID == itemID
              join f in db.tFinancialInstitutions on c.FinInstID equals f.FinancialInstitutionID into f_
              from f in f_.DefaultIfEmpty()
              join oc3 in db.tObjClsRelations.Where(p => p.ObjClassifierID == 1003 && p.ObjType == 741604640) on c.FinInstID equals oc3.ObjectID into oc3_
              from oc3 in oc3_.DefaultIfEmpty()
              join ocp in db.tObjClassifiers on c.PfpID equals ocp.ObjClassifierID into ocp_
              from ocp in ocp_.DefaultIfEmpty()
              join oc in db.tObjClassifiers on c.ItemID equals oc.ObjClassifierID
              join oc2 in db.tObjClassifiers on c.TypeID equals oc2.ObjClassifierID into oc2_
              from oc2 in oc2_.DefaultIfEmpty()
              join co in db.tContragents on c.ReceiverID equals co.Id into co_
              from co in co_.DefaultIfEmpty()
              join oc4 in db.tObjClassifiers on c.PeriodicityID equals oc4.ObjClassifierID into oc4_
              from oc4 in oc4_.DefaultIfEmpty()
              join oc8 in db.tObjClassifiers on c.TRID equals oc8.ObjClassifierID into oc8_
              from oc8 in oc8_.DefaultIfEmpty()
              join s in db.tSecurities on c.FundID equals s.SecurityID into s_
              from s in s_.DefaultIfEmpty()
              select new
              {
                id = c.ID,
                c.DateReg,
                c.DatePay,
                c.FinInstID,
                FinInst = ((oc3.Comment ?? f.NameBrief) ?? "").TrimEnd(),
                c.ReceiverID,
                Receiver = co.Name,
                ItemID = c.ItemID,
                Item = oc.NameBrief,
                PfpID = c.PfpID,
                Pfp = ocp.Name,
                c.TypeID,
                TypeName = oc2.Name,
                c.DateRegEnd,
                c.QtyP,
                c.Comment,
                c.FileName,
                c.PeriodicityID,
                PeriodicityName = oc4.Name,
                c.TRID,
                TRName = oc8.Name,
                c.FundID,
                Fund = (s.NameBrief ?? "").TrimEnd()
              };

      return q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
    }

    public IEnumerable<dynamic> GetObjClsByParentID(int ParentID)
    {
      return from o in db.tObjClassifiers
             where o.ParentID == ParentID
             select new
             {
               id = o.ObjClassifierID,
               text = o.Name,
               o.NameBrief,
               o.Comment
             };
    }

    public IEnumerable<dynamic> ChargesCreate(List<tCharges2> data)
    {
      foreach (tCharges2 o in data.Where(p => p.ID == 0))
      {
        o.InDateTime = DateTime.Now;
        db.tCharges2s.InsertOnSubmit(o);
      }
      db.SubmitChanges();
      var q = from c in db.tCharges2s
              where c.ID == data[0].ID
              join f in db.tFinancialInstitutions on c.FinInstID equals f.FinancialInstitutionID into f_
              from f in f_.DefaultIfEmpty()
              join oc3 in db.tObjClsRelations.Where(p => p.ObjClassifierID == 1003 && p.ObjType == 741604640) on c.FinInstID equals oc3.ObjectID into oc3_
              from oc3 in oc3_.DefaultIfEmpty()
              join ocp in db.tObjClassifiers on c.PfpID equals ocp.ObjClassifierID into ocp_
              from ocp in ocp_.DefaultIfEmpty()
              join oc in db.tObjClassifiers on c.ItemID equals oc.ObjClassifierID
              join oc2 in db.tObjClassifiers on c.TypeID equals oc2.ObjClassifierID into oc2_
              from oc2 in oc2_.DefaultIfEmpty()
              join co in db.tContragents on c.ReceiverID equals co.Id into co_
              from co in co_.DefaultIfEmpty()
              join oc4 in db.tObjClassifiers on c.PeriodicityID equals oc4.ObjClassifierID into oc4_
              from oc4 in oc4_.DefaultIfEmpty()
              join oc8 in db.tObjClassifiers on c.TRID equals oc8.ObjClassifierID into oc8_
              from oc8 in oc8_.DefaultIfEmpty()
              join s in db.tSecurities on c.FundID equals s.SecurityID into s_
              from s in s_.DefaultIfEmpty()
              select new
              {
                id = c.ID,
                c.DateReg,
                c.DatePay,
                c.FinInstID,
                FinInst = ((oc3.Comment ?? f.NameBrief) ?? "").TrimEnd(),
                c.ReceiverID,
                Receiver = co.Name,
                ItemID = c.ItemID,
                Item = oc.NameBrief,
                PfpID = c.PfpID,
                Pfp = ocp.Name,
                c.TypeID,
                TypeName = oc2.Name,
                c.DateRegEnd,
                c.QtyP,
                c.Comment,
                c.FileName,
                c.PeriodicityID,
                PeriodicityName = oc4.Name,
                c.TRID,
                TRName = oc8.Name,
                c.FundID,
                Fund = (s.NameBrief ?? "").TrimEnd()
              };

      return q;
    }

    public IEnumerable<dynamic> ChargesUpdate(List<tCharges2> data)
    {
      var q1 = db.tCharges2s.Where(o => o.ID == data[0].ID).First();

      q1.ItemID = data[0].ItemID;
      q1.PfpID = data[0].PfpID;
      q1.QtyP = data[0].QtyP;
      q1.DateReg = data[0].DateReg;
      q1.DatePay = data[0].DatePay;
      q1.FinInstID = data[0].FinInstID;
      q1.ReceiverID = data[0].ReceiverID;
      q1.Comment = data[0].Comment;
      q1.TypeID = data[0].TypeID;
      q1.DateRegEnd = data[0].DateRegEnd;
      q1.FileName = data[0].FileName;
      q1.PeriodicityID = data[0].PeriodicityID;
      q1.TRID = data[0].TRID;
      q1.FundID = data[0].FundID;
      q1.InDateTime = DateTime.Now;
      db.SubmitChanges();
      var q = from c in db.tCharges2s
              where c.ID == data[0].ID
              join f in db.tFinancialInstitutions on c.FinInstID equals f.FinancialInstitutionID into f_
              from f in f_.DefaultIfEmpty()
              join oc3 in db.tObjClsRelations.Where(p => p.ObjClassifierID == 1003 && p.ObjType == 741604640) on c.FinInstID equals oc3.ObjectID into oc3_
              from oc3 in oc3_.DefaultIfEmpty()
              join ocp in db.tObjClassifiers on c.PfpID equals ocp.ObjClassifierID into ocp_
              from ocp in ocp_.DefaultIfEmpty()
              join oc in db.tObjClassifiers on c.ItemID equals oc.ObjClassifierID
              join oc2 in db.tObjClassifiers on c.TypeID equals oc2.ObjClassifierID into oc2_
              from oc2 in oc2_.DefaultIfEmpty()
              join co in db.tContragents on c.ReceiverID equals co.Id into co_
              from co in co_.DefaultIfEmpty()
              join oc4 in db.tObjClassifiers on c.PeriodicityID equals oc4.ObjClassifierID into oc4_
              from oc4 in oc4_.DefaultIfEmpty()
              join oc8 in db.tObjClassifiers on c.TRID equals oc8.ObjClassifierID into oc8_
              from oc8 in oc8_.DefaultIfEmpty()
              join s in db.tSecurities on c.FundID equals s.SecurityID into s_
              from s in s_.DefaultIfEmpty()
              select new
              {
                id = c.ID,
                c.DateReg,
                c.DatePay,
                c.FinInstID,
                FinInst = ((oc3.Comment ?? f.NameBrief) ?? "").TrimEnd(),
                c.ReceiverID,
                Receiver = co.Name,
                ItemID = c.ItemID,
                Item = oc.NameBrief,
                PfpID = c.PfpID,
                Pfp = ocp.Name,
                c.TypeID,
                TypeName = oc2.Name,
                c.DateRegEnd,
                c.QtyP,
                c.Comment,
                c.FileName,
                c.PeriodicityID,
                PeriodicityName = oc4.Name,
                c.TRID,
                TRName = oc8.Name,
                c.FundID,
                Fund = (s.NameBrief ?? "").TrimEnd()
              };
      return q;
    }

    public bool ChargesDel(List<tCharges2> data)
    {
      try
      {
        var q = db.tCharges2s.Where(o => o.ID == data[0].ID);
        db.tCharges2s.DeleteAllOnSubmit(q);
        db.SubmitChanges();
        return true;
      }
      catch
      {
        return false;
      }
    }

    public dynamic GetContragent(int Id)
    {
      var q = (from f in db.tContragents
               where f.Id == Id
               select new
               {
                 id = f.Id,
                 brief = f.Brief,
                 name = f.Name
               })
             .FirstOrDefault();
      return q;
    }

    public IQueryable<dynamic> GetContragents(string q, int limit)
    {
      return (from c in db.tContragents.Where(p => p.IsArchive == false && (p.Brief.Contains(q) || p.Name.Contains(q) || p.INN.StartsWith(q)))
              orderby c.Name
              select new
              {
                id = c.Id,
                name = c.Name,
                brief = c.Brief
              }).Take(limit);
    }

    public dynamic AddContragent(string name)
    {
      var c = new tContragent { Name = name, IsArchive = false };
      db.tContragents.InsertOnSubmit(c);
      db.SubmitChanges();
      return db.tContragents.Where(p => p.Id == c.Id).Select(p => new
      {
        id = p.Id,
        name = p.Name
      }).FirstOrDefault();
    }

    public IEnumerable<ChargesExport> ChargesExport(DateTime? d1, DateTime? d2, int? DateType, int? FinInstID)
    {
      var q1 = db.tCharges2s as IQueryable<tCharges2>;
      if (DateType == 0)
      {
        if (d1.HasValue)
        {
          q1 = q1.Where(a => a.DatePay >= d1);
        }
        if (d2.HasValue)
        {
          q1 = q1.Where(a => a.DatePay <= d2);
        }
      }
      else if (DateType == 1)
      {
        if (d1.HasValue)
        {
          q1 = q1.Where(a => a.DateReg >= d1);
        }
        if (d2.HasValue)
        {
          q1 = q1.Where(a => a.DateReg <= d2);
        }
      }
      if (FinInstID.HasValue)
        q1 = q1.Where(a => a.FinInstID == FinInstID);

      var q = from c in q1
              join f in db.tFinancialInstitutions on c.FinInstID equals f.FinancialInstitutionID into f_
              from f in f_.DefaultIfEmpty()
              join oc3 in db.tObjClsRelations.Where(p => p.ObjClassifierID == 1003 && p.ObjType == 741604640) on c.FinInstID equals oc3.ObjectID into oc3_
              from oc3 in oc3_.DefaultIfEmpty()
              join ocp in db.tObjClassifiers on c.PfpID equals ocp.ObjClassifierID into ocp_
              from ocp in ocp_.DefaultIfEmpty()
              join oc in db.tObjClassifiers on c.ItemID equals oc.ObjClassifierID
              join oc2 in db.tObjClassifiers on c.TypeID equals oc2.ObjClassifierID into oc2_
              from oc2 in oc2_.DefaultIfEmpty()
              join co in db.tContragents on c.ReceiverID equals co.Id into co_
              from co in co_.DefaultIfEmpty()
              join oc4 in db.tObjClassifiers on c.PeriodicityID equals oc4.ObjClassifierID into oc4_
              from oc4 in oc4_.DefaultIfEmpty()
              join oc8 in db.tObjClassifiers on c.TRID equals oc8.ObjClassifierID into oc8_
              from oc8 in oc8_.DefaultIfEmpty()
              join s in db.tSecurities on c.FundID equals s.SecurityID into s_
              from s in s_.DefaultIfEmpty()
              orderby c.DateReg
              select new ChargesExport
              {
                id = c.ID,
                DateReg = c.DateReg,
                DatePay = c.DatePay,
                FinInst = ((oc3.Comment ?? f.NameBrief) ?? "").TrimEnd(),
                Receiver = co.Name,
                Item = oc.NameBrief,
                Pfp = ocp.Name,
                TypeName = oc2.Name,
                DateRegEnd = c.DateRegEnd,
                QtyP = c.QtyP,
                Comment = c.Comment,
                PeriodicityName = oc4.Name,
                TRName = oc8.Name,
                Fund = (s.NameBrief ?? "").TrimEnd()
              };

      return q;
    }

    public bool ImpCharges(ChargesExport ce)
    {
      var rez = false;
      var c = new tCharges2
      {
        ID = ce.id ?? 0,
        Comment = ce.Comment,
        DatePay = ce.DatePay,
        DateReg = ce.DateReg,
        DateRegEnd = ce.DateRegEnd,
        InDateTime = DateTime.Now,
        QtyP = ce.QtyP
      };
      var q = db.tFinancialInstitutions.FirstOrDefault(p => p.NameBrief == ce.FinInst);
      if (q != null)
        c.FinInstID = q.FinancialInstitutionID;
      if (!c.FinInstID.HasValue)
      {
        var ocr = db.tObjClsRelations.FirstOrDefault(p => p.ObjClassifierID == 1003 && p.ObjType == 741604640 && p.Comment == ce.FinInst);
        if (ocr != null)
          c.FinInstID = ocr.ObjectID;
      }
      var contr = db.tContragents.FirstOrDefault(p => p.Name == ce.Receiver);
      if (contr != null)
        c.ReceiverID = contr.Id;

      var item = (from c2 in db.tObjClassifiers
                  join c1 in db.tObjClassifiers on c2.ParentID equals c1.ObjClassifierID
                  join c0 in db.tObjClassifiers.Where(p => p.ParentID == 1019) on c1.ParentID equals c0.ObjClassifierID
                  where c2.NameBrief == ce.Item
                  select new { c2.ObjClassifierID }).FirstOrDefault();
      if (item != null)
        c.ItemID = item.ObjClassifierID;

      var oc = db.tObjClassifiers.FirstOrDefault(p => p.Name == ce.Pfp && p.ParentID == 1054 && p.ObjType == 0);
      if (oc != null)
        c.PfpID = oc.ObjClassifierID;
      oc = db.tObjClassifiers.FirstOrDefault(p => p.Name == ce.TypeName && p.ParentID == 15185 && p.ObjType == 0);
      if (oc != null)
        c.TypeID = oc.ObjClassifierID;
      oc = db.tObjClassifiers.FirstOrDefault(p => p.Name == ce.PeriodicityName && p.ParentID == 47185 && p.ObjType == 0);
      if (oc != null)
        c.PeriodicityID = oc.ObjClassifierID;
      oc = db.tObjClassifiers.FirstOrDefault(p => p.Name == ce.TRName && p.ParentID == 58880 && p.ObjType == 0);
      if (oc != null)
        c.TRID = oc.ObjClassifierID;
      var Fund = db.tSecurities.FirstOrDefault(p => p.NameBrief == ce.Fund && p.SecType == 4);
      if (Fund != null)
        c.FundID = Fund.SecurityID;
      if (c.DateReg.HasValue && c.DatePay.HasValue && c.FinInstID.HasValue && c.ReceiverID.HasValue && c.ItemID.HasValue && c.PfpID.HasValue)
      {
        if (c.ID > 0)
        {
          var q1 = db.tCharges2s.Where(o => o.ID == c.ID).FirstOrDefault();
          if (q1 != null)
          {
            q1.ItemID = c.ItemID;
            q1.PfpID = c.PfpID;
            q1.QtyP = c.QtyP;
            q1.DateReg = c.DateReg;
            q1.DatePay = c.DatePay;
            q1.FinInstID = c.FinInstID;
            q1.ReceiverID = c.ReceiverID;
            q1.Comment = c.Comment;
            q1.TypeID = c.TypeID;
            q1.DateRegEnd = c.DateRegEnd;
            q1.FileName = c.FileName;
            q1.PeriodicityID = c.PeriodicityID;
            q1.TRID = c.TRID;
            q1.FundID = c.FundID;
            db.SubmitChanges();
            rez = true;
          }
          else
          {
            db.tCharges2s.InsertOnSubmit(c);
            db.SubmitChanges();
            rez = true;
          }
        }
        else
        {
          db.tCharges2s.InsertOnSubmit(c);
          db.SubmitChanges();
          rez = true;
        }
      }
      else
        rez = false;
      return rez;
    }

    public IEnumerable<dynamic> ChargesBCSList(DateTime? d1, DateTime? d2, int? DateType, bool? np, string sort, string dir)
    {
      var q1 = db.tCharges2s as IQueryable<tCharges2>;
      if (DateType == 0)
      {
        if (d1.HasValue)
        {
          q1 = q1.Where(a => a.DatePay >= d1);
        }
        if (d2.HasValue)
        {
          q1 = q1.Where(a => a.DatePay <= d2);
        }
      }
      else if (DateType == 1)
      {
        if (d1.HasValue)
        {
          q1 = q1.Where(a => a.DateReg >= d1);
        }
        if (d2.HasValue)
        {
          q1 = q1.Where(a => a.DateReg <= d2);
        }
      }

      var q = from oc1 in db.tObjClassifiers.Where(p => p.ParentID == 114735)
              join oc2 in db.tObjClassifiers on oc1.ObjClassifierID equals oc2.ParentID
              join oc3 in db.tObjClassifiers on oc2.ObjClassifierID equals oc3.ParentID
              join oc4 in db.tObjClassifiers on oc3.ObjClassifierID equals oc4.ParentID
              from c in
                (from od in q1.Where(p => p.ItemID == oc4.ObjClassifierID)
                 from rc in db.tObjClsRelations.Where(r1 => r1.ObjClassifierID == 60230 && r1.ObjType == 1104993180 && r1.ObjectID == od.FundID && r1.OnDate <= od.DatePay).OrderByDescending(r1 => r1.OnDate).Take(1).DefaultIfEmpty()
                 group new { od.QtyP, rc.Value } by 1 into g
                 select new
                 {
                   QtyP = g.Sum(p => p.QtyP * (decimal?)p.Value)
                 }).DefaultIfEmpty()
              select new
              {
                ItemID = oc4.ObjClassifierID,
                Item1 = oc1.Name,
                Item2 = oc2.Name,
                Item3 = oc3.Name,
                Item4 = oc4.Name,
                Item5 = oc4.NameBrief,
                c.QtyP
              };
      if (np == true)
        q = q.Where(p => p.QtyP.HasValue);
      if (string.IsNullOrEmpty(sort))
        q = q.OrderBy(p => p.Item5);
      else
        q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
      return q;
    }

    public bool ImpChargesBCS(ChargesExport ce)
    {
      var rez = false;
      var c = new tCharges2
      {
        ID = ce.id ?? 0,
        Comment = ce.Comment,
        DatePay = ce.DatePay,
        DateReg = ce.DateReg,
        DateRegEnd = ce.DateRegEnd,
        InDateTime = DateTime.Now,
        QtyP = ce.QtyP
      };
      var contr = db.tContragents.FirstOrDefault(p => p.Name == ce.Receiver);
      if (contr != null)
        c.ReceiverID = contr.Id;

      var item = (from c3 in db.tObjClassifiers
                  join c2 in db.tObjClassifiers on c3.ParentID equals c2.ObjClassifierID
                  join c1 in db.tObjClassifiers on c2.ParentID equals c1.ObjClassifierID
                  join c0 in db.tObjClassifiers.Where(p => p.ParentID == 114735) on c1.ParentID equals c0.ObjClassifierID
                  where c3.NameBrief == ce.Item
                  select new { c2.ObjClassifierID }).FirstOrDefault();
      if (item != null)
        c.ItemID = item.ObjClassifierID;

      var oc = db.tObjClassifiers.FirstOrDefault(p => p.Name == ce.TypeName && p.ParentID == 15185 && p.ObjType == 0);
      if (oc != null)
        c.TypeID = oc.ObjClassifierID;
      oc = db.tObjClassifiers.FirstOrDefault(p => p.Name == ce.PeriodicityName && p.ParentID == 47185 && p.ObjType == 0);
      if (oc != null)
        c.PeriodicityID = oc.ObjClassifierID;
      oc = db.tObjClassifiers.FirstOrDefault(p => p.Name == ce.TRName && p.ParentID == 58880 && p.ObjType == 0);
      if (oc != null)
        c.TRID = oc.ObjClassifierID;
      var Fund = db.tSecurities.FirstOrDefault(p => p.NameBrief == ce.Fund && p.SecType == 4);
      if (Fund != null)
        c.FundID = Fund.SecurityID;
      if (c.DateReg.HasValue && c.DatePay.HasValue && c.FinInstID.HasValue && c.ReceiverID.HasValue && c.ItemID.HasValue && c.PfpID.HasValue)
      {
        if (c.ID > 0)
        {
          var q1 = db.tCharges2s.Where(o => o.ID == c.ID).FirstOrDefault();
          if (q1 != null)
          {
            q1.ItemID = c.ItemID;
            q1.QtyP = c.QtyP;
            q1.DateReg = c.DateReg;
            q1.DatePay = c.DatePay;
            q1.ReceiverID = c.ReceiverID;
            q1.Comment = c.Comment;
            q1.TypeID = c.TypeID;
            q1.DateRegEnd = c.DateRegEnd;
            q1.FileName = c.FileName;
            q1.PeriodicityID = c.PeriodicityID;
            q1.TRID = c.TRID;
            q1.FundID = c.FundID;
            db.SubmitChanges();
            rez = true;
          }
          else
          {
            db.tCharges2s.InsertOnSubmit(c);
            db.SubmitChanges();
            rez = true;
          }
        }
        else
        {
          db.tCharges2s.InsertOnSubmit(c);
          db.SubmitChanges();
          rez = true;
        }
      }
      else
        rez = false;
      return rez;
    }

  }
}
