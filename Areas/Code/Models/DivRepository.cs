using MO.Models;
using MvcContrib.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MO.Areas.Code.Models
{
  public interface IDivRepository
  {
    IEnumerable<dynamic> getDivList(string sort, string dir, DateTime? d1, DateTime? d2, string n);
    IEnumerable<dynamic> addDiv(List<tDiv> data);
    IEnumerable<dynamic> updDiv(List<tDiv> data);
    bool delDiv(List<tDiv> data);
    IEnumerable<dynamic> getDiv2List(string sort, string dir, int id);
    IEnumerable<dynamic> getDiv3List(string sort, string dir, int? id, int? TreatyID);
    IEnumerable<dynamic> getDiv4List(string sort, string dir, DateTime? d1, DateTime? d2, string n);
    IEnumerable<dynamic> getDiv5List(string sort, string dir, decimal? Id);
    IEnumerable<dynamic> getSecurity(int id);
    IEnumerable<dynamic> getSecurities(string query, int limit);
    IEnumerable<dynamic> GetFunds();
    IEnumerable<dynamic> addDivRel(List<tDivRel> data);

    IEnumerable<dynamic> getDiv7List(string sort, string dir, DateTime? d1, DateTime? d2, string n);
    IEnumerable<dynamic> getDiv8List(string sort, string dir, DateTime? d1, DateTime? d2, int? tid, string n);
  }

  public class DivRepository : IDivRepository
  {
    private MiddleOfficeDataContext db = new MiddleOfficeDataContext() { CommandTimeout = 600 };

    public IEnumerable<dynamic> getDivList(string sort, string dir, DateTime? d1, DateTime? d2, string n)
    {
      var qs = from s in db.tSecurities select s;

      if (!String.IsNullOrEmpty(n))
        qs = qs.Where(p => p.NameBrief.Contains(n) || p.Name1.Contains(n) || p.Number.StartsWith(n));

      //var qe = from e in db.tFinancialInstitutions select e;

      var q = from d in db.tDivs
              join s in qs on d.SecurityID equals s.SecurityID
              join e in db.tFinancialInstitutions on s.IssuerID equals e.FinancialInstitutionID
              join sk in db.tSecKinds on s.Category equals sk.SecKindID
              join f in db.tSecurities on d.FundID equals f.SecurityID
              join ssg in db.tSecuritySecurityGroups on d.SecurityID equals ssg.SecurityID
              join sg in db.tSecurityGroups.Where(p => p.SecurityGroupTypeID == 1) on ssg.SecurityGroupID equals sg.SecurityGroupID
              from dd in
                (
                  from t in
                    (
                      from t in
                        (
                          from a in db.tAccounts
                          join ttt1 in db.tTreatyTreatyTypes.Where(t => new int[] { 1, 2, 340 }.Contains(t.TreatyTypeID)) on a.TreatyID equals ttt1.TreatyID
                          join ab in db.tAccountBalances.Where(ab => ab.OutcomeBalanceP > 0) on new { a.AccountID, d.RDate } equals new { ab.AccountID, RDate = ab.BalanceDate }
                          where a.SecurityID == d.SecurityID
                          select new { TreatyID = a.TreatyID, ValueDiv = (double?)ab.OutcomeBalanceP * d.Value, QtyPayed = (double?)0 }
                        )
                        .Union
                        (
                          from dr in db.tDivRels
                          join dc in db.tDictionariesConnections.Where(p => p.Dictionary == -461522885) on dr.DiasoftDealID equals dc.DiasoftBOID
                          join dl in db.tDeals on dc.CompositeID equals dl.DealID
                          join ttt1 in db.tTreatyTreatyTypes.Where(t => new int[] { 1, 2, 340 }.Contains(t.TreatyTypeID)) on dl.LeftSideID equals ttt1.TreatyID
                          from abt in
                            (
                              from abt in db.tAccountBalanceTurns
                              where abt.DealID == dl.DealID && abt.AccountBalanceTurnTypeID == 7
                              group abt by 1 into g
                              select new { QtyPayed = g.Sum(p => p.TurnValue) }
                            ).DefaultIfEmpty()
                          where dr.DivId == d.Id
                          select new
                          {
                            TreatyID = dr.TreatyID.HasValue ? dr.TreatyID.Value : dl.LeftSideID, //dr.DiasoftDealID == 20002825578 ? 7011
                            ValueDiv = (double?)0,
                            QtyPayed = (double?)(dr.Qty.HasValue ? dr.Qty : abt.QtyPayed)
                          }
                        )
                      group t by t.TreatyID into g
                      select new { ValueDiv = g.Sum(p => p.ValueDiv), QtyPayed = g.Sum(p => p.QtyPayed) }
                    )
                  group t by 1 into g
                  select new { ValueDiv = g.Sum(p => p.ValueDiv), QtyPayed = g.Sum(p => p.QtyPayed), CountDiv = g.Sum(p => p.ValueDiv > 0 ? (int?)1 : null), CountPayed = g.Sum(p => p.QtyPayed > 0 ? (int?)1 : null) }
                ).DefaultIfEmpty()
              select new
              {
                id = d.Id,
                d.FundID,
                d.RDate,
                d.XDate,
                PayDate = s.Number.StartsWith("RU") ? db.tWorkDates.Where(w => w.WorkDate > d.RDate).OrderBy(w => w.WorkDate).Take(10).Max(w => w.WorkDate) : d.RDate.AddDays(30),
                d.SecurityID,
                d.Value,
                SecBrief = s.NameBrief.TrimEnd(),
                SecName = s.Name1,
                Category = sk.Brief,
                SecGroup = sg.NameBrief.TrimEnd(),
                Fund = f.NameBrief.TrimEnd(),
                ISIN = s.Number,
                e.INN,
                ValueDiv = dd.ValueDiv,
                CountDiv = dd.CountDiv,
                QtyPayed = (decimal?)dd.QtyPayed,
                CountPayed = dd.CountPayed,
                PercPayed = (dd.ValueDiv ?? 0) == 0 ? 0 : Math.Round(((double?)dd.QtyPayed / ((double?)dd.ValueDiv)) ?? 0, 2, MidpointRounding.AwayFromZero) * 100,
                d.CheckKD,
                d.CheckPayed
              };
      if (d1.HasValue)
        q = q.Where(a => a.RDate >= d1);
      if (d2.HasValue)
        q = q.Where(a => a.RDate <= d2);
      //if (!String.IsNullOrEmpty(n))
      //  q = q.Where(p => p.SecBrief.Contains(n) || p.SecName.Contains(n) || p.ISIN.StartsWith(n) || p.INN.StartsWith(n));
      if (String.IsNullOrEmpty(sort))
        q = q.OrderBy(p => p.RDate);
      else
        q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
      return q;
    }

    public IEnumerable<dynamic> addDiv(List<tDiv> data)
    {
      data[0].InDateTime = DateTime.Now;
      db.tDivs.InsertAllOnSubmit(data);
      db.SubmitChanges();

      var q = from d in db.tDivs.Where(p => data.Select(n => n.Id).Contains(p.Id))
              join s in db.tSecurities on d.SecurityID equals s.SecurityID
              join e in db.tFinancialInstitutions on s.IssuerID equals e.FinancialInstitutionID
              join sk in db.tSecKinds on s.Category equals sk.SecKindID
              join f in db.tSecurities on d.FundID equals f.SecurityID
              join ssg in db.tSecuritySecurityGroups on d.SecurityID equals ssg.SecurityID
              join sg in db.tSecurityGroups.Where(p => p.SecurityGroupTypeID == 1) on ssg.SecurityGroupID equals sg.SecurityGroupID
              from dd in
                (
                  from t in
                    (
                      from t in
                        (
                          from a in db.tAccounts
                          join ttt1 in db.tTreatyTreatyTypes.Where(t => new int[] { 1, 2, 340 }.Contains(t.TreatyTypeID)) on a.TreatyID equals ttt1.TreatyID
                          join ab in db.tAccountBalances.Where(ab => ab.OutcomeBalanceP > 0) on new { a.AccountID, d.RDate } equals new { ab.AccountID, RDate = ab.BalanceDate }
                          where a.SecurityID == d.SecurityID
                          select new { TreatyID = a.TreatyID, ValueDiv = (double?)ab.OutcomeBalanceP * d.Value, QtyPayed = (double?)0 }
                          ).Union
                          (
                          from dr in db.tDivRels
                          join dc in db.tDictionariesConnections.Where(p => p.Dictionary == -461522885) on dr.DiasoftDealID equals dc.DiasoftBOID
                          join dl in db.tDeals on dc.CompositeID equals dl.DealID
                          join ttt1 in db.tTreatyTreatyTypes.Where(t => new int[] { 1, 2, 340 }.Contains(t.TreatyTypeID)) on dl.LeftSideID equals ttt1.TreatyID
                          from abt in
                            (
                              from abt in db.tAccountBalanceTurns
                              where abt.DealID == dl.DealID && abt.AccountBalanceTurnTypeID == 7
                              group abt by 1 into g
                              select new { QtyPayed = g.Sum(p => p.TurnValue) }
                              ).DefaultIfEmpty()
                          where dr.DivId == d.Id
                          select new
                          {
                            TreatyID = dr.TreatyID.HasValue ? dr.TreatyID.Value : dl.LeftSideID,
                            ValueDiv = (double?)0,
                            QtyPayed = (double?)(dr.Qty.HasValue ? dr.Qty : abt.QtyPayed)
                          }
                          )
                      group t by t.TreatyID into g
                      select new { ValueDiv = g.Sum(p => p.ValueDiv), QtyPayed = g.Sum(p => p.QtyPayed) }
                      )
                  group t by 1 into g
                  select new { ValueDiv = g.Sum(p => p.ValueDiv), QtyPayed = g.Sum(p => p.QtyPayed), CountDiv = g.Sum(p => p.ValueDiv > 0 ? (int?)1 : null), CountPayed = g.Sum(p => p.QtyPayed > 0 ? (int?)1 : null) }
                  ).DefaultIfEmpty()
              select new
              {
                id = d.Id,
                d.FundID,
                d.RDate,
                d.XDate,
                PayDate = s.Number.StartsWith("RU") ? db.tWorkDates.Where(w => w.WorkDate > d.RDate).OrderBy(w => w.WorkDate).Take(10).Max(w => w.WorkDate) : d.RDate.AddDays(30),
                d.SecurityID,
                d.Value,
                SecBrief = s.NameBrief.TrimEnd(),
                SecName = s.Name1,
                Category = sk.Brief.TrimEnd(),
                SecGroup = sg.NameBrief.TrimEnd(),
                Fund = f.NameBrief.TrimEnd(),
                ISIN = s.Number,
                e.INN,
                ValueDiv = dd.ValueDiv,
                CountDiv = dd.CountDiv,
                QtyPayed = (decimal?)dd.QtyPayed,
                CountPayed = dd.CountPayed,
                PercPayed = (dd.ValueDiv ?? 0) == 0 ? 0 : Math.Round(((double?)dd.QtyPayed / ((double?)dd.ValueDiv)) ?? 0, 2, MidpointRounding.AwayFromZero) * 100,
                d.CheckKD,
                d.CheckPayed
              };
      return q;
    }

    public IEnumerable<dynamic> updDiv(List<tDiv> data)
    {
      foreach (var e in data.Where(p => p.Id > 0))
      {
        var q1 = db.tDivs.Where(p => p.Id == e.Id).First();
        if (q1 != null)
        {
          //AutoMapper.Mapper.Map<tDiv, tDiv>(e, q1);
          q1.RDate = e.RDate;
          q1.XDate = e.XDate;
          q1.SecurityID = e.SecurityID;
          q1.FundID = e.FundID;
          q1.Value = e.Value;
          q1.CheckKD = e.CheckKD;
          q1.CheckPayed = e.CheckPayed;
          q1.InDateTime = DateTime.Now;
        }
      }
      db.SubmitChanges();

      var q = from d in db.tDivs.Where(p => data.Select(n => n.Id).Contains(p.Id))
              join s in db.tSecurities on d.SecurityID equals s.SecurityID
              join e in db.tFinancialInstitutions on s.IssuerID equals e.FinancialInstitutionID
              join sk in db.tSecKinds on s.Category equals sk.SecKindID
              join f in db.tSecurities on d.FundID equals f.SecurityID
              join ssg in db.tSecuritySecurityGroups on d.SecurityID equals ssg.SecurityID
              join sg in db.tSecurityGroups.Where(p => p.SecurityGroupTypeID == 1) on ssg.SecurityGroupID equals sg.SecurityGroupID
              from dd in
                (
                  from t in
                    (
                      from t in
                        (
                          from a in db.tAccounts
                          join ttt1 in db.tTreatyTreatyTypes.Where(t => new int[] { 1, 2, 340 }.Contains(t.TreatyTypeID)) on a.TreatyID equals ttt1.TreatyID
                          join ab in db.tAccountBalances.Where(ab => ab.OutcomeBalanceP > 0) on new { a.AccountID, d.RDate } equals new { ab.AccountID, RDate = ab.BalanceDate }
                          where a.SecurityID == d.SecurityID
                          select new { TreatyID = a.TreatyID, ValueDiv = (double?)ab.OutcomeBalanceP * d.Value, QtyPayed = (double?)0 }
                          ).Union
                          (
                          from dr in db.tDivRels
                          join dc in db.tDictionariesConnections.Where(p => p.Dictionary == -461522885) on dr.DiasoftDealID equals dc.DiasoftBOID
                          join dl in db.tDeals on dc.CompositeID equals dl.DealID
                          join ttt1 in db.tTreatyTreatyTypes.Where(t => new int[] { 1, 2, 340 }.Contains(t.TreatyTypeID)) on dl.LeftSideID equals ttt1.TreatyID
                          from abt in
                            (
                              from abt in db.tAccountBalanceTurns
                              where abt.DealID == dl.DealID && abt.AccountBalanceTurnTypeID == 7
                              group abt by 1 into g
                              select new { QtyPayed = g.Sum(p => p.TurnValue) }
                              ).DefaultIfEmpty()
                          where dr.DivId == d.Id
                          select new
                          {
                            TreatyID = dr.TreatyID.HasValue ? dr.TreatyID.Value : dl.LeftSideID,
                            ValueDiv = (double?)0,
                            QtyPayed = (double?)(dr.Qty.HasValue ? dr.Qty : abt.QtyPayed)
                          }
                          )
                      group t by t.TreatyID into g
                      select new { ValueDiv = g.Sum(p => p.ValueDiv), QtyPayed = g.Sum(p => p.QtyPayed) }
                      )
                  group t by 1 into g
                  select new { ValueDiv = g.Sum(p => p.ValueDiv), QtyPayed = g.Sum(p => p.QtyPayed), CountDiv = g.Sum(p => p.ValueDiv > 0 ? (int?)1 : null), CountPayed = g.Sum(p => p.QtyPayed > 0 ? (int?)1 : null) }
                  ).DefaultIfEmpty()
              select new
              {
                id = d.Id,
                d.FundID,
                d.RDate,
                d.XDate,
                PayDate = s.Number.StartsWith("RU") ? db.tWorkDates.Where(w => w.WorkDate > d.RDate).OrderBy(w => w.WorkDate).Take(10).Max(w => w.WorkDate) : d.RDate.AddDays(30),
                d.SecurityID,
                d.Value,
                SecBrief = s.NameBrief.TrimEnd(),
                SecName = s.Name1,
                Category = sk.Brief.TrimEnd(),
                SecGroup = sg.NameBrief.TrimEnd(),
                Fund = f.NameBrief.TrimEnd(),
                ISIN = s.Number,
                e.INN,
                ValueDiv = dd.ValueDiv,
                CountDiv = dd.CountDiv,
                QtyPayed = (decimal?)dd.QtyPayed,
                CountPayed = dd.CountPayed,
                PercPayed = (dd.ValueDiv ?? 0) == 0 ? 0 : Math.Round(((double?)dd.QtyPayed / ((double?)dd.ValueDiv)) ?? 0, 2, MidpointRounding.AwayFromZero) * 100,
                d.CheckKD,
                d.CheckPayed
              };
      return q;
    }

    public bool delDiv(List<tDiv> data)
    {
      try
      {
        IEnumerable<tDiv> e = db.tDivs.Where(p => data.Select(n => n.Id).Contains(p.Id));
        db.tDivs.DeleteAllOnSubmit(e);
        db.SubmitChanges();
        return true;
      }
      catch (Exception /*ex*/)
      {
        return false;
      }
    }

    public IEnumerable<dynamic> getDiv2List(string sort, string dir, int id)
    {
      var q = from c1 in
                (
                  from c in
                    (
                      from d in db.tDivs
                      join a in db.tAccounts on d.SecurityID equals a.SecurityID
                      join ttt1 in db.tTreatyTreatyTypes.Where(t => new int[] { 1, 2, 340 }.Contains(t.TreatyTypeID)) on a.TreatyID equals ttt1.TreatyID
                      join ab in db.tAccountBalances.Where(ab => ab.OutcomeBalanceP > 0) on new { a.AccountID, d.RDate } equals new { ab.AccountID, RDate = ab.BalanceDate }
                      where d.Id == id
                      select new
                      {
                        DivId = d.Id,
                        a.TreatyID,
                        ab.OutcomeBalanceP
                      })
                  group c by new { c.DivId, c.TreatyID } into g
                  select new
                  {
                    g.Key.DivId,
                    g.Key.TreatyID,
                    OutcomeBalanceP = g.Sum(p => p.OutcomeBalanceP)
                  })
              join d in db.tDivs on c1.DivId equals d.Id
              join s in db.tSecurities on d.SecurityID equals s.SecurityID
              join e in db.tFinancialInstitutions on s.IssuerID equals e.FinancialInstitutionID
              join tr in db.tTreaties on c1.TreatyID equals tr.TreatyID
              join fi in db.tFinancialInstitutions on tr.FinancialInstitutionID equals fi.FinancialInstitutionID
              join f in db.tSecurities on d.FundID equals f.SecurityID
              from dp in
                (from dr in db.tDivRels
                 join dc in db.tDictionariesConnections.Where(p => p.Dictionary == -461522885) on dr.DiasoftDealID equals dc.DiasoftBOID
                 join dl in db.tDeals on dc.CompositeID equals dl.DealID
                 join ttt1 in db.tTreatyTreatyTypes.Where(t => new int[] { 1, 2, 340 }.Contains(t.TreatyTypeID)) on dl.LeftSideID equals ttt1.TreatyID
                 where (dr.TreatyID.HasValue ? dr.TreatyID.Value : dl.LeftSideID) == c1.TreatyID
                 from abt in
                   (
                     from abt in db.tAccountBalanceTurns
                     where abt.DealID == dl.DealID && abt.AccountBalanceTurnTypeID == 7
                     group abt by 1 into g
                     select new { QtyPayed = g.Sum(p => p.TurnValue) }
                     ).DefaultIfEmpty()
                 where dr.DivId == d.Id
                 group new { dr.Qty, abt.QtyPayed, dl.LeftSideID } by 1 into g
                 select new
                 {
                   TreatyID = g.Min(p => p.LeftSideID),
                   QtyPayed = g.Sum(p => (double?)(p.Qty.HasValue ? p.Qty : p.QtyPayed) )
                 }).DefaultIfEmpty()
              select new
              {
                d.SecurityID,
                SecBrief = s.NameBrief.TrimEnd(),
                SecName = s.Name1,
                ISIN = s.Number,
                TreatyID = (int?)dp.TreatyID,
                TreatyName = tr.NameBrief.TrimEnd(),
                ClientName = fi.NameBrief.TrimEnd(),
                dt = d.RDate,
                Rest = c1.OutcomeBalanceP,
                ValueDiv = (double?)c1.OutcomeBalanceP * d.Value,
                Fund = f.NameBrief.TrimEnd(),
                Stavka = d.Value,
                QtyPayed = (decimal?)dp.QtyPayed,
                PercPayed = ((double?)c1.OutcomeBalanceP * d.Value == 0) ? 100 : Math.Round(((double?)dp.QtyPayed / ((double?)c1.OutcomeBalanceP * d.Value == 0 ? null : ((double?)c1.OutcomeBalanceP * d.Value)) ?? 0), 2, MidpointRounding.AwayFromZero) * 100,
                e.INN
              };

      return q;
    }

    public IEnumerable<dynamic> getDiv3List(string sort, string dir, int? id, int? TreatyID)
    {
      var q = from d in db.tDivs
              from s in db.tSecurities.Where(s => s.SecurityID == d.SecurityID || (s.ParentID == d.SecurityID && s.IsDeleted == 0))
              join e in db.tFinancialInstitutions on s.IssuerID equals e.FinancialInstitutionID
              from dl in db.tDeals.Where(dl => dl.DealTypeID == 18 && (dl.LeftSideID == TreatyID || (TreatyID == 7011 && dl.DealDate >= new DateTime(2016, 10, 17) && dl.LeftSideID == 5904)) && dl.SecurityID == s.SecurityID && dl.DealDate >= (d.XDate ?? d.RDate) && dl.DealDate <= d.RDate.AddDays(180))
              from abt in
                (
                  from abt in db.tAccountBalanceTurns
                  where abt.DealID == dl.DealID && abt.AccountBalanceTurnTypeID == 7
                  group abt by 1 into g
                  select new { vdt = g.Max(p => p.FixDate), QtyPayed = g.Sum(p => p.TurnValue) }
                  ).DefaultIfEmpty()
              join f in db.tSecurities on dl.FundID equals f.SecurityID
              join tr in db.tTreaties on dl.LeftSideID equals tr.TreatyID
              join fi in db.tFinancialInstitutions on tr.FinancialInstitutionID equals fi.FinancialInstitutionID
              //from sr in db.tSecurityRates.Where(sr => sr.SecurityID == dl.FundID && sr.RateType == 0 && sr.Date <= dl.DealDate).OrderByDescending(sr => sr.Date).Take(1)
              join dc in db.tDictionariesConnections.Where(p => p.Dictionary == -461522885) on dl.DealID equals dc.CompositeID
              join dr in db.tDivRels on new { DiasoftDealID = dc.DiasoftBOID.Value, DivId = d.Id } equals new { DiasoftDealID = dr.DiasoftDealID, dr.DivId } into _dr
              from dr in _dr.DefaultIfEmpty()
              where d.Id == id
              select new
              {
                id = dc.DiasoftBOID,
                s.SecurityID,
                SecBrief = s.NameBrief.TrimEnd(),
                SecName = s.Name1,
                ISIN = s.Number,
                tr.TreatyID,
                TreatyName = tr.NameBrief.TrimEnd(),
                ClientName = fi.NameBrief.TrimEnd(),
                dt = dl.DealDate,
                vd = abt.vdt,
                Qty = dl.CouponQuantity,
                QtyPayed = (decimal?)abt.QtyPayed,
                Fund = f.NameBrief.TrimEnd(),
                dl.Comment,
                RelId = (int?)dr.Id,
                e.INN
              };
      if (String.IsNullOrEmpty(sort))
        q = q.OrderBy(p => p.dt);
      else
        q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
      return q;
    }

    public IEnumerable<dynamic> getDiv4List(string sort, string dir, DateTime? d1, DateTime? d2, string n)
    {
      var qs = from s in db.tSecurities
               where new byte?[] { 0, 15 }.Contains(s.SecType) && s.IsDeleted == 0
               select s;

      if (!String.IsNullOrEmpty(n))
        qs = qs.Where(p => p.NameBrief.Contains(n) || p.Name1.Contains(n) || p.Number.StartsWith(n));

      var q = from dl in db.tDeals
              from abt in
                (
                  from abt in db.tAccountBalanceTurns
                  where abt.DealID == dl.DealID && abt.AccountBalanceTurnTypeID == 7
                  group abt by 1 into g
                  select new { vdt = g.Max(p => p.FixDate), QtyPayed = g.Sum(p => p.TurnValue) }
                  ).DefaultIfEmpty()
              join tr in db.tTreaties on dl.LeftSideID equals tr.TreatyID
              join ttt1 in db.tTreatyTreatyTypes.Where(t => new int[] { 1, 2, 340 }.Contains(t.TreatyTypeID)) on tr.TreatyID equals ttt1.TreatyID
              join f in db.tSecurities on dl.FundID equals f.SecurityID
              join fi in db.tFinancialInstitutions on tr.FinancialInstitutionID equals fi.FinancialInstitutionID
              join s in qs on dl.SecurityID equals s.SecurityID
              join dc in db.tDictionariesConnections.Where(p => p.Dictionary == -461522885) on dl.DealID equals dc.CompositeID
              join dr in db.tDivRels on new { DiasoftDealID = dc.DiasoftBOID.Value } equals new { DiasoftDealID = dr.DiasoftDealID } into _dr
              from dr in _dr.DefaultIfEmpty()
              where dl.DealDate >= d1 && dl.DealDate <= d2 && dl.DealTypeID == 18
              select new
              {
                id = dc.DiasoftBOID,
                s.SecurityID,
                SecBrief = s.NameBrief.TrimEnd(),
                SecName = s.Name1,
                ISIN = s.Number,
                tr.TreatyID,
                TreatyName = tr.NameBrief.TrimEnd(),
                ClientName = fi.NameBrief.TrimEnd(),
                dt = dl.DealDate,
                vdt = abt.vdt,
                Qty = dl.CouponQuantity,
                QtyPayed = (decimal?)abt.QtyPayed,
                Fund = f.NameBrief.TrimEnd(),
                dl.Comment,
                RelId = (int?)dr.Id
              };

      //      if (!String.IsNullOrEmpty(n))
      //        q = q.Where(p => p.SecBrief.TrimEnd().Contains(n) || p.SecName.Contains(n) || p.ISIN.StartsWith(n));
      if (String.IsNullOrEmpty(sort))
        q = q.OrderBy(p => p.dt);
      else
        q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
      return q;
    }

    public IEnumerable<dynamic> getDiv5List(string sort, string dir, decimal? Id)
    {
      var q = from dc in db.tDictionariesConnections
              join dl in db.tDeals on dc.CompositeID equals dl.DealID
              join d in db.tDivs on dl.SecurityID equals d.SecurityID
              join s in db.tSecurities on d.SecurityID equals s.SecurityID
              join sk in db.tSecKinds on s.Category equals sk.SecKindID
              join f in db.tSecurities on d.FundID equals f.SecurityID
              join ssg in db.tSecuritySecurityGroups on d.SecurityID equals ssg.SecurityID
              join sg in db.tSecurityGroups.Where(p => p.SecurityGroupTypeID == 1) on ssg.SecurityGroupID equals sg.SecurityGroupID
              join dr in db.tDivRels on new { DiasoftDealID = dc.DiasoftBOID.Value, DivId = d.Id } equals new { DiasoftDealID = dr.DiasoftDealID, dr.DivId } into _dr
              from dr in _dr.DefaultIfEmpty()
              where dc.Dictionary == -461522885 && dc.DiasoftBOID == Id
              select new
              {
                id = d.Id,
                d.FundID,
                d.RDate,
                d.XDate,
                d.SecurityID,
                d.Value,
                SecBrief = s.NameBrief.TrimEnd(),
                SecName = s.Name1,
                Category = sk.Brief.TrimEnd(),
                SecGroup = sg.NameBrief.TrimEnd(),
                Fund = f.NameBrief.TrimEnd(),
                ISIN = s.Number,
                RelId = (int?)dr.Id
              };

      if (String.IsNullOrEmpty(sort))
        q = q.OrderByDescending(p => p.RDate);
      else
        q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
      return q;
    }

    private bool WrongLines(tDivRel o)
    {
      return (from dc in db.tDictionariesConnections
              join dl in db.tDeals on dc.CompositeID equals dl.DealID
              where dc.Dictionary == -461522885 && dc.DiasoftBOID == o.DiasoftDealID
              select dl.SecurityID).FirstOrDefault() != (db.tDivs.Where(p => p.Id == o.DivId).Select(p => p.SecurityID).FirstOrDefault());
    }

    public IEnumerable<dynamic> addDivRel(List<tDivRel> data)
    {
      data.RemoveAll(WrongLines);
      foreach (var e in data)
      {
        var o = db.tDivRels.Where(p => p.DiasoftDealID == e.DiasoftDealID);
        if (o != null)
          db.tDivRels.DeleteAllOnSubmit(o);
        e.InDateTime = DateTime.Now;
      }
      db.tDivRels.InsertAllOnSubmit(data);
      db.SubmitChanges();

      return (from dr in db.tDivRels
              where data.Select(n => n.Id).Contains(dr.Id)
              select new
              {
                dr.Id,
                dr.DivId,
                dr.DiasoftDealID
              }).ToList();
    }

    public IEnumerable<dynamic> getSecurity(int id)
    {
      return from s in db.tSecurities
             where s.SecurityID == id
             select new
             {
               id = s.SecurityID,
               brief = s.NameBrief.TrimEnd(),
               name = s.Name1.TrimEnd(),
               isin = s.Number
             };
    }

    public IEnumerable<dynamic> getSecurities(string query, int limit)
    {
      return from s in db.tSecurities
             where new byte?[] { 0, 15 }.Contains(s.SecType) && s.ParentID == null && (s.NameBrief.Contains(query) || s.Name1.Contains(query) || s.Number.StartsWith(query) || s.RegistrationNumber.StartsWith(query))
             orderby s.NameBrief
             select new
             {
               id = s.SecurityID,
               brief = s.NameBrief.TrimEnd(),
               name = s.Name1.TrimEnd(),
               isin = s.Number,
               fundid = s.RatedSecurityID
             };
    }

    public IEnumerable<dynamic> GetFunds()
    {
      return from p in db.tSecurities
             where p.SecurityID == 39191 || p.SecurityID == 39192 || p.SecurityID == 39199 || p.SecurityID == 39188
             select new
             {
               id = p.SecurityID,
               brief = p.NameBrief.TrimEnd(),
               name = p.Name1.TrimEnd()
             };
    }

    public IEnumerable<dynamic> getDiv7List(string sort, string dir, DateTime? d1, DateTime? d2, string n)
    {
      var q1 = from dv in db.tDivs select new { dv.RDate, dv.SecurityID };
      if (d1.HasValue)
        q1 = q1.Where(a => a.RDate >= d1);
      if (d2.HasValue)
        q1 = q1.Where(a => a.RDate <= d2);

      var q2 = from tr in db.tTreaties
               join fi in db.tFinancialInstitutions on tr.FinancialInstitutionID equals fi.FinancialInstitutionID
               select new
               {
                 id = tr.TreatyID,
                 trBrief = tr.NameBrief,
                 fiName = fi.Name,
                 fiBrief = fi.NameBrief
               };
      if (!String.IsNullOrEmpty(n))
        q2 = q2.Where(p => p.trBrief.Contains(n) || p.fiBrief.Contains(n) || p.fiName.Contains(n));

      var q =
        from t in
          (
            (from dv in q1
             join a in db.tAccounts on new { dv.SecurityID } equals new { a.SecurityID }
             join ttt1 in db.tTreatyTreatyTypes.Where(t => new int[] { 1, 2, 340 }.Contains(t.TreatyTypeID)) on a.TreatyID equals ttt1.TreatyID
             join ab in db.tAccountBalances.Where(p => p.OutcomeBalanceP > 0) on new { a.AccountID, dv.RDate } equals new { ab.AccountID, RDate = ab.BalanceDate }
             select new
             {
               TreatyID = a.TreatyID
             }).Distinct())
        join _q2 in q2 on t.TreatyID equals _q2.id
        //join tr in db.tTreaties on t.TreatyID equals tr.TreatyID
        //join fi in db.tFinancialInstitutions on tr.FinancialInstitutionID equals fi.FinancialInstitutionID
        select new
        {
          id = t.TreatyID,
          trBrief = _q2.trBrief,
          fiName = _q2.fiName,
          fiBrief = _q2.fiBrief
        };

      if (String.IsNullOrEmpty(sort))
        q = q.OrderBy(p => p.fiBrief);
      else
        q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);

      return q;
    }

    public IEnumerable<dynamic> getDiv8List(string sort, string dir, DateTime? d1, DateTime? d2, int? tid, string n)
    {
      var qs = from s in db.tSecurities select s;
      if (!String.IsNullOrEmpty(n))
        qs = qs.Where(p => p.NameBrief.Contains(n) || p.Name1.Contains(n) || p.Number.StartsWith(n));

      var q = from d in db.tDivs
              from dd in
                (
                  from t in
                    (
                      from t in
                        (
                          from a in db.tAccounts.Where(p => p.TreatyID == tid)
                          join ab in db.tAccountBalances.Where(ab => ab.OutcomeBalanceP > 0) on new { a.AccountID, d.RDate } equals new { ab.AccountID, RDate = ab.BalanceDate }
                          where a.SecurityID == d.SecurityID
                          select new
                          {
                            TreatyID = a.TreatyID,
                            Rest = ab.OutcomeBalanceP,
                            ValueDiv = (double?)ab.OutcomeBalanceP * d.Value,
                            QtyPayed = (double?)0
                          }
                          ).Union
                          (
                          from dr in db.tDivRels
                          join dc in db.tDictionariesConnections.Where(p => p.Dictionary == -461522885) on dr.DiasoftDealID equals dc.DiasoftBOID
                          join dl in db.tDeals.Where(p => p.LeftSideID == tid) on dc.CompositeID equals dl.DealID
                          from abt in
                            (
                              from abt in db.tAccountBalanceTurns
                              where abt.DealID == dl.DealID && abt.AccountBalanceTurnTypeID == 7
                              group abt by 1 into g
                              select new { QtyPayed = g.Sum(p => p.TurnValue) }
                              ).DefaultIfEmpty()
                          where dr.DivId == d.Id
                          select new
                          {
                            TreatyID = dl.LeftSideID,
                            Rest = (decimal)0,
                            ValueDiv = (double?)0,
                            QtyPayed = (double?)abt.QtyPayed
                          }
                          )
                      group t by t.TreatyID into g
                      select new
                      {
                        Rest = g.Sum(p => p.Rest),
                        ValueDiv = g.Sum(p => p.ValueDiv),
                        QtyPayed = g.Sum(p => p.QtyPayed)
                      }
                      )
                  group t by 1 into g
                  select new { Rest = g.Sum(p => p.Rest), ValueDiv = g.Sum(p => p.ValueDiv), QtyPayed = g.Sum(p => p.QtyPayed) }
                  )
              join s in qs on d.SecurityID equals s.SecurityID
              join e in db.tFinancialInstitutions on s.IssuerID equals e.FinancialInstitutionID
              join sk in db.tSecKinds on s.Category equals sk.SecKindID
              join f in db.tSecurities on d.FundID equals f.SecurityID
              join ssg in db.tSecuritySecurityGroups on d.SecurityID equals ssg.SecurityID
              join sg in db.tSecurityGroups.Where(p => p.SecurityGroupTypeID == 1) on ssg.SecurityGroupID equals sg.SecurityGroupID
              select new
              {
                id = d.Id,
                d.FundID,
                d.RDate,
                d.XDate,
                PayDate = s.Number.StartsWith("RU") ? db.tWorkDates.Where(w => w.WorkDate > d.RDate).OrderBy(w => w.WorkDate).Take(10).Max(w => w.WorkDate) : d.RDate.AddDays(30),
                d.SecurityID,
                d.Value,
                SecBrief = s.NameBrief.TrimEnd(),
                SecName = s.Name1,
                Category = sk.Brief,
                SecGroup = sg.NameBrief.TrimEnd(),
                Fund = f.NameBrief.TrimEnd(),
                ISIN = s.Number,
                e.INN,
                dd.Rest,
                ValueDiv = dd.ValueDiv,
                QtyPayed = (decimal?)dd.QtyPayed,
                PercPayed = (dd.ValueDiv ?? 0) == 0 ? 100 : Math.Round(((double?)dd.QtyPayed / ((double?)dd.ValueDiv)) ?? 0, 2, MidpointRounding.AwayFromZero) * 100
              };

      if (d1.HasValue)
        q = q.Where(a => a.RDate >= d1);
      if (d2.HasValue)
        q = q.Where(a => a.RDate <= d2);

      if (String.IsNullOrEmpty(sort))
        q = q.OrderBy(p => p.RDate);
      else
        q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);

      return q;
    }

  }
}