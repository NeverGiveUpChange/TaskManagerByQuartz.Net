
using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;

using System.Web;
using System.Web.Mvc;

namespace JobManagerByQuartz.CustomerFilters
{
  
    public class CustomerLogAtrribute : ActionFilterAttribute, IExceptionFilter
    {
        //const string log4netInfoKey = "logWebInfo";
        //const string log4netErrorKey = "logWebError";
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var uniqueID = Guid.NewGuid().ToString("N");
            //var requestMethodParameters = _getRequestMethodParameters(filterContext);
            //CustomerLogUtil.Info(Log4NetKeys.Log4netWebInfoKey, CustomerLogFormatUtil.LogWebMsgFormat(uniqueID, filterContext.HttpContext.Request.Url.ToString(), requestMethodParameters["httpMethod"], requestMethodParameters["requestParams"]));
            //filterContext.Controller.ViewData["ID"] = uniqueID;
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Result is JsonResult)
            {
                //var uniqueID = filterContext.Controller.ViewData["ID"] as string;
                //var jsonResult = filterContext.Result as JsonResult;
                //CustomerLogUtil.Info(Log4NetKeys.Log4netWebInfoKey, CustomerLogFormatUtil.LogWebMsgFormat(uniqueID, filterContext.HttpContext.Request.Url.ToString(), filterContext.HttpContext.Request.HttpMethod, reponseData: JsonConvert.SerializeObject(jsonResult.Data)));
            }
            base.OnActionExecuted(filterContext);
        }
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled) return;
            var exception = filterContext.Exception;
            _logError(exception, filterContext);
            //RedirectUrl(exception, filterContext);
        }
        private void _redirectUrl(Exception exception, ExceptionContext filterContext)
        {
            var httpException = new HttpException(null, exception);

            var response = filterContext.HttpContext.Response;

            if (httpException != null)
            {
                var httpCode = httpException.GetHttpCode();
                if (httpCode == 400)
                {
                    response.StatusCode = 400;
                    response.Redirect("/Error/Error_400");
                }
                else if (httpCode == 404)
                {
                    response.StatusCode = 404;
                    response.Redirect("/Error/Error_404");
                }
                else
                {
                    response.StatusCode = 500;
                    response.Redirect("/Error/Error_500");
                }
            }

        }
        private void _logError(Exception exception, ExceptionContext filterContext)
        {
            //var uniqueID = filterContext.Controller.ViewData["ID"] as string;
            //var url = filterContext.RequestContext.HttpContext.Request.Url.ToString();
            //var requestMethodParameters = _getRequestMethodParameters(filterContext);
            //filterContext.ExceptionHandled = true;
            //filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            //CustomerLogUtil.Error(Log4NetKeys.Log4netWebErrorKey, CustomerLogFormatUtil.LogWebMsgFormat(uniqueID, url, requestMethodParameters["httpMethod"], requestMethodParameters["requestParams"]), exception);

        }
        private Dictionary<string, string> _getRequestMethodParameters(ControllerContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var httpMethod = request.HttpMethod;
            var requestData = string.Empty;
            if (httpMethod.ToUpper() == "GET")
            {
                NameValueCollection form = request.QueryString;
                var dicParams = new Dictionary<string, string>();
                for (int i = 0; i < form.Count; i++)
                {
                    var key = form.Keys[i];
                    dicParams.Add(key, form[key]);
                }
                requestData = string.Join("&", dicParams.Select(x => x.Key + "=" + HttpUtility.UrlDecode( x.Value.Trim(), filterContext.HttpContext.Request.ContentEncoding)).ToArray());
            }
            else if (httpMethod.ToUpper() == "POST")
            {
                Stream sm = filterContext.HttpContext.Request.InputStream;
                StreamReader streamReader = new StreamReader(sm);
                requestData = streamReader.ReadToEnd();
                HttpContext.Current.Request.InputStream.Position = 0;
            }

            return new Dictionary<string, string> { { "httpMethod", httpMethod }, { "requestParams", HttpUtility.UrlDecode(requestData, filterContext.HttpContext.Request.ContentEncoding) } };

        }

    }
}