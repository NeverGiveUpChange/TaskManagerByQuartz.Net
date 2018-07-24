using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobManagerByQuartz.Models
{
    public class AjaxResponseData
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public string StausCode { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public object Data { get; set; }
    }
}