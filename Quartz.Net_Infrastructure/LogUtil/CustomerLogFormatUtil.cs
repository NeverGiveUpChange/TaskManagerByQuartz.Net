using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.Net_Infrastructure.LogUtil
{
    public class CustomerLogFormatUtil
    {
        public static string LogWebMsgFormat(string uniqueID, string requestUrl, string httpMethod, string parameters="",string reponseData="")
        {

            return $@"唯一编号：{uniqueID}
                                请求地址：{requestUrl}
                                请求方式：{httpMethod}
                                请求参数：{parameters}
                                返回内容：{reponseData}";
                        
        }
        public static string LogJobMsgFormat(string jobName, int jobState, string operateType)
        {
            return $@"任务名称：{jobName}
                                调用状态：{"完成"}
                                操作类型：{operateType}
                                操作状态：{jobState}";
        }

        public static string LogSchedulerMsgFormat(string quartzServerIp,string quartzServerName,string operateType,string operateState) {

            return $@"服务器IP：{quartzServerIp}
                                服务名称：{quartzServerName}
                                操作类型：{operateType}
                                操作状态：{operateState}";
        }
        public static string LogMailMsgfFormat(string  fromMailAddress,string toMailAddress,string ccMailAddress,string subject,string body,string sendState) {
            return $@"发送邮箱：{fromMailAddress}
                                接受邮箱：{toMailAddress}
                                抄送邮箱：{ccMailAddress}
                                主题：{subject}
                                内容：{body}
                                发送状态：{sendState}";

        }
    }
}
