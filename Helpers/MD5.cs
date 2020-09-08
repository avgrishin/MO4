using System.Security.Cryptography;
using System.Text;

namespace MO.Helpers
{
  public static class MD5Hash
  {
    public static string GetMd5Hash(string input)
    {
      StringBuilder sBuilder = new StringBuilder();
      using (MD5 md5Hash = MD5.Create())
      {
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        for (int i = 0; i < data.Length; i++)
        {
          sBuilder.Append(data[i].ToString("x2"));
        }
      }
      return sBuilder.ToString();
    }

  }
}