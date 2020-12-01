using Sail.Common;

namespace Investment.Models.Abstract
{
    /// <summary>
    /// 字典视图
    /// </summary>
    [HTable(ViewType = typeof(Dict))]
    public class DictView : IModel
    {
        /// <summary>
        /// 字典Id
        /// </summary>
        [HColumn(IsPrimary = true, IsGuid = true)]
        public string Id { get; set; }

        /// <summary>
        /// 字典名称
        /// </summary>
        [HColumn(Length = 50, IsUniqueness = true, Remark = "名称")]
        public string Name { get; set; }

        /// <summary>
        /// 字典类型
        /// </summary>
        [HColumn(IsNotNeedUniqueness = true)]
        public DictType Type { get; set; }
    }
}
