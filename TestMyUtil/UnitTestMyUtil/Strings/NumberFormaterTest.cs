using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyUtil.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUtil.Strings
{
    [TestClass()]
    public class NumberFormaterTest
    {
        [TestMethod()]
        public void Run()
        {
            NumberFormater nf = new NumberFormater(26);//内置2-62进制的转换
            //NumberFormater nf = new NumberFormater("0123456789abcdefghijklmnopqrstuvwxyz");// 自定义进制字符，可用于生成验证码
            string s36 = nf.ToString(18);
            long num = nf.FromString("7clzi");
            Console.WriteLine("12345678的36进制是：" + s36); // 7clzi
            Console.WriteLine("36进制的7clzi是：" + num); // 12345678

            Console.WriteLine(NumberFormater.ToChineseMoney(123456789.12));
        }

    }
}