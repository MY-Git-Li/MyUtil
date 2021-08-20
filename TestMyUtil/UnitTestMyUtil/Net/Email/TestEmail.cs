using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MyUtil.Net;
namespace UnitTestMyUtil.Net.Email
{
    [TestClass]
    public class TestEmail
    {
        [TestMethod]
        public void Run()
        {
            new MyUtil.Net.Email()
            {
                SmtpServer = "smtp.qq.com",// SMTP服务器
                SmtpPort = 587, // SMTP服务器端口
                EnableSsl = true,//使用SSL
                Username = "lining_wx@qq.com",// 邮箱用户名
                Password = "fstimzxevvgaccie",// 邮箱密码
                Tos = "528952776@qq.com", //收件人
                Subject = "测试邮件",//邮件标题
                Body = "你好啊,这是异步发送",//邮件内容
            }.SendAsync(s =>
            {
                Console.WriteLine(s);// 发送成功后的回调
            });// 异步发送邮件

            new MyUtil.Net.Email()
            {
                SmtpServer = "smtp.qq.com",// SMTP服务器
                SmtpPort = 587, // SMTP服务器端口
                EnableSsl = true,//使用SSL
                Username = "lining_wx@qq.com",// 邮箱用户名
                Password = "fstimzxevvgaccie",// 邮箱密码
                Tos = "528952776@qq.com", //收件人
                Subject = "测试邮件",//邮件标题
                Body = "你好啊，这是同步发送",//邮件内容
            }.Send();
        }
    }
}
