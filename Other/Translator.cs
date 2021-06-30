using System;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
using MyUtil.Json;
namespace MyUtil.Other
{
    public class Translator
    {
        public enum Returnformat
        {
            String,
            Json
        }
        private static Returnformat returnType=Returnformat.Json;

        public static Returnformat ReturnType { get => returnType; set => returnType = value; }

        public static string Translate(string originText, string originLanguage, string targetLanguage)
        {
            return run(originText, originLanguage, targetLanguage); ;
        }
        public static string Translate(string originText, string targetLanguage)
        {
            return run(originText, "", targetLanguage); ;
        }

        static string run(string originText, string originLanguage, string targetLanguage)
        {
            // 原文
            string q = originText;

            string from;
            if (originLanguage == "")
            {
                // 源语言
                from = "auto";
            }
            else
            {
                // 源语言
                from = originLanguage;
            }
            // 目标语言
            string to = targetLanguage;
            // 改成您的APP ID
            string appId = "20200401000410434";
            Random rd = new Random();
            string salt = rd.Next(100000).ToString();
            // 改成您的密钥
            string secretKey = "wa2Wj3iTiLgORijprAXa";
            string sign = EncryptString(appId + q + salt + secretKey);
            string url = "http://api.fanyi.baidu.com/api/trans/vip/translate?";
            url += "q=" + HttpUtility.UrlEncode(q);
            url += "&from=" + from;
            url += "&to=" + to;
            url += "&appid=" + appId;
            url += "&salt=" + salt;
            url += "&sign=" + sign;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.UserAgent = null;
            request.Timeout = 6000;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            if (returnType!=Returnformat.Json)
            {

                //JavaScriptSerializer js = new JavaScriptSerializer();   //实例化一个能够序列化数据的类
                //Root list = js.Deserialize<Root>(retString);    //将json数据转化为对象类型并赋值给list
                //retString = list.trans_result[0].dst;
                
                var res =  (Root)JsonHelper.JsonDes<Root>(retString);
                retString = res.trans_result[0].dst;
            }

            return retString;
        }

        // 计算MD5值
        static string EncryptString(string str)
        {
            MD5 md5 = MD5.Create();
            // 将字符串转换成字节数组
            byte[] byteOld = Encoding.UTF8.GetBytes(str);
            // 调用加密方法
            byte[] byteNew = md5.ComputeHash(byteOld);
            // 将加密结果转换为字符串
            StringBuilder sb = new StringBuilder();
            foreach (byte b in byteNew)
            {
                // 将字节转换成16进制表示的字符串，
                sb.Append(b.ToString("x2"));
            }
            // 返回加密的字符串
            return sb.ToString();
        }
    }
    class Trans_resultItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string src { get; set; }
        /// <summary>
        /// 苹果
        /// </summary>
        public string dst { get; set; }
    }

    class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public string @from { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string to { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Trans_resultItem> trans_result { get; set; }
    }
}
