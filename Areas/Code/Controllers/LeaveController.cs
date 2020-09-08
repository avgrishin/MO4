using MO.Areas.Code.Models;
using MO.Helpers;
using MO.Hubs;
using MO.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Web.Mvc;

namespace MO.Areas.Code.Controllers
{
  [Authorize]
  public class LeaveController : Controller
  {
    public ILeaveRepository leaveRepository;
    protected readonly Lazy<Microsoft.AspNet.SignalR.IHubContext> AHub = new Lazy<Microsoft.AspNet.SignalR.IHubContext>(() => Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<AHub>());

    public LeaveController(ILeaveRepository _leaveRepository)
    {
      leaveRepository = _leaveRepository;
    }

    public ActionResult Index()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, this.Url.RequestContext.HttpContext.Request.CurrentExecutionFilePath);
      var principal = UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Domain), User.Identity.Name);
      var u = leaveRepository.GetUserByUserName(User.Identity.Name);
      var vm = new LeaveViewModel
      {
        Name1 = u != null ? u.Name : principal.Name,
        //UserName1 = User.Identity.Name,
        Email1 = u != null ? u.Email : principal.EmailAddress,
        DateB = DateTime.Today.AddDays(1)
      };
      vm.DateE = vm.DateB;
      vm.Days = ((vm.DateE ?? DateTime.Today) - (vm.DateB ?? DateTime.Today)).Days + 1;
      ViewBag.TypeList = leaveRepository.GetType1();
      ViewBag.Title = "Заявление на отпуск";
      return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Index(LeaveViewModel vm)
    {
      if (ModelState.IsValid)
      {
        vm.UserName1 = User.Identity.Name;
        var principal = UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Domain), User.Identity.Name);
        vm.Name1 = principal.Name;
        vm.Email1 = principal.EmailAddress;
        if (leaveRepository.CreateLeave(vm))
        {
          if (leaveRepository.SendConfirm(vm.ID, (HttpContext.Request).Url.Authority))
            return RedirectToAction("Created", new { id = vm.ID, key = MD5Hash.GetMd5Hash(User.Identity.Name + vm.ID.ToString() + "hjkl") });
          else
            return View("Error");
        }
      }
      ViewBag.TypeList = leaveRepository.GetType1();
      ViewBag.Title = "Заявление на отпуск";
      return View(vm);
    }

    public ActionResult Created(int? id, string key)
    {
      if (id == null)
        return HttpNotFound();
      var vm = leaveRepository.GetLeave(id.Value);
      if (vm == null)
        return HttpNotFound();
      if (MD5Hash.GetMd5Hash(vm.UserName1 + vm.ID.ToString() + "hjkl") != key)
        return HttpNotFound();
      ViewBag.Title = "Заявление на отпуск зарегистрировано";
      return View(vm);
    }

    public ActionResult ConfirmLeave(int id)
    {
      var r = leaveRepository.CheckConfirmLeave(id, User.Identity.Name, (HttpContext.Request).Url.Authority);
      if (r == 1)
      {
        var vm = leaveRepository.GetLeave(id);
        ViewBag.Title = "Согласование заявления на отпуск";
        ViewBag.UserName = User.Identity.Name;
        return View("Confirm", vm);
      }
      else if (r == 0)
      {
        return HttpNotFound();
      }
      ViewBag.Title = "Заявление на отпуск";
      ViewBag.Message = r == 2 ? "Вы не имеете прав на согласование заявки" : r == 3 ? "Еще не пришло время на согласование заявки" : "Вы уже согласовали данную заявку";
      return View("Error");
    }

    [HttpPost]
    public ActionResult ConfirmLeave(ConfirmViewModel data)
    {
      var r = leaveRepository.ConfirmLeave(data, User.Identity.Name, (HttpContext.Request).Url.Authority);
      if (r == 1)
      {
        var vm = leaveRepository.GetLeave(data.ID);
        if (leaveRepository.SendConfirm(vm.ID, (HttpContext.Request).Url.Authority))
        {
          ViewBag.Title = "Заявление на отпуск согласовано";
          return View("Created", vm);
        }
      }
      else if (r == 0)
      {
        return HttpNotFound();
      }
      ViewBag.Title = "Заявление на отпуск";
      ViewBag.Message = r == 2 ? "Вы не имеете прав на согласование заявки" : r == 3 ? "Еще не пришло время на согласование заявки" : "Вы уже согласовали данную заявку";
      return View("Error");
    }

    [Authorize(Roles = "admin")]
    public ActionResult Users()
    {
      return View("Users");
    }

    [Authorize(Roles = "admin")]
    public ActionResult getUserList(string sort, string dir, bool? IsActive)
    {
      return new JsonnResult { Data = new { success = true, data = leaveRepository.getUserList(sort, dir, IsActive) } };
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    public ActionResult addUser(List<UserViewModel> data)
    {
      return new JsonnResult { Data = new { success = true, data = leaveRepository.addUser(data) } };
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    public ActionResult updUser(List<UserViewModel> data)
    {
      return new JsonnResult { Data = new { success = true, data = leaveRepository.updUser(data) } };
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    public ActionResult delUser(List<UserViewModel> data)
    {
      return new JsonnResult { Data = new { success = leaveRepository.delUser(data) } };
    }

    [Authorize(Roles = "admin")]
    public ActionResult findADInfo(string userName)
    {
      using (var context = new PrincipalContext(ContextType.Domain))
      {
        using (var principal = UserPrincipal.FindByIdentity(context, userName))
        {
          if (principal != null)
            return new JsonnResult
            {
              Data = new
              {
                success = true,
                UserName = "FC\\" + principal.SamAccountName,
                Name = principal.Name,
                Name1 = principal.Surname,
                Name2 = principal.GivenName,
                Name3 = principal.MiddleName,
                Email = principal.EmailAddress
              }
            };
          else
            return new JsonnResult { Data = new { success = false } };
        }
      }
    }

    [Authorize(Roles = "admin")]
    public ActionResult getKM()
    {
      return new JsonnResult { Data = new { success = true, data = leaveRepository.GetKM() } };
    }

    public ActionResult FindU(string id)
    {
      //return Json(leaveRepository.FindU(id), JsonRequestBehavior.AllowGet);
      return new JsonnResult { Data = leaveRepository.FindU(id) };
    }

    public ActionResult History(HistoryParamVM histParam)
    {
      ViewBag.Title = "История заявлений на отпуск";
      ViewBag.histParam = histParam;
      return View(leaveRepository.GetHistory(histParam, User.Identity.Name));
    }

  }
}