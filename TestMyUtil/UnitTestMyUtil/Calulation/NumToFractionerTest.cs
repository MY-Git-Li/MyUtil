using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyUtil.Calulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUtil.Calulation
{
    [TestClass()]
    public class NumToFractionerTest
    {
        [TestMethod()]
        public void Run()
        {
            NumToFractioner a= new NumToFractioner();
            Console.WriteLine(a.Run(1.235));

            string cc = "123456";
         
            string num1Mutnum2 =BigNum.Pow(cc, '3');
            Console.WriteLine(num1Mutnum2);
        }

        [TestMethod()]
        public void Trans()
        {

        }
    }
}