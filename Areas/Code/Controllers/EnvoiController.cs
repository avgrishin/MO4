using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MO.Areas.Code.Models;
using MO.Helpers;
using MO.Hubs;
using MO.Models;
using System.DirectoryServices;

namespace MO.Areas.Code.Controllers
{
  [Authorize(Roles = "risk")]
  public class EnvoiController : Controller
  {
    public IEnvoiRepository envoiRepository;
    protected readonly Lazy<Microsoft.AspNet.SignalR.IHubContext> AHub = new Lazy<Microsoft.AspNet.SignalR.IHubContext>(() => Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<AHub>());

    public EnvoiController(IEnvoiRepository _envoiRepository)
    {
      envoiRepository = _envoiRepository;
    }

    public ActionResult Index()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);
      return View();
    }

    public ActionResult getEnvoiList(int TypeID, bool? isAuto, bool? IsActive, string sort, string dir)
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.getEnvoiList(TypeID, isAuto, IsActive, sort, dir) } };
    }

    public ActionResult addEnvoi(List<tEnvoi> data)
    {
      try
      {
        return new JsonnResult { Data = new { success = true, data = envoiRepository.addEnvoi(data) } };
      }
      catch (Exception /*ex*/)
      {
        return new JsonnResult { Data = new { success = false } };
      }
    }

    public ActionResult updEnvoi(List<tEnvoi> data)
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.updEnvoi(data) } };
    }

    public ActionResult delEnvoi(List<tEnvoi> data)
    {
      return new JsonnResult { Data = new { success = envoiRepository.delEnvoi(data) } };
    }

    public ActionResult getConseilList(DateTime? d1, DateTime? d2, int? type, Boolean? nopen, string sort, string dir)
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.getConseilList(d1, d2, type, nopen, sort, dir) } };
    }

    public ActionResult addConseil(List<tConseil> data)
    {
      try
      {
        return new JsonnResult { Data = new { success = true, data = envoiRepository.addConseil(data) } };
      }
      catch (Exception /*ex*/)
      {
        return new JsonnResult { Data = new { success = false } };
      }
    }

    public ActionResult updConseil(List<tConseil> data)
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.updConseil(data) } };
    }

    public ActionResult delConseil(List<tConseil> data)
    {
      return new JsonnResult { Data = new { success = envoiRepository.delConseil(data) } };
    }

    public ActionResult getCPriorite()
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.getCPriorite() } };
    }

    [Authorize(Roles = "admin,risk,regdoc")]
    public ActionResult EMail()
    {
      return View("EMail7");
    }

    [Authorize(Roles = "admin,risk,regdoc")]
    public ActionResult getEMailList(string sort, string dir)
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.getEMailList(sort, dir) } };
    }

    [Authorize(Roles = "admin,risk,regdoc")]
    public ActionResult addEMail(List<EMailItem> data)
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.addEMail(data) } };
    }

    [Authorize(Roles = "admin,risk,regdoc")]
    public ActionResult updEMail(List<EMailItem> data)
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.updEMail(data) } };
    }

    [Authorize(Roles = "admin,risk,regdoc")]
    public ActionResult delEMail(List<EMailItem> data)
    {
      return new JsonnResult { Data = new { success = envoiRepository.delEMail(data) } };
    }

    [Authorize(Roles = "admin,risk,regdoc")]
    public ActionResult findADInfo(string email)
    {
      var directory = new DirectorySearcher("(mail=" + email + ")");
      var result = directory.FindOne();
      if (result == null)
      {
        directory = new DirectorySearcher("(userprincipalname=" + email + ")");
        result = directory.FindOne();
        if (result == null)
        {
          directory = directory ?? new DirectorySearcher("(proxyaddresses=smtp:" + email + ")");
          result = directory.FindOne();
        }
      }
      if (result == null)
        return new JsonnResult { Data = new { success = false } };
      var surname = "";
      var givenname = "";
      var initials = "";
      if (result.Properties["sn"].Count == 0)
      {
        if (result.Properties["cn"].Count > 0)
        {
          var cn = result.Properties["cn"][0].ToString().Split(new char[] { ' ' });
          if (cn.Length > 0)
            surname = cn[0];
          if (cn.Length > 1)
            givenname = cn[1];
          if (cn.Length > 2)
            initials = cn[2].Left(1) + ".";
        }
      }
      else
      {
        surname = result.Properties["sn"][0].ToString();
        givenname = result.Properties["givenname"].Count > 0 ? result.Properties["givenname"][0].ToString() : "";
        initials = result.Properties["initials"].Count > 0 ? result.Properties["initials"][0].ToString() : "";
      }
      //var names = result.Properties["name"][0].ToString().Split(new char[] { ' ' });
      //var title = result.Properties["title"][0];
      return new JsonnResult { Data = new { success = true, name = surname + (givenname.Length > 0 ? " " + givenname.Left(1) + "." + (initials.Length > 0 ? initials /*+ "."*/ : "") : "") } };
    }

    public ActionResult envoyerCourriel(int? id, string Comment)
    {
      return new JsonnResult { Data = new { success = envoiRepository.envoyerCourriel(id, Comment, (HttpContext.Request).Url.Authority) } };
    }

    public ActionResult getEnvoiHoraire(int? id)
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.getEnvoiHoraire(id) } };
    }

    public ActionResult addEnvoiHoraire(List<tEnvoiHoraire> data)
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.addEnvoiHoraire(data) } };
    }

    public ActionResult updEnvoiHoraire(List<tEnvoiHoraire> data)
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.updEnvoiHoraire(data) } };
    }

    public ActionResult delEnvoiHoraire(List<tEnvoiHoraire> data)
    {
      return new JsonnResult { Data = new { success = envoiRepository.delEnvoiHoraire(data) } };
    }

    public ActionResult getEnvoiHoraireType()
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.getEnvoiHoraireType() } };
    }

    public ActionResult getEnvoiExecList(int TypeID, DateTime? d1, DateTime? d2, bool? IsExec, string sort, string dir)
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.getEnvoiExecList(TypeID, d1, d2, IsExec, sort, dir) } };
    }

    public ActionResult addEnvoiExec(List<tEnvoiExec> data)
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.addEnvoiExec(data) } };
    }

    public ActionResult updEnvoiExec(List<tEnvoiExec> data)
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.updEnvoiExec(data) } };
    }

    public ActionResult delEnvoiExec(List<tEnvoiExec> data)
    {
      return new JsonnResult { Data = new { success = envoiRepository.delEnvoiExec(data) } };
    }

    public ActionResult envoiExecCourriel()
    {
      return new JsonnResult { Data = new { success = envoiRepository.envoiExecCourriel((HttpContext.Request).Url.Authority) } };
    }

    public ActionResult envoiExecRiCourriel()
    {
      return new JsonnResult { Data = new { success = envoiRepository.envoiExecRiCourriel((HttpContext.Request).Url.Authority) } };
    }

    public ActionResult conseilCourriel(int? id)
    {
      return new JsonnResult { Data = new { success = envoiRepository.conseilCourriel(id) } };
    }

    public ActionResult conseilCourrielAll()
    {
      return new JsonnResult { Data = new { success = envoiRepository.conseilCourrielAll() } };
    }

    public ActionResult conseilEnbCourriel()
    {
      return new JsonnResult { Data = new { success = envoiRepository.conseilEnabledCourriel() } };
    }

    public ActionResult getConseilHoraire(int? id)
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.getConseilHoraire(id) } };
    }

    public ActionResult addConseilHoraire(List<tConseilHoraire> data)
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.addConseilHoraire(data) } };
    }

    public ActionResult updConseilHoraire(List<tConseilHoraire> data)
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.updConseilHoraire(data) } };
    }

    public ActionResult delConseilHoraire(List<tConseilHoraire> data)
    {
      return new JsonnResult { Data = new { success = envoiRepository.delConseilHoraire(data) } };
    }

    public ActionResult getRiskMapList(string sort, string dir)
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.getRiskMapList(sort, dir) } };
    }

    public ActionResult addRiskMap(List<tRiskMap> data)
    {
      try
      {
        return new JsonnResult { Data = new { success = true, data = envoiRepository.addRiskMap(data) } };
      }
      catch (Exception /*ex*/)
      {
        return new JsonnResult { Data = new { success = false } };
      }
    }

    public ActionResult updRiskMap(List<tRiskMap> data)
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.updRiskMap(data) } };
    }

    public ActionResult delRiskMap(List<tRiskMap> data)
    {
      return new JsonnResult { Data = new { success = envoiRepository.delRiskMap(data) } };
    }

    public ActionResult getRMLevel()
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.getRMLevel() } };
    }

    public ActionResult getRiskMapHoraire(int? id)
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.getRiskMapHoraire(id) } };
    }

    public ActionResult addRiskMapHoraire(List<tRiskMapHoraire> data)
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.addRiskMapHoraire(data) } };
    }

    public ActionResult updRiskMapHoraire(List<tRiskMapHoraire> data)
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.updRiskMapHoraire(data) } };
    }

    public ActionResult delRiskMapHoraire(List<tRiskMapHoraire> data)
    {
      return new JsonnResult { Data = new { success = envoiRepository.delRiskMapHoraire(data) } };
    }

    public ActionResult riskMapCourriel(List<int> id)
    {
      return new JsonnResult { Data = new { success = envoiRepository.riskMapCourriel(id, (HttpContext.Request).Url.Authority) } };
    }

    public ActionResult getDeclViolList(DateTime? d1, DateTime? d2, bool? op, string sort, string dir)
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.getDeclViol(d1, d2, op, sort, dir) } };
    }

    public ActionResult addDeclViol(List<tInvDeclErrorJournal> data)
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.addDeclViol(data) } };
    }

    public ActionResult updDeclViol(List<tInvDeclErrorJournal> data)
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.updDeclViol(data) } };
    }

    public ActionResult delDeclViol(List<tInvDeclErrorJournal> data)
    {
      return new JsonnResult { Data = new { success = envoiRepository.delDeclViol(data) } };
    }

    public ActionResult GetFI(int? id, string query, int? start, int? limit)
    {
      if (id.HasValue)
        return Json(new { data = envoiRepository.GetFinInst(id.Value) }, JsonRequestBehavior.AllowGet);
      else
        return Json(new { data = envoiRepository.GetFinInsts(query ?? "", limit ?? 10) }, JsonRequestBehavior.AllowGet);
    }

    public ActionResult GetDeclByInstID(int? id)
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.GetDeclByInstID(id) } };
    }

    public ActionResult GetDeclWhereByDeclID(int? id, int? wid)
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.GetDeclWhereByDeclID(id, wid) } };
    }

    public ActionResult GetObjClsByParent(int id)
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.GetObjClsByParent(id) } };
    }

    public ActionResult GetIssuers(int? id, DateTime? d)
    {
      d = d ?? DateTime.Today.AddDays(-1);
      return new JsonnResult { Data = new { success = true, data = envoiRepository.GetIssuers(id, d) } };
    }

    public ActionResult DeclViolCourriel()
    {
      return new JsonnResult { Data = new { success = envoiRepository.declViolCourriel((HttpContext.Request).Url.Authority) } };
    }

    public ActionResult GetTermDate(DateTime? d, int? prid)
    {
      var ret = envoiRepository.getTermDate(d, prid);
      if (ret.HasValue)
        return new JsonnResult { Data = new { success = ret.Value.ToShortDateString() } };
      else
        return new JsonnResult { Data = new { success = "" } };
    }

    [ActionName("Dep")]
    public ActionResult GetEnvoiExecByDep()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);
      return View("GetEnvoiExecByDepList");
    }

    public ActionResult GetEnvoiExecByDepList(DateTime? d1, DateTime? d2, bool? IsExec, string sort, string dir)
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.getEnvoiExecByDepList(d1, d2, IsExec, sort, dir) } };
    }

    public ActionResult updEnvoiExecByDep(List<EnvoiDep> data)
    {
      return new JsonnResult { Data = new { success = true, data = envoiRepository.updEnvoiExecByDep(data) } };
    }

  }
}
