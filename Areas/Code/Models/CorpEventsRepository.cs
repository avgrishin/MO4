using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using MO.Helpers;
using MO.Models;
using MvcContrib.Sorting;
using Newtonsoft.Json;
using System.Net;

namespace MO.Areas.Code.Models
{
  public interface ICorpEventsRepository
  {
    ISingleResult<up_avgGetEmitentsResult> up_avgGetEmitents(DateTime? d1, DateTime? d2, string n);
    ISingleResult<up_avgGetEmitSecurityResult> up_avgGetEmitSecurity(DateTime? d, int? issuerID);
    IEnumerable<up_avgGetRestSecurityResult> up_avgGetRestSecurity(DateTime? d, int? securityID, string sort, string dir);
    IEnumerable<dynamic> getCorpEventsList();
    IEnumerable<dynamic> getCorpEventsDateList(int id);
    IEnumerable<dynamic> getCorpEmitentEvents(int? FinInstID, int? SecurityID, int? eventID, int? dateID, DateTime? d1, DateTime? d2, Boolean? all, Boolean? isuk, string n, int? start, int? limit, ref int? cnt);
    Tuple<int, string>[] getCorpEventsFields();
    Tuple<int, string>[] getCorpEventsColumns();
    string getEventHtml(string id);
    ISingleResult<up_avgGetEmitClientsResult> up_avgGetEmitClients(DateTime? d, string n);
    ISingleResult<up_avgGetEmitClientRestsResult> up_avgGetEmitClientRests(DateTime? d, int? f);
    ISingleResult<up_avgGetEventSecurityResult> up_avgGetEventSecurity(int? EventID, int? DateID);
    IEnumerable<dynamic> CorpEmitentEventsUpdate2(List<up_avgGetEmitentEvents2> data);
    IEnumerable<dynamic> CorpEmitentEventsUpdate3(List<up_avgGetEmitentEvents3> data);
    IEnumerable<dynamic> CorpEmitentEventsUpdate4(List<up_avgGetEmitentEvents4> data);
    IEnumerable<dynamic> CorpEmitentEventsUpdate5(List<up_avgGetEmitentEvents5> data);
    IEnumerable<dynamic> CorpEmitentEventsUpdate95(List<up_avgGetEmitentEvents95> data);
    ISingleResult<up_avgGetDividendsResult> up_avgGetDividends(DateTime? d1, DateTime? d2);
    IEnumerable<dynamic> GetDividends3(DateTime? d1, DateTime? d2, string n, string sort, string dir);
    IEnumerable<dynamic> GetDividends3_2(int? IssuerID, decimal? Category, DateTime? d, decimal? Stavka, string sort, string dir);
    IEnumerable<dynamic> GetDividends3_3(int? SecurityID, int? TreatyID, DateTime? d, string sort, string dir);
    bool ClnEventsAdd(List<ClnEvents> data);
    IEnumerable<up_avgGetEngagementsResult> up_avgGetEngagements(DateTime? d, string sort, string dir);
    ISingleResult<up_avgGetEngagementDealsResult> up_avgGetEngagementDeals(DateTime? d, int? f, int? s, string r);

    IEnumerable<dynamic> getEnregList(DateTime? d1, DateTime? d2, Boolean? sd, string sort, string dir);
    IEnumerable<dynamic> getTreaties(string q, int limit);
    IEnumerable<dynamic> addEnreg(List<tEnregistrement> data);
    IEnumerable<dynamic> updEnreg(List<tEnregistrement> data, string Name);
    bool delEnreg(List<tEnregistrement> data);
    IEnumerable<dynamic> getObjClsByParent(int id);
    IEnumerable<dynamic> getStrategy();
    IEnumerable<dynamic> getEnregDetList(int? id);
    IEnumerable<dynamic> addEnregDet(List<tEnregistrementDet> data);
    IEnumerable<dynamic> updEnregDet(List<tEnregistrementDet> data, string Name);
    bool delEnregDet(List<tEnregistrementDet> data);
    IEnumerable<dynamic> getMaxNumEnreg();
    bool confirmEnrDet(Guid id, numDep t, string Name, ref bool isSendMail, ref string dep);
    bool refusalEnrDet(Guid id, numDep t, string Login, string Descr, ref int td);
    EnregDet getEnregDet(Guid id);
    bool enrdetCourriel(Guid id, string host, enNext next, string descr);
    IEnumerable<dynamic> getEnregDetLogList(int id);
    IEnumerable<dynamic> getDaysDog(int id, int? dtid);
    numDep findNextEnrDet(Guid id, bool isNext);
    bool canRefuseEnregDet(Guid id, numDep t);
    bool enrdetRappel(string host, int type);
    IEnumerable<dynamic> getEnreg(int id);
    bool toggleStatus(int id, string status, string Name);
    DateTime? getCancelDate(int TreatyID, DateTime recuDate);
    DateTime? getOutDate(int TreatyID, DateTime cancelDate);
  }

  public enum numDep { IsAll = 0, IsKM = 1, IsUA = 2, IsBU = 3, IsRC = 4, IsPM = 5, IsSS = 6, IsNone = 10 }
  public enum enNext { isPrev, isNext, isAll, isSS }

  public class clsDep
  {
    public numDep dep { get; set; }
    public bool? value { get; set; }
  }

  public class CorpEventsRepository : ICorpEventsRepository
  {
    private MiddleOfficeDataContext db = new MiddleOfficeDataContext() { CommandTimeout = 600 };

    public ISingleResult<up_avgGetEmitentsResult> up_avgGetEmitents(DateTime? d1, DateTime? d2, string n)
    {
      return db.up_avgGetEmitents(d1, d2, n);
    }

    public ISingleResult<up_avgGetEmitSecurityResult> up_avgGetEmitSecurity(DateTime? d, int? issuerID)
    {
      return db.up_avgGetEmitSecurity(d, issuerID);
    }

    public IEnumerable<up_avgGetRestSecurityResult> up_avgGetRestSecurity(DateTime? d, int? securityID, string sort, string dir)
    {
      if (!String.IsNullOrEmpty(sort))
        return db.up_avgGetRestSecurity(d, securityID).OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);

      return db.up_avgGetRestSecurity(d, securityID);
    }

    public IEnumerable<dynamic> getCorpEventsList()
    {
      var q = from a in db.tClasses
              where a.IDPrnt == 12
              select new { id = a.IDChild, text = a.ChildName };
      return q;
    }

    public IEnumerable<dynamic> getCorpEventsDateList(int id)
    {
      var q = from a in db.tClasses
              where a.IDPrnt == id && new int[] { 6, 7 }.Contains(a.ChildType)
              select new { id = a.IDChild, text = a.ChildName };
      return q;
    }

    public IEnumerable<dynamic> getCorpEmitentEvents(int? FinInstID, int? SecurityID, int? eventID, int? dateID, DateTime? d1, DateTime? d2, Boolean? all, Boolean? isuk, string n, int? start, int? limit, ref int? cnt)
    {
      if (eventID == 2)
        return db.up_avgGetEmitentEvents(FinInstID, SecurityID, dateID, d1, d2, all, isuk, n, start, limit, ref cnt).GetResult<up_avgGetEmitentEvents2>();
      if (eventID == 3)
        return db.up_avgGetEmitentEvents(FinInstID, SecurityID, dateID, d1, d2, all, isuk, n, start, limit, ref cnt).GetResult<up_avgGetEmitentEvents3>();
      if (eventID == 4)
        return db.up_avgGetEmitentEvents(FinInstID, SecurityID, dateID, d1, d2, all, isuk, n, start, limit, ref cnt).GetResult<up_avgGetEmitentEvents4>();
      if (eventID == 5)
        return db.up_avgGetEmitentEvents(FinInstID, SecurityID, dateID, d1, d2, all, isuk, n, start, limit, ref cnt).GetResult<up_avgGetEmitentEvents5>();
      if (eventID == 95)
        return db.up_avgGetEmitentEvents(FinInstID, SecurityID, dateID, d1, d2, all, isuk, n, start, limit, ref cnt).GetResult<up_avgGetEmitentEvents95>();
      return null;
    }

    public Tuple<int, string>[] getCorpEventsFields()
    {
      var p = (from a in db.tClasses where a.IDPrnt == 12 select a.IDChild).ToArray();
      var q = new Tuple<int, string>[p.Count()];
      for (int j = 0; j < p.Count(); j++)
      {
        var c = (from a in db.tClasses
                 where (a.IDPrnt == p[j] && a.ChildType != 5 && a.Visible == true) || new int[] { 57, 58, 59, 114, 116 }.Contains(a.IDChild)
                 select new { a.ChildType, a.Comment }).ToList();

        StringBuilder sb = new StringBuilder();
        sb.Append("'ID'");
        for (int i = 0; i < c.Count(); i++)
        {
          if (new int[] { 6, 7 }.Contains(c[i].ChildType))
            sb.Append(string.Format(",{{name:'{0}',type:'date',dateFormat:'c'}}", c[i].Comment));
          else if (c[i].ChildType == 4)
            sb.Append(string.Format(",{{name:'{0}',type:'boolean'}}", c[i].Comment));
          else
            sb.Append(string.Format(",'{0}'", c[i].Comment));
        }
        sb.Append(",'FinInstID'");
        q[j] = Tuple.Create<int, string>(p[j], sb.ToString());
      }
      return q;
    }

