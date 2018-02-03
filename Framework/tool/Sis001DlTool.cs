using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace Framework.tool
{
    public class Sis001DlTool
    {
        static Cookie cdb2_sid = new Cookie("cdb2_sid", "YLSfx3", "/", ".sis001.com");
        public static string GetHtml(string url, bool useProxy, string encodingStr)
        {
            string str = string.Empty;
            bool success = false;
            while (!success)
            {
                HttpWebRequest request = null;
                HttpWebResponse response = null;
                Console.WriteLine(url);
                StreamReader streamReader = null;

                try
                {
                    Config1.mre.WaitOne();
                    CookieContainer cookieContainer = new CookieContainer();
                    Cookie __cfduid = new Cookie("__cfduid", "d388668d6cbc4666a3d0cc13e5c96ebb01450437129", "/", ".sis001.com");
                    Cookie __utma = new Cookie("__utma", "55300009.1731467101.1450437133.1465127726.1466939474.32", "/", ".sis001.com");
                    Cookie __utmb = new Cookie("__utmb", "55300009.14.10.1466939474", "/", ".sis001.com");
                    Cookie __utmc = new Cookie("__utmc", "55300009", "/", ".sis001.com");
                    Cookie __utmz = new Cookie("__utmc", "55300009.1450666650.9.2.utmcsr=google|utmccn=(organic)|utmcmd=organic|utmctr=(not%20provided)", "/", ".sis001.com");
                    Cookie cdb2_oldtopics = new Cookie("cdb2_oldtopics", "D9668619D9668602D9668995D", "/", ".sis001.com");
                    cookieContainer.Add(__utma);
                    cookieContainer.Add(__utmb);
                    cookieContainer.Add(__utmc);
                    cookieContainer.Add(__utmz);
                    cookieContainer.Add(cdb2_sid);
                    cookieContainer.Add(__cfduid);
                    cookieContainer.Add(cdb2_oldtopics);
                    request = (HttpWebRequest)WebRequest.Create(url);
                    request.CookieContainer = cookieContainer;
                    request.UserAgent = "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.153 Safari/537.36";
                    request.Timeout = 15000;
                    request.KeepAlive = false;
                    request.Referer = "http://www.sis001.com/forum/forum-230-28.html";
                    request.Host = "www.sis001.com";
                    //  request.SendChunked = true;
                    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    //   request.TransferEncoding = "gzip,deflate,sdch";

                    if (useProxy)
                    {

                        WebProxy proxy = new WebProxy("127.0.0.1", 1080);
                        request.Proxy = proxy;
                    }
                    response = (HttpWebResponse)request.GetResponse();
                    if (response.Cookies["cdb2_sid"] != null)
                        cdb2_sid.Value = response.Cookies["cdb2_sid"].Value;
                    Stream streamReceive = response.GetResponseStream();
                    Encoding encoding = Encoding.GetEncoding(encodingStr);
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



        public static void downLoadFile(string url, string name, bool useProxy, string content)
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
                    CookieContainer cookieContainer = new CookieContainer();
                    Cookie vDVPaqSe = new Cookie("vDVPaqSe", "r9jSB2Wk", "/", "rarbg.to");
                    Cookie lastVisit = new Cookie("LastVisit", Config1.getLastVisit(), "/", "rarbg.to");
                    Cookie __utma = new Cookie("__utma", "9515318.860353583.1429342721.1449335760.1449670802.1", "/", ".rarbg.to");
                    Cookie __utmb = new Cookie("__utmb", "9515318.23.10.1449670802", "/", ".rarbg.to");
                    Cookie __utmc = new Cookie("__utmc", "9515318", "/", ".rarbg.to");
                    Cookie __utmz = new Cookie("__utmz", "9515318.1447862416.86.2.utmcsr=rarbg.com|utmccn=(referral)|utmcmd=referral|utmcct=/download.php", "/", ".rarbg.to");
                    Cookie __utmt = new Cookie("__utmt", "1", "/", ".rarbg.to");
                    cookieContainer.Add(vDVPaqSe);
                    cookieContainer.Add(lastVisit);
                    // cookieContainer.Add(bSbTZF2j);
                    cookieContainer.Add(__utma);
                    cookieContainer.Add(__utmb);
                    cookieContainer.Add(__utmc);
                    cookieContainer.Add(__utmz);
                    cookieContainer.Add(__utmt);
                    request = (HttpWebRequest)WebRequest.Create(url);
                    request.CookieContainer = cookieContainer;
                    request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/33.0.1750.154 Safari/537.36";
                    request.Timeout = 15000;
                    request.KeepAlive = false;
                    request.Referer = "http://rarbg.to/torrent/j1kx3ny";
                    if (useProxy)
                    {
                        WebProxy proxy = new WebProxy("10.10.8.1", 3128);
                        request.Proxy = proxy;
                    }
                    response = (HttpWebResponse)request.GetResponse();
                    if (response.Cookies["LastVisit"] != null)
                        Config1.setLastVisit(response.Cookies["LastVisit"].ToString());
                    streamReceive = response.GetResponseStream();
                    string path = Path.GetDirectoryName(name);
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    if (File.Exists(name))
                    {
                        name = Path.Combine(Path.GetDirectoryName(name), "duplicateName", Path.GetFileNameWithoutExtension(name) + "(" + System.Guid.NewGuid().ToString().Substring(0, 4) + ").torrent");
                        Console.WriteLine("duplicate filename: " + name);
                    }
                    stream = new MemoryStream();
                    streamReceive.CopyTo(stream);
                    reader = new StreamReader(stream);
                    stream.Position = 0;
                    string fileContent = reader.ReadToEnd();
                    stream.Position = 0;
                    fstream = new FileStream(name, FileMode.Create);
                    stream.CopyTo(fstream);
                    DlTool.SaveFile(content, name + ".htm");
                    success = true;

                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "  " + url);
                    //if (ex.Message.Contains("接收时发生错误") && !Config1.checkTime() || ex.Message.Contains("不支持给定路径的格式") || ex.Message.Contains("指定的路径或文件名太长"))
                    if (ex.Message.Contains("不支持给定路径的格式") || ex.Message.Contains("指定的路径或文件名太长"))
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
