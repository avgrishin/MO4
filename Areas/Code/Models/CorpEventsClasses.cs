using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MO.Areas.Code.Models
{
  public class CorpEventsViewModel
  {
    public Tuple<int, string>[] fields { get; set; }
    public Tuple<int, string>[] columns { get; set; }
  }

  public class EnregDet
  {
    public string DocNum { get; set; }
  }

  /*
  public class EnregDetViewModel
  {
    public string dep { get; set; }
    public string NameTo { get; set; }
    public EnregDet ed;
  }
  */
  public class EMailED
  {
    public string NameTo { get; set; }
    public string EMailTo { get; set; }
    public string FileName { get; set; }
    public string str { get; set; }
  }
}