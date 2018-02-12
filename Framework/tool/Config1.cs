using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace Framework.tool
{
    public class Config1
    {
        public static string Cookie;

        public const string EMPTY_URL = "https://www.akiba-online.com/forums/iv-torrents.172/page-2?order=post_date";

        public static BlockingQueue<AsynObj> BlockingQueue=new BlockingQueue<AsynObj>();

        public static string InvalidPathFilter(string path)
        {
            char root = path[0];

            path= path.Substring(2);
            string invalid = new string(Path.GetInvalidPathChars()) + "*:?";

            foreach (char c in invalid)
            {
                path = path.Replace(c.ToString(), "");
            }
            return root+":"+ path;
        }

        static object txtLock = new object();
        public static void appendFile(string content, string path)
        {

            lock (txtLock)
            {
                try
                {
                    FileStream stream;
                    if (!File.Exists(path))
                    {
                        stream = File.Create(path);
                        stream.Close();
                    }

                    StreamWriter sw = File.AppendText(path);
                    sw.WriteLine(content);
                    sw.Flush();
                    sw.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("FileApprendError:  " + e.Message + content + ":" + path);
                }
            }
        }


        static bool isFisrtTimeConnectionReset = true;
        static TimeSpan ts1 = new TimeSpan(DateTime.Now.Ticks);

        public static bool checkTime()
        {
            Console.WriteLine("开始检测时间");
            TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            if (!isFisrtTimeConnectionReset)
            {
                if (ts.TotalSeconds > 30)
                {
                    Console.WriteLine("时间>30s, 新的REST");
                    ts1 = ts2;
                    return false;
                }
                else
                {
                    Console.WriteLine("时间<30s, retry");
                    return true;
                }
            }
            else
            {
                Console.WriteLine("第一次REST");
                isFisrtTimeConnectionReset = false;
                ts1 = ts2;
                return false;
            }
        }
        public DateTime time = new DateTime();

        static object lock1 = new object();
        public static string failList = "";

        public static void setFailList(string s)
        {
            lock (lock1)
            {
                failList += s;
            }
        }

        static object ob = new object();
        public static ManualResetEvent mre = new ManualResetEvent(true);

        public static void Flooding()
        {

            if (Monitor.TryEnter(ob))
            {
                mre.Reset();
                while (true)
                {
                    if (RouterRedail(0))
                        break;
                }
                while (true)
                {
                    if (RouterRedail(1))
                        break;
                }
                mre.Set();
                Monitor.Exit(ob);
            }

        }

        static void redailRouter(int param)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://192.168.1.1/start_apply2.htm");

            request.Method = "get";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Credentials = CredentialCache.DefaultCredentials;

            //获得用户名密码的Base64编码
            string code = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "admin", "admin")));
            request.Method = "POST";
            //添加Authorization到HTTP头
            request.Headers.Add("Authorization", "Basic " + code);
                                                 //current_page=%2Findex.asp&next_page=%2Findex.asp&flag=Internet&action_mode=apply&action_script=restart_wan_if&action_wait=5&
            byte[] data = Encoding.ASCII.GetBytes("current_page=%2Findex.asp&next_page=%2Findex.asp&flag=Internet&action_mode=apply&action_script=restart_wan_if&action_wait=5&wan_enable=" + param + "&wans_dualwan=wan+none&wan_unit=0&wans_mode=fo");
            request.ContentLength = data.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamReceive = response.GetResponseStream();
            Encoding encoding = Encoding.GetEncoding("GB2312");
            StreamReader streamReader = new StreamReader(streamReceive, encoding);
            string str = streamReader.ReadToEnd();
            Console.WriteLine(str);
            response.Close();
        }

        static void routerRedail(int param)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            string gethost = string.Empty;
            CookieContainer cc = new CookieContainer();
            string Cookiesstr = string.Empty;

            string postData = "group_id=&action_mode=&action_script=&action_wait=5&current_page=Main_Login.asp&next_page=index.asp&login_authorization=QVNVUzoxMTExMTFh";
            string LoginUrl = "http://192.168.50.1/login.cgi";
            request = (HttpWebRequest)WebRequest.Create(LoginUrl);//实例化web访问类   
            request.Method = "POST";//数据提交方式为POST   
            //模拟头   
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] postdatabytes = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = postdatabytes.Length;
            request.AllowAutoRedirect = false;
            request.CookieContainer = cc;
            request.KeepAlive = true;
            //提交请求   
            Stream stream;
            stream = request.GetRequestStream();
            stream.Write(postdatabytes, 0, postdatabytes.Length);
            stream.Close();
            //接收响应   
            response = (HttpWebResponse)request.GetResponse();
            //保存返回cookie   
            response.Cookies = request.CookieContainer.GetCookies(request.RequestUri);
            CookieCollection cook = response.Cookies;
            string strcrook = request.CookieContainer.GetCookieHeader(request.RequestUri);
            Cookiesstr = strcrook;
            //取第一次GET跳转地址   
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string content = sr.ReadToEnd();
            response.Close();
            gethost = "http://192.168.50.1/start_apply2.htm";

           
            request = (HttpWebRequest)WebRequest.Create(gethost);
            request.Method = "POST";
            request.KeepAlive = true;
                                                         //_ceg.s=ogqswo; _ceg.u=ogqswo; traffic_warning_0=2017.0:1; _ga=GA1.2.1397880773.1479308566; wireless_list_9C:5C:8E:8C:05:A0_temp=<CC:2D:83:43:C2:B8>Yes<64:A5:C3:E2:7C:68>No<A4:D1:8C:C4:53:C4>No; asus_token=7160076249881435368878789362127; wireless_list_9C:5C:8E:8C:05:A0=<CC:2D:83:43:C2:B8>Yes
            request.Headers.Add("Cookie:" + Cookiesstr );
            request.ContentType = "application/x-www-form-urlencoded";
            request.AllowAutoRedirect = true;
            request.Host = "router.asus.com";
            request.Referer = "http://router.asus.com/device-map/internet.asp";
            request.Headers.Add("Origin", "http://router.asus.com");
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.99 Safari/537.36";
                                                 //current_page=%2Findex.asp&next_page=%2Findex.asp&flag=Internet&action_mode=apply&action_script=restart_wan_if&action_wait=5&wan_enable=0        &wans_dualwan=wan+none&wan_unit=0&wans_mode=fo 
            byte[] data = Encoding.UTF8.GetBytes("current_page=%2Findex.asp&next_page=%2Findex.asp&flag=Internet&action_mode=apply&action_script=restart_wan_if&action_wait=5&wan_enable=" + param + "&wans_dualwan=wan+none&wan_unit=0&wans_mode=fo");
            request.ContentLength = data.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();

            response = (HttpWebResponse)request.GetResponse();

        
            StreamReader sr1 = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string ss = sr1.ReadToEnd();
            sr1.Close();
            response.Close();
        }

        static void redail()
        {
            Console.WriteLine("StartRedail");
            Process p1 = new Process();
            p1.StartInfo.FileName = "cmd.exe";
            p1.StartInfo.Arguments = "/c " + "rasdial.exe  /disconnect";
            p1.Start();
            p1.WaitForExit();
            Thread.Sleep(3000);
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/c " + "rasdial.exe 宽带连接 300000162885 68819054";
            p.Start();
            p.WaitForExit();
            Console.WriteLine("EndRedail");
            Thread.Sleep(10000);
            mre.Set();

        }

        static bool checkConnection()
        {
            string url = "http://rarbg.to/index5.php";
            bool success = false;
            string str;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Console.WriteLine("CHECKCONNECT:" + "http://rarbg.to/index5.php");
            StreamReader streamReader = null;

            try
            {
                CookieContainer cookieContainer = new CookieContainer();

                Cookie lastVisit = new Cookie("LastVisit", Config1.getLastVisit(), "/", "rarbg.to");
                Cookie __utma = new Cookie("__utma", "211336342.1333136546.1369105449.1369109171.1369112684.3", "/", "rarbg.to");
                Cookie __utmb = new Cookie("__utmb", "211336342.5.10.1369112684", "/", "rarbg.to");
                Cookie __utmc = new Cookie("__utmc", "211336342", "/", "rarbg.to");
                Cookie __utmz = new Cookie("__utmz", "211336342.1369105449.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none)", "/", "rarbg.to");
                Cookie bSbTZF2j = new Cookie("bSbTZF2j", "6BdPQ9qs", "/", "rarbg.to");
                cookieContainer.Add(lastVisit);
                cookieContainer.Add(bSbTZF2j);
                cookieContainer.Add(__utma);
                cookieContainer.Add(__utmb);
                cookieContainer.Add(__utmc);
                cookieContainer.Add(__utmz);

                request = (HttpWebRequest)WebRequest.Create(url);
                request.CookieContainer = cookieContainer;
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/33.0.1750.154 Safari/537.36";
                request.Timeout = 15000;
                request.KeepAlive = false;
                request.Referer = "http://rarbg.to/torrent/j1kx3ny";


                response = (HttpWebResponse)request.GetResponse();
                if (response.Cookies["LastVisit"] != null)
                    Config1.setLastVisit(response.Cookies["LastVisit"].ToString());
                Stream streamReceive = response.GetResponseStream();
                Encoding encoding = Encoding.GetEncoding("GB2312");
                streamReader = new StreamReader(streamReceive, encoding);
                str = streamReader.ReadToEnd();
                success = true;
                Console.WriteLine("检测成功");
            }

            catch (Exception ex)
            {

                Console.WriteLine(ex.Message + ":检测失败:" + url);
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
            return success;
        }



        static object o = new object();
        private static string lastVisit = "1374857595";

        public static void setLastVisit(string s)
        {
            lock (o)
            {
                lastVisit = s;
            }
        }

        public static CookieContainer coockieContainer=new CookieContainer();

        public static void setCoockies(Uri uri, string cookieHeader)
        {

            coockieContainer.SetCookies(uri, cookieHeader);
            
        }

        public static string getLastVisit()
        {
            return lastVisit;
        }


        public static bool RouterRedail(int param)
        {
            try
            {
                if (param == 0)
                    while (true)
                    {
                        routerRedail(0);
                        Console.WriteLine("disconnect");
                        Thread.Sleep(5000);
                        if (getRouterStat(5))
                        {
                            Console.WriteLine("disconnect Check Successful");
                            break;
                        }
                        Console.WriteLine("disconnect Check Fail");

                    }
                if (param == 1)
                    while (true)
                    {
                        routerRedail(1);
                        Console.WriteLine("connect");
                        Thread.Sleep(10000);
                        if (getRouterStat(2))
                        {
                            Console.WriteLine("connect Check Successful");
                            break;
                        }
                        Console.WriteLine("connect Check Fail");
                    }
            }catch(Exception e)
            {
                Console.WriteLine("路由器错误 " + e.Message);
                return false;
            }
            return true;
        }

        public static bool checkRouterStatus(int status)
        {
            string code = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "admin", "admin")));

            HttpWebRequest request = null;
            HttpWebResponse response = null;

            StreamReader streamReader = null;
            request = (HttpWebRequest)WebRequest.Create("http://192.168.1.1/ajax_status.asp");
            request.Method = "POST";
            //添加Authorization到HTTP头
            request.Headers.Add("Authorization", "Basic " + code);
            byte[] data = Encoding.ASCII.GetBytes("current_page=%2Findex.asp&next_page=%2Findex.asp&flag=Internet&action_mode=apply&action_script=restart_wan_if&action_wait=5&wan_enable=" + status + "&wans_dualwan=wan+none&wan_unit=0");
            request.ContentLength = data.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);




            response = (HttpWebResponse)request.GetResponse();
            Stream streamReceive = response.GetResponseStream();
            Encoding encoding = Encoding.GetEncoding("GB2312");
            streamReader = new StreamReader(streamReceive, encoding);
            string str;
            str = streamReader.ReadToEnd();
            if (str.Contains("<wan>" + status + "</wan>"))
                return true;
            else
                return false;
        }

        public static bool getRouterStat(int status)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            string gethost = string.Empty;
            CookieContainer cc = new CookieContainer();
            string Cookiesstr = string.Empty;

            string postData = "group_id=&action_mode=&action_script=&action_wait=5&current_page=Main_Login.asp&next_page=index.asp&login_authorization=QVNVUzoxMTExMTFh";
            string LoginUrl = "http://192.168.50.1/login.cgi";
            request = (HttpWebRequest)WebRequest.Create(LoginUrl);//实例化web访问类   
            request.Method = "POST";//数据提交方式为POST   
            //模拟头   
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] postdatabytes = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = postdatabytes.Length;
            request.AllowAutoRedirect = false;
            request.CookieContainer = cc;
            request.KeepAlive = true;
            //提交请求   
            Stream stream;
            stream = request.GetRequestStream();
            stream.Write(postdatabytes, 0, postdatabytes.Length);
            stream.Close();
            //接收响应   
            response = (HttpWebResponse)request.GetResponse();
            //保存返回cookie   
            response.Cookies = request.CookieContainer.GetCookies(request.RequestUri);
            CookieCollection cook = response.Cookies;
            string strcrook = request.CookieContainer.GetCookieHeader(request.RequestUri);
            Cookiesstr = strcrook;
            //取第一次GET跳转地址   
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string content = sr.ReadToEnd();
            response.Close();
            gethost = "http://192.168.50.1/ajax_status.xml";
           
            request = (HttpWebRequest)WebRequest.Create(gethost);
            request.Method = "POST";
            request.KeepAlive = true;
            request.Headers.Add("Cookie:" + Cookiesstr);
     
            request.AllowAutoRedirect = true;
            request.Host = "router.asus.com";
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.99 Safari/537.36";
            response = (HttpWebResponse)request.GetResponse();

        
            StreamReader sr1 = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string ss = sr1.ReadToEnd();
            sr1.Close();
            response.Close();
            if (ss.Contains("<first_wan>" + status + "</first_wan>"))
                return true;
            return false;
        }

        public static Dictionary<String, AsynObj> dictionary = new Dictionary<String, AsynObj>();

        public static string ValidePath(string path)
        {
            return path.Replace('/', '_').Replace(":", "^").Replace("?", "_").Replace('\\','_').Replace("|", "");
        }
    }
}
