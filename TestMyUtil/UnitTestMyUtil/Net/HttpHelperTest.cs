using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace MyUtil.Net
{
    [TestClass()]
    public class HttpHelperTest
    {
        [TestMethod()]
        public void GetHtmlRun()
        {
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = "https://i0.hdslb.com/bfs/album/7855946ffe6b88ef2d287622010c6b9903692424.png",//URL     必需项  
                ResultType = ResultType.Byte,
            };
            HttpResult result = http.GetHtml(item);
            Image img = result.GetImage();
        }
    }
}