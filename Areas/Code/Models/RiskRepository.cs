using System;
using System.Collections.Generic;
using System.Linq;
using MO.Models;
using MvcContrib.Sorting;

namespace MO.Areas.Code.Models
{
  public interface IRiskRepository
  {
    IEnumerable<dynamic> getRiskEventsList(int? RiskType, string sort, string dir);
    IEnumerable<dynamic> addRiskEvent(List<tRiskEvent> data, string UserName);
    IEnumerable<dynamic> updRiskEvent(List<tRiskEvent> data, string UserName);
    bool delRiskEvent(List<tRiskEvent> data);
    IEnumerable<dynamic> GetObjClsByParent(int id);
    IEnumerable<dynamic> GetObjClsNode(int ParentID);
    bool LoadVar();
  }

  public class RiskRepository : IRiskRepository
  {
    private MiddleOfficeDataContext db = new MiddleOfficeDataContext() { CommandTimeout = 600 };

    public IEnumerable<dynamic> getRiskEventsList(int? RiskType, string sort, string dir)
    {
      var q = from e in db.tRiskEvents.Where(f => f.RiskType == RiskType)
              join c1 in db.tObjClassifiers on e.SourceID equals c1.ObjClassifierID into _c1
              from c1 in _c1.DefaultIfEmpty()
              join c2 in db.tObjClassifiers on e.TypeID equals c2.ObjClassifierID into _c2
              from c2 in _c2.DefaultIfEmpty()
              join c3 in db.tObjClassifiers on e.DirectID equals c3.ObjClassifierID into _c3
              from c3 in _c3.DefaultIfEmpty()
              join c4 in db.tObjClassifiers on e.IndirectID equals c4.ObjClassifierID into _c4
              from c4 in _c4.DefaultIfEmpty()
              select new
              {
                id = e.ID,
                e.CodeKIR,
                e.ConfirmDoc,
                e.Date1,
                e.Date2,
                e.Date3,
                e.Date4,
                e.Dep,
                e.DescrComp,
                e.DescrCP,
                e.IsCompensation,
                e.SourceID,
                SrcName = c1.Name,
                e.TypeID,
                TypePID = (int?)c2.ParentID,
                TypeName = c2.Name,
                e.DirectID,
                DirectName = c3.Name,
                e.QtyDir,
                e.QtyDirCompense,
                e.IndirectID,
                IndirectName = c4.Name,
                e.QtyIndir,
                e.QtyIndirCompense,
                e.Author,
                e.RiskType
              };

      if (sort != null) q = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending);
      return q;
    }

    public IEnumerable<dynamic> addRiskEvent(List<tRiskEvent> data, string UserName)
    {
      foreach (var d in data)
      {
        d.Date1 = d.Date1.HasValue ? d.Date1.Value.AddHours(1).Date : d.Date1;
        d.Date2 = d.Date2.HasValue ? d.Date2.Value.AddHours(1).Date : d.Date2;
        d.Date3 = d.Date3.HasValue ? d.Date3.Value.AddHours(1).Date : d.Date3;
        d.Date4 = d.Date4.HasValue ? d.Date4.Value.AddHours(1).Date : d.Date4;
        d.InDateTime = DateTime.Now;
        d.UserName = UserName;
      }
      db.tRiskEvents.InsertAllOnSubmit(data);
      db.SubmitChanges();

      var q = from e in db.tRiskEvents.Where(p => data.Select(n => n.ID).Contains(p.ID))
              join c1 in db.tObjClassifiers on e.SourceID equals c1.ObjClassifierID into _c1
              from c1 in _c1.DefaultIfEmpty()
              join c2 in db.tObjClassifiers on e.TypeID equals c2.ObjClassifierID into _c2
              from c2 in _c2.DefaultIfEmpty()
              join c3 in db.tObjClassifiers on e.DirectID equals c3.ObjClassifierID into _c3
              from c3 in _c3.DefaultIfEmpty()
              join c4 in db.tObjClassifiers on e.IndirectID equals c4.ObjClassifierID into _c4
              from c4 in _c4.DefaultIfEmpty()
              select new
              {
                id = e.ID,
                e.CodeKIR,
                e.ConfirmDoc,
                e.Date1,
                e.Date2,
                e.Date3,
                e.Date4,
                e.Dep,
                e.DescrComp,
                e.DescrCP,
                e.IsCompensation,
                e.SourceID,
                SrcName = c1.Name,
                e.TypeID,
                TypePID = (int?)c2.ParentID,
                TypeName = c2.Name,
                e.DirectID,
                DirectName = c3.Name,
                e.QtyDir,
                e.QtyDirCompense,
                e.IndirectID,
                IndirectName = c4.Name,
                e.QtyIndir,
                e.QtyIndirCompense,
                e.Author,
                e.RiskType
              };
      return q;
    }

