using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Mod.Common
{
    public class EmailHelper
    {
        ///  <summary>
        ///  发送邮件程序调用方法 SendMail("abc@126.com", "某某人", "cba@126.com", "你好", "我测试下邮件", "邮箱登录名", "邮箱密码", "smtp.126.com", true,);
        ///  </summary>
        ///  <param name="from">发送人邮件地址</param>
        ///  <param name="fromname">发送人显示名称</param>
        ///  <param name="to">发送给谁（邮件地址）</param>
        ///  <param name="subject">标题</param>
        ///  <param name="body">内容</param>
        ///  <param name="username">邮件登录名</param>
        ///  <param name="password">邮件密码</param>
        ///  <param name="server">邮件服务器 smtp服务器地址</param>
        /// <param name="isHtml">是否是HTML格式的邮件 </param>
        /// <returns>send ok</returns>
        public static bool SendMail(string from, string fromname, string to, string subject, string body, string server, string username, string password, bool isHtml)
        {
            //邮件发送类
            MailMessage mail = new MailMessage();
            try
            {
                //是谁发送的邮件
                mail.From = new MailAddress(from, fromname);
                //发送给谁
                mail.To.Add(to);
                //标题
                mail.Subject = subject;
                //内容编码
                mail.BodyEncoding = Encoding.Default;
                //发送优先级
                mail.Priority = MailPriority.High;
                //邮件内容
                mail.Body = body;
                //是否HTML形式发送
                mail.IsBodyHtml = isHtml;
                //邮件服务器和端口
                var smtp = new SmtpClient(server, 25)
                {
                    UseDefaultCredentials = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };
                //指定发送方式
                //发件人身份验证,否则163   发不了
                smtp.UseDefaultCredentials = true;
                //指定登录名和密码
                smtp.Credentials = new System.Net.NetworkCredential(username, password);
                //超时时间
                smtp.EnableSsl = false;
                smtp.Timeout = 10000;
                smtp.Send(mail);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            finally
            {
                mail.Dispose();
            }
        }

        //读取指定URL地址的HTML，用来以后发送网页用
        public static string ScreenScrapeHtml(string url)
        {
            //读取stream并且对于中文页面防止乱码
            StreamReader reader = new StreamReader(System.Net.WebRequest.Create(url).GetResponse().GetResponseStream(), System.Text.Encoding.UTF8);
            string str = reader.ReadToEnd();
            reader.Close();
            return str;
        }

        //发送plaintxt
        public static bool SendText(string from, string fromname, string to, string subject, string body, string server, string username, string password)
        {
            return SendMail(from, fromname, to, subject, body, server, username, password, false);
        }

        //发送HTML内容
        public static bool SendHtml(string from, string fromname, string to, string subject, string body, string server, string username, string password)
        {
            return SendMail(from, fromname, to, subject, body, server, username, password, true);
        }

        //发送制定网页
        public static bool SendWebUrl(string from, string fromname, string to, string subject, string server, string username, string password, string url)
        {
            //发送制定网页
            return SendHtml(from, fromname, to, subject, ScreenScrapeHtml(url), server, username, password);

        }
        //默认发送格式
        public static bool SendEmailDefault( string toEmail, string userId)
        {
            StringBuilder MailContent = new StringBuilder();
            MailContent.Append("亲爱的飞驶特员工：<br/>");
            MailContent.Append("&nbsp;&nbsp;&nbsp;&nbsp;您好！你于");
            MailContent.Append(DateTime.Now.ToLongTimeString());
            MailContent.Append("通过<a href='#'>飞驶特</a>管理系统申请找回密码。<br/>");
            MailContent.Append("&nbsp;&nbsp;&nbsp;&nbsp;为了安全起见，请用户点击以下链接重设个人密码：<br/><br/>");
            string url = "http://localhost:1424/Member/User/ResetPassword?userId=" + userId+"01F";
            MailContent.Append("<a href='" + url + "'>" + url + "</a><br/><br/>");
            MailContent.Append("&nbsp;&nbsp;&nbsp;&nbsp;(如果无法点击该URL链接地址，请将它复制并粘帖到浏览器的地址输入框，然后单击回车即可。)");
            return SendHtml("z8414@qq.com", "飞驶特员工管理中心", toEmail, "飞驶特找回密码", MailContent.ToString(), "smtp.qq.com", "z8414@qq.com", "123qwe");
        }
    }
}

//        接收邮箱返回数据并重置密码：

//    string  memeberid = "";//用来接收找回密码时返回的email地址,与会员编号
//        protected void Page_Load(object sender, EventArgs e)
//        {
//            if (Request.QueryString["email"] != null)
//            {
//                email = Request.QueryString["email"].ToString();
//            }
//            if (Request.QueryString["userId"] != null)
//            {
//                memeberid = Request.QueryString["userId"].ToString();
//                modelMember = bllMember.GetModel(long.Parse(memeberid));
//                modelMember.PassWord = TianDao.Common.DEncrypt.DESEncrypt.Encrypt("123456");
//                if (modelMember.Email == email)//验证邮箱地址是否为用户注册时候的邮箱地址 
//                {
//                    bllMember.Update(modelMember);
//                    Response.Write("<script>alert('您的密码已重置，默认密码为123456,请尽快修改密码！');window.location.href='../login.aspx';</script>");
//                }
//            }
//            if (!IsPostBack)
//            {

//            }
//        }
//    }
//}



