using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MO.Areas.Code.Models
{
  public class EMailItem
  {
    public int? ID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
  }

  public class EnvoiDep {
    public int ID { get; set; }
    public int EnvoiID { get; set; }
    public string Comment { get; set; }
    public DateTime? Date3 { get; set; }
    public string Responsible { get; set; }
  }
}