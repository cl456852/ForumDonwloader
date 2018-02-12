using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;


namespace Framework.tool
{
    public class DownloadTool
    {
        public static string GetHtml(string url, bool useProxy, HttpWebRequest request1)
        {
            ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;
            string str = string.Empty;
            bool success = false;
            while (!success)
            {
                HttpWebResponse response = null;
                Console.WriteLine(url);
                StreamReader streamReader = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                try
                {
                    Config1.mre.WaitOne();
           

                    request.Referer = "http://taohuabt.info/forum.php";
                    request.Host = "thzvv.com";

                    request.CookieContainer = Config1.coockieContainer;
                    request.UserAgent = "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.153 Safari/537.36";
                    request.Timeout = 15000;
                    request.KeepAlive = false;
                    //  request.SendChunked = true;
                    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    //   request.TransferEncoding = "gzip,deflate,sdch";
                    if (useProxy)
                    {

                        WebProxy proxy = new WebProxy("127.0.0.1", 8087);
                        request.Proxy = proxy;
                    }
                    response = (HttpWebResponse)request.GetResponse();
                    //Config1.setCoockies(new Uri("http://taohuabt.info"),response.Headers[HttpResponseHeader.SetCookie]);
                    Stream streamReceive = response.GetResponseStream();
                    Encoding encoding = Encoding.GetEncoding("utf-8");
                    streamReader = new StreamReader(streamReceive, encoding);
                    str = streamReader.ReadToEnd();
                    success = true;
                }

                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message + "  " + url);
                    //if (ex.Message.Contains("接收时发生错误")&&!Config1.checkTime())
                    //{
                    //    Config1.appendFile(url,"d:\\test\\failList.txt");
                    //    success = true;
                    //}
                    //Config1.Check();
                }
                finally
                {
                    if (request != null)
                        request.Abort();
                    if (streamReader != null)
                        streamReader.Close();
                    if (response != null)
                        response.Close();

                    Thread.Sleep(1000);



                }
            }
            return str;
        }


        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public static void downLoadFile(string url, string name, bool useProxy, HttpWebRequest downloadParam)
        {

            bool success = false;
            while (!success)
            {
                Console.WriteLine(url);
                HttpWebResponse response = null;
                FileStream fstream = null;
                HttpWebRequest request = null;
                Stream stream = null;
                StreamReader reader = null;
                Stream streamReceive = null;
                try
                {
                    Config1.mre.WaitOne();
                    request = (HttpWebRequest)WebRequest.Create(url);
                    request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/33.0.1750.154 Safari/537.36";
                    request.CookieContainer = Config1.coockieContainer;
                    request.Timeout = 15000;
                    request.KeepAlive = false;
                    request.Referer = downloadParam.Referer;
                    request.Host = downloadParam.Host;
                    if (useProxy)
                    {

                        WebProxy proxy = new WebProxy("127.0.0.1", 8087);
                        request.Proxy = proxy;
                    }
                    response = (HttpWebResponse)request.GetResponse();
                    Config1.setCoockies(new Uri("http://taohuabt.info"), response.Headers[HttpResponseHeader.SetCookie]);
                    streamReceive = response.GetResponseStream();
                    string path = Path.GetDirectoryName(name);
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    if (File.Exists(name))
                    {
                        name += "1";
                    }
                    stream = new MemoryStream();
                    streamReceive.CopyTo(stream);
                    reader = new StreamReader(stream);
                    stream.Position = 0;
                    string fileContent = reader.ReadToEnd();
                    stream.Position = 0;
                    fstream = new FileStream(name, FileMode.Create);
                    stream.CopyTo(fstream);
                    success = true;

                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "  " + url);
                    //if (ex.Message.Contains("接收时发生错误") && !Config1.checkTime() || ex.Message.Contains("不支持给定路径的格式") || ex.Message.Contains("指定的路径或文件名太长"))
                    if (ex.Message.Contains("不支持给定路径的格式") || ex.Message.Contains("指定的路径或文件名太长") || ex.Message.Contains("404") || ex.Message.Contains("非法字符"))
                    {
                        Config1.appendFile(url, "d:\\test\\failList.txt");
                        success = true;
                    }
                    //Config1.Check();

                }
                finally
                {
                    if (request != null)
                        request.Abort();
                    if (fstream != null)
                        fstream.Close();
                    if (response != null)
                        response.Close();
                    if (stream != null)
                        stream.Close();
                    if (reader != null)
                        reader.Close();
                    if (streamReceive != null)
                        streamReceive.Close();
                    Thread.Sleep(1000);
                }
            }

        }
    }
}
