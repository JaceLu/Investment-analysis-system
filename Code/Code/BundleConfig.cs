using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebOptimizer;

namespace Investment.Code
{
    public static class BundleConfig
    {
        /// <summary>
        ///  注册管理端样式
        /// </summary>
        /// <param name="pipeline"></param>
        public static IAssetPipeline RegisterMainCss(this IAssetPipeline pipeline)
        {
            pipeline.AddCssBundle("/Content/manage.min.css",
                                                    "Content/css/zqui.css",
                                                    "Content/css/tooltip.css",
                                                    "Content/css/print.css",
                                                    "Content/css/stockcontrol.css",
                                                    "Content/css/select2.min.css",
                                                    "Content/css/WdatePicker.css",
                                                    "Content/css/wangEditor.css",
                                                    "Content/fonts/font-awesome.min.css"
                                                    );
            return pipeline;
        }


        /// <summary>
        /// 注册管理端js
        /// </summary>
        /// <param name="pipeline"></param>
        public static IAssetPipeline RegisterMainJs(this IAssetPipeline pipeline)
        {
            pipeline.AddJavaScriptBundle("/Scripts/Sites.js",
                                                "/Scripts/Base/bootstrap.min.js",
                                                "/Scripts/Base/jPlugin.js",
                                                "/Scripts/Base/jquery.min.js",
                                                "/Scripts/Base/jquery.pin.min.js",
                                                "/Scripts/Base/jsviews.js",
                                                "/Scripts/Base/layout.min.js",
                                                "/Scripts/Base/lodash.js",
                                                "/Scripts/Base/select2.min.js",
                                                "/Scripts/Base/stickUp.min.js",
                                                "/Scripts/Base/swiper.animate1.0.3.min.js",
                                                "/Scripts/Base/swiper.min.js",
                                                "/Scripts/other/wangEditor.js",
                                                "/Scripts/other/highCharts.js",
                                                "/Scripts/other/exporting.js",
                                                "/Scripts/select2/select2Ext.js",
                                                "/Scripts/customs/Extends.js");
            return pipeline;
        }
    }
}
