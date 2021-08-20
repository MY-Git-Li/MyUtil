using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyUtil.FileOperate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUtil.FileOperate
{
    [TestClass()]
    public class SevenZipCompressorTest
    {
        [TestMethod()]
        public void Decompress()
        {
            SevenZipCompressor.Decompress(@"D:\360极速浏览器下载\VisualAssistX 10.9.2358.zip", @"H:\text");
        }
    }
}