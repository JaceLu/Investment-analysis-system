using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investment.Code.TagHelpers
{
    /// <summary>
    /// 
    /// </summary>
    public class HtmlElement
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string Class { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string InnertHtml { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public HtmlElement()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="class"></param>
        /// <param name="name"></param>
        /// <param name="innerHtml"></param>
        public HtmlElement(string id, string @class, string name, string innerHtml)
        {
            this.Id = id;
            this.Class = @class;
            this.Name = name;
            this.InnertHtml = innerHtml;
        }

        public HtmlElement(string @class, string innerHtml) : this("", @class, "", innerHtml)
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal string ToAttributesString()
        {
            var build = new StringBuilder();
            if (!string.IsNullOrEmpty(this.Id))
                build.AppendFormat("id='{0}'", this.Id);
            if (!string.IsNullOrEmpty(this.Class))
                build.AppendFormat("class='{0}'", this.Class);
            if (!string.IsNullOrEmpty(this.Name))
                build.AppendFormat("name='{0}'", this.Name);
            return build.ToString();
        }
    }
}
