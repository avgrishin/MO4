﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MO.Areas.Code.Views.Leave
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    public partial class SendFinal : RazorGenerator.Templating.RazorTemplateBase
    {
#line hidden
        #line 3 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
            
  public dynamic q { get; set; }
  public string host { get; set; }
  public string email { get; set; }

        #line default
        #line hidden
        
        public override void Execute()
        {
WriteLiteral("\r\n\r\n");

WriteLiteral(@"
<style>
  table {
    border-collapse: collapse;
    border: 1px solid gray;
  }

  td, span, th {
    font-size: .8em;
    font-family: Verdana, Helvetica, Sans-Serif;
    text-align: left;
  }

  span {
    font-style: italic;
  }

  th {
    font-size: .7em;
  }
</style>
<h3>Заявление на отпуск</h3>
<table>
  <tr>
    <th>Сотрудник</th>
    <td>");

            
            #line 33 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
   Write(q.Name1);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n    <td>");

            
            #line 34 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
   Write(q.UserName1);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n    <td>");

            
            #line 35 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
   Write(q.Sign1.ToString("dd.MM.yyyy HH:mm"));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n  </tr>\r\n  <tr>\r\n    <th>Вид отпуска</th>\r\n    <td");

WriteLiteral(" colspan=\"3\"");

WriteLiteral(">");

            
            #line 39 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
               Write(q.TypeName);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n  </tr>\r\n  <tr>\r\n    <th>Период</th>\r\n    <td>");

            
            #line 43 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
   Write(q.DateB.ToString("dd.MM.yyyy"));

            
            #line default
            #line hidden
WriteLiteral(" - ");

            
            #line 43 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
                                     Write(q.DateE.ToString("dd.MM.yyyy"));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n    <th>Календарных дней</th>\r\n    <td>");

            
            #line 45 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
   Write(q.Days.ToString());

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n  </tr>\r\n");

            
            #line 47 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
  
            
            #line default
            #line hidden
            
            #line 47 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
   if (!string.IsNullOrEmpty(q.Comment1))
  {

            
            #line default
            #line hidden
WriteLiteral("    <tr>\r\n      <th>Примечание</th>\r\n      <td");

WriteLiteral(" colspan=\"3\"");

WriteLiteral(">");

            
            #line 51 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
                 Write(q.Comment1);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n    </tr>\r\n");

            
            #line 53 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
  }

            
            #line default
            #line hidden
WriteLiteral("  <tr>\r\n    <th>Обязанности будут возлагаться на</th>\r\n    <td");

WriteLiteral(" colspan=\"3\"");

WriteLiteral(">");

            
            #line 56 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
               Write(q.Name2);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n  </tr>\r\n  <tr>\r\n");

            
            #line 59 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
    
            
            #line default
            #line hidden
            
            #line 59 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
     if (q.Sign4 == null)
    {

            
            #line default
            #line hidden
WriteLiteral("      <th>Требует согласования</th>\r\n");

WriteLiteral("      <td>");

            
            #line 62 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
     Write(q.Name4);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n");

WriteLiteral("      <td>");

            
            #line 63 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
     Write(q.UserName4);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n");

WriteLiteral("      <td>&nbsp;</td>\r\n");

            
            #line 65 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
    }
    else
    {

            
            #line default
            #line hidden
WriteLiteral("      <th>Согласовано</th>\r\n");

WriteLiteral("      <td>");

            
            #line 69 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
     Write(q.Name4);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n");

WriteLiteral("      <td>");

            
            #line 70 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
     Write(q.UserName4);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n");

WriteLiteral("      <td>");

            
            #line 71 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
     Write(q.Sign4.ToString("dd.MM.yyyy HH:mm"));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n");

            
            #line 72 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("  </tr>\r\n");

            
            #line 74 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
  
            
            #line default
            #line hidden
            
            #line 74 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
   if (!string.IsNullOrEmpty(q.Comment4))
  {

            
            #line default
            #line hidden
WriteLiteral("    <tr>\r\n      <th>Примечание</th>\r\n      <td");

WriteLiteral(" colspan=\"3\"");

WriteLiteral(">");

            
            #line 78 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
                 Write(q.Comment4);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n    </tr>\r\n");

            
            #line 80 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
  }

            
            #line default
            #line hidden
WriteLiteral("  ");

            
            #line 81 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
   if (!string.IsNullOrEmpty(q.Name5))
  {

            
            #line default
            #line hidden
WriteLiteral("    <tr>\r\n");

            
            #line 84 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
      
            
            #line default
            #line hidden
            
            #line 84 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
       if (q.Sign5 == null)
      {

            
            #line default
            #line hidden
WriteLiteral("        <th>Требует согласования</th>\r\n");

WriteLiteral("        <td>");

            
            #line 87 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
       Write(q.Name5);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n");

WriteLiteral("        <td>");

            
            #line 88 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
       Write(q.UserName5);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n");

WriteLiteral("        <td>&nbsp;</td>\r\n");

            
            #line 90 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
      }
      else
      {

            
            #line default
            #line hidden
WriteLiteral("        <th>Согласовано</th>\r\n");

WriteLiteral("        <td>");

            
            #line 94 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
       Write(q.Name5);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n");

WriteLiteral("        <td>");

            
            #line 95 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
       Write(q.UserName5);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n");

WriteLiteral("        <td>");

            
            #line 96 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
       Write(q.Sign5.ToString("dd.MM.yyyy HH:mm"));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n");

            
            #line 97 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
      }

            
            #line default
            #line hidden
WriteLiteral("    </tr>\r\n");

            
            #line 99 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
      if (!string.IsNullOrEmpty(q.Comment5))
      {

            
            #line default
            #line hidden
WriteLiteral("    <tr>\r\n      <th>Примечание</th>\r\n      <td");

WriteLiteral(" colspan=\"3\"");

WriteLiteral(">");

            
            #line 103 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
                 Write(q.Comment5);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n    </tr>\r\n");

            
            #line 105 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
      }

  }

            
            #line default
            #line hidden
WriteLiteral("</table>\r\n<p");

WriteLiteral(" style=\"color: white; font-style: italic\"");

WriteLiteral(">");

            
            #line 109 "..\..\Areas\Code\Views\Leave\SendFinal.cshtml"
                                       Write(email);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

        }
    }
}
#pragma warning restore 1591