    public Tuple<int, string>[] getCorpEventsColumns()
    {
      JsonSerializerSettings jss = new JsonSerializerSettings() { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore };
      var p = (from a in db.tClasses where a.IDPrnt == 12 select a.IDChild).ToArray();
      var q = new Tuple<int, string>[p.Count()];
      for (int j = 0; j < p.Count(); j++)
      {
        q[j] = Tuple.Create<int, string>(p[j], JsonConvert.SerializeObject(
          (from a in
             (
               (from a in db.tClasses
                where (a.IDPrnt == p[j] && a.ChildType != 5 && a.Visible == true) || new int[] { 57, 58, 59, 114, 116 }.Contains(a.IDChild)
                orderby a.IDPrnt == p[j] ? 0 : 1, a.IDChild
                select new
                {
                  header = a.ChildName,
                  dataIndex = a.Comment,
                  width = a.Size,
                  a.ChildType

                }).ToList())
           select new
           {
             header = a.header,
             dataIndex = a.dataIndex,
             width = a.width,
             xtype = new int[] { 6, 7 }.Contains(a.ChildType) ? (object)"datecolumn" : new int[] { 2, 3 }.Contains(a.ChildType) ? (object)"numbercolumn" : new int[] { 4 }.Contains(a.ChildType) ? (object)"booleancolumn" : null,
             format = new int[] { 6, 7 }.Contains(a.ChildType) ? (object)"d.m.Y" : null,
             trueText = a.ChildType == 4 ? (object)"да" : null,
             falseText = a.ChildType == 4 ? (object)"нет" : null,
             //hidden = a.ChildType == 8 ? (object)true : null,
             dir = a.ChildType == 8 ? (object)true : null,
             renderer = a.ChildType == 8 ? (object)"renderFile" : null
           }), jss));
      }
      return q;
    }

    public string getEventHtml(string id)
    {
      if (id.Contains("-B-B"))
        return (from a in db.tEDiscs
                where a.EventId == id
                select a.Message).FirstOrDefault();
      else
        return (from a in db.tIfxNews
                where a.IfxID == id
                select a.Message).FirstOrDefault();
    }

    public ISingleResult<up_avgGetEmitClientsResult> up_avgGetEmitClients(DateTime? d, string n)
    {
      return db.up_avgGetEmitClients(d, n);
    }

    public ISingleResult<up_avgGetEmitClientRestsResult> up_avgGetEmitClientRests(DateTime? d, int? f)
    {
      return db.up_avgGetEmitClientRests(d, f);
    }

    public ISingleResult<up_avgGetEventSecurityResult> up_avgGetEventSecurity(int? EventID, int? DateID)
    {
      return db.up_avgGetEventSecurity(EventID, DateID);
    }

    private void CorpEmitentEventsUpdate(int? ID, int? FinInstID, string FileName, DateTime? BLDate, DateTime? MessDate, DateTime? NoticeDate, bool? Is_UK)
    {
      var q3 = db.taLibs.FirstOrDefault(p => p.LConcept == 430251 && p.LID1 == ID && p.LID2 == FinInstID);
      if (q3 == null)
      {
        q3 = new taLib() { LConcept = 430251, LParent = 430251, LID1 = ID, LID2 = FinInstID, LName1 = FileName, LDate1 = BLDate, LDate2 = MessDate, LDate3 = NoticeDate, LInt1 = Is_UK == true ? 1 : 0, InDateTime = DateTime.Now };
        db.taLibs.InsertOnSubmit(q3);
      }
      else
      {
        q3.LName1 = FileName;
        q3.LDate1 = BLDate;
        q3.LDate2 = MessDate;
        q3.LDate3 = NoticeDate;
        q3.LInt1 = Is_UK == true ? 1 : 0;
      }
    }

    private void ChangeDate(int? ID, int? IDPrnt, string Comment, DateTime? dt)
    {
      var q2 = db.tClassDatas.Where(p => p.ExClass == ID && p.Class == db.tClasses.Where(b => b.IDPrnt == IDPrnt && b.Comment == Comment).FirstOrDefault().IDChild).FirstOrDefault();
      if (q2 != null)
        q2.V_Date = dt;
    }

    public IEnumerable<dynamic> CorpEmitentEventsUpdate2(List<up_avgGetEmitentEvents2> data)
    {
      CorpEmitentEventsUpdate(data[0].ID, data[0].FinInstID, data[0].FileName, data[0].BLDate, data[0].MessDate, data[0].NoticeDate, data[0].Is_UK);
      ChangeDate(data[0].ID, 2, "PublishingDate", data[0].PublishingDate);
      ChangeDate(data[0].ID, 2, "DecisionDate", data[0].DecisionDate);
      ChangeDate(data[0].ID, 2, "PerformanceDate", data[0].PerformanceDate);
      ChangeDate(data[0].ID, 2, "ProtocolDate", data[0].ProtocolDate);
      db.SubmitChanges();
      return data;
    }

    public IEnumerable<dynamic> CorpEmitentEventsUpdate3(List<up_avgGetEmitentEvents3> data)
    {
      CorpEmitentEventsUpdate(data[0].ID, data[0].FinInstID, data[0].FileName, data[0].BLDate, data[0].MessDate, data[0].NoticeDate, data[0].Is_UK);

      ChangeDate(data[0].ID, 3, "PublishingDate", data[0].PublishingDate);
      ChangeDate(data[0].ID, 3, "DecisionDate", data[0].DecisionDate);
      ChangeDate(data[0].ID, 3, "FinishDate", data[0].FinishDate);
      ChangeDate(data[0].ID, 3, "ProtocolDate", data[0].ProtocolDate);
      ChangeDate(data[0].ID, 3, "ListDate", data[0].ListDate);

      db.SubmitChanges();
      return data;
    }

    public IEnumerable<dynamic> CorpEmitentEventsUpdate4(List<up_avgGetEmitentEvents4> data)
    {
      CorpEmitentEventsUpdate(data[0].ID, data[0].FinInstID, data[0].FileName, data[0].BLDate, data[0].MessDate, data[0].NoticeDate, data[0].Is_UK);

      ChangeDate(data[0].ID, 4, "PublishingDate", data[0].PublishingDate);
      ChangeDate(data[0].ID, 4, "DecisionDate", data[0].DecisionDate);
      ChangeDate(data[0].ID, 4, "FinishDate", data[0].FinishDate);

      db.SubmitChanges();
      return data;
    }

    public IEnumerable<dynamic> CorpEmitentEventsUpdate5(List<up_avgGetEmitentEvents5> data)
    {
      CorpEmitentEventsUpdate(data[0].ID, data[0].FinInstID, data[0].FileName, data[0].BLDate, data[0].MessDate, data[0].NoticeDate, data[0].Is_UK);

      ChangeDate(data[0].ID, 5, "PublishingDate", data[0].PublishingDate);
      ChangeDate(data[0].ID, 5, "DecisionDate", data[0].DecisionDate);

      db.SubmitChanges();
      return data;
    }

    public IEnumerable<dynamic> CorpEmitentEventsUpdate95(List<up_avgGetEmitentEvents95> data)
    {
      CorpEmitentEventsUpdate(data[0].ID, data[0].FinInstID, data[0].FileName, data[0].BLDate, data[0].MessDate, data[0].NoticeDate, data[0].Is_UK);

      ChangeDate(data[0].ID, 95, "PublishingDate", data[0].PublishingDate);
      ChangeDate(data[0].ID, 95, "DecisionDate", data[0].DecisionDate);
      ChangeDate(data[0].ID, 95, "ProtocolDate", data[0].ProtocolDate);

      db.SubmitChanges();
      return data;
    }

    public ISingleResult<up_avgGetDividendsResult> up_avgGetDividends(DateTime? d1, DateTime? d2)
    {
      return db.up_avgGetDividends(d1, d2);
    }

    public void GetFields()
    {
      PropertyInfo[] fieldInfos;
      fieldInfos = typeof(up_avgGetDividendsResult).GetProperties();
    }

    public IEnumerable<dynamic> GetDividends3(DateTime? d1, DateTime? d2, string n, string sort, string dir)
    {
      var q = from d in
                ((
                  from d in db.tDividents
                  where d.INN != "" && d.DateSobr >= d1 && d.DateSobr <= d2 && d.StavkaAO != 0
                  select new { d.INN, d.DateFix, d.DateSobr, d.Source, d.Period, d.IsPayed, d.DateNews, d.Comment, Stavka = d.StavkaAO, Category = 10000000003, CatName = "АО" })
                  .Union(
                  from d in db.tDividents
                  where d.INN != "" && d.DateSobr >= d1 && d.DateSobr <= d2 && d.StavkaAP != 0
                  select new { d.INN, d.DateFix, d.DateSobr, d.Source, d.Period, d.IsPayed, d.DateNews, d.Comment, Stavka = d.StavkaAP, Category = 10000000004, CatName = "АП" })
                  )
              from f in
                (
                  from f in db.tFinancialInstitutions.Where(f => f.INN == d.INN && f.IsDisabled == 0)
                  from s in db.tSecurities.Where(s => s.IssuerID == f.FinancialInstitutionID && s.IsDeleted == 0 && s.SecType == 0 && s.Category == d.Category && s.ParentID == null)
                  select new { FinInstID = f.FinancialInstitutionID, NameBrief = f.NameBrief }).Take(1)
              select new { d.INN, d.DateFix, d.DateSobr, d.Source, d.Period, d.IsPayed, d.DateNews, d.Comment, d.Stavka, d.Category, IssuerName = f.NameBrief/*.TrimEnd()*/, IssuerID = f.FinInstID, d.CatName };
      if (!String.IsNullOrEmpty(n))
      {
        q = q.Where(w => w.INN == n || w.IssuerName.Contains(n));
      }
      if (!String.IsNullOrEmpty(sort))
      {
        q = q.OrderBy(p => p.DateSobr);
      }
      else
        q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
      return q;
    }

