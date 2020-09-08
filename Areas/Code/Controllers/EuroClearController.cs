using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MO.Helpers;
using MO.Models;
using System.Linq;
using System.Data.Linq;
using System.Net.Security;

namespace MO.Areas.Code.Controllers
{
  public class Message
  {
    public string detailviewallowed { get; set; }
    public int? numberofresults { get; set; }
    public Results[] results { get; set; }
  }

  public class Results
  {
    public string securitytype { get; set; }
    public string id { get; set; }
    public string commoncode { get; set; }
    public string name { get; set; }
  }

  public class SecBody
  {
    public int? numberofresults { get; set; }
    public SecBodyRes[] results { get; set; }
  }

  public class SecBodyRes
  {
    public string shortname { get; set; }
    public string totalissuedamount { get; set; }
    public string issuername { get; set; }
    public string issuercountryname { get; set; }
    public string issuercityname { get; set; }
    public string securitytype { get; set; }
    public string issuercategory { get; set; }
    public string currencylabel { get; set; }
    public string isin { get; set; }
    public string firstclosingdate { get; set; }
    public string maturitydate { get; set; }
    public string multipleamount { get; set; }
    public string additionalspecificationname { get; set; }
    public string cficode { get; set; }
  }

  public class EuroClearController : Controller
  {
    //
    // GET: /Code/EuroClear/

