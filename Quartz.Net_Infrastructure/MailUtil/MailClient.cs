
using log4net;
using Quartz.Net_Infrastructure.LogUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Quartz.Net_Infrastructure.MailUtil
{
    public class MailClient
    {

        //const string log4netMailInfoKey = "logMailInfo";
        //const string log4netMailErrorKey = "logMailError";
        private MailMessageConfigurationInfo mailMessageInfo = null;
        private string sendState = string.Empty;
       



        public void SendMail(MailMessageConfigurationInfo mailMessageConfigurationInfo)
        {
            try
            {

                mailMessageInfo = mailMessageConfigurationInfo;
                var smtpClient = _configureSmtpClient();
                var mailMessage = _configureMailMessage();

                smtpClient.SendCompleted += new SendCompletedEventHandler(_stmp_SendCompleted);
                 smtpClient.SendAsync(mailMessage, "ok");
            }
            catch (Exception ex)
            {
                sendState = "异步发送邮件失败";
                CustomerLogUtil.Error(Log4NetKeys.Log4netMailErrorKey, CustomerLogFormatUtil.LogMailMsgfFormat(SmtpClientConfigurationInfo.UserName, string.Join(",", mailMessageInfo.ToMailAddressList), string.Join(",", mailMessageInfo.CCMailAddressList), mailMessageInfo.Subject, mailMessageInfo.Body, sendState), ex);
            }
        }
        private void _stmp_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            string sendState = string.Empty;
            Exception ex = null;
            if (e.Cancelled)
            {
                sendState = "异步发送邮件取消";
                CustomerLogUtil.Error(Log4NetKeys.Log4netMailErrorKey, CustomerLogFormatUtil.LogMailMsgfFormat(SmtpClientConfigurationInfo.UserName, string.Join(",", mailMessageInfo.ToMailAddressList), string.Join(",", mailMessageInfo.CCMailAddressList), mailMessageInfo.Subject, mailMessageInfo.Body, sendState));
            }
            else if (e.Error != null)
            {
                sendState = "异步发送邮件失败";
                ex = e.Error;
                CustomerLogUtil.Error(Log4NetKeys.Log4netMailErrorKey, CustomerLogFormatUtil.LogMailMsgfFormat(SmtpClientConfigurationInfo.UserName, string.Join(",", mailMessageInfo.ToMailAddressList), string.Join(",", mailMessageInfo.CCMailAddressList), mailMessageInfo.Subject, mailMessageInfo.Body, sendState), e.Error);
            }
            else
            {
                sendState = "异步发送邮件成功";
                CustomerLogUtil.Info(Log4NetKeys.Log4netMailInfoKey, CustomerLogFormatUtil.LogMailMsgfFormat(SmtpClientConfigurationInfo.UserName, string.Join(",", mailMessageInfo.ToMailAddressList), string.Join(",", mailMessageInfo.CCMailAddressList), mailMessageInfo.Subject, mailMessageInfo.Body, sendState));
            }
        }
        private SmtpClient _configureSmtpClient()
        {

            var smtpClient = new SmtpClient();
            smtpClient.DeliveryMethod = SmtpClientConfigurationInfo.SmtpDeliveryMethod;
            smtpClient.Host = SmtpClientConfigurationInfo.Host;
            smtpClient.UseDefaultCredentials = SmtpClientConfigurationInfo.UseDefaultCredentials;
            smtpClient.Credentials = new System.Net.NetworkCredential(SmtpClientConfigurationInfo.UserName, SmtpClientConfigurationInfo.PassWord);
            return smtpClient;
        }
        private MailMessage _configureMailMessage()
        {
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(MailMessageConfigurationInfo.FromMailAddress);
            mailMessageInfo.ToMailAddressList.ForEach(x => mailMessage.To.Add(x));
            mailMessageInfo.CCMailAddressList.ForEach(x => mailMessage.CC.Add(x));
            mailMessage.Subject = mailMessageInfo.Subject;
            mailMessage.Body = mailMessageInfo.Body;
            mailMessage.BodyEncoding = MailMessageConfigurationInfo.BodyEncoding;
            mailMessage.IsBodyHtml = MailMessageConfigurationInfo.IsBodyHtml;
            mailMessage.Priority = MailMessageConfigurationInfo.Priority;
            return mailMessage;


        }

    }
}