    public IEnumerable<dynamic> GetDividends3_2(int? IssuerID, decimal? Category, DateTime? d, decimal? Stavka, string sort, string dir)
    {
      var q = from s in db.tSecurities.Where(s => s.IssuerID == IssuerID && s.IsDeleted == 0 && s.SecType == 0 && s.Category == Category && s.ParentID == null)
              from s1 in
                ((
                  from s1 in db.tSecurities.Where(s1 => s1.SecurityID == s.SecurityID)
                  select new { s1.SecurityID, BaseSecQty = (decimal?)1 }).Union(
                  from s1 in db.tSecurities.Where(s1 => s1.BaseSecurityID == s.SecurityID && s1.SecType == 15 && s1.IsDeleted == 0)
                  select new { s1.SecurityID, s1.BaseSecQty }))
              join s2 in db.tSecurities on s1.SecurityID equals s2.SecurityID
              join a in db.tAccounts on s1.SecurityID equals a.SecurityID
              join ttt1 in db.tTreatyTreatyTypes.Where(t => new int[] { 1, 2, 340 }.Contains(t.TreatyTypeID)) on a.TreatyID equals ttt1.TreatyID
              join ab in db.tAccountBalances.Where(ab => ab.BalanceDate == d && ab.OutcomeBalanceP > 0) on a.AccountID equals ab.AccountID
              join tr in db.tTreaties on a.TreatyID equals tr.TreatyID
              join fi in db.tFinancialInstitutions on tr.FinancialInstitutionID equals fi.FinancialInstitutionID
              select new { s2.SecurityID, SecName = s2.Name1, ISIN = s2.Number, tr.TreatyID, TreatyName = tr.NameBrief, ClientName = fi.NameBrief, dt = d, Rest = ab.OutcomeBalanceP, s1.BaseSecQty, ValueDiv = ab.OutcomeBalanceP * s1.BaseSecQty * Stavka, Stavka = Stavka };
      if (!String.IsNullOrEmpty(sort))
      {
        q = q.OrderBy(p => p.ClientName);
      }
      else
        q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
      return q;
    }

    public IEnumerable<dynamic> GetDividends3_3(int? SecurityID, int? TreatyID, DateTime? d, string sort, string dir)
    {
      var q = from s in db.tSecurities.Where(s => s.SecurityID == SecurityID || (s.ParentID == SecurityID && s.IsDeleted == 0))
              from dl in db.tDeals.Where(dl => dl.DealTypeID == 18 && dl.LeftSideID == TreatyID && dl.SecurityID == s.SecurityID && dl.DealDate >= d && dl.DealDate <= d.Value.AddDays(180))
              join tr in db.tTreaties on dl.LeftSideID equals tr.TreatyID
              join fi in db.tFinancialInstitutions on tr.FinancialInstitutionID equals fi.FinancialInstitutionID
              from sr in db.tSecurityRates.Where(sr => sr.SecurityID == dl.FundID && sr.RateType == 0 && sr.Date <= dl.DealDate).OrderByDescending(sr => sr.Date).Take(1)
              select new { s.SecurityID, SecName = s.Name1, ISIN = s.Number, TreatyName = tr.NameBrief, ClientName = fi.NameBrief, dt = dl.DealDate, Qty = ((double)dl.CouponQuantity) * sr.Course };
      if (!String.IsNullOrEmpty(sort))
      {
        q = q.OrderBy(p => p.dt);
      }
      else
        q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
      return q;
    }

    public bool ClnEventsAdd(List<ClnEvents> data)
    {
      foreach (var d in data)
      {
        var q3 = db.taLibs.FirstOrDefault(p => p.LConcept == 430251 && p.LID1 == d.ID && p.LID2 == d.FinInstID);
        if (q3 == null)
        {
          q3 = new taLib() { LConcept = 430251, LParent = 430251, LID1 = d.ID, LID2 = d.FinInstID, LName1 = d.FileName, LDate1 = d.BLDate, LInt1 = d.Is_UK == true ? 1 : 0, InDateTime = DateTime.Now };
          db.taLibs.InsertOnSubmit(q3);
        }
        else
        {
          q3.LName1 = d.FileName;
          q3.LDate1 = d.BLDate;
          q3.LInt1 = d.Is_UK == true ? 1 : 0;
        }
      }
      db.SubmitChanges();
      return true;
    }

    public IEnumerable<up_avgGetEngagementsResult> up_avgGetEngagements(DateTime? d, string sort, string dir)
    {
      if (!String.IsNullOrEmpty(sort))
        return db.up_avgGetEngagements(d, 0).OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);

