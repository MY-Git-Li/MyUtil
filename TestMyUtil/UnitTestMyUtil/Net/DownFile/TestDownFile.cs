using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MyUtil.Net;

namespace UnitTestMyUtil.Net.DownFile
{
    [TestClass]
    public class TestDownFile
    {
        [TestMethod]
        public void Run()
        {
            string url = "https://down.rbread02.cn/down/pcsoft/7/24/wjnmzhgj.zip?timestamp=611f17b3&md5hash=b9909cc46beb9b8038fe0bf1f6f88221";
            var mtd = new MultiThreadDownloader(url, Environment.GetEnvironmentVariable("temp"), "E:\\Downloads\\wjnmzhgj.zip", 8);

            mtd.Configure(req =>
            {

                req.Referer = "https://masuit.com";
                req.Headers.Add("Origin", "https://baidu.com");

            });

            mtd.TotalProgressChanged += (sender, e) =>
            {
                var downloader = sender as MultiThreadDownloader;
                Console.WriteLine("下载进度：" + downloader.TotalProgress + "%");
                Console.WriteLine("下载速度：" + downloader.TotalSpeedInBytes / 1024 / 1024 + "MBps");
            };
            //mtd.FileMergeProgressChanged += (sender, e) =>
            //{
            //    Console.WriteLine("下载完成");
            //};
            mtd.FileMergedComplete += (sender, e) =>
            {
                Console.WriteLine("文件合并完成");
            };
            mtd.Start();//开始下载
        }
    }
}
