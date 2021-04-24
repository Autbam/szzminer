using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace szzminer.Tools
{
    class sendMail
    {
        /// <summary>
        /// C#发送邮件函数
        /// </summary>
        /// <param name="from">发送者邮箱</param>
        /// <param name="fromer">发送人</param>
        /// <param name="to">接受者邮箱</param>
        /// <param name="toer">收件人</param>
        /// <param name="Subject">主题</param>
        /// <param name="Body">内容</param>
        /// <param name="file">附件</param>
        /// <param name="SMTPHost">smtp服务器</param>
        /// <param name="SMTPuser">邮箱</param>
        /// <param name="SMTPpass">密码</param>
        /// <returns></returns>
        public static bool sendmail(string sfrom, string sfromer, string sto, string stoer, string sSubject, string sBody, string sfile, string sSMTPHost, string sSMTPuser, string sSMTPpass)
        {
            ////设置from和to地址
            MailAddress from = new MailAddress(sfrom, sfromer);
            MailAddress to = new MailAddress(sto, stoer);

            ////创建一个MailMessage对象
            MailMessage oMail = new MailMessage(from, to);

            //// 添加附件
            if (sfile != "")
            {
                oMail.Attachments.Add(new Attachment(sfile));
            }

            ////邮件标题
            oMail.Subject = sSubject;


            ////邮件内容
            oMail.Body = sBody;

            ////邮件格式
            oMail.IsBodyHtml = false;

            ////邮件采用的编码
            oMail.BodyEncoding = System.Text.Encoding.GetEncoding("GB2312");

            ////设置邮件的优先级为高
            oMail.Priority = MailPriority.High;

            ////发送邮件
            SmtpClient client = new SmtpClient();
            ////client.UseDefaultCredentials = false;
            client.Host = sSMTPHost;
            //企业邮箱需设置端口，个人邮箱不需要
            client.Port = 25;
            client.Credentials = new NetworkCredential(sSMTPuser, sSMTPpass);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            try
            {
                client.Send(oMail);
                return true;
            }
            catch (Exception err)
            {
                //Response.Write(err.Message.ToString());
                return false;
            }
            finally
            {
                ////释放资源
                oMail.Dispose();
            }

        }
    }
}
