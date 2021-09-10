using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyUtil.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyUtil.Images
{
    [TestClass()]
    public class WordCloudTest
    {
        [TestMethod()]
        public void WordCloud()
        {
            //核心
            //var wordCloud = new WordCloud(width, height, mask: mask, allowVerical: true, fontname: "YouYuan");
            //wordCloud.OnProgress += Wc_OnProgress;//过程函数，每一步都会执行
            //var image = wordCloud.Draw(Words, Frequencies);//阻塞函数，运行完成会生成对于的词云图片

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }

        
    }
}