    public ActionResult Index()
    {
      ViewBag.Title = "Ввод ISIN";
      return View();
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult Search(string ISIN)
    {
      try
      {
        CookieContainer cookie = new CookieContainer();

        //var hch = new System.Net.Http.HttpClientHandler
        //{
        //  Proxy = new WebProxy("proxy-cluster-bc.srv.nikoil.ru", 8080),
        //  UseProxy = true,
        //  UseCookies = true,
        //  CookieContainer = cookie
        //};
        //hch.Proxy.Credentials = CredentialCache.DefaultCredentials;

        //var hc = new System.Net.Http.HttpClient();
        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
        ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslPolicyErrors) => true;
        HttpWebRequest wr;
        StreamWriter requestWriter;
        string postData;

        wr = (HttpWebRequest)WebRequest.Create("https://my.euroclear.com/en/login.html ");
        //wr.Proxy = null;
        //wr.Proxy = new WebProxy("proxy-cluster-bc.srv.nikoil.ru", 8080);
        wr.Proxy.Credentials = CredentialCache.DefaultCredentials;
        wr.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko";
        wr.Accept = "text/html, application/xhtml+xml, */*";
        wr.Headers.Add("Accept-Language", "en-US,en;q=0.7,ru;q=0.3");
        wr.CookieContainer = cookie;
        wr.Timeout = 600000;
        wr.ReadWriteTimeout = 600000;
        var hwr = (HttpWebResponse)wr.GetResponse();
        var receiveStream = hwr.GetResponseStream();
        var readStream = new StreamReader(receiveStream, System.Text.Encoding.GetEncoding("utf-8"));
        string bodyText = readStream.ReadToEnd();
        readStream.Close();
        receiveStream.Close();
        hwr.Close();

        wr = (HttpWebRequest)WebRequest.Create("https://my.euroclear.com/content/marketing/en/login/j_security_check");
        //wr.Proxy = null;
        //wr.Proxy = new WebProxy("proxy-cluster-bc.srv.nikoil.ru", 8080);
        wr.Proxy.Credentials = CredentialCache.DefaultCredentials;
        wr.Method = WebRequestMethods.Http.Post;
        wr.Timeout = 600000;
        wr.ReadWriteTimeout = 600000;
        wr.Accept = "text/html, application/xhtml+xml, */*";
        wr.Referer = "https://my.euroclear.com/en/login.html";
        wr.Headers.Add("Accept-Language", "en-US,en;q=0.7,ru;q=0.3");
        wr.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko";
        wr.ContentType = "multipart/form-data; boundary=---------------------------7dd1f2d80288";
        wr.Headers.Add("Cache-Control", "no-cache");
        wr.AllowAutoRedirect = false;
        //cookie.Add(new Uri("http://my.euroclear.com"), new Cookie("s_cc", "true"));
        //cookie.Add(new Uri("http://my.euroclear.com"), new Cookie("s_sq", "euroclearcomeuroclearcomnewsiteprod%3D%2526c.%2526a.%2526activitymap.%2526page%253Dmarketing%25252Fen%25252Flogin%2526link%253DLog%252520in%2526region%253Dlogin%2526pageIDType%253D1%2526.activitymap%2526.a%2526.c%2526pid%253Dmarketing%25252Fen%25252Flogin%2526pidt%253D1%2526oid%253DLog%252520in%2526oidt%253D3%2526ot%253DSUBMIT%2526oi%253D498"));
        //cookie.Add(new Uri("http://my.euroclear.com"), new Cookie("s_fid", "562FBDCAF7CCBA04-2E64EFEF10BF7355"));
        wr.CookieContainer = cookie;
        postData = "-----------------------------7dd1f2d80288\r\nContent-Disposition: form-data; name=\"resource\"\r\n\r\n/content/operations/apps.html\r\n-----------------------------7dd1f2d80288\r\nContent-Disposition: form-data; name=\"_charset_\"\r\n\r\nutf-8\r\n-----------------------------7dd1f2d80288\r\nContent-Disposition: form-data; name=\"j_username\"\r\n\r\nGrishinAV@am-uralsib.ru\r\n-----------------------------7dd1f2d80288\r\nContent-Disposition: form-data; name=\"j_password\"\r\n\r\nvbJ4h$5Uu#ib24*\r\n-----------------------------7dd1f2d80288\r\nContent-Disposition: form-data; name=\"goto\"\r\n\r\nmarketing\r\n-----------------------------7dd1f2d80288--\r\n";
        wr.ContentLength = postData.Length;
        requestWriter = new StreamWriter(wr.GetRequestStream());
        requestWriter.Write(postData);
        requestWriter.Close();
        wr.GetResponse().Close();

        wr = (HttpWebRequest)WebRequest.Create(string.Format("https://my.euroclear.com/bin/euroclear/db/banksecurities.search.json?q=or:{0}&limit=0&search=1", ISIN));
        //wr.Proxy = null; 
        //wr.Proxy = new WebProxy("proxy-cluster-bc.srv.nikoil.ru", 8080);
        wr.Proxy.Credentials = CredentialCache.DefaultCredentials;
        wr.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko";
        wr.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
        wr.Headers.Add("Cache-Control", "max-age=0");
        wr.Headers.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
        wr.Headers.Add("Upgrade-Insecure-Requests", "1");
        wr.Referer = "https://my.euroclear.com/en/login.html";
        wr.CookieContainer = cookie;
        wr.Timeout = 600000;
        wr.ReadWriteTimeout = 600000;
        hwr = (HttpWebResponse)wr.GetResponse();
        receiveStream = hwr.GetResponseStream();
        readStream = new StreamReader(receiveStream, System.Text.Encoding.GetEncoding("utf-8"));
        bodyText = readStream.ReadToEnd();
        readStream.Close();
        receiveStream.Close();
        hwr.Close();

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        Message jsonData = serializer.Deserialize<Message>(bodyText);

        MiddleOfficeDataContext db = new MiddleOfficeDataContext();
        db.tLogs.InsertOnSubmit(new tLog { Text = bodyText, InDateTime = DateTime.Now });
        db.SubmitChanges();

        if (jsonData.detailviewallowed == "true")
        {
          wr = (HttpWebRequest)WebRequest.Create(string.Format("https://my.euroclear.com/bin/euroclear/db/banksecurities.detail.json?id=eq:{0}&", jsonData.results[0].commoncode == null ? jsonData.results[0].id : jsonData.results[0].commoncode));
          //wr.Proxy = null;
          //wr.Proxy = new WebProxy("proxy-cluster-bc.srv.nikoil.ru", 8080);
          wr.Proxy.Credentials = CredentialCache.DefaultCredentials;
          wr.Accept = "text/html, application/xhtml+xml, */*";
          wr.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko";

          wr.CookieContainer = cookie;
          wr.Timeout = 600000;
          wr.ReadWriteTimeout = 600000;
          hwr = (HttpWebResponse)wr.GetResponse();
          receiveStream = hwr.GetResponseStream();
          readStream = new StreamReader(receiveStream, System.Text.Encoding.GetEncoding("utf-8"));
          bodyText = readStream.ReadToEnd();
          readStream.Close();
          receiveStream.Close();
          hwr.Close();

          SecBody jsonBody = serializer.Deserialize<SecBody>(bodyText);
          if (jsonBody.results.Length > 0)
          {
            var d = jsonBody.results[0];
            Regex rg = new Regex("(\\d+).*?=.*?(\\d+)", RegexOptions.Singleline);
            Match m = rg.Match(d.additionalspecificationname);
            var BaseSecQty = 0m;
            if (m.Success)
            {
              BaseSecQty = decimal.Parse(m.Groups[2].Value) / decimal.Parse(m.Groups[1].Value);
            }

            //tExchangeCFI eCFI = db.tExchangeCFIs.FirstOrDefault(p => p.ISIN == d.isin.Trim());
            //string CFI = eCFI == null ? "" : eCFI.CFI;

            return new JsonnResult
            {
              Data = new
              {
                success = true,
                data = new
                {
                  FirstClosingDate = GetDateTime(d.firstclosingdate),
                  Instrument = d.securitytype.Trim(),
                  ISIN = d.isin.Trim(),
                  IssuerName = d.issuername.Trim(),
                  IssuerCountry = d.issuercountryname.Trim(),
                  IssuerCity = d.issuercityname.Trim(),
                  IssuerCategory = d.issuercategory.Trim(),
                  MaturityDate = GetDateTime(d.maturitydate),
                  Nominal = d.multipleamount.Trim() == "" ? 0 : double.Parse(d.multipleamount.Replace(",", "")),
                  NominalCurrency = d.currencylabel.Trim(),
                  SecurityShortName = d.shortname.Trim().Replace(" ", "<>").Replace("><", "").Replace("<>", " ").Left(25),
                  SecurityName = d.shortname.Trim().Replace(" ", "<>").Replace("><", "").Replace("<>", " "),
                  Volume = d.totalissuedamount.Trim() == "" ? 0 : double.Parse(d.totalissuedamount.Replace(",", "")),
                  BaseSecQty = BaseSecQty,
                  d.additionalspecificationname,
                  IsETF = d.shortname.Contains("ETF") || d.additionalspecificationname.Contains("ETF"),
                  Ticker = "",
                  BaseID = (int?)null,
                  CFI = d.cficode.Trim()
                }
              }
            };
          }
        }
        return new JsonnResult { Data = new { success = false, data = new { } } };
      }
      catch (Exception ex)
      {
        MiddleOfficeDataContext db = new MiddleOfficeDataContext();
        db.tLogs.InsertOnSubmit(new tLog { Text = ex.Message, InDateTime = DateTime.Now });
        db.SubmitChanges();
        return Json(new { success = false, message = ex.Message });
      }
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult LoadSec(string ISIN, DateTime? FirstClosingDate, string Instrument, string IssuerCountry, string IssuerCity, string IssuerCategory, string IssuerName, DateTime? MaturityDate, double? Nominal, string NominalCurrency, string SecurityShortName, string SecurityName, double? Volume, decimal? BaseSecQty, string Ticker, int? BaseID, bool? IsETF, decimal? Category, string CFI, decimal? TrSys, bool? DealRate)
    {
      MiddleOfficeDataContext db = new MiddleOfficeDataContext();
      db.taEuroClears.DeleteAllOnSubmit(db.taEuroClears);

      taEuroClear ec = new taEuroClear()
      {
        FirstClosingDate = FirstClosingDate,
        Instrument = Instrument,
        ISIN = ISIN,
        IssueName = IssuerName,
        IssuerCity = IssuerCity,
        IssuerCountry = IssuerCountry,
        IssuerName = IssuerName,
        IssuerCategory = IssuerCategory,
        MaturityDate = MaturityDate,
        Nominal = Nominal,
        NominalCurrency = NominalCurrency,
        SecurityShortName = SecurityShortName,
        SecurityName = SecurityName,
        Volume = Volume,
        Ticker = Ticker,
        IsETF = IsETF,
        BaseSecurityID = BaseID,
        BaseSecQty = BaseSecQty,
        Category = Category,
        CFI = CFI,
        TrSys = TrSys,
        DealRate = DealRate
      };
      db.taEuroClears.InsertOnSubmit(ec);
      db.SubmitChanges();
      db.up_avgRunJob(new Guid("956318E6-D4EB-47EC-974A-EF4D8E253C02"));

      return Json(new { success = true });
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult LoadSec1(string ISIN, DateTime? FirstClosingDate, string Instrument, string ticker, int? baseid)
    {
      try
      {
        HttpWebRequest wr;
        StreamWriter requestWriter;
        string postData;

        //WebProxy wp = new WebProxy("proxy-cluster-bc.srv.nikoil.ru", 8080);
        //wp.Credentials = CredentialCache.DefaultCredentials;
        wr = (HttpWebRequest)WebRequest.Create("https://my.euroclear.com/content/marketing/en/login/j_security_check");
        //wr.Proxy = null;
        //wr.Proxy = new WebProxy("proxy-cluster-bc.srv.nikoil.ru", 8080);
        wr.Proxy.Credentials = CredentialCache.DefaultCredentials;
        //wr.Proxy = wp;
        wr.Method = WebRequestMethods.Http.Post;
        wr.Timeout = 600000;
        wr.ReadWriteTimeout = 600000;
        wr.Accept = "text/html, application/xhtml+xml, */*";
        wr.Headers.Add("Accept-Language", "ru-RU");
        wr.Headers.Add("Pragma", "no-cache");
        wr.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; .NET CLR 1.1.4322; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
        //wr.ContentType = "multipart/form-data; boundary=---------------------------7dd1f2d80288";
        CookieContainer cookie = new CookieContainer();
        wr.CookieContainer = cookie;
        postData = "-----------------------------7dd1f2d80288\r\nContent-Disposition: form-data; name=\"resource\"\r\n\r\n/content/operations/apps.html\r\n-----------------------------7dd1f2d80288\r\nContent-Disposition: form-data; name=\"_charset_\"\r\n\r\nutf-8\r\n-----------------------------7dd1f2d80288\r\nContent-Disposition: form-data; name=\"j_username\"\r\n\r\nGrishinAV@am-uralsib.ru\r\n-----------------------------7dd1f2d80288\r\nContent-Disposition: form-data; name=\"j_password\"\r\n\r\nvbJ4h$5Uu#ib24*\r\n-----------------------------7dd1f2d80288\r\nContent-Disposition: form-data; name=\"goto\"\r\n\r\nmarketing\r\n-----------------------------7dd1f2d80288--\r\n";
        wr.ContentLength = postData.Length;
        requestWriter = new StreamWriter(wr.GetRequestStream());
        requestWriter.Write(postData);
        requestWriter.Close();
        wr.GetResponse().Close();

        MiddleOfficeDataContext db = new MiddleOfficeDataContext();
        db.taEuroClears.DeleteAllOnSubmit(db.taEuroClears);

        wr = (HttpWebRequest)WebRequest.Create(string.Format("https://my.euroclear.com/bin/euroclear/db/banksecurities.detail.json?id=eq:{0}&", 0/*commoncode*/));
        //wr.Proxy = null;
        wr.Proxy.Credentials = CredentialCache.DefaultCredentials;
        //wr.Proxy = wp;
        wr.Accept = "*/*";
        wr.CookieContainer = cookie;
        wr.Timeout = 600000;
        wr.ReadWriteTimeout = 600000;
        var hwr = (HttpWebResponse)wr.GetResponse();
        var receiveStream = hwr.GetResponseStream();
        var readStream = new StreamReader(receiveStream, System.Text.Encoding.GetEncoding("utf-8"));
        var bodyText = readStream.ReadToEnd();
        readStream.Close();
        receiveStream.Close();
        hwr.Close();

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        SecBody jsonBody = serializer.Deserialize<SecBody>(bodyText);
        if ((jsonBody.numberofresults ?? 0) > 0)
        {
          var d = jsonBody.results[0];
          Regex rg = new Regex("(\\d+).+?= (\\d+)", RegexOptions.Singleline);
          Match m = rg.Match(d.additionalspecificationname);
          decimal BaseSecQty = 0;
          if (m.Success)
          {
            BaseSecQty = decimal.Parse(m.Groups[2].Value) / decimal.Parse(m.Groups[1].Value);
          }

          taEuroClear ec = new taEuroClear()
          {
            FirstClosingDate = GetDateTime(d.firstclosingdate),
            Instrument = d.securitytype,
            ISIN = d.isin,
            IssueName = d.issuername.Trim(),
            IssuerCity = d.issuercityname.Trim(),
            IssuerCountry = d.issuercountryname.Trim(),
            IssuerName = d.issuername.Trim(),
            IssuerCategory = d.issuercategory.Trim(),
            MaturityDate = GetDateTime(d.maturitydate),
            Nominal = d.multipleamount.Trim() == "" ? 0 : double.Parse(d.multipleamount.Replace(",", "")),
            NominalCurrency = d.currencylabel,
            SecurityShortName = d.shortname,
            Volume = d.totalissuedamount.Trim() == "" ? 0 : double.Parse(d.totalissuedamount.Replace(",", "")),
            Ticker = ticker,
            BaseSecurityID = baseid,
            BaseSecQty = BaseSecQty
          };
          db.taEuroClears.InsertOnSubmit(ec);
          db.SubmitChanges();

          //db.up_avgRunJob(new Guid("956318E6-D4EB-47EC-974A-EF4D8E253C02"));
        }
        else
          return Json(new { success = false, message = string.Format("{0} не найден.", 0/*commoncode*/) });
      }
      catch (Exception ex)
      {
        return Json(new { success = false, message = ex.Message });
      }
      return Json(new { success = true });
    }

    public static DateTime? GetDateTime(string text)
    {
      DateTime? dt = null;
      Match m = Regex.Match(text, "(\\d{1,2}).{0,1}\\s*(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\\s{1,2}(\\d{4})");
      if (m.Success)
      {
        DateTime d;
        if (DateTime.TryParseExact(string.Format("{0} {1} {2}", m.Groups[1], m.Groups[2], m.Groups[3]), "d MMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
          dt = d;
      }
      else
      {
        m = Regex.Match(text, "\\d{1,2}[.]\\d{1,2}[.]\\d{2,4}");
        if (m.Success)
        {
          DateTime d;
          if (DateTime.TryParseExact(m.Value, "d.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
            dt = d;
        }
      }
      if (dt < new DateTime(1900, 1, 1) || dt > new DateTime(2100, 1, 1))
        dt = null;
      return dt;
    }


    public ActionResult GetSec(string query, int? start, int? limit)
    {
      MiddleOfficeDataContext db = new MiddleOfficeDataContext();
      var q = db.tSecurities
        .Where(p => p.ParentID == null && new byte?[] { 0, 2, 15 }.Contains(p.SecType) && (p.Name1.Contains(query) || p.NameBrief.Contains(query) || p.Number.StartsWith(query)))
        .OrderBy(p => p.Name1)
        .Select(p => new { id = p.SecurityID, name = p.Name1.Trim(), brief = p.NameBrief.Trim(), isin = p.Number.Trim() })
        .Take(limit ?? 0);

      return new JsonnResult { Data = new { success = true, data = q } };
    }

  }
}
