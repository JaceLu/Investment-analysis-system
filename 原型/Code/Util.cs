using Sail.Common;
using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace Lamtip
{

  
    /// <summary>
    /// 
    /// </summary>
    public class Util
    {
        private Util()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        public static async Task<bool> RunTran(DataContext db, Func<DataContext, Task> act)
        {
            bool result = false;
            try
            {
                db.BeginTran();
                await act(db);
                db.Commit();
                result = true;
            }
            catch (Exception)
            {
                db.Rollback();
                throw;
            }
            finally
            {
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task<AjaxResult> DBFuncAsync(Func<DataContext, Task> action, string message = "操作成功")
        {
            using (var db = new DataContext())
            {
                var res = new AjaxResult
                {
                    Msg = message
                };
                try
                {
                    await action(db);
                    res.IsSuccess = true;
                    return res;
                }
                catch (Exception e)
                {
                    res.Exception = e.ToString();
                    res.Msg = e.Message;
                    return res;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task<AjaxResult> DBFuncAsync(Func<DataContext, Task<object>> action, string message = "操作成功")
        {
            using (var db = new DataContext())
            {
                var res = new AjaxResult
                {
                    Msg = message
                };
                try
                {
                    res.Data = await action(db);
                    res.IsSuccess = true;
                    return res;
                }
                catch (Exception e)
                {
                    res.Exception = e.ToString();
                    res.Msg = e.Message;
                    return res;
                }
            }
        }
    }
}
