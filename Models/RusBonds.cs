using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace MO.Models
{
  public static class RusBonds
  {
    public static byte[] getRusBonds()
    {
      ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
      WebClient wc = new WebClient { Proxy = null };
      string p = wc.DownloadString("https://www.rusbonds.ru/");
      using (var ms = new MemoryStream())
      using (var sw = new StreamWriter(ms, Encoding.GetEncoding(1251)))
      {
        sw.WriteLine("Name;NameBrief;RegDate;RegNum;ISIN;Nominal;Volume;Period;RazmDateB;RazmDateE;EndDate;OffertDate;EmitentName;EmitentBrief;EmitentINN;EmitentOKPO;EmitentReg;EmitentState;GarantName;GarantState;Branch;Valuta");
        MatchCollection mt1 = Regex.Matches(p, "<table.*?>.*?<caption>Ближайшие размещения облигаций</caption>.*?<tbody>(.+?)</tbody>.*?</table>", RegexOptions.Singleline);
        {
          if (mt1.Count > 0)
          {
            MatchCollection mr1 = Regex.Matches(mt1[0].Groups[1].Value, "<tr.*?>(.+?)</tr>", RegexOptions.Singleline);
            for (int i = 0; i < mr1.Count; i++)
            {
              MatchCollection mc1 = Regex.Matches(mr1[i].Groups[1].Value, "<td.*?>(.*?)</td>", RegexOptions.Singleline);
              if (mc1.Count == 4)
              {
                Match m = Regex.Match(mc1[1].Groups[1].Value, "<a href=/ank_obl.asp\\?tool=(.*?)>(.*?)</a>", RegexOptions.Singleline);
                if (m.Success)
                {
                  var sec = new Sec();
                  sec.code = m.Groups[1].Value;
                  sec.nameBrief = m.Groups[2].Value.Replace("’", "").Replace("'", "");
                  string s = wc.DownloadString(string.Format("https://rusbonds.ru/ank_obl.asp?tool={0}", sec.code));
                  MatchCollection mn1 = Regex.Matches(s, "<table border=0 width=100% cellpadding=3 cellspacing=0 class=tbl_data>.*?<tbody.*?>(.+?)</tbody>.*?</table>", RegexOptions.Singleline);
                  if (mn1.Count > 0)
                  {
                    MatchCollection mn2 = Regex.Matches(mn1[0].Groups[1].Value, "<tr.*?>(.+?)</tr>", RegexOptions.Singleline);
                    for (int j = 0; j < mn2.Count; j++)
                    {
                      MatchCollection mn3 = Regex.Matches(mn2[j].Groups[1].Value, "<td.*?>(.*?)</td>", RegexOptions.Singleline);
                      if (mn3.Count == 2)
                      {
                        var td1 = mn3[0].Groups[1].Value.Replace("<nobr>", "");
                        var td2 = mn3[1].Groups[1].Value;
                        if (td1 == "Наименование:")
                        {
                          sec.name = td2.Replace("’", "").Replace("'", "");
                        }
                        else if (td1 == "Данные госрегистрации:")
                        {
                          sec.regDate = GetDateTime(td2);
                          sec.regNum = GetRegNum(td2);
                        }
                        else if (td1 == "ISIN код:")
                        {
                          sec.ISIN = td2;
                        }
                        else if (td1 == "Номинал:")
                        {
                          sec.nominal = GetInt(td2);
                          sec.Valuta = td2.Contains("RUB") ? 39191 : td2.Contains("USD") ? 39192 : td2.Contains("USD") ? 39199 : (int?)null;
                        }
                        else if (td1 == "Объем эмиссии, шт.:")
                        {
                          sec.volume = GetInt(td2.Replace("\u00a0", ""));
                        }
                        else if (td1 == "Период обращения, дней:")
                        {
                          sec.period = GetInt(td2);
                        }
                        else if (td1 == "Дата начала размещения:")
                        {
                          sec.razmDateB = GetDateTime(td2);
                        }
                        else if (td1 == "Дата окончания размещения:")
                        {
                          sec.razmDateE = GetDateTime(td2);
                        }
                        else if (td1 == "Дата погашения:")
                        {
                          sec.endDate = GetDateTime(td2);
                        }
                        else if (td1 == "Дата ближайшей оферты:")
                        {
                          sec.offertDate = GetDateTime(td2);
                        }

                      }
                    }
                  }
                  Match r = Regex.Match(s, "(ank_org.asp\\?emit=\\d+).*?>(.*?)</a>");
                  if (r.Success)
                  {
                    sec.EmitentBrief = r.Groups[2].Value.Replace("’", "").Replace("'", "");
                    string e = wc.DownloadString(string.Format("https://www.rusbonds.ru/{0}", r.Groups[1].Value));

                    MatchCollection me1 = Regex.Matches(e, "<table border=0 width=100% cellpadding=0 cellspacing=0 class='tbl_data'>.*?<tbody.*?>(.+?)</tbody>.*?</table>", RegexOptions.Singleline);
                    if (me1.Count > 0)
                    {
                      MatchCollection me2 = Regex.Matches(me1[0].Groups[1].Value, "<tr.*?>(.+?)</tr>", RegexOptions.Singleline);
                      for (int j = 0; j < me2.Count; j++)
                      {
                        MatchCollection me3 = Regex.Matches(me2[j].Groups[1].Value, "<td.*?>(.*?)</td>", RegexOptions.Singleline);
                        if (me3.Count == 2)
                        {
                          var td1 = me3[0].Groups[1].Value.Replace("<nobr>", "");
                          var td2 = me3[1].Groups[1].Value;
                          if (td1 == "Наименование:")
                          {
                            sec.Emitent = td2.Replace("’", "").Replace("'", "").Replace('–', '-');
                          }
                          else if (td1 == "Страна:")
                          {
                            sec.EmitentState = td2;
                          }
                          else if (td1 == "ИНН:")
                          {
                            sec.EmitentINN = td2;
                          }
                          else if (td1 == "ОКПО или др.:")
                          {
                            sec.EmitentOKPO = td2;
                          }
                          else if (td1 == "Данные госрегистрации:")
                          {
                            sec.EmitentReg = td2.Replace("&nbsp;", " ").Replace(";", ":");
                          }

                        }
                      }
                    }

                    MatchCollection me12 = Regex.Matches(e, "<table width=100% border=0 cellpadding=2 cellspacing=0 class='tbl_data tbl_headgrid'><caption class=simple>Виды деятельности</caption>.*?<tbody.*?>(.+?)</tbody>.*?</table>", RegexOptions.Singleline);
                    if (me12.Count > 0)
                    {
                      MatchCollection me2 = Regex.Matches(me12[0].Groups[1].Value, "<tr.*?>(.+?)</tr>", RegexOptions.Singleline);
                      for (int j = 0; j < me2.Count; j++)
                      {
                        MatchCollection me3 = Regex.Matches(me2[j].Groups[1].Value, "<td.*?>(.*?)</td>", RegexOptions.Singleline);
                        if (me3.Count == 3)
                        {
                          var td2 = me3[1].Groups[1].Value;
                          var td3 = me3[2].Groups[1].Value;
                          if (td2 == "ФМ Отрасли")
                          {
                            sec.Branch = td3;
                          }
                        }
                      }
                    }

                  }
                  if (sec.ISIN != null && sec.ISIN.StartsWith("XS"))
                  {
                    string g = wc.DownloadString(string.Format("https://rusbonds.ru/toolorg.asp?tool={0}", sec.code));
                    MatchCollection me1 = Regex.Matches(g, "<table width=100% border=0 cellpadding=2 cellspacing=0 class='tbl_data tbl_headgrid'>.*?<tbody.*?>(.+?)</tbody>.*?</table>", RegexOptions.Singleline);
                    if (me1.Count > 0)
                    {
                      MatchCollection me2 = Regex.Matches(me1[0].Groups[1].Value, "<tr.*?>(.+?)</tr>", RegexOptions.Singleline);
                      for (int j = 0; j < me2.Count; j++)
                      {
                        MatchCollection me3 = Regex.Matches(me2[j].Groups[1].Value, "<td.*?>(.*?)</td>", RegexOptions.Singleline);
                        if (me3.Count == 3)
                        {
                          var td1 = me3[0].Groups[1].Value.Replace("<nobr>", "");
                          var td2 = me3[1].Groups[1].Value;
                          if (td1 == "гарант")
                          {
                            sec.Garant = td2;
                            Match a = Regex.Match(td2, "(ank_org.asp\\?emit=\\d+).*?>.*?</a>");
                            if (a.Success)
                              sec.GarantHREF = a.Groups[1].Value;
                          }
                        }
                      }
                      if (sec.GarantHREF != null)
                      {
                        string g1 = wc.DownloadString(string.Format("https://www.rusbonds.ru/{0}", sec.GarantHREF));
                        MatchCollection mg1 = Regex.Matches(g1, "<table border=0 width=100% cellpadding=0 cellspacing=0 class='tbl_data'>.*?<tbody.*?>(.+?)</tbody>.*?</table>", RegexOptions.Singleline);
                        if (mg1.Count > 0)
                        {
                          MatchCollection mg2 = Regex.Matches(mg1[0].Groups[1].Value, "<tr.*?>(.+?)</tr>", RegexOptions.Singleline);
                          for (int j = 0; j < mg2.Count; j++)
                          {
                            MatchCollection mg3 = Regex.Matches(mg2[j].Groups[1].Value, "<td.*?>(.*?)</td>", RegexOptions.Singleline);
                            if (mg3.Count == 2)
                            {
                              var td1 = mg3[0].Groups[1].Value.Replace("<nobr>", "");
                              var td2 = mg3[1].Groups[1].Value;
                              if (td1 == "Страна:")
                              {
                                sec.GarantState = td2;
                              }
                            }
                          }
                        }
                      }
                    }
                  }
                  if (!string.IsNullOrEmpty(sec.regNum) || !string.IsNullOrEmpty(sec.ISIN))
                  {

                    sw.WriteLine(string.Format("{0};{1};{2:dd.MM.yyyy};{3};{4};{5};{6};{7};{8:dd.MM.yyyy};{9:dd.MM.yyyy};{10:dd.MM.yyyy};{11:dd.MM.yyyy};{12};{13};{14};{15};{16};{17};{18};{19};{20};{21}",
                      sec.name,
                      sec.nameBrief,
                      sec.regDate,
                      sec.regNum,
                      /*sec.regNum.Contains("RMFS") ? null :*/ sec.ISIN,
                      sec.nominal,
                      sec.volume,
                      sec.period,
                      sec.razmDateB,
                      sec.razmDateE,
                      sec.endDate,
                      sec.offertDate,
                      sec.Emitent,
                      sec.EmitentBrief,
                      sec.EmitentINN,
                      sec.EmitentOKPO,
                      sec.EmitentReg,
                      sec.EmitentState,
                      sec.Garant,
                      sec.GarantState,
                      sec.Branch,
                      sec.Valuta
                    ));
                  }
                }
              }
            }
          }
        }
        sw.Flush();
        ms.Position = 0;
        return ms.ToArray();
      }
    }
    static IFormatProvider culture = new CultureInfo("ru-RU", true);

    public static DateTime? GetDateTime(string text)
    {
      DateTime? dt = null;
      Match m = Regex.Match(text, "(\\d{1,2}).{0,1}\\s*(января|февраля|марта|апреля|мая|июня|июля|августа|сентября|октября|ноября|декабря)\\s{1,2}(\\d{4})");
      if (m.Success)
      {
        DateTime d;
        if (DateTime.TryParseExact(string.Format("{0} {1} {2}", m.Groups[1], m.Groups[2], m.Groups[3]), "d MMMM yyyy", culture, DateTimeStyles.None, out d))
          dt = d;
      }
      else
      {
        m = Regex.Match(text, "\\d{1,2}[.]\\d{1,2}[.]\\d{2,4}");
        if (m.Success)
        {
          DateTime d;
          if (DateTime.TryParseExact(m.Value, "d.MM.yyyy", culture, DateTimeStyles.None, out d))
            dt = d;
        }
      }
      if (dt < new DateTime(1900, 1, 1))
        dt = null;
      return dt;
    }

    public static string GetRegNum(string text)
    {
      string rn = null;
      Match m = Regex.Match(text, "№([0-9A-Za-z\\-^\\.]+|\\d.\\d{9-10}.\\d{3}[A-Za-z]|\\d{8}.|\\d{2}-\\d{1}.-\\d{4}|\\d-\\d{2}-[0-9A-Za-z]{5}-.|[0-9A-Za-z]{4}-\\d{2}-\\d{5}-.-[0-9A-Za-z]{4}|[0-9A-Za-z]{4}-\\d{2}-\\d{5}-.|\\d{5}RMFS|\\d.\\d{9}.|RU\\d{5}.{4}|RU[0-9A-Za-z]{10})"); /* |\\d */
      if (m.Success)
      {
        rn = m.Groups[1].Value.Trim();
      }
      return rn;
    }

    public static int? GetInt(string text)
    {
      int? i = null;
      Match m = Regex.Match(text, "\\d+");
      if (m.Success)
      {
        i = int.Parse(m.Value);
      }
      return i;
    }
}
  public class Sec
  {
    public string code { get; set; }
    public string name { get; set; }
    public string nameBrief { get; set; }
    public DateTime? regDate { get; set; }
    public string regNum { get; set; }
    public string ISIN { get; set; }
    public int? nominal { get; set; }
    public int? Valuta { get; set; }
    public int? volume { get; set; }
    public int? period { get; set; }
    public DateTime? razmDateB { get; set; }
    public DateTime? razmDateE { get; set; }
    public DateTime? endDate { get; set; }
    public DateTime? offertDate { get; set; }
    public string Emitent { get; set; }
    public string EmitentBrief { get; set; }
    public string EmitentINN { get; set; }
    public string EmitentOKPO { get; set; }
    public string EmitentReg { get; set; }
    public string EmitentState { get; set; }
    public string Garant { get; set; }
    public string GarantHREF { get; set; }
    public string GarantState { get; set; }
    public string Branch { get; set; }
  }
}