      return db.up_avgGetEngagements(d, 0);
    }

    public ISingleResult<up_avgGetEngagementDealsResult> up_avgGetEngagementDeals(DateTime? d, int? f, int? s, string r)
    {
      return db.up_avgGetEngagementDeals(d, f, s, r);
    }

    public IEnumerable<dynamic> getEnregList(DateTime? d1, DateTime? d2, Boolean? sd, string sort, string dir)
    {
      var q = from e in db.tEnregistrements.Where(p => p.DocDate >= d1 && p.DocDate <= d2)
              join tr in db.tTreaties on e.TreatyID equals tr.TreatyID into tr_
              from tr1 in tr_.DefaultIfEmpty()
              join f in db.tFinancialInstitutions on tr1.FinancialInstitutionID equals f.FinancialInstitutionID into f_
              from f in f_.DefaultIfEmpty()
              //from su in
              //  (from ap in db.tAccountPortfolios.Where(a => a.TreatyID == e.TreatyID && a.StartDate <= DateTime.Today && a.FinishDate > DateTime.Today)
              //   join p in db.tPortfolios.Where(p => p.PortfolioTypeID == 8) on ap.PortfolioID equals p.PortfolioID
              //   from pu in db.tPortfolioUsers.Where(a => a.PortfolioID == p.PortfolioID && a.StartDate <= DateTime.Today && a.FinishDate > DateTime.Today).Take(1)
              //   join u in db.tUsers on pu.UserID equals u.UserID
              //   select new { StrategyUser = u.NameLast }).DefaultIfEmpty()
              from us in db.tObjClassifiers.Where(a => a.ObjClassifierID == e.EmployeID).DefaultIfEmpty()
              from dt in db.tObjClassifiers.Where(a => a.ObjClassifierID == e.DocTypeID).DefaultIfEmpty()
              from st in db.tObjClassifiers.Where(a => a.ObjClassifierID == e.StrategyID).DefaultIfEmpty()
              select new
              {
                id = e.ID,
                e.Num,
                e.Numero,
                e.TreatyID,
                Portal = f.Portal.Trim(),
                trNameBrief = tr1.NameBrief.Trim(),
                ClnNameBrief = f.NameBrief.Trim(),
                ClnName = f.Name.Trim(),
                Number = tr1.Number.Trim(),
                DateStart = (tr1.DateStart as DateTime? ?? null),
                ClnManager = db.uf_avgEnregCM(e.ID),
                e.DocDate,
                e.RecuDate,
                e.Tm,
                e.Original,
                e.Fax,
                e.ScanCopy,
                e.DocTypeID,
                DTName = dt.Name,
                e.EmployeID,
                EmployeNom = us.Name,
                e.Remarque,
                e.CancelDate,
                e.OutDate,
                IsKM = e.IsKM,
                IsUA = e.IsUA,
                IsBU = e.IsBU,
                IsRC = e.IsRC,
                IsPM = e.IsPM,
                IsDone = (!(e.IsKM ?? false) || !(e.IsUA ?? false) || !(e.IsBU ?? false) || !(e.IsRC ?? false)) && !(e.Abolition ?? false),
                e.Abolition,
                e.StrategyID,
                Strategy = st.Name
              };
      if (sd ?? false)
        q = q.Where(p => p.IsDone == true);
      if (!String.IsNullOrEmpty(sort)) q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);

      //string[] s = new string[] { "d", "s" };
      //string s1 = s.ToString();
      return q;
    }

    public IEnumerable<dynamic> addEnreg(List<tEnregistrement> data)
    {
      db.tEnregistrements.InsertAllOnSubmit(data);
      foreach (var d in data)
        d.InDateTime = DateTime.Now;
      db.SubmitChanges();

      var q = from e in db.tEnregistrements.Where(p => data.Select(n => n.ID).Contains(p.ID))
              join tr in db.tTreaties on e.TreatyID equals tr.TreatyID into tr_
              from tr in tr_.DefaultIfEmpty()
              join f in db.tFinancialInstitutions on tr.FinancialInstitutionID equals f.FinancialInstitutionID into f_
              from f in f_.DefaultIfEmpty()
              //from su in
              //  (from ap in db.tAccountPortfolios.Where(a => a.TreatyID == e.TreatyID && a.StartDate <= DateTime.Today && a.FinishDate > DateTime.Today)
              //   join p in db.tPortfolios.Where(p => p.PortfolioTypeID == 8) on ap.PortfolioID equals p.PortfolioID
              //   from pu in db.tPortfolioUsers.Where(a => a.PortfolioID == p.PortfolioID && a.StartDate <= DateTime.Today && a.FinishDate > DateTime.Today).Take(1)
              //   join u in db.tUsers on pu.UserID equals u.UserID
              //   select new { StrategyUser = u.NameLast }).DefaultIfEmpty()
              from us in db.tObjClassifiers.Where(a => a.ObjClassifierID == e.EmployeID).DefaultIfEmpty()
              from dt in db.tObjClassifiers.Where(a => a.ObjClassifierID == e.DocTypeID).DefaultIfEmpty()
              from st in db.tObjClassifiers.Where(a => a.ObjClassifierID == e.StrategyID).DefaultIfEmpty()
              select new
              {
                id = e.ID,
                e.Num,
                e.Numero,
                e.TreatyID,
                Portal = f.Portal.Trim(),
                trNameBrief = tr.NameBrief.Trim(),
                ClnNameBrief = f.NameBrief.Trim(),
                ClnName = f.Name.Trim(),
                Number = tr.Number.Trim(),
                DateStart = tr.DateStart as DateTime?,
                ClnManager = db.uf_avgEnregCM(e.ID),
                e.DocDate,
                e.RecuDate,
                e.Tm,
                e.Original,
                e.Fax,
                e.ScanCopy,
                e.DocTypeID,
                DTName = dt.Name,
                e.EmployeID,
                EmployeNom = us.Name,
                e.Remarque,
                e.CancelDate,
                e.OutDate,
                IsKM = e.IsKM,
                IsUA = e.IsUA,
                IsBU = e.IsBU,
                IsRC = e.IsRC,
                IsPM = e.IsPM,
                IsDone = (!(e.IsKM ?? false) || !(e.IsUA ?? false) || !(e.IsBU ?? false) || !(e.IsRC ?? false)) && !(e.Abolition ?? false),
                e.Abolition,
                e.StrategyID,
                Strategy = st.Name
              };

      return q;
    }

    public IEnumerable<dynamic> updEnreg(List<tEnregistrement> data, string Name)
    {
      foreach (var e in data.Where(p => p.ID > 0))
      {
        var q1 = db.tEnregistrements.Where(p => p.ID == e.ID).First();
        if (q1 != null)
        {
          q1.DocDate = e.DocDate;
          q1.DocTypeID = e.DocTypeID;
          q1.EmployeID = e.EmployeID;
          q1.Fax = e.Fax;
          q1.ScanCopy = e.ScanCopy;
          //q1.InDateTime = DateTime.Now.ToString();
          q1.Num = e.Num;
          q1.Numero = e.Numero;
          q1.Original = e.Original;
          q1.RecuDate = e.RecuDate;
          q1.Remarque = e.Remarque;
          q1.CancelDate = e.CancelDate;
          q1.OutDate = e.OutDate;
          q1.Tm = e.Tm;
          q1.TreatyID = e.TreatyID;
          if (e.Abolition == true && (q1.Abolition ?? false) == false)
          {
            var q2 = db.tAccountPortfolios.FirstOrDefault(p => p.TreatyID == e.TreatyID && p.StartDate <= DateTime.Today && p.FinishDate > DateTime.Today);
            if (q2 != null)
            {
              if (q2.PortfolioID == 9937)
              {
                db.tAccountPortfolios.DeleteOnSubmit(q2);
                var q3 = db.tAccountPortfolios.Where(p => p.TreatyID == e.TreatyID && p.StartDate <= q2.StartDate && p.FinishDate <= q2.StartDate).OrderByDescending(p => p.StartDate).FirstOrDefault();
                if (q3 != null)
                  if (q3.PortfolioID != 9937)
                    q3.FinishDate = new DateTime(2050, 1, 1);
              }
            }
            foreach (var q3 in from tr in db.tTreaties
                               where tr.TreatyID == e.TreatyID
                               join tb in db.tTreaties on tr.FinancialInstitutionID equals tb.FinancialInstitutionID
                               join ttt in db.tTreatyTreatyTypes.Where(p => p.TreatyTypeID == 3) on tb.TreatyID equals ttt.TreatyID
                               select new { tb.TreatyID })
            {
              var q4 = db.tAccountPortfolios.FirstOrDefault(p => p.TreatyID == q3.TreatyID && p.StartDate <= DateTime.Today && p.FinishDate > DateTime.Today);
              if (q4 != null)
              {
                if (q4.PortfolioID == 9980)
                {
                  db.tAccountPortfolios.DeleteOnSubmit(q4);
                  var q5 = db.tAccountPortfolios.Where(p => p.TreatyID == q3.TreatyID && p.StartDate <= q4.StartDate && p.FinishDate <= q4.StartDate).OrderByDescending(p => p.StartDate).FirstOrDefault();
                  if (q5 != null)
                    if (q5.PortfolioID != 9980)
                      q5.FinishDate = new DateTime(2050, 1, 1);
                }
              }
            }
          }
          q1.Abolition = e.Abolition;
          q1.StrategyID = e.StrategyID;
          db.SubmitChanges();
        }
      }
      db.SubmitChanges();

      var q = from e in db.tEnregistrements.Where(p => data.Select(n => n.ID).Contains(p.ID))
              join tr in db.tTreaties on e.TreatyID equals tr.TreatyID into tr_
              from tr in tr_.DefaultIfEmpty()
              join f in db.tFinancialInstitutions on tr.FinancialInstitutionID equals f.FinancialInstitutionID into f_
              from f in f_.DefaultIfEmpty()
              //from su in
              //  (from ap in db.tAccountPortfolios.Where(a => a.TreatyID == e.TreatyID && a.StartDate <= DateTime.Today && a.FinishDate > DateTime.Today)
              //   join p in db.tPortfolios.Where(p => p.PortfolioTypeID == 8) on ap.PortfolioID equals p.PortfolioID
              //   from pu in db.tPortfolioUsers.Where(a => a.PortfolioID == p.PortfolioID && a.StartDate <= DateTime.Today && a.FinishDate > DateTime.Today).Take(1)
              //   join u in db.tUsers on pu.UserID equals u.UserID
              //   select new { StrategyUser = u.NameLast }).DefaultIfEmpty()
              from us in db.tObjClassifiers.Where(a => a.ObjClassifierID == e.EmployeID).DefaultIfEmpty()
              from dt in db.tObjClassifiers.Where(a => a.ObjClassifierID == e.DocTypeID).DefaultIfEmpty()
              from st in db.tObjClassifiers.Where(a => a.ObjClassifierID == e.StrategyID).DefaultIfEmpty()
              select new
              {
                id = e.ID,
                e.Num,
                e.Numero,
                e.TreatyID,
                Portal = f.Portal.Trim(),
                trNameBrief = tr.NameBrief.Trim(),
                ClnNameBrief = f.NameBrief.Trim(),
                ClnName = f.Name.Trim(),
                Number = tr.Number.Trim(),
                DateStart = tr.DateStart as DateTime?,
                ClnManager = db.uf_avgEnregCM(e.ID),
                e.DocDate,
                e.RecuDate,
                e.Tm,
                e.Original,
                e.Fax,
                e.ScanCopy,
                e.DocTypeID,
                DTName = dt.Name,
                e.EmployeID,
                EmployeNom = us.Name,
                e.Remarque,
                e.CancelDate,
                e.OutDate,
                IsKM = e.IsKM,
                IsUA = e.IsUA,
                IsBU = e.IsBU,
                IsRC = e.IsRC,
                IsPM = e.IsPM,
                IsDone = (!(e.IsKM ?? false) || !(e.IsUA ?? false) || !(e.IsBU ?? false) || !(e.IsRC ?? false)) && !(e.Abolition ?? false),
                e.Abolition,
                e.StrategyID,
                Strategy = st.Name
              };

      return q;
    }

    public bool delEnreg(List<tEnregistrement> data)
    {
      IEnumerable<tEnregistrement> e = db.tEnregistrements.Where(p => data.Select(n => n.ID).Contains(p.ID));
      db.tEnregistrements.DeleteAllOnSubmit(e);
      db.SubmitChanges();
      return true;
    }

    public IEnumerable<dynamic> getTreaties(string q, int limit)
    {
      return (from t in db.tTreaties
              join ttt in db.tTreatyTreatyTypes.Where(t => new int[] { 1, 2 }.Contains(t.TreatyTypeID)) on t.TreatyID equals ttt.TreatyID
              join f in db.tFinancialInstitutions on t.FinancialInstitutionID equals f.FinancialInstitutionID
              where t.IsDisabled == 0 && (t.DateFinish == new DateTime(1900, 1, 1) || t.DateFinish < DateTime.Today) && (t.NameBrief.Contains(q) || f.NameBrief.Contains(q))
              orderby t.NameBrief
              select new cFinInst { id = t.TreatyID, name = f.NameBrief.Trim(), brief = t.NameBrief.Trim() }).Take(limit);
    }

    public IEnumerable<dynamic> getObjClsByParent(int id)
    {
      return (from oc in db.tObjClassifiers
              where oc.ParentID == id
              orderby oc.Name
              select new
              {
                Value = oc.ObjClassifierID,
                Text = oc.Name
              });
    }

    public IEnumerable<dynamic> getStrategy()
    {
      return (from oc in db.tObjClassifiers
              where oc.ParentID == 66541 && System.Data.Linq.SqlClient.SqlMethods.Like(oc.NameBrief, "[1-9]%")
              orderby oc.Name
              select new
              {
                Value = oc.ObjClassifierID,
                Text = oc.Name
              });
    }

    public IEnumerable<dynamic> getEnregDetList(int? id)
    {
      var q = db.tEnregistrementDets.Where(d => d.EnregID == id).Select(n => new
      {
        id = n.ID,
        RecuDate = n.tEnregistrement.RecuDate,
        DateDoc = n.DaysDoc < 0 ? db.tWorkDates.Where(w => w.WorkDate <= n.tEnregistrement.RecuDate).OrderByDescending(w => w.WorkDate).Take(1 - n.DaysDoc ?? -1).Min(w => w.WorkDate as DateTime?) : db.tWorkDates.Where(w => w.WorkDate >= n.tEnregistrement.RecuDate).OrderBy(w => w.WorkDate).Take(1 + n.DaysDoc ?? 1).Max(w => w.WorkDate as DateTime?),
        DateDog = n.DayDogTypeID == 1 ? (n.DaysDog < 0 ? db.tWorkDates.Where(w => w.WorkDate <= n.tEnregistrement.RecuDate).OrderByDescending(w => w.WorkDate).Take(1 - n.DaysDog ?? -1).Min(w => w.WorkDate as DateTime?) : db.tWorkDates.Where(w => w.WorkDate >= n.tEnregistrement.RecuDate).OrderBy(w => w.WorkDate).Take(1 + n.DaysDog ?? 1).Max(w => w.WorkDate as DateTime?)) : n.tEnregistrement.RecuDate.Value.AddDays(n.DaysDog ?? 0),
        DateFact = n.DaysFact < 0 ? db.tWorkDates.Where(w => w.WorkDate <= n.tEnregistrement.RecuDate).OrderByDescending(w => w.WorkDate).Take(1 - n.DaysFact ?? -1).Min(w => w.WorkDate as DateTime?) : db.tWorkDates.Where(w => w.WorkDate >= n.tEnregistrement.RecuDate).OrderBy(w => w.WorkDate).Take(1 + n.DaysFact ?? 1).Max(w => w.WorkDate as DateTime?),
        n.DayDogTypeID,
        DayDogType = n.DayDogTypeID == 1 ? "рабочие" : "календарные",
        n.DaysDoc,
        n.DaysDog,
        n.DaysFact,
        n.DocNum,
        n.EnregID,
        n.FileName,
        n.IsNeedStrah,
        IsKM = n.tEnregistrement.IsKM,
        IsUA = n.tEnregistrement.IsUA,
        IsBU = n.tEnregistrement.IsBU,
        IsRC = n.tEnregistrement.IsRC,
        IsPM = n.tEnregistrement.IsPM,
        n.Remarque,
        n.Qty
      });
      return q;
    }

    public IEnumerable<dynamic> addEnregDet(List<tEnregistrementDet> data)
    {
      db.tEnregistrementDets.InsertAllOnSubmit(data);
      db.SubmitChanges();

      var q = db.tEnregistrementDets.Where(p => data.Select(n => n.ID).Contains(p.ID)).Select(n => new
      {
        id = n.ID,
        RecuDate = n.tEnregistrement.RecuDate,
        DateDoc = n.DaysDoc < 0 ? db.tWorkDates.Where(w => w.WorkDate <= n.tEnregistrement.RecuDate).OrderByDescending(w => w.WorkDate).Take(1 - n.DaysDoc ?? -1).Min(w => w.WorkDate as DateTime?) : db.tWorkDates.Where(w => w.WorkDate >= n.tEnregistrement.RecuDate).OrderBy(w => w.WorkDate).Take(1 + n.DaysDoc ?? 1).Max(w => w.WorkDate as DateTime?),
        DateDog = n.DayDogTypeID == 1 ? (n.DaysDog < 0 ? db.tWorkDates.Where(w => w.WorkDate <= n.tEnregistrement.RecuDate).OrderByDescending(w => w.WorkDate).Take(1 - n.DaysDog ?? -1).Min(w => w.WorkDate as DateTime?) : db.tWorkDates.Where(w => w.WorkDate >= n.tEnregistrement.RecuDate).OrderBy(w => w.WorkDate).Take(1 + n.DaysDog ?? 1).Max(w => w.WorkDate as DateTime?)) : n.tEnregistrement.RecuDate.Value.AddDays(n.DaysDog ?? 0),
        DateFact = n.DaysFact < 0 ? db.tWorkDates.Where(w => w.WorkDate <= n.tEnregistrement.RecuDate).OrderByDescending(w => w.WorkDate).Take(1 - n.DaysFact ?? -1).Min(w => w.WorkDate as DateTime?) : db.tWorkDates.Where(w => w.WorkDate >= n.tEnregistrement.RecuDate).OrderBy(w => w.WorkDate).Take(1 + n.DaysFact ?? 1).Max(w => w.WorkDate as DateTime?),
        n.DayDogTypeID,
        DayDogType = n.DayDogTypeID == 1 ? "рабочие" : "календарные",
        n.DaysDoc,
        n.DaysDog,
        n.DaysFact,
        n.DocNum,
        n.EnregID,
        n.FileName,
        n.IsNeedStrah,
        IsKM = n.tEnregistrement.IsKM,
        IsUA = n.tEnregistrement.IsUA,
        IsBU = n.tEnregistrement.IsBU,
        IsRC = n.tEnregistrement.IsRC,
        IsPM = n.tEnregistrement.IsPM,
        n.Remarque,
        n.Qty
      });
      return q;
    }

    public IEnumerable<dynamic> updEnregDet(List<tEnregistrementDet> data, string Name)
    {
      foreach (var e in data)
      {
        var q1 = db.tEnregistrementDets.Where(p => p.ID == e.ID).First();
        if (q1 != null)
        {
          q1.DaysDoc = e.DaysDoc;
          q1.DaysDog = e.DaysDog;
          q1.DaysFact = e.DaysFact;
          q1.DayDogTypeID = e.DayDogTypeID;
          q1.DocNum = e.DocNum;
          q1.FileName = e.FileName;
          q1.IsNeedStrah = e.IsNeedStrah;
          q1.Remarque = e.Remarque;
          q1.Qty = e.Qty;
          q1.InDateTime = DateTime.Now;

          db.SubmitChanges();
        }
      }

      var q = db.tEnregistrementDets.Where(p => data.Select(n => n.ID).Contains(p.ID)).Select(n => new
      {
        id = n.ID,
        RecuDate = n.tEnregistrement.RecuDate,
        DateDoc = n.DaysDoc < 0 ? db.tWorkDates.Where(w => w.WorkDate <= n.tEnregistrement.RecuDate).OrderByDescending(w => w.WorkDate).Take(1 - n.DaysDoc ?? -1).Min(w => w.WorkDate as DateTime?) : db.tWorkDates.Where(w => w.WorkDate >= n.tEnregistrement.RecuDate).OrderBy(w => w.WorkDate).Take(1 + n.DaysDoc ?? 1).Max(w => w.WorkDate as DateTime?),
        DateDog = n.DayDogTypeID == 1 ? (n.DaysDog < 0 ? db.tWorkDates.Where(w => w.WorkDate <= n.tEnregistrement.RecuDate).OrderByDescending(w => w.WorkDate).Take(1 - n.DaysDog ?? -1).Min(w => w.WorkDate as DateTime?) : db.tWorkDates.Where(w => w.WorkDate >= n.tEnregistrement.RecuDate).OrderBy(w => w.WorkDate).Take(1 + n.DaysDog ?? 1).Max(w => w.WorkDate as DateTime?)) : n.tEnregistrement.RecuDate.Value.AddDays(n.DaysDog ?? 0),
        DateFact = n.DaysFact < 0 ? db.tWorkDates.Where(w => w.WorkDate <= n.tEnregistrement.RecuDate).OrderByDescending(w => w.WorkDate).Take(1 - n.DaysFact ?? -1).Min(w => w.WorkDate as DateTime?) : db.tWorkDates.Where(w => w.WorkDate >= n.tEnregistrement.RecuDate).OrderBy(w => w.WorkDate).Take(1 + n.DaysFact ?? 1).Max(w => w.WorkDate as DateTime?),
        n.DayDogTypeID,
        DayDogType = n.DayDogTypeID == 1 ? "рабочие" : "календарные",
        n.DaysDoc,
        n.DaysDog,
        n.DaysFact,
        n.DocNum,
        n.EnregID,
        n.FileName,
        n.IsNeedStrah,
        IsKM = n.tEnregistrement.IsKM,
        IsUA = n.tEnregistrement.IsUA,
        IsBU = n.tEnregistrement.IsBU,
        IsRC = n.tEnregistrement.IsRC,
        IsPM = n.tEnregistrement.IsPM,
        n.Remarque,
        n.Qty
      });
      return q;
    }

    public bool delEnregDet(List<tEnregistrementDet> data)
    {
      var e = db.tEnregistrementDets.Where(p => data.Select(n => n.ID).Contains(p.ID));
      db.tEnregistrementDets.DeleteAllOnSubmit(e);
      db.SubmitChanges();
      return true;
    }

    public IEnumerable<dynamic> getMaxNumEnreg()
    {
      var rg = new Regex("(.*[^\\d])(\\d+)");
      var q = from t in
                (from tr in db.tEnregistrements
                 group tr by 1 into g
                 select new
                 {
                   Num = g.Max(t => t.Num),
                   Numero = g.Max(t => t.Numero)
                 }).ToList()
              select new { Num = (t.Num ?? 0) + 1, Numero = rg.Match(t.Numero).Success ? rg.Match(t.Numero).Groups[1].Value + (int.Parse(rg.Match(t.Numero).Groups[2].Value) + 1001).ToString().Right(3) : t.Numero };
      return q;
    }

    public bool confirmEnrDet(Guid id, numDep t, string Name, ref bool isSendMail, ref string dep)
    {
      var q1 = db.tEnregistrements.First(e => e.ID == db.tEnregistrementDets.First(p => p.ID == id).EnregID);
      var td = 0;
      if (findNextEnrDet(id, true) == t || ((t == numDep.IsKM && (q1.IsKM ?? false)) || (t == numDep.IsUA && (q1.IsUA ?? false)) || (t == numDep.IsBU && (q1.IsBU ?? false)) || (t == numDep.IsRC && (q1.IsRC ?? false)) || (t == numDep.IsRC && (q1.IsPM ?? false))) && q1 != null)
      {
        if (t == numDep.IsKM)
        {
          isSendMail = !(q1.IsKM ?? false);
          q1.IsKM = true;
          td = 1;
        }
        else if (t == numDep.IsUA)
        {
          isSendMail = !(q1.IsUA ?? false);
          q1.IsUA = true;
          td = 2;
        }
        else if (t == numDep.IsBU)
        {
          isSendMail = !(q1.IsBU ?? false);
          q1.IsBU = true;
          td = 3;
        }
        else if (t == numDep.IsRC)
        {
          isSendMail = !(q1.IsRC ?? false);
          q1.IsRC = true;
          td = 4;
        }
        else if (t == numDep.IsPM)
        {
          isSendMail = !(q1.IsPM ?? false);
          q1.IsPM = true;
          td = 5;
        }
        tEnregistrementDetLog q2 = new tEnregistrementDetLog { EnregID = q1.ID, EnregDetID = id, Login = Name, Action = 1, InDateTime = DateTime.Now, Type = td };
        db.tEnregistrementDetLogs.InsertAllOnSubmit(new List<tEnregistrementDetLog> { q2 });
        db.SubmitChanges();
        return true;
      }
      return false;
    }

    public bool canRefuseEnregDet(Guid id, numDep t)
    {
      return (findNextEnrDet(id, true) == t);
    }

    public bool refusalEnrDet(Guid id, numDep t, string Login, string Descr, ref int td)
    {
      //var q1 = db.tEnregistrementDets.First(p => p.ID == id);
      var q1 = db.tEnregistrements.First(e => e.ID == db.tEnregistrementDets.First(p => p.ID == id).EnregID);
      td = 0;
      if (findNextEnrDet(id, true) == t && q1 != null)
      {
        var tp = findNextEnrDet(id, false);
        if (tp == numDep.IsKM)
        {
          q1.IsKM = false;
          td = 1;
        }
        else if (tp == numDep.IsUA)
        {
          q1.IsUA = false;
          td = 2;
        }
        else if (tp == numDep.IsBU)
        {
          q1.IsBU = false;
          td = 3;
        }
        else if (tp == numDep.IsRC)
        {
          q1.IsRC = false;
          td = 4;
        }
        else if (tp == numDep.IsPM)
        {
          q1.IsPM = false;
          td = 5;
        }
        if (td == 0) td = (int)t;
        //if (td > 0)
        //{
        tEnregistrementDetLog q2 = new tEnregistrementDetLog { EnregID = q1.ID, EnregDetID = id, Login = Login, Action = 2, InDateTime = DateTime.Now, Type = td, Descr = Descr };
        db.tEnregistrementDetLogs.InsertAllOnSubmit(new List<tEnregistrementDetLog> { q2 });
        db.SubmitChanges();
        //}
        return true;
      }
      return false;
    }

    //public int findNextStep(Guid id)
    //{
    //  var q1 = db.tEnregistrementDets.Where(p => p.ID == id).First();
    //  var l = new Dictionary<int, bool?>();
    //  if (q1.tEnregistrement.DocTypeID != 1973)
    //  {
    //    l.Add(1, q1.IsKM);
    //    l.Add(2, q1.IsUA);
    //    l.Add(3, q1.IsBU);
    //    l.Add(4, q1.IsRC);
    //  }
    //  else
    //  {
    //    l.Add(1, q1.IsKM);
    //    l.Add(3, q1.IsBU);
    //    l.Add(4, q1.IsRC);
    //    l.Add(2, q1.IsUA);
    //  }
    //  try
    //  {
    //    var el = l.First(d => !(d.Value ?? false));
    //    return el.Key;
    //  }
    //  catch (InvalidOperationException)
    //  {
    //    return 5;
    //  }
    //}


    public numDep findNextEnrDet(Guid id, bool isNext)
    {
      //var q1 = db.tEnregistrementDets.Where(p => p.ID == id).First();
      var q1 = db.tEnregistrements.First(e => e.ID == db.tEnregistrementDets.First(p => p.ID == id).EnregID);
      var l = new List<clsDep>();
      if (q1.DocTypeID != 1973)
      {
        l.Add(new clsDep { dep = numDep.IsKM, value = q1.IsKM });
        if (q1.TreatyID == 7263)
          l.Add(new clsDep { dep = numDep.IsPM, value = q1.IsPM });
        l.Add(new clsDep { dep = numDep.IsUA, value = q1.IsUA });
        l.Add(new clsDep { dep = numDep.IsBU, value = q1.IsBU });
        l.Add(new clsDep { dep = numDep.IsRC, value = q1.IsRC });
      }
      else
      {
        l.Add(new clsDep { dep = numDep.IsKM, value = q1.IsKM });
        l.Add(new clsDep { dep = numDep.IsBU, value = q1.IsBU });
        l.Add(new clsDep { dep = numDep.IsRC, value = q1.IsRC });
        l.Add(new clsDep { dep = numDep.IsUA, value = q1.IsUA });
      }
      var t = isNext ? l.FindIndex(findNext) : l.FindLastIndex(findLast);
      return t == -1 ? numDep.IsNone : l[t].dep;
    }

    private static bool findNext(clsDep val)
    {
      return !(val.value ?? false);
    }

    private static bool findLast(clsDep val)
    {
      return (val.value ?? false);
    }

    public EnregDet getEnregDet(Guid id)
    {
      var q = db.tEnregistrementDets.Where(d => d.ID == id).Select(n => new EnregDet
      {
        DocNum = n.DocNum
      }).FirstOrDefault();
      return q;
    }

    private List<EMailED> getMailParam(Guid id, numDep tp)
    {
      var q = new List<EMailED>();

      if (tp == numDep.IsAll)
      {
        q = (from ed in db.tEnregistrementDets.Where(d => d.ID == id)
             select new EMailED
             {
               NameTo = "",
               EMailTo = string.Join(", ", db.tObjClassifiers.Where(o => new int[] { 2096, 2100, 1964, 43767 }.Contains(o.ParentID)).Select(o => o.Comment.Trim()).Distinct().ToArray()),
               FileName = ed.FileName,
               str = "информируем Вас о получении требования в отношении активов от клиента"
             }).ToList();
      }
      else if (tp == numDep.IsSS)
      {
        q = (from ed in db.tEnregistrementDets.Where(d => d.ID == id)
             select new EMailED
             {
               NameTo = "",
               EMailTo = string.Join(", ", db.tObjClassifiers.Where(o => new int[] { 1964 }.Contains(o.ParentID)).Select(o => o.Comment.Trim()).Distinct().ToArray()),
               FileName = ed.FileName,
               str = "информируем Вас о отклонении уведомления"
             }).ToList();
      }
      else if (tp == numDep.IsKM)
      {
        var q1 = (
          from ed in db.tEnregistrementDets.Where(d => d.ID == id)
          join e in db.tEnregistrements on ed.EnregID equals e.ID
          from oc in
            (from oc in db.tObjClassifiers.Where(o => o.ParentID == 1949 && System.Data.Linq.SqlClient.SqlMethods.Like(o.Comment, "%@am-uralsib.ru%"))
             join ocr in db.tObjClsRelations.Where(o => o.ObjectID == e.TreatyID && o.ObjType == 1631275800) on oc.ObjClassifierID equals ocr.ObjClassifierID
             select new { nb = oc.NameBrief, em = oc.Comment }
            ).Union(
            from oc in db.tObjClassifiers
            where e.DocTypeID == 1970 && (oc.ObjClassifierID == 68808 || oc.ObjClassifierID == 68651) && System.Data.Linq.SqlClient.SqlMethods.Like(oc.Comment, "%@%")
            select new { nb = oc.NameBrief, em = oc.Comment }
            )
          select new EMailED
          {
            NameTo = oc.nb,
            EMailTo = oc.em,
            FileName = ed.FileName,
            str = "подтвердить распоряжение"
          });
        q = q1.ToList();
      }
      else if (tp == numDep.IsUA)
      {

        q = (from ed in db.tEnregistrementDets.Where(d => d.ID == id)
             from oc in
               (from ocr in db.tObjClsRelations.Where(o => o.ObjClassifierID == 45790 && o.ObjType == 609335803)
                join u in db.tUsers.Where(o => (o.Mail ?? "") != "") on ocr.ObjectID equals u.UserID
                select new { nb = u.NameLast, em = u.Mail })
             select new EMailED
             {
               NameTo = oc.nb,
               EMailTo = oc.em,
               FileName = ed.FileName,
               str = "выполнить и подтвердить распоряжение"
             }).ToList();
      }
      else if (tp == numDep.IsBU)
      {
        q = (from ed in db.tEnregistrementDets.Where(d => d.ID == id)
             from oc in db.tObjClassifiers.Where(o => o.ParentID == 2096)
             select new EMailED
             {
               NameTo = oc.NameBrief.Trim(),
               EMailTo = oc.Comment.Trim(),
               FileName = ed.FileName,
               str = "проверить, получить разрешение СД и подтвердить возможность исполнения распоряжение"
             }).ToList();
      }
      else if (tp == numDep.IsRC)
      {
        q = (from ed in db.tEnregistrementDets.Where(d => d.ID == id)
             from oc in db.tObjClassifiers.Where(o => o.ParentID == 2100)
             select new EMailED
             {
               NameTo = oc.NameBrief.Trim(),
               EMailTo = oc.Comment.Trim(),
               FileName = ed.FileName,
               str = "вывести активы и подтвердить исполнение распоряжения"
             }).ToList();
      }
      else if (tp == numDep.IsPM)
      {
        q = (from ed in db.tEnregistrementDets.Where(d => d.ID == id)
             from oc in db.tObjClassifiers.Where(o => o.ParentID == 43767)
             select new EMailED
             {
               NameTo = oc.NameBrief.Trim(),
               EMailTo = oc.Comment.Trim(),
               FileName = ed.FileName,
               str = "подтвердить распоряжение"
             }).ToList();
      }
      return q;
    }

    public bool enrdetCourriel(Guid id, string host, enNext next, string descr)
    {
      var q1 = (from ed in db.tEnregistrementDets.Where(d => d.ID == id)
                join tr in db.tTreaties on ed.tEnregistrement.TreatyID equals tr.TreatyID
                join f in db.tFinancialInstitutions on tr.FinancialInstitutionID equals f.FinancialInstitutionID
                from dt in db.tObjClassifiers.Where(a => a.ObjClassifierID == ed.tEnregistrement.DocTypeID).DefaultIfEmpty()
                select new
                {
                  ClientName = (f.IsJuridicalPerson == 0 && f.AddressActual != "") ? (f.AddressActual + (f.NameLat == "" ? "" : (" " + f.NameLat))) : f.NameBrief.Trim(),
                  ClientCode = string.Join(", ", (from ocr in db.tObjClsRelations.Where(o => o.ObjClassifierID == 995 && o.ObjectID == tr.FinancialInstitutionID && o.ObjType == 741604640)
                                                  select ocr.Comment.Trim()).ToArray()),
                  trNameBrief = tr.Number.TrimEnd(),
                  tr.DateStart,
                  DTName = dt.Name,
                  Original = ed.tEnregistrement.Original,
                  ClnManager = db.uf_avgEnregCM(ed.tEnregistrement.ID),
                  StrategyUser = "Управляющий",
                  ed.DaysDoc,
                  ed.DaysDog,
                  DateDoc = ed.DaysDoc < 0 ? db.tWorkDates.Where(w => w.WorkDate <= ed.tEnregistrement.RecuDate).OrderByDescending(w => w.WorkDate).Take(1 - ed.DaysDoc ?? -1).Min(w => w.WorkDate as DateTime?) : db.tWorkDates.Where(w => w.WorkDate >= ed.tEnregistrement.RecuDate).OrderBy(w => w.WorkDate).Take(1 + ed.DaysDoc ?? 1).Max(w => w.WorkDate as DateTime?),
                  DateDog = ed.DayDogTypeID == 1 ? (ed.DaysDog < 0 ? db.tWorkDates.Where(w => w.WorkDate <= ed.tEnregistrement.RecuDate).OrderByDescending(w => w.WorkDate).Take(1 - ed.DaysDog ?? -1).Min(w => w.WorkDate as DateTime?) : db.tWorkDates.Where(w => w.WorkDate >= ed.tEnregistrement.RecuDate).OrderBy(w => w.WorkDate).Take(1 + ed.DaysDog ?? 1).Max(w => w.WorkDate as DateTime?)) : ed.tEnregistrement.RecuDate.Value.AddDays(ed.DaysDog ?? 0),
                  ed.tEnregistrement.Remarque,
                  ed.Qty,
                  DocTypeID = ed.tEnregistrement.DocTypeID,
                  ed.FileName
                }).FirstOrDefault();
      var q = new List<EMailED>();
      var t = numDep.IsNone;
      if (next == enNext.isAll)
      {
        t = numDep.IsAll;
        q = getMailParam(id, t);
      }
      else if (next == enNext.isNext)
      {
        t = findNextEnrDet(id, true);
        if (t != numDep.IsNone)
        {
          q = getMailParam(id, t);
        }
      }
      else if (next == enNext.isPrev)
      {
        t = findNextEnrDet(id, false);
        if (t != numDep.IsNone)
        {
          q = getMailParam(id, t);
        }
      }
      else if (next == enNext.isSS)
      {
        t = numDep.IsSS;
        q = getMailParam(id, t);
      }

      if (q != null && q1 != null)
      {
        foreach (var qd in q)
        {
          SmtpClient sc = new SmtpClient("m.am-uralsib.ru");
          sc.UseDefaultCredentials = true;
          MailMessage message = new MailMessage();
          message.From = new MailAddress("MiddleOffice <assets_msg@am-uralsib.ru>");
          if (!String.IsNullOrEmpty(qd.EMailTo))
          {
            message.To.Add((host.Contains("localhost") || host.Contains("10.158.32.10")) ? "GrishinAV@am-uralsib.ru" : qd.EMailTo);
            //if (qd.EMailTo == "IsaevaYuYu@am-uralsib.ru") message.Bcc.Add("GrishinAV@am-uralsib.ru");
          }
          var template = new MO.Areas.Code.Views.CorpEvents.SampleTemplate { q1 = q1, q2 = qd, id = id, host = host, t = t, descr = descr };
          message.Body = template.TransformText();
          //if (!String.IsNullOrWhiteSpace(qd.FileName))
          //{
          //  var prefix = @"\\fc.uralsibbank.ru\uralsib\MSK\COMMON\VOL1\ASSETS\CorpEvents\Enreg";
          //  var path = System.IO.Path.Combine(prefix, qd.FileName);
          //  if (File.Exists(path))
          //  {
          //    message.Attachments.Add(AttachmentHelper.CreateAttachment(path, System.IO.Path.GetFileName(path), TransferEncoding.Base64));
          //  }
          //}
          message.IsBodyHtml = true;
          message.Priority = MailPriority.High;
          message.Headers.Add("Importance", "High");
          message.IsBodyHtml = true;
          message.Subject = string.Format("{0} {1} {2}", t == numDep.IsAll ? "Информация о поручении клиента" : t == numDep.IsSS ? "Отклонение уведомления" : "Подтверждение выполнения поручения клиента", q1.ClientName, q1.DocTypeID == 1972 ? "(Вывод активов)" : q1.DocTypeID == 1973 ? "(Ввод активов)" : q1.DocTypeID == 116042 ? "Смена стратегии)" : "");
          sc.Send(message);
        }
        return true;
      }
      return false;
    }

    public bool enrdetRappel(string host, int type)
    {
      var q = (from e in db.tEnregistrements
               where !(e.IsKM ?? false) || !(e.IsUA ?? false) || !(e.IsBU ?? false) || !(e.IsRC ?? false)
               where !(e.Abolition ?? false)
               from ed in db.tEnregistrementDets.Where(p => p.EnregID == e.ID /*&& (!(p.IsKM ?? false) || !(p.IsUA ?? false) || !(p.IsBU ?? false) || !(p.IsRC ?? false))*/)
               join tr in db.tTreaties on e.TreatyID equals tr.TreatyID
               join f in db.tFinancialInstitutions on tr.FinancialInstitutionID equals f.FinancialInstitutionID
               from dt in db.tObjClassifiers.Where(a => a.ObjClassifierID == e.DocTypeID).DefaultIfEmpty()
               from stn in db.tObjClassifiers.Where(a => a.ObjClassifierID == e.StrategyID).DefaultIfEmpty()
               from st in
                 (from ap in db.tAccountPortfolios.Where(a => a.TreatyID == e.TreatyID && a.StartDate <= DateTime.Today && a.FinishDate > DateTime.Today)
                  join p in db.tPortfolios.Where(p => p.PortfolioTypeID == 8) on ap.PortfolioID equals p.PortfolioID
                  select new { Strategy = p.Name, ap.StartDate, ap.PortfolioID }).DefaultIfEmpty()
               from ap in db.tAccountPortfolios.Where(a => a.TreatyID == e.TreatyID && a.StartDate < st.StartDate && st.PortfolioID == 9937).OrderByDescending(a => a.StartDate).Join(db.tPortfolios.Where(p => p.PortfolioTypeID == 8), p => p.PortfolioID, p => p.PortfolioID, (p, pp) => new { StrategyP = pp.Name }).Take(1).DefaultIfEmpty()
               orderby e.Num
               select new
               {
                 ed.ID,
                 PID = e.ID,
                 Num = e.Num,
                 ClientName = f.NameBrief.Trim(),
                 Remarque = e.Remarque,
                 ed.Qty,
                 e.TreatyID,
                 e.RecuDate,
                 e.CancelDate,
                 DTName = dt.Name,
                 st.Strategy,
                 ap.StrategyP,
                 Portal = tr.FinancialInstitutionPortal.TrimEnd(),
                 DaysDoc = db.tWorkDates.Where(w => w.WorkDate >= DateTime.Today && w.WorkDate <= db.tWorkDates.Where(w1 => w1.WorkDate >= e.RecuDate).OrderBy(w1 => w1.WorkDate).Take(1 + ed.DaysDoc ?? 1).Max(w1 => w1.WorkDate as DateTime?)).Count(),
                 DateDoc = ed.DaysDoc < 0 ? db.tWorkDates.Where(w => w.WorkDate <= e.RecuDate).OrderByDescending(w => w.WorkDate).Take(1 - ed.DaysDoc ?? -1).Min(w => w.WorkDate as DateTime?) : db.tWorkDates.Where(w => w.WorkDate >= e.RecuDate).OrderBy(w => w.WorkDate).Take(1 + ed.DaysDoc ?? 1).Max(w => w.WorkDate as DateTime?),
                 IsKM = e.IsKM ?? false ? "+" : "-",
                 IsUA = e.IsUA ?? false ? "+" : "-",
                 IsBU = e.IsBU ?? false ? "+" : "-",
                 IsRC = e.IsRC ?? false ? "+" : "-",
                 DocTypeID = e.DocTypeID,
                 e.Abolition,
                 e.StrategyID,
                 newStrategy = stn.Name
               }).ToList()
               .Select(p => new
               {
                 p.ID,
                 p.PID,
                 p.Num,
                 p.ClientName,
                 p.Remarque,
                 p.Qty,
                 p.TreatyID,
                 p.RecuDate,
                 p.DaysDoc,
                 p.DateDoc,
                 p.CancelDate, //
                 p.Strategy, //
                 p.StrategyP, //
                 p.Portal, //
                 p.IsKM,
                 p.IsUA,
                 p.IsBU,
                 p.IsRC,
                 p.DocTypeID,
                 DTName = p.DTName + (p.DocTypeID == 116042 ? (" (" + p.newStrategy + ")") : ""), //
                 t = findNextEnrDet(p.ID, true)
               });

      var z = type == 1 ? Enum.GetValues(typeof(numDep)).Cast<numDep>().Where(p => p == numDep.IsKM) : type == 2 ? Enum.GetValues(typeof(numDep)).Cast<numDep>().Where(p => p == numDep.IsUA) : Enum.GetValues(typeof(numDep)).Cast<numDep>().Where(p => p == numDep.IsKM || p == numDep.IsUA || p == numDep.IsBU || p == numDep.IsRC || p == numDep.IsPM);
      foreach (numDep nd in z)
      {
        var q2 = nd == numDep.IsRC ? q.ToArray() : q.Where(a => a.t == nd).ToArray();

        var q1 = (nd == numDep.IsKM) ?

          from p in q2
          from oc in
            (
              from oc in db.tObjClassifiers.Where(o => o.ParentID == 1949 && System.Data.Linq.SqlClient.SqlMethods.Like(o.Comment, "%@am-uralsib.ru%"))
              join ocr in db.tObjClsRelations.Where(o => o.ObjectID == p.TreatyID && o.ObjType == 1631275800) on oc.ObjClassifierID equals ocr.ObjClassifierID
              select new { nb = oc.NameBrief, em = oc.Comment }
            ).Union(
              from oc in db.tObjClassifiers
              where p.DocTypeID == 1970 && (oc.ObjClassifierID == 68808 || oc.ObjClassifierID == 68651) && System.Data.Linq.SqlClient.SqlMethods.Like(oc.Comment, "%@am-uralsib.ru%")
              select new { nb = oc.NameBrief, em = oc.Comment }
            )
          group p by new { oc.nb, oc.em }

        : (nd == numDep.IsUA) ?

        from p in q2
        from oc in
          (from ocr in db.tObjClsRelations.Where(o => o.ObjClassifierID == 45790 && o.ObjType == 609335803)
           join u in db.tUsers.Where(o => (o.Mail ?? "") != "") on ocr.ObjectID equals u.UserID
           select new { nb = u.NameLast, em = u.Mail })
        group p by new { oc.nb, oc.em }

        //from p in q2
          //from oc in
          //  (from oc in db.tObjClassifiers.Where(o => o.ParentID == 2019 && (o.Comment ?? "") != "")
          //   join ocr in db.tObjClsRelations.Where(o => o.ObjectID == p.TreatyID && o.ObjType == 1631275800) on oc.ObjClassifierID equals ocr.ObjClassifierID
          //   select new { nb = oc.NameBrief, em = oc.Comment })
          //group p by new { oc.nb, oc.em }

        : (nd == numDep.IsBU) ?

        from p in q2
        from oc in db.tObjClassifiers.Where(o => o.ParentID == 2096)
        group p by new { nb = oc.NameBrief, em = oc.Comment }

        : (nd == numDep.IsRC) ?

        from p in q2
        from oc in db.tObjClassifiers.Where(o => o.ParentID == 2100)
        group p by new { nb = oc.NameBrief, em = oc.Comment }

        : (nd == numDep.IsPM) ?

        from p in q2
        from oc in db.tObjClassifiers.Where(o => o.ParentID == 43767)
        group p by new { nb = oc.NameBrief, em = oc.Comment }

        :

        from p in q2
        from oc in db.tObjClassifiers.Where(o => 1 == 2)
        group p by new { nb = oc.NameBrief, em = oc.Comment }

        ;

        foreach (var qg in q1.ToArray())
        {
          SmtpClient sc = new SmtpClient("m.am-uralsib.ru");
          sc.UseDefaultCredentials = true;
          try
          {
            MailMessage message = new MailMessage();
            message.From = new MailAddress("MiddleOffice <assets_msg@am-uralsib.ru>");
            if (!String.IsNullOrEmpty(qg.Key.em))
              message.To.Add((host.Contains("localhost") || host.Contains("10.158.32.10")) ? "GrishinAV@am-uralsib.ru" : qg.Key.em);
            //message.Bcc.Add("GrishinAV@am-uralsib.ru");
            var template = new MO.Areas.Code.Views.CorpEvents.Rappel { q1 = qg, em = qg.Key.em, nb = qg.Key.nb, host = host };
            message.Body = template.TransformText();
            message.IsBodyHtml = true;
            message.Priority = MailPriority.High;
            message.Headers.Add("Importance", "High");
            message.IsBodyHtml = true;
            message.Subject = "Напоминание о необходимости исполнения поручения клиента";
            sc.Send(message);
          }
          catch (Exception ex)
          {
            MailMessage message = new MailMessage();
            message.From = new MailAddress("MiddleOffice <assets_msg@am-uralsib.ru>");
            message.To.Add("GrishinAV@am-uralsib.ru");
            message.Body = ex.Message;
            message.IsBodyHtml = true;
            message.Priority = MailPriority.High;
            message.Headers.Add("Importance", "High");
            message.IsBodyHtml = true;
            message.Subject = "Ошибка в рассылке \"Напоминание о необходимости исполнения поручения клиента\"";
            sc.Send(message);
          }
        }
      }
      return false;
    }

    public IEnumerable<dynamic> getEnregDetLogList(int id)
    {
      var q = from e in db.tEnregistrementDetLogs.Where(p => p.EnregID == id)
              select new
              {
                id = e.ID,
                Act = e.Action == 1 ? "Подтверждение" : "Отмена",
                e.InDateTime,
                e.Login,
                Type = e.Type == 1 ? "КМ" : e.Type == 2 ? "УА" : e.Type == 3 ? "БУ" : e.Type == 4 ? "РЦ" : e.Type == 5 ? "РМ" : ""
              };
      return q;
    }

    public IEnumerable<dynamic> getDaysDog(int id, int? dtid)
    {
      var q = from e in db.tEnregistrements.Where(p => p.ID == id)
              from oc in db.tObjClassifiers.Where(o => o.ObjClassifierID == dtid)
              from ocr in db.tObjClsRelations.Where(o => o.ObjClassifierID == oc.ObjClassifierID && o.ObjectID == e.TreatyID && o.ObjType == 1631275800)
              select new { Days = ocr.Value, DayTypeID = ocr.Comment.Contains("рабочие") ? 1 : 0 };
      return q;
    }

    public IEnumerable<dynamic> getEnreg(int id)
    {
      var q = (from ed in db.tEnregistrementDets.Where(p => p.EnregID == id)
               join tr in db.tTreaties on ed.tEnregistrement.TreatyID equals tr.TreatyID
               join f in db.tFinancialInstitutions on tr.FinancialInstitutionID equals f.FinancialInstitutionID
               from dt in db.tObjClassifiers.Where(a => a.ObjClassifierID == ed.tEnregistrement.DocTypeID).DefaultIfEmpty()
               select new
               {
                 id = ed.ID,
                 Num = ed.tEnregistrement.Num,
                 ClientName = f.NameBrief.Trim(),
                 ClientCode = string.Join(", ", (from ocr in db.tObjClsRelations.Where(o => o.ObjClassifierID == 995 && o.ObjectID == tr.FinancialInstitutionID && o.ObjType == 741604640)
                                                 select ocr.Comment.Trim()).ToArray()),
                 trNameBrief = tr.Number.TrimEnd(),
                 tr.DateStart,
                 DTName = dt.Name,
                 Original = ed.tEnregistrement.Original,
                 ClnManager = db.uf_avgEnregCM(ed.tEnregistrement.ID),
                 ed.DaysDoc,
                 ed.DaysDog,
                 DateDoc = ed.DaysDoc < 0 ? db.tWorkDates.Where(w => w.WorkDate <= ed.tEnregistrement.RecuDate).OrderByDescending(w => w.WorkDate).Take(1 - ed.DaysDoc ?? -1).Min(w => w.WorkDate as DateTime?) : db.tWorkDates.Where(w => w.WorkDate >= ed.tEnregistrement.RecuDate).OrderBy(w => w.WorkDate).Take(1 + ed.DaysDoc ?? 1).Max(w => w.WorkDate as DateTime?),
                 DateDog = ed.DayDogTypeID == 1 ? (ed.DaysDog < 0 ? db.tWorkDates.Where(w => w.WorkDate <= ed.tEnregistrement.RecuDate).OrderByDescending(w => w.WorkDate).Take(1 - ed.DaysDog ?? -1).Min(w => w.WorkDate as DateTime?) : db.tWorkDates.Where(w => w.WorkDate >= ed.tEnregistrement.RecuDate).OrderBy(w => w.WorkDate).Take(1 + ed.DaysDog ?? 1).Max(w => w.WorkDate as DateTime?)) : ed.tEnregistrement.RecuDate.Value.AddDays(ed.DaysDog ?? 0),
                 ed.tEnregistrement.Remarque,
                 ed.Qty,
                 DocTypeID = ed.tEnregistrement.DocTypeID,
               }).ToList().Select(p => new
                {
                  p.id,
                  p.Num,
                  p.ClientName,
                  p.ClientCode,
                  p.trNameBrief,
                  p.DateStart,
                  p.DTName,
                  p.Original,
                  p.ClnManager,
                  //p.StrategyUser,
                  p.DaysDoc,
                  p.DaysDog,
                  p.DateDoc,
                  p.DateDog,
                  p.Remarque,
                  p.Qty,
                  p.DocTypeID,
                  t = findNextEnrDet(p.id, true)
                }.ToExpando());
      return q;
    }

    public bool toggleStatus(int id, string status, string Name)
    {
      bool? ret = true;
      var q1 = db.tEnregistrements.Where(p => p.ID == id).First();
      if (q1 != null)
      {
        if (status == "KM")
        {
          ret = q1.IsKM = !(q1.IsKM ?? false);
          tEnregistrementDetLog q2 = new tEnregistrementDetLog { EnregID = id, Login = Name, Action = q1.IsKM == true ? 1 : 2, InDateTime = DateTime.Now, Type = 1 };
          db.tEnregistrementDetLogs.InsertAllOnSubmit(new List<tEnregistrementDetLog> { q2 });
        }
        else if (status == "UA")
        {
          ret = q1.IsUA = !(q1.IsUA ?? false);
          tEnregistrementDetLog q2 = new tEnregistrementDetLog { EnregID = id, Login = Name, Action = q1.IsUA == true ? 1 : 2, InDateTime = DateTime.Now, Type = 2 };
          db.tEnregistrementDetLogs.InsertAllOnSubmit(new List<tEnregistrementDetLog> { q2 });
        }
        else if (status == "BU")
        {
          ret = q1.IsBU = !(q1.IsBU ?? false);
          tEnregistrementDetLog q2 = new tEnregistrementDetLog { EnregID = id, Login = Name, Action = q1.IsBU == true ? 1 : 2, InDateTime = DateTime.Now, Type = 3 };
          db.tEnregistrementDetLogs.InsertAllOnSubmit(new List<tEnregistrementDetLog> { q2 });
        }
        else if (status == "RC")
        {
          ret = q1.IsRC = !(q1.IsRC ?? false);
          tEnregistrementDetLog q2 = new tEnregistrementDetLog { EnregID = id, Login = Name, Action = q1.IsRC == true ? 1 : 2, InDateTime = DateTime.Now, Type = 4 };
          db.tEnregistrementDetLogs.InsertAllOnSubmit(new List<tEnregistrementDetLog> { q2 });
        }
        db.SubmitChanges();
      }
      return ret ?? true;
    }

    public DateTime? getCancelDate(int TreatyID, DateTime recuDate)
    {
      var q = db.tTreaties.FirstOrDefault(p => p.TreatyID == TreatyID);
      if (q != null)
      {
        return recuDate.AddDays(q.Number.Contains("ДУИ") ? 5 : 30);
      }
      return null;
    }

    public DateTime? getOutDate(int TreatyID, DateTime cancelDate)
    {
      var q = db.tTreaties.FirstOrDefault(p => p.TreatyID == TreatyID);
      if (q != null)
      {
        return db.tWorkDates.Where(w => w.WorkDate > cancelDate).OrderBy(w => w.WorkDate).Take(q.Number.Contains("ДУИ") ? 17 : 23).Max(w => w.WorkDate as DateTime?);
      }
      return null;
    }
  }
}