using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcContrib.Sorting;
using MO.Models;

namespace MO.Areas.Code.Controllers
{
  public class InvestDeclController : Controller
  {

    [Authorize(Roles = "admin")]
    public ActionResult Index()
    {
      return View();
    }

    [Authorize(Roles = "admin")]
    public ActionResult InvestDeclList(string sort, string dir, char? enb, int? type)
    {
      MiddleOfficeDataContext db = new MiddleOfficeDataContext();
      var q = (from i in db.sInvestmentDeclarations
               orderby i.InvestmentDeclarationID
               select new
               {
                 InvestDeclID = i.InvestmentDeclarationID,
                 Name = i.Name == "" ? "&lt;пусто&gt;" : (i.Name ?? "&lt;null&gt;"),
                 Enb = i.Enb,
                 IDType = i.InvestmentDeclarationTypeID,
                 CreateDate = i.Create_Date,
                 ModifyDate = i.Modify_Date,
                 Type = i.sInvestmentDeclarationType.NameType
               });
      if (enb.HasValue)
      {
        q = q.Where(a => a.Enb == enb);
      }
      if (type.HasValue)
      {
        q = q.Where(a => a.IDType == type);
      }
      return Json(new { data = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending), totalCount = q.Count() });
    }

    [Authorize(Roles = "admin, bank, mo")]
    public ActionResult InvestDeclTypeList()
    {
      MiddleOfficeDataContext db = new MiddleOfficeDataContext();
      var q = (from i in db.sInvestmentDeclarationTypes
               select new { Text = i.NameType, Value = i.InvestmentDeclarationTypeID });
      return Json(new { data = q });
    }

    [Authorize(Roles = "admin")]
    public ActionResult InvestDeclWhere(int id, string sort, string dir, char? enb)
    {
      MiddleOfficeDataContext db = new MiddleOfficeDataContext();
      var q = (from i in db.sInvestmentDeclarationWheres
               join id1 in db.sInvestmentDeclarations on i.InvestmentDeclarationID equals id1.InvestmentDeclarationID
               where i.InvestmentDeclarationID == id
               orderby i.InvestmentDeclarationWhereID
               select new
               {
                 InvestDeclID = i.InvestmentDeclarationID,
                 InvestDeclWhereID = i.InvestmentDeclarationWhereID,
                 Name = i.NameWhere,
                 NameDecl = id1.Name,
                 Enb = i.Enb,
                 StartValue = i.StartValue,
                 StopValue = i.StopValue,
                 FLAG_Calculation = i.FLAG_Calculation,
                 FLAG_Group = i.FLAG_Group,
                 FLAG_Type = i.FLAG_Type,
                 FLAG_Long = i.FLAG_Long
               });
      if (enb.HasValue)
      {
        q = q.Where(a => a.Enb == enb);
      }
      return Json(new { data = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending) });
    }

    [Authorize(Roles = "admin")]
    public ActionResult InvestDeclSec(int id, string sort, string dir, int? div)
    {
      MiddleOfficeDataContext db = new MiddleOfficeDataContext();
      var q = (from i in db.sInvestmentDeclarationSecurities
               where i.InvestmentDeclarationWhereID == id
               orderby i.FLAG_Div, i.Number
               select new
               {
                 InvestDeclWhereID = i.InvestmentDeclarationWhereID,
                 InvestDeclSecID = i.InvestmentDeclarationSecurityID,
                 Number = i.Number,
                 FLAG_Div = i.FLAG_Div,
                 FLAG_Not = i.FLAG_Not,
                 ObjClassifierID = i.ObjClassifierID,
                 SecurityGroupID = i.SecurityGroupID,
                 SecurityID = i.SecurityID,
                 FinancialInstitutionID = i.FinancialInstitutionID,
                 Name = i.SecurityGroupID != null ? i.tSecurityGroup.Name : i.SecurityID != null ? i.tSecurity.Name1 : i.FinancialInstitutionID != null ? i.tFinancialInstitution.NameBrief : i.ObjClassifierID != null ? i.tObjClassifier.Name : ""
               });
      if (div.HasValue)
      {
        q = q.Where(a => a.FLAG_Div == div);
      }
      return Json(new { data = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending) });
    }

    [Authorize(Roles = "admin")]
    public ActionResult SecGroup(int id, string sort, string dir, char? enb, int? start, int? limit, string n)
    {
      MiddleOfficeDataContext db = new MiddleOfficeDataContext();
      var q = (from s in db.tSecuritySecurityGroups
               where s.SecurityGroupID == id
               orderby s.tSecurity.Name1
               select new
               {
                 Auto_Enb = s.AUTO_ENB,
                 Enb = s.ENB,
                 StartDate = s.StartDate,
                 FinishDate = s.FinishDate,
                 OnLine = s.OnLine,
                 SecName = s.tSecurity.Name1,
                 SecurityGroupID = s.SecurityGroupID,
                 SecurityID = s.SecurityID,
                 CreateDate = s.CreateDate
               });
      if (enb.HasValue)
      {
        q = q.Where(a => a.Enb == enb);
      }
      if (n != "")
      {
        if (n.StartsWith("="))
          q = q.Where(a => a.SecName == n.Substring(1));
        else
          q = q.Where(a => a.SecName.Contains(n));
      }
      return Json(new { data = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending).Skip(start ?? 0).Take(limit ?? 500), totalCount = q.Count() });
    }

    [Authorize(Roles = "admin")]
    public ActionResult ObjClsRel(int id, string sort, string dir, int? start, int? limit)
    {
      MiddleOfficeDataContext db = new MiddleOfficeDataContext();
      var q = (from ocr in db.tObjClsRelations
               where ocr.ObjClassifierID == id
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
               });
      return Json(new { data = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending).Skip(start ?? 0).Take(limit ?? 500), totalCount = q.Count() });
    }
  }
}
