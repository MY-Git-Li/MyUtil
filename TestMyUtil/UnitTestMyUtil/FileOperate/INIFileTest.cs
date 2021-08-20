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
    public class INIFileTest
    {
        [TestMethod()]
        public void Run()
        {
            DirFileHelper.CreateFile(@".\Text.ini");
            INIFile inihelp = new INIFile(@"C:\Users\Mr.Li\Desktop\Text.ini");
            inihelp.IniWriteValue("系统", "秘钥", "123456789");
        }

    }
}