    public IEnumerable<dynamic> updRiskEvent(List<tRiskEvent> data, string UserName)
    {
      foreach (var e in data.Where(p => p.ID > 0))
      {
        var q1 = db.tRiskEvents.Where(p => p.ID == e.ID).FirstOrDefault();
        if (q1 != null)
        {
          //var q2 = AutoMapper.Mapper.Map<tRiskEvent, tRiskEvent>(e);
          q1.Author = e.Author;
          q1.CodeKIR = e.CodeKIR;
          q1.ConfirmDoc = e.ConfirmDoc;
          q1.Date1 = e.Date1.HasValue ? e.Date1.Value.AddHours(1).Date : e.Date1;
          q1.Date2 = e.Date2.HasValue ? e.Date2.Value.AddHours(1).Date : e.Date2;
          q1.Date3 = e.Date3.HasValue ? e.Date3.Value.AddHours(1).Date : e.Date3;
          q1.Date4 = e.Date4.HasValue ? e.Date4.Value.AddHours(1).Date : e.Date4;
          q1.Dep = e.Dep;
          q1.DescrComp = e.DescrComp;
          q1.DescrCP = e.DescrCP;
          q1.InDateTime = DateTime.Now;
          q1.IsCompensation = e.IsCompensation;
          q1.SourceID = e.SourceID;
          q1.TypeID = e.TypeID;
          q1.DirectID = e.DirectID;
          q1.QtyDir = e.QtyDir;
          q1.QtyDirCompense = e.QtyDirCompense;
          q1.IndirectID = e.IndirectID;
          q1.QtyIndir = e.QtyIndir;
          q1.QtyIndirCompense = e.QtyIndirCompense;
          //db.tRiskEvents.Attach(q2, q1);
          db.SubmitChanges();
        }
      }
      //db.SubmitChanges();
      var q = from e in db.tRiskEvents.Where(p => data.Select(n => n.ID).Contains(p.ID))
              join c1 in db.tObjClassifiers on e.SourceID equals c1.ObjClassifierID into _c1
              from c1 in _c1.DefaultIfEmpty()
              join c2 in db.tObjClassifiers on e.TypeID equals c2.ObjClassifierID into _c2
              from c2 in _c2.DefaultIfEmpty()
              join c3 in db.tObjClassifiers on e.DirectID equals c3.ObjClassifierID into _c3
              from c3 in _c3.DefaultIfEmpty()
              join c4 in db.tObjClassifiers on e.IndirectID equals c4.ObjClassifierID into _c4
              from c4 in _c4.DefaultIfEmpty()
              select new
              {
                id = e.ID,
                e.CodeKIR,
                e.ConfirmDoc,
                e.Date1,
                e.Date2,
                e.Date3,
                e.Date4,
                e.Dep,
                e.DescrComp,
                e.DescrCP,
                e.IsCompensation,
                e.SourceID,
                SrcName = c1.Name,
                e.TypeID,
                TypePID = (int?)c2.ParentID,
                TypeName = c2.Name,
                e.DirectID,
                DirectName = c3.Name,
                e.QtyDir,
                e.QtyDirCompense,
                e.IndirectID,
                IndirectName = c4.Name,
                e.QtyIndir,
                e.QtyIndirCompense,
                e.Author,
                e.RiskType
              };
      return q;

    }

    public bool delRiskEvent(List<tRiskEvent> data)
    {
      try
      {
        var e = db.tRiskEvents.Where(p => data.Select(n => n.ID).Contains(p.ID));
        db.tRiskEvents.DeleteAllOnSubmit(e);
        db.SubmitChanges();
        return true;
      }
      catch (Exception /*ex*/)
      {
        return false;
      }
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

    public IEnumerable<dynamic> GetObjClsNode(int ParentID)
    {
      return (from oc in db.tObjClassifiers
              where oc.ParentID == ParentID
              orderby oc.ObjClassifierID
              select new
              {
                id = oc.ObjClassifierID,
                text = oc.Name,
                iconCls = "file",
                expanded = true,
                //qtip = oc.ObjClassifierID.ToString(),
                leaf = (from oc1 in db.tObjClassifiers
                        where oc1.ParentID == oc.ObjClassifierID
                        select 1).Count() == 0
              });
    }

    public bool LoadVar()
    {
      db.ExecuteCommand(@"exec msdb..sp_start_job @job_id='{25F2B3E6-F8F0-4827-8652-54675633E868}'
declare @l int = 1
while @l = 1
begin
	waitfor delay '0:0:10'
	if object_id ('tempdb..#jobs1') is not null
		drop table #jobs1
	select job_id into #jobs1 from openrowset('SQLNCLI', 'Server=assetsmgr;Trusted_Connection=yes;', 'exec msdb.dbo.sp_help_job @job_id=''{25F2B3E6-F8F0-4827-8652-54675633E868}'', @execution_status = 1, @job_aspect=''job''');
	set @l = case when exists(select 1 from #jobs1) then 1 else 0 end
end");
      return true;
    }

  }
}
