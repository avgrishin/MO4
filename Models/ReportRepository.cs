using MO.Helpers;
using MO.ViewModels;
using MvcContrib.Sorting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.SqlClient;
using System.Data.Objects.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MO.Models
{
  public interface IReportRepository
  {
    int Test();
    bool CheckFinInst(int FinInstID, bool isBank, bool isPIF);
    IQueryable<tSecurity> GetFunds();
    IQueryable<tExchangeIndex> GetIndexes();
    IEnumerable<dynamic> GetStrategies();
    dynamic GetFinInst(int FinInstID);
    IQueryable<dynamic> GetFinInsts(string q, int limit, bool isBank, bool isPIF);
    List<up_avgRepClientResult0> GetRepClient0(DateTime? DateB, DateTime? DateE, int? finInstID, int? fundID, int? indexID, int? strategyID = null);
    List<up_avgRepClientResult1> GetRepClient1(DateTime? DateB, DateTime? DateE, int? finInstID, int? fundID, int? indexID, int? strategyID = null);
    up_avgRepClientResult2 GetRepClient2(DateTime? DateB, DateTime? DateE, int? finInstID, int? fundID, int? indexID, int? strategyID = null);
    List<up_avgRepClientResult3> GetRepClient3(DateTime? DateB, DateTime? DateE, int? finInstID, int? fundID, int? indexID, int? strategyID = null);
    List<up_avgRepClientResult6> GetRepClient6(DateTime? DateB, DateTime? DateE, int? finInstID, int? fundID, int? indexID, int? strategyID = null);
    List<up_avgRepClientResult7> GetRepClient7(DateTime? DateB, DateTime? DateE, int? finInstID, int? fundID, int? indexID, int? strategyID = null);
    List<up_avgRepClientResult20> GetRepClient20(DateTime? DateB, DateTime? DateE, int? finInstID, int? fundID, int? indexID, int? strategyID = null);
    List<up_avgRepClientResult21> GetRepClient21(DateTime? DateB, DateTime? DateE, int? finInstID, int? fundID, int? indexID, int? strategyID = null);
    List<up_avgRepClientResult22> GetRepClient22(DateTime? DateB, DateTime? DateE, int? finInstID, int? fundID, int? indexID, int? strategyID = null);
    List<up_avgRepClientResult23> GetRepClient23(DateTime? DateB, DateTime? DateE, int? finInstID, int? fundID, int? indexID, int? strategyID = null);
    List<up_avgRepClientResult25> GetRepClient25(DateTime? DateB, DateTime? DateE, int? finInstID, int? fundID, int? indexID, int? strategyID = null);
    List<up_avgRepClientResult24> GetRepClient24(DateTime? DateB, DateTime? DateE, int? finInstID, int? fundID, int? indexID, int? strategyID = null);
    List<up_avgRepClientResult26> GetRepClient26(DateTime? DateB, DateTime? DateE, int? finInstID, int? fundID, int? indexID, int? strategyID = null);
    bool IsShowAssets(int FinInstID, bool isBank);
    IEnumerable<dynamic> GetSecChart();
    ISingleResult<up_avgRepDUCBBResult> up_avgRepDUCBB(DateTime? DateB, DateTime? DateE);
    dynamic GetFinRepData(int id);
    IEnumerable<dynamic> GetTreaties(string q, int limit);
    IEnumerable<dynamic> GetObjClsByParent(int id);
    IEnumerable<dynamic> GetFinRep();
    void SetFinRepData(int id, int? ocp, int? oc);
    //IEnumerable<dynamic> GetOrderControl(string sort, string dir, DateTime? db, DateTime? de, bool? IsDatePay, string filter);
    IEnumerable<dynamic> GetObjClsByParentID(int ParentID);
    IEnumerable<dynamic> GetPlatList();
    IEnumerable<ObjClsNode> GetObjClsNode(int ParentID);
    IEnumerable<dynamic> GetOrderPayment(string sort, string dir, DateTime? db, DateTime? de, int? DateType, Boolean? NotPayed, Boolean? Reserved, int? TypeID, string filter);
    IEnumerable<dynamic> GetOrderPayment(DateTime? dateb, DateTime? datee, int? DateType);
    IEnumerable<dynamic> OrderPaymentCreate(List<tOrdPayment1> data);
    IEnumerable<dynamic> OrderPaymentUpdate(List<tOrdPayment> data, bool IsAdmin);
    bool OrderPaymentDel(List<tOrdPayment> data);
    IEnumerable<dynamic> GetPaymFunds();
    IEnumerable<dynamic> GetOrderPaymDet(int? id);
    IEnumerable<dynamic> OrderPaymDetCreate(List<tOrdPaymentDet> data);
    IEnumerable<dynamic> OrderPaymDetUpdate(List<tOrdPaymentDet> data, bool IsAdmin);
    bool OrderPaymDetDel(List<tOrdPaymentDet> data);
    dynamic GetOrderPaym(int? id);
    IEnumerable<dynamic> GetOrderPaymDetF(int? id);
    ISingleResult<up_avgRepFinDataResult> getOrdPaymData(int? id, string item);
    IEnumerable<dynamic> GetPoluch1(string query);
    IEnumerable<dynamic> GetSign(string query);

    int NextStateID(int id, int action, int StateIdCur);
    bool confirmOrdPaym(int id, string Login, int action, int StateIdCur, string descr, DateTime? DatePay);
    bool ordpaymCourriel(int id, string url, string host, string descr, bool refuse = false);
    IEnumerable<dynamic> getOrdPaymLogList(int id);
    int? getOrdPaymStateID(int id);
    bool OrdPaymUpdFilePP(int id, string FileNamePP);
    bool ordPaymRappel(string host, UrlHelper Url);
    bool ordPaymRappel2(string host, UrlHelper Url);
    IEnumerable<dynamic> GetDogovor(int id);

    IEnumerable<dynamic> GetOrderPaymConf(int? id);
    IEnumerable<dynamic> OrderPaymConfCreate(List<tOrdPaymentConf> data);
    IEnumerable<dynamic> OrderPaymConfUpdate(List<tOrdPaymentConf> data);
    bool OrderPaymConfDel(List<tOrdPaymentConf> data);

    dynamic CheckOrdPaymByPlatNumb(string Receiver, string DocNumb);
    dynamic CheckOrdPaymByPlatNumb1(int ReceiverID, string DocNumb);
    IEnumerable<dynamic> ChargesList(string sort, string dir, int? y, int? m, int? pfp, bool? np);
    IEnumerable<dynamic> ChargesUpdate(List<tCharge> data);
    void ImportOrdControl(int? y, int? q);
    IEnumerable<dynamic> GetFPS(string sort, string dir, string filter);
    IEnumerable<dynamic> FPSCreate(List<tFinPlanSchema> data);
    IEnumerable<dynamic> FPSUpdate(List<tFinPlanSchema> data);
    bool FPSDel(List<tFinPlanSchema> data);
    IEnumerable<dynamic> FinPlanList(string sort, string dir, int? y, int? q);
    IEnumerable<dynamic> FinPlanUpdate(List<tFinPlan> data);
    IEnumerable<dynamic> FinActList(string sort, string dir, int? y, int? q);
    IEnumerable<dynamic> FinActUpdate(List<tFinActive> data);
    IEnumerable<dynamic> FinSalesList(string sort, string dir, int? y, int? q);
    IEnumerable<dynamic> FinSalesUpdate(List<tFinSale> data);
    IEnumerable<dynamic> FinProfitList(string sort, string dir, int? y, int? q);
    IEnumerable<dynamic> FinProfitUpdate(List<tFinProfit> data);

    IEnumerable<dynamic> GetFDS(string sort, string dir, string filter);
    IEnumerable<dynamic> FDSCreate(List<tFinDohodSchema> data);
    IEnumerable<dynamic> FDSUpdate(List<tFinDohodSchema> data);
    bool FDSDel(List<tFinDohodSchema> data);
    IEnumerable<dynamic> FinDohodList(string sort, string dir, int? y, int? q);
    IEnumerable<dynamic> FinDohodUpdate(List<tFinDohod> data);

    IEnumerable<dynamic> GetValRates();
    IEnumerable<dynamic> GetCourses();
    IEnumerable<dynamic> GetPifRates();
    IEnumerable<dynamic> GetPifers(string q);
    IEnumerable<dynamic> GetPiferRest(int id, DateTime? d1, DateTime? d2);
    object GetPiferYield(int id, DateTime? d1, DateTime? d2);
    object GetPiferFondYield(int id, int secId, DateTime? d1, DateTime? d2);
    IEnumerable<dynamic> GetPiferOrders(int id, int secId);
    ISingleResult<up_avgGetPiferGraphResult> GetPiferGraph(int id);
    ISingleResult<up_avgGetPiferGraph3Result> GetPiferGraph3(int id, DateTime? d1, DateTime? d2);

    IQueryable<dynamic> GetKladr(int? id, string q, int limit, string Code1);
    IQueryable<dynamic> GetKladr1(int? id, string Code1);

    IEnumerable<dynamic> FinActProfitList(string sort, string dir, int? y);
    IEnumerable<dynamic> FinActProfitUpdate(List<tFinActProfit> data);

    IEnumerable<dynamic> GetPIFerRest(DateTime? d, int? FinInstID, string clName);
    IEnumerable<dynamic> GetPIFList();

    IEnumerable<dynamic> FinInOutComeList(string sort, string dir, int? an);
    IEnumerable<dynamic> FinInOutComeUpdate(List<tFinInOutCome> data);

    IEnumerable<dynamic> FinPartList(string sort, string dir, DateTime? dt);
    IEnumerable<dynamic> FinPartUpdate(List<FinPartViewModel> data);

    IEnumerable<dynamic> GetCbBPifer();
    IEnumerable<dynamic> GetPiferData(int? id, DateTime? d1, DateTime? d2);

    ISingleResult<up_avgGetPIFRestsResult> GetPIFRests(DateTime? d, int? FinInstID, string clName);
    ISingleResult<up_avgGetPIFRestStatResult> GetPIFRestStat(DateTime? d);

    bool MissionCreate(Mission m, string UserName);

    ISingleResult<up_avgRepPIFOrdersResult> RepPIFOrders(DateTime? d);

    IEnumerable<dynamic> GetChartData();

    ISingleResult<up_avgRepPIFOrderStatResult> RepPIFOrderStat(DateTime? d1, DateTime? d2);
    ISingleResult<up_avgRepPIFOrderOutStatResult> RepPIFOrderOutStat(DateTime? d1, DateTime? d2);

    IEnumerable<dynamic> GetContragentList(string sort, string dir, bool? isNotArchive, string name);
    IEnumerable<dynamic> ContragentCreate(List<tContragent> data);
    IEnumerable<dynamic> ContragentUpdate(List<tContragent> data);
    bool ContragentDel(List<tContragent> data);
    dynamic AddContragent(string name);
    IEnumerable<dynamic> GetCountries();

    IEnumerable<dynamic> GetContragentDogList(int Id);
    IEnumerable<dynamic> ContragentDogCreate(List<tContragentDog> data);
    IEnumerable<dynamic> ContragentDogUpdate(List<tContragentDog> data);
    bool ContragentDogDel(List<tContragentDog> data);

    dynamic GetContragent(int Id);
    IQueryable<dynamic> GetContragents(string q, int limit);

    IEnumerable<dynamic> GetClientIKList(string sort, string dir);
    IEnumerable<dynamic> addClientIK(List<taNab> data);
    IEnumerable<dynamic> updClientIK(List<taNab> data);
    bool delClientIK(List<taNab> data);
    IEnumerable<dynamic> GetIKList();
    IEnumerable<dynamic> GetClIKList();

    bool ImpCharges(HttpPostedFileBase file, int? mois, int? an);
    ISingleResult<up_avgRepPIFDepStatResult> RepPIFDepStat(DateTime? d1, DateTime? d2);
    ISingleResult<up_avgRepPIFDepStat2Result> RepPIFDepStat2(DateTime? d1, DateTime? d2);
    ISingleResult<up_avgRepBankPIFIncomeResult> RepBankPIFIncome(DateTime? d1, DateTime? d2);
    ISingleResult<up_avgRepBankPIFIncomeLKResult> RepBankPIFIncomeLK(DateTime? d1, DateTime? d2);
    IEnumerable<up_avgRepPIFStatResult> RepPIFStat(DateTime? d1, DateTime? d2);
    IEnumerable<up_avgRepPIFStat1Result> RepPIFStat1(DateTime? d1, DateTime? d2);
    ISingleResult<up_avgRepFin5Result> RepFin5(DateTime? de, bool? withDog, string FinInstIDs);
    ISingleResult<up_avgRepFin6Result> RepFin6(DateTime? de, bool? withDog, string FinInstIDs);
    ISingleResult<up_avgRepUralsibCommisResult> RepUralsibCommis(DateTime d1, DateTime d2);
    ISingleResult<up_avgRepUralsibCommisLKResult> RepUralsibCommisLK(DateTime d1, DateTime d2);
    ISingleResult<up_avgRepUralsibInOutResult> RepUralsibInOut(DateTime d1, DateTime d2);

    IEnumerable<dynamic> getRepKM1(DateTime? d1, DateTime? d2);
    IEnumerable<dynamic> getRepDURests(DateTime? d);
    IEnumerable<dynamic> getRepDUBank(DateTime? d1, DateTime? d2, int Type);
    IEnumerable<dynamic> getRepDUInOut(DateTime? d1, DateTime? d2, int Type);
    bool AddUpdSurvey(SurveyViewModel data, string UserName);
    decimal GetValRate(int Id, decimal q);
    DateTime GetLastWorkDate();
    IEnumerable<dynamic> getPifBankBuy(DateTime? d1, DateTime? d2);
    IEnumerable<dynamic> getPifRepmt(DateTime? d1, DateTime? d2);

    List<up_avgRep4BCSResult1> Rep4BCS1(DateTime? DateB, DateTime? DateE);
    List<up_avgRep4BCSResult2> Rep4BCS2(DateTime? DateE);
    List<up_avgRep4BCSResult3> Rep4BCS3(DateTime? DateB, DateTime? DateE);
    List<up_avgRep4BCSResult4> Rep4BCS4(DateTime? DateB, DateTime? DateE);
    List<up_avgRep4BCSResult5> Rep4BCS5(DateTime? DateB, DateTime? DateE);
    ISingleResult<up_avgRep4BCS2Result> Rep4BCS_2(DateTime? DateB, DateTime? DateE);

    IEnumerable<dynamic> GetBotPifRates();
    IEnumerable<dynamic> GetBotPifYield();
    IEnumerable<dynamic> GetBotPifBranch();
    IEnumerable<dynamic> GetBotDuBranch();
    DateTime? GetLastPifDate();
    int? GetFinInstID(string Brief);
    IEnumerable<dynamic> StrategyChartData(int? strategyID);
  }

  public class ReportRepository : IReportRepository
  {
    private MiddleOfficeDataContext db = new MiddleOfficeDataContext() { CommandTimeout = 600 };

    int IReportRepository.Test()
    {
      return 1;
    }

    public bool CheckFinInst(int FinInstID, bool isBank, bool isPIF)
    {
      var q = isBank || isPIF ?
        (from ocr in db.tObjClsRelations
         where ocr.ObjClassifierID == 175 && ocr.ObjType == 1631275800 && !isPIF
         join t in db.tTreaties.Where(t => t.FinancialInstitutionID == FinInstID) on ocr.ObjectID equals t.TreatyID
         select t.FinancialInstitutionID)
       .Union
       (from ocr in db.tObjClsRelations
        where ocr.ObjClassifierID == (isPIF ? 791 : 430) && ocr.ObjType == 741604640 && ocr.ObjectID == FinInstID
        select ocr.ObjectID)
      :
        from t in db.tTreaties.Where(f => f.FinancialInstitutionID == FinInstID)
        join ttt in db.tTreatyTreatyTypes.Where(ttt => new int[] { 1, 2, 3 }.Contains(ttt.TreatyTypeID)) on t.TreatyID equals ttt.TreatyID
        select t.FinancialInstitutionID;
      return q.Count() > 0;
    }

    public IQueryable<tSecurity> GetFunds()
    {
      return from p in db.GetTable<tSecurity>()
             where p.SecurityID == 39191 || p.SecurityID == 39192
             select p;
    }

    public IQueryable<tExchangeIndex> GetIndexes()
    {
      return from p in db.GetTable<tExchangeIndex>()
             orderby p.ExchangeIndexID
             select p;
    }

    public IEnumerable<dynamic> GetStrategies()
    {
      return from p in db.GetTable<tPortfolio>()
             where p.PortfolioTypeID == 8 && p.DateFinish > DateTime.Today.AddMonths(-1)
             orderby p.PortfolioID
             select p;
    }

    public IQueryable<dynamic> GetFinInsts(string q, int limit, bool isBank, bool isPIF)
    {
      return (from f in db.tFinancialInstitutions.Where(fi => fi.NameBrief.Contains(q) || fi.Name.Contains(q))
              join c in
                (isBank || isPIF ? (from ocr in db.tObjClsRelations
                                    where ocr.ObjClassifierID == 175 && ocr.ObjType == 1631275800 && !isPIF
                                    join t in db.tTreaties on ocr.ObjectID equals t.TreatyID
                                    select new { t.FinancialInstitutionID })
                          .Union(
                           from ocr in db.tObjClsRelations
                           where ocr.ObjClassifierID == (isPIF ? 791 : 430) && ocr.ObjType == 741604640
                           select new { FinancialInstitutionID = ocr.ObjectID })

                        : (from t in db.tTreaties
                           join ttt in db.tTreatyTreatyTypes.Where(ttt => new int[] { 1, 2, 3 }.Contains(ttt.TreatyTypeID)) on t.TreatyID equals ttt.TreatyID
                           select new { t.FinancialInstitutionID }).Distinct())
              on f.FinancialInstitutionID equals c.FinancialInstitutionID
              orderby f.NameBrief
              select new { id = f.FinancialInstitutionID, name = f.Name.Trim(), brief = f.NameBrief.Trim() }).Take(limit);
    }

    public dynamic GetFinInst(int FinInstID)
    {
      var q = (from f in db.tFinancialInstitutions
               where f.FinancialInstitutionID == FinInstID
               select new cFinInst
               {
                 id = f.FinancialInstitutionID,
                 brief = f.NameBrief.Trim(),
                 name = f.Name.Trim()
               })
             .FirstOrDefault();
      return q; //== null ? new cFinInst() { brief = "Клиент не найден" } : q;
    }

    public List<up_avgRepClientResult0> GetRepClient0(DateTime? DateB, DateTime? DateE, int? finInstID, int? fundID, int? indexID, int? strategyID = null)
    {
      var q = from p in db.up_avgRepClient(DateB, DateE, finInstID, fundID, 0, indexID, strategyID, null, 1, null, null).GetResult<up_avgRepClientResult0>()
              select p;
      return q.ToList();
    }

    public up_avgRepClientResult2 GetRepClient2(DateTime? DateB, DateTime? DateE, int? finInstID, int? fundID, int? indexID, int? strategyID = null)
    {

      return (from p in db.up_avgRepClient(DateB, DateE, finInstID, fundID, 2, indexID, strategyID, null, 1, null, null).GetResult<up_avgRepClientResult2>()
              select p).FirstOrDefault();
    }

    public List<up_avgRepClientResult3> GetRepClient3(DateTime? DateB, DateTime? DateE, int? finInstID, int? fundID, int? indexID, int? strategyID = null)
    {
      return db.up_avgRepClient(DateB, DateE, finInstID, fundID, 3, indexID, strategyID, null, 1, null, null).GetResult<up_avgRepClientResult3>().ToList();
    }

    public List<up_avgRepClientResult6> GetRepClient6(DateTime? DateB, DateTime? DateE, int? finInstID, int? fundID, int? indexID, int? strategyID = null)
    {
      return db.up_avgRepClient(DateB, DateE, finInstID, fundID, 6, indexID, strategyID, null, 1, null, null).GetResult<up_avgRepClientResult6>().ToList();
    }

    public List<up_avgRepClientResult1> GetRepClient1(DateTime? DateB, DateTime? DateE, int? finInstID, int? fundID, int? indexID, int? strategyID = null)
    {
      return (from p in db.up_avgRepClient(DateB, DateE, finInstID, fundID, 1, indexID, strategyID, null, 1, null, null).GetResult<up_avgRepClientResult1>()
              select p).ToList();
    }

    public List<up_avgRepClientResult7> GetRepClient7(DateTime? DateB, DateTime? DateE, int? finInstID, int? fundID, int? indexID, int? strategyID = null)
    {
      return (from p in db.up_avgRepClient(DateB, DateE, finInstID, fundID, 7, indexID, strategyID, null, 1, null, null).GetResult<up_avgRepClientResult7>()
              select p).ToList();
    }

    public List<up_avgRepClientResult20> GetRepClient20(DateTime? DateB, DateTime? DateE, int? finInstID, int? fundID, int? indexID, int? strategyID = null)
    {
      return (from p in db.up_avgRepClient(DateB, DateE, finInstID, fundID, 20, indexID, strategyID, null, 1, null, null).GetResult<up_avgRepClientResult20>()
              select p).ToList();
    }

    public List<up_avgRepClientResult21> GetRepClient21(DateTime? DateB, DateTime? DateE, int? finInstID, int? fundID, int? indexID, int? strategyID = null)
    {
      return (from p in db.up_avgRepClient(DateB, DateE, finInstID, fundID, 21, indexID, strategyID, null, 1, null, null).GetResult<up_avgRepClientResult21>()
              select p).ToList();
    }

    public List<up_avgRepClientResult22> GetRepClient22(DateTime? DateB, DateTime? DateE, int? finInstID, int? fundID, int? indexID, int? strategyID = null)
    {
      return (from p in db.up_avgRepClient(DateB, DateE, finInstID, fundID, 22, indexID, strategyID, null, 1, null, null).GetResult<up_avgRepClientResult22>()
              select p).ToList();
    }

    public List<up_avgRepClientResult23> GetRepClient23(DateTime? DateB, DateTime? DateE, int? finInstID, int? fundID, int? indexID, int? strategyID = null)
    {
      return (from p in db.up_avgRepClient(DateB, DateE, finInstID, fundID, 23, indexID, strategyID, null, 1, null, null).GetResult<up_avgRepClientResult23>()
              select p).ToList();

    }
    public List<up_avgRepClientResult25> GetRepClient25(DateTime? DateB, DateTime? DateE, int? finInstID, int? fundID, int? indexID, int? strategyID = null)
    {
      return (from p in db.up_avgRepClient(DateB, DateE, finInstID, fundID, 25, indexID, strategyID, null, 1, null, null).GetResult<up_avgRepClientResult25>()
              select p).ToList();
    }

    public List<up_avgRepClientResult24> GetRepClient24(DateTime? DateB, DateTime? DateE, int? finInstID, int? fundID, int? indexID, int? strategyID = null)
    {
      return (from p in db.up_avgRepClient(DateB, DateE, finInstID, fundID, 24, indexID, strategyID, null, 1, null, null).GetResult<up_avgRepClientResult24>()
              select p).ToList();
    }

    public List<up_avgRepClientResult26> GetRepClient26(DateTime? DateB, DateTime? DateE, int? finInstID, int? fundID, int? indexID, int? strategyID = null)
    {
      return (from p in db.up_avgRepClient(DateB, DateE, finInstID, fundID, 26, indexID, strategyID, null, 1, null, null).GetResult<up_avgRepClientResult26>()
              select p).ToList();
    }

    public bool IsShowAssets(int FinInstID, bool isBank)
    {
      return !isBank || (isBank &&
        (from ocr in db.tObjClsRelations
         where ocr.ObjClassifierID == 430 && ocr.ObjType == 741604640 && ocr.ObjectID == FinInstID
         select ocr.ObjectID).Count() == 0);
    }

    public IEnumerable<dynamic> GetSecChart()
    {
      var dateB = new DateTime(2008, 1, 1);
      var q1 = from es in db.tExchangeSecurities.Where(pp => new int[] { 495, 1970, 867, 96332, 859, 1971, 4, 849, 4309, 4987, 4535, 5016 }.Contains(pp.ExchangeSecurityID))
               from wd in db.tWorkDates.Where(pp => pp.WorkDate < DateTime.Today).OrderByDescending(pp => pp.WorkDate).Take(1)
               from re in db.tExchangePrices.Where(pp => pp.ExchangeSecurityID == es.ExchangeSecurityID && pp.RateDate == wd.WorkDate).DefaultIfEmpty()
               from rmax in db.tExchangePrices.Where(pp => pp.ExchangeSecurityID == es.ExchangeSecurityID && pp.RateDate >= dateB && pp.RateDate <= wd.WorkDate && pp.Close != null).OrderByDescending(pp => pp.Close).Take(1).DefaultIfEmpty()
               from rmin in db.tExchangePrices.Where(pp => pp.ExchangeSecurityID == es.ExchangeSecurityID && pp.RateDate >= dateB && pp.RateDate <= wd.WorkDate && pp.Close != null).OrderBy(pp => pp.Close).Take(1).DefaultIfEmpty()
               select new { sec = es.Brief, vcur = (re.Close - rmin.Close) * 100 / rmax.Close, vmax = (rmax.Close - re.Close) * 100 / rmax.Close, vmin = rmin.Close * 100 / rmax.Close, scur = re.Close, smin = rmin.Close, smax = rmax.Close };
      return q1;
    }

    public ISingleResult<up_avgRepDUCBBResult> up_avgRepDUCBB(DateTime? DateB, DateTime? DateE)
    {
      return db.up_avgRepDUCBB(DateB, DateE);
    }

    public IEnumerable<dynamic> GetTreaties(string q, int limit)
    {
      return (from t in db.tTreaties
              join f in db.tFinancialInstitutions on t.FinancialInstitutionID equals f.FinancialInstitutionID
              where t.NameBrief.Contains(q) || f.NameBrief.Contains(q)
              orderby t.NameBrief
              select new cFinInst { id = t.TreatyID, name = f.NameBrief.Trim(), brief = t.NameBrief.Trim() }).Take(limit);
    }

    public IEnumerable<dynamic> GetObjClsByParent(int id)
    {
      return (from oc in db.tObjClassifiers
              where oc.ParentID == id
              select new
              {
                id = oc.ObjClassifierID.ToString(),
                name = oc.Name
              });
    }

    public IEnumerable<dynamic> GetFinRep()
    {
      return (from l in db.taLibs
              join oc in db.tObjClassifiers on l.LInt1 equals oc.ObjClassifierID
              join ocp in db.tObjClassifiers on oc.ParentID equals ocp.ObjClassifierID
              join ocp1 in db.tObjClassifiers on ocp.ParentID equals ocp1.ObjClassifierID
              where l.LParent == 183864
              orderby (ocp1.ObjClassifierID != 873 ? ocp1.Name + "/" : "") + ocp.Name + "/" + oc.Name
              select new
              {
                ObjClassifierID = oc.ObjClassifierID,
                Name = (ocp1.ObjClassifierID != 873 ? ocp1.Name + "/" : "") + ocp.Name + "/" + oc.Name,
                NameBrief = oc.NameBrief
              });
    }

    public dynamic GetFinRepData(int id)
    {
      var q = (from l in db.taLibs
               join oc in db.tObjClassifiers on l.LInt1 equals oc.ObjClassifierID
               join ocr in
                 (from p in db.tObjClsRelations.Where(p => p.ObjectID == id)
                  join oc1 in db.tObjClassifiers on p.ObjClassifierID equals oc1.ObjClassifierID
                  select new { p.ObjClassifierID, oc1.ParentID }) on oc.ObjClassifierID equals ocr.ParentID into jocs1
               from joc1 in jocs1.DefaultIfEmpty()
               select new
               {
                 id = oc.ObjClassifierID,
                 value = ((int?)joc1.ObjClassifierID).ToString() ?? ""
               });
      return q;
    }

    public void SetFinRepData(int id, int? ocp, int? oc)
    {
      db.up_avgSetFinRepData(id, ocp, oc);
    }


    public IEnumerable<dynamic> GetPlatList()
    {
      return from o in db.tObjClsRelations
             join f in db.tFinancialInstitutions on o.ObjectID equals f.FinancialInstitutionID
             where o.ObjClassifierID == 1003
             select new
             {
               Value = f.FinancialInstitutionID,
               Text = o.Comment ?? f.NameBrief
             };
    }

    public IEnumerable<dynamic> GetObjClsByParentID(int ParentID)
    {
      return from o in db.tObjClassifiers
             where o.ParentID == ParentID
             select new
             {
               Value = o.ObjClassifierID,
               Text = o.Name,
               o.NameBrief,
               o.Comment
             };
    }

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

    public IEnumerable<dynamic> GetOrderPayment(string sort, string dir, DateTime? dateb, DateTime? datee, int? DateType, Boolean? NotPayed, Boolean? Reserved, int? TypeID, string filter)
    {
      var q1 = db.tOrdPayments as IQueryable<tOrdPayment>;
      if (DateType == 0)
      {
        if (dateb.HasValue)
        {
          q1 = q1.Where(a => a.DateCreate >= dateb);
        }
        if (datee.HasValue)
        {
          q1 = q1.Where(a => a.DateCreate <= datee);
        }
      }
      else if (DateType == 1)
      {
        if (dateb.HasValue)
        {
          q1 = q1.Where(a => a.DateDoc >= dateb);
        }
        if (datee.HasValue)
        {
          q1 = q1.Where(a => a.DateDoc <= datee);
        }
      }
      else if (DateType == 2)
      {
        if (dateb.HasValue)
        {
          q1 = q1.Where(a => a.DatePay >= dateb);
        }
        if (datee.HasValue)
        {
          q1 = q1.Where(a => a.DatePay <= datee);
        }
      }
      else if (DateType == 3)
      {
        if (dateb.HasValue)
        {
          q1 = q1.Where(a => a.DateReg >= dateb);
        }
        if (datee.HasValue)
        {
          q1 = q1.Where(a => a.DateReg <= datee);
        }
      }

      if (TypeID.HasValue)
        q1 = q1.Where(p => p.StateID == TypeID);
      if (NotPayed ?? false)
        q1 = q1.Where(p => p.DatePay == null);
      if (Reserved ?? false)
        q1 = q1.Where(p => p.IsReserve == true);

      var q = from i in q1 //db.tOrdPayments
              from opd in
                (from od in i.tOrdPaymentDets
                 group od by od.OrdPaymID into g
                 select new
                 {
                   Qty = g.Sum(p => p.Qty),
                   VAT = g.Sum(p => p.VAT),
                   QtyR = g.Sum(p => p.QtyR),
                   VATR = g.Sum(p => p.VATR)
                 }).DefaultIfEmpty()
              from opс in
                (from oс in i.tOrdPaymentConfs
                 group oс by oс.OrdPaymID into g
                 select new
                 {
                   Qty = g.Sum(p => p.Qty)
                 }).DefaultIfEmpty()
              join f in db.tFinancialInstitutions on i.FinInstID equals f.FinancialInstitutionID into fl
              from f in fl.DefaultIfEmpty()
              join oc3 in db.tObjClsRelations.Where(p => p.ObjClassifierID == 1003 && p.ObjType == 741604640) on i.FinInstID equals oc3.ObjectID into oc3_
              from oc3 in oc3_.DefaultIfEmpty()
              join oc in db.tObjClassifiers on i.ExecutorID equals oc.ObjClassifierID into oc_
              from oc in oc_.DefaultIfEmpty()
              join oc2 in db.tObjClassifiers on i.DocTypeID equals oc2.ObjClassifierID into oc2_
              from oc2 in oc2_.DefaultIfEmpty()
              join oc4 in db.tObjClassifiers.Where(p => p.ParentID == 48759) on i.StateID.ToString() equals oc4.NameBrief into oc4_
              from oc4 in oc4_.DefaultIfEmpty()
              join s in db.tSecurities on i.FundID equals s.SecurityID into s_
              from s in s_.DefaultIfEmpty()
              join c in db.tContragents on i.ReceiverID equals c.Id into c_
              from c in c_.DefaultIfEmpty()
              join d in db.tContragentDogs on i.DogovorID equals d.Id into d_
              from d in d_.DefaultIfEmpty()
              select new
              {
                id = i.ID,
                i.Number,
                i.IsBudget,
                IsBudgetS = i.IsBudget.Value ? "Да" : "Нет",
                i.IsPlan,
                IsPlanS = i.IsPlan.Value ? "Да" : "Нет",
                i.IsReserve,
                IsReserveS = i.IsReserve.Value ? "Да" : "Нет",
                i.FinInstID,
                NameBrief = oc3.Comment ?? f.NameBrief,
                i.DocTypeID,
                DocTypeName = oc2.Name,
                i.DateDoc,
                i.DocNumb,
                i.DogovorID,
                DogName = (d.Name ?? "") + (d.Number == null ? "" : " №" + d.Number),
                d.DogDate,
                i.Dogovor,
                Receiver = c.Name,
                i.ReceiverID,
                i.ReceiverID2,
                i.ExecutorID,
                ExecutorName = oc.Name,
                i.FundID,
                FundName = s.NameBrief.Trim(),
                i.DateCreate,
                i.DatePay,
                i.DateReg,
                i.PlatNumb,
                Qty = opd.Qty,
                VAT = opd.VAT,
                QtyR = opd.QtyR,
                VATR = opd.VATR,
                QtyC = opс.Qty,
                QtyD = (opd.Qty ?? 0m) - (opс.Qty ?? 0m),
                i.SignFIO1,
                i.SignPost1,
                i.FileName,
                i.FileNamePP,
                i.IsNeedPP,
                i.PPTypeID,
                i.SignID,
                i.Sign2ID,
                i.StateID,
                State = oc4.Name,
                i.IsLoan
              };

      if (filter != null)
      {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        object jsonData = serializer.DeserializeObject(filter);
        IList l = jsonData as IList;
        if (l != null)
          for (int i = 0; i < l.Count; i++)
          {
            IDictionary<string, object> d = l[i] as IDictionary<string, object>;
            if (d != null)
            {
              string field = null;
              object value = null;
              foreach (KeyValuePair<string, object> entry in d)
              {
                if (entry.Key == "field")
                  field = entry.Value as string;
                if (entry.Key == "value")
                  value = entry.Value;
              }
              if (field != null)
              {
                if (field == "NameBrief")
                  q = q.Where(p => p.NameBrief.Contains(value as string));
                if (field == "Receiver")
                  q = q.Where(p => p.Receiver.Contains(value as string));
              }
            }
          }
      }
      return q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
    }

    public IEnumerable<dynamic> GetOrderPayment(DateTime? dateb, DateTime? datee, int? DateType)
    {
      var q = (from i in db.tOrdPayments
               join pd in db.tOrdPaymentDets on i.ID equals pd.OrdPaymID into _pd
               from pd in _pd.DefaultIfEmpty()
               join f in db.tFinancialInstitutions on i.FinInstID equals f.FinancialInstitutionID into fl
               from f in fl.DefaultIfEmpty()
               join oc3 in db.tObjClsRelations.Where(p => p.ObjClassifierID == 1003 && p.ObjType == 741604640) on i.FinInstID equals oc3.ObjectID into oc3_
               from oc3 in oc3_.DefaultIfEmpty()
               join oc in db.tObjClassifiers on i.ExecutorID equals oc.ObjClassifierID into oc_
               from oc in oc_.DefaultIfEmpty()
               join oc2 in db.tObjClassifiers on i.DocTypeID equals oc2.ObjClassifierID into oc2_
               from oc2 in oc2_.DefaultIfEmpty()
               join s in db.tSecurities on i.FundID equals s.SecurityID into s_
               from s in s_.DefaultIfEmpty()
               join oc4 in db.tObjClassifiers on pd.TypeID equals oc4.ObjClassifierID into oc4_
               from oc4 in oc4_.DefaultIfEmpty()
               from oc5 in
                 (
                   from o1 in db.tObjClassifiers.Where(p => p.ParentID == 1019)
                   join oc6 in db.tObjClassifiers on o1.ObjClassifierID equals oc6.ParentID
                   join oc7 in db.tObjClassifiers.Where(p => p.NameBrief == pd.Item) on oc6.ObjClassifierID equals oc7.ParentID
                   select new { ItemName = oc7.Name, ItemParent = o1.Name }).Take(1).DefaultIfEmpty()
               join c in db.tContragents on i.ReceiverID equals c.Id into c_
               from c in c_.DefaultIfEmpty()
               join d in db.tContragentDogs on i.DogovorID equals d.Id into d_
               from d in d_.DefaultIfEmpty()
               join oc6 in db.tObjClassifiers on pd.PeriodicityID equals oc6.ObjClassifierID into oc6_
               from oc6 in oc6_.DefaultIfEmpty()
               join oc7 in db.tObjClassifiers.Where(p => p.ParentID == 48759) on i.StateID.ToString() equals oc7.NameBrief into oc7_
               from oc7 in oc7_.DefaultIfEmpty()
               join oc8 in db.tObjClassifiers on pd.TRID equals oc8.ObjClassifierID into oc8_
               from oc8 in oc8_.DefaultIfEmpty()
               select new
               {
                 id = i.ID,
                 i.Number,
                 i.IsBudget,
                 IsBudgetS = i.IsBudget.Value ? "Да" : "Нет",
                 i.IsPlan,
                 IsPlanS = i.IsPlan.Value ? "Да" : "Нет",
                 i.IsReserve,
                 IsReserveS = i.IsReserve.Value ? "Да" : "Нет",
                 i.FinInstID,
                 NameBrief = oc3.Comment ?? f.NameBrief,
                 i.DocTypeID,
                 DocTypeName = oc2.Name,
                 i.DateDoc,
                 i.DocNumb,
                 i.DogovorID,
                 DogName = (d.Name ?? "") + (d.Number == null ? "" : " №" + d.Number),
                 d.DogDate,
                 i.Dogovor,
                 Receiver = c.Name,
                 i.ReceiverID,
                 i.ReceiverID2,
                 i.ExecutorID,
                 ExecutorName = oc.Name,
                 i.FundID,
                 FundName = s.NameBrief,
                 i.DateCreate,
                 i.DatePay,
                 i.DateReg,
                 i.PlatNumb,
                 pd.Comment,
                 pd.Item,
                 oc5.ItemName,
                 oc5.ItemParent,
                 pd.PFP,
                 pd.Qty,
                 pd.VAT,
                 pd.QtyR,
                 pd.VATR,
                 TypeName = oc4.Name,
                 pd.DateRegEnd,
                 i.SignFIO1,
                 i.SignPost1,
                 i.FileName,
                 i.SignID,
                 i.Sign2ID,
                 PeriodicityName = oc6.Name,
                 TRName = oc8.Name,
                 State = oc7.Name,
                 pd.IsCITU,
                 i.IsLoan
               });

      if (DateType == 0)
      {
        if (dateb.HasValue)
        {
          q = q.Where(a => a.DateCreate >= dateb);
        }
        if (datee.HasValue)
        {
          q = q.Where(a => a.DateCreate <= datee);
        }
      }
      else if (DateType == 1)
      {
        if (dateb.HasValue)
        {
          q = q.Where(a => a.DateDoc >= dateb);
        }
        if (datee.HasValue)
        {
          q = q.Where(a => a.DateDoc <= datee);
        }
      }
      else if (DateType == 2)
      {
        if (dateb.HasValue)
        {
          q = q.Where(a => a.DatePay >= dateb);
        }
        if (datee.HasValue)
        {
          q = q.Where(a => a.DatePay <= datee);
        }
      }
      else if (DateType == 3)
      {
        if (dateb.HasValue)
        {
          q = q.Where(a => a.DateReg >= dateb);
        }
        if (datee.HasValue)
        {
          q = q.Where(a => a.DateReg <= datee);
        }
      }

      return q;
    }

    public dynamic GetOrderPaym(int? id)
    {
      var q = from i in db.tOrdPayments
              join f in db.tFinancialInstitutions on i.FinInstID equals f.FinancialInstitutionID into fl
              from f in fl.DefaultIfEmpty()
              join oc3 in db.tObjClsRelations.Where(p => p.ObjClassifierID == 1003 && p.ObjType == 741604640) on i.FinInstID equals oc3.ObjectID into oc3_
              from oc3 in oc3_.DefaultIfEmpty()
              join oc in db.tObjClassifiers on i.ExecutorID equals oc.ObjClassifierID into oc_
              from oc in oc_.DefaultIfEmpty()
              join oc2 in db.tObjClassifiers on i.DocTypeID equals oc2.ObjClassifierID into oc2_
              from oc2 in oc2_.DefaultIfEmpty()
              join oc4 in db.tObjClassifiers.Where(p => p.ParentID == 48759) on i.StateID.ToString() equals oc4.NameBrief into oc4_
              from oc4 in oc4_.DefaultIfEmpty()
              join s in db.tSecurities on i.FundID equals s.SecurityID into s_
              from s in s_.DefaultIfEmpty()
              join c in db.tContragents on i.ReceiverID equals c.Id into c_
              from c in c_.DefaultIfEmpty()
              join d in db.tContragentDogs on i.DogovorID equals d.Id into d_
              from d in d_.DefaultIfEmpty()
              where i.ID == id
              select new
              {
                id = i.ID,
                i.Number,
                i.IsBudget,
                IsBudgetS = i.IsBudget.Value ? "Да" : "Нет",
                i.IsPlan,
                IsPlanS = i.IsPlan.Value ? "Да" : "Нет",
                i.IsReserve,
                IsReserveS = i.IsReserve.Value ? "Да" : "Нет",
                i.FinInstID,
                NameBrief = oc3.Comment ?? f.NameBrief,
                i.DocTypeID,
                DocTypeName = oc2.Name,
                i.DateDoc,
                i.DocNumb,
                i.DogovorID,
                DogName = (d.Name ?? "") + (d.Number == null ? "" : " №" + d.Number),
                d.DogDate,
                i.Dogovor,
                Receiver = c.Name,
                i.ReceiverID,
                i.ReceiverID2,
                i.ExecutorID,
                ExecutorName = oc.Name,
                i.FundID,
                FundName = s.NameBrief.Trim(),
                i.DateCreate,
                i.DateReg,
                i.SignFIO1,
                i.SignPost1,
                i.FileName,
                i.SignID,
                i.Sign2ID,
                i.StateID,
                State = oc4.Name,
                i.FileNamePP,
                i.PPTypeID,
                DatePay = i.DatePay ?? DateTime.Today,
                FileNameCD = d.FileName,
                i.IsLoan
              };
      return q.FirstOrDefault().ToExpando();
    }

    public IEnumerable<dynamic> GetOrderPaymDetF(int? id)
    {
      var q = from p in
                (
                  (
                  from p in db.tOrdPaymentDets.Where(r => r.OrdPaymID == id)
                  select new
                  {
                    s = 1,
                    p.ID,
                    p.Item,
                    p.PFP,
                    Qty = p.Qty - (p.VAT ?? 0),
                    QtyR = p.QtyR - (p.VATR ?? 0),
                    p.Comment
                  }).Union(
                  from p in db.tOrdPaymentDets.Where(r => r.OrdPaymID == id && ((r.tOrdPayment.FundID == 39191 && r.VAT > 0) || (r.tOrdPayment.FundID != 39191 && r.VATR > 0)))
                  select new
                  {
                    s = 2,
                    p.ID,
                    p.Item,
                    p.PFP,
                    Qty = p.VAT,
                    QtyR = p.VATR,
                    Comment = "НДС"
                  })
                )
              orderby p.ID, p.s
              select new
              {
                p.s,
                p.ID,
                p.Item,
                p.PFP,
                p.Qty,
                p.QtyR,
                p.Comment
              }.ToExpando();
      return q;
    }

    public IEnumerable<dynamic> OrderPaymentCreate(List<tOrdPayment1> data)
    {
      List<tOrdPayment> d1 = new List<tOrdPayment>();
      foreach (tOrdPayment1 o in data.Where(p => p.ID == 0))
      {
        tOrdPayment o1 = AutoMapper.Mapper.Map<tOrdPayment1, tOrdPayment>(o);
        o1.tContragent = db.tContragents.Single(p => p.Id == o.ReceiverID);
        d1.Add(o1);
        o1.InDateTime = DateTime.Now;

        db.tOrdPayments.InsertOnSubmit(o1);
        if (o.CopyID != null)
        {
          var lopd = db.tOrdPaymentDets.Where(p => p.OrdPaymID == o.CopyID);
          foreach (var od in lopd)
          {
            tOrdPaymentDet od1 = AutoMapper.Mapper.Map<tOrdPaymentDet, tOrdPaymentDet>(od);
            od1.tOrdPayment = o1;
            db.tOrdPaymentDets.InsertOnSubmit(od1);
          }
        }
      }
      db.SubmitChanges();
      var q = from i in db.tOrdPayments
              from opd in
                (from od in i.tOrdPaymentDets
                 group od by 1 into g
                 select new
                 {
                   Qty = g.Sum(p => p.Qty),
                   VAT = g.Sum(p => p.VAT),
                   QtyR = g.Sum(p => p.QtyR),
                   VATR = g.Sum(p => p.VATR)
                 }).DefaultIfEmpty()
              join f in db.tFinancialInstitutions on i.FinInstID equals f.FinancialInstitutionID into fl
              from f in fl.DefaultIfEmpty()
              join oc3 in db.tObjClsRelations.Where(p => p.ObjClassifierID == 1003 && p.ObjType == 741604640) on i.FinInstID equals oc3.ObjectID into oc3_
              from oc3 in oc3_.DefaultIfEmpty()
              join oc in db.tObjClassifiers on i.ExecutorID equals oc.ObjClassifierID into oc_
              from oc in oc_.DefaultIfEmpty()
              join oc2 in db.tObjClassifiers on i.DocTypeID equals oc2.ObjClassifierID into oc2_
              from oc2 in oc2_.DefaultIfEmpty()
              join s in db.tSecurities on i.FundID equals s.SecurityID into s_
              from s in s_.DefaultIfEmpty()
              join c in db.tContragents on i.ReceiverID equals c.Id into c_
              from c in c_.DefaultIfEmpty()
              join d in db.tContragentDogs on i.DogovorID equals d.Id into d_
              from d in d_.DefaultIfEmpty()
              where i.ID == d1[0].ID
              select new
              {
                id = i.ID,
                i.Number,
                i.IsBudget,
                IsBudgetS = i.IsBudget.Value ? "Да" : "Нет",
                i.IsPlan,
                IsPlanS = i.IsPlan.Value ? "Да" : "Нет",
                i.IsReserve,
                IsReserveS = i.IsReserve.Value ? "Да" : "Нет",
                i.FinInstID,
                NameBrief = oc3.Comment ?? f.NameBrief,
                i.DocTypeID,
                DocTypeName = oc2.Name,
                i.DateDoc,
                i.DocNumb,
                i.DogovorID,
                DogName = (d.Name ?? "") + (d.Number == null ? "" : " №" + d.Number),
                d.DogDate,
                i.Dogovor,
                Receiver = c.Name,
                i.ReceiverID,
                i.ExecutorID,
                ExecutorName = oc.Name,
                i.FundID,
                FundName = s.NameBrief,
                i.DateCreate,
                i.DatePay,
                i.DateReg,
                i.PlatNumb,
                Qty = opd.Qty,
                VAT = opd.VAT,
                QtyR = opd.QtyR,
                VATR = opd.VATR,
                i.SignFIO1,
                i.SignPost1,
                i.FileName,
                i.SignID,
                i.Sign2ID,
                i.FileNamePP,
                i.IsNeedPP,
                i.PPTypeID,
                i.IsLoan
              };
      return q;
    }

    public IEnumerable<dynamic> OrderPaymentUpdate(List<tOrdPayment> data, bool IsAdmin)
    {
      var q1 = db.tOrdPayments.Where(o => o.ID == data[0].ID).First();

      if (!(q1.StateID > 0) || IsAdmin)
      {
        q1.FinInstID = data[0].FinInstID;
        q1.Number = data[0].Number;
        q1.SignID = data[0].SignID;
        q1.Sign2ID = data[0].Sign2ID;
        q1.ReceiverID = data[0].ReceiverID;
        q1.DocTypeID = data[0].DocTypeID;
        q1.FundID = data[0].FundID;
      }
      q1.Dogovor = data[0].Dogovor;
      q1.DogovorID = data[0].DogovorID;
      q1.DocNumb = data[0].DocNumb;
      q1.DateDoc = data[0].DateDoc;
      q1.DateCreate = data[0].DateCreate;
      q1.ExecutorID = data[0].ExecutorID;
      //q1.Receiver = data[0].Receiver;
      q1.IsBudget = data[0].IsBudget;
      q1.IsPlan = data[0].IsPlan;
      q1.IsReserve = data[0].IsReserve;
      q1.DatePay = data[0].DatePay;
      q1.DateReg = data[0].DateReg;
      q1.PlatNumb = data[0].PlatNumb;
      q1.SignFIO1 = data[0].SignFIO1;
      q1.SignPost1 = data[0].SignPost1;
      q1.FileName = data[0].FileName;
      q1.FileNamePP = data[0].FileNamePP;
      q1.IsNeedPP = data[0].IsNeedPP;
      q1.PPTypeID = data[0].PPTypeID;
      q1.ReceiverID2 = data[0].ReceiverID2;
      q1.IsLoan = data[0].IsLoan;
      db.SubmitChanges();

      var q = from i in db.tOrdPayments
              from opd in
                (from od in i.tOrdPaymentDets
                 group od by 1 into g
                 select new
                 {
                   Qty = g.Sum(p => p.Qty),
                   VAT = g.Sum(p => p.VAT),
                   QtyR = g.Sum(p => p.QtyR),
                   VATR = g.Sum(p => p.VATR)
                 }).DefaultIfEmpty()
              join f in db.tFinancialInstitutions on i.FinInstID equals f.FinancialInstitutionID into fl
              from f in fl.DefaultIfEmpty()
              join oc3 in db.tObjClsRelations.Where(p => p.ObjClassifierID == 1003 && p.ObjType == 741604640) on i.FinInstID equals oc3.ObjectID into oc3_
              from oc3 in oc3_.DefaultIfEmpty()
              join oc in db.tObjClassifiers on i.ExecutorID equals oc.ObjClassifierID into oc_
              from oc in oc_.DefaultIfEmpty()
              join oc2 in db.tObjClassifiers on i.DocTypeID equals oc2.ObjClassifierID into oc2_
              from oc2 in oc2_.DefaultIfEmpty()
              join s in db.tSecurities on i.FundID equals s.SecurityID into s_
              from s in s_.DefaultIfEmpty()
              join c in db.tContragents on i.ReceiverID equals c.Id into c_
              from c in c_.DefaultIfEmpty()
              join d in db.tContragentDogs on i.DogovorID equals d.Id into d_
              from d in d_.DefaultIfEmpty()
              where i.ID == data[0].ID
              select new
              {
                id = i.ID,
                i.Number,
                i.IsBudget,
                IsBudgetS = i.IsBudget.Value ? "Да" : "Нет",
                i.IsPlan,
                IsPlanS = i.IsPlan.Value ? "Да" : "Нет",
                i.IsReserve,
                IsReserveS = i.IsReserve.Value ? "Да" : "Нет",
                i.FinInstID,
                NameBrief = oc3.Comment ?? f.NameBrief,
                i.DocTypeID,
                DocTypeName = oc2.Name,
                i.DateDoc,
                i.DocNumb,
                i.DogovorID,
                DogName = (d.Name ?? "") + (d.Number == null ? "" : " №" + d.Number),
                d.DogDate,
                i.Dogovor,
                Receiver = c.Name,
                i.ReceiverID,
                i.ExecutorID,
                ExecutorName = oc.Name,
                i.FundID,
                FundName = s.NameBrief,
                i.DateCreate,
                i.DatePay,
                i.DateReg,
                i.PlatNumb,
                Qty = opd.Qty,
                VAT = opd.VAT,
                QtyR = opd.QtyR,
                VATR = opd.VATR,
                i.SignFIO1,
                i.SignPost1,
                i.FileName,
                i.SignID,
                i.Sign2ID,
                i.FileNamePP,
                i.IsNeedPP,
                i.PPTypeID,
                i.IsLoan
              };
      return q;
    }

    public bool OrderPaymentDel(List<tOrdPayment> data)
    {
      try
      {
        var q = db.tOrdPayments.Where(o => o.ID == data[0].ID && !((o.StateID ?? 0) > 0));
        db.tOrdPayments.DeleteAllOnSubmit(q);
        db.SubmitChanges();
        return true;
      }
      catch
      {
        return false;
      }
    }

    public IEnumerable<dynamic> GetPaymFunds()
    {
      return from p in db.tSecurities
             where p.SecurityID == 39191 || p.SecurityID == 39192 || p.SecurityID == 39199 || p.SecurityID == 39188
             select new { id = p.SecurityID, name = p.NameBrief };
    }

    public IEnumerable<dynamic> GetPoluch1(string query)
    {
      var q = db.tOrdPayments
        .Where(p => p.DateCreate > DateTime.Today.AddYears(-1))
        .Where(p => p.Receiver.Contains(query))
        .Select(p => new { Value = p.Receiver }).Distinct().Take(20);
      return q;
    }

    public IEnumerable<dynamic> GetSign(string query)
    {
      var q = db.tOrdPayments
        .Where(p => p.DateCreate > DateTime.Today.AddYears(-1))
        .Where(p => p.SignFIO1.StartsWith(query))
        .OrderBy(p => p.SignFIO1)
        .Select(p => new { Value = p.SignFIO1, Value1 = p.SignPost1 }).Distinct().Take(20);
      return q;
    }

    public dynamic CheckOrdPaymByPlatNumb(string Receiver, string DocNumb)
    {
      return db.tOrdPayments.Where(p => p.Receiver == Receiver && p.DocNumb == DocNumb).Select(p => new { p.DateDoc }).FirstOrDefault();
    }

    public dynamic CheckOrdPaymByPlatNumb1(int ReceiverID, string DocNumb)
    {
      return db.tOrdPayments.Where(p => p.ReceiverID == ReceiverID && p.DocNumb == DocNumb).Select(p => new { p.DateDoc }).FirstOrDefault();
    }

    public ISingleResult<up_avgRepFinDataResult> getOrdPaymData(int? id, string item)
    {
      ISingleResult<up_avgRepFinDataResult> q = db.up_avgRepFinData(id, item);
      return q;
    }

    public IEnumerable<dynamic> GetOrderPaymDet(int? id)
    {
      var q = from p in db.tOrdPaymentDets.Where(r => r.OrdPaymID == id)
              join oc2 in db.tObjClassifiers on p.TypeID equals oc2.ObjClassifierID into oc2_
              from oc2 in oc2_.DefaultIfEmpty()
              join oc3 in db.tObjClassifiers on p.PeriodicityID equals oc3.ObjClassifierID into oc3_
              from oc3 in oc3_.DefaultIfEmpty()
              join oc8 in db.tObjClassifiers on p.TRID equals oc8.ObjClassifierID into oc8_
              from oc8 in oc8_.DefaultIfEmpty()
              select new
              {
                id = p.ID,
                p.OrdPaymID,
                p.Item,
                p.PFP,
                p.Qty,
                p.VAT,
                p.QtyR,
                p.VATR,
                p.Comment,
                p.TypeID,
                TypeName = oc2.Name,
                p.DateRegEnd,
                p.PeriodicityID,
                PeriodicityName = oc3.Name,
                p.TRID,
                TRName = oc8.Name,
                p.IsCITU,
                StateID = p.tOrdPayment.StateID
              };
      return q;
    }

    public IEnumerable<dynamic> OrderPaymDetCreate(List<tOrdPaymentDet> data)
    {
      foreach (tOrdPaymentDet o in data.Where(p => p.ID == 0))
        db.tOrdPaymentDets.InsertOnSubmit(o);
      db.SubmitChanges();
      var q = (from p in db.tOrdPaymentDets
               join oc2 in db.tObjClassifiers on p.TypeID equals oc2.ObjClassifierID into oc2_
               from oc2 in oc2_.DefaultIfEmpty()
               join oc3 in db.tObjClassifiers on p.PeriodicityID equals oc3.ObjClassifierID into oc3_
               from oc3 in oc3_.DefaultIfEmpty()
               join oc8 in db.tObjClassifiers on p.TRID equals oc8.ObjClassifierID into oc8_
               from oc8 in oc8_.DefaultIfEmpty()
               where p.ID == data[0].ID
               select new
               {
                 id = p.ID,
                 p.OrdPaymID,
                 p.Item,
                 p.PFP,
                 p.Qty,
                 p.VAT,
                 p.QtyR,
                 p.VATR,
                 p.Comment,
                 p.TypeID,
                 TypeName = oc2.Name,
                 p.DateRegEnd,
                 p.PeriodicityID,
                 PeriodicityName = oc3.Name,
                 p.TRID,
                 TRName = oc8.Name,
                 p.IsCITU,
                 StateID = p.tOrdPayment.StateID
               });
      return q;
    }

    public IEnumerable<dynamic> OrderPaymDetUpdate(List<tOrdPaymentDet> data, bool IsAdmin)
    {
      var q1 = db.tOrdPaymentDets.Where(o => o.ID == data[0].ID).First();

      if (!(q1.tOrdPayment.StateID > 0) || IsAdmin)
      {
        q1.Item = data[0].Item;
        q1.PFP = data[0].PFP;
        q1.Qty = data[0].Qty;
        q1.VAT = data[0].VAT;
        q1.QtyR = data[0].QtyR;
        q1.VATR = data[0].VATR;
      }
      q1.Comment = data[0].Comment;
      q1.TypeID = data[0].TypeID;
      q1.DateRegEnd = data[0].DateRegEnd;
      q1.PeriodicityID = data[0].PeriodicityID;
      q1.TRID = data[0].TRID;
      q1.IsCITU = data[0].IsCITU;
      db.SubmitChanges();

      var q = (from p in db.tOrdPaymentDets
               join oc2 in db.tObjClassifiers on p.TypeID equals oc2.ObjClassifierID into oc2_
               from oc2 in oc2_.DefaultIfEmpty()
               join oc3 in db.tObjClassifiers on p.PeriodicityID equals oc3.ObjClassifierID into oc3_
               from oc3 in oc3_.DefaultIfEmpty()
               join oc8 in db.tObjClassifiers on p.TRID equals oc8.ObjClassifierID into oc8_
               from oc8 in oc8_.DefaultIfEmpty()
               where p.ID == data[0].ID
               select new
               {
                 id = p.ID,
                 p.OrdPaymID,
                 p.Item,
                 p.PFP,
                 p.Qty,
                 p.VAT,
                 p.QtyR,
                 p.VATR,
                 p.Comment,
                 p.TypeID,
                 TypeName = oc2.Name,
                 p.DateRegEnd,
                 p.PeriodicityID,
                 PeriodicityName = oc3.Name,
                 p.TRID,
                 TRName = oc8.Name,
                 p.IsCITU,
                 StateID = p.tOrdPayment.StateID
               });
      return q;
    }

    public bool OrderPaymDetDel(List<tOrdPaymentDet> data)
    {
      try
      {
        var q = db.tOrdPaymentDets.Where(o => o.ID == data[0].ID && (o.tOrdPayment.StateID ?? 0) == 0);
        db.tOrdPaymentDets.DeleteAllOnSubmit(q);
        db.SubmitChanges();
        return true;
      }
      catch
      {
        return false;
      }
    }

    public IEnumerable<dynamic> GetContragentList(string sort, string dir, bool? isNotArchive, string name)
    {
      var q = from p in db.tContragents
              join s in db.tSecurities on p.FundID equals s.SecurityID into s_
              from s in s_.DefaultIfEmpty()
              join oc in db.tObjClassifiers on p.TypeID equals oc.ObjClassifierID into oc_
              from oc in oc_.DefaultIfEmpty()
              join c in db.tCountries on p.CountryID equals c.CountryID into c_
              from c in c_.DefaultIfEmpty()
              select new
              {
                id = p.Id,
                p.Name,
                p.Brief,
                p.INN,
                p.Address1,
                p.Address2,
                p.NamePrev,
                p.IsArchive,
                p.TypeID,
                TypeName = oc.Name,
                p.CountryID,
                Country = c.Name,
                p.KPP,
                p.OGRN,
                p.Bank,
                p.Account,
                p.FundID,
                FundName = s.NameBrief
              };
      if (isNotArchive.HasValue && isNotArchive == true)
        q = q.Where(p => p.IsArchive == !isNotArchive);
      if (!string.IsNullOrWhiteSpace(name))
        q = q.Where(p => p.Brief.Contains(name) || p.Name.Contains(name) || p.INN.StartsWith(name));
      return q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
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

    public IEnumerable<dynamic> ContragentCreate(List<tContragent> data)
    {
      foreach (tContragent o in data.Where(p => p.Id == 0))
        db.tContragents.InsertOnSubmit(o);
      db.SubmitChanges();
      var q = from p in db.tContragents
              join s in db.tSecurities on p.FundID equals s.SecurityID into s_
              from s in s_.DefaultIfEmpty()
              join oc in db.tObjClassifiers on p.TypeID equals oc.ObjClassifierID into oc_
              from oc in oc_.DefaultIfEmpty()
              join c in db.tCountries on p.CountryID equals c.CountryID into c_
              from c in c_.DefaultIfEmpty()
              where p.Id == data[0].Id
              select new
              {
                id = p.Id,
                p.Name,
                p.Brief,
                p.INN,
                p.Address1,
                p.Address2,
                p.NamePrev,
                p.IsArchive,
                p.TypeID,
                TypeName = oc.Name,
                p.CountryID,
                Country = c.Name,
                p.KPP,
                p.OGRN,
                p.Bank,
                p.Account,
                p.FundID,
                FundName = s.NameBrief
              };
      return q;
    }

    public IEnumerable<dynamic> ContragentUpdate(List<tContragent> data)
    {
      var q1 = db.tContragents.Where(o => o.Id == data[0].Id).First();

      AutoMapper.Mapper.Map<tContragent, tContragent>(data[0], q1);
      db.SubmitChanges();

      var q = from p in db.tContragents
              join s in db.tSecurities on p.FundID equals s.SecurityID into s_
              from s in s_.DefaultIfEmpty()
              join oc in db.tObjClassifiers on p.TypeID equals oc.ObjClassifierID into oc_
              from oc in oc_.DefaultIfEmpty()
              join c in db.tCountries on p.CountryID equals c.CountryID into c_
              from c in c_.DefaultIfEmpty()
              where p.Id == data[0].Id
              select new
              {
                id = p.Id,
                p.Name,
                p.Brief,
                p.INN,
                p.Address1,
                p.Address2,
                p.NamePrev,
                p.IsArchive,
                p.TypeID,
                TypeName = oc.Name,
                p.CountryID,
                Country = c.Name,
                p.KPP,
                p.OGRN,
                p.Bank,
                p.Account,
                p.FundID,
                FundName = s.NameBrief
              };
      return q;
    }

    public bool ContragentDel(List<tContragent> data)
    {
      try
      {
        var q = db.tContragents.Where(o => o.Id == data[0].Id);
        db.tContragents.DeleteAllOnSubmit(q);
        db.SubmitChanges();
        return true;
      }
      catch
      {
        return false;
      }
    }

    public IEnumerable<dynamic> GetCountries()
    {
      var q = from o in db.tObjClsRelations
              join c in db.tCountries on o.ObjectID equals c.CountryID
              where o.ObjClassifierID == 49408 && o.ObjType == -887623042
              select new
              {
                id = c.CountryID,
                name = c.Name
              };
      return q;
    }

    public IEnumerable<dynamic> GetContragentDogList(int id)
    {
      var q = from p in db.tContragentDogs.Where(p => p.ContragentId == id)
              select new
              {
                id = p.Id,
                p.Name,
                p.DogDate,
                p.ContragentId,
                p.Number,
                p.FileName
              };
      return q;
    }

    public IEnumerable<dynamic> ContragentDogCreate(List<tContragentDog> data)
    {
      foreach (tContragentDog o in data.Where(p => p.Id == 0))
        db.tContragentDogs.InsertOnSubmit(o);
      db.SubmitChanges();
      var q = from p in db.tContragentDogs
              where p.Id == data[0].Id
              select new
              {
                id = p.Id,
                p.Name,
                p.DogDate,
                p.ContragentId,
                p.Number,
                p.FileName
              };
      return q;
    }

    public IEnumerable<dynamic> ContragentDogUpdate(List<tContragentDog> data)
    {
      var q1 = db.tContragentDogs.Where(o => o.Id == data[0].Id).First();

      //AutoMapper.Mapper.Map<tContragentDog, tContragentDog>(data[0], q1);
      q1.DogDate = data[0].DogDate;
      q1.FileName = data[0].FileName;
      q1.Name = data[0].Name;
      q1.Number = data[0].Number;
      q1.InDateTime = DateTime.Now;
      db.SubmitChanges();

      var q = from p in db.tContragentDogs
              where p.Id == data[0].Id
              select new
              {
                id = p.Id,
                p.Name,
                p.DogDate,
                p.ContragentId,
                p.Number,
                p.FileName
              };
      return q;
    }

    public bool ContragentDogDel(List<tContragentDog> data)
    {
      try
      {
        var q = db.tContragentDogs.Where(o => o.Id == data[0].Id);
        db.tContragentDogs.DeleteAllOnSubmit(q);
        db.SubmitChanges();
        return true;
      }
      catch
      {
        return false;
      }
    }

    public IEnumerable<dynamic> GetDogovor(int id)
    {
      var q = (from p in db.tContragentDogs
               where p.ContragentId == id
               select new
               {
                 p.Id,
                 p.Number,
                 p.DogDate,
                 p.Name
               }).ToList()
              .Select(p => new
              {
                Value = p.Id,
                Text = p.Name + (string.IsNullOrWhiteSpace(p.Number) ? "" : " №" + p.Number) + (p.DogDate.HasValue ? " от " + p.DogDate.Value.ToShortDateString() + "г." : "")
              });
      return q;
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

    public IEnumerable<dynamic> ChargesList(string sort, string dir, int? y, int? m, int? pfp, bool? np)
    {
      var q1 = from o in db.tObjClassifiers.Where(p => p.ParentID == 1019)
               join oc in db.tObjClassifiers on o.ObjClassifierID equals oc.ParentID
               join oc2 in db.tObjClassifiers on oc.ObjClassifierID equals oc2.ParentID
               from ocp in db.tObjClassifiers.Where(p => p.ObjClassifierID == pfp || (pfp == null && p.ParentID == 1054))
               from c in db.tCharges.Where(p => p.Mois == m && p.An == y && p.ItemID == oc2.ObjClassifierID && p.PfpID == ocp.ObjClassifierID).DefaultIfEmpty()
               select new
               {
                 ID = (int?)c.ID,
                 ItemID = oc2.ObjClassifierID,
                 Item1 = o.Name,
                 Item2 = oc.Name,
                 Item3 = oc.NameBrief,
                 Item4 = oc2.Name,
                 Item5 = oc2.NameBrief,
                 PfpID = ocp.ObjClassifierID,
                 Pfp = ocp.Name,
                 Mois = m,
                 Quartier = (m + 2) / 3,
                 An = y,
                 c.QtyF,
                 c.QtyP
               };
      if (np == true)
        q1 = q1.Where(p => p.QtyP.HasValue || p.QtyF.HasValue);
      return q1.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
    }

    public IEnumerable<dynamic> ChargesUpdate(List<tCharge> data)
    {
      if (data.Count() > 0)
      {
        data[0].Quartier = (data[0].Mois + 2) / 3;
        if (data[0].ID > 0)
        {
          var r = db.tCharges.Where(o => o.ID == data[0].ID).FirstOrDefault();
          if (r != null)
          {
            if (data[0].QtyF.HasValue || data[0].QtyP.HasValue)
            {
              r.QtyF = data[0].QtyF;
              r.QtyP = data[0].QtyP;
            }
            else
              db.tCharges.DeleteOnSubmit(r);
            db.SubmitChanges();
          }
        }
        else
        {
          tCharge o = data[0];
          db.tCharges.InsertOnSubmit(o);
          db.SubmitChanges();
        }
      }
      var q1 = from oc2 in db.tObjClassifiers.Where(p => p.ObjClassifierID == data[0].ItemID)
               join oc in db.tObjClassifiers on oc2.ParentID equals oc.ObjClassifierID
               join o in db.tObjClassifiers on oc.ParentID equals o.ObjClassifierID
               from ocp in db.tObjClassifiers.Where(p => p.ObjClassifierID == data[0].PfpID)
               join c in db.tCharges.Where(p => p.PfpID == data[0].PfpID && p.Mois == data[0].Mois && p.An == data[0].An) on oc2.ObjClassifierID equals c.ItemID into c1
               from c in c1.DefaultIfEmpty()
               select new
               {
                 ID = c.ID as int?,
                 ItemID = oc2.ObjClassifierID,
                 Item1 = o.Name,
                 Item2 = oc.Name,
                 Item3 = oc.NameBrief,
                 Item4 = oc2.Name,
                 Item5 = oc2.NameBrief,
                 PfpID = ocp.ObjClassifierID,
                 Pfp = ocp.Name,
                 Mois = data[0].Mois,
                 Quartier = (data[0].Mois + 2) / 3,
                 An = data[0].An,
                 c.QtyF,
                 c.QtyP
               };
      return q1;
    }

    public void ImportOrdControl(int? y, int? q)
    {
      //var q1 = (from c1 in
      //            (from c in db.tOrdPayments.Where(p => p.IsBudget == true)
      //             join d in db.tOrdPaymentDets on c.ID equals d.OrdPaymID
      //             from ocp in db.tObjClassifiers.Where(p => p.ParentID == 1054 && p.Name == d.PFP)
      //             from oc in
      //               (
      //                from oc1 in db.tObjClassifiers.Where(p => p.ParentID == 1019)
      //                join oc2 in db.tObjClassifiers on oc1.ObjClassifierID equals oc2.ParentID
      //                join oc3 in db.tObjClassifiers.Where(p => p.NameBrief == d.Item) on oc2.ObjClassifierID equals oc3.ParentID
      //                select new { oc3.ObjClassifierID }
      //               )
      //             where (new int[] { 15186, 15187 }.Contains(d.TypeID.Value) && c.DatePay.Value.Year == y && (c.DatePay.Value.Month - 1) / 3 + 1 == q) || (d.TypeID == 15188 && c.DatePay.Value.Year * 12 + c.DatePay.Value.Month >= y * 12 + (q - 1) * 3 + 1 - 11 && c.DatePay.Value.Year * 12 + c.DatePay.Value.Month <= y * 12 + q * 3)
      //             select new
      //             {
      //               ItemID = oc.ObjClassifierID,
      //               PfpID = ocp.ObjClassifierID,
      //               Qty = d.Qty * (d.TypeID == 15186 || d.TypeID == 15187 ? 1 : d.TypeID == 15188 ?
      //                 (c.DatePay.Value.Year * 12 + c.DatePay.Value.Month - y * 12 + (q - 1) * 3 + 1 == 11 || c.DatePay.Value.Year * 12 + c.DatePay.Value.Month == y * 12 + q * 3 ? 1 : c.DatePay.Value.Year * 12 + c.DatePay.Value.Month - y * 12 + (q - 1) * 3 + 1 == 10 || c.DatePay.Value.Year * 12 + c.DatePay.Value.Month == y * 12 + q * 3 - 1 ? 2 : 3) / 12m
      //                 : 1)
      //             })
      //          group c1 by new { c1.ItemID, c1.PfpID }
      //            into g
      //            select new { g.Key.ItemID, g.Key.PfpID, Qty = g.Sum(t => t.Qty) }).ToList();

      var q1 = (from c1 in
                  (from c in db.tOrdPayments.Where(p => p.IsBudget == true)
                   join d in db.tOrdPaymentDets on c.ID equals d.OrdPaymID
                   from ocp in db.tObjClassifiers.Where(p => p.ParentID == 1054 && p.Name == d.PFP)
                   from oc in
                     (
                      from oc1 in db.tObjClassifiers.Where(p => p.ParentID == 1019)
                      join oc2 in db.tObjClassifiers on oc1.ObjClassifierID equals oc2.ParentID
                      join oc3 in db.tObjClassifiers.Where(p => p.NameBrief == d.Item) on oc2.ObjClassifierID equals oc3.ParentID
                      select new { oc3.ObjClassifierID }
                     )
                   where (c.DatePay.Value.Year == y && (c.DatePay.Value.Month - 1) / 3 + 1 == q)
                   select new
                   {
                     ItemID = oc.ObjClassifierID,
                     PfpID = ocp.ObjClassifierID,
                     Qty = d.Qty
                   })
                group c1 by new { c1.ItemID, c1.PfpID }
                  into g
                  select new { g.Key.ItemID, g.Key.PfpID, Qty = g.Sum(t => t.Qty) }).ToList();


      //db.tCharges.DeleteAllOnSubmit(db.tCharges.Where(p => p.An == y && p.Quartier == q));
      foreach (var d in db.tCharges.Where(p => p.Quartier == q && p.An == y && (p.QtyF ?? 0) != 0))
      {
        d.QtyF = null;
      }
      db.SubmitChanges();
      foreach (var r in q1)
      {
        var cq = db.tCharges.Where(p => p.An == y && p.Quartier == q && p.ItemID == r.ItemID && p.PfpID == r.PfpID).FirstOrDefault();
        if (cq != null)
        {
          cq.QtyF = r.Qty;
        }
        else
        {
          tCharge o = new tCharge() { An = y, Quartier = q, ItemID = r.ItemID, PfpID = r.PfpID, QtyF = r.Qty };
          db.tCharges.InsertOnSubmit(o);
        }
        db.SubmitChanges();
      }
    }

    public IEnumerable<dynamic> GetFPS(string sort, string dir, string filter)
    {
      var q = from v in db.tFinPlanSchemas
              select new { v.Agent_Name, v.Agent_Type, v.ID, v.IFRS_SubSector, v.Instr_Type, v.Prod_Group, v.Prod_Klass, v.Prod_Residence, v.Prod_ShortName, v.Prod_SubGroup, v.Sub_OrgUnitName_To, v.Sub_UrUnitName_From, v.Sub_UrUnitName_To, v.TaxGroup, v.TCS_Istochnik_From_Kategory, v.TCS_Type_From, v.TCS_Vid_From, v.UR_Format_AG, v.UR_Format_KL };
      return q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
    }

    public IEnumerable<dynamic> FPSCreate(List<tFinPlanSchema> data)
    {
      foreach (tFinPlanSchema o in data.Where(p => p.ID == 0))
        db.tFinPlanSchemas.InsertOnSubmit(o);
      db.SubmitChanges();
      return db.tFinPlanSchemas.Where(p => p.ID == data[0].ID);
    }

    public IEnumerable<dynamic> FPSUpdate(List<tFinPlanSchema> data)
    {
      var q1 = db.tFinPlanSchemas.Where(o => o.ID == data[0].ID).First();

      q1.Agent_Name = data[0].Agent_Name;
      q1.Agent_Type = data[0].Agent_Type;
      q1.IFRS_SubSector = data[0].IFRS_SubSector;
      q1.Instr_Type = data[0].Instr_Type;
      q1.Prod_Group = data[0].Prod_Group;
      q1.Prod_Klass = data[0].Prod_Klass;
      q1.Prod_Residence = data[0].Prod_Residence;
      q1.Prod_ShortName = data[0].Prod_ShortName;
      q1.Prod_SubGroup = data[0].Prod_SubGroup;
      q1.Sub_OrgUnitName_To = data[0].Sub_OrgUnitName_To;
      q1.Sub_UrUnitName_From = data[0].Sub_UrUnitName_From;
      q1.Sub_UrUnitName_To = data[0].Sub_UrUnitName_To;
      q1.TaxGroup = data[0].TaxGroup;
      q1.TCS_Istochnik_From_Kategory = data[0].TCS_Istochnik_From_Kategory;
      q1.TCS_Type_From = data[0].TCS_Type_From;
      q1.TCS_Vid_From = data[0].TCS_Vid_From;
      q1.UR_Format_AG = data[0].UR_Format_AG;
      q1.UR_Format_KL = data[0].UR_Format_KL;
      db.SubmitChanges();
      return db.tFinPlanSchemas.Where(p => p.ID == data[0].ID);
    }

    public bool FPSDel(List<tFinPlanSchema> data)
    {
      try
      {
        var q = db.tFinPlanSchemas.Where(o => o.ID == data[0].ID);
        db.tFinPlanSchemas.DeleteAllOnSubmit(q);
        db.SubmitChanges();
        return true;
      }
      catch
      {
        return false;
      }
    }

    public IEnumerable<dynamic> FinPlanList(string sort, string dir, int? y, int? q)
    {
      var q1 = from o in db.tObjClassifiers.Where(p => p.ParentID == 1091)
               from s in db.tFinPlanSchemas
               from c in db.tFinPlans.Where(p => p.FpsID == s.ID && p.Quartier == q && p.An == y && p.Fin_form == o.Name && p.Fin_Sector == o.NameBrief && p.Fin_SubSector == o.Comment).DefaultIfEmpty()
               select new
               {
                 c.ID,
                 FpsID = s.ID,
                 Fin_form = o.Name,
                 Fin_Sector = o.NameBrief,
                 Fin_SubSector = o.Comment,
                 Sub_UrUnitName_To = s.Sub_UrUnitName_To,
                 Sub_OrgUnitName_To = s.Sub_OrgUnitName_To,
                 TCS_Type_From = s.TCS_Type_From,
                 Sub_UrUnitName_From = s.Sub_UrUnitName_From,
                 Prod_ShortName = s.Prod_ShortName,
                 Quartier = q,
                 An = y,
                 c.QtyP
               };
      if (sort == null)
      {
        return q1.OrderByDescending(p => p.Fin_form).ThenBy(p => p.Fin_Sector).ThenByDescending(p => p.Fin_SubSector);
      }
      return q1.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
    }

    public IEnumerable<dynamic> FinPlanUpdate(List<tFinPlan> data)
    {
      if (data.Count() > 0)
      {
        if (data[0].ID > 0)
        {
          var r = db.tFinPlans.Where(o => o.ID == data[0].ID).FirstOrDefault();
          if (r != null)
          {
            if (data[0].QtyP.HasValue)
            {
              r.QtyP = data[0].QtyP;
            }
            else
              db.tFinPlans.DeleteOnSubmit(r);
            db.SubmitChanges();
          }
        }
        else
        {
          tFinPlan o = data[0];
          db.tFinPlans.InsertOnSubmit(o);
          db.SubmitChanges();
        }
      }
      var q1 = from s in db.tFinPlanSchemas.Where(p => p.ID == data[0].FpsID)
               from o in db.tObjClassifiers.Where(p => p.ParentID == 1091 && p.Name == data[0].Fin_form && p.NameBrief == data[0].Fin_Sector && p.Comment == data[0].Fin_SubSector)
               from c in db.tFinPlans.Where(p => p.FpsID == s.ID && p.Quartier == data[0].Quartier && p.An == data[0].An && p.Fin_form == o.Name && p.Fin_Sector == o.NameBrief && p.Fin_SubSector == o.Comment).DefaultIfEmpty()
               select new
               {
                 c.ID,
                 FpsID = s.ID,
                 Fin_form = o.Name,
                 Fin_Sector = o.NameBrief,
                 Fin_SubSector = o.Comment,
                 Sub_UrUnitName_To = s.Sub_UrUnitName_To,
                 Sub_OrgUnitName_To = s.Sub_OrgUnitName_To,
                 TCS_Type_From = s.TCS_Type_From,
                 Sub_UrUnitName_From = s.Sub_UrUnitName_From,
                 Prod_ShortName = s.Prod_ShortName,
                 Quartier = data[0].Quartier,
                 An = data[0].An,
                 c.QtyP
               };
      return q1;
    }

    public IEnumerable<dynamic> FinActList(string sort, string dir, int? y, int? q)
    {
      var q1 = from o in db.tObjClassifiers.Where(p => p.ParentID == 1189)
               from c in db.tFinActives.Where(p => p.ObjClsID == o.ObjClassifierID && p.Quartier == q && p.An == y).DefaultIfEmpty()
               select new
               {
                 c.ID,
                 ObjClsID = o.ObjClassifierID,
                 o.Name,
                 Quartier = q,
                 An = y,
                 c.QtyP,
                 c.QtyF
               };
      if (sort == null)
      {
        return q1.OrderBy(p => p.ObjClsID);
      }
      return q1.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
    }

    public IEnumerable<dynamic> FinActUpdate(List<tFinActive> data)
    {
      if (data.Count() > 0)
      {
        if (data[0].ID > 0)
        {
          var r = db.tFinActives.Where(o => o.ID == data[0].ID).FirstOrDefault();
          if (r != null)
          {
            if (data[0].QtyP.HasValue || data[0].QtyF.HasValue)
            {
              r.QtyF = data[0].QtyF;
              r.QtyP = data[0].QtyP;
            }
            else
              db.tFinActives.DeleteOnSubmit(r);
            db.SubmitChanges();
          }
        }
        else
        {
          tFinActive o = data[0];
          db.tFinActives.InsertOnSubmit(o);
          db.SubmitChanges();
        }
      }
      var q1 = from o in db.tObjClassifiers.Where(p => p.ParentID == 1189 && p.ObjClassifierID == data[0].ObjClsID)
               from c in db.tFinActives.Where(p => p.ObjClsID == o.ObjClassifierID && p.Quartier == data[0].Quartier && p.An == data[0].An).DefaultIfEmpty()
               select new
               {
                 c.ID,
                 ObjClsID = o.ObjClassifierID,
                 o.Name,
                 Quartier = data[0].Quartier,
                 An = data[0].An,
                 c.QtyP,
                 c.QtyF
               };

      return q1;
    }

    public IEnumerable<dynamic> FinSalesList(string sort, string dir, int? y, int? q)
    {
      var q1 = from o in db.tObjClassifiers.Where(p => p.ParentID == 1795)
               from o2 in db.tObjClassifiers.Where(p => p.ParentID == o.ObjClassifierID).DefaultIfEmpty()
               from c in db.tFinSales.Where(p => p.ObjClsID == (o2.ObjClassifierID ?? o.ObjClassifierID) && p.Quartier == q && p.An == y).DefaultIfEmpty()
               select new
               {
                 c.ID,
                 o.ObjClassifierID,
                 ObjClsID = o2.ObjClassifierID ?? o.ObjClassifierID,
                 o.Name,
                 Name2 = o2.Name,
                 Quartier = q,
                 An = y,
                 c.QtyI,
                 c.QtyO
               };
      if (sort == null)
      {
        return q1.OrderBy(p => p.ObjClassifierID).ThenBy(p => p.ObjClsID);
      }
      return q1.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
    }

    public IEnumerable<dynamic> FinSalesUpdate(List<tFinSale> data)
    {
      if (data.Count() > 0)
      {
        if (data[0].ID > 0)
        {
          var r = db.tFinSales.Where(o => o.ID == data[0].ID).FirstOrDefault();
          if (r != null)
          {
            if (data[0].QtyI.HasValue || data[0].QtyO.HasValue)
            {
              r.QtyI = data[0].QtyI;
              r.QtyO = data[0].QtyO;
            }
            else
              db.tFinSales.DeleteOnSubmit(r);
            db.SubmitChanges();
          }
        }
        else
        {
          tFinSale o = data[0];
          db.tFinSales.InsertOnSubmit(o);
          db.SubmitChanges();
        }
      }
      var q1 = (from o in db.tObjClassifiers.Where(p => p.ParentID == 1795 && p.ObjClassifierID == data[0].ObjClsID)
                from c in db.tFinSales.Where(p => p.ObjClsID == o.ObjClassifierID && p.Quartier == data[0].Quartier && p.An == data[0].An).DefaultIfEmpty()
                select new
                {
                  c.ID,
                  ObjClsID = o.ObjClassifierID,
                  o.Name,
                  Name2 = "",
                  Quartier = data[0].Quartier,
                  An = data[0].An,
                  c.QtyI,
                  c.QtyO
                }).Union(
                from o2 in db.tObjClassifiers.Where(p => p.ObjClassifierID == data[0].ObjClsID)
                from o in db.tObjClassifiers.Where(p => p.ObjClassifierID == o2.ParentID && p.ParentID == 1795)
                from c in db.tFinSales.Where(p => p.ObjClsID == o2.ObjClassifierID && p.Quartier == data[0].Quartier && p.An == data[0].An).DefaultIfEmpty()
                select new
                {
                  c.ID,
                  ObjClsID = o2.ObjClassifierID,
                  o.Name,
                  Name2 = o2.Name,
                  Quartier = data[0].Quartier,
                  An = data[0].An,
                  c.QtyI,
                  c.QtyO
                });

      return q1;
    }

    public IEnumerable<dynamic> FinProfitList(string sort, string dir, int? y, int? q)
    {
      var q1 = from o in db.tObjClassifiers.Where(p => p.ParentID == 1811)
               from o2 in db.tObjClassifiers.Where(p => p.ParentID == o.ObjClassifierID).DefaultIfEmpty()
               from c in db.tFinProfits.Where(p => p.ObjClsID == (o2.ObjClassifierID ?? o.ObjClassifierID) && p.Quartier == q && p.An == y).DefaultIfEmpty()
               select new
               {
                 c.ID,
                 o.ObjClassifierID,
                 ObjClsID = o2.ObjClassifierID ?? o.ObjClassifierID,
                 o.Name,
                 Name2 = o2.Name,
                 Quartier = q,
                 An = y,
                 c.Qty,
                 c.QtyF
               };
      if (sort == null)
      {
        return q1.OrderBy(p => p.ObjClassifierID).ThenBy(p => p.ObjClsID);
      }
      return q1.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
    }

    public IEnumerable<dynamic> FinProfitUpdate(List<tFinProfit> data)
    {
      if (data.Count() > 0)
      {
        if (data[0].ID > 0)
        {
          var r = db.tFinProfits.Where(o => o.ID == data[0].ID).FirstOrDefault();
          if (r != null)
          {
            if (data[0].Qty.HasValue || data[0].QtyF.HasValue)
            {
              r.Qty = data[0].Qty;
              r.QtyF = data[0].QtyF;
            }
            else
              db.tFinProfits.DeleteOnSubmit(r);
            db.SubmitChanges();
          }
        }
        else
        {
          tFinProfit o = data[0];
          db.tFinProfits.InsertOnSubmit(o);
          db.SubmitChanges();
        }
      }
      var q1 = (from o in db.tObjClassifiers.Where(p => p.ParentID == 1811 && p.ObjClassifierID == data[0].ObjClsID)
                from c in db.tFinProfits.Where(p => p.ObjClsID == o.ObjClassifierID && p.Quartier == data[0].Quartier && p.An == data[0].An).DefaultIfEmpty()
                select new
                {
                  c.ID,
                  ObjClsID = o.ObjClassifierID,
                  o.Name,
                  Name2 = "",
                  Quartier = data[0].Quartier,
                  An = data[0].An,
                  c.Qty,
                  c.QtyF
                }).Union(
                from o2 in db.tObjClassifiers.Where(p => p.ObjClassifierID == data[0].ObjClsID)
                from o in db.tObjClassifiers.Where(p => p.ObjClassifierID == o2.ParentID && p.ParentID == 1811)
                from c in db.tFinProfits.Where(p => p.ObjClsID == o2.ObjClassifierID && p.Quartier == data[0].Quartier && p.An == data[0].An).DefaultIfEmpty()
                select new
                {
                  c.ID,
                  ObjClsID = o2.ObjClassifierID,
                  o.Name,
                  Name2 = o2.Name,
                  Quartier = data[0].Quartier,
                  An = data[0].An,
                  c.Qty,
                  c.QtyF
                });

      return q1;
    }

    public IEnumerable<dynamic> GetFDS(string sort, string dir, string filter)
    {

      var q = from v in db.tFinDohodSchemas
              select new
              {
                v.Agent_Name,
                v.Agent_Type,
                v.DEF_L1,
                v.DEF_L2,
                v.Event_MethodCalc,
                v.Fin_Item,
                v.Fin_Sector,
                v.Fin_SubSector,
                v.ID,
                v.IFRS_Item,
                v.IFRS_SubSector,
                v.Instr_Type,
                v.Prod_Group,
                v.Prod_Klass,
                v.Prod_Residence,
                v.Prod_ShortName,
                v.Prod_SubGroup,
                v.Sub_OrgUnitName_To,
                v.Sub_UrUnitName_From,
                v.Sub_UrUnitName_To,
                v.TaxGroup,
                v.TCS_Istochnik_From,
                v.TCS_Istochnik_From_Kategory,
                v.TCS_Residence,
                v.TCS_Type_From,
                v.TCS_Vid_From,
                v.UR_Format_AG,
                v.UR_Format_KL
              };
      //select new { v.Event_MethodCalc, v.Fin_Item, v.Fin_Sector, v.Fin_SubSector, v.ID, v.Instr_Type, v.Prod_Group, v.Prod_Klass, v.Prod_Residence, v.Prod_ShortName, v.Prod_SubGroup, v.Sub_OrgUnitName_To, v.Sub_UrUnitName_From, v.Sub_UrUnitName_To, v.TaxGroup, v.TCS_Istochnik_From, v.TCS_Istochnik_From_Kategory, v.TCS_Residence, v.TCS_Type_From, v.UR_Format_KL };
      return q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
    }

    public IEnumerable<dynamic> FDSCreate(List<tFinDohodSchema> data)
    {
      foreach (tFinDohodSchema o in data.Where(p => p.ID == 0))
        db.tFinDohodSchemas.InsertOnSubmit(o);
      db.SubmitChanges();
      return db.tFinDohodSchemas.Where(p => p.ID == data[0].ID);
    }

    public IEnumerable<dynamic> FDSUpdate(List<tFinDohodSchema> data)
    {
      var q1 = db.tFinDohodSchemas.Where(o => o.ID == data[0].ID).First();

      q1.Agent_Name = data[0].Agent_Name;
      q1.Agent_Type = data[0].Agent_Type;
      q1.DEF_L1 = data[0].DEF_L1;
      q1.DEF_L2 = data[0].DEF_L2;
      q1.Event_MethodCalc = data[0].Event_MethodCalc;
      q1.Fin_Item = data[0].Fin_Item;
      q1.Fin_Sector = data[0].Fin_Sector;
      q1.Fin_SubSector = data[0].Fin_SubSector;
      q1.IFRS_Item = data[0].IFRS_Item;
      q1.IFRS_SubSector = data[0].IFRS_SubSector;
      q1.Instr_Type = data[0].Instr_Type;
      q1.Prod_Group = data[0].Prod_Group;
      q1.Prod_Klass = data[0].Prod_Klass;
      q1.Prod_Residence = data[0].Prod_Residence;
      q1.Prod_ShortName = data[0].Prod_ShortName;
      q1.Prod_SubGroup = data[0].Prod_SubGroup;
      q1.Sub_OrgUnitName_To = data[0].Sub_OrgUnitName_To;
      q1.Sub_UrUnitName_From = data[0].Sub_UrUnitName_From;
      q1.Sub_UrUnitName_To = data[0].Sub_UrUnitName_To;
      q1.TaxGroup = data[0].TaxGroup;
      q1.TCS_Istochnik_From = data[0].TCS_Istochnik_From;
      q1.TCS_Istochnik_From_Kategory = data[0].TCS_Istochnik_From_Kategory;
      q1.TCS_Type_From = data[0].TCS_Type_From;
      q1.TCS_Vid_From = data[0].TCS_Vid_From;
      q1.UR_Format_KL = data[0].UR_Format_KL;
      q1.UR_Format_AG = data[0].UR_Format_AG;
      db.SubmitChanges();
      return db.tFinDohodSchemas.Where(p => p.ID == data[0].ID);
    }

    public bool FDSDel(List<tFinDohodSchema> data)
    {
      try
      {
        var q = db.tFinDohodSchemas.Where(o => o.ID == data[0].ID);
        db.tFinDohodSchemas.DeleteAllOnSubmit(q);
        db.SubmitChanges();
        return true;
      }
      catch
      {
        return false;
      }
    }

    public IEnumerable<dynamic> FinDohodList(string sort, string dir, int? y, int? q)
    {
      var q1 = from s in db.tFinDohodSchemas
               from c in db.tFinDohods.Where(p => p.FdsID == s.ID && p.Quartier == q && p.An == y).DefaultIfEmpty()
               select new
               {
                 c.ID,
                 FdsID = s.ID,
                 s.Fin_Sector,
                 s.Fin_SubSector,
                 s.Fin_Item,
                 s.Sub_UrUnitName_To,
                 s.Sub_OrgUnitName_To,
                 s.TCS_Type_From,
                 s.Prod_Klass,
                 s.Prod_Group,
                 s.Prod_ShortName,
                 Quartier = q,
                 An = y,
                 c.QtyP,
                 c.QtyF
               };
      if (sort == null)
      {
        return q1.OrderBy(p => p.Fin_Sector).ThenByDescending(p => p.Fin_SubSector);
      }
      return q1.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
    }

    public IEnumerable<dynamic> FinDohodUpdate(List<tFinDohod> data)
    {
      if (data.Count() > 0)
      {
        if (data[0].ID > 0)
        {
          var r = db.tFinDohods.Where(o => o.ID == data[0].ID).FirstOrDefault();
          if (r != null)
          {
            if (data[0].QtyP.HasValue || data[0].QtyF.HasValue)
            {
              r.QtyP = data[0].QtyP;
              r.QtyF = data[0].QtyF;
              r.InDateTime = DateTime.Now;
            }
            else
              db.tFinDohods.DeleteOnSubmit(r);
            db.SubmitChanges();
          }
        }
        else
        {
          tFinDohod o = data[0];
          o.InDateTime = DateTime.Now;
          db.tFinDohods.InsertOnSubmit(o);
          db.SubmitChanges();
        }
      }
      var q1 = from s in db.tFinDohodSchemas.Where(p => p.ID == data[0].FdsID)
               from c in db.tFinDohods.Where(p => p.FdsID == s.ID && p.Quartier == data[0].Quartier && p.An == data[0].An).DefaultIfEmpty()
               select new
               {
                 c.ID,
                 FdsID = s.ID,
                 s.Fin_Sector,
                 s.Fin_SubSector,
                 s.Fin_Item,
                 s.Sub_UrUnitName_To,
                 s.Sub_OrgUnitName_To,
                 s.TCS_Type_From,
                 s.Prod_Klass,
                 s.Prod_Group,
                 s.Prod_ShortName,
                 Quartier = data[0].Quartier,
                 An = data[0].An,
                 c.QtyP,
                 c.QtyF
               };
      return q1;
    }

    public IEnumerable<dynamic> GetValRates()
    {
      var q = (from r in db.tRates
               from s in db.tSecurities.Where(s => s.SecurityID == r.SecurityID)
               from rp in db.tRates.Where(r1 => r1.TradeSystemID == r.TradeSystemID && r1.RawDataProviderID == r.RawDataProviderID && r1.SecurityID == r.SecurityID && r1.ActualizationDateTime < r.ActualizationDateTime).OrderByDescending(r1 => r1.ActualizationDateTime).Take(1).DefaultIfEmpty()
               where r.TradeSystemID == 1 && r.RawDataProviderID == 1 && r.SecurityID == 39192 && r.ActualizationDateTime <= DateTime.Today.AddDays(1)
               orderby r.ActualizationDateTime descending
               select new
               {
                 s = s.NameBrief.Trim(),
                 c = r.CourseCurrent,
                 dt = r.ActualizationDateTime,
                 y = Math.Round(((r.CourseCurrent / (rp.CourseCurrent != 0 ? rp.CourseCurrent : null) - 1) * 100).Value, 3, MidpointRounding.AwayFromZero)
               })
               .Take(1).Concat((
               from r in db.tRates
               from s in db.tSecurities.Where(s => s.SecurityID == r.SecurityID)
               from rp in db.tRates.Where(r1 => r1.TradeSystemID == r.TradeSystemID && r1.RawDataProviderID == r.RawDataProviderID && r1.SecurityID == r.SecurityID && r1.ActualizationDateTime < r.ActualizationDateTime).OrderByDescending(r1 => r1.ActualizationDateTime).Take(1).DefaultIfEmpty()
               where r.TradeSystemID == 1 && r.RawDataProviderID == 1 && r.SecurityID == 39199 && r.ActualizationDateTime <= DateTime.Today.AddDays(1)
               orderby r.ActualizationDateTime descending
               select new
               {
                 s = s.NameBrief.Trim(),
                 c = r.CourseCurrent,
                 dt = r.ActualizationDateTime,
                 y = Math.Round(((r.CourseCurrent / (rp.CourseCurrent != 0 ? rp.CourseCurrent : null) - 1) * 100).Value, 3, MidpointRounding.AwayFromZero)
               }).Take(1));

      return q;
    }

    public IEnumerable<dynamic> GetPifRates()
    {
      var q = (from s in db.tSecurities
               where s.SecType == 6 && s.IsDeleted == 0 && new int?[] { 261, 262, 264, 265, 6626, 6629, 6630, 24145, 24681, 24683/*, 231190, 231191, 231192, 231193, 231194, 231195, 231196, 231197, 231198, 231199*/ }.Contains(s.IssuerID)
               join o in db.tObjClsRelations.Where(p => p.ObjClassifierID == 791) on s.IssuerID equals o.ObjectID
               from r in db.tRates.Where(p => p.SecurityID == s.SecurityID && p.RawDataProviderID == 1 && p.TradeSystemID == 446 && p.ActualizationDateTime <= DateTime.Today).OrderByDescending(p => p.ActualizationDateTime).Take(1)
               from rp in db.tRates.Where(p => p.SecurityID == s.SecurityID && p.RawDataProviderID == 1 && p.TradeSystemID == 446 && p.ActualizationDateTime < r.ActualizationDateTime).OrderByDescending(p => p.ActualizationDateTime).Take(1)
               orderby s.IssuerID
               select new
               {
                 Name = o.Comment,
                 Date = r.ActualizationDateTime,
                 SCHA = r.ValuePrice,
                 PricePai = r.CourseCurrent,
                 SCHAP = rp.ValuePrice,
                 PricePaiP = rp.CourseCurrent
               }).ToList().Select(p => new
               {
                 name = p.Name.Replace("УРАЛСИБ ", ""),
                 date = p.Date.ToShortDateString(),
                 scha = string.Format("{0:N2}", p.SCHA),
                 schad = Math.Round(((p.SCHA / p.SCHAP - 1) * 100).Value, 3, MidpointRounding.AwayFromZero),
                 pricePai = string.Format("{0:N2}", p.PricePai),
                 pricePaiD = Math.Round(((p.PricePai / p.PricePaiP - 1) * 100).Value, 3, MidpointRounding.AwayFromZero)
               });

      return q;
    }

    public IEnumerable<dynamic> GetPifers(string q)
    {
      var q1 = from fi in db.tFinancialInstitutions
               join fgf in db.tFinancialInstitutionGroupFinancialInstitutions.Where(p => p.FinancialInstitutionGroupID == 66) on fi.FinancialInstitutionID equals fgf.FinancialInstitutionID
               join fia in db.tFinInstAttrs on fi.FinancialInstitutionID equals fia.FinInstID
               where fi.NameBrief.StartsWith(q) || fi.Name.StartsWith(q)
               select new
               {
                 id = fi.FinancialInstitutionID,
                 brief = fi.NameBrief.Trim(),
                 fam = fi.Name.Trim(),
                 im = fi.NameLat.Trim(),
                 ot = fi.NameState.Trim(),
                 bd = fia.BirthDay != null ? fia.BirthDay.Value.ToShortDateString() : ""
               };
      return q1.Take(10);
    }

    public IEnumerable<dynamic> GetPiferRest(int id, DateTime? d1, DateTime? d2)
    {
      var ld = db.taLibs.First(p => p.LID == 482562).LDate1;
      d2 = d2.HasValue ? d2.Value.ToLocalTime() : ld;
      if (d2 > ld) d2 = ld;
      if (d1.HasValue)
        d1 = d1.Value.ToLocalTime();
      if (d1.HasValue && ld >= d1)
      {
        var q1 = from t in
                   (
                     from dc in db.tDictionariesConnections.Where(p => p.CompositeID == id && p.Dictionary == 741604640)
                     join o in db.pOrders.Where(p => !new string[] { "Отказ", "ОбмВВ", "ВВ" }.Contains(p.nodeBrief) && p.Num > 0 && p.InstrumentID != 10000000338 && (p.DealType == 0 || p.InstrumentID == 10000001350 ? p.ContrDate : p.DealDate) <= d2) on dc.DiasoftBOID equals o.ClientID
                     join dc1 in db.tDictionariesConnections.Where(p => p.Dictionary == 1104993180) on o.SecurityID equals dc1.DiasoftBOID
                     from rp in db.tRates.Where(p => o.Qty == 0 && p.SecurityID == dc1.CompositeID && p.RawDataProviderID == 1 && p.TradeSystemID == 446 && p.ActualizationDateTime == o.ContrDate).DefaultIfEmpty()
                     group new
                     {
                       NumI = (o.DealType == 0 || o.InstrumentID == 10000001350 ? o.ContrDate : o.DealDate) < d1 ? o.Num * (o.DealType * 2 - 1) : 0,
                       NumO = o.Num * (o.DealType * 2 - 1),
                       Qty = (o.DealType == 0 || o.InstrumentID == 10000001350 ? o.ContrDate : o.DealDate) >= d1 ? (Double)(((o.Qty == 0 ? (o.Num) * (decimal)(rp.CourseCurrent ?? 0) : o.Qty) - (o.DealType == 0 ? o.Commission + o.Tax : 0m)) * (1 - o.DealType * 2)) : 0
                     } by dc1.CompositeID into grp
                     select new
                     {
                       SecurityID = grp.Key,
                       NumI = grp.Sum(t => t.NumI),
                       NumO = grp.Sum(t => t.NumO),
                       Qty = grp.Sum(t => t.Qty)
                     }
                   )
                 join s in db.tSecurities on t.SecurityID equals s.SecurityID
                 from rp in db.tRates.Where(p => p.SecurityID == s.SecurityID && p.RawDataProviderID == 1 && p.TradeSystemID == 446 && p.ActualizationDateTime <= d2).OrderByDescending(p => p.ActualizationDateTime).Take(1).DefaultIfEmpty()
                 from rpi in db.tRates.Where(p => p.SecurityID == s.SecurityID && p.RawDataProviderID == 1 && p.TradeSystemID == 446 && p.ActualizationDateTime < d1).OrderByDescending(p => p.ActualizationDateTime).Take(1).DefaultIfEmpty()
                 select new
                 {
                   id = s.SecurityID,
                   name = s.NameBrief.Trim(),
                   num = t.NumO,
                   course = rp.CourseCurrent ?? 0,
                   //qty = t.NumO * (decimal?)(rp.CourseCurrent ?? 0),
                   qty = Math.Round((t.NumO ?? 0) * (decimal)(rp.CourseCurrent ?? 0), 2, MidpointRounding.AwayFromZero),
                   PnL = Math.Round(-(t.NumI ?? 0) * (decimal)(rpi.CourseCurrent ?? 0) + (t.NumO ?? 0) * (decimal)(rp.CourseCurrent ?? 0) + (decimal)t.Qty, 2, MidpointRounding.AwayFromZero)
                   //PnL = -t.NumI * (decimal?)(rpi.CourseCurrent ?? 0) + t.NumO * (decimal?)(rp.CourseCurrent ?? 0) + (decimal?)t.Qty
                 };
        return q1;
      }
      else
      {
        var q1 = from t in
                   (
                     from dc in db.tDictionariesConnections.Where(p => p.CompositeID == id && p.Dictionary == 741604640)
                     join o in db.pOrders.Where(p => !new string[] { "Отказ", "ОбмВВ", "ВВ" }.Contains(p.nodeBrief) && p.Num > 0 && p.InstrumentID != 10000000338 && (p.DealType == 0 || p.InstrumentID == 10000001350 ? p.ContrDate : p.DealDate) <= d2) on dc.DiasoftBOID equals o.ClientID
                     join dc1 in db.tDictionariesConnections.Where(p => p.Dictionary == 1104993180) on o.SecurityID equals dc1.DiasoftBOID
                     from rp in db.tRates.Where(p => o.Qty == 0 && p.SecurityID == dc1.CompositeID && p.RawDataProviderID == 1 && p.TradeSystemID == 446 && p.ActualizationDateTime == o.ContrDate).DefaultIfEmpty()
                     group new
                     {
                       Num = o.Num * (o.DealType * 2 - 1),
                       Qty = (Double)(((o.Qty == 0 ? (o.Num) * (decimal)(rp.CourseCurrent ?? 0) : o.Qty) - (o.DealType == 0 ? o.Commission + o.Tax : 0m)) * (1 - o.DealType * 2))
                     } by dc1.CompositeID into grp
                     select new
                     {
                       SecurityID = grp.Key,
                       Num = grp.Sum(t => t.Num),
                       Qty = grp.Sum(t => t.Qty)
                     }
                   )
                 join s in db.tSecurities on t.SecurityID equals s.SecurityID
                 from rp in db.tRates.Where(p => p.SecurityID == s.SecurityID && p.RawDataProviderID == 1 && p.TradeSystemID == 446 && p.ActualizationDateTime <= d2).OrderByDescending(p => p.ActualizationDateTime).Take(1).DefaultIfEmpty()
                 select new
                 {
                   id = s.SecurityID,
                   name = s.NameBrief.Trim(),
                   num = t.Num,
                   course = rp.CourseCurrent ?? 0,
                   qty = Math.Round((t.Num ?? 0) * (decimal)(rp.CourseCurrent ?? 0), 2, MidpointRounding.AwayFromZero),
                   PnL = Math.Round((t.Num ?? 0) * (decimal)(rp.CourseCurrent ?? 0) + (decimal)t.Qty, 2, MidpointRounding.AwayFromZero)
                 };
        return q1;
      }
    }

    internal class TYield { public DateTime Date { get; set; } public double Qty { get; set; } }
    public object GetPiferYield(int id, DateTime? d1, DateTime? d2)
    {
      var trx = new List<TYield>();
      var ld = db.taLibs.First(p => p.LID == 482562).LDate1;
      d2 = d2.HasValue ? d2.Value.ToLocalTime() : ld;
      if (d2 > ld) d2 = ld;
      if (d1.HasValue)
        d1 = d1.Value.ToLocalTime();
      if (d1.HasValue && ld >= d1)
      {
        trx.AddRange(from t in
                       (
                       from t in
                         (
                           from dc in db.tDictionariesConnections.Where(p => p.CompositeID == id && p.Dictionary == 741604640)
                           join o in db.pOrders.Where(p => !new string[] { "Отказ", "ОбмВВ", "ВВ" }.Contains(p.nodeBrief) && p.Num > 0 && p.InstrumentID != 10000000338 && (p.DealType == 0 || p.InstrumentID == 10000001350 ? p.ContrDate : p.DealDate) < d1) on dc.DiasoftBOID equals o.ClientID
                           group new { o.SecurityID, Num = o.Num * (1 - o.DealType * 2) } by o.SecurityID into grp
                           select new { SecurityID = grp.Key, Num = grp.Sum(t => t.Num) }
                           )
                       join dc in db.tDictionariesConnections.Where(p => p.Dictionary == 1104993180) on t.SecurityID equals dc.DiasoftBOID
                       from rp in db.tRates.Where(p => p.SecurityID == dc.CompositeID && p.RawDataProviderID == 1 && p.TradeSystemID == 446 && p.ActualizationDateTime < d1).OrderByDescending(p => p.ActualizationDateTime).Take(1).DefaultIfEmpty()
                       select new
                       {
                         Qty = (Double)t.Num * rp.CourseCurrent ?? 0
                       })
                     group t by 1 into grp
                     select new TYield { Date = d1.Value, Qty = grp.Sum(p => p.Qty) });

        trx.AddRange(from t in
                       (
                         from dc in db.tDictionariesConnections.Where(p => p.CompositeID == id && p.Dictionary == 741604640)
                         join o in db.pOrders.Where(p => !new string[] { "Отказ", "ОбмВВ", "ВВ" }.Contains(p.nodeBrief) && p.Num > 0 && p.InstrumentID != 10000000338 && (p.DealType == 0 || p.InstrumentID == 10000001350 ? p.ContrDate : p.DealDate) >= d1 && (p.DealType == 0 || p.InstrumentID == 10000001350 ? p.ContrDate : p.DealDate) <= d2) on dc.DiasoftBOID equals o.ClientID
                         join dc1 in db.tDictionariesConnections.Where(p => p.Dictionary == 1104993180) on o.SecurityID equals dc1.DiasoftBOID
                         from rp in db.tRates.Where(p => o.Qty == 0 && p.SecurityID == dc1.CompositeID && p.RawDataProviderID == 1 && p.TradeSystemID == 446 && p.ActualizationDateTime == o.ContrDate).DefaultIfEmpty()
                         select new
                         {
                           Date = (DateTime)(o.DealType == 0 || o.InstrumentID == 10000001350 ? o.ContrDate : o.DealDate),
                           Qty = (Double)(((o.Qty == 0 ? (o.Num) * (decimal)(rp.CourseCurrent ?? 0) : o.Qty) - (o.DealType == 0 ? o.Commission + o.Tax : 0m)) * (1 - o.DealType * 2))
                         })
                     group t by t.Date into grp
                     orderby grp.Key
                     select new TYield { Date = grp.Key, Qty = grp.Sum(p => p.Qty) });
      }
      else
      {
        trx.AddRange(from t in
                       (
                         from dc in db.tDictionariesConnections.Where(p => p.CompositeID == id && p.Dictionary == 741604640)
                         join o in db.pOrders.Where(p => !new string[] { "Отказ", "ОбмВВ", "ВВ" }.Contains(p.nodeBrief) && p.Num > 0 && p.InstrumentID != 10000000338 && (p.DealType == 0 || p.InstrumentID == 10000001350 ? p.ContrDate : p.DealDate) <= d2) on dc.DiasoftBOID equals o.ClientID
                         join dc1 in db.tDictionariesConnections.Where(p => p.Dictionary == 1104993180) on o.SecurityID equals dc1.DiasoftBOID
                         from rp in db.tRates.Where(p => o.Qty == 0 && p.SecurityID == dc1.CompositeID && p.RawDataProviderID == 1 && p.TradeSystemID == 446 && p.ActualizationDateTime == o.ContrDate).DefaultIfEmpty()
                         select new
                         {
                           Date = (DateTime)(o.DealType == 0 || o.InstrumentID == 10000001350 ? o.ContrDate : o.DealDate),
                           Qty = (Double)(((o.Qty == 0 ? (o.Num) * (decimal)(rp.CourseCurrent ?? 0) : o.Qty) - (o.DealType == 0 ? o.Commission + o.Tax : 0m)) * (1 - o.DealType * 2))
                         })
                     group t by t.Date into grp
                     orderby grp.Key
                     select new TYield { Date = grp.Key, Qty = grp.Sum(p => p.Qty) });
      }
      trx.AddRange(from t in
                     (
                       from t in
                         (
                           from dc in db.tDictionariesConnections.Where(p => p.CompositeID == id && p.Dictionary == 741604640)
                           join o in db.pOrders.Where(p => !new string[] { "Отказ", "ОбмВВ", "ВВ" }.Contains(p.nodeBrief) && p.Num > 0 && p.InstrumentID != 10000000338 && (p.DealType == 0 || p.InstrumentID == 10000001350 ? p.ContrDate : p.DealDate) <= d2) on dc.DiasoftBOID equals o.ClientID
                           group new { o.SecurityID, Num = o.Num * (o.DealType * 2 - 1) } by o.SecurityID into grp
                           select new { SecurityID = grp.Key, Num = grp.Sum(t => t.Num) }
                           )
                       join dc in db.tDictionariesConnections.Where(p => p.Dictionary == 1104993180) on t.SecurityID equals dc.DiasoftBOID
                       from rp in db.tRates.Where(p => p.SecurityID == dc.CompositeID && p.RawDataProviderID == 1 && p.TradeSystemID == 446 && p.ActualizationDateTime <= d2).OrderByDescending(p => p.ActualizationDateTime).Take(1).DefaultIfEmpty()
                       select new
                       {
                         Date = (DateTime)d2,
                         Qty = (Double)t.Num * rp.CourseCurrent ?? 0
                       })
                   group t by t.Date into grp
                   orderby grp.Key
                   select new TYield { Date = grp.Key, Qty = grp.Sum(p => p.Qty) });

      var v = new List<double>();
      var dl = new List<DateTime>();
      foreach (var e in trx.Where(p => p.Qty != 0))
      {
        v.Add(Math.Round(e.Qty, 2));
        dl.Add(e.Date);
      }
      var y = 0.0;
      var ya = 0.0;
      var Success = true;
      try
      {
        y = Excel.FinancialFunctions.Financial.XIrr(v, dl);
        ya = Math.Pow(y + 1, ((double)(dl.Max() - dl.Min()).TotalDays) / 365) - 1;
      }
      catch
      {
        Success = false;
      }
      return new { Yield = (decimal)y * 100, Yielda = (decimal)ya * 100, Success };
    }

    public object GetPiferFondYield(int id, int secId, DateTime? d1, DateTime? d2)
    {
      var trx = new List<TYield>();
      var ld = db.taLibs.First(p => p.LID == 482562).LDate1;
      d2 = d2.HasValue ? d2.Value.ToLocalTime() : ld;
      if (d2 > ld) d2 = ld;
      if (d1.HasValue)
        d1 = d1.Value.ToLocalTime();
      if (d1.HasValue && ld >= d1)
      {
        trx.AddRange(from t in
                       (
                     from t in
                       (
                     from dc1 in db.tDictionariesConnections.Where(p => p.CompositeID == id && p.Dictionary == 741604640)
                     from dc2 in db.tDictionariesConnections.Where(p => p.CompositeID == secId && p.Dictionary == 1104993180)
                     join o in db.pOrders.Where(p => !new string[] { "Отказ", "ОбмВВ", "ВВ" }.Contains(p.nodeBrief) && p.Num > 0 && p.InstrumentID != 10000000338 && (p.DealType == 0 || p.InstrumentID == 10000001350 ? p.ContrDate : p.DealDate) < d1) on new { ClientID = dc1.DiasoftBOID, SecurityID = dc2.DiasoftBOID } equals new { o.ClientID, o.SecurityID }
                     group new { Num = o.Num * (1 - o.DealType * 2) } by dc2.CompositeID into grp
                     select new { Num = grp.Sum(t => t.Num) }
                     )
                     from rp in db.tRates.Where(p => p.SecurityID == secId && p.RawDataProviderID == 1 && p.TradeSystemID == 446 && p.ActualizationDateTime < d1).OrderByDescending(p => p.ActualizationDateTime).Take(1).DefaultIfEmpty()
                     select new
                     {
                       Qty = (Double)t.Num * rp.CourseCurrent ?? 0
                     })
                     group t by 1 into grp
                     select new TYield { Date = d1.Value, Qty = grp.Sum(p => p.Qty) });

        trx.AddRange(from t in
                       (
                         from dc1 in db.tDictionariesConnections.Where(p => p.CompositeID == id && p.Dictionary == 741604640)
                         from dc2 in db.tDictionariesConnections.Where(p => p.CompositeID == secId && p.Dictionary == 1104993180)
                         join o in db.pOrders.Where(p => !new string[] { "Отказ", "ОбмВВ", "ВВ" }.Contains(p.nodeBrief) && p.Num > 0 && p.InstrumentID != 10000000338 && (p.DealType == 0 || p.InstrumentID == 10000001350 ? p.ContrDate : p.DealDate) >= d1 && (p.DealType == 0 || p.InstrumentID == 10000001350 ? p.ContrDate : p.DealDate) <= d2) on new { ClientID = dc1.DiasoftBOID, SecurityID = dc2.DiasoftBOID } equals new { o.ClientID, o.SecurityID }
                         from rp in db.tRates.Where(p => o.Qty == 0 && p.SecurityID == dc2.CompositeID && p.RawDataProviderID == 1 && p.TradeSystemID == 446 && p.ActualizationDateTime == o.ContrDate).DefaultIfEmpty()
                         select new
                         {
                           Date = (DateTime)(o.DealType == 0 || o.InstrumentID == 10000001350 ? o.ContrDate : o.DealDate),
                           Qty = (Double)(((o.Qty == 0 ? (o.Num) * (decimal)(rp.CourseCurrent ?? 0) : o.Qty) - (o.DealType == 0 ? o.Commission + o.Tax : 0m)) * (1 - o.DealType * 2))
                         })
                     group t by t.Date into grp
                     orderby grp.Key
                     select new TYield { Date = grp.Key, Qty = grp.Sum(p => p.Qty) });
      }
      else
      {
        trx.AddRange(from t in
                       (
                         from dc1 in db.tDictionariesConnections.Where(p => p.CompositeID == id && p.Dictionary == 741604640)
                         from dc2 in db.tDictionariesConnections.Where(p => p.CompositeID == secId && p.Dictionary == 1104993180)
                         join o in db.pOrders.Where(p => !new string[] { "Отказ", "ОбмВВ", "ВВ" }.Contains(p.nodeBrief) && p.Num > 0 && p.InstrumentID != 10000000338 && (p.DealType == 0 || p.InstrumentID == 10000001350 ? p.ContrDate : p.DealDate) <= d2) on new { ClientID = dc1.DiasoftBOID, SecurityID = dc2.DiasoftBOID } equals new { o.ClientID, o.SecurityID }
                         from rp in db.tRates.Where(p => o.Qty == 0 && p.SecurityID == dc1.CompositeID && p.RawDataProviderID == 1 && p.TradeSystemID == 446 && p.ActualizationDateTime == o.ContrDate).DefaultIfEmpty()
                         select new
                         {
                           Date = (DateTime)(o.DealType == 0 || o.InstrumentID == 10000001350 ? o.ContrDate : o.DealDate),
                           Qty = (Double)(((o.Qty == 0 ? (o.Num) * (decimal)(rp.CourseCurrent ?? 0) : o.Qty) - (o.DealType == 0 ? o.Commission + o.Tax : 0m)) * (1 - o.DealType * 2))
                         })
                     group t by t.Date into grp
                     orderby grp.Key
                     select new TYield { Date = grp.Key, Qty = grp.Sum(p => p.Qty) });
      }
      trx.AddRange(from t in
                     (
                       from t in
                         (
                           from dc1 in db.tDictionariesConnections.Where(p => p.CompositeID == id && p.Dictionary == 741604640)
                           from dc2 in db.tDictionariesConnections.Where(p => p.CompositeID == secId && p.Dictionary == 1104993180)
                           join o in db.pOrders.Where(p => !new string[] { "Отказ", "ОбмВВ", "ВВ" }.Contains(p.nodeBrief) && p.Num > 0 && p.InstrumentID != 10000000338 && (p.DealType == 0 || p.InstrumentID == 10000001350 ? p.ContrDate : p.DealDate) <= d2) on new { ClientID = dc1.DiasoftBOID, SecurityID = dc2.DiasoftBOID } equals new { o.ClientID, o.SecurityID }
                           group new { Num = o.Num * (o.DealType * 2 - 1) } by dc2.CompositeID into grp
                           select new { SecurityID = grp.Key, Num = grp.Sum(t => t.Num) }
                           )
                       from rp in db.tRates.Where(p => p.SecurityID == t.SecurityID && p.RawDataProviderID == 1 && p.TradeSystemID == 446 && p.ActualizationDateTime <= d2).OrderByDescending(p => p.ActualizationDateTime).Take(1).DefaultIfEmpty()
                       select new
                       {
                         Date = (DateTime)d2,
                         Qty = (Double)t.Num * rp.CourseCurrent ?? 0
                       })
                   group t by t.Date into grp
                   orderby grp.Key
                   select new TYield { Date = grp.Key, Qty = grp.Sum(p => p.Qty) });

      var v = new List<double>();
      var dl = new List<DateTime>();
      foreach (var e in trx.Where(p => p.Qty != 0))
      {
        v.Add(Math.Round(e.Qty, 2));
        dl.Add(e.Date);
      }
      var y = 0.0;
      var ya = 0.0;
      var Success = true;
      try
      {
        y = Excel.FinancialFunctions.Financial.XIrr(v, dl);
        ya = Math.Pow(y + 1, ((double)(dl.Max() - dl.Min()).TotalDays) / 365) - 1;
      }
      catch
      {
        Success = false;
      }
      return new { Yield = (decimal)y * 100, Yielda = (decimal)ya * 100, Success };
    }

    public IEnumerable<dynamic> GetPiferOrders(int id, int secId)
    {
      var q1 = from dc1 in db.tDictionariesConnections.Where(p => p.CompositeID == id && p.Dictionary == 741604640)
               from dc2 in db.tDictionariesConnections.Where(p => p.CompositeID == secId && p.Dictionary == 1104993180)
               join o in db.pOrders.Where(p => !new string[] { "Отказ", "ОбмВВ", "ВВ" }.Contains(p.nodeBrief) && p.Num > 0 && p.InstrumentID != 10000000338) on new { ClientID = dc1.DiasoftBOID, SecurityID = dc2.DiasoftBOID } equals new { o.ClientID, o.SecurityID }
               join s in db.tSecurities on dc2.CompositeID equals s.SecurityID
               orderby o.ContrDate ascending
               select new
               {
                 id = o.DealID,
                 sid = s.SecurityID,
                 name = s.NameBrief.Trim(),
                 dd = o.DealDate,
                 cd = o.ContrDate,
                 num = o.Num,
                 price = o.DealPrice,
                 qty = o.Qty,
                 ppz = o.PPZCode,
                 dt = o.DealType,
                 instr = o.instrBrief,
                 node = o.nodeBrief,
                 commission = o.Commission,
                 tax = o.Tax,
                 number = o.Number,
                 seller = o.Seller
               };

      return q1;
    }

    public ISingleResult<up_avgGetPiferGraphResult> GetPiferGraph(int id)
    {
      return db.up_avgGetPiferGraph(id);
    }

    public ISingleResult<up_avgGetPiferGraph3Result> GetPiferGraph3(int id, DateTime? d1, DateTime? d2)
    {
      return db.up_avgGetPiferGraph3(id, db: d1, de: d2);
    }

    public IEnumerable<dynamic> GetCourses()
    {
      var r = from q in
                (
                  (from q in db.tQUIK_Rates
                   where new string[] { "GAZP", "GMKN", "SBER", "ROSN", "NLMK", "LKOH", "VTBR", "RTKM", "DSKY" }.Contains(q.NumberSecurity)
                   select new
                   {
                     sec = q.NameBrief,
                     cl = q.CourseLast,
                     cc = q.CourseClose,
                     chg = q.CourseClose != 0 && q.CourseLast != 0 ? (q.CourseLast * 100 / q.CourseClose - 100) : 0,
                     tm = q.TimeCourse/*.Value.ToString("{0:hh:nn}")*/,
                     q.Precision1
                   }).ToList())
              select new
              {
                q.sec,
                q.cc,
                cl = q.cl.Value.ToString(string.Format("n{0}", q.Precision1)),
                chg = q.chg.Value,
                tm = (q.tm ?? (DateTime?)DateTime.Today).Value.ToString("HH:mm", new CultureInfo("ru-RU", true))
              };
      return r;
    }

    public IQueryable<dynamic> GetKladr(int? id, string q, int limit, string Code1)
    {
      if (id == 1)
      {
        return db.tKladrKLADRs.Where(k => SqlMethods.Like(k.CODE, "__00000000000") && k.NAME.StartsWith(q)).Select(k => new { k.CODE, k.NAME, k.SOCR });
      }
      else if (id == 2)
      {
        return db.tKladrKLADRs.Where(k => SqlMethods.Like(k.CODE, Code1 + "___00000000") && !SqlMethods.Like(k.CODE, Code1 + "00000000000") && k.NAME.StartsWith(q)).Select(k => new { k.CODE, k.NAME, k.SOCR });
      }
      else if (id == 3)
      {
        return db.tKladrKLADRs.Where(k => SqlMethods.Like(k.CODE, Code1 + "___00000") && !SqlMethods.Like(k.CODE, Code1 + "00000000") && k.NAME.StartsWith(q)).Select(k => new { k.CODE, k.NAME, k.SOCR });
      }
      else if (id == 4)
      {
        return db.tKladrKLADRs.Where(k => SqlMethods.Like(k.CODE, Code1 + "___00") && !SqlMethods.Like(k.CODE, Code1 + "00000") && k.NAME.StartsWith(q)).Select(k => new { k.CODE, k.NAME, k.SOCR });
      }
      return null;
    }

    public IQueryable<dynamic> GetKladr1(int? id, string Code1)
    {
      if (id == 1)
      {
        return db.tKladrKLADRs.Where(k => SqlMethods.Like(k.CODE, "__00000000000")).Select(k => new { k.CODE, k.NAME, k.SOCR });
      }
      else if (id == 2)
      {
        return db.tKladrKLADRs.Where(k => SqlMethods.Like(k.CODE, Code1 + "___00000000") && !SqlMethods.Like(k.CODE, Code1 + "00000000000")).Select(k => new { k.CODE, k.NAME, k.SOCR });
      }
      else if (id == 3)
      {
        return db.tKladrKLADRs.Where(k => SqlMethods.Like(k.CODE, Code1 + "___00000") && !SqlMethods.Like(k.CODE, Code1 + "00000000")).Select(k => new { k.CODE, k.NAME, k.SOCR });
      }
      else if (id == 4)
      {
        return db.tKladrKLADRs.Where(k => SqlMethods.Like(k.CODE, Code1 + "___00") && !SqlMethods.Like(k.CODE, Code1 + "00000")).Select(k => new { k.CODE, k.NAME, k.SOCR });
      }
      return null;
    }

    public IEnumerable<dynamic> FinActProfitList(string sort, string dir, int? y)
    {
      var q1 = from o in db.tObjClassifiers.Where(p => p.ParentID == 5232)
               from q in db.tObjClassifiers.Where(p => p.ObjClassifierID >= 1 && p.ObjClassifierID <= 4).Select(p => new { q = p.ObjClassifierID })
               from c in db.tFinActProfits.Where(p => p.ObjClsID == o.ObjClassifierID && p.An == y && p.Quartier == q.q).DefaultIfEmpty()
               select new
               {
                 c.ID,
                 ObjClsID = o.ObjClassifierID,
                 o.Name,
                 Quartier = q.q,
                 An = y,
                 c.Value
               };

      if (sort == null)
      {
        return q1.OrderBy(p => p.ObjClsID);
      }
      return q1.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
    }

    public IEnumerable<dynamic> FinActProfitUpdate(List<tFinActProfit> data)
    {
      if (data.Count() > 0)
      {
        data[0].InDateTime = DateTime.Now;
        if (data[0].ID > 0)
        {
          var r = db.tFinActProfits.Where(o => o.ID == data[0].ID).FirstOrDefault();
          if (r != null)
          {
            if (data[0].Value.HasValue)
            {
              r.Value = data[0].Value;
              r.InDateTime = data[0].InDateTime;
            }
            else
              db.tFinActProfits.DeleteOnSubmit(r);
            db.SubmitChanges();
          }
        }
        else
        {
          tFinActProfit o = data[0];
          db.tFinActProfits.InsertOnSubmit(o);
          db.SubmitChanges();
        }
      }
      var q1 = from o in db.tObjClassifiers.Where(p => p.ObjClassifierID == data[0].ObjClsID)
               from c in db.tFinActProfits.Where(p => p.ObjClsID == o.ObjClassifierID && p.An == data[0].An && p.Quartier == data[0].Quartier).DefaultIfEmpty()
               select new
               {
                 c.ID,
                 ObjClsID = o.ObjClassifierID,
                 o.Name,
                 Quartier = data[0].Quartier,
                 An = data[0].An,
                 c.Value
               };

      return q1;
    }

    public IEnumerable<dynamic> GetPIFerRest(DateTime? d, int? FinInstID, string clName)
    {
      var q1 = db.tSecurities.Where(s => s.SecType == 6 && s.IsDeleted == 0);
      if (FinInstID.HasValue)
        q1 = q1.Where(s => s.IssuerID == FinInstID);
      var q = from s in q1
              join r in db.tPIFerRests.Where(p => p.RestDate == d) on s.SecurityID equals r.SecurityID
              join a in db.tPIFerResources on r.ResourceID equals a.ResourceID
              join ocr in db.tObjClsRelations.Where(p => p.ObjClassifierID == 791 && p.ObjType == 741604640) on s.IssuerID equals ocr.ObjectID
              join fi in db.tFinancialInstitutions.Where(p => p.NameBrief.Contains(clName)) on r.FinInstID equals fi.FinancialInstitutionID
              from sr in db.tRates.Where(rt => rt.RawDataProviderID == 1 && rt.TradeSystemID == 446 && rt.ActualizationDateTime == r.RestDate && rt.SecurityID == r.SecurityID).DefaultIfEmpty()
              select new
              {
                r.FinInstID,
                Name = fi.IsJuridicalPerson == 0 ? fi.Name + " " + fi.NameLat + (fi.NameState != "" ? " " + fi.NameState : "") : fi.Name,
                Name1 = s.Name1,
                Acc = a.Brief,
                r.RestDate,
                r.Rest,
                sr.CourseCurrent,
                Qty = sr.CourseCurrent * (double?)r.Rest
              };
      return q;
    }

    public ISingleResult<up_avgGetPIFRestStatResult> GetPIFRestStat(DateTime? d)
    {
      db.CommandTimeout = 3 * 60;
      return db.up_avgGetPIFRestStat(d);
    }

    public ISingleResult<up_avgGetPIFRestsResult> GetPIFRests(DateTime? d, int? FinInstID, string clName)
    {
      db.CommandTimeout = 3 * 60;
      return db.up_avgGetPIFRests(d, FinInstID, clName);
      //var res = from r in db.tPIFerRests.Where(p => p.RestDate == d)
      //          join s in db.tSecurities on r.SecurityID equals s.SecurityID
      //          join ocr in db.tObjClsRelations.Where(p => p.ObjClassifierID == 791 && p.ObjType == 741604640) on s.IssuerID equals ocr.ObjectID
      //          join fi in db.tFinancialInstitutions on r.FinInstID equals fi.FinancialInstitutionID
      //          join fs in db.tFinancialInstitutions on s.IssuerID equals fs.FinancialInstitutionID
      //          join fia in db.tFinInstAttrs on r.FinInstID equals fia.FinInstID into fia1
      //          from fia in fia1.DefaultIfEmpty()
      //          join sr in db.tRates.Where(rt => rt.RawDataProviderID == 1 && rt.TradeSystemID == 446 && rt.ActualizationDateTime == d) on s.SecurityID equals sr.SecurityID into _sr
      //          from sr in _sr.DefaultIfEmpty()
      //          join dc1 in db.tDictionariesConnections.Where(p => p.Dictionary == 1104993180) on r.SecurityID equals dc1.CompositeID
      //          join dc3 in db.tDictionariesConnections.Where(p => p.Dictionary == 741604640) on r.FinInstID equals dc3.CompositeID
      //          from o in db.pOrders.Where(p => dc3.DiasoftBOID == p.ClientID && dc1.DiasoftBOID == p.SecurityID && p.PPZCode != null && p.ContrDate <= r.RestDate).OrderBy(p => p.ContrDate).Take(1).DefaultIfEmpty()
      //          //join qa in
      //          //  (from q1 in
      //          //     (
      //          //       from ocr in db.tObjClsRelations.Where(p => p.ObjClassifierID == 791)
      //          //       join s in db.tSecurities.Where(p => p.IsDeleted == 0 && p.SecType == 6) on ocr.ObjectID equals s.IssuerID
      //          //       join r in db.tPIFerRests.Where(p => p.RestDate == d) on s.SecurityID equals r.SecurityID
      //          //       from sr in db.tRates.Where(rt => rt.RawDataProviderID == 1 && rt.TradeSystemID == 446 && rt.ActualizationDateTime == r.RestDate && rt.SecurityID == r.SecurityID).DefaultIfEmpty()
      //          //       select new { r.FinInstID, Qty = sr.CourseCurrent * (double?)r.Rest }
      //          //       )
      //          //   group q1 by q1.FinInstID into grp
      //          //   select new
      //          //   {
      //          //     FinInstID = grp.Key,
      //          //     Qty = grp.Sum(t => t.Qty)
      //          //   }) on r.FinInstID equals qa.FinInstID
      //          select new
      //          {
      //            r.FinInstID,
      //            r.SecurityID,
      //            s.IssuerID,
      //            Brief = fi.NameBrief.TrimEnd(),
      //            Name = fi.IsJuridicalPerson == 0 ? fi.Name + " " + fi.NameLat + (fi.NameState != "" ? " " + fi.NameState : "") : fi.Name,
      //            fia.BirthDay,
      //            PIFBrief = fs.NameBrief.TrimEnd(),
      //            ContrDate = o.ContrDate,
      //            r.RestDate,
      //            r.Rest,
      //            sr.CourseCurrent,
      //            Qty = sr.CourseCurrent * (double?)r.Rest,
      //            PortQty = 0//qa.Qty
      //          };
      //if (FinInstID != null)
      //{
      //  res = res.Where(p => p.IssuerID == FinInstID);
      //}

      //return res.OrderBy(p => p.SecurityID).ThenBy(p => p.Name);
    }

    public ISingleResult<up_avgRepPIFOrdersResult> RepPIFOrders(DateTime? d)
    {
      db.CommandTimeout = 3 * 60;
      return db.up_avgRepPIFOrders(d);
    }

    public IEnumerable<dynamic> GetPIFList()
    {
      var q = db.tObjClsRelations
        .Where(p => p.ObjClassifierID == 791)
        .Join(db.tFinancialInstitutions, p => p.ObjectID, f => f.FinancialInstitutionID, (p, f) => new { id = p.ObjectID, name = p.Comment, brief = f.NameBrief.Trim() })
        .OrderBy(p => p.id);
      return q;
    }

    public IEnumerable<dynamic> FinInOutComeList(string sort, string dir, int? an)
    {
      var q = from t in db.tTreaties.Where(t => t.IsDisabled == 0 && t.DateFinish == new DateTime(1900, 1, 1))
              join ttt in db.tTreatyTreatyTypes.Where(i => i.TreatyTypeID == 1 || i.TreatyTypeID == 2) on t.TreatyID equals ttt.TreatyID
              join fi in db.tFinancialInstitutions on t.FinancialInstitutionID equals fi.FinancialInstitutionID
              join io in db.tFinInOutComes.Where(f => f.An == an) on t.TreatyID equals io.TreatyID into io1
              from io in io1.DefaultIfEmpty()
              select new { t.TreatyID, trName = t.Name.Trim(), fiName = fi.NameBrief.Trim(), io.ID, io.Value1, io.Value2, io.Value3, io.Value4, An = an };
      return q;
    }

    public IEnumerable<dynamic> FinInOutComeUpdate(List<tFinInOutCome> data)
    {
      if (data.Count() > 0)
      {
        data[0].InDateTime = DateTime.Now;
        if (data[0].ID > 0)
        {
          var r = db.tFinInOutComes.Where(o => o.ID == data[0].ID).FirstOrDefault();
          if (r != null)
          {
            if (data[0].Value1.HasValue || data[0].Value2.HasValue || data[0].Value3.HasValue || data[0].Value4.HasValue)
            {
              r.Value1 = data[0].Value1;
              r.Value2 = data[0].Value2;
              r.Value3 = data[0].Value3;
              r.Value4 = data[0].Value4;
              r.InDateTime = data[0].InDateTime;
            }
            else
              db.tFinInOutComes.DeleteOnSubmit(r);
            db.SubmitChanges();
          }
        }
        else
        {
          tFinInOutCome o = data[0];
          db.tFinInOutComes.InsertOnSubmit(o);
          db.SubmitChanges();
        }
      }
      return data;
    }

    public IEnumerable<dynamic> FinPartList(string sort, string dir, DateTime? dt)
    {
      var q1 = from t in
                 (
                   from t in
                     (
                       from t in db.tTreaties.Where(t => t.DateFinish == new DateTime(1900, 1, 1) && !(t.FinancialInstitutionID >= 24924 && t.FinancialInstitutionID <= 24929) && !db.tSecurities.Where(s => s.SecType == 6 && s.IsDeleted == 0 && s.IssuerID == t.FinancialInstitutionID).Any())
                       join ttt in db.tTreatyTreatyTypes.Where(ttt => new int[] { 1, 2 }.Contains(ttt.TreatyTypeID)) on t.TreatyID equals ttt.TreatyID
                       join a in db.tAccounts on t.TreatyID equals a.TreatyID
                       join s in db.tSecurities.Where(s => !new byte?[] { 18, 24 }.Contains(s.SecType)) on a.SecurityID equals s.SecurityID
                       join ab in db.tAccountBalances.Where(ab => ab.BalanceDate == dt) on a.AccountID equals ab.AccountID
                       from ocr in
                         (
                           (from ocr3 in db.tObjClsRelations.Where(ocr3 => s.SecType == 4 && ocr3.ObjType == 741604640 && ocr3.ObjectID == a.BalanceInstitutionID)
                            join oc3 in db.tObjClassifiers.Where(oc3 => oc3.ObjType == 741604640 && oc3.ParentID == 125) on ocr3.ObjClassifierID equals oc3.ObjClassifierID
                            select new { ocr3.ObjClsRelationID, oc3.UniqueFlag }).Take(1).DefaultIfEmpty()
                           )
                       join ocr1 in db.tObjClsRelations.Where(ocr1 => new int?[] { 129, 130, 139 }.Contains(ocr1.ObjClassifierID) && ocr1.ObjType == 1631275800) on a.TreatyID equals ocr1.ObjectID into _ocr1
                       from ocr1 in _ocr1.DefaultIfEmpty()
                       from r in db.tSecurityRates.Where(r => r.Date == ab.BalanceDate && r.SecurityID == (new byte?[] { 5, 18, 24 }.Contains(s.SecType) || ocr1.ObjClassifierID == 139 ? a.SecIssuerID : a.SecurityID) && r.RateType == (ocr1.ObjClassifierID == 129 ? 1 : ocr1.ObjClassifierID == 130 ? 2 : ocr1.ObjClassifierID == 139 ? 3 : 0) && r.FundID == 39191 && r.FinInstID == (ocr1.ObjClassifierID == 139 ? t.FinancialInstitutionID : 0)).Take(1).DefaultIfEmpty()
                       from sg in
                         (from sg1 in db.tSecurityGroups.Where(sg1 => sg1.SecurityGroupTypeID == 1)
                          join ssg in db.tSecuritySecurityGroups.Where(ssg => ssg.SecurityID == s.SecurityID) on sg1.SecurityGroupID equals ssg.SecurityGroupID
                          select new { sg1.SecurityGroupID }).DefaultIfEmpty()
                       join ocr4 in db.tObjClsRelations.Where(ocr4 => ocr4.ObjClassifierID == 607 && ocr4.ObjType == 1631275800) on t.TreatyID equals ocr4.ObjectID into _ocr4
                       from ocr4 in _ocr4.DefaultIfEmpty()
                       from dc in db.tDictionariesConnections.Where(dc => s.SecType == 4 && dc.Dictionary == -461522885 && dc.DiasoftBOID == ab.ID1).Take(1).DefaultIfEmpty()
                       join d in db.tDeals on dc.CompositeID equals d.DealID into _d
                       from d in _d.DefaultIfEmpty()
                       join s1 in db.tSecurities on d.SecurityID equals s1.SecurityID into _s1
                       from s1 in _s1.DefaultIfEmpty()
                       from rt in db.tRates.Where(rt => (rt.SecurityID == (d != null ? d.SecurityID : s.SecType == 2 ? (int?)a.SecurityID : null)) && rt.TradeSystemID == 16218 && rt.ActualizationDateTime < ab.BalanceDate.AddDays(1)).OrderByDescending(rt => rt.ActualizationDateTime).Take(1).DefaultIfEmpty()
                       select new
                       {
                         t.TreatyID,
                         ObjClsID = (s.SecType == 4 && ocr.UniqueFlag == 3) ? 5236 : new byte?[] { 0, 15, 6 }.Contains(s.SecType) ? 5233 : new int[] { 3, 4, 5, 6, 9, 10, 15 }.Contains(sg.SecurityGroupID) ? 5234 : s.SecType == 4 ? 5235 : 0,
                         Qty = (double?)(ocr4 == null ? ab.OutcomeBalanceF : ab.OutcomeBalanceP) * (r.Course + r.Coupon) * (rt == null ? 1 : rt.CourseCurrent / 100) * (new byte?[] { 18, 24 }.Contains(s1.SecType) || new byte?[] { 18, 24 }.Contains(s.SecType) ? 0 : 1)
                       })
                   from h in db.tObjClassifiers.Where(h => h.ParentID == 5232).Select(h => new { ObjClsID = h.ObjClassifierID })
                   select new { t.TreatyID, h.ObjClsID, Qty = h.ObjClsID == t.ObjClsID ? t.Qty : null })
               group t by new { t.TreatyID, t.ObjClsID } into grp
               select new { grp.Key.TreatyID, grp.Key.ObjClsID, Qty = grp.Sum(t => t.Qty) };

      var q2 = from q in q1
               group q by q.TreatyID into grp
               where grp.Sum(t => t.Qty) != 0
               select new { TreatyID = grp.Key, Qty = grp.Sum(t => t.Qty) };

      var q3 = from _q1 in q1
               join _q2 in q2 on _q1.TreatyID equals _q2.TreatyID
               join t in db.tTreaties on _q1.TreatyID equals t.TreatyID
               join fi in db.tFinancialInstitutions on t.FinancialInstitutionID equals fi.FinancialInstitutionID
               join oc in db.tObjClassifiers on _q1.ObjClsID equals oc.ObjClassifierID
               from p1 in db.tFinParts.Where(p => p.TreatyID == _q1.TreatyID && p.ObjClsID == _q1.ObjClsID && p.An == dt.Value.Year && p.Quartier == 1).DefaultIfEmpty()
               from p2 in db.tFinParts.Where(p => p.TreatyID == _q1.TreatyID && p.ObjClsID == _q1.ObjClsID && p.An == dt.Value.Year && p.Quartier == 2).DefaultIfEmpty()
               from p3 in db.tFinParts.Where(p => p.TreatyID == _q1.TreatyID && p.ObjClsID == _q1.ObjClsID && p.An == dt.Value.Year && p.Quartier == 3).DefaultIfEmpty()
               from p4 in db.tFinParts.Where(p => p.TreatyID == _q1.TreatyID && p.ObjClsID == _q1.ObjClsID && p.An == dt.Value.Year && p.Quartier == 4).DefaultIfEmpty()
               select new
               {
                 _q1.TreatyID,
                 _q1.ObjClsID,
                 ocName = oc.Name,
                 _q1.Qty,
                 Part = _q2.Qty != 0 ? _q1.Qty * 100 / _q2.Qty : 0,
                 trName = t.Name.Trim(),
                 fiName = fi.NameBrief.Trim(),
                 ID1 = (int?)p1.ID,
                 Value1 = p1.Value,
                 ID2 = (int?)p2.ID,
                 Value2 = p2.Value,
                 ID3 = (int?)p3.ID,
                 Value3 = p3.Value,
                 ID4 = (int?)p4.ID,
                 Value4 = p4.Value,
                 Dt = dt
               };
      if (sort == null)
      {
        return q3.OrderBy(p => p.TreatyID).ThenBy(p => p.ObjClsID);
      }
      return q3.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
    }

    public IEnumerable<dynamic> FinPartUpdate(List<FinPartViewModel> data)
    {
      if (data.Count() > 0)
      {
        if (data[0].ID1 > 0)
        {
          var r = db.tFinParts.Where(o => o.ID == data[0].ID1).FirstOrDefault();
          if (r != null)
          {
            if (data[0].Value1.HasValue)
            {
              r.Value = data[0].Value1;
              r.InDateTime = DateTime.Now;
            }
            else
              db.tFinParts.DeleteOnSubmit(r);
            db.SubmitChanges();
          }
        }
        else if (data[0].Value1.HasValue)
        {
          tFinPart o = new tFinPart { TreatyID = data[0].TreatyID.Value, ObjClsID = data[0].ObjClsID.Value, An = data[0].Dt.Value.Year, Quartier = 1, Value = data[0].Value1, InDateTime = DateTime.Now };
          db.tFinParts.InsertOnSubmit(o);
          db.SubmitChanges();
        }

        if (data[0].ID2 > 0)
        {
          var r = db.tFinParts.Where(o => o.ID == data[0].ID2).FirstOrDefault();
          if (r != null)
          {
            if (data[0].Value2.HasValue)
            {
              r.Value = data[0].Value2;
              r.InDateTime = DateTime.Now;
            }
            else
              db.tFinParts.DeleteOnSubmit(r);
            db.SubmitChanges();
          }
        }
        else if (data[0].Value2.HasValue)
        {
          tFinPart o = new tFinPart { TreatyID = data[0].TreatyID.Value, ObjClsID = data[0].ObjClsID.Value, An = data[0].Dt.Value.Year, Quartier = 2, Value = data[0].Value2, InDateTime = DateTime.Now };
          db.tFinParts.InsertOnSubmit(o);
          db.SubmitChanges();
        }

        if (data[0].ID3 > 0)
        {
          var r = db.tFinParts.Where(o => o.ID == data[0].ID3).FirstOrDefault();
          if (r != null)
          {
            if (data[0].Value3.HasValue)
            {
              r.Value = data[0].Value3;
              r.InDateTime = DateTime.Now;
            }
            else
              db.tFinParts.DeleteOnSubmit(r);
            db.SubmitChanges();
          }
        }
        else if (data[0].Value3.HasValue)
        {
          tFinPart o = new tFinPart { TreatyID = data[0].TreatyID.Value, ObjClsID = data[0].ObjClsID.Value, An = data[0].Dt.Value.Year, Quartier = 3, Value = data[0].Value3, InDateTime = DateTime.Now };
          db.tFinParts.InsertOnSubmit(o);
          db.SubmitChanges();
        }

        if (data[0].ID4 > 0)
        {
          var r = db.tFinParts.Where(o => o.ID == data[0].ID4).FirstOrDefault();
          if (r != null)
          {
            if (data[0].Value4.HasValue)
            {
              r.Value = data[0].Value4;
              r.InDateTime = DateTime.Now;
            }
            else
              db.tFinParts.DeleteOnSubmit(r);
            db.SubmitChanges();
          }
        }
        else if (data[0].Value4.HasValue)
        {
          tFinPart o = new tFinPart { TreatyID = data[0].TreatyID.Value, ObjClsID = data[0].ObjClsID.Value, An = data[0].Dt.Value.Year, Quartier = 4, Value = data[0].Value4, InDateTime = DateTime.Now };
          db.tFinParts.InsertOnSubmit(o);
          db.SubmitChanges();
        }
      }
      var q3 = from t in db.tTreaties.Where(t => t.TreatyID == data[0].TreatyID)
               from p1 in db.tFinParts.Where(p => p.TreatyID == t.TreatyID && p.ObjClsID == data[0].ObjClsID && p.An == data[0].Dt.Value.Year && p.Quartier == 1).DefaultIfEmpty()
               from p2 in db.tFinParts.Where(p => p.TreatyID == t.TreatyID && p.ObjClsID == data[0].ObjClsID && p.An == data[0].Dt.Value.Year && p.Quartier == 2).DefaultIfEmpty()
               from p3 in db.tFinParts.Where(p => p.TreatyID == t.TreatyID && p.ObjClsID == data[0].ObjClsID && p.An == data[0].Dt.Value.Year && p.Quartier == 3).DefaultIfEmpty()
               from p4 in db.tFinParts.Where(p => p.TreatyID == t.TreatyID && p.ObjClsID == data[0].ObjClsID && p.An == data[0].Dt.Value.Year && p.Quartier == 4).DefaultIfEmpty()
               select new
               {
                 t.TreatyID,
                 data[0].ObjClsID,
                 ID1 = (int?)p1.ID,
                 Value1 = p1.Value,
                 ID2 = (int?)p2.ID,
                 Value2 = p2.Value,
                 ID3 = (int?)p3.ID,
                 Value3 = p3.Value,
                 ID4 = (int?)p4.ID,
                 Value4 = p4.Value,
                 Dt = data[0].Dt
               };
      return q3;
    }

    public IEnumerable<dynamic> GetCbBPifer()
    {
      var q = (from p in db.tObjClsRelations.Where(p => p.ObjClassifierID == 6700)
               join f in db.tFinancialInstitutions on p.ObjectID equals f.FinancialInstitutionID
               select new { id = f.FinancialInstitutionID, name = f.Name + " " + f.NameLat + (f.NameState != "" ? " " + f.NameState : "") }
              ).OrderBy(n => n.name);
      return q;
    }

    public IEnumerable<dynamic> GetPiferData(int? id, DateTime? d1, DateTime? d2)
    {

      var q = from pr in db.tPIFerRests.Where(p => p.FinInstID == id && p.RestDate >= d1 && p.RestDate <= d2 && p.Rest != 0).Select(p => new { p.FinInstID, p.SecurityID }).Distinct()
              from pre in db.tPIFerRests.Where(p => p.FinInstID == id && p.SecurityID == pr.SecurityID && p.RestDate == d2 && p.Rest != 0).DefaultIfEmpty()
              from rte in db.tRates.Where(r1 => r1.TradeSystemID == 446 && r1.SecurityID == pr.SecurityID && r1.ActualizationDateTime < d2).OrderByDescending(r1 => r1.ActualizationDateTime).Take(1).DefaultIfEmpty()
              join dc1 in db.tDictionariesConnections.Where(p => p.Dictionary == 741604640) on pr.FinInstID equals dc1.CompositeID
              join dc2 in db.tDictionariesConnections.Where(p => p.Dictionary == 1104993180) on pr.SecurityID equals dc2.CompositeID
              from ps in db.pPifSecDates.Where(p => p.SecurityID == dc2.DiasoftBOID && p.InstitutionID == dc1.DiasoftBOID)
              join f in db.tFinancialInstitutions on pr.FinInstID equals f.FinancialInstitutionID
              join s in db.tSecurities on pr.SecurityID equals s.SecurityID
              join fi in db.tFinancialInstitutions on s.IssuerID equals fi.FinancialInstitutionID
              join op in db.tObjClsRelations.Where(p => p.ObjClassifierID == 791) on s.IssuerID equals op.ObjectID
              from oc in db.tObjClassifiers.Where(p => p.ObjType == 1104993180 && p.ParentID == 589)
              from ocr in db.tObjClsRelations.Where(p => p.ObjType == 1104993180 && p.ObjectID == pr.SecurityID && p.ObjClassifierID == oc.ObjClassifierID)
              join i in db.tExchangeIndexes on oc.Comment equals i.ExchangeIndexID.ToString()
              from prb in db.tPIFerRests.Where(p => p.FinInstID == id && p.SecurityID == pr.SecurityID && p.RestDate == d1).DefaultIfEmpty()
              from rtb in db.tRates.Where(p => p.TradeSystemID == 446 && p.SecurityID == pr.SecurityID && p.ActualizationDateTime < d1).OrderByDescending(p => p.ActualizationDateTime).Take(1).DefaultIfEmpty()
              from rtw in db.tRates.Where(p => p.TradeSystemID == 446 && p.SecurityID == pr.SecurityID && p.ActualizationDateTime < d2.Value.AddDays(-6)).OrderByDescending(p => p.ActualizationDateTime).Take(1).DefaultIfEmpty()
              from rtm in db.tRates.Where(p => p.TradeSystemID == 446 && p.SecurityID == pr.SecurityID && p.ActualizationDateTime < d2.Value.AddMonths(-1)).OrderByDescending(p => p.ActualizationDateTime).Take(1).DefaultIfEmpty()
              from rtq in db.tRates.Where(p => p.TradeSystemID == 446 && p.SecurityID == pr.SecurityID && p.ActualizationDateTime < d2.Value.AddMonths(-3)).OrderByDescending(p => p.ActualizationDateTime).Take(1).DefaultIfEmpty()
              from rty in db.tRates.Where(p => p.TradeSystemID == 446 && p.SecurityID == pr.SecurityID && p.ActualizationDateTime < d2.Value.AddYears(-1)).OrderByDescending(p => p.ActualizationDateTime).Take(1).DefaultIfEmpty()
              from rt3y in db.tRates.Where(p => p.TradeSystemID == 446 && p.SecurityID == pr.SecurityID && p.ActualizationDateTime < d2.Value.AddYears(-3)).OrderByDescending(p => p.ActualizationDateTime).Take(1).DefaultIfEmpty()
              from rtyb in db.tRates.Where(p => p.TradeSystemID == 446 && p.SecurityID == pr.SecurityID && p.ActualizationDateTime <= d2.Value.AddDays(d2.Value.DayOfYear)).OrderByDescending(p => p.ActualizationDateTime).Take(1).DefaultIfEmpty()
              from rtf in db.tRates.Where(p => p.TradeSystemID == 446 && p.SecurityID == pr.SecurityID && p.ActualizationDateTime <= ps.DateFirst).OrderByDescending(p => p.ActualizationDateTime).Take(1).DefaultIfEmpty()
              from rtf1 in db.tRates.Where(p => p.TradeSystemID == 446 && p.SecurityID == pr.SecurityID && p.ActualizationDateTime >= ps.DateFirst && p.CourseCurrent != 0).OrderBy(p => p.ActualizationDateTime).Take(1).DefaultIfEmpty()
              select new
              {
                name = f.Name + " " + f.NameLat + (f.NameState != "" ? " " + f.NameState : ""),
                id = fi.FinancialInstitutionID,
                PIFBrief = fi.NameBrief.Trim(),
                PIFName = op.Comment,
                ps.DateFirst,
                iid = i.ExchangeIndexID,
                i.IndexName,
                RestB = prb.Rest,
                RateB = rtb.CourseCurrent,
                RestE = pre.Rest,
                RateE = rte.CourseCurrent,
                Turn = (from pt in db.tPIFerTurns.Where(p => p.FinInstID == id && p.SecurityID == pr.SecurityID && p.TurnDate > d1 && p.TurnDate <= d2)
                        from rt in db.tRates.Where(p => p.TradeSystemID == 446 && p.SecurityID == pt.SecurityID && p.ActualizationDateTime <= pt.TurnDate).OrderByDescending(p => p.ActualizationDateTime).Take(1).DefaultIfEmpty()
                        select new { fr = ((double?)(pt.TurnCre - pt.TurnDeb)) * rt.CourseCurrent }).Sum(p => p.fr),
                rtw = rtw.CourseCurrent,
                rtm = rtm.CourseCurrent,
                rtq = rtq.CourseCurrent,
                rty = rty.CourseCurrent,
                rt3y = rt3y.CourseCurrent,
                rtyb = rtyb.CourseCurrent,
                rtf = rtf.CourseCurrent > 0 ? rtf.CourseCurrent : rtf1.CourseCurrent
              };
      return q;
    }

    public bool MissionCreate(Mission m, string UserName)
    {
      m.Comment = m.Comment ?? "";
      taLib l = new taLib { LConcept = 483281, LParent = 483281, LID1 = m.Option, LName = m.Comment.Left(80), LName1 = UserName, LName2 = m.Comment.Length <= 80 ? "" : m.Comment.Length > 160 ? m.Comment.Substring(80, 80) : m.Comment.Substring(80, m.Comment.Length - 80), InDateTime = DateTime.Now };
      try
      {
        db.taLibs.InsertOnSubmit(l);
        db.SubmitChanges();
      }
      catch
      {
        return false;
      }
      return true;
    }

    public IEnumerable<dynamic> GetChartData()
    {
      var d = new DateTime(1970, 1, 1).Ticks;

      var q = (from g in
                 (
                   from ocr in db.tObjClsRelations.Where(p => p.ObjClassifierID == 791 && p.ObjType == 741604640)
                   join s in db.tSecurities.Where(p => p.SecType == 6) on ocr.ObjectID equals s.IssuerID
                   from dt in db.tDates.Where(p => p.Date >= new DateTime(2000, 1, 1) && p.Date <= DateTime.Today)
                   from rt in db.tRates.Where(p => p.RawDataProviderID == 1 && p.TradeSystemID == 446 && p.SecurityID == s.SecurityID && p.ActualizationDateTime <= dt.Date).OrderByDescending(p => p.ActualizationDateTime).Select(p => new { p.ValuePrice }).Take(1).DefaultIfEmpty()
                   join t in
                     (from s in db.tSecurities.Where(p => p.SecType == 6 && p.IsDeleted == 0)
                      join tr in db.tTreaties.Where(p => p.DateFinish == new DateTime(1900, 1, 1)) on s.IssuerID equals tr.FinancialInstitutionID
                      join ttt in db.tTreatyTreatyTypes.Where(p => p.TreatyTypeID == 1) on tr.TreatyID equals ttt.TreatyID
                      select new { s.SecurityID, tr.TreatyID }) on s.SecurityID equals t.SecurityID
                   from c in db.tObjClsRelations.Where(p => p.ObjClassifierID == 171 && p.ObjectID == t.TreatyID && (p.OnDate == new DateTime(2009, 1, 1) ? new DateTime(2000, 1, 1) : p.OnDate) <= dt.Date).OrderByDescending(p => p.OnDate).Select(p => new { p.Value }).Take(1).DefaultIfEmpty()
                   select new { dt.Date, Qty = rt.ValuePrice * c.Value / 36500.0 })
               group g by g.Date into grp
               orderby grp.Key
               select new { Date = grp.Key, Value = grp.Sum(p => p.Qty) }).ToList().Select(i => new object[] { (i.Date.Ticks - d) / TimeSpan.TicksPerMillisecond /* / 1000*/, i.Value });

      //var q = (from i in db.tExchangeIndexValues
      //         where i.ExchangeIndexID == 1
      //         orderby i.Date
      //         select new { i.Date, value = i.Close_value }).ToList().Select(i => new object[] { (i.Date.Ticks - d) / TimeSpan.TicksPerMillisecond /*/ 1000*/, i.value });
      return q;
    }

    public ISingleResult<up_avgRepPIFOrderStatResult> RepPIFOrderStat(DateTime? d1, DateTime? d2)
    {
      db.CommandTimeout = 3 * 60;
      return db.up_avgRepPIFOrderStat(d1, d2);
    }

    public ISingleResult<up_avgRepPIFOrderOutStatResult> RepPIFOrderOutStat(DateTime? d1, DateTime? d2)
    {
      db.CommandTimeout = 3 * 60;
      return db.up_avgRepPIFOrderOutStat(d1, d2);
    }

    public IEnumerable<dynamic> GetClientIKList(string sort, string dir)
    {
      var q = from n in db.taNabs
              where n.NConcept == 2020
              join c in db.tFinancialInstitutions on n.NID1 equals c.FinancialInstitutionID into c_
              from c in c_.DefaultIfEmpty()
              join i in db.tFinancialInstitutions on n.NID2 equals i.FinancialInstitutionID into i_
              from i in i_.DefaultIfEmpty()
              select new
              {
                id = n.ID,
                n.NDate1,
                n.NDate2,
                n.NID1,
                n.NID2,
                Client = c.NameBrief.Trim(),
                IK = i.NameBrief.Trim()
              };
      if (String.IsNullOrEmpty(sort))
        q = q.OrderBy(p => p.Client);
      else
        q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
      return q;
    }

    public IEnumerable<dynamic> addClientIK(List<taNab> data)
    {
      data[0].NConcept = 2020;
      db.taNabs.InsertAllOnSubmit(data);
      db.SubmitChanges();

      var q = from n in db.taNabs.Where(p => data.Select(s => s.ID).Contains(p.ID))
              where n.NConcept == 2020
              join c in db.tFinancialInstitutions on n.NID1 equals c.FinancialInstitutionID into c_
              from c in c_.DefaultIfEmpty()
              join i in db.tFinancialInstitutions on n.NID2 equals i.FinancialInstitutionID into i_
              from i in i_.DefaultIfEmpty()
              select new
              {
                id = n.ID,
                n.NDate1,
                n.NDate2,
                n.NID1,
                n.NID2,
                Client = c.NameBrief.Trim(),
                IK = i.NameBrief.Trim()
              };
      return q;
    }

    public IEnumerable<dynamic> updClientIK(List<taNab> data)
    {
      foreach (var e in data.Where(p => p.ID > 0))
      {
        var q1 = db.taNabs.Where(p => p.ID == e.ID && p.NConcept == 2020).First();
        if (q1 != null)
        {
          q1.NDate1 = e.NDate1;
          q1.NDate2 = e.NDate2;
          q1.NID1 = e.NID1;
          q1.NID2 = e.NID2;
        }
      }
      db.SubmitChanges();

      var q = from n in db.taNabs.Where(p => data.Select(s => s.ID).Contains(p.ID))
              where n.NConcept == 2020
              join c in db.tFinancialInstitutions on n.NID1 equals c.FinancialInstitutionID into c_
              from c in c_.DefaultIfEmpty()
              join i in db.tFinancialInstitutions on n.NID2 equals i.FinancialInstitutionID into i_
              from i in i_.DefaultIfEmpty()
              select new
              {
                id = n.ID,
                n.NDate1,
                n.NDate2,
                n.NID1,
                n.NID2,
                Client = c.NameBrief.Trim(),
                IK = i.NameBrief.Trim()
              };
      return q;
    }

    public bool delClientIK(List<taNab> data)
    {
      try
      {
        IEnumerable<taNab> e = db.taNabs.Where(p => data.Select(n => n.ID).Contains(p.ID) && p.NConcept == 2020);
        db.taNabs.DeleteAllOnSubmit(e);
        db.SubmitChanges();
        return true;
      }
      catch (Exception /*ex*/)
      {
        return false;
      }
    }

    public IEnumerable<dynamic> GetIKList()
    {
      var q = from o in db.tObjClsRelations
              where o.ObjClassifierID == 36028 && o.ObjType == 741604640
              join c in db.tFinancialInstitutions on o.ObjectID equals c.FinancialInstitutionID
              orderby c.NameBrief
              select new
              {
                id = o.ObjectID,
                Name = c.NameBrief.Trim()
              };
      return q;
    }

    public IEnumerable<dynamic> GetClIKList()
    {
      var q = from o in db.tObjClsRelations
              where o.ObjClassifierID == 38078 && o.ObjType == 741604640
              join c in db.tFinancialInstitutions on o.ObjectID equals c.FinancialInstitutionID
              orderby c.NameBrief
              select new
              {
                id = o.ObjectID,
                Name = c.NameBrief.Trim()
              };
      return q;
    }

    public bool ImpCharges(HttpPostedFileBase file, int? mois, int? an)
    {
      var rez = true;
      using (StreamReader readStream = new StreamReader(file.InputStream, System.Text.Encoding.GetEncoding("windows-1251")))
      {
        db.tCharges.DeleteAllOnSubmit(db.tCharges.Where(p => p.Mois == mois && p.An == an));
        var l = new List<tCharge>();
        while (readStream.Peek() >= 0)
        {
          string fText = readStream.ReadLine();
          string[] sText = fText.Split(new char[] { ';' });
          if (sText.Length == 3)
          {
            var c = new tCharge { An = an, Mois = mois, Quartier = (mois + 2) / 3 };
            var pfpId = db.tObjClassifiers.Where(p => p.Name == sText[1] && p.ParentID == 1054).Select(p => p.ObjClassifierID).FirstOrDefault();
            if (pfpId.HasValue)
            {
              c.PfpID = pfpId;
            }
            var itemId = (from c0 in db.tObjClassifiers
                          join c1 in db.tObjClassifiers on c0.ObjClassifierID equals c1.ParentID
                          join c2 in db.tObjClassifiers.Where(p => p.NameBrief == sText[0]) on c1.ObjClassifierID equals c2.ParentID
                          where c0.ParentID == 1019
                          select c2.ObjClassifierID).FirstOrDefault();
            if (itemId.HasValue)
            {
              c.ItemID = itemId;
            }
            decimal qtyP;
            var b = decimal.TryParse(sText[2], out qtyP);
            if (c.ItemID.HasValue && c.PfpID.HasValue)
            {
              c.QtyP = qtyP;
              l.Add(c);
            }
            else
            {
              rez = false;
              break;
            }
          }
          else
          {
            rez = false;
            break;
          }
        }
        if (rez)
        {
          db.tCharges.InsertAllOnSubmit(l);
          db.SubmitChanges();
        }
      }
      return rez;
    }

    public ISingleResult<up_avgRepPIFDepStatResult> RepPIFDepStat(DateTime? d1, DateTime? d2)
    {
      return db.up_avgRepPIFDepStat(d1, d2);
    }

    public ISingleResult<up_avgRepPIFDepStat2Result> RepPIFDepStat2(DateTime? d1, DateTime? d2)
    {
      return db.up_avgRepPIFDepStat2(d1, d2);
    }

    public ISingleResult<up_avgRepBankPIFIncomeResult> RepBankPIFIncome(DateTime? d1, DateTime? d2)
    {
      return db.up_avgRepBankPIFIncome(d1, d2);
    }

    public ISingleResult<up_avgRepBankPIFIncomeLKResult> RepBankPIFIncomeLK(DateTime? d1, DateTime? d2)
    {
      return db.up_avgRepBankPIFIncomeLK(d1, d2);
    }

    public IEnumerable<up_avgRepPIFStatResult> RepPIFStat(DateTime? d1, DateTime? d2)
    {
      return db.up_avgRepPIFStat(d1, d2, 0).GetResult<up_avgRepPIFStatResult>();
    }

    public IEnumerable<up_avgRepPIFStat1Result> RepPIFStat1(DateTime? d1, DateTime? d2)
    {
      return db.up_avgRepPIFStat(d1, d2, 1).GetResult<up_avgRepPIFStat1Result>();
    }

    public bool ordpaymCourriel(int id, string url, string host, string descr, bool refuse = false)
    {
      var sb = new StringBuilder();
      var q1 = (from i in db.tOrdPayments
                join f in db.tFinancialInstitutions on i.FinInstID equals f.FinancialInstitutionID into fl
                from f in fl.DefaultIfEmpty()
                join oc3 in db.tObjClsRelations.Where(p => p.ObjClassifierID == 1003 && p.ObjType == 741604640) on i.FinInstID equals oc3.ObjectID into oc3_
                from oc3 in oc3_.DefaultIfEmpty()
                join oc in db.tObjClassifiers on i.ExecutorID equals oc.ObjClassifierID into oc_
                from oc in oc_.DefaultIfEmpty()
                join oc2 in db.tObjClassifiers on i.DocTypeID equals oc2.ObjClassifierID into oc2_
                from oc2 in oc2_.DefaultIfEmpty()
                join oc4 in db.tObjClassifiers on i.SignID equals oc4.ObjClassifierID into oc4_
                from oc4 in oc4_.DefaultIfEmpty()
                join oc5 in db.tObjClassifiers on i.Sign2ID equals oc5.ObjClassifierID into oc5_
                from oc5 in oc5_.DefaultIfEmpty()
                join s in db.tSecurities on i.FundID equals s.SecurityID into s_
                from s in s_.DefaultIfEmpty()
                join c in db.tContragents on i.ReceiverID equals c.Id into c_
                from c in c_.DefaultIfEmpty()
                join d in db.tContragentDogs on i.DogovorID equals d.Id into d_
                from d in d_.DefaultIfEmpty()
                where i.ID == id
                select new
                {
                  id = i.ID,
                  i.Number,
                  i.IsBudget,
                  IsBudgetS = i.IsBudget.Value ? "Да" : "Нет",
                  i.IsPlan,
                  IsPlanS = i.IsPlan.Value ? "Да" : "Нет",
                  i.IsReserve,
                  IsReserveS = i.IsReserve.Value ? "Да" : "Нет",
                  i.FinInstID,
                  i.PPTypeID,
                  NameBrief = oc3.Comment ?? f.NameBrief.Trim(),
                  i.DocTypeID,
                  DocTypeName = oc2.Name,
                  i.DateDoc,
                  i.DocNumb,
                  i.DogovorID,
                  DogName = (d.Name ?? "") + (d.Number == null ? "" : " №" + d.Number),
                  d.DogDate,
                  i.Dogovor,
                  Receiver = c.Name,
                  i.ReceiverID,
                  i.ExecutorID,
                  ExecutorName = oc.Name,
                  i.FundID,
                  FundName = s.NameBrief.Trim(),
                  i.DateCreate,
                  i.DateReg,
                  i.SignFIO1,
                  i.SignPost1,
                  i.FileName,
                  i.SignID,
                  i.Sign2ID,
                  i.StateID,
                  ExecMail = oc.Comment,
                  SignMail = oc4.Comment,
                  Sign2Mail = oc5.Comment,
                  FileNameCD = d.FileName,
                  i.IsLoan
                }).FirstOrDefault();

      if (q1 != null)
      {
        var stateIdNew = NextStateID(id, 1, q1.StateID ?? 0);
        sb.Append("<style>table{border-collapse:collapse;}td,th{border:1px solid gray;}td,span,th{font-size:.8em;font-family:'Segoe UI',Verdana,Helvetica,Sans-Serif;text-align:left;}span{font-style:italic;}th{font-size: .8em;}.r{text-align:right}</style>");

        if (stateIdNew == 4)
        {
          sb.Append("<h4>Уважаемый коллега, просим осуществить оплату и подтвердить исполнение распоряжения по <a href='" + url + "'>ссылке</a>.</h4>");
          sb.Append("<h4>Вернуть распоряжение <a href='" + url + "&a=-1'>ссылка</a>.</h4>");
        }
        else if ((new int?[] { 2, 3, 5, 6 }).Contains(stateIdNew))
        {
          sb.Append("<h4>Уважаемый коллега, просим подтвердить исполнение распоряжения по <a href='" + url + "'>ссылке</a>.</h4>");
        }

        sb.Append("<table>");
        sb.Append(string.Format("<tr><th>Номер</th><td>{0}</td></tr>", string.IsNullOrWhiteSpace(q1.Number) ? q1.id.ToString() : q1.Number));
        sb.Append(string.Format("<tr><th>Бюджет</th><td>{0}</td></tr>", q1.IsBudgetS));
        if (q1.IsReserve == true)
          sb.Append(string.Format("<tr><th>Резерв</th><td>Да</td></tr>"));
        if (stateIdNew == 4 && q1.PPTypeID > 0)
        {
          sb.Append(string.Format("<tr><th>Платежное поручение</th><td>Требуется {0}</td></tr>", q1.PPTypeID == 1 ? "копия" : q1.PPTypeID == 2 ? "оригинал" : "?"));
        }
        sb.Append(string.Format("<tr><th>Компания плательщик</th><td>{0}</td></tr>", q1.NameBrief));
        sb.Append(string.Format("<tr><th>Вид документа</th><td>{0}</td></tr>", q1.DocTypeName));
        sb.Append(string.Format("<tr><th>Дата документа</th><td>{0:dd.MM.yyyy}</td></tr>", q1.DateDoc));
        sb.Append(string.Format("<tr><th>Номер документа</th><td>{0}</td></tr>", q1.DocNumb));
        sb.Append(string.Format("<tr><th>Контрагент -  получатель средств</th><td>{0}</td></tr>", q1.Receiver));
        sb.Append(string.Format("<tr><th>Номер договора</th><td>{0}</td></tr>", (q1.DogovorID == null ? q1.Dogovor : (q1.DogName + (q1.DogDate.HasValue ? " от " + q1.DogDate.Value.ToShortDateString() + "г." : "")))));
        if (!String.IsNullOrWhiteSpace(q1.FileNameCD))
        {
          sb.Append(string.Format("<tr><th>Файл договора</th><td><a href='http://{1}/report/getfilecd?data={2}'>{0}</a></td></tr>", q1.FileNameCD, host, HttpUtility.UrlEncode(q1.FileNameCD)));
        }

        sb.Append(string.Format("<tr><th>Валюта документа</th><td>{0}</td></tr>", q1.FundName));
        sb.Append(string.Format("<tr><th>Исполнитель</th><td>{0}</td></tr>", q1.ExecutorName));
        sb.Append(string.Format("<tr><th>Дата составления</th><td>{0:dd.MM.yyyy}</td></tr>", q1.DateCreate));
        sb.Append(string.Format("<tr><th>Дата учёта</th><td>{0:dd.MM.yyyy}</td></tr>", q1.DateReg));
        if (!String.IsNullOrWhiteSpace(q1.FileName))
        {
          sb.Append(string.Format("<tr><th>Файл</th><td><a href='http://{1}/report/getfile?data={2}'>{0}</a></td></tr>", q1.FileName, host, HttpUtility.UrlEncode(q1.FileName)));
        }

        sb.Append("</table>");
        var q2 = (from p in db.tOrdPaymentDets.Where(r => r.OrdPaymID == id)
                  select new
                  {
                    s = 1,
                    p.ID,
                    p.Item,
                    p.PFP,
                    Qty = p.Qty - (p.VAT ?? 0),
                    QtyR = p.QtyR - (p.VATR ?? 0),
                    p.Comment
                  }).Union(
          from p in db.tOrdPaymentDets.Where(r => r.OrdPaymID == id && ((r.tOrdPayment.FundID == 39191 && r.VAT > 0) || (r.tOrdPayment.FundID != 39191 && r.VATR > 0)))
          select new
          {
            s = 2,
            p.ID,
            p.Item,
            p.PFP,
            Qty = p.VAT,
            QtyR = p.VATR,
            Comment = "НДС"
          })
          .OrderBy(p => p.ID).ThenBy(p => p.s);

        sb.Append("<span>Позиции документа</span>");
        sb.Append("<table>");
        sb.Append("<tr><th>№</th><th>Статья расходов</th><th width='100'>ПФП</th><th class='r'>Сумма в валюте документа</th><th>Экономическое содержание</th></tr>");
        var c1 = 1;
        decimal? Qty = 0m;
        foreach (var qp2 in q2)
        {
          sb.Append(string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td class='r'>{3:N2}</td><td>{4}</td></tr>", c1++, qp2.Item, qp2.PFP, (q1.FundID == 39191) ? qp2.Qty : qp2.QtyR, qp2.Comment));
          Qty += (q1.FundID == 39191) ? qp2.Qty : qp2.QtyR;
        }
        sb.Append(string.Format("<tr><th colspan='3'>Всего</td><th class='r'>{0:N2}</td><td>&nbsp;</td></tr>", Qty));
        sb.Append("</table>");
        if (!string.IsNullOrWhiteSpace(descr))
          sb.Append(string.Format("<strong>{0}</strong><br>", descr));

        var EMailTo = "";
        var EMailCC = "";
        if (stateIdNew == 0 || stateIdNew == 1)
        {
          EMailTo = q1.ExecMail;
        }
        else if (stateIdNew == 2)
        {
          EMailTo = q1.SignMail + (string.IsNullOrWhiteSpace(q1.ExecMail) ? "" : ("," + q1.ExecMail));
        }
        else if (stateIdNew == 5)
        {
          EMailTo = q1.Sign2Mail + (string.IsNullOrWhiteSpace(q1.ExecMail) ? "" : ("," + q1.ExecMail));
        }
        else if (stateIdNew == 3)
        {
          EMailTo = string.Join(", ", db.tObjClassifiers.Where(o => o.ParentID == 48068).Select(p => p.Comment).ToArray()) + (refuse ? "," + q1.ExecMail : ""); // БК
        }
        else if (q1.StateID == 4)
        {
          EMailTo = q1.ExecMail;
        }
        else if (stateIdNew == 4)
        {
          EMailTo = string.Join(", ", db.tObjClassifiers.Where(o => o.ParentID == 2100).Select(p => p.Comment).ToArray()) + (refuse ? "," + q1.ExecMail : ""); // РЦ
        }
        //else if (stateIdNew == 6)
        //{
        //  EMailTo = "TogobitskySV@uralsib.ru,KabaninGV@uralsib.ru";
        //}

        SmtpClient sc = new SmtpClient("m.am-uralsib.ru");
        sc.UseDefaultCredentials = true;
        MailMessage message = new MailMessage();
        message.From = new MailAddress("OrderPayment <assets_msg@am-uralsib.ru>");

        message.To.Add((host.Contains("localhost") || host.Contains("10.158.32.10")) ? "GrishinAV@am-uralsib.ru" : EMailTo);
        if (!string.IsNullOrWhiteSpace(EMailCC))
          message.CC.Add((host.Contains("localhost") || host.Contains("10.158.32.10")) ? "GrishinAV@am-uralsib.ru" : EMailCC);
        message.Body = sb.ToString();
        message.IsBodyHtml = true;
        message.Priority = MailPriority.High;
        message.Headers.Add("Importance", "High");
        message.IsBodyHtml = true;
        message.Subject = string.Format("Распоряжение на оплату {1} ({2:N2}{3}) {0} {4}", (q1.StateID == 4 ? "Выполнено" : refuse ? "Отказ!" : ""), q1.Receiver, Qty, q1.FundName, descr);
        sc.Send(message);

        return true;
      }
      return false;
    }

    public bool ordPaymRappel(string host, UrlHelper Url)
    {
      var q = (from i in db.tOrdPayments
               from opd in
                 (from od in i.tOrdPaymentDets
                  group od by 1 into g
                  select new
                  {
                    Qty = g.Sum(p => p.Qty),
                    VAT = g.Sum(p => p.VAT),
                    QtyR = g.Sum(p => p.QtyR),
                    VATR = g.Sum(p => p.VATR)
                  }).DefaultIfEmpty()
               join oc2 in db.tObjClassifiers on i.DocTypeID equals oc2.ObjClassifierID into oc2_
               from oc2 in oc2_.DefaultIfEmpty()
               join s in db.tSecurities on i.FundID equals s.SecurityID into s_
               from s in s_.DefaultIfEmpty()
               join c in db.tContragents on i.ReceiverID equals c.Id into c_
               from c in c_.DefaultIfEmpty()
               join d in db.tContragentDogs on i.DogovorID equals d.Id into d_
               from d in d_.DefaultIfEmpty()
               where i.StateID == 3
               //where i.StateID == 6 || (i.StateID == 3 && ((opd.Qty ?? 0) < 249999 || i.IsLoan == true))
               select new
               {
                 id = i.ID,
                 i.PPTypeID,
                 i.DateDoc,
                 i.DocNumb,
                 i.DogovorID,
                 DogName = (d.Name ?? "") + (d.Number == null ? "" : " №" + d.Number),
                 d.DogDate,
                 i.Dogovor,
                 Receiver = c.Name,
                 i.FundID,
                 FundName = s.NameBrief.Trim(),
                 Qty = (i.FundID == 39191) ? opd.Qty : opd.QtyR,
                 i.DateCreate,
                 i.SignID,
                 i.Sign2ID,
                 i.StateID
               }).ToList();

      if (q != null)
      {
        var EMailTo = string.Join(", ", db.tObjClassifiers.Where(o => o.ParentID == 2100).Select(p => p.Comment).ToArray());

        SmtpClient sc = new SmtpClient("m.am-uralsib.ru");
        sc.UseDefaultCredentials = true;
        try
        {
          MailMessage message = new MailMessage();
          message.From = new MailAddress("OrderPayments <assets_msg@am-uralsib.ru>");
          if (!String.IsNullOrEmpty(EMailTo))
            message.To.Add((host.Contains("localhost") || host.Contains("10.158.32.10")) ? "GrishinAV@am-uralsib.ru" : EMailTo);
          //message.Bcc.Add("GrishinAV@am-uralsib.ru");
          var template = new MO.Views.Report.OrdPaymRappel { q1 = q, em = EMailTo, host = host, url = Url };
          message.Body = template.TransformText();
          message.IsBodyHtml = true;
          message.Priority = MailPriority.High;
          message.Headers.Add("Importance", "High");
          message.IsBodyHtml = true;
          message.Subject = "Напоминание о необходимости исполнения распоряжений на оплату";
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
          message.Subject = "Ошибка в рассылке \"Напоминание о необходимости исполнения распоряжений на оплату\"";
          sc.Send(message);
        }
      }
      return false;
    }

    public bool ordPaymRappel2(string host, UrlHelper Url)
    {
      var q = (from i in db.tOrdPayments
               from opd in
                 (from od in i.tOrdPaymentDets
                  group od by 1 into g
                  select new
                  {
                    Qty = g.Sum(p => p.Qty),
                    VAT = g.Sum(p => p.VAT),
                    QtyR = g.Sum(p => p.QtyR),
                    VATR = g.Sum(p => p.VATR)
                  }).DefaultIfEmpty()
               join oc2 in db.tObjClassifiers on i.DocTypeID equals oc2.ObjClassifierID into oc2_
               from oc2 in oc2_.DefaultIfEmpty()
               join s in db.tSecurities on i.FundID equals s.SecurityID into s_
               from s in s_.DefaultIfEmpty()
               join c in db.tContragents on i.ReceiverID equals c.Id into c_
               from c in c_.DefaultIfEmpty()
               join d in db.tContragentDogs on i.DogovorID equals d.Id into d_
               from d in d_.DefaultIfEmpty()
               where /*(i.StateID ?? -1) != 4 &&*/ i.DateCreate >= DateTime.Today.AddMonths(-1) && i.DateCreate < DateTime.Today && (opd.Qty ?? 0) > 0 && !i.DatePay.HasValue
               select new
               {
                 id = i.ID,
                 i.PPTypeID,
                 i.DateDoc,
                 i.DocNumb,
                 i.DogovorID,
                 DogName = (d.Name ?? "") + (d.Number == null ? "" : " №" + d.Number),
                 d.DogDate,
                 i.Dogovor,
                 Receiver = c.Name,
                 i.FundID,
                 FundName = s.NameBrief.Trim(),
                 Qty = (i.FundID == 39191) ? opd.Qty : opd.QtyR,
                 i.DateCreate,
                 i.SignID,
                 i.Sign2ID,
                 i.StateID
               }).ToList();

      if (q != null)
      {
        var EMailTo = "NaumkinaYuA@am-uralsib.ru";

        SmtpClient sc = new SmtpClient("m.am-uralsib.ru");
        sc.UseDefaultCredentials = true;
        try
        {
          MailMessage message = new MailMessage();
          message.From = new MailAddress("OrderPayments <assets_msg@am-uralsib.ru>");
          if (!String.IsNullOrEmpty(EMailTo))
            message.To.Add((host.Contains("localhost") || host.Contains("10.158.32.10")) ? "GrishinAV@am-uralsib.ru" : EMailTo);
          var template = new MO.Views.Report.OrdPaymRappel { q1 = q, em = EMailTo, host = host, url = Url };
          message.Body = template.TransformText();
          message.IsBodyHtml = true;
          message.Priority = MailPriority.High;
          message.Headers.Add("Importance", "High");
          message.IsBodyHtml = true;
          message.Subject = "Напоминание о необходимости исполнения распоряжений на оплату";
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
          message.Subject = "Ошибка в рассылке \"Напоминание о необходимости исполнения распоряжений на оплату\"";
          sc.Send(message);
        }
      }
      return false;
    }

    public int NextStateID(int id, int action, int StateIdCur)
    {
      var op = db.tOrdPayments.First(p => p.ID == id);
      var stateIdOld = (op.StateID ?? 0);
      var stateIdNew = stateIdOld;

      if (StateIdCur == stateIdOld)
      {
        if (action == 1)
        {
          if (stateIdOld != 4 /* Final */)
          {
            if (stateIdOld == 0 && op.SignID == null)
            {
              stateIdNew = 2;
            }
            else if (stateIdOld == 2 && op.Sign2ID != null)
            {
              stateIdNew = 5;
            }
            else if (stateIdOld == 5)
            {
              stateIdNew = 3;
            }
            //else if (stateIdOld == 3 && db.tOrdPaymentDets.Where(p => p.OrdPaymID == id).Sum(p => p.Qty) >= 249999 && !(db.tOrdPayments.FirstOrDefault(p => p.ID == id).IsLoan == true))
            //{
            //  stateIdNew = 6;
            //}
            else if (stateIdOld == 6)
            {
              stateIdNew = 4;
            }
            else
            {
              stateIdNew++;
            }
          }
        }
        else if (action == -1)
        {
          if (stateIdOld > 0)
          {
            if (stateIdOld == 2 && op.SignID == null)
            {
              stateIdNew = 0;
            }
            else if (stateIdOld == 3 && op.Sign2ID != null)
            {
              stateIdNew = 5;
            }
            else if (stateIdOld == 5)
            {
              stateIdNew = 2;
            }
            //else if (stateIdOld == 4 && db.tOrdPaymentDets.Where(p => p.OrdPaymID == id).Sum(p => p.Qty) >= 249999 && !(db.tOrdPayments.FirstOrDefault(p => p.ID == id).IsLoan == true))
            //{
            //  stateIdNew = 6;
            //}
            else if (stateIdOld == 6)
            {
              stateIdNew = 3;
            }
            else
            {
              stateIdNew--;
            }
          }
        }
      }
      return stateIdNew;
    }

    public bool confirmOrdPaym(int id, string Login, int action, int StateIdCur, string comment, DateTime? DatePay)
    {
      var op = db.tOrdPayments.First(p => p.ID == id);
      var stateIdOld = (op.StateID ?? 0);
      if (StateIdCur == stateIdOld)
      {
        var stateIdNew = NextStateID(id, action, StateIdCur);
        if (action == 1)
        {
          if (stateIdNew == 4 && DatePay.HasValue)
          {
            op.DatePay = DatePay;
            if (op.FundID != 39191)
            {
              var c = (decimal?)db.tRates.Where(p => p.TradeSystemID == 1 && p.RawDataProviderID == 1 && p.SecurityID == op.FundID && p.ActualizationDateTime <= DatePay).OrderByDescending(p => p.ActualizationDateTime).FirstOrDefault().CourseCurrent;
              if (c > 0m)
              {
                //foreach (var od in db.tOrdPaymentDets.Where(p => p.OrdPaymID == id).ToList())
                //{
                //  od.Qty = Math.Round((od.QtyR ?? 0m) * (c ?? 0m), 2);
                //  od.VAT = Math.Round((od.VATR ?? 0m) * (c ?? 0m), 2);
                //}
                db.tOrdPaymentDets.Where(p => p.OrdPaymID == id).ToList().ForEach(od =>
                {
                  od.Qty = Math.Round((od.QtyR ?? 0m) * (c ?? 0m), 2);
                  od.VAT = Math.Round((od.VATR ?? 0m) * (c ?? 0m), 2);
                });
              }
            }
          }
        }
        if (stateIdNew != stateIdOld)
        {
          op.StateID = stateIdNew;
          var q2 = new tOrdPaymentLog { OrdPaymID = id, Login = Login, Action = action, InDateTime = DateTime.Now, Type = stateIdNew, Comment = comment };
          db.tOrdPaymentLogs.InsertOnSubmit(q2);
          db.SubmitChanges();
          return true;
        }
      }
      return false;
    }

    public IEnumerable<dynamic> getOrdPaymLogList(int id)
    {
      var q = from e in db.tOrdPaymentLogs.Where(p => p.OrdPaymID == id)
              join oc4 in db.tObjClassifiers.Where(p => p.ParentID == 48759) on e.Type.ToString() equals oc4.NameBrief into oc4_
              from oc4 in oc4_.DefaultIfEmpty()
              select new
              {
                id = e.ID,
                Act = e.Action == 1 ? "Подтверждение" : "Отмена",
                e.InDateTime,
                e.Login,
                Type = oc4.Name, //e.Type == 0 ? "Подготовка" : e.Type == 1 ? "Отправлена" : e.Type == 2 ? "Подписана СКР" : e.Type == 3 ? "Подписана БК" : e.Type == 4 ? "Выполнена РЦ" : "",
                e.Comment
              }.ToExpando();
      return q;
    }

    public int? getOrdPaymStateID(int id)
    {
      return db.tOrdPayments.FirstOrDefault(p => p.ID == id).StateID;
    }

    public bool OrdPaymUpdFilePP(int id, string FileNamePP)
    {
      var q = db.tOrdPayments.FirstOrDefault(p => p.ID == id);
      if (q != null)
      {
        q.FileNamePP = FileNamePP;
        db.SubmitChanges();
        return true;
      }
      return false;
    }


    public IEnumerable<dynamic> GetOrderPaymConf(int? id)
    {
      var q = from p in db.tOrdPaymentConfs.Where(r => r.OrdPaymID == id)
              join oc2 in db.tObjClassifiers on p.TypeID equals oc2.ObjClassifierID into oc2_
              from oc2 in oc2_.DefaultIfEmpty()
              join oc3 in db.tObjClassifiers on p.PeriodicityID equals oc3.ObjClassifierID into oc3_
              from oc3 in oc3_.DefaultIfEmpty()
              select new
              {
                p.id,
                p.OrdPaymID,
                p.DocName,
                p.DocDate,
                p.Item,
                p.DocNumb,
                p.Qty,
                p.FileName,
                p.Comment,
                p.ReceiveDate,
                p.IsCopy,
                p.OrigDate,
                p.TypeID,
                TypeName = oc2.Name,
                p.DateRegEnd,
                p.PeriodicityID,
                PeriodicityName = oc3.Name
              };
      return q;
    }

    public IEnumerable<dynamic> OrderPaymConfCreate(List<tOrdPaymentConf> data)
    {
      foreach (tOrdPaymentConf o in data.Where(p => p.id == 0))
        db.tOrdPaymentConfs.InsertOnSubmit(o);
      db.SubmitChanges();
      var q = from p in db.tOrdPaymentConfs
              join oc2 in db.tObjClassifiers on p.TypeID equals oc2.ObjClassifierID into oc2_
              from oc2 in oc2_.DefaultIfEmpty()
              join oc3 in db.tObjClassifiers on p.PeriodicityID equals oc3.ObjClassifierID into oc3_
              from oc3 in oc3_.DefaultIfEmpty()
              where p.id == data[0].id
              select new
              {
                p.id,
                p.OrdPaymID,
                p.DocName,
                p.DocDate,
                p.Item,
                p.DocNumb,
                p.Qty,
                p.FileName,
                p.Comment,
                p.ReceiveDate,
                p.IsCopy,
                p.OrigDate,
                p.TypeID,
                TypeName = oc2.Name,
                p.DateRegEnd,
                p.PeriodicityID,
                PeriodicityName = oc3.Name
              };
      return q;
    }

    public IEnumerable<dynamic> OrderPaymConfUpdate(List<tOrdPaymentConf> data)
    {
      var q1 = db.tOrdPaymentConfs.Where(o => o.id == data[0].id).First();

      q1.Comment = data[0].Comment;
      q1.DocName = data[0].DocName;
      q1.DocDate = data[0].DocDate;
      q1.DocNumb = data[0].DocNumb;
      q1.Item = data[0].Item;
      q1.Qty = data[0].Qty;
      q1.Comment = data[0].Comment;
      q1.FileName = data[0].FileName;
      q1.ReceiveDate = data[0].ReceiveDate;
      q1.IsCopy = data[0].IsCopy;
      q1.OrigDate = data[0].OrigDate;
      q1.DateRegEnd = data[0].DateRegEnd;
      q1.PeriodicityID = data[0].PeriodicityID;
      q1.TypeID = data[0].TypeID;
      db.SubmitChanges();

      var q = from p in db.tOrdPaymentConfs
              join oc2 in db.tObjClassifiers on p.TypeID equals oc2.ObjClassifierID into oc2_
              from oc2 in oc2_.DefaultIfEmpty()
              join oc3 in db.tObjClassifiers on p.PeriodicityID equals oc3.ObjClassifierID into oc3_
              from oc3 in oc3_.DefaultIfEmpty()
              where p.id == data[0].id
              select new
              {
                p.id,
                p.OrdPaymID,
                p.DocName,
                p.DocDate,
                p.Item,
                p.DocNumb,
                p.Qty,
                p.FileName,
                p.Comment,
                p.ReceiveDate,
                p.IsCopy,
                p.OrigDate,
                p.TypeID,
                TypeName = oc2.Name,
                p.DateRegEnd,
                p.PeriodicityID,
                PeriodicityName = oc3.Name
              };
      return q;
    }

    public bool OrderPaymConfDel(List<tOrdPaymentConf> data)
    {
      try
      {
        var q = db.tOrdPaymentConfs.Where(o => o.id == data[0].id);
        db.tOrdPaymentConfs.DeleteAllOnSubmit(q);
        db.SubmitChanges();
        return true;
      }
      catch
      {
        return false;
      }
    }

    public ISingleResult<up_avgRepFin5Result> RepFin5(DateTime? de, bool? withDog, string FinInstIDs)
    {
      return db.up_avgRepFin5(de, withDog, FinInstIDs);
    }

    public ISingleResult<up_avgRepFin6Result> RepFin6(DateTime? de, bool? withDog, string FinInstIDs)
    {
      return (ISingleResult<up_avgRepFin6Result>)db.up_avgRepFin6(de, withDog, FinInstIDs);
    }

    public IEnumerable<dynamic> getRepKM1(DateTime? d1, DateTime? d2)
    {
      var q = from t in
                (from e in db.pOrdersTrees
                 where e.DealDate >= d1 && e.DealDate <= d2
                   && e.InstrumentID != 10000000140
                   && (new decimal?[] { 20000000097, 20000000098, 20000000099 }).Contains(e.DepBrokerInstID)
                   && e.InfAgentID == null
                 group e by e.Seller into g
                 select new
                    {
                      Seller = g.Key,
                      QtyP = g.Sum(p => p.DealType == 1 ? p.Qty : 0),
                      QtyR = g.Sum(p => p.DealType == 0 ? p.Qty : 0)
                    })
              let s = db.taLibs.Where(p => p.LConcept == 482709 && p.LName == t.Seller).Select(p => new { a = 1 }).FirstOrDefault()
              let p = db.taLibs.Where(p => p.LConcept == 482710 && p.LName == t.Seller).Select(p => new { a = 1 }).FirstOrDefault()
              where s.a != null || p.a != null
              orderby t.Seller
              select new
              {
                t.Seller,
                t.QtyP,
                t.QtyR,
                Act = (s.a == 1 ? "Статус" : "") + (p.a == 1 ? "Приоритет" : "")
              }.ToExpando();
      return q;
    }

    public ISingleResult<up_avgRepUralsibCommisResult> RepUralsibCommis(DateTime d1, DateTime d2)
    {
      return db.up_avgRepUralsibCommis(d1, d2);
    }

    public ISingleResult<up_avgRepUralsibCommisLKResult> RepUralsibCommisLK(DateTime d1, DateTime d2)
    {
      return db.up_avgRepUralsibCommisLK(d1, d2);
    }

    public ISingleResult<up_avgRepUralsibInOutResult> RepUralsibInOut(DateTime d1, DateTime d2)
    {
      return db.up_avgRepUralsibInOut(d1, d2);
    }

    public IEnumerable<dynamic> getRepDURests(DateTime? d)
    {
      var q = from tr in db.tTreaties.Where(p => p.IsDisabled == 0 && p.DateFinish == new DateTime(1900, 1, 1) || p.DateFinish >= d.Value.AddYears(-1))
              join ttt in db.tTreatyTreatyTypes.Where(p => p.TreatyTypeID == 1) on tr.TreatyID equals ttt.TreatyID
              join fi in db.tFinancialInstitutions.Where(p => p.IsJuridicalPerson == 0) on tr.FinancialInstitutionID equals fi.FinancialInstitutionID
              join fia in db.tFinInstAttrs on tr.FinancialInstitutionID equals fia.FinInstID into _fia
              from fia in _fia.DefaultIfEmpty()
              join tv in db.tTreatyValues.Where(p => p.ValueDate == d /*&& p.ValueRUR > 100*/) on tr.TreatyID equals tv.TreatyID
              from mf in db.tObjClsRelations.Where(p => p.ObjClassifierID == 171 && p.ObjectID == tr.TreatyID && p.OnDate <= tv.ValueDate).OrderByDescending(p => p.OnDate).Select(p => new { p.Value }).Take(1).DefaultIfEmpty()
              from sf in db.tObjClsRelations.Where(p => p.ObjClassifierID == 172 && p.ObjectID == tr.TreatyID && p.OnDate <= tv.ValueDate).OrderByDescending(p => p.OnDate).Select(p => new { p.Value }).Take(1).DefaultIfEmpty()
              from of in db.tObjClsRelations.Where(p => p.ObjClassifierID == 66540 && p.ObjectID == tr.TreatyID && p.OnDate <= tv.ValueDate).OrderByDescending(p => p.OnDate).Select(p => new { p.Value }).Take(1).DefaultIfEmpty()
              from st in
                (
                  from ocr in db.tObjClsRelations
                  join oc in db.tObjClassifiers.Where(p => p.ParentID == 66541) on ocr.ObjClassifierID equals oc.ObjClassifierID
                  where ocr.ObjectID == tr.TreatyID
                  select new { oc.Name }
                  ).Take(1).DefaultIfEmpty()
              from dl in
                (
                  from d1 in db.tDeals
                  where d1.LeftSideID == tr.TreatyID && d1.DealTypeID == 10 && d1.DealDate == (db.tDeals.Where(p => p.LeftSideID == tr.TreatyID && p.DealTypeID == 10).OrderBy(p => p.DealDate).FirstOrDefault().DealDate)
                  group d1 by 1 into gr
                  select new { QtyB = gr.Sum(p => p.Amount) }
                  )
              from dln in
                (
                  from d1 in db.tDeals
                  where d1.LeftSideID == tr.TreatyID && d1.DealTypeID == 10 && d1.DealDate > (db.tDeals.Where(p => p.LeftSideID == tr.TreatyID && p.DealTypeID == 10).OrderBy(p => p.DealDate).FirstOrDefault().DealDate)
                  group d1 by 1 into gr
                  select new { QtyN = gr.Sum(p => p.Amount) }
                  ).DefaultIfEmpty()
              join ppz in db.pPPZs on tr.PPZ equals ppz.CODE into _ppz
              from ppz in _ppz.DefaultIfEmpty()
              select new
              {
                Client = fi.AddressActual,
                Qty = (double?)tv.ValueRUR,
                Number = fi.Name,
                fi.Phone1,
                tr.DateStart,
                fi.Email,
                //Strategy = pr.Name,
                MF = (double?)mf.Value,
                SF = (double?)sf.Value,
                OF = (double?)of.Value,
                ClManager = db.uf_avgClManagStr(tr.TreatyID),
                Strategy = st.Name,
                QtyB = (decimal?)dl.QtyB,
                tr.PPZ,
                ppz.DO,
                ppz.TD,
                StPr = db.uf_avgClMngStPrStr(tr.TreatyID),
                ppz.Cluster2,
                tr.DateFinish,
                trNumber = tr.Number,
                fia.BirthDay,
                QtyN = (decimal?)dln.QtyN
              };
      return q;
    }

    public IEnumerable<dynamic> getRepDUInOut(DateTime? d1, DateTime? d2, int Type)
    {
      var q = (from tr in db.tTreaties
               where ((tr.Number.StartsWith("УБ") || db.tObjClsRelations.Where(p => p.ObjClassifierID == 136 && p.ObjType == 1631275800).Select(p => p.ObjectID).Contains(tr.TreatyID)) && tr.IsDisabled == 0) || Type == 1
               join ttt in db.tTreatyTreatyTypes.Where(p => p.TreatyTypeID == 1) on tr.TreatyID equals ttt.TreatyID
               join d in db.tDeals.Where(p => p.DealDate >= d1 && p.DealDate <= d2 && ((p.DealTypeID == 10 && p.Direction == 1) || (p.DealTypeID == 3 && p.Direction == 0))) on tr.TreatyID equals d.LeftSideID
               join f in db.tSecurities on d.FundID equals f.SecurityID
               from rd in db.tRates.Where(p => p.TradeSystemID == 1 && p.RawDataProviderID == 1 && p.SecurityID == d.FundID && p.ActualizationDateTime <= d.DealDate).OrderByDescending(p => p.ActualizationDateTime).Take(1).DefaultIfEmpty()
               join fi in db.tFinancialInstitutions on tr.FinancialInstitutionID equals fi.FinancialInstitutionID
               join fia in db.tFinInstAttrs on tr.FinancialInstitutionID equals fia.FinInstID into _fia
               from fia in _fia.DefaultIfEmpty()
               from st in
                 (
                   from ocr in db.tObjClsRelations
                   join oc in db.tObjClassifiers.Where(p => p.ParentID == 66541) on ocr.ObjClassifierID equals oc.ObjClassifierID
                   where ocr.ObjectID == tr.TreatyID
                   select new { oc.Name, StrategyID = oc.ObjClassifierID }
                   ).Take(1).DefaultIfEmpty()
               join ppz in db.pPPZs on tr.PPZ equals ppz.CODE into _ppz
               from ppz in _ppz.DefaultIfEmpty()
               join dc1 in db.tDictionariesConnections.Where(p => p.Dictionary == 741604640) on tr.FinancialInstitutionID equals dc1.CompositeID
               where Type == 1 || !new int?[] { 68529 }.Contains(st.StrategyID)
               orderby d.DealDate, tr.Number
               select new
               {
                 ClientID = dc1.DiasoftBOID,
                 d.DealDate,
                 fi.Name,
                 Client = fi.IsJuridicalPerson == 1 ? fi.NameBrief.Trim() : fi.AddressActual,
                 fia.BirthDay,
                 BS = d.Direction == 1 ? "Ввод ДС" : "Вывод ДС",
                 Qty = (double?)d.Amount,
                 tr.Number,
                 tr.DateStart,
                 ClManager = db.uf_avgClManagStr(tr.TreatyID),
                 Strategy = st.Name,
                 tr.PPZ,
                 ppz.DO,
                 ppz.TD,
                 TabNum = db.uf_avgClMngTabNumStr(tr.TreatyID),
                 StPr = db.uf_avgClMngStPrStr(tr.TreatyID),
                 fi.ExternalID,
                 Portal = tr.FinancialInstitutionPortal,
                 ClientCode = fi.NameBrief.Trim(),
                 tr.DateFinish,
                 FundCourse = rd.CourseCurrent,
                 Fund = f.NameBrief
               }).Concat(from tr in db.tTreaties
                         where ((tr.Number.StartsWith("УБ") || db.tObjClsRelations.Where(p => p.ObjClassifierID == 136 && p.ObjType == 1631275800).Select(p => p.ObjectID).Contains(tr.TreatyID)) && tr.IsDisabled == 0) || Type == 1
                         join ttt in db.tTreatyTreatyTypes.Where(p => p.TreatyTypeID == 1) on tr.TreatyID equals ttt.TreatyID
                         join d in db.tDeals.Where(p => p.DealDate >= d1 && p.DealDate <= d2 && p.DealTypeID == 312) on tr.TreatyID equals d.LeftSideID
                         join f in db.tSecurities on d.FundID equals f.SecurityID
                         from rd in db.tRates.Where(p => p.TradeSystemID == 1 && p.RawDataProviderID == 1 && p.SecurityID == d.FundID && p.ActualizationDateTime <= d.DealDate).OrderByDescending(p => p.ActualizationDateTime).Take(1).DefaultIfEmpty()
                         join fi in db.tFinancialInstitutions on tr.FinancialInstitutionID equals fi.FinancialInstitutionID
                         join fia in db.tFinInstAttrs on tr.FinancialInstitutionID equals fia.FinInstID into _fia
                         from fia in _fia.DefaultIfEmpty()
                         from st in
                           (
                             from ocr in db.tObjClsRelations
                             join oc in db.tObjClassifiers.Where(p => p.ParentID == 66541) on ocr.ObjClassifierID equals oc.ObjClassifierID
                             where ocr.ObjectID == tr.TreatyID
                             select new { oc.Name, StrategyID = oc.ObjClassifierID }
                             ).Take(1).DefaultIfEmpty()
                         join ppz in db.pPPZs on tr.PPZ equals ppz.CODE into _ppz
                         from ppz in _ppz.DefaultIfEmpty()
                         join dc1 in db.tDictionariesConnections.Where(p => p.Dictionary == 741604640) on tr.FinancialInstitutionID equals dc1.CompositeID
                         orderby d.DealDate, tr.Number
                         select new
                         {
                           ClientID = dc1.DiasoftBOID,
                           d.DealDate,
                           fi.Name,
                           Client = fi.IsJuridicalPerson == 1 ? fi.NameBrief.Trim() : fi.AddressActual,
                           fia.BirthDay,
                           BS = d.Direction == 0 ? "Ввод ЦБ" : "Вывод ЦБ",
                           Qty = (double?)d.CouponQuantity,
                           tr.Number,
                           tr.DateStart,
                           ClManager = db.uf_avgClManagStr(tr.TreatyID),
                           Strategy = st.Name,
                           tr.PPZ,
                           ppz.DO,
                           ppz.TD,
                           TabNum = db.uf_avgClMngTabNumStr(tr.TreatyID),
                           StPr = db.uf_avgClMngStPrStr(tr.TreatyID),
                           fi.ExternalID,
                           Portal = tr.FinancialInstitutionPortal,
                           ClientCode = fi.NameBrief.Trim(),
                           tr.DateFinish,
                           FundCourse = rd.CourseCurrent,
                           Fund = f.NameBrief
                         });
      return q;
    }

    public IEnumerable<dynamic> getRepDUBank(DateTime? d1, DateTime? d2, int Type)
    {
      var d1a = d1.HasValue ? d1.Value.AddDays(-1) : d1;
      var dc = d1.HasValue ? d1.Value.AddDays(-60) : d1;
      var q = from tr in db.tTreaties
              where Type == 3 || tr.Number.StartsWith("УБ") || db.tObjClsRelations.Where(p => p.ObjClassifierID == 136 && p.ObjType == 1631275800).Select(p => p.ObjectID).Contains(tr.TreatyID)
              where tr.IsDisabled == 0 && (tr.DateFinish == new DateTime(1900, 1, 1) || tr.DateFinish >= dc)
              where (Type == 1 || Type == 3 || Type == 4 || (tr.Number.Contains("ДУИ")))
              where tr.ContractorID == 1790
              join op in db.tObjClsRelations.Where(p => p.ObjClassifierID == 791 && p.ObjType == 741604640) on tr.FinancialInstitutionID equals op.ObjectID into _op
              from op in _op.DefaultIfEmpty()
              where op.ObjClassifierID == null
              join ttt in db.tTreatyTreatyTypes.Where(p => p.TreatyTypeID == 1) on tr.TreatyID equals ttt.TreatyID
              join fi in db.tFinancialInstitutions on tr.FinancialInstitutionID equals fi.FinancialInstitutionID
              join fia in db.tFinInstAttrs on tr.FinancialInstitutionID equals fia.FinInstID into _fia
              from fia in _fia.DefaultIfEmpty()
              join tv in db.tTreatyValues.Where(p => p.ValueDate == d2) on tr.TreatyID equals tv.TreatyID into _tv
              from tv in _tv.DefaultIfEmpty()
              join tvp in db.tTreatyValues.Where(p => p.ValueDate == d1a) on tr.TreatyID equals tvp.TreatyID into _tvp
              from tvp in _tvp.DefaultIfEmpty()
              from rd in db.tRates.Where(p => p.TradeSystemID == 1 && p.RawDataProviderID == 1 && p.SecurityID == 39192 && p.ActualizationDateTime <= d2).OrderByDescending(p => p.ActualizationDateTime).Take(1).DefaultIfEmpty()
              from mf in db.tObjClsRelations.Where(p => p.ObjClassifierID == 171 && p.ObjectID == tr.TreatyID && p.OnDate <= d2).OrderByDescending(p => p.OnDate).Select(p => new { p.Value }).Take(1).DefaultIfEmpty()
              from sf in db.tObjClsRelations.Where(p => p.ObjClassifierID == 172 && p.ObjectID == tr.TreatyID && p.OnDate <= d2).OrderByDescending(p => p.OnDate).Select(p => new { p.Value }).Take(1).DefaultIfEmpty()
              from of in db.tObjClsRelations.Where(p => p.ObjClassifierID == 66540 && p.ObjectID == tr.TreatyID && p.OnDate <= d2).OrderByDescending(p => p.OnDate).Select(p => new { p.Value }).Take(1).DefaultIfEmpty()
              from tv2 in
                (from tv1 in db.tTreatyValues.Where(p => p.ValueDate >= d1 && p.ValueDate <= d2 && p.TreatyID == tr.TreatyID)
                 group tv1 by 1 into g
                 select new
                 {
                   QtyInR = g.Sum(p => p.CashflowTreatyRUR > 0 ? p.CashflowTreatyRUR : 0),
                   QtyOutR = g.Sum(p => p.CashflowTreatyRUR < 0 ? p.CashflowTreatyRUR : 0),
                   QtyInU = g.Sum(p => p.CashflowTreatyUSD > 0 ? p.CashflowTreatyUSD : 0),
                   QtyOutU = g.Sum(p => p.CashflowTreatyUSD < 0 ? p.CashflowTreatyUSD : 0)
                 }).DefaultIfEmpty()
              from st in
                (
                  from ocr in db.tObjClsRelations
                  join oc in db.tObjClassifiers.Where(p => p.ParentID == 66541) on ocr.ObjClassifierID equals oc.ObjClassifierID
                  where ocr.ObjectID == tr.TreatyID
                  select new { oc.Name, StrategyID = oc.ObjClassifierID }
                  ).Take(1).DefaultIfEmpty()
              join ppz in db.pPPZs on tr.PPZ equals ppz.CODE into _ppz
              from ppz in _ppz.DefaultIfEmpty()
              join vd in db.tObjClsRelations.Where(p => p.ObjClassifierID == 88302) on tr.TreatyID equals vd.ObjectID into _vd
              from vd in _vd.DefaultIfEmpty()
              let fund = vd.ObjClassifierID != null || new int?[] { 91723, 94484 }.Contains(st.StrategyID) ? "USD" : "RUB"
              where Type != 1 || !new int?[] { 68529,/* 68539,*/ 68540 /*68556, 68557, 68657, 68803, 72333, 72332, 74254, 71824, 81109, 91517, 91723, 94484, 98352*/ }.Contains(st.StrategyID)
              where Type != 4 || !new int?[] { 68529 }.Contains(st.StrategyID)
              orderby tr.DateStart, tr.Number
              select new
              {
                d1,
                d2,
                fi.Name,
                Client = fi.IsJuridicalPerson == 1 ? fi.NameBrief.Trim() : fi.AddressActual,
                fia.BirthDay,
                Qtyp = fund == "RUB" ? (double?)tvp.ValueRUR : (double?)tvp.ValueUSD,
                QtyAvg = (double?)(db.tTreatyValues.Where(p => p.ValueDate >= d1 && p.ValueDate <= d2 && p.TreatyID == tr.TreatyID).Select(p => new { Val = fund == "RUB" ? p.ValueRUR : p.ValueUSD }).Average(p => p.Val)),
                Qty = fund == "USD" ? (double?)tv.ValueUSD : (double?)tv.ValueRUR,
                //Income = (double?)(db.tTreatyValues.Where(p => p.ValueDate >= d1 && p.ValueDate <= d2 && p.TreatyID == tr.TreatyID && p.CashflowTreatyRUR > 0).Select(p => new { CF = fund == "RUB" ? p.CashflowTreatyRUR : p.CashflowTreatyUSD }).Sum(p => p.CF)),
                //Outcome = (double?)(db.tTreatyValues.Where(p => p.ValueDate >= d1 && p.ValueDate <= d2 && p.TreatyID == tr.TreatyID && p.CashflowTreatyRUR < 0).Select(p => new { CF = fund == "RUB" ? p.CashflowTreatyRUR : p.CashflowTreatyUSD }).Sum(p => p.CF)),
                Income = (double?)(fund == "RUB" ? tv2.QtyInR : tv2.QtyInU),
                Outcome = (double?)(fund == "RUB" ? tv2.QtyOutR : tv2.QtyOutU),
                IncomeR = (double?)tv2.QtyInR,
                OutcomeR = (double?)tv2.QtyOutR,
                tr.Number,
                tr.DateStart,
                MF = new int[] { 71824, 72332, 72333, 74254 }.Contains(st.StrategyID ?? 0) ? (tv.ValueRUR > 30000000 ? 0.5 : tv.ValueRUR > 10000000 ? 0.75 : 1) : (double?)mf.Value,
                MFV = new int[] { 71824, 72332, 72333, 74254 }.Contains(st.StrategyID ?? 0) ? (double?)(db.tTreatyValues.Where(p => p.ValueDate >= d1 && p.ValueDate <= d2 && p.TreatyID == tr.TreatyID).Sum(p => p.ValueRUR * (p.ValueRUR > 30000000 ? 0.5 : p.ValueRUR > 10000000 ? 0.75 : 1))) / 36500 : (double?)(db.tTreatyValues.Where(p => p.ValueDate >= d1 && p.ValueDate <= d2 && p.TreatyID == tr.TreatyID).Sum(p => p.ValueRUR)) * mf.Value / 36500,
                SF = st.StrategyID == 71824 ? 5.0 : st.StrategyID == 72332 ? 10.0 : st.StrategyID == 72333 ? 5.0 : st.StrategyID == 74254 ? 5.0 : (double?)sf.Value,
                OF = (double?)of.Value,
                OFV = (double?)(db.tDeals.Where(p => p.DealDate >= d1 && p.DealDate <= d2 && p.DealTypeID == 25 && p.LeftSideID == tr.TreatyID && p.M_S_Flag == 3 && !new decimal?[] { 20003115702m, 20003115703m, 20003115704m, 20003115706m }.Contains(db.tDictionariesConnections.FirstOrDefault(c => c.CompositeID == p.DealID && c.Dictionary == -461522885).DiasoftBOID)).Sum(p => p.Amount * (1 - 2 * p.Direction))),
                ClManager = db.uf_avgClManagStr(tr.TreatyID),
                Strategy = st.Name,
                tr.PPZ,
                ppz.DO,
                ppz.TD,
                TabNum = db.uf_avgClMngTabNumStr(tr.TreatyID),
                StPr = db.uf_avgClMngStPrStr(tr.TreatyID),
                fi.ExternalID,
                Portal = tr.FinancialInstitutionPortal,
                ClientCode = fi.NameBrief.Trim(),
                ppz.UPR,
                ppz.Cluster2,
                tr.DateFinish,
                FundCourse = fund == "USD" ? rd.CourseCurrent : (double?)1,
                Fund = fund
              };
      return q;
    }

    public IEnumerable<dynamic> getPifBankBuy(DateTime? d1, DateTime? d2)
    {
      var q = from o in db.pOrders
              where o.DealDate >= d1 && o.DealDate <= d2 && o.DealType == 1 && o.InstrumentID != 10000000140 && o.DepBrokerInstID == 20000000098 && o.nodeBrief != "Отказ" && o.Qty > 0
              join dc2 in db.tDictionariesConnections.Where(p => p.Dictionary == 741604640) on o.ClientID equals dc2.DiasoftBOID
              join dc1 in db.tDictionariesConnections.Where(p => p.Dictionary == 1104993180) on o.SecurityID equals dc1.DiasoftBOID
              join f in db.tFinancialInstitutions on dc2.CompositeID equals f.FinancialInstitutionID
              join fia in db.tFinInstAttrs on f.FinancialInstitutionID equals fia.FinInstID into _fia
              from fia in _fia.DefaultIfEmpty()
              join s in db.tSecurities on dc1.CompositeID equals s.SecurityID
              join p in db.pPPZs on o.PPZCode equals p.CODE into _p
              from p in _p.DefaultIfEmpty()
              select new
              {
                f.NameBrief,
                Name = f.IsJuridicalPerson == 0 ? f.Name + " " + f.NameLat + (f.NameState != "" ? " " + f.NameState : "") : f.Name,
                o.Qty,
                PIF = s.NameBrief,
                o.PPZCode,
                p.TD,
                o.Seller,
                f.Phone1,
                o.DealDate,
                o.Number,
                fia.BirthDay
              };
      return q;
    }

    public IEnumerable<dynamic> getPifRepmt(DateTime? d1, DateTime? d2)
    {
      var q = from o in db.pOrders
              where o.DealDate >= d1 && o.DealDate <= d2 && o.DealType == 0 && o.InstrumentID != 10000000140 && o.nodeBrief != "Отказ" && o.Qty > 0
              join dc2 in db.tDictionariesConnections.Where(p => p.Dictionary == 741604640) on o.ClientID2 ?? o.ClientID equals dc2.DiasoftBOID
              join dc1 in db.tDictionariesConnections.Where(p => p.Dictionary == 1104993180) on o.SecurityID equals dc1.DiasoftBOID
              join f in db.tFinancialInstitutions on dc2.CompositeID equals f.FinancialInstitutionID
              join fia in db.tFinInstAttrs on f.FinancialInstitutionID equals fia.FinInstID into _fia
              from fia in _fia.DefaultIfEmpty()
              join s in db.tSecurities on dc1.CompositeID equals s.SecurityID
              select new
              {
                f.NameBrief,
                Name = f.IsJuridicalPerson == 0 ? f.Name + " " + f.NameLat + (f.NameState != "" ? " " + f.NameState : "") : f.Name,
                Chanel = o.Number.Contains("КК") ? "КК" : o.ClientID2 != null ? "НД" : o.DepBrokerInstID == 20000000098 ? "Банк" : "УК",
                o.Qty,
                PIF = s.NameBrief,
                o.Seller,
                f.Phone1,
                o.DealDate,
                o.Number,
                fia.BirthDay
              };
      return q;
    }

    public bool AddUpdSurvey(SurveyViewModel data, string UserName)
    {
      var s = db.tSurveys.FirstOrDefault(p => p.UserName == UserName);
      try
      {
        if (s == null)
        {
          s = new tSurvey
          {
            UserName = UserName,
            a1 = data.a1,
            a2 = data.a2,
            a3 = data.a3,
            a4 = data.a4,
            a5 = data.a5,
            a6 = data.a6,
            a7 = data.a7,
            a8 = data.a8,
            a9 = data.a9,
            a10 = data.a10,
            a11 = data.a11,
            a12 = data.a12,
            a13 = data.a13,
            a14 = data.a14,
            a151 = data.a151,
            a152 = data.a152,
            a153 = data.a153,
            a154 = data.a154,
            a155 = data.a155,
            a156 = data.a156,
            a157 = data.a157,
            a158 = data.a158,
            a161 = data.a161,
            a162 = data.a162,
            a163 = data.a163,
            a164 = data.a164,
            a165 = data.a165,
            a166 = data.a166,
            a167 = data.a167,
            a168 = data.a168,
            a169 = data.a169,
            a1610 = data.a1610,
            a1611 = data.a1611,
            a1612 = data.a1612,
            a1613 = data.a1613,
            a1614 = data.a1614,
            a1615 = data.a1615,
            a1616 = data.a1616,
            a1617 = data.a1617,
            a17 = data.a17,
            a18 = data.a18,
            a19 = data.a19,
            a20 = data.a20,
            a21 = data.a21,
            n15 = data.n15,
            n16 = data.n16,
            n17 = data.n17,
            n18 = data.n18,
            n19 = data.n19,
            n20 = data.n20,
            n21 = data.n21,
            n73 = data.n73,
            n74 = data.n74,
            InDateTime = DateTime.Now
          };
          db.tSurveys.InsertOnSubmit(s);
        }
        else
        {
          s.a1 = data.a1;
          s.a2 = data.a2;
          s.a3 = data.a3;
          s.a4 = data.a4;
          s.a5 = data.a5;
          s.a6 = data.a6;
          s.a7 = data.a7;
          s.a8 = data.a8;
          s.a9 = data.a9;
          s.a10 = data.a10;
          s.a11 = data.a11;
          s.a12 = data.a12;
          s.a13 = data.a13;
          s.a14 = data.a14;
          s.a151 = data.a151;
          s.a152 = data.a152;
          s.a153 = data.a153;
          s.a154 = data.a154;
          s.a155 = data.a155;
          s.a156 = data.a156;
          s.a157 = data.a157;
          s.a158 = data.a158;
          s.a161 = data.a161;
          s.a162 = data.a162;
          s.a163 = data.a163;
          s.a164 = data.a164;
          s.a165 = data.a165;
          s.a166 = data.a166;
          s.a167 = data.a167;
          s.a168 = data.a168;
          s.a169 = data.a169;
          s.a1610 = data.a1610;
          s.a1611 = data.a1611;
          s.a1612 = data.a1612;
          s.a1613 = data.a1613;
          s.a1614 = data.a1614;
          s.a1615 = data.a1615;
          s.a1616 = data.a1616;
          s.a1617 = data.a1617;
          s.a17 = data.a17;
          s.a18 = data.a18;
          s.a19 = data.a19;
          s.a20 = data.a20;
          s.a21 = data.a21;
          s.n15 = data.n15;
          s.n16 = data.n16;
          s.n17 = data.n17;
          s.n18 = data.n18;
          s.n19 = data.n19;
          s.n20 = data.n20;
          s.n21 = data.n21;
          s.n73 = data.n73;
          s.n74 = data.n74;
          s.InDateTime = DateTime.Now;
        }
        db.SubmitChanges();
      }
      catch
      {
        return false;
      }
      return true;
    }

    public decimal GetValRate(int Id, decimal q)
    {
      var q1 = db.tOrdPayments.FirstOrDefault(p => p.ID == Id);
      if (q1 != null)
      {
        var fundId = q1.FundID ?? 39191;
        var dt = q1.DatePay ?? q1.DateCreate;
        var c = (decimal?)db.tRates.Where(p => p.TradeSystemID == 1 && p.RawDataProviderID == 1 && p.SecurityID == fundId && p.ActualizationDateTime <= dt).OrderByDescending(p => p.ActualizationDateTime).FirstOrDefault().CourseCurrent;
        return Math.Round(q * (c ?? 0m), 2);
      }
      return 0m;
    }

    public DateTime GetLastWorkDate()
    {
      return db.tWorkDates.OrderByDescending(p => p.WorkDate).FirstOrDefault(p => p.WorkDate < DateTime.Today).WorkDate;
    }

    public List<up_avgRep4BCSResult1> Rep4BCS1(DateTime? DateB, DateTime? DateE)
    {
      return db.up_avgRep4BCS(DateB, DateE, 1).GetResult<up_avgRep4BCSResult1>().ToList();
    }

    public List<up_avgRep4BCSResult2> Rep4BCS2(DateTime? DateE)
    {
      return db.up_avgRep4BCS(null, DateE, 2).GetResult<up_avgRep4BCSResult2>().ToList();
    }

    public List<up_avgRep4BCSResult3> Rep4BCS3(DateTime? DateB, DateTime? DateE)
    {
      return db.up_avgRep4BCS(DateB, DateE, 3).GetResult<up_avgRep4BCSResult3>().ToList();
    }

    public List<up_avgRep4BCSResult4> Rep4BCS4(DateTime? DateB, DateTime? DateE)
    {
      return db.up_avgRep4BCS(DateB, DateE, 4).GetResult<up_avgRep4BCSResult4>().ToList();
    }

    public List<up_avgRep4BCSResult5> Rep4BCS5(DateTime? DateB, DateTime? DateE)
    {
      return db.up_avgRep4BCS(DateB, DateE, 5).GetResult<up_avgRep4BCSResult5>().ToList();
    }

    public ISingleResult<up_avgRep4BCS2Result> Rep4BCS_2(DateTime? DateB, DateTime? DateE)
    {
      return db.up_avgRep4BCS2(DateB, DateE);
    }

    public IEnumerable<dynamic> GetBotPifRates()
    {
      var q = (from s in db.tSecurities
               where s.SecType == 6 && s.IsDeleted == 0 && new int?[] { 261, 262, 264, 265, 6626, 6629, 6630, 24145, 24681, 24683 }.Contains(s.IssuerID)
               from l in db.taLibs
               where l.LID == 482562
               from r in db.tRates.Where(p => p.SecurityID == s.SecurityID && p.RawDataProviderID == 1 && p.TradeSystemID == 446 && p.ActualizationDateTime <= l.LDate1).OrderByDescending(p => p.ActualizationDateTime).Take(1)
               from rp in db.tRates.Where(p => p.SecurityID == s.SecurityID && p.RawDataProviderID == 1 && p.TradeSystemID == 446 && p.ActualizationDateTime < r.ActualizationDateTime).OrderByDescending(p => p.ActualizationDateTime).Take(1)
               orderby s.IssuerID
               select new
               {
                 Brief = s.NameBrief,
                 Date = r.ActualizationDateTime,
                 DateP = rp.ActualizationDateTime,
                 PricePai = r.CourseCurrent,
                 PricePaiP = rp.CourseCurrent,
                 Scha = r.ValuePrice,
                 SchaP = rp.ValuePrice
               }).ToList().Select(p => new
               {
                 Brief = p.Brief.TrimEnd(),
                 date = p.Date.ToShortDateString(),
                 datep = p.DateP.ToShortDateString(),
                 pricePai = string.Format("{0:N2}", p.PricePai),
                 pricePaiP = string.Format("{0:N2}", p.PricePaiP),
                 pricePaiD = Math.Round(((p.PricePai / p.PricePaiP - 1) * 100).Value, 3, MidpointRounding.AwayFromZero),
                 scha = string.Format("{0:N2}", p.Scha),
                 schaP = string.Format("{0:N2}", p.SchaP),
                 schaD = Math.Round(((p.Scha / p.SchaP - 1) * 100).Value, 3, MidpointRounding.AwayFromZero)
               });
      return q;
    }

    public IEnumerable<dynamic> GetBotPifYield()
    {
      var d = db.taLibs.FirstOrDefault(p => p.LID == 482562).LDate1.Value;
      var dby = new DateTime(d.Year, 1, 1);
      var d1y = d.AddYears(-1);
      var d3y = d.AddYears(-3);
      var d5y = d.AddYears(-5);
      var q = (from s in db.tSecurities
               where s.SecType == 6 && s.IsDeleted == 0 && new int?[] { 261, 262, 264, 265, 6626, 6629, 6630, 24145, 24681, 24683 }.Contains(s.IssuerID)
               from r in db.tRates.Where(p => p.SecurityID == s.SecurityID && p.RawDataProviderID == 1 && p.TradeSystemID == 446 && p.ActualizationDateTime <= d).OrderByDescending(p => p.ActualizationDateTime).Take(1)
               from rpy in db.tRates.Where(p => p.SecurityID == s.SecurityID && p.RawDataProviderID == 1 && p.TradeSystemID == 446 && p.ActualizationDateTime < dby).OrderByDescending(p => p.ActualizationDateTime).Take(1)
               from rp1 in db.tRates.Where(p => p.SecurityID == s.SecurityID && p.RawDataProviderID == 1 && p.TradeSystemID == 446 && p.ActualizationDateTime < d1y).OrderByDescending(p => p.ActualizationDateTime).Take(1)
               from rp3 in db.tRates.Where(p => p.SecurityID == s.SecurityID && p.RawDataProviderID == 1 && p.TradeSystemID == 446 && p.ActualizationDateTime < d3y).OrderByDescending(p => p.ActualizationDateTime).Take(1)
               from rp5 in db.tRates.Where(p => p.SecurityID == s.SecurityID && p.RawDataProviderID == 1 && p.TradeSystemID == 446 && p.ActualizationDateTime < d5y).OrderByDescending(p => p.ActualizationDateTime).Take(1)
               orderby s.IssuerID
               select new
               {
                 Brief = s.NameBrief,
                 y = r.CourseCurrent,
                 yy = rpy.CourseCurrent,
                 y1 = rp1.CourseCurrent,
                 y3 = rp3.CourseCurrent,
                 y5 = rp5.CourseCurrent

               }).ToList().Select(p => new
               {
                 Brief = p.Brief.TrimEnd(),
                 yy = Math.Round(((p.y / p.yy - 1) * 100).Value, 3, MidpointRounding.AwayFromZero),
                 y1 = Math.Round(((p.y / p.y1 - 1) * 100).Value, 3, MidpointRounding.AwayFromZero),
                 y3 = Math.Round(((p.y / p.y3 - 1) * 100).Value, 3, MidpointRounding.AwayFromZero),
                 y5 = Math.Round(((p.y / p.y5 - 1) * 100).Value, 3, MidpointRounding.AwayFromZero)
               });
      return q;
    }

    public IEnumerable<dynamic> GetBotPifBranch()
    {
      return db.up_avgGetPifBranch();
    }

    public IEnumerable<dynamic> GetBotDuBranch()
    {
      return db.up_avgGetDuBranch();
    }

    public DateTime? GetLastPifDate()
    {
      return db.taLibs.First(p => p.LID == 482562).LDate1;
    }
    public int? GetFinInstID(string Brief)
    {
      return db.tFinancialInstitutions.FirstOrDefault(p => p.NameBrief == Brief).FinancialInstitutionID;
    }
    public IEnumerable<dynamic> StrategyChartData(int? strategyID)
    {
      return db.up_avgStrategyChartData(strategyID);
    }
  }
}
