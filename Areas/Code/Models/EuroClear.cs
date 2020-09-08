using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MO.Areas.Code.Models
{
  public class EuroClear
  {
    public string IssueName { get; set; }
    public string Instrument { get; set; }
    public string NominalCurrency { get; set; }
    public string ISIN { get; set; }
    public DateTime? FirstClosingDate { get; set; }
    public DateTime? MaturityDate { get; set; }
    public string SecurityShortName { get; set; }
    public string IssuerName { get; set; }
    public string IssuerCategory { get; set; }
    public string IssuerCountry { get; set; }
    public string IssuerCity { get; set; }
    public double? Nominal { get; set; }
    public Int64? Volume { get; set; }
  }
}