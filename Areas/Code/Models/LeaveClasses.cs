using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MO.Areas.Code.Models
{
  public class LeaveViewModel
  {
    public int ID { get; set; }

    [Display(Name = "Фамилия Имя Отчество", Prompt = "ФИО")]
    [Required]
    public string Name1 { get; set; }

    [Display(Name = "Логин")]
    public string UserName1 { get; set; }

    [Display(Name = "Email")]
    [Required]
    public string Email1 { get; set; }

    [Display(Name = "Вид отпуска")]
    [Required]
    public int? TypeId { get; set; }

    [Display(Name = "Тип")]
    public string TypeName { get; set; }

    [Display(Name = "С")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
    [Required]
    public DateTime? DateB { get; set; }

    [Display(Name = "по")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
    [Required]
    public DateTime? DateE { get; set; }

    [Display(Name = "календарных дней")]
    [Required]
    public int? Days { get; set; }

    [Display(Name = "Фамилия Имя Отчество", Prompt = "ФИО")]
    public string Name2 { get; set; }

    [Display(Name = "Примечание")]
    [MaxLength(255)]
    public string Comment1 { get; set; }

    [Display(Name = "Email")]
    public string Email2 { get; set; }

    [Display(Name = "Фамилия Имя Отчество", Prompt = "ФИО")]
    public string Name3 { get; set; }

    [Display(Name = "Фамилия Имя Отчество", Prompt = "ФИО")]
    [Required]
    public string Name4 { get; set; }

    [Display(Name = "Логин")]
    [Required]
    public string UserName4 { get; set; }

    [Display(Name = "Email")]
    [Required]
    public string Email4 { get; set; }

    [Display(Name = "Фамилия Имя Отчество", Prompt = "ФИО")]
    public string Name5 { get; set; }

    [Display(Name = "Логин")]
    public string UserName5 { get; set; }

    [Display(Name = "Email")]
    public string Email5 { get; set; }

    public DateTime? Sign1 { get; set; }
    public DateTime? Sign4 { get; set; }
    public DateTime? Sign5 { get; set; }

    [Display(Name = "Примечание")]
    [MaxLength(255)]
    public string Comment4 { get; set; }

    [Display(Name = "Примечание")]
    [MaxLength(255)]
    public string Comment5 { get; set; }

  }

  public class UserViewModel
  {
    public int? ID { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Name1 { get; set; }
    public string Name2 { get; set; }
    public string Name3 { get; set; }
    public int? TabNomer { get; set; }
    public int? ObjClsID { get; set; }
    public string ObjClsS { get; set; }
    public bool IsActive { get; set; }
  }

  public class ConfirmViewModel
  {
    public int ID { get; set; }
    public string Comment { get; set; }
  }

  public class HistoryParamVM
  {
    public HistoryParamVM()
    {
      Sort = "DateB";
      Dir = "desc";
    }

    [Display(Name = "С ")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? DateB { get; set; }

    [Display(Name = " по ")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? DateE { get; set; }

    public string Sort { get; set; }

    public string Dir { get; set; }
  }

}