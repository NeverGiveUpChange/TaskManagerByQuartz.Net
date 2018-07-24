using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.Net_Infrastructure.MailUtil
{
    public class MailMessageConfigurationInfo
    {
        public static string FromMailAddress { get { return SmtpClientConfigurationInfo.UserName; } private set { } }
        public static Encoding BodyEncoding { get { return Encoding.UTF8; }private set { } }
        public static bool  IsBodyHtml { get { return true; }private  set { } }
        public static MailPriority Priority { get { return MailPriority.High; } private  set { } }

        private  List<string> _toMailAddressList = new List<string>();
        public  List<string> ToMailAddressList { get { return _toMailAddressList; } set { _toMailAddressList = value; } }
        private  List<string> _ccMailAddressList = new List<string>();
        public  List<string> CCMailAddressList { get { return _ccMailAddressList; } set { _ccMailAddressList = value; } }

        public  string Subject { get; set; }
        public  string Body { get; set; }
    }
}
