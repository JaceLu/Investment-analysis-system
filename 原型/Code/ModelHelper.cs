using System;
using System.Collections.Generic;
using Sail.Common;
using System.Linq;

namespace Lamtip.Code
{
    public static class ModelHelper
    {
        public static List<PropHistory> GetDiff(this IModel newModel, IModel oldModel)
        {
            var type = newModel.GetType();
            if (type != oldModel.GetType()) throw new Exception("");
            return type.GetHTable().Columns.Select(col =>
                new PropHistory
                {
                    Name = col.Remark,
                    Porperty = col.Property,
                    OldValue = oldModel.GetValue(col.Property).ToString(),
                    NewValue = newModel.GetValue(col.Property).ToString()
                }
             )
             .Where(x => x.OldValue != x.NewValue).ToList();
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="newModel"></param>
        ///// <param name="oldModel"></param>
        ///// <returns></returns>
        //public static List<PropHistory> GeneratePropHistory(this IModel oldModel, IModel newModel)
        //{
        //    var type = newModel.GetType();
        //    if (type != oldModel.GetType())
        //        throw new ArgumentException("require same type", nameof(newModel));

        //    return type.GetProperties()
        //        .Where(v =>
        //        {
        //            var hColumn = v.GetAttribute<HColumnAttribute>();
        //            var historyProp = v.GetAttribute<IgnoreHistoryAttribute>();
        //            return historyProp != null && hColumn != null;
        //        })
        //        .Select(v =>
        //        new PropHistory
        //        {
        //            Name = type.GetSummary(v.Name),
        //            Porperty = v.Name,
        //            OldValue = oldModel.GetValue(v.Name).ToString(),
        //            NewValue = newModel.GetValue(v.Name).ToString()
        //        }
        //     )
        //     .Where(x => x.OldValue != x.NewValue)
        //     .ToList();
        //}
    }
}
