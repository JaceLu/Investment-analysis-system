using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Sail.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Investment.Code
{
    /// <summary>
    /// 
    /// </summary>
    public static class Util
    {



        /// <summary>
        /// read excel from filePath
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="sheetIndex"></param>
        /// <returns></returns>
        public static IEnumerable<DataRow> ReadExcel(this string filePath, int sheetIndex = 0)
        {
            IWorkbook wk;
            var extension = Path.GetExtension(filePath);
            using (var fs = File.OpenRead(filePath))
            {
                if (extension.Equals(".xls"))
                {
                    //把xls文件中的数据写入wk中
                    wk = new HSSFWorkbook(fs);
                }
                else
                {
                    //把xlsx文件中的数据写入wk中
                    wk = new XSSFWorkbook(fs);
                }
            }

            var sheet = wk.GetSheetAt(0);

            var dt = new DataTable(sheet.SheetName);

            // write header row
            var headerRow = sheet.GetRow(sheetIndex);
            foreach (var headerCell in headerRow)
            {
                dt.Columns.Add(headerCell.ToString());
            }

            // write the rest
            var rowIndex = 0;
            foreach (IRow row in sheet)
            {
                if (rowIndex++ == 0) continue;
                var dataRow = dt.NewRow();
                dataRow.ItemArray = Enumerable.Range(0, headerRow.Count())
                    .Select(i => row.GetCell(i)?.ToString() ?? "").ToArray();
                dt.Rows.Add(dataRow);
            }

            return dt.Rows.Cast<DataRow>();
        }

        /// <summary>
        /// convert dataRow to dictionary
        /// </summary>
        /// <param name="dataRows"></param>
        /// <returns></returns>
        public static Dictionary<string, IEnumerable<string>> ExcelToDict(this IEnumerable<DataRow> dataRows)
        {
            var result = new Dictionary<string, IEnumerable<string>>();
            if (dataRows.IsNullOrEmpty())
            {
                return result;
            }
            var headers = dataRows.First().Table.Columns.GetHeader<string>().ToList();

            var index = 0;
            dataRows.Select(x => x.ItemArray.Cast<string>().ToList()).ForEach(x =>
            {
                result.Add(headers[index], x);
                index++;
            });
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetHeader<T>(this DataColumnCollection column) where T : class
        {
            var result = new List<object>();
            for (int i = 0; i < column.Count; i++)
            {
                result.Add(column[i].ColumnName);
            }

            return result.Cast<T>();
        }


        /// <summary>
        /// convert obj to dict
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="expression"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="SailCommonException"></exception>
        public static Dictionary<string, object> ToDictionary<T>(this T obj, Expression<Func<T, object>> expression)
        {
            var payload = new Dictionary<string, object>();
            var exp = expression.Body as NewExpression;
            if (exp == null) throw new SailCommonException("无效表达式");
            exp.Members.ForEach(mm => { payload.Add(mm.Name, obj.GetValue(mm.Name)); });
            return payload;
        }

        /// <summary>
        /// convert dict to obj
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="expression"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="SailCommonException"></exception>
        public static T ToObject<T>(this Dictionary<string, object> data) where T : class
        {
            var type = typeof(T);
            var instance = Activator.CreateInstance(type);
            var members = type
                .GetMembers()
                .Where(x => x is PropertyInfo)
                .Cast<PropertyInfo>()
                .ToDictionaryWith(x => KeyValuePair.Create(x.Name, x));
            data.ForEach(x =>
            {
                if (members.ContainsKey(x.Key))
                {
                    instance.SetValue(x.Key, x.Value);
                }

            });
            return instance as T;
        }

        /// <summary>
        /// convert obj to dict
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="expression"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="SailCommonException"></exception>
        public static Dictionary<K, V> ToDictionaryWith<T, K, V>(this IEnumerable<T> obj, Func<T, KeyValuePair<K, V>> expression)
        {
            var result = new Dictionary<K, V>();
            if (obj.IsNullOrEmpty())
            {
                return result;
            }

            foreach (var v in obj)
            {
                var kV = expression.Invoke(v);
                result.Add(kV.Key, kV.Value);
            }

            return result;
        }

    }


}
