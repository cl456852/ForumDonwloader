using Framework.tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ThzDownloader
{
    public class Common
    {

        public static HttpWebRequest CreateHttpWebRequest(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/33.0.1750.154 Safari/537.36";
            request.CookieContainer = Config1.coockieContainer;
            request.Timeout = 15000;
            request.KeepAlive = false;
            request.Referer = "http://taohuabt.info/forum.php";
            request.Host = "taohuabt.info";
            return request;
        }
    }
}
