using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Web.Mvc;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using System.Web.UI.DataVisualization.Charting;

namespace MO.Models
{
  public class Mission
  {
    public int? Option { get; set; }
    public string Comment { get; set; }
  }

  public class cFinInst
  {
    public int id { get; set; }
    public string brief { get; set; }
    public string name { get; set; }
  }

  public class Rep1Param
  {
    [Required(ErrorMessage = "Укажите Начальную дату")]
    [DisplayName("Начальная дата:")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
    [DataType(DataType.Date)]
    public DateTime? DateB { get; set; }

    [Required(ErrorMessage = "Укажите Конечную дату")]
    [DisplayName("Конечная дата:")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
    [CheckPeriod("DateB", ErrorMessage = "\"Начальная дата\" больше \"Конечной даты\".")]
    public DateTime? DateE { get; set; }

    [DisplayName("Клиент:")]
    [Required(ErrorMessage = "Укажите Клиента")]
    public int? finInstID { get; set; }

    [DisplayName("Валюта:")]
    [Required(ErrorMessage = "Укажите Валюту")]
    public int? fundID { get; set; }

    [DisplayName("Индекс:")]
    [Required(ErrorMessage = "Укажите Индекс")]
    public int? indexID { get; set; }

    [DisplayName("Стратегия:")]
    public int? strategyID { get; set; }

    public IReportRepository repRepository;
    public bool isBank { get; set; }
    public bool isPIF { get; set; }

    public IEnumerable<RuleViolation> GetRuleViolations()
    {
      if (finInstID == null)
        yield return new RuleViolation("*", "finInst.brief");
      if (finInstID != null && !repRepository.CheckFinInst(finInstID.Value, isBank, isPIF))
        yield return new RuleViolation("Недопустимое значение", "FinInstID");
      yield break;
    }

    public bool IsValid
    {
      get { return (GetRuleViolations().Count() == 0); }
    }
  }

  [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
  sealed public class CheckPeriodAttribute : ValidationAttribute, IClientValidatable
  {
    private const string _defaultErrorMessage = "\"{0}\" больше \"{1}\".";

    public CheckPeriodAttribute(string otherProperty)
      : base(_defaultErrorMessage)
    {
      if (otherProperty == null)
      {
        throw new ArgumentNullException("otherProperty");
      }
      OtherProperty = otherProperty;
    }

    public string OtherProperty { get; private set; }

    public override string FormatErrorMessage(string name)
    {
      return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, OtherProperty);
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      PropertyInfo otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
      if (otherPropertyInfo == null)
      {
        return new ValidationResult(String.Format(CultureInfo.CurrentCulture, "Неизвестный свойство \"{0}\"", OtherProperty));
      }

      object otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
      if ((DateTime)value < (DateTime)otherPropertyValue)
      {
        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
      }
      return null;
    }

    public static string FormatPropertyForClientValidation(string property)
    {
      if (property == null)
      {
        throw new ArgumentException("Value cannot be null or empty. {0}", "property");
      }
      return "*." + property;
    }

    public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
    {
      yield return new ModelClientValidationDateGreateOrEqualToRule(FormatErrorMessage(metadata.GetDisplayName()), FormatPropertyForClientValidation(OtherProperty));
    }
  }

  public class ModelClientValidationDateGreateOrEqualToRule : ModelClientValidationRule
  {
    public ModelClientValidationDateGreateOrEqualToRule(string errorMessage, object other)
    {
      ErrorMessage = errorMessage;
      ValidationType = "checkperiod";
      ValidationParameters["other"] = other;
    }
  }

  public class Rep3Param
  {
    [Required(ErrorMessage = "Укажите Начальную дату")]
    [DisplayName("Начальная дата:")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
    [DataType(DataType.Date)]
    public DateTime? DateB { get; set; }

    [Required(ErrorMessage = "Укажите Конечную дату")]
    [DisplayName("Конечная дата:")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
    [CheckPeriod("DateB", ErrorMessage = "\"Начальная дата\" больше \"Конечной даты\".")]
    public DateTime? DateE { get; set; }
  }

  public class Rep4Param : IValidatableObject
  {
    [Required(ErrorMessage = "Укажите Дату")]
    [DisplayName("Дата:")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
    [DataType(DataType.Date)]
    public DateTime? d { get; set; }


    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      yield break;
    }
  }

  public class up_avgRepClientResult0
  {
    public string Name { get; set; }
    public double? Qty { get; set; }
  }

  public class up_avgRepClientResult1
  {
    public string Name { get; set; }
    public double? Qty { get; set; }
  }

  public partial class up_avgRepClientResult2
  {
    public Int32? id { get; set; }
    public DateTime? db { get; set; }
    public DateTime? de { get; set; }
    public Int16? spid { get; set; }
    public string fiName { get; set; }
    public string Fund { get; set; }
    public Decimal? Qty { get; set; }
    public string IndexName { get; set; }
  }

  public partial class up_avgRepClientResult3
  {
    public string SecTypeName { get; set; }
    public string SecName { get; set; }
    public string AccType { get; set; }
    public Double? Qty { get; set; }
    public Double? Num { get; set; }
    public Double? Price { get; set; }
    public Double? Coupon { get; set; }
    public Byte? SecType { get; set; }
    public Double? Perc { get; set; }
    public string Brand { get; set; }
    public string ISIN { get; set; }
  }

  public class up_avgRepClientResult6
  {
    public int? id { get; set; }
    public string title { get; set; }
    public double? y { get; set; }
    public double? d30 { get; set; }
    public double? d7 { get; set; }
    public DateTime? db { get; set; }
  }

  public class up_avgRepClientResult7
  {
    public DateTime? Date2 { get; set; }
    public double? coef_all { get; set; }
    public double? coef_allb { get; set; }
  }
  public class up_avgStrategyChartDataResult
  {
    public DateTime? Date2 { get; set; }
    public double? coef_all { get; set; }
  }
  public class up_avgRepClientResult20
  {
    public string Name { get; set; }
    public string NameRus { get; set; }
    public double? Qty { get; set; }
  }

  public class up_avgRepClientResult21
  {
    public string NameLat { get; set; }
    public string NameRus { get; set; }
    public decimal? Perc { get; set; }
  }

  public class up_avgRepClientResult22
  {
    public DateTime? Date2 { get; set; }
    public double? coef_all { get; set; }
  }

  public class up_avgRepClientResult24
  {
    public DateTime? Date2 { get; set; }
    public double? coef_all { get; set; }
  }

  public class up_avgRepClientResult23
  {
    public string fm1 { get; set; }
    public string fy1 { get; set; }
    public string fy3 { get; set; }
    public string fy5 { get; set; }
    public string fy10 { get; set; }
    public string im1 { get; set; }
    public string iy1 { get; set; }
    public string iy3 { get; set; }
    public string iy5 { get; set; }
    public string iy10 { get; set; }
    public double? c { get; set; }
    public string des { get; set; }
    public DateTime? de { get; set; }
  }

  public class up_avgRepClientResult25
  {
    public string fm1 { get; set; }
    public string fy1 { get; set; }
    public string fy3 { get; set; }
    public string fy5 { get; set; }
    public string fy10 { get; set; }
    public string fb { get; set; }
    public string im1 { get; set; }
    public string iy1 { get; set; }
    public string iy3 { get; set; }
    public string iy5 { get; set; }
    public string iy10 { get; set; }
    public string ib { get; set; }
    public double? c { get; set; }
    public string des { get; set; }
    public DateTime? de { get; set; }
  }

  public class up_avgRepClientResult26
  {
    public double? Vol { get; set; }
    public double? Sharpe { get; set; }
    public double? Correl { get; set; }
    public double? TrackErr { get; set; }
    public double? Alpha { get; set; }
    public double? Beta { get; set; }
    public double? InfRatio { get; set; }
    public double? Volb { get; set; }
    public double? Sortino { get; set; }
  }

  public class up_avgRep4BCSResult1
  {
    public decimal? ID { get; set; }
    public string Name { get; set; }
    public string Product { get; set; }
    public byte IsJuridicalPerson { get; set; } 
  }

  public class up_avgRep4BCSResult2
  {
    public decimal? ID { get; set; }
    public string Product { get; set; }
    public string Active { get; set; }
    public string ISIN { get; set; }
    public double? Rest { get; set; }
    public double? Course { get; set; }
    public double? Qty { get; set; }
    public double? Rate { get; set; }
    public double? QtyRur { get; set; }
    public string Portal { get; set; }
  }

  public class up_avgRep4BCSResult3
  {
    public decimal? ID { get; set; }
    public string Product { get; set; }
    public string Active { get; set; }
    public string ISIN { get; set; }
    public decimal? Num { get; set; }
    public DateTime? Date { get; set; }
    public string Fund { get; set; }
    public double? Qty { get; set; }
    public double? Rate { get; set; }
    public double? QtyRur { get; set; }
  }

  public class up_avgRep4BCSResult4
  {
    public decimal? ID { get; set; }
    public string Product { get; set; }
    public string Type { get; set; }
    public double? Qty { get; set; }
    public DateTime? Date { get; set; }
    public string Portal { get; set; }
  }

  public class up_avgRep4BCSResult5
  {
    public decimal? ID { get; set; }
    public string Product { get; set; }
    public string ClientType { get; set; }
    public string TD { get; set; }
    public DateTime? Date { get; set; }
    public double? Income { get; set; }
    public double? Outcome { get; set; }
  }

  public class up_avgGetPifBranchResult
  {
    public string Brief { get; set; }
    public string Value { get; set; }
  }
  
  public class up_avgGetEmitentEvents2
  {
    public int? ID { get; set; }
    public Int64? Status { get; set; }
    public string INN { get; set; }
    public DateTime? PublishingDate { get; set; }
    public string NameFin { get; set; }
    public string NameBiefFin { get; set; }
    public string EventID { get; set; }
    public string RegNumber { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? DecisionDate { get; set; }
    public DateTime? ProtocolDate { get; set; }
    public DateTime? PerformanceDate { get; set; }
    public DateTime? VSAData { get; set; }
    public DateTime? SrezData { get; set; }
    public double? RateDividend { get; set; }
    public string SecType { get; set; }
    public int? c { get; set; }
    public Boolean? Is_UK { get; set; }
    public DateTime? BLDate { get; set; }
    public DateTime? MessDate { get; set; }
    public DateTime? NoticeDate { get; set; }
    public string FileName { get; set; }
    public int? FinInstID { get; set; }
  }

  public class up_avgGetEmitentEvents3
  {
    public int? ID { get; set; }
    public Int64? Status { get; set; }
    public DateTime? PublishingDate { get; set; }
    public string NameFin { get; set; }
    public string NameBiefFin { get; set; }
    public string INN { get; set; }
    public string EventID { get; set; }
    public string RegNumber { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? ListDate { get; set; }
    public string PurposeList { get; set; }
    public DateTime? ProtocolDate { get; set; }
    public string RegNumber2 { get; set; }
    public string TypeSec { get; set; }
    public string TypeSec2 { get; set; }
    public string IDFin { get; set; }
    public string ITOG_UK { get; set; }
    public DateTime? FinishDate { get; set; }
    public DateTime? DecisionDate { get; set; }
    public int? c { get; set; }
    public Boolean? Is_UK { get; set; }
    public DateTime? BLDate { get; set; }
    public DateTime? MessDate { get; set; }
    public DateTime? NoticeDate { get; set; }
    public string FileName { get; set; }
    public int? FinInstID { get; set; }
  }

  public class up_avgGetEmitentEvents4
  {
    public int? ID { get; set; }
    public Int64? Status { get; set; }
    public DateTime? PublishingDate { get; set; }
    public string NameFin { get; set; }
    public string NameBiefFin { get; set; }
    public string INN { get; set; }
    public string EventID { get; set; }
    public string RegNumber { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? DecisionDate { get; set; }
    public DateTime? FinishDate { get; set; }
    public int? c { get; set; }
    public Boolean? Is_UK { get; set; }
    public DateTime? BLDate { get; set; }
    public DateTime? MessDate { get; set; }
    public DateTime? NoticeDate { get; set; }
    public string FileName { get; set; }
    public int? FinInstID { get; set; }
    public DateTime? ListDate { get; set; }
  }

  public class up_avgGetEmitentEvents5
  {
    public int? ID { get; set; }
    public Int64? Status { get; set; }
    public DateTime? PublishingDate { get; set; }
    public string NameFin { get; set; }
    public string NameBiefFin { get; set; }
    public string INN { get; set; }
    public string EventID { get; set; }
    public string RegNumber { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? DecisionDate { get; set; }
    public int? c { get; set; }
    public Boolean? Is_UK { get; set; }
    public DateTime? BLDate { get; set; }
    public DateTime? MessDate { get; set; }
    public DateTime? NoticeDate { get; set; }
    public string FileName { get; set; }
    public int? FinInstID { get; set; }
  }

  public class up_avgGetEmitentEvents95
  {
    public int? ID { get; set; }
    public Int64? Status { get; set; }
    public DateTime? PublishingDate { get; set; }
    public string NameFin { get; set; }
    public string NameBiefFin { get; set; }
    public string INN { get; set; }
    public string EventID { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? DecisionDate { get; set; }
    public DateTime? ProtocolDate { get; set; }
    public int? c { get; set; }
    public Boolean? Is_UK { get; set; }
    public DateTime? BLDate { get; set; }
    public DateTime? MessDate { get; set; }
    public DateTime? NoticeDate { get; set; }
    public string FileName { get; set; }
    public int? FinInstID { get; set; }
  }

  public partial class up_avgRepPIFStatResult
  {

    public decimal? SecurityID { get; set; }
    public string PIF { get; set; }
    public int? AccCnt { get; set; }
    public int? AccCntDep { get; set; }
    public int? p0 { get; set; }
    public int? p1 { get; set; }
    public int? p2 { get; set; }
    public int? p3 { get; set; }
    public int? p4 { get; set; }
    public int? p5 { get; set; }
    public int? r0 { get; set; }
    public int? r00 { get; set; }
    public int? r01 { get; set; }
    public int? r1 { get; set; }
    public int? r11 { get; set; }
    public int? s0 { get; set; }
    public int? s01 { get; set; }
    public int? d0 { get; set; }
    public int? d00 { get; set; }
  }

  public partial class up_avgRepPIFStat1Result
  {

    public decimal? ClientID { get; set; }
    public string Client { get; set; }
    public decimal? SecurityID { get; set; }
    public string PIF { get; set; }
    public int? Days { get; set; }
    public decimal? Rest { get; set; }
    public int? Dep { get; set; }
    public string Type1 { get; set; }
    public string Type2 { get; set; }
    public string Type3 { get; set; }
  }

  public class ClnEvents
  {
    public int? ID { get; set; }
    public Boolean? Is_UK { get; set; }
    public DateTime? BLDate { get; set; }
    public string FileName { get; set; }
    public int? FinInstID { get; set; }
  }

  public partial class MiddleOfficeDataContext : System.Data.Linq.DataContext
  {
    [Function(Name = "dbo.up_avgRepClient")]
    [ResultType(typeof(up_avgRepClientResult0))]
    [ResultType(typeof(up_avgRepClientResult1))]
    [ResultType(typeof(up_avgRepClientResult2))]
    [ResultType(typeof(up_avgRepClientResult3))]
    [ResultType(typeof(up_avgRepClientResult6))]
    [ResultType(typeof(up_avgRepClientResult7))]
    [ResultType(typeof(up_avgRepClientResult20))]
    [ResultType(typeof(up_avgRepClientResult21))]
    [ResultType(typeof(up_avgRepClientResult22))]
    public IMultipleResults up_avgRepClient([Parameter(Name = "DateB", DbType = "SmallDateTime")] System.Nullable<System.DateTime> dateB, [Parameter(Name = "DateE", DbType = "SmallDateTime")] System.Nullable<System.DateTime> dateE, [Parameter(Name = "FinInstID", DbType = "Int")] System.Nullable<int> finInstID, [Parameter(Name = "FundID", DbType = "Int")] System.Nullable<int> fundID, [Parameter(Name = "Type", DbType = "TinyInt")] System.Nullable<byte> type, [Parameter(Name = "IndexID", DbType = "Int")] System.Nullable<int> indexID, [Parameter(Name = "StrategyID", DbType = "Int")] System.Nullable<int> strategyID, [Parameter(Name = "PortfolioID", DbType = "Int")] System.Nullable<int> portfolioID, [Parameter(Name = "CalcMethod", DbType = "TinyInt")] System.Nullable<byte> calcMethod, [Parameter(Name = "SecurityID", DbType = "Int")] System.Nullable<int> securityID, [Parameter(DbType = "TinyInt")] System.Nullable<byte> debug)
    {
      IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), dateB, dateE, finInstID, fundID, type, indexID, strategyID, portfolioID, calcMethod, securityID, debug);
      return ((IMultipleResults)(result.ReturnValue));
    }

    [Function(Name = "dbo.up_avgGetEmitentEvents")]
    [ResultType(typeof(up_avgGetEmitentEvents2))]
    [ResultType(typeof(up_avgGetEmitentEvents3))]
    [ResultType(typeof(up_avgGetEmitentEvents4))]
    [ResultType(typeof(up_avgGetEmitentEvents5))]
    [ResultType(typeof(up_avgGetEmitentEvents95))]
    public IMultipleResults up_avgGetEmitentEvents([Parameter(Name = "FinInstID", DbType = "int")] System.Nullable<int> FinInstID, [Parameter(Name = "SecurityID", DbType = "int")] System.Nullable<int> SecurityID, [Parameter(Name = "DateID", DbType = "Int")] System.Nullable<int> dateID, [Parameter(Name = "d1", DbType = "SmallDateTime")] System.Nullable<System.DateTime> d1, [Parameter(Name = "d2", DbType = "SmallDateTime")] System.Nullable<System.DateTime> d2, [Parameter(Name = "all", DbType = "bit")] System.Nullable<System.Boolean> all, [Parameter(Name = "isuk", DbType = "bit")] System.Nullable<System.Boolean> isuk, [Parameter(Name = "n", DbType = "varchar(255)")] string n, [Parameter(Name = "start", DbType = "Int")] System.Nullable<int> start, [Parameter(Name = "limit", DbType = "Int")] System.Nullable<int> limit, [Parameter(Name = "cnt", DbType = "Int")] ref System.Nullable<int> cnt)
    {
      IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), FinInstID, SecurityID, dateID, d1, d2, all, isuk, n, start, limit, cnt);
      cnt = ((System.Nullable<int>)(result.GetParameterValue(9)));

      return ((IMultipleResults)(result.ReturnValue));
    }

    [Function(Name = "dbo.up_avgRepPIFStat")]
    [ResultType(typeof(up_avgRepPIFStatResult))]
    [ResultType(typeof(up_avgRepPIFStat1Result))]
    public IMultipleResults up_avgRepPIFStat([Parameter(Name = "d1", DbType = "SmallDateTime")] System.Nullable<System.DateTime> d1, [Parameter(Name = "d2", DbType = "SmallDateTime")] System.Nullable<System.DateTime> d2, [Parameter(Name = "Type", DbType = "TinyInt")] System.Nullable<byte> type)
    {
      IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), d1, d2, type);
      return ((IMultipleResults)(result.ReturnValue));
    }

    [Function(Name = "dbo.up_avgRep4BCS")]
    [ResultType(typeof(up_avgRep4BCSResult1))]
    [ResultType(typeof(up_avgRep4BCSResult2))]
    [ResultType(typeof(up_avgRep4BCSResult3))]
    [ResultType(typeof(up_avgRep4BCSResult4))]
    [ResultType(typeof(up_avgRep4BCSResult5))]
    public IMultipleResults up_avgRep4BCS([Parameter(Name = "DateB", DbType = "SmallDateTime")] System.Nullable<System.DateTime> dateB, [Parameter(Name = "DateE", DbType = "SmallDateTime")] System.Nullable<System.DateTime> dateE, [Parameter(Name = "Type", DbType = "Int")] System.Nullable<int> type)
    {
      IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), dateB, dateE, type);
      return ((IMultipleResults)(result.ReturnValue));
    }

    [Function(Name = "up_avgGetPifBranch")]
    public ISingleResult<up_avgGetPifBranchResult> up_avgGetPifBranch()
    {
      IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())));
      return ((ISingleResult<up_avgGetPifBranchResult>)(result.ReturnValue));
    }

    [Function(Name = "up_avgGetDuBranch")]
    public ISingleResult<up_avgGetPifBranchResult> up_avgGetDuBranch()
    {
      IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())));
      return ((ISingleResult<up_avgGetPifBranchResult>)(result.ReturnValue));
    }

    [Function(Name = "up_avgStrategyChartData")]
    public ISingleResult<up_avgStrategyChartDataResult> up_avgStrategyChartData([Parameter(Name = "StrategyID", DbType = "Int")] System.Nullable<int> strategyID)
    {
      IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), strategyID);
      return ((ISingleResult<up_avgStrategyChartDataResult>)(result.ReturnValue));
    }
  }

  public class SaveFinRep
  {
    public int? ocp { get; set; }
    public int? oc { get; set; }
  }

  public class OrdCtrl
  {
    public int? ID { get; set; }
    public string Number { get; set; }
    public int? FinInstID { get; set; }
    public string Receiver { get; set; }
    public string Item { get; set; }
    public string PFP { get; set; }
    public string Comment { get; set; }
    public string Qty { get; set; }
    public System.DateTime DateCreate { get; set; }
    public System.DateTime DateDef { get; set; }
    public System.DateTime DateBuh { get; set; }
    public System.DateTime DatePay { get; set; }
    public int? ExecutorID { get; set; }
    public int? DepartID { get; set; }
    public string PlatNumb { get; set; }
  }

  public class ObjClsNode
  {
    public int? id { get; set; }
    public string text { get; set; }
    public string classes { get; set; }
    public bool hasChildren { get; set; }
  }

  /*
  public class FinAct
  {
    public int? ID { get; set; }
    public int? ObjClsID { get; set; }
    public string Name { get; set; }
    public int? Quartier { get; set; }
    public int? An { get; set; }
    public decimal? QtyP { get; set; }
  }
   * */
  public class FinPartViewModel
  {
    public int? TreatyID { get; set; }
    public int? ObjClsID { get; set; }
    public int? ID1 { get; set; }
    public int? ID2 { get; set; }
    public int? ID3 { get; set; }
    public int? ID4 { get; set; }
    public double? Value1 { get; set; }
    public double? Value2 { get; set; }
    public double? Value3 { get; set; }
    public double? Value4 { get; set; }
    public DateTime? Dt { get; set; }
  }

  public class RepParam17
  {
    public DateTime? d1 { get; set; }
    public DateTime? d2 { get; set; }
    public int? cid { get; set; }
  }

  public partial class tOrdPayment1 : tOrdPayment
  {
    public int? CopyID { get; set; }
  }

  public class ValRates
  {
    public string s { get; set; }
    public double? c { get; set; }
    public DateTime dt { get; set; }
    public double y { get; set; }
  }
}