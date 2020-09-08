using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using MO.Models;
using MvcContrib.Sorting;
using System.Collections.Generic;

namespace MO.Controllers
{
  [Authorize(Roles = "admin")]
  public class AdminController : Controller
  {
    public ActionResult Index()
    {
      //System.Web.Security.Roles.CreateRole("clienta");
      //System.Web.Security.Roles.CreateRole("regindocv");
      //var email = System.DirectoryServices.AccountManagement.UserPrincipal.Current.EmailAddress;
      var context = new System.DirectoryServices.AccountManagement.PrincipalContext(System.DirectoryServices.AccountManagement.ContextType.Domain);
      //var principal = System.DirectoryServices.AccountManagement.UserPrincipal.FindByIdentity(context, "FC\\YurievaIS");
      //var firstName = principal.GivenName;
      //var lastName = principal.Surname;
      //var middleName = principal.MiddleName;
      return View();
    }

    public ActionResult ListJsonExt(string sort, string dir)
    {
      aspnetdbDataContext db = new aspnetdbDataContext();
      var q = (from u in db.aspnet_Users
               select new
               {
                 UID = u.UserId,
                 UN = u.UserName,
                 LAD = u.LastActivityDate.ToLocalTime(),
                 LUD = u.aspnet_Profile.LastUpdatedDate.HasValue ? (DateTime?)u.aspnet_Profile.LastUpdatedDate.Value.ToLocalTime() : null
               }).ToList();
      return Json(new { data = q.OrderBy(sort, dir == "DESC" ? SortDirection.Descending : SortDirection.Ascending) }, JsonRequestBehavior.AllowGet);
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult Create(string UN)
    {
      try
      {
        if (ValidateRegistration(UN))
        {
          aspnetdbDataContext db = new aspnetdbDataContext();
          if (db.aspnet_Users.Where(u => u.UserName == UN).Select(u => u).Take(1).Count() == 0)
          {
            var u = new aspnet_User();
            u.ApplicationId = (db.aspnet_Applications.Where(a => a.ApplicationName == Membership.ApplicationName).Select(a => a.ApplicationId).First());
            u.UserName = UN;
            u.LoweredUserName = UN.ToLower();
            u.IsAnonymous = false;
            u.UserId = Guid.NewGuid();
            u.LastActivityDate = DateTime.Today;
            db.aspnet_Users.InsertOnSubmit(u);
            db.SubmitChanges();
          }
          else
          {
            return Json(new { success = false, msg = "Пользователь уже есть" });
          }
        }
      }
      catch (Exception ex)
      {
        return Json(new { success = false, msg = ex.Message });
      }
      return Json(new { success = true, msg = string.Format("\"{0}\" добавлен.", UN) });
    }

    private bool ValidateRegistration(string userName)
    {
      if (String.IsNullOrEmpty(userName))
      {
        ModelState.AddModelError("username", "You must specify a username.");
      }
      return ModelState.IsValid;
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult Delete1(Guid id)
    {
      try
      {
        aspnetdbDataContext db = new aspnetdbDataContext();
        string UN = db.aspnet_Users.Where(u => u.UserId == id).Select(u => u.UserName).First();
        Membership.DeleteUser(UN, true);
        return Json(new { success = true, msg = string.Format("\"{0}\" удален.", UN) });
      }
      catch (Exception ex)
      {
        return Json(new { success = false, msg = ex.Message });
      }
    }

    public ActionResult GetUserRolesJson(Guid id)
    {
      aspnetdbDataContext db = new aspnetdbDataContext();
      var q = (from p in db.aspnet_Roles
               where p.aspnet_Application.ApplicationName == Membership.ApplicationName
               join c in db.aspnet_UsersInRoles.Where(ur => ur.UserId == id) on p.RoleId equals c.RoleId into t
               from x in t.DefaultIfEmpty()
               select new
               {
                 RoleName = p.RoleName,
                 UserID = id,
                 uinr = x.UserId != null,
               }).ToList();
      return Json(new { data = q, success = true });
    }

    public ActionResult SetUserRolesJson1(List<UserRolesList> data)
    {
      aspnetdbDataContext db = new aspnetdbDataContext();

      foreach (var role in data)
      {
        var UserName = (from u in db.aspnet_Users
                        where u.UserId == role.UserID
                        select u.UserName).First();
        if (role.uinr)
        {
          if (!Roles.IsUserInRole(UserName, role.RoleName))
            Roles.AddUserToRole(UserName, role.RoleName);
        }
        else
        {
          if (Roles.IsUserInRole(UserName, role.RoleName))
            Roles.RemoveUserFromRole(UserName, role.RoleName);
        }
      }
      return Json(new { success = true, message = "Сохранено", data = data });
    }

  }
}
