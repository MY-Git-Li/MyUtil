using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyUtil.Other;
namespace UnitTestMyUtil.Other
{
    [TestClass]
   public class TestOther
   {
        [TestMethod]
        public void TestTrans()
        {
            Translator.ReturnType = Translator.Returnformat.String;
            Console.WriteLine(Translator.Translate("你好吗", "en"));
        }
   }
}
