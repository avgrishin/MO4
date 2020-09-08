using Microsoft.AspNet.SignalR;
using MO.Helpers;
using MO.Hubs;
using MO.Models;
using MO.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MO.Controllers
{
  public class HomeController : Controller
  {
    public IReportRepository repRepository;

    protected readonly Lazy<IHubContext> RHub = new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<RHub>());
    protected readonly Lazy<IHubContext> AHub = new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<AHub>());

    public HomeController(IReportRepository repo)
    {
      repRepository = repo;
    }

    public ActionResult Index()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, "Home/Index");
      ViewBag.Title = "Home Page";
      //dynamic viewModel = new ExpandoObject();
      ////var viewModel = new { Course = repRepository.GetValRates(), Rate = repRepository.GetCourses() }.ToExpando();

      //System.Security.Principal.WindowsIdentity wi = System.Security.Principal.WindowsIdentity.GetCurrent();
      //string[] a = User.Identity.Name.Split('\\');
      //System.DirectoryServices.DirectoryEntry ADEntry = new System.DirectoryServices.DirectoryEntry("WinNT://" + a[0] + "/" + a[1]);
      //string Name = ADEntry.Properties["FullName"].Value.ToString();

      //using (var de = new DirectoryEntry("LDAP://DC=uralsib,DC=ru"))
      //using (var ds = new DirectorySearcher(de))
      //{
      //  ds.Filter = string.Format("(sAMAccountName={0})", User.Identity.Name);
      //  ds.PropertiesToLoad.AddRange(new[] {
      //      "sn",  // last name
      //      "givenName",  // first name
      //      "mail",  // email
      //      "telephoneNumber",  // phone number
      //      // etc - add other properties you need
      //      });
      //  var res = ds.FindOne();

      //  foreach (string propName in res.Properties.PropertyNames)
      //  {
      //    ResultPropertyValueCollection valueCollection = res.Properties[propName];
      //    foreach (Object propertyValue in valueCollection)
      //    {
      //      Console.WriteLine("Property: " + propName + ": " + propertyValue.ToString());
      //    }
      //  }
      //}

      return View(/*viewModel*/);
    }

    public ActionResult About()
    {
      AHub.Value.Clients.All.newmsg(User.Identity.Name, "Home/About");
      ViewBag.Message = "Your app description page.";
      //var files = new DirectoryInfo(ConfigurationManager.AppSettings["WebLogs"].ToString()).GetFiles("*.log");
      return View();
    }

    public ActionResult Contact()
    {
      ViewBag.Message = "Your contact page.";

      return View();
    }

    public ActionResult Rates()
    {
      RHub.Value.Clients.All.hello();
      return View();
    }

    [HttpPost]
    [ValidateInput(false)]
    public ActionResult ExportExcel(string f, string d)
    {
      string mimeType = "application/octet-stream";
      string ext = Path.GetExtension(f).ToLower();
      if (ext == ".zip")
        mimeType = "application/x-zip-compressed";
      else if (ext == ".rar")
        mimeType = "application/x-rar-compressed";
      else
      {
        Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
        if (regKey != null && regKey.GetValue("Content Type") != null)
        {
          mimeType = regKey.GetValue("Content Type").ToString();
        }
      }
      if (ext == ".csv")
        return File(System.Text.Encoding.GetEncoding(1251).GetBytes(d), mimeType, f);
      else
        return File(System.Text.Encoding.UTF8.GetBytes(d), mimeType, f);
    }

    public ActionResult Mission()
    {
      Mission m = new Mission();
      return View(m);
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult Mission(Mission m)
    {
      if (m.Option == null)
      {
        ViewBag.Error = "Не выбран ответ.";
        return View(m);
      }
      if (repRepository.MissionCreate(m, User.Identity.Name))
        return View("MissionCompleted");
      else
      {
        ViewBag.Error = "Произошла ошибка при отправке.";
        return View(m);
      }
    }

    public ActionResult Comments()
    {
      return File("comment.json", "application/json");
    }

    public ActionResult getRuDataMicex(DateTime dt, string engine = "stock", string market = "shares", string fields = "BOARDID", string boardId = "")
    {
      //var Url = "https://new-datahub.efir-net.ru/hub.axd/";
      var Url = "https://dh1.efir-net.ru/";
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
      WebClient wc = new WebClient();
      try
      {
        wc.Headers.Add("Content-type", "application/json");
        wc.Encoding = Encoding.UTF8;
        wc.Proxy.Credentials = CredentialCache.DefaultCredentials;
        var json = wc.UploadString(String.Format("{0}Account/Login", Url), "{login:'uralsibam_web',password:'9D56AwE'}");
        var accountLogin = JsonConvert.DeserializeObject<AccountLogin>(json);

        wc.Headers.Add("Content-type", "application/json");
        json = wc.UploadString(string.Format("{0}MOEX/History?token={1}", Url, accountLogin.Token), string.Format("{{ engine: '{1}',market: '{2}',boardid:'{3}',dateFrom: '{0:yyyy-MM-dd}T00:00:00.0000000Z',dateTo: '{0:yyyy-MM-dd}T00:00:00.0000000Z'}}", dt, engine, market, boardId));

        wc.Headers.Add("Content-type", "application/json");
        wc.UploadString(string.Format("{0}Account/Logoff", Url), string.Format("{{token: '{0}'}}", accountLogin.Token));

        var r = new Regex("(\\d{4})-(\\d{2})-(\\d{2})T\\d{2}:\\d{2}:\\d{2}.\\d{7}");
        var j = JsonConvert.DeserializeObject<InfoEmitents>(json);
        var ms = new MemoryStream();
        var sw = new StreamWriter(ms, Encoding.GetEncoding(1251));
        {
          sw.NewLine = "\n";
          var s = Regex.Match(json, "\"Columns\":\\[(.+?)\\]");
          if (s.Success)
          {
            sw.WriteLine("history");
            sw.WriteLine("");
            var cols = Regex.Matches(s.Groups[1].Value, "\\{\"(.+?)\":\"(.+?)\"\\}");
            List<string> ls = new List<string>();
            foreach (Match col in cols)
            {
              ls.Add(col.Groups[1].Value);
            }
            //var bn = ls.FindIndex(e => e == "BOARDID");
            var f = fields.Split(';');
            var b = boardId.Split(';');
            var fc = new List<int>();
            foreach (var _f in f)
            {
              fc.Add(ls.FindIndex((e) => e == _f));
            }
            sw.WriteLine(fields);
            if (j.Rows.Count > 0)
            {
              foreach (var row in j.Rows)
              {
                var rs = new List<string>();
                //var lbn = true;
                //if (bn != -1)
                //  lbn = b.Contains(row[bn]);
                //if (lbn)
                //{
                foreach (var _fc in fc)
                {
                  rs.Add(_fc == -1 ? "" : row[_fc]);
                }
                sw.WriteLine(r.Replace(string.Join(";", rs), "$3.$2.$1"));
                //}
              }
            }
          }
          sw.Flush();
          ms.Position = 0;
          return File(ms.ToArray(), "application/text", "securities.csv");
        }
      }
      catch (Exception ex)
      {
        return Content(ex.Message);
      }
    }

    public ActionResult getSpbExchange()
    {
      HttpWebRequest wr = null;
      var cookie = new CookieContainer() { MaxCookieSize = 16000, Capacity = 60, PerDomainCapacity = 60 };
      string postData = "";
      string s = "";
      StreamWriter requestWriter = null;

      ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
      ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
      ServicePointManager.Expect100Continue = true;
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
      var ViewState = "";
      wr = (HttpWebRequest)WebRequest.Create("https://spbexchange.ru/ru/listing/securities/list/");
      wr.Proxy = null;
      //wr.Proxy.Credentials = CredentialCache.DefaultCredentials;
      wr.Method = WebRequestMethods.Http.Get;
      wr.Timeout = 600000;
      wr.ReadWriteTimeout = 600000;
      wr.Accept = "text/html, application/xhtml+xml, */*";
      wr.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.73 Safari/537.36";
      wr.Headers.Add("Accept-Language", "en-US,en;q=0.7,ru;q=0.3");
      wr.CookieContainer = cookie;
      using (HttpWebResponse hwr = (HttpWebResponse)wr.GetResponse())
      {
        using (Stream receiveStream = hwr.GetResponseStream())
        {
          using (StreamReader sr = new StreamReader(receiveStream/*, Encoding.GetEncoding(1251)*/))
          {
            s = sr.ReadToEnd();
            var rm = Regex.Match(s, "<input type=\"hidden\" name=\"__VIEWSTATE\" id=\"__VIEWSTATE\" value=\"(.+?)\"");
            if (rm.Success)
              ViewState = HttpUtility.UrlEncode(rm.Groups[1].Value);//
          }
        }
      }

      wr = (HttpWebRequest)WebRequest.Create("https://spbexchange.ru/ru/listing/securities/list/");
      wr.Proxy = null;
      wr.Method = WebRequestMethods.Http.Post;
      wr.ProtocolVersion = HttpVersion.Version11;
      wr.Timeout = 600000;
      wr.ReadWriteTimeout = 600000;
      wr.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
      wr.UserAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36";
      wr.ContentType = "application/x-www-form-urlencoded";
      wr.Referer = "https://spbexchange.ru/ru/listing/securities/list/";
      wr.Headers.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
      wr.KeepAlive = true;
      wr.Headers.Add("Cache-Control", "no-cache");
      //wr.Headers.Add("Cookie", "ASP.NET1_SessionId=vqhowlylab0m1zz5fjxbdji4; jv_enter_ts_bo8WU67mTN=1539341568570; jv_visits_count_bo8WU67mTN=1; jv_utm_bo8WU67mTN=; BXBANNERS=24_11_181012; jv_pages_count_bo8WU67mTN=11");
      wr.Headers.Add("Cookie", "ASP.NET1_SessionId=vqhowlylab0m1zz5fjxbdji4");
      //wr.CookieContainer = cookie;
      postData = "bitrix_include_areas=N&__EVENTTARGET=ctl00%24BXContent%24list%24LinkButton1&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=%2FwEPDwUKLTcwOTA1MTYxNg9kFgJmD2QWAgIBD2QWBgIBD2QWAgIBDxYCHgVzdHlsZQUnd2lkdGg6MHB4OyBoZWlnaHQ6MHB4OyBvdmVyZmxvdzpoaWRkZW47ZAIFDw8WAh4EVGV4dAUfMTIg0L7QutGC0Y%2FQsdGA0Y8gMjAxOCDQs9C%2B0LTQsGRkAgwQZGQWAgIBD2QWBAICD2QWAmYPZBYCAgEPZBYEAgUPEGQPFgVmAgECAgIDAgQWBRBlZWcQBQrQkNC60YbQuNC4BQrQkNC60YbQuNC4ZxAFKdCU0LXQv9C%2B0LfQuNGC0LDRgNC90YvQtSDRgNCw0YHQv9C40YHQutC4BSnQlNC10L%2FQvtC30LjRgtCw0YDQvdGL0LUg0YDQsNGB0L%2FQuNGB0LrQuGcQBSPQmNC90LLQtdGB0YLQuNGG0LjQvtC90L3Ri9C1INC%2F0LDQuAUj0JjQvdCy0LXRgdGC0LjRhtC40L7QvdC90YvQtSDQv9Cw0LhnEAUS0J7QsdC70LjQs9Cw0YbQuNC4BRLQntCx0LvQuNCz0LDRhtC40LhnFgFmZAIHDxBkEBURAE7QkNC60YbQuNC4INC40L3QvtGB0YLRgNCw0L3QvdC%2B0LPQviDRjdC80LjRgtC10L3RgtCwICDQvtCx0YvQutC90L7QstC10L3QvdGL0LVd0JDQutGG0LjQuCDQuNC90L7RgdGC0YDQsNC90L3QvtCz0L4g0Y3QvNC40YLQtdC90YLQsCAg0L7QsdGL0LrQvdC%2B0LLQtdC90L3Ri9C1INC60LvQsNGB0YHQsCBBXdCQ0LrRhtC40Lgg0LjQvdC%2B0YHRgtGA0LDQvdC90L7Qs9C%2BINGN0LzQuNGC0LXQvdGC0LAgINC%2B0LHRi9C60L3QvtCy0LXQvdC90YvQtSDQutC70LDRgdGB0LAgQl3QkNC60YbQuNC4INC40L3QvtGB0YLRgNCw0L3QvdC%2B0LPQviDRjdC80LjRgtC10L3RgtCwICDQvtCx0YvQutC90L7QstC10L3QvdGL0LUg0LrQu9Cw0YHRgdCwIENd0JDQutGG0LjQuCDQuNC90L7RgdGC0YDQsNC90L3QvtCz0L4g0Y3QvNC40YLQtdC90YLQsCAg0L7QsdGL0LrQvdC%2B0LLQtdC90L3Ri9C1INC60LvQsNGB0YHQsCBQTdCQ0LrRhtC40Lgg0LjQvdC%2B0YHRgtGA0LDQvdC90L7Qs9C%2BINGN0LzQuNGC0LXQvdGC0LAg0L7QsdGL0LrQvdC%2B0LLQtdC90L3Ri9C1XNCQ0LrRhtC40Lgg0LjQvdC%2B0YHRgtGA0LDQvdC90L7Qs9C%2BINGN0LzQuNGC0LXQvdGC0LAg0L7QsdGL0LrQvdC%2B0LLQtdC90L3Ri9C1INC60LvQsNGB0YHQsCBBI9CQ0LrRhtC40Lgg0L7QsdGL0LrQvdC%2B0LLQtdC90L3Ri9C1LdCQ0LrRhtC40Lgg0L%2FRgNC40LLQuNC70LXQs9C40YDQvtCy0LDQvdC90YvQtWPQlNC10L%2FQvtC30LjRgtCw0YDQvdGL0LUg0YDQsNGB0L%2FQuNGB0LrQuCDQuNC90L7RgdGC0YDQsNC90L3QvtCz0L4g0Y3QvNC40YLQtdC90YLQsCDQvdCwINCw0LrRhtC40Li2AdCY0L3QstC10YHRgtC40YbQuNC%2B0L3QvdGL0LUg0L%2FQsNC4INC30LDQutGA0YvRgtC%2B0LPQviDQv9Cw0LXQstC%2B0LPQviDQuNC90LLQtdGB0YLQuNGG0LjQvtC90L3QvtCz0L4g0YTQvtC90LTQsCDQvdC10LTQstC40LbQuNC80L7RgdGC0LggICLQkNCaINCR0JDQoNChIOKAkyDQndC10LTQstC40LbQuNC80L7RgdGC0YwiugHQmNC90LLQtdGB0YLQuNGG0LjQvtC90L3Ri9C1INC%2F0LDQuCDQt9Cw0LrRgNGL0YLQvtCz0L4g0L%2FQsNC10LLQvtCz0L4g0LjQvdCy0LXRgdGC0LjRhtC40L7QvdC90L7Qs9C%2BINGE0L7QvdC00LAg0L%2FRgNGP0LzRi9GFINC40L3QstC10YHRgtC40YbQuNC5ICLQkNCaINCR0JDQoNChIOKAkyDQmNC90LLQtdGB0YLQuNGG0LjQuCIj0J7QsdC70LjQs9Cw0YbQuNC4INCx0LjRgNC20LXQstGL0LVL0J7QsdC70LjQs9Cw0YbQuNC4INCy0L3QtdGI0L3QtdCz0L4g0L7QsdC70LjQs9Cw0YbQuNC%2B0L3QvdC%2B0LPQviDQt9Cw0LnQvNCwPNCe0LHQu9C40LPQsNGG0LjQuCDQuNC90L7RgdGC0YDQsNC90L3QvtCz0L4g0Y3QvNC40YLQtdC90YLQsC3QntCx0LvQuNCz0LDRhtC40Lgg0LrQvtGA0L%2FQvtGA0LDRgtC40LLQvdGL0LUVEQBO0JDQutGG0LjQuCDQuNC90L7RgdGC0YDQsNC90L3QvtCz0L4g0Y3QvNC40YLQtdC90YLQsCAg0L7QsdGL0LrQvdC%2B0LLQtdC90L3Ri9C1XdCQ0LrRhtC40Lgg0LjQvdC%2B0YHRgtGA0LDQvdC90L7Qs9C%2BINGN0LzQuNGC0LXQvdGC0LAgINC%2B0LHRi9C60L3QvtCy0LXQvdC90YvQtSDQutC70LDRgdGB0LAgQV3QkNC60YbQuNC4INC40L3QvtGB0YLRgNCw0L3QvdC%2B0LPQviDRjdC80LjRgtC10L3RgtCwICDQvtCx0YvQutC90L7QstC10L3QvdGL0LUg0LrQu9Cw0YHRgdCwIEJd0JDQutGG0LjQuCDQuNC90L7RgdGC0YDQsNC90L3QvtCz0L4g0Y3QvNC40YLQtdC90YLQsCAg0L7QsdGL0LrQvdC%2B0LLQtdC90L3Ri9C1INC60LvQsNGB0YHQsCBDXdCQ0LrRhtC40Lgg0LjQvdC%2B0YHRgtGA0LDQvdC90L7Qs9C%2BINGN0LzQuNGC0LXQvdGC0LAgINC%2B0LHRi9C60L3QvtCy0LXQvdC90YvQtSDQutC70LDRgdGB0LAgUE3QkNC60YbQuNC4INC40L3QvtGB0YLRgNCw0L3QvdC%2B0LPQviDRjdC80LjRgtC10L3RgtCwINC%2B0LHRi9C60L3QvtCy0LXQvdC90YvQtVzQkNC60YbQuNC4INC40L3QvtGB0YLRgNCw0L3QvdC%2B0LPQviDRjdC80LjRgtC10L3RgtCwINC%2B0LHRi9C60L3QvtCy0LXQvdC90YvQtSDQutC70LDRgdGB0LAgQSPQkNC60YbQuNC4INC%2B0LHRi9C60L3QvtCy0LXQvdC90YvQtS3QkNC60YbQuNC4INC%2F0YDQuNCy0LjQu9C10LPQuNGA0L7QstCw0L3QvdGL0LVj0JTQtdC%2F0L7Qt9C40YLQsNGA0L3Ri9C1INGA0LDRgdC%2F0LjRgdC60Lgg0LjQvdC%2B0YHRgtGA0LDQvdC90L7Qs9C%2BINGN0LzQuNGC0LXQvdGC0LAg0L3QsCDQsNC60YbQuNC4tgHQmNC90LLQtdGB0YLQuNGG0LjQvtC90L3Ri9C1INC%2F0LDQuCDQt9Cw0LrRgNGL0YLQvtCz0L4g0L%2FQsNC10LLQvtCz0L4g0LjQvdCy0LXRgdGC0LjRhtC40L7QvdC90L7Qs9C%2BINGE0L7QvdC00LAg0L3QtdC00LLQuNC20LjQvNC%2B0YHRgtC4ICAi0JDQmiDQkdCQ0KDQoSDigJMg0J3QtdC00LLQuNC20LjQvNC%2B0YHRgtGMIroB0JjQvdCy0LXRgdGC0LjRhtC40L7QvdC90YvQtSDQv9Cw0Lgg0LfQsNC60YDRi9GC0L7Qs9C%2BINC%2F0LDQtdCy0L7Qs9C%2BINC40L3QstC10YHRgtC40YbQuNC%2B0L3QvdC%2B0LPQviDRhNC%2B0L3QtNCwINC%2F0YDRj9C80YvRhSDQuNC90LLQtdGB0YLQuNGG0LjQuSAi0JDQmiDQkdCQ0KDQoSDigJMg0JjQvdCy0LXRgdGC0LjRhtC40LgiI9Ce0LHQu9C40LPQsNGG0LjQuCDQsdC40YDQttC10LLRi9C1S9Ce0LHQu9C40LPQsNGG0LjQuCDQstC90LXRiNC90LXQs9C%2BINC%2B0LHQu9C40LPQsNGG0LjQvtC90L3QvtCz0L4g0LfQsNC50LzQsDzQntCx0LvQuNCz0LDRhtC40Lgg0LjQvdC%2B0YHRgtGA0LDQvdC90L7Qs9C%2BINGN0LzQuNGC0LXQvdGC0LAt0J7QsdC70LjQs9Cw0YbQuNC4INC60L7RgNC%2F0L7RgNCw0YLQuNCy0L3Ri9C1FCsDEWdnZ2dnZ2dnZ2dnZ2dnZ2dnFgFmZAIDD2QWAmYPZBYEAgEPFCsAAg8WBB4LXyFEYXRhQm91bmRnHgtfIUl0ZW1Db3VudAL5BGRkFgJmD2QWFAIBD2QWAmYPFRABMTbQndC10LrQvtGC0LjRgNC%2B0LLQsNC70YzQvdCw0Y8g0YfQsNGB0YLRjCDQodC%2F0LjRgdC60LAaQWdpbGVudCBUZWNobm9sb2dpZXMsIEluYy4BLQrQkNC60YbQuNC4TtCQ0LrRhtC40Lgg0LjQvdC%2B0YHRgtGA0LDQvdC90L7Qs9C%2BINGN0LzQuNGC0LXQvdGC0LAgINC%2B0LHRi9C60L3QvtCy0LXQvdC90YvQtUw8YSBocmVmPSIvcnUvbGlzdGluZy9zZWN1cml0aWVzL2ljYl9wYWdlcy8%2FaXNzdWU9MzY5MyIgdGFyZ2V0PSJfYmxhbmsiPkE8L2E%2BDFVTMDA4NDZVMTAxNgEtAS0EMCwwMRPQlNC%2B0LvQu9Cw0YAg0KHQqNCQCjE3LjExLjIwMTYBLQEtAS1kAgIPZBYCZg8VEAEyNtCd0LXQutC%2B0YLQuNGA0L7QstCw0LvRjNC90LDRjyDRh9Cw0YHRgtGMINCh0L%2FQuNGB0LrQsBFBbGNvYSBDb3Jwb3JhdGlvbgEtCtCQ0LrRhtC40LhO0JDQutGG0LjQuCDQuNC90L7RgdGC0YDQsNC90L3QvtCz0L4g0Y3QvNC40YLQtdC90YLQsCAg0L7QsdGL0LrQvdC%2B0LLQtdC90L3Ri9C1TTxhIGhyZWY9Ii9ydS9saXN0aW5nL3NlY3VyaXRpZXMvaWNiX3BhZ2VzLz9pc3N1ZT02MDQxIiB0YXJnZXQ9Il9ibGFuayI%2BQUE8L2E%2BDFVTMDEzODcyMTA2NQEtAS0EMCwwMRPQlNC%2B0LvQu9Cw0YAg0KHQqNCQCjIyLjExLjIwMTYBLQEtAS1kAgMPZBYCZg8VEAEzNtCd0LXQutC%2B0YLQuNGA0L7QstCw0LvRjNC90LDRjyDRh9Cw0YHRgtGMINCh0L%2FQuNGB0LrQsAtBbHRhYmEgSW5jLgEtCtCQ0LrRhtC40LhO0JDQutGG0LjQuCDQuNC90L7RgdGC0YDQsNC90L3QvtCz0L4g0Y3QvNC40YLQtdC90YLQsCAg0L7QsdGL0LrQvdC%2B0LLQtdC90L3Ri9C1TzxhIGhyZWY9Ii9ydS9saXN0aW5nL3NlY3VyaXRpZXMvaWNiX3BhZ2VzLz9pc3N1ZT01OTUwIiB0YXJnZXQ9Il9ibGFuayI%2BQUFCQTwvYT4MVVMwMjEzNDYxMDE3AS0BLQUwLDAwMRPQlNC%2B0LvQu9Cw0YAg0KHQqNCQCjI3LjExLjIwMTQBLQEtAS1kAgQPZBYCZg8VEAE0NtCd0LXQutC%2B0YLQuNGA0L7QstCw0LvRjNC90LDRjyDRh9Cw0YHRgtGMINCh0L%2FQuNGB0LrQsBxBbWVyaWNhbiBBaXJsaW5lcyBHcm91cCBJbmMuAS0K0JDQutGG0LjQuE7QkNC60YbQuNC4INC40L3QvtGB0YLRgNCw0L3QvdC%2B0LPQviDRjdC80LjRgtC10L3RgtCwICDQvtCx0YvQutC90L7QstC10L3QvdGL0LVOPGEgaHJlZj0iL3J1L2xpc3Rpbmcvc2VjdXJpdGllcy9pY2JfcGFnZXMvP2lzc3VlPTM2OTUiIHRhcmdldD0iX2JsYW5rIj5BQUw8L2E%2BDFVTMDIzNzZSMTAyMwEtAS0EMCwwMRPQlNC%2B0LvQu9Cw0YAg0KHQqNCQCjE3LjExLjIwMTYBLQEtAS1kAgUPZBYCZg8VEAE1NtCd0LXQutC%2B0YLQuNGA0L7QstCw0LvRjNC90LDRjyDRh9Cw0YHRgtGMINCh0L%2FQuNGB0LrQsBhBZHZhbmNlIEF1dG8gUGFydHMsIEluYy4BLQrQkNC60YbQuNC4TtCQ0LrRhtC40Lgg0LjQvdC%2B0YHRgtGA0LDQvdC90L7Qs9C%2BINGN0LzQuNGC0LXQvdGC0LAgINC%2B0LHRi9C60L3QvtCy0LXQvdC90YvQtU48YSBocmVmPSIvcnUvbGlzdGluZy9zZWN1cml0aWVzL2ljYl9wYWdlcy8%2FaXNzdWU9MzY5OCIgdGFyZ2V0PSJfYmxhbmsiPkFBUDwvYT4MVVMwMDc1MVkxMDY0AS0BLQYwLDAwMDET0JTQvtC70LvQsNGAINCh0KjQkAoxNy4xMS4yMDE2AS0BLQEtZAIGD2QWAmYPFRABNjbQndC10LrQvtGC0LjRgNC%2B0LLQsNC70YzQvdCw0Y8g0YfQsNGB0YLRjCDQodC%2F0LjRgdC60LAKQXBwbGUgSW5jLgEtCtCQ0LrRhtC40LhO0JDQutGG0LjQuCDQuNC90L7RgdGC0YDQsNC90L3QvtCz0L4g0Y3QvNC40YLQtdC90YLQsCAg0L7QsdGL0LrQvdC%2B0LLQtdC90L3Ri9C1TzxhIGhyZWY9Ii9ydS9saXN0aW5nL3NlY3VyaXRpZXMvaWNiX3BhZ2VzLz9pc3N1ZT0zNjk5IiB0YXJnZXQ9Il9ibGFuayI%2BQUFQTDwvYT4MVVMwMzc4MzMxMDA1AS0BLQcwLDAwMDAxE9CU0L7Qu9C70LDRgCDQodCo0JAKMjcuMTEuMjAxNAEtAS0BLWQCBw9kFgJmDxUQATc20J3QtdC60L7RgtC40YDQvtCy0LDQu9GM0L3QsNGPINGH0LDRgdGC0Ywg0KHQv9C40YHQutCwP9CQ0LrRhtC40L7QvdC10YDQvdC%2B0LUg0L7QsdGJ0LXRgdGC0LLQviAi0JHQsNC90Log0JDRgdGC0LDQvdGLIgEtCtCQ0LrRhtC40LhN0JDQutGG0LjQuCDQuNC90L7RgdGC0YDQsNC90L3QvtCz0L4g0Y3QvNC40YLQtdC90YLQsCDQvtCx0YvQutC90L7QstC10L3QvdGL0LVPPGEgaHJlZj0iL3J1L2xpc3Rpbmcvc2VjdXJpdGllcy9pY2JfcGFnZXMvP2lzc3VlPTYxMDYiIHRhcmdldD0iX2JsYW5rIj5BQkJOPC9hPgxLWjFDMDAwMDEwMjMBLQEtBDEwMDAK0KLQtdC90LPQtQoyMC4wNi4yMDE3AS0BLQEtZAIID2QWAmYPFRABODbQndC10LrQvtGC0LjRgNC%2B0LLQsNC70YzQvdCw0Y8g0YfQsNGB0YLRjCDQodC%2F0LjRgdC60LALQWJiVmllIEluYy4BLQrQkNC60YbQuNC4TtCQ0LrRhtC40Lgg0LjQvdC%2B0YHRgtGA0LDQvdC90L7Qs9C%2BINGN0LzQuNGC0LXQvdGC0LAgINC%2B0LHRi9C60L3QvtCy0LXQvdC90YvQtU88YSBocmVmPSIvcnUvbGlzdGluZy9zZWN1cml0aWVzL2ljYl9wYWdlcy8%2FaXNzdWU9MzcwMyIgdGFyZ2V0PSJfYmxhbmsiPkFCQlY8L2E%2BDFVTMDAyODdZMTA5MQEtAS0EMCwwMRPQlNC%2B0LvQu9Cw0YAg0KHQqNCQCjI3LjExLjIwMTQBLQEtAS1kAgkPZBYCZg8VEAE5NtCd0LXQutC%2B0YLQuNGA0L7QstCw0LvRjNC90LDRjyDRh9Cw0YHRgtGMINCh0L%2FQuNGB0LrQsBNBYmJvdHQgTGFib3JhdG9yaWVzAS0K0JDQutGG0LjQuE7QkNC60YbQuNC4INC40L3QvtGB0YLRgNCw0L3QvdC%2B0LPQviDRjdC80LjRgtC10L3RgtCwICDQvtCx0YvQutC90L7QstC10L3QvdGL0LVOPGEgaHJlZj0iL3J1L2xpc3Rpbmcvc2VjdXJpdGllcy9pY2JfcGFnZXMvP2lzc3VlPTM3MDkiIHRhcmdldD0iX2JsYW5rIj5BQlQ8L2E%2BDFVTMDAyODI0MTAwMAEtAS0w0LHQtdC3INC90L7QvNC40L3QsNC70YzQvdC%2B0Lkg0YHRgtC%2B0LjQvNC%2B0YHRgtC4AS0KMTcuMTEuMjAxNgEtAS0BLWQCCg9kFgJmDxUQAjEwNtCd0LXQutC%2B0YLQuNGA0L7QstCw0LvRjNC90LDRjyDRh9Cw0YHRgtGMINCh0L%2FQuNGB0LrQsBtUaGUgQmFuayBvZiBOZXcgWW9yayBNZWxsb24BLSnQlNC10L%2FQvtC30LjRgtCw0YDQvdGL0LUg0YDQsNGB0L%2FQuNGB0LrQuGPQlNC10L%2FQvtC30LjRgtCw0YDQvdGL0LUg0YDQsNGB0L%2FQuNGB0LrQuCDQuNC90L7RgdGC0YDQsNC90L3QvtCz0L4g0Y3QvNC40YLQtdC90YLQsCDQvdCwINCw0LrRhtC40LhWPGEgaHJlZj0iL3J1L2xpc3Rpbmcvc2VjdXJpdGllcy9pY2JfcGFnZXMvYWRyLmFzcHg%2FaXNzdWU9NjAyMiIgdGFyZ2V0PSJfYmxhbmsiPkFDSDwvYT4MVVMwMjIyNzYxMDkyAS0BLQEtAS0KMjQuMDguMjAxNgEtAS0BLWQCAw8UKwACZBAWABYAFgBkGAMFHl9fQ29udHJvbHNSZXF1aXJlUG9zdEJhY2tLZXlfXxYBBTFjdGwwMCRzZWFyY2hmb3JtMSRzZWFyY2hmb3JtMSRzZWFyY2hmb3JtMSRidG5TcmNoBRpjdGwwMCRCWENvbnRlbnQkbGlzdCRwYWdlcg88KwAEAQMC%2BQRkBRdjdGwwMCRCWENvbnRlbnQkbGlzdCRsdg8UKwAOZGRkZGRkZDwrAAoAAvkEZGRkZgIKZImlvaGGyQSIVNrdioKymVl%2Fcc8G&bxValidationToken=39ed32296dfab1184da7e0babf386e95&ctl00%24searchform1%24searchform1%24searchform1%24query=%D0%9F%D0%BE%D0%B8%D1%81%D0%BA...&ctl00%24BXContent%24list%24tbSearch=&ctl00%24BXContent%24list%24ddlCBView=&ctl00%24BXContent%24list%24ddlCBCat=&__VIEWSTATEGENERATOR=8882E091&__EVENTVALIDATION=%2FwEdAClWPR8UdDSwPUu%2FUkJbqvM9fFsezz3GuGexXlwRGdGgE4jhaSetPRJbd7PpVxUwh7GjmJTkdUc8cHgVjH91N7IQOisum7gS8BofEndmb6UEeqDZ%2BqK4NR4QwRKSRnfP55p7YkrNvXEGN%2B2Dhy6N32J%2FLLGZd5JC4dt%2BkLfPIUlTSilI87q5qRJWcm3cNab9qeilh3nogKNUjNt6xIqATX7CsKl6SaizbCVweundZOmMMMSfVGUGIgX9Np69%2BMKo92gtdnzeTA4s1MGOOvXFw9hoIC2iyRn1nG0HxtNekV1aZHMcgn1vrP7ObwWEtW%2BW5tbxU1nua7MQU4HOB%2F5Lbt4AhETJ7B7%2Fp6mbX56UWKzYsBMGQUh%2BQ09%2Bykv04%2FR15XF0fLg1x3AGNPuD79Tsf%2B7HiC2tolYL1KF00DJNhPvtoMjxjt4N4NLpPK5FBakR74nel62UlMpqrUrg78fzdrSp7ub652gPJJlEF9TVjLs2o6s0QjfgvDHWS%2B2yD2bRr%2FpfuwnPp2NwCUvLdCPaGQ%2FNP8qz8BtLBR2KlYzkeWHcFFyZxobl3C2X427fq78LdWlrqFOb%2FZB%2Fkg%2BLPZgPdW53SfYfkcpXPXIcDpoeJHiNap21iVR1%2FJDQkfwLHnk4mcVNPtaW%2BtkSeIA1krFjeM67AuV5G7lM6bgZTaItIvCmzJwyMtcu%2BIgjf%2FoRlbfCiU0qmhodZNxQHvcy8n2gFlSfIDfra2lh5WSbHDXLV5HJNNRF2uJufbgi99tWUWmqpe%2BMlDJfDupd19rDXMbn8ZMxLRSwiWa0E2e45i9RC6op403xnOIJGNUUkdvB5aoZ8RyXMNi4uxI4G6N3%2B394jpjsDJ78EmdlNYx9GN%2FlNrm4HDIkJb3wPh8v5yuhlhBt0v59D30E";
      //postData = string.Format("bitrix_include_areas=N&__EVENTTARGET=ctl00%24BXContent%24list%24LinkButton1&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=" + ViewState);
      wr.ContentLength = postData.Length;
      using (requestWriter = new StreamWriter(wr.GetRequestStream()))
      {
        requestWriter.Write(postData);
        requestWriter.Close();
      }

      var ms = new MemoryStream();
      using (HttpWebResponse hwr = (HttpWebResponse)wr.GetResponse())
      {
        using (Stream receiveStream = hwr.GetResponseStream())
        {
          using (StreamReader sr = new StreamReader(receiveStream, Encoding.GetEncoding(1251)))
          {
            s = sr.ReadToEnd();
            var lines = s.Split(new char[] { '\n' });
            using (var sw = new StreamWriter(ms, Encoding.GetEncoding(1251)))
            {
              for (int i = 1; i < lines.Length; i++)
              {
                var row = lines[i].Split(new char[] { ';' });
                if (row.Length > 7 && !string.IsNullOrEmpty(row[7]))
                {
                  sw.WriteLine(row[7]);
                }
              }
              sw.Flush();
              ms.Position = 0;
              return File(ms.ToArray(), "application/text", "securities.csv");
            }
          }
        }
      } 
    }

    public ActionResult getRusBonds()
    {
      return File(RusBonds.getRusBonds(), "application/text", "rusbonds.txt");
    }

    public ActionResult getMoexList()
    {
      return File(MoexList.getMoexList(), "application/text", "rusbonds.txt");
    }

    public ActionResult Pai()
    {
      SurveyViewModel data = new SurveyViewModel();
      return View(data);
    }

    [HttpPost]
    public ActionResult Pai(SurveyViewModel data)
    {
      if (data.a1 == null)
      {
        ModelState.AddModelError("a1", "Незаполнено");
      }
      if (data.a2 == null)
      {
        ModelState.AddModelError("a2", "Незаполнено");
      }
      if (data.a3 == null)
      {
        ModelState.AddModelError("a3", "Незаполнено");
      }
      if (data.a4 == null)
      {
        ModelState.AddModelError("a4", "Незаполнено");
      }
      if (data.a5 == null)
      {
        ModelState.AddModelError("a5", "Незаполнено");
      }
      if (data.a6 == null)
      {
        ModelState.AddModelError("a6", "Незаполнено");
      }
      if (data.a7 == null || data.a7 == 3 && string.IsNullOrWhiteSpace(data.n73) || data.a7 == 4 && string.IsNullOrWhiteSpace(data.n74))
      {
        ModelState.AddModelError("a7", "Незаполнено");
      }
      if (data.a8 == null)
      {
        ModelState.AddModelError("a8", "Незаполнено");
      }
      if (data.a9 == null)
      {
        ModelState.AddModelError("a9", "Незаполнено");
      }
      if (data.a10 == null)
      {
        ModelState.AddModelError("a10", "Незаполнено");
      }
      if (data.a11 == null)
      {
        ModelState.AddModelError("a11", "Незаполнено");
      }
      if (data.a12 == null)
      {
        ModelState.AddModelError("a12", "Незаполнено");
      }
      if (data.a13 == null)
      {
        ModelState.AddModelError("a13", "Незаполнено");
      }
      if (data.a14 == null)
      {
        ModelState.AddModelError("a14", "Незаполнено");
      }
      if (data.a17 == null)
      {
        ModelState.AddModelError("a17", "Незаполнено");
      }
      if (data.a18 == null)
      {
        ModelState.AddModelError("a18", "Незаполнено");
      }
      if (data.a19 == null)
      {
        ModelState.AddModelError("a19", "Незаполнено");
      }
      if (data.a20 == null)
      {
        ModelState.AddModelError("a20", "Незаполнено");
      }
      if (data.a21 == null)
      {
        ModelState.AddModelError("a21", "Незаполнено");
      }
      if (data.a17 == 4 && string.IsNullOrWhiteSpace(data.n17))
      {
        ModelState.AddModelError("a17", "Незаполнено");
      }
      if (data.a18 == 4 && string.IsNullOrWhiteSpace(data.n18))
      {
        ModelState.AddModelError("a18", "Незаполнено");
      }
      if (data.a19 == 4 && string.IsNullOrWhiteSpace(data.n19))
      {
        ModelState.AddModelError("a19", "Незаполнено");
      }
      if (data.a20 == 4 && string.IsNullOrWhiteSpace(data.n20))
      {
        ModelState.AddModelError("a20", "Незаполнено");
      }
      if (data.a21 == 4 && string.IsNullOrWhiteSpace(data.n21))
      {
        ModelState.AddModelError("a21", "Незаполнено");
      }
      var n15 = (data.a151 == true ? 1 : 0) + (data.a152 == true ? 1 : 0) + (data.a153 == true ? 1 : 0) + (data.a154 == true ? 1 : 0) + (data.a155 == true ? 1 : 0) + (data.a156 == true ? 1 : 0) + (data.a157 == true ? 1 : 0) + (data.a158 == true ? 1 : 0);
      if (n15 == 0 || (data.a158 == true && string.IsNullOrWhiteSpace(data.n15)))
      {
        ModelState.AddModelError("n15", "Незаполнено");
      }
      var n16 = (data.a161 == true ? 1 : 0) + (data.a162 == true ? 1 : 0) + (data.a163 == true ? 1 : 0) + (data.a164 == true ? 1 : 0) + (data.a165 == true ? 1 : 0) + (data.a166 == true ? 1 : 0) + (data.a167 == true ? 1 : 0) + (data.a168 == true ? 1 : 0) + (data.a169 == true ? 1 : 0) + (data.a1610 == true ? 1 : 0) + (data.a1611 == true ? 1 : 0) + (data.a1612 == true ? 1 : 0) + (data.a1613 == true ? 1 : 0) + (data.a1614 == true ? 1 : 0) + (data.a1615 == true ? 1 : 0) + (data.a1616 == true ? 1 : 0) + (data.a1617 == true ? 1 : 0);
      if (n16 > 5)
      {
        ModelState.AddModelError("n16", "Не более 5 вариантов");
      }
      else if (n16 == 0 || (data.a1616 == true && string.IsNullOrWhiteSpace(data.n16)))
      {
        ModelState.AddModelError("n16", "Незаполнено");
      }

      if (ModelState.IsValid)
      {
        if (repRepository.AddUpdSurvey(data, User.Identity.Name))
        {
          ViewBag.Type = "опросе";
          ViewBag.Title = "Опрос";
          return View("MissionCompleted");
        }
        else
          ViewBag.Error = "Ошибка при сохранении данных";
      }
      else
        ViewBag.Error = "Ошибка! Не на все вопросы дан ответ";
      return View(data);
    }
    public ActionResult GetLastWorkDate()
    {
      return Content(string.Format("{0:ddMMyy}", repRepository.GetLastWorkDate()));
    }
  }
}
