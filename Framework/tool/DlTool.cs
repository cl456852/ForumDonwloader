using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.Net.Sockets;

namespace Framework.tool
{
    public class DlTool
    {

        public static string GetHtml(string url,bool useProxy)
        {
            string str = string.Empty;
            bool success=false;
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
                    Cookie lastVisit = new Cookie("LastVisit", Config1.getLastVisit(), "/", "rarbg.to");
                    //Cookie __utma = new Cookie("__utma", "9515318.860353583.1429342721.1449335760.1449670802.1", "/", ".rarbg.to");
                    //Cookie __utmb = new Cookie("__utmb", "9515318.23.10.1449670802", "/", ".rarbg.to");
                    //Cookie __utmc = new Cookie("__utmc", "9515318", "/", ".rarbg.to");
                    //Cookie __utmz = new Cookie("__utmz", "9515318.1447862416.86.2.utmcsr=rarbg.com|utmccn=(referral)|utmcmd=referral|utmcct=/download.php", "/", ".rarbg.to");
                    //Cookie __utmt = new Cookie("__utmt", "1", "/", ".rarbg.to");
                    Cookie c_cookie = new Cookie("c_cookie", "9ctp471aws", "/", ".rarbg.to");
                    Cookie rarbg = new Cookie("rarbg", "1%7CThu%2C%2026%20Jan%202017%2014%3A13%3A00%20GMT", "/", "rarbg.to");
                    Cookie tcc = new Cookie("tcc", "", "/", ".rarbg.to");
                    Cookie skt = new Cookie("skt", "FBK40c6gie", "/", "rarbg.to");
                    Cookie skt1 = new Cookie("skt", "FBK40c6gie", "/", ".rarbg.to");
                    Cookie wQnP98Kj = new Cookie("wQnP98Kj", "wZkvrmuL", "/", "rarbg.to");
                    Cookie wQnP98Kj1 = new Cookie("wQnP98Kj", "wZkvrmuL", "/", ".rarbg.to");
                    Cookie expla = new Cookie("expla", "4", "/", "rarbg.to");
                    Cookie aby = new Cookie("aby", "1", "/", "rarbg.to");
                    cookieContainer.Add(lastVisit);
                    cookieContainer.Add(c_cookie);
                    cookieContainer.Add(expla);
                    cookieContainer.Add(wQnP98Kj1);
                    cookieContainer.Add(rarbg);
                    cookieContainer.Add(tcc);
                    cookieContainer.Add(skt);
                    cookieContainer.Add(aby);
                    cookieContainer.Add(wQnP98Kj);
                    cookieContainer.Add(skt1);
                    request = (HttpWebRequest)WebRequest.Create(url);
                    request.CookieContainer = cookieContainer;
                    request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36";
                    request.Timeout = 15000;
                    request.KeepAlive = false;
                    request.Referer = "https://rarbg.to/torrents.php?category=1%3B4&page=542";
                    request.Host = "rarbg.to";
                    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";

                    response = (HttpWebResponse)request.GetResponse();
                    if (response.Cookies["LastVisit"] != null)
                        Config1.setLastVisit( response.Cookies["LastVisit"].ToString());
                    Stream streamReceive = response.GetResponseStream();
                    Encoding encoding = Encoding.GetEncoding("GB2312");
                    streamReader = new StreamReader(streamReceive, encoding);
                    str = streamReader.ReadToEnd();
                    if (str.Contains("We have too many requests from your ip"))
                    {
                       // Config1.Flooding();
                        Console.Write("We have too many requests from your ip");
                        continue;
                    }
                    success = true;
                }

                catch (Exception ex)
                {
                    
                    Console.WriteLine(ex.Message + "  " + url);
        
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

        public static void SaveFile(string content, string fileName)
        {
            if (fileName.Length > 240)
            {
                fileName = fileName.Substring(0, 240)+".htm";
            }
            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            }
            fileName = fileName.Replace("%20", "").Replace("%2C", "").Replace("%22","").Replace("*","");
            //实例化一个文件流--->与写入文件相关联
            FileStream fs = new FileStream(fileName, FileMode.Create);
            //实例化一个StreamWriter-->与fs相关联
            StreamWriter sw = new StreamWriter(fs,Encoding.UTF8);
            //开始写入
            sw.Write(content);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }

        public static void AppendFile(string content,string fileName)
        {
            StreamWriter sw = File.AppendText(fileName);
            sw.WriteLine(content);
            sw.Flush();
            sw.Close();   
        }
            
        public static void downLoadFile(string url,string name,bool useProxy,string content, string cookieStr)
        {

            bool success = false;
            while (!success)
            {
                Console.WriteLine(url);
                HttpWebResponse response = null;
                FileStream fstream = null;
                HttpWebRequest request = null;
                Stream stream=null;
                StreamReader reader = null;
                Stream streamReceive=null;
                try
                {
                    Config1.mre.WaitOne();
//                    CookieContainer cookieContainer = new CookieContainer();
//                    Cookie lastVisit = new Cookie("LastVisit", Config1.getLastVisit(), "/", "rarbg.to");
//                    Cookie c_cookie = new Cookie("c_cookie", "9ctp471aws", "/", ".rarbg.to");
//                    Cookie sk = new Cookie("sk", "ge7v25kibl", "/", "rarbg.to");
//                    Cookie skt = new Cookie("c_cookie", "68grb0gz8l", "/", ".rarbg.to");
//                    Cookie skt1 = new Cookie("skt", "68grb0gz8l", "/", "rarbg.to");
//                    Cookie wQnP98Kj = new Cookie("wQnP98Kj", "wZkvrmuL", "/", "rarbg.to");
//                    Cookie wQnP98Kj1 = new Cookie("wQnP98Kj", "wZkvrmuL", "/", ".rarbg.to");
//                    Cookie expla = new Cookie("expla", "expla", "/", "rarbg.to");
//                    cookieContainer.Add(lastVisit);
//                    cookieContainer.Add(c_cookie);
//                    cookieContainer.Add(expla);
//                    cookieContainer.Add(sk);
//                    cookieContainer.Add(skt);
//                    cookieContainer.Add(skt1);
//                    cookieContainer.Add(wQnP98Kj);
//                    cookieContainer.Add(wQnP98Kj1);
                    request = (HttpWebRequest)WebRequest.Create(url);
                    request.Headers.Add("Cookie", cookieStr);
                  //  request.CookieContainer = cookieContainer;
                    request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36";
                    request.Timeout = 15000;
                    request.KeepAlive = false;
                    request.Referer = "http://rarbg.to/torrent/j1kx3ny";
                    request.Host = "rarbg.to";
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
                        name =Path.Combine( Path.GetDirectoryName(name),"duplicateName", Path.GetFileNameWithoutExtension(name) + "(" + System.Guid.NewGuid().ToString().Substring(0,4) + ").torrent");
                        Console.WriteLine("duplicate filename: " + name);
                    }
                    stream = new MemoryStream();
                    streamReceive.CopyTo(stream);
                     reader = new StreamReader(stream);
                    stream.Position = 0;
                    string fileContent = reader.ReadToEnd();
                    if (fileContent.Contains("We are sorry but this is pure flooding"))
                    {
                        Config1.Flooding();
                        Console.Write("We have too many requests from your ip");

                        continue;
                    }
                    stream.Position = 0;
                    fstream = new FileStream(name, FileMode.Create);
                    stream.CopyTo(fstream);
                    SaveFile(content, name+".htm");
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

        public static string ReplaceUrl(string url)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                url = url.Replace(c.ToString(), "");
            }
            return url;
        }
    }
}
