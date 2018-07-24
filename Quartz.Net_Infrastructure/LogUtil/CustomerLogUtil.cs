using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Quartz.Net_Infrastructure.LogUtil
{
    public class CustomerLogUtil
    {

        //TODOThink实现动态切换ILog   当添加ILog实例后 如果做到调用端不需要增加key
        #region 新版代码
        static readonly ConcurrentDictionary<string, ILog> ConnectionLogCache = new ConcurrentDictionary<string, ILog>(new List<KeyValuePair<string, ILog>> { new KeyValuePair<string, ILog>(Log4NetKeys.Log4netWebInfoKey, LogManager.GetLogger(Log4NetKeys.Log4netWebInfoKey)), new KeyValuePair<string, ILog>(Log4NetKeys.Log4netWebErrorKey, LogManager.GetLogger(Log4NetKeys.Log4netWebErrorKey)), new KeyValuePair<string, ILog>(Log4NetKeys.Log4netJobInfoKey, LogManager.GetLogger(Log4NetKeys.Log4netJobInfoKey)), new KeyValuePair<string, ILog>(Log4NetKeys.Log4netJobErrorKey, LogManager.GetLogger(Log4NetKeys.Log4netJobErrorKey)), new KeyValuePair<string, ILog>(Log4NetKeys.Log4netSchedulerInfoKey, LogManager.GetLogger(Log4NetKeys.Log4netSchedulerInfoKey)), new KeyValuePair<string, ILog>(Log4NetKeys.Log4netSchedulerErrorKey, LogManager.GetLogger(Log4NetKeys.Log4netSchedulerErrorKey)), new KeyValuePair<string, ILog>(Log4NetKeys.Log4netMailInfoKey, LogManager.GetLogger(Log4NetKeys.Log4netMailInfoKey)), new KeyValuePair<string, ILog>(Log4NetKeys.Log4netMailErrorKey, LogManager.GetLogger(Log4NetKeys.Log4netMailErrorKey)) });


        public static void Error(string log4netKey, string errorMsg, Exception ex = null)
        {
            
            var _logEror = ConnectionLogCache[log4netKey];
            if (ex != null)
            {
               
                _logEror.Error(errorMsg, ex);
            }
            else
            {
                _logEror.Error(errorMsg);
            }
        }
        public static void Info(string log4netKey, string msg,Exception ex=null)
        {
            if (ex != null) { return; }
            var _logInfo = ConnectionLogCache[log4netKey];
            _logInfo.Info(msg);
        }
        #endregion

        #region 第一版代码
        //static readonly ILog _logWebInfo = LogManager.GetLogger("logWebInfo");
        //static readonly ILog _logWebError = LogManager.GetLogger("logWebError");
        //static readonly ILog _logJobInfo = LogManager.GetLogger("logJobInfo");
        //static readonly ILog _logJobError = LogManager.GetLogger("logJobError");
        //static readonly ILog _logSchedulerInfo = LogManager.GetLogger("logSchedulerlInfo");
        //static readonly ILog _logSchedulerError = LogManager.GetLogger("logSchedulerError");
        //    public static void Error(string errorMsg,Exception ex=null) {
        //        if (ex != null)
        //        {
        //            _logWebError.Error(errorMsg, ex);
        //        }
        //        else {
        //            _logWebError.Error(errorMsg);
        //        }

        //    }
        //    public static void Info(string msg) {
        //        _logWebInfo.Info(msg);
        //    }
        //    public static void SchedulerInfo(string msg) {
        //        _logSchedulerInfo.Info(msg);
        //    }
        //    public static void SchedulerError(string errorMsg, Exception ex = null) {
        //        if (ex != null)
        //        {
        //            _logSchedulerError.Error(errorMsg, ex);
        //        }
        //        else {
        //            _logSchedulerError.Error(errorMsg);
        //        }
        //    }
        //    public static void JobError(string errorMsg, Exception ex = null) {
        //        if (ex != null)
        //        {
        //            _logJobError.Error(errorMsg, ex);
        //        }
        //        else {

        //        }
        //    }
        //    public static void JobInfo(string msg) {
        //        _logJobInfo.Info(msg);
        //    }
        //}
        #endregion
    }
}
