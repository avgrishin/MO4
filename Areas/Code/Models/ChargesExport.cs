using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MO.Areas.Code.Models
{
  public class ChargesExport
  {
    public int? id { get; set; }
    public DateTime? DateReg { get; set; }
    public DateTime? DatePay { get; set; }
    public string FinInst { get; set; }
    public string Receiver { get; set; }
    public string Item { get; set; }
    public string Pfp { get; set; }
    public string TypeName { get; set; }
    public DateTime? DateRegEnd { get; set; }
    public decimal? QtyP { get; set; }
    public string Comment { get; set; }
    public string PeriodicityName { get; set; }
    public string TRName { get; set; }
    public string Fund { get; set; }
  }
}