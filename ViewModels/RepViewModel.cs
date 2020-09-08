using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MO.Models;
using System.Web.UI.DataVisualization.Charting;
using System.Web.Mvc;

namespace MO.ViewModels
{

  public class Rep1ParamViewModel
  {
    public Rep1Param rep1Param;
    public cFinInst finInst { get; set; }
    public SelectList Funds { get; set; }
    public SelectList Indexes { get; set; }

    public Rep1ParamViewModel(IReportRepository repRepository, Rep1Param _rep1Param)
    {
      rep1Param = _rep1Param;
      if (rep1Param.finInstID != null)
        finInst = repRepository.GetFinInst(rep1Param.finInstID.Value);
      else
        finInst = new cFinInst();
      Funds = new SelectList(repRepository.GetFunds(), "SecurityID", "NameBrief", rep1Param.fundID);
      Indexes = new SelectList(repRepository.GetIndexes(), "ExchangeIndexID", "Brief", rep1Param.indexID);
    }
  }

  public class Rep1ViewModel
  {
    public Rep1Param rep1Param { get; set; }
    public up_avgRepClientResult2 rep1Cls2 { get; set; }
    public IEnumerable<up_avgRepClientResult3> rep1Cls3 { get; set; }
    public IEnumerable<up_avgRepClientResult6> rep1Cls6 { get; set; }
    public Chart chart0;
    public Chart chart1;
    public Chart chart7;
    public bool isShowAssets { get; set; }

    public Rep1ViewModel(IReportRepository repRepository, Rep1Param _rep1Param, bool _isShowAssets)
    {
      isShowAssets = _isShowAssets;
      rep1Param = _rep1Param;
      rep1Cls2 = repRepository.GetRepClient2(rep1Param.DateB, rep1Param.DateE, rep1Param.finInstID, rep1Param.fundID, rep1Param.indexID, rep1Param.strategyID);
      if (_isShowAssets)
        rep1Cls3 = repRepository.GetRepClient3(rep1Param.DateB, rep1Param.DateE, rep1Param.finInstID, rep1Param.fundID, rep1Param.indexID, rep1Param.strategyID);
      rep1Cls6 = repRepository.GetRepClient6(rep1Param.DateB, rep1Param.DateE, rep1Param.finInstID, rep1Param.fundID, rep1Param.indexID, rep1Param.strategyID);
    }
  }

}