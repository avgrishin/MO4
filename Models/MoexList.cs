using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace MO.Models
{
  public static class MoexList
  {
    public static byte[] getMoexList()
    {
      ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });
      ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
      HttpWebRequest wr;
      wr = (HttpWebRequest)WebRequest.Create("https://www.moex.com/ru/listing/securities-list-csv.aspx?type=2");
      wr.Proxy.Credentials = CredentialCache.DefaultCredentials;
      wr.Proxy = null;
      wr.ProtocolVersion = HttpVersion.Version10;
      wr.Method = WebRequestMethods.Http.Get;
      wr.Timeout = 600000;
      wr.ReadWriteTimeout = 600000;
      wr.Accept = "text/html, application/xhtml+xml, */*";
      wr.Headers.Add("Accept-Language", "ru-RU");
      wr.Headers.Add("Pragma", "no-cache");
      wr.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)";
      using (HttpWebResponse hwr = (HttpWebResponse)wr.GetResponse())
      using (Stream receiveStream = hwr.GetResponseStream())
      using (StreamReader sr = new StreamReader(receiveStream, Encoding.UTF8))
      {
        string s = sr.ReadToEnd();
        using (var ms = new MemoryStream())
        using (StreamWriter sw1 = new StreamWriter(ms, Encoding.GetEncoding(1251)))
        {
          sw1.Write(s.Replace("\"", "").Replace(";\r\n", "\r")/*.Replace("\r\n", "\r")*/.Replace("\n", " "));
          sw1.Flush();
          ms.Position = 0;
          return ms.ToArray();
        }
      }
    }
  }
}