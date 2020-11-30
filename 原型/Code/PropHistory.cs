namespace Lamtip.Code
{
    /// <summary>
    /// 属性变动记录
    /// </summary>
    public class PropHistory
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string Porperty { set; get; }
        /// <summary>
        /// 旧值
        /// </summary>
        public string OldValue { set; get; }
        /// <summary>
        /// 新值
        /// </summary>
        public string NewValue { set; get; }
    }
}
