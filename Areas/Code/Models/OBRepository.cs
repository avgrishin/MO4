using MO.Helpers;
using MO.Models;
using MvcContrib.Sorting;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace MO.Areas.Code.Models
{
  public interface IOBRepository
  {
    IEnumerable<dynamic> getPoolList(int TypeID, string sort, string dir);
    IEnumerable<dynamic> getPoolUser(int PoolID, string sort, string dir);
    IEnumerable<dynamic> getUserByGroup(int GroupID, string sort, string dir);
    Boolean addPoolUser(int PoolID, int UserID, DateTime StartDate);
    Boolean delPoolUser(int id);

    IEnumerable<dynamic> GetClientList(string filter, string UserName, bool IsAdm, string sort, string dir);
    IEnumerable<dynamic> AddClient(List<tClient> data, string UserName);
    IEnumerable<dynamic> UpdClient(List<tClient> data, string UserName, bool IsAdm);

    IEnumerable<dynamic> GetClientActList(int id, DateTime? d1, DateTime? d2, string sort, string dir);
    IEnumerable<dynamic> AddClientAct(List<tClientAct> data, string UserName);
    IEnumerable<dynamic> UpdClientAct(List<tClientAct> data, string UserName);
    Boolean DelClientAct(List<tClientAct> data);
    IEnumerable<dynamic> RepClient(DateTime? d1, DateTime? d2, string UserName, bool IsAdm);
    IEnumerable<dynamic> RepClientBalls(DateTime d, string UserName, bool IsAdm);

    IEnumerable<dynamic> GetObjClsByParentID(int ParentID);
    IEnumerable<dynamic> getDogNumber(string q);
    IEnumerable<dynamic> getOrdNumber(string q);
    IEnumerable<dynamic> GetKM(string q);
    IEnumerable<dynamic> getPPZ();
    bool AddReqChange1(tReqChange data, string UserName);
    bool AddReqChange2(tReqChange data, string UserName);
    up_avgRepCouponPaysResult[] getCouponPays(int TreatyID, DateTime d);

    IEnumerable<dynamic> GetTreatyList(string filter, int TreatyTypeID, string sort, string dir);
    IEnumerable<dynamic> GetPortfolioTreatyList(int TreatyID, int PortfolioTypeID);
    IEnumerable<dynamic> GetPortfolioList(string filter, int? TypeID, string sort, string dir);
    bool AddPortfolioTreaty(int TreatyID, int PortfolioID, DateTime DateStart);
    bool DelPortfolioTreaty(int id);
  }

  public class OBRepository : IOBRepository
  {
    private MiddleOfficeDataContext db = new MiddleOfficeDataContext { CommandTimeout = 600 };

    public IEnumerable<dynamic> getPoolList(int TypeID, string sort, string dir)
    {
      var q = from f in db.tPortfolios.Where(p => p.PortfolioTypeID == TypeID && p.isDeleted == 0 && p.DateFinish > DateTime.Today.AddMonths(-1))
              select new
              {
                id = f.PortfolioID,
                Brief = f.NameBrief,
                Name = f.Name
              };
      if (string.IsNullOrEmpty(sort))
      {
        sort = "Brief";
        dir = "ASC";
      }
      if (sort != null) q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
      return q;
    }

    public IEnumerable<dynamic> getPoolUser(int PoolID, string sort, string dir)
    {
      var q = from pu in db.tPortfolioUsers.Where(p => p.PortfolioID == PoolID)
              join u in db.tUsers on pu.UserID equals u.UserID
              select new
              {
                id = pu.PortfolioUserID,
                pu.PortfolioID,
                pu.UserID,
                pu.StartDate,
                pu.FinishDate,
                UserName = u.NameLast + " " + u.NameFirst
              };
      if (string.IsNullOrEmpty(sort))
      {
        sort = "StartDate";
        dir = "ASC";
      }
      q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
      return q;
    }

    public IEnumerable<dynamic> getUserByGroup(int GroupID, string sort, string dir)
    {
      var q = from u in db.tUsers.Where(p => p.UserGroupID == GroupID && p.Enb == 'T')
              select new
              {
                id = u.UserID,
                Name = u.NameLast + " " + u.NameFirst
              };
      if (string.IsNullOrEmpty(sort))
      {
        sort = "Name";
        dir = "ASC";
      }
      q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
      return q;
    }

    public Boolean addPoolUser(int PoolID, int UserID, DateTime StartDate)
    {
      var port = db.tPortfolios.FirstOrDefault(p => p.PortfolioID == PoolID);
      if (port == null)
        return false;
      var maxStartDate = db.tPortfolioUsers.Where(p => p.PortfolioID == PoolID).Max(p => (DateTime?)p.StartDate);
      if (maxStartDate >= StartDate)
        return false;
      if (maxStartDate.HasValue)
      {
        var pu = db.tPortfolioUsers.Where(p => p.PortfolioID == PoolID && p.StartDate == maxStartDate.Value).First();
        if (pu.UserID == UserID)
          return false;
        pu.FinishDate = StartDate.AddDays(-1);
      }
      var pun = new tPortfolioUser();
      pun.PortfolioID = PoolID;
      pun.UserID = UserID;
      pun.StartDate = StartDate;
      pun.FinishDate = new DateTime(2050, 12, 31);
      db.tPortfolioUsers.InsertOnSubmit(pun);
      db.SubmitChanges();
      return true;
    }

    public Boolean delPoolUser(int id)
    {
      var pu = db.tPortfolioUsers.Where(p => p.PortfolioUserID == id).FirstOrDefault();
      if (pu == null)
        return false;
      if (db.tPortfolioUsers.Any(p => p.PortfolioID == pu.PortfolioID && p.StartDate > pu.StartDate))
        return false;
      var maxStartDate = db.tPortfolioUsers.Where(p => p.PortfolioID == pu.PortfolioID && p.StartDate < pu.StartDate).Max(p => (DateTime?)p.StartDate);
      if (maxStartDate.HasValue)
      {
        var pup = db.tPortfolioUsers.Where(p => p.PortfolioID == pu.PortfolioID && p.StartDate == maxStartDate.Value).First();
        pup.FinishDate = new DateTime(2050, 12, 31);
      }
      db.tPortfolioUsers.DeleteOnSubmit(pu);
      db.SubmitChanges();
      return true;
    }


    public IEnumerable<dynamic> GetClientList(string filter, string UserName, bool IsAdm, string sort, string dir)
    {
      var q1 = db.tClients.Where(p => 1 == 1);
      if (!IsAdm)
        q1 = q1.Where(p => p.UserName == UserName);
      if (!string.IsNullOrEmpty(filter))
        q1 = q1.Where(p => p.FIO.Contains(filter));
      var q = from c in q1
              select c;
      q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
      return q;
    }

    public IEnumerable<dynamic> AddClient(List<tClient> data, string UserName)
    {
      foreach (var e in data)
      {
        e.InDateTime = DateTime.Now;
        e.UserName = UserName.Left(30);
      }
      db.tClients.InsertAllOnSubmit(data);
      db.SubmitChanges();
      var q = from c in db.tClients.Where(p => data.Select(n => n.ID).Contains(p.ID))
              select c;
      return q;
    }

    public IEnumerable<dynamic> UpdClient(List<tClient> data, string UserName, bool IsAdm)
    {
      foreach (var e in data.Where(p => p.ID > 0))
      {
        var q1 = db.tClients.Where(p => p.ID == e.ID).First();
        if (q1 != null)
        {
          if (IsAdm)
          {
            q1.FIO = e.FIO;
            q1.Phone1 = e.Phone1;
          }
          if (UserName == q1.UserName || IsAdm)
          {
            q1.ActiveID = e.ActiveID;
            q1.Address = e.Address;
            q1.AddressTown = e.AddressTown;
            q1.BirthDay = e.BirthDay.HasValue ? e.BirthDay.Value.Date : (DateTime?)null;
            q1.Email = e.Email;
            q1.InDateTime = DateTime.Now;
            q1.KindActivity = e.KindActivity;
            q1.KM = e.KM;
            q1.Passport = e.Passport;
            q1.Phone1 = e.Phone1;
            q1.Phone2 = e.Phone2;
            q1.Phone3 = e.Phone3;
            q1.Position = e.Position;
            q1.Potential = e.Potential;
            q1.RiskProfil = e.RiskProfil;
            //q1.UserName = UserName.Left(30);
          }
          db.SubmitChanges();
        }
      }

      var q = from c in db.tClients.Where(p => data.Select(n => n.ID).Contains(p.ID))
              select c;
      return q;
    }

    public IEnumerable<dynamic> GetClientActList(int id, DateTime? d1, DateTime? d2, string sort, string dir)
    {
      var q1 = db.tClientActs.Where(p => p.ClientID == id);
      if (d1.HasValue)
        q1 = q1.Where(p => p.DateC >= d1);
      if (d2.HasValue)
        q1 = q1.Where(p => p.DateC <= d2);
      var q = from c in q1
              join c1 in db.tObjClassifiers on c.KindID equals c1.ObjClassifierID into _c1
              from c1 in _c1.DefaultIfEmpty()
              orderby c.DateC
              select new
              {
                c.ID,
                c.DateC,
                c.DateN,
                c.IsDeal,
                c.KindID,
                KindS = c1.Name,
                c.Product,
                c.Qty,
                c.Result
              };
      q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
      return q;
    }

    public IEnumerable<dynamic> AddClientAct(List<tClientAct> data, string UserName)
    {
      foreach (var e in data)
      {
        e.InDateTime = DateTime.Now;
        e.UserName = UserName.Left(30);
      }
      db.tClientActs.InsertAllOnSubmit(data);
      db.SubmitChanges();
      var q = from c in db.tClientActs.Where(p => data.Select(n => n.ID).Contains(p.ID))
              join c1 in db.tObjClassifiers on c.KindID equals c1.ObjClassifierID into _c1
              from c1 in _c1.DefaultIfEmpty()
              select new
              {
                c.ID,
                c.DateC,
                c.DateN,
                c.IsDeal,
                c.KindID,
                KindS = c1.Name,
                c.Product,
                c.Qty,
                c.Result
              };
      return q;
    }

    public IEnumerable<dynamic> UpdClientAct(List<tClientAct> data, string UserName)
    {
      foreach (var e in data.Where(p => p.ID > 0))
      {
        var q1 = db.tClientActs.Where(p => p.ID == e.ID).First();
        if (q1 != null)
        {
          q1.DateC = e.DateC;
          q1.DateN = e.DateN;
          q1.IsDeal = e.IsDeal;
          q1.InDateTime = DateTime.Now;
          q1.KindID = e.KindID;
          q1.Product = e.Product;
          q1.Qty = e.Qty == 0 ? null : e.Qty;
          q1.Result = e.Result;
          q1.UserName = UserName.Left(30);
          db.SubmitChanges();
        }
      }
      var q = from c in db.tClientActs.Where(p => data.Select(n => n.ID).Contains(p.ID))
              join c1 in db.tObjClassifiers on c.KindID equals c1.ObjClassifierID into _c1
              from c1 in _c1.DefaultIfEmpty()
              select new
              {
                c.ID,
                c.DateC,
                c.DateN,
                c.IsDeal,
                c.KindID,
                KindS = c1.Name,
                c.Product,
                c.Qty,
                c.Result
              };
      return q;
    }

    public bool DelClientAct(List<tClientAct> data)
    {
      try
      {
        IEnumerable<tClientAct> e = db.tClientActs.Where(p => data.Select(n => n.ID).Contains(p.ID));
        db.tClientActs.DeleteAllOnSubmit(e);
        db.SubmitChanges();
        return true;
      }
      catch (Exception /*ex*/)
      {
        return false;
      }
    }


    public IEnumerable<dynamic> GetObjClsByParentID(int ParentID)
    {
      return from o in db.tObjClassifiers
             where o.ParentID == ParentID
             select new
             {
               Value = o.ObjClassifierID,
               Text = o.Name
             };
    }

    private enum KindTreaty : int
    {
      InitialCall = 63955,
      SecondCall = 63956,
      InitialMeet = 63957,
      SecondMeet = 63958
    }

    public IEnumerable<dynamic> RepClient(DateTime? d1, DateTime? d2, string UserName, bool IsAdm)
    {
      var q1 = db.tClients.Where(p => 1 == 1);
      if (!IsAdm)
        q1 = q1.Where(p => p.UserName == UserName);

      var q2 = db.tClientActs.Where(p => 1 == 1);
      if (d1.HasValue)
        q2 = q2.Where(p => p.DateC >= d1);
      if (d2.HasValue)
        q2 = q2.Where(p => p.DateC <= d2);

      var q = from cl in q1
              join cla in q2 on cl.ID equals cla.ClientID into cla_
              from cla in cla_.DefaultIfEmpty()
              join c1 in db.tObjClassifiers on cla.KindID equals c1.ObjClassifierID into _c1
              from c1 in _c1.DefaultIfEmpty()
              orderby cl.UserName, cla.DateC descending
              select new
              {
                cl.FIO,
                cl.IsUL,
                cl.BirthDay,
                cl.Email,
                cl.AddressTown,
                cl.KindActivity,
                cl.KM,
                cl.Phone1,
                cl.Position,
                cl.Potential,
                cl.RiskProfil,
                cla.DateC,
                cla.DateN,
                cla.IsDeal,
                cla.KindID,
                KindS = c1.Name,
                cla.Product,
                cla.Qty,
                cla.Result,
                Ball = (cla.KindID == (int)KindTreaty.InitialCall || cla.KindID == (int)KindTreaty.SecondCall ? 1 : cla.KindID == (int)KindTreaty.InitialMeet || cla.KindID == (int)KindTreaty.SecondMeet ? 5 : 0) + (cla.IsDeal == true ? 10 : 0),
                cl.UserName
              };
      return q;
    }

    public IEnumerable<dynamic> RepClientBalls(DateTime d, string UserName, bool IsAdm)
    {
      var d2 = d;
      var d1 = d.AddDays(-6);
      var q1 = db.tClients.Where(p => 1 == 1);
      if (!IsAdm)
        q1 = q1.Where(p => p.UserName == UserName);

      var q2 = db.tClientActs.Where(p => p.DateC >= d1 && p.DateC <= d2);

      var q = from c in
                (from cl in q1
                 join cla in q2 on cl.ID equals cla.ClientID into cla_
                 from cla in cla_.DefaultIfEmpty()
                 group new { cl.UserName, cla.KindID, cla.IsDeal } by cl.UserName into g
                 select new
                 {
                   UserName = g.Key,
                   Call = g.Sum(p => p.KindID == (int)KindTreaty.InitialCall || p.KindID == (int)KindTreaty.SecondCall ? 1 : 0),
                   Meet = g.Sum(p => p.KindID == (int)KindTreaty.InitialMeet || p.KindID == (int)KindTreaty.SecondMeet ? 1 : 0),
                   Deal = g.Sum(p => p.IsDeal == true ? 1 : 0),
                 }
              )
              join l in db.taLibs.Where(p => p.LConcept == 503782 && p.LParent == 503782) on c.UserName equals l.LName2
              join lc in db.taLibs.Where(p => p.LConcept == 503785) on l.LID equals lc.LParent

              from oc1 in db.tObjClassifiers.Where(p => p.ParentID == 1949 && p.Comment == l.LName1).DefaultIfEmpty().Take(1)

              from dl2 in
                (
                  from dl1 in
                    (from ocr in db.tObjClsRelations
                     where ocr.ObjClassifierID == oc1.ObjClassifierID
                     join ocr1 in db.tObjClsRelations.Where(p => p.ObjType == 1631275800 && new int?[] { 199, 200, 196 }.Contains(p.ObjClassifierID)) on ocr.ObjectID equals ocr1.ObjectID
                     join dl in db.tDeals.Where(p => p.DealDate >= d1 && p.DealDate <= d2 && new int[] { 312, 10, 3 }.Contains(p.DealTypeID)) on ocr.ObjectID equals dl.LeftSideID
                     from r in db.tRates.Where(p => p.SecurityID == dl.FundID && p.RawDataProviderID == 1 && p.TradeSystemID == 1 && p.ActualizationDateTime <= dl.DealDate).OrderByDescending(p => p.ActualizationDateTime).Take(1)
                     select new
                     {
                       Qty = (double?)dl.CouponQuantity * r.CourseCurrent,
                       dl.DealTypeID,
                       dl.Direction
                     }
                    )
                  group dl1 by 1 into g
                  select new
                  {
                    QtyI = g.Sum(p => (p.DealTypeID == 10 && p.Direction == 1) || (p.DealTypeID == 312 && p.Direction == 0) ? p.Qty : null),
                    QtyO = g.Sum(p => (p.DealTypeID == 3 && p.Direction == 0) || (p.DealTypeID == 312 && p.Direction == 1) ? p.Qty : null)
                  }
                ).DefaultIfEmpty()

              select new
              {
                UN = lc.LName + (lc.LName1.Length > 0 ? (" " + lc.LName1.Substring(0, 1) + "." + (lc.LName2.Length > 0 ? (" " + lc.LName2.Substring(0, 1) + ".") : "")) : ""),
                c.Call,
                c.Meet,
                c.Deal,
                Ball = c.Call + c.Meet * 5 + c.Deal * 10,
                paib = (from o in db.pOrders
                        where o.ContrDate >= d1 && o.ContrDate <= d2 && o.DealType == 1 && o.InstrumentID != 10000000140 && o.Num > 0 && o.nodeBrief != "Отказ" && o.TabNomer == l.LInt2.ToString()
                        select o.Qty).Sum(),
                dl2.QtyI,
                dl2.QtyO,
                Qty = (from ocr in db.tObjClsRelations
                       where ocr.ObjClassifierID == oc1.ObjClassifierID
                       join ocr1 in db.tObjClsRelations.Where(p => p.ObjType == 1631275800 && new int?[] { 199, 200, 196 }.Contains(p.ObjClassifierID)) on ocr.ObjectID equals ocr1.ObjectID
                       from w in db.tWorkDates.Where(p => p.WorkDate <= d2 && p.WorkDate < DateTime.Today).OrderByDescending(p => p.WorkDate).Take(1)
                       join tv in db.tTreatyValues on new { o = ocr.ObjectID, d = w.WorkDate } equals new { o = tv.TreatyID, d = tv.ValueDate }
                       select (double?)tv.ValueRUR
                    ).Sum()
              };
      return q.OrderBy(p => p.UN);
    }

    public IEnumerable<dynamic> getDogNumber(string q)
    {
      return db.tTreaties
        .Where(p => p.Number.StartsWith(q) || p.NameBrief.StartsWith(q))
        .Join(db.tTreatyTreatyTypes.Where(p => new int[] { 1, 340 }.Contains(p.TreatyTypeID)), p => p.TreatyID, p => p.TreatyID, (p, p2) => new
        {
          p.TreatyID,
          p.Number,
          p.FinancialInstitutionPortal
        })
        //        .Where(p => System.Data.Linq.SqlClient.SqlMethods.Like(p.NameBrief, "%ДУ%"))
        .ToArray()
        .Select(p => new
        {
          p.TreatyID,
          p.Number,
          Portal = p.FinancialInstitutionPortal.TrimEnd()
        });
    }
    public IEnumerable<dynamic> GetKM(string q)
    {
      return db.tObjClassifiers
             .Where(p => p.ParentID == 1949)
             .Where(p => p.Name.StartsWith(q) || p.NameBrief.StartsWith(q))
             .Select(p => new
              {
                p.Name
              })
              .Distinct()
              .Take(10);
    }
    public IEnumerable<dynamic> getPPZ()
    {
      return db.pPPZs
        .Where(p => p.DO != "")
        .OrderBy(p => p.CODE)
        .Select(p => new
        {
          p.CODE,
          p.DO,
          p.TD
        });
    }
    public bool AddReqChange1(tReqChange data, string UserName)
    {
      var val = true;
      if (string.IsNullOrEmpty(data.Number))
        val = false;
      if (string.IsNullOrEmpty(data.Name1) && string.IsNullOrEmpty(data.Name2))
        val = false;
      data.TypeID = 1;
      data.IsDone = false;
      data.InDateTime = DateTime.Now;
      data.UserName = UserName.Left(50);
      if (!val)
        return false;
      db.tReqChanges.InsertOnSubmit(data);
      db.SubmitChanges();
      return true;
    }

    public bool AddReqChange2(tReqChange data, string UserName)
    {
      var val = true;
      if (string.IsNullOrEmpty(data.Number))
        val = false;
      if (string.IsNullOrEmpty(data.Name1) && string.IsNullOrEmpty(data.Name2) && string.IsNullOrEmpty(data.Name3))
        val = false;
      if (!db.pOrders.Where(p => p.Number == data.Number).Any()) val = false;
      data.TypeID = 2;
      data.IsDone = false;
      data.InDateTime = DateTime.Now;
      data.UserName = UserName.Left(50);
      if (!val)
        return false;
      db.tReqChanges.InsertOnSubmit(data);
      db.SubmitChanges();
      return true;
    }

    public IEnumerable<dynamic> getOrdNumber(string q)
    {
      return db.pOrders.Where(p => p.Number.StartsWith(q)).Select(p => new { p.Number, p.PPZCode, p.Seller, p.TabNomer }).Distinct();
    }

    public up_avgRepCouponPaysResult[] getCouponPays(int TreatyID, DateTime d)
    {
      return db.up_avgRepCouponPays(TreatyID, d).ToArray();
      //var q = from tr in db.tTreaties
      //        where tr.TreatyID == TreatyID
      //        join ttt in db.tTreatyTreatyTypes.Where(p => p.TreatyTypeID == 1) on tr.TreatyID equals ttt.TreatyID
      //        join a in db.tAccounts on tr.TreatyID equals a.TreatyID
      //        join s in db.tSecurities.Where(p => p.SecType == 2) on a.SecurityID equals s.SecurityID
      //        join ab in db.tAccountBalances.Where(p => p.BalanceDate == d) on a.AccountID equals ab.AccountID
      //        let isin = s.Number
      //        let Brief = s.NameBrief.TrimEnd()
      //        let Rest = ab.OutcomeBalanceF
      //        from v in
      //          (
      //            from c in db.tCouponPeriods.Where(p => p.SecurityID == a.SecurityID && p.DateEnd >= d && p.CouponValue > 0)
      //            select new { isin, Brief, ym = c.DateEnd.Value.Year * 100 + c.DateEnd.Value.Month, val = c.CouponValue, t = "cu" }
      //            ).Concat(
      //            from am in db.tAmortizations.Where(w => w.SecurityID == a.SecurityID && w.AmortizationDate >= d && w.AmortizationQuantity > 0)
      //            select new { isin, Brief, ym = am.AmortizationDate.Year * 100 + am.AmortizationDate.Month, val = am.AmortizationQuantity, t = "am" }
      //            ).Concat(
      //            from c in db.tRedemptions.Where(p => p.SecurityID == a.SecurityID && p.RedemptionDate >= d && p.RedemptionPrice > 0)
      //            select new { isin, Brief, ym = c.RedemptionDate.Year * 100 + c.RedemptionDate.Month, val = (decimal)((decimal)c.RedemptionPrice / 100 * (((decimal?)db.tAmortizations.Where(p => p.SecurityID == s.SecurityID && p.AmortizationDate <= c.RedemptionDate).OrderByDescending(p => p.AmortizationDate).FirstOrDefault().Nominal) ?? s.Nominal)), t = "rd" }
      //            ).Concat(
      //            from c in db.tSecurities.Where(p => p.SecurityID == a.SecurityID && !db.tRedemptions.Where(r => r.SecurityID == a.SecurityID && r.RedemptionDate >= d && r.RedemptionPrice > 0).Any())
      //            select new { isin, Brief, ym = s.DateEnd.Value.Year * 100 + c.DateEnd.Value.Month, val = (decimal)((((decimal?)db.tAmortizations.Where(p => p.SecurityID == s.SecurityID && p.AmortizationDate <= c.DateEnd).OrderByDescending(p => p.AmortizationDate).FirstOrDefault().Nominal) ?? s.Nominal)), t = "po" }
      //            )
      //        select new { isin = s.Number, Brief = s.NameBrief.TrimEnd(), val = v.val*ab.OutcomeBalanceF, v.t };
      //return q.ToList();
    }

    public IEnumerable<dynamic> GetTreatyList(string filter, int TreatyTypeID, string sort, string dir)
    {
      var q1 = db.tTreaties.Where(p => p.IsDisabled == 0).Where(p => p.DateFinish == new DateTime(1900, 1, 1) || p.DateFinish > DateTime.Today.AddDays(-100)).Where(p => p.TreatyID != 7978)
        .Where(p => !db.tObjClsRelations.Where(o => o.ObjClassifierID == 46912).Select(o => o.ObjectID).Contains(p.ContractorID));
      if (!string.IsNullOrEmpty(filter))
        q1 = q1.Where(p => p.Name.Contains(filter) || p.tFinancialInstitution.Name.Contains(filter) || (p.tFinancialInstitution.IsJuridicalPerson == 0 && p.tFinancialInstitution.AddressActual.Contains(filter)));
      var q = from tr in q1
              join ttt in db.tTreatyTreatyTypes.Where(p => p.TreatyTypeID == TreatyTypeID) on tr.TreatyID equals ttt.TreatyID
              select new
              {
                tr.TreatyID,
                tr.Name,
                FinInstID = tr.FinancialInstitutionID,
                ClientBrief = tr.tFinancialInstitution.NameBrief,
                ClientName = tr.tFinancialInstitution.IsJuridicalPerson == 0 ? (tr.tFinancialInstitution.AddressActual ?? tr.tFinancialInstitution.Name) : tr.tFinancialInstitution.Name,
                ContrName = tr.tFinancialInstitution1.NameBrief,
                Portal = tr.FinancialInstitutionPortal.TrimEnd(),
                tr.DateStart,
                DateFinish = tr.DateFinish == new DateTime(1900, 1, 1) ? null : (DateTime?)tr.DateFinish
              };
      if (string.IsNullOrEmpty(sort))
      {
        sort = "Name";
        dir = "ASC";
      }
      q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
      return q;
    }

    public IEnumerable<dynamic> GetPortfolioTreatyList(int TreatyID, int PortfolioTypeID)
    {
      var q = from ap in db.tAccountPortfolios
              join p in db.tPortfolios.Where(t => t.PortfolioTypeID == PortfolioTypeID) on ap.PortfolioID equals p.PortfolioID
              where ap.TreatyID == TreatyID
              orderby ap.StartDate
              select new
              {
                ID = ap.AccountPortfolioID,
                ap.TreatyID,
                ap.PortfolioID,
                ap.StartDate,
                ap.FinishDate,
                p.NameBrief,
                p.Name,
                TypeName = p.tPortfolioType.Name
              };
      return q;
    }

    public IEnumerable<dynamic> GetPortfolioList(string filter, int? TypeID, string sort, string dir)
    {
      var q1 = db.tPortfolios.Where(p => p.DateFinish > DateTime.Today.AddMonths(-1));
      if (TypeID.HasValue)
        q1 = q1.Where(p => p.PortfolioTypeID == TypeID);
      if (!string.IsNullOrEmpty(filter))
        q1 = q1.Where(p => p.Name.Contains(filter));
      var q = from qp in q1
              select new
              {
                qp.PortfolioID,
                Brief = qp.NameBrief,
                qp.Name,
                qp.PortfolioTypeID,
                PortfolioType = qp.tPortfolioType.Name
              };
      if (string.IsNullOrEmpty(sort))
      {
        sort = "Brief";
        dir = "ASC";
      }
      q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
      return q;
    }

    public bool AddPortfolioTreaty(int TreatyID, int PortfolioID, DateTime DateStart)
    {
      var treaty = db.tTreaties.FirstOrDefault(p => p.TreatyID == TreatyID);
      if (treaty == null)
        return false;
      var portfolio = db.tPortfolios.FirstOrDefault(p => p.PortfolioID == PortfolioID);
      if (portfolio == null)
        return false;
      var maxDateStart = (
        from ap in db.tAccountPortfolios
        join p in db.tPortfolios.Where(t => t.PortfolioTypeID == portfolio.PortfolioTypeID) on ap.PortfolioID equals p.PortfolioID
        where ap.TreatyID == TreatyID
        select (DateTime?)ap.StartDate).Max();
      if (maxDateStart >= DateStart)
        return false;
      if (maxDateStart.HasValue)
      {
        var pu = (
          from ap in db.tAccountPortfolios
          join p in db.tPortfolios.Where(t => t.PortfolioTypeID == portfolio.PortfolioTypeID) on ap.PortfolioID equals p.PortfolioID
          where ap.TreatyID == TreatyID && ap.StartDate == maxDateStart.Value
          select ap).First();

        if (pu.PortfolioID == PortfolioID)
          return false;
        pu.FinishDate = DateStart;
      }
      var ptn = new tAccountPortfolio();
      ptn.PortfolioID = PortfolioID;
      ptn.TreatyID = TreatyID;
      ptn.StartDate = DateStart;
      ptn.FinishDate = new DateTime(2050, 12, 31);
      ptn.Create_Date = DateTime.Now;
      db.tAccountPortfolios.InsertOnSubmit(ptn);
      db.SubmitChanges();

      return true;
    }

    public bool DelPortfolioTreaty(int id)
    {
      var ap = db.tAccountPortfolios.FirstOrDefault(p => p.AccountPortfolioID == id);
      if (ap == null)
        return false;

      var pt = db.tPortfolios.First(p => p.PortfolioID == ap.PortfolioID);
      if ((from a in db.tAccountPortfolios
           join p in db.tPortfolios.Where(t => t.PortfolioTypeID == pt.PortfolioTypeID) on a.PortfolioID equals p.PortfolioID
           where a.TreatyID == ap.TreatyID && a.StartDate > ap.StartDate
           select a).Any())
        return false;
      var maxDateStart = (
        from a in db.tAccountPortfolios
        join p in db.tPortfolios.Where(t => t.PortfolioTypeID == pt.PortfolioTypeID) on ap.PortfolioID equals p.PortfolioID
        where a.TreatyID == ap.TreatyID && a.StartDate < ap.StartDate
        select (DateTime?)a.StartDate).Max();

      if (maxDateStart.HasValue)
      {
        var pup = (
          from a in db.tAccountPortfolios
          join p in db.tPortfolios.Where(t => t.PortfolioTypeID == pt.PortfolioTypeID) on a.PortfolioID equals p.PortfolioID
          where a.TreatyID == ap.TreatyID && a.StartDate == maxDateStart.Value
          select a).First();

        pup.FinishDate = new DateTime(2050, 12, 31);
      }
      db.tAccountPortfolios.DeleteOnSubmit(ap);
      db.SubmitChanges();
      return true;
    }

  }
}