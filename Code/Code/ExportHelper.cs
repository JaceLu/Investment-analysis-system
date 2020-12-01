
using Sail.Common;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;

namespace Investment
{
    public static class ExportHelper
    {
        //
        // 摘要:
        //     /// 通过字符串生成文件流 ///
        //
        // 参数:
        //   s:
        public static Stream GenerateStreamFromString(this string s)
        {
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
            streamWriter.Write(s);
            streamWriter.Flush();
            memoryStream.Position = 0L;
            return memoryStream;
        }

        //
        // 摘要:
        //     /// 添加单元格 ///
        //
        // 参数:
        //   sb:
        //
        //   text:
        public static void AppendColumn(this StringBuilder sb, object text, string cssClass = "")
        {
            sb.AppendFormat($"<td class='{cssClass}'>{text}</td>");
        }

        //
        // 摘要:
        //     /// 按照指定格式导出文件到excel ///
        //
        // 参数:
        //   fileName:
        //     文件名（不含扩展名）
        //
        //   titles:
        //     标题列表
        //
        //   caption:
        //     表格标题
        //
        //   act:
        //     组织表格体的方法
        //
        //   styleSheet:
        //     附加的样式表,不包含style标签
        public static (string name,string content) ExportToExcel(string fileName, string caption, IEnumerable<string> titles, Action<StringBuilder> act, string styleSheet = null)
        {
          //  HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            StringBuilder str = new StringBuilder();
            if (styleSheet.IsNotNull())
            {
                str.AppendLine("<style>" + styleSheet + "</style>");
            }
            str.AppendLine("<table  border='1'>");
            if (caption.IsNotNull())
            {
                str.AppendLine("<caption>" + caption + "</caption>");
            }
            str.AppendLine("<thead><tr>");
            titles.ForEach(delegate (string s)
            {
                str.AppendLine("<td>" + s + "</td>");
            });
            str.AppendLine("</tr></thead><tbody>");
            act(str);
            str.AppendLine("</tbody></table>");

            return (fileName + ".xls", str.ToString());
        }

        //
        // 摘要:
        //     /// 导出 ///
        //
        // 参数:
        //   name:
        //
        //   expression:
        //
        //   classExp:
        //
        //   caption:
        //
        //   page:
        //
        //   style:
        //     附加样式表
        //
        // 类型参数:
        //   T:
        public static (string name, string content) Export<T>(string name, string caption, Func<int, int, DataContext, PageResult> page, Expression<Func<T, object>> expression, Expression<Func<object>> classExp = null, string style = null)
        {
            NewExpression exp = expression.Body as NewExpression;
            NewExpression classexp = classExp?.Body as NewExpression;
            if (exp == null)
            {
                throw new SailCommonException("无效表达式");
            }
            IEnumerable<string> titles = from x in exp.Members
                                         select x.Name;
            DataContext db = new DataContext();
            try
            {
                int pageSize = 200;
                return ExportToExcel(name, caption, titles, delegate (StringBuilder sb)
                {
                    int num = 0;
                    int num2 = 1;
                    while (num < num2)
                    {
                        PageResult pageResult = page(++num, pageSize, db);
                        pageResult.Data.Cast<T>().ForEach(delegate (T s)
                        {
                            sb.Append("<tr>");
                            object data = expression.Compile()(s);
                            object classData = classExp?.Compile()?.Invoke();
                            exp.Members.ForEach(delegate (MemberInfo mm)
                            {
                                MemberInfo memberInfo = classexp?.Members?.Find((MemberInfo mmm) => mmm.Name == mm.Name);
                                string cssClass = "";
                                if (memberInfo.IsNotNull() && classData.IsNotNull())
                                {
                                    cssClass = classData.GetValue(memberInfo.Name).ToString();
                                }
                                sb.AppendColumn(data.GetValue(mm.Name), cssClass);
                            });
                            sb.Append("</tr>");
                        });
                        num2 = pageResult.PageInfo.PageCount;
                    }
                }, style);
            }
            finally
            {
                if (db != null)
                {
                    ((IDisposable)db).Dispose();
                }
            }
        }
    }

}
