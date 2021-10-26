using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUtil
{
    [TestClass()]
    public class IStringExtensionsTest
    {
        [TestMethod()]
        public void ToSimplifiedOrTraditional()
        {
            string FanStr = "大戰三國";
            Console.WriteLine(FanStr.ToSimplified());

            string janstr = "大战三国";
            Console.WriteLine(janstr.ToTraditional());
        }
    }
}