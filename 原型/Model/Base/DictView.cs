using Sail.Common;

namespace Lamtip.Model
{
    /// <summary>
    /// 字典视图
    /// </summary>
    [HTable(ViewType = typeof(Dict))]
    public class DictView : IModel
    {
        /// <summary>
        /// 字典id
        /// </summary>
        [HColumn(IsPrimary = true, IsGuid = true)]
        public string Id { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [HColumn(Length = 50, IsUniqueness = true, Remark = "名称")]
        public string Name { set; get; }

        ///// <summary>
        ///// 字典类型
        ///// </summary>
        //[HColumn(IsNotNeedUniqueness = true)]
        //public DictType Type { set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name?.ToString();
        }
    }


}