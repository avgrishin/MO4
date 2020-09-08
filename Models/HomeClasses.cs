using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MO.Models
{
  public class AccountLogin
  {
    public string Token { get; set; }
  }

  public class InfoEmitents
  {
    public List<object> Metadata { get; set; }
    public List<object> Columns { get; set; }
    public List<List<string>> Rows { get; set; }
  }
}