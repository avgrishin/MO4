using System.Collections.Generic;
using System.Linq;
using MO.Models;
using MvcContrib.Sorting;
using System;

namespace MO.Areas.Code.Models
{
  public interface IObjClsRepository
  {
    IEnumerable<dynamic> GetObjClsNode(int ParentID);
    IEnumerable<dynamic> addObjCls(List<tObjClassifier> data);
    IEnumerable<dynamic> updObjCls(List<tObjClassifier> data);
    IEnumerable<dynamic> GetObjClsRel(int ObjClsID, string sort, string dir);
    bool DelObjClsRel(List<OCR> id);
  }

  public class ObjClsRepository : IObjClsRepository
  {
    private MiddleOfficeDataContext db = new MiddleOfficeDataContext();

    public IEnumerable<dynamic> GetObjClsNode(int ParentID)
    {
      return from oc in db.tObjClassifiers
             where oc.ParentID == ParentID
             orderby oc.ObjClassifierID
             select new
             {
               ObjClassifierID = oc.ObjClassifierID,
               oc.Name,
               //iconCls = "file",
               oc.NameBrief,
               oc.Comment,
               oc.ObjType,
               parentId = oc.ParentID,
               oc.UniqueFlag,
               oc.RequiredFlag,
               oc.OnDateFlag,
               oc.UserName,
               oc.InDateTime,
               qtip = oc.ObjClassifierID.ToString(),
               leaf = (from oc1 in db.tObjClassifiers
                       where oc1.ParentID == oc.ObjClassifierID
                       select 1).Count() == 0
             };
    }

    public IEnumerable<dynamic> addObjCls(List<tObjClassifier> data)
    {
      foreach (var e in data)
      {
        e.InDateTime = DateTime.Now;
      }
      db.tObjClassifiers.InsertAllOnSubmit(data);
      db.SubmitChanges();

      var ids = data.Select(p => p.ObjClassifierID);

      var q = from oc in db.tObjClassifiers.Where(p => ids.Contains(p.ObjClassifierID))
              select new
              {
                ObjClassifierID = oc.ObjClassifierID,
                oc.Name,
                //iconCls = "file",
                oc.NameBrief,
                oc.Comment,
                oc.ObjType,
                parentId = oc.ParentID,
                oc.UniqueFlag,
                oc.RequiredFlag,
                oc.OnDateFlag,
                oc.UserName,
                oc.InDateTime,
                qtip = oc.ObjClassifierID.ToString(),
                leaf = (from oc1 in db.tObjClassifiers
                        where oc1.ParentID == oc.ObjClassifierID
                        select 1).Count() == 0
              };
      return q;
    }

    public IEnumerable<dynamic> updObjCls(List<tObjClassifier> data)
    {
      foreach (var e in data)
      {
        var q1 = db.tObjClassifiers.FirstOrDefault(p => p.ObjClassifierID == e.ObjClassifierID);
        if (q1 != null)
        {
          q1.Name = e.Name ?? "";
          q1.NameBrief = e.NameBrief ?? "";
          q1.Comment = e.Comment ?? "";
          q1.ObjType = e.ObjType;
          q1.RequiredFlag = e.RequiredFlag;
          q1.UniqueFlag = e.UniqueFlag;
          q1.OnDateFlag = e.OnDateFlag;
          q1.UserName = e.UserName ?? "";
          q1.InDateTime = DateTime.Now;
          db.SubmitChanges();
        }
      }
      var ids = data.Select(p => p.ObjClassifierID);

      var q = from oc in db.tObjClassifiers.Where(p => ids.Contains(p.ObjClassifierID))
              select new
              {
                ObjClassifierID = oc.ObjClassifierID,
                oc.Name,
                //iconCls = "file",
                oc.NameBrief,
                oc.Comment,
                oc.ObjType,
                parentId = oc.ParentID,
                oc.UniqueFlag,
                oc.RequiredFlag,
                oc.OnDateFlag,
                oc.UserName,
                oc.InDateTime,
                qtip = oc.ObjClassifierID.ToString(),
                leaf = (from oc1 in db.tObjClassifiers
                        where oc1.ParentID == oc.ObjClassifierID
                        select 1).Count() == 0
              };
      return q;

    }

    public IEnumerable<dynamic> GetObjClsRel(int ObjClsID, string sort, string dir)
    {
      return (from ocr in db.tObjClsRelations
              where ocr.ObjClassifierID == ObjClsID
              select new
              {
                ObjClsRelationID = ocr.ObjClsRelationID,
                ObjClassifierID = ocr.ObjClassifierID,
                ObjectID = ocr.ObjectID,
                UserName = ocr.UserName,
                Comment = ocr.Comment,
                Value = ocr.Value,
                InDateTime = ocr.InDateTime,
                OnDate = ocr.OnDate,
                Obj = ocr.ObjType == 741604640 ?
                  db.tFinancialInstitutions.Where(f => f.FinancialInstitutionID == ocr.ObjectID).Select(f => f.NameBrief).First()
                  : ocr.ObjType == 1631275800 ?
                  db.tTreaties.Where(t => t.TreatyID == ocr.ObjectID).Select(t => t.NameBrief).First() :
                  ocr.ObjType == 1104993180 ?
                  db.tSecurities.Where(s => s.SecurityID == ocr.ObjectID).Select(s => s.Name1).First() :
                  ocr.ObjType == -2062882741 ?
                  db.tAccounts.Where(a => a.AccountID == ocr.ObjectID).Select(a => a.Brief).First() :
                  ocr.ObjType == -594782533 ?
                  db.tPortfolios.Where(p => p.PortfolioID == ocr.ObjectID).Select(p => p.NameBrief).First() :
                  ocr.ObjType == -801821404 ?
                  db.tTreatyTypes.Where(tt => tt.TreatyTypeID == ocr.ObjectID).Select(tt => tt.NameBrief).First() :
                  ocr.ObjType == -751446354 ?
                  db.tSecurityGroups.Where(sg => sg.SecurityGroupID == ocr.ObjectID).Select(sg => sg.NameBrief).First() :
                  ""
              }).OrderBy(sort, dir == "DESC" ? MvcContrib.Sorting.SortDirection.Descending : MvcContrib.Sorting.SortDirection.Ascending);
    }

    public bool DelObjClsRel(List<OCR> id)
    {
      try
      {
        var q = db.tObjClsRelations.Where(o => id.Contains(new OCR { ObjClsRelationID = o.ObjClsRelationID }));
        db.tObjClsRelations.DeleteAllOnSubmit(q);
        db.SubmitChanges();
        return true;
      }
      catch
      {
        return false;
      }
    }

  }
}