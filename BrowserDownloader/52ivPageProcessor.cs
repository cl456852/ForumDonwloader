using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CefSharp.WinForms;
using Framework.tool;
//https://www.52iv.tv/forum.php?mod=forumdisplay&fid=136&forumdefstyle=yes&page={0}
namespace BrowserDownloader
{
    class _52ivPageProcessor : IPageProcessor
    {
        Regex nameRegex = new Regex("class=\"s xst\">.*?</a>");
        Regex listRegex = new Regex("href=\"thread.*? onclick=");

        public void NavigateHandle(ChromiumWebBrowser webBrowser1, string url, string path1, string html)
        {
            if(html.Contains("自动登录"))
            {
                Console.WriteLine("登录");
                return;
            }
            Console.WriteLine(url);
            string gethtml = html;
            if (html==""|| html.Contains("安全问题") || html.Contains("登录  - 我爱写真网"))
            {
                Console.WriteLine("login page");
                return;
            }
            if(html== "<html><head></head><body></body></html>")
            {
                Console.WriteLine("<html><head></head><body></body></html> error  " + url);
                AsynObj asynObj2 = Config1.BlockingQueue.Peek();
                webBrowser1.Load(asynObj2.Url);
                return;
            }
            if (url.ToString().Contains("thread") )
            {
                string path = Config1.dictionary[url.ToString()].Path;
                DlTool.SaveFile(gethtml, path);
            }
            else
            {
                DlTool.SaveFile(gethtml, Path.Combine(path1, DlTool.ReplaceUrl(url) + ".htm"));
                string[] threadList= gethtml.Split(new string[] { "normalthread_" },StringSplitOptions.None);
                foreach (string threadString in threadList)
                {
                    if(threadString.Contains("有新回复 - 新窗口打开")||threadString.Contains("<!DOCTYPE html PUBLIC"))
                    {
                        continue;
                    }
                    string list = listRegex.Match(threadString).Value;
                    list = list.Split('\"')[1];
                    string threadUrl = "https://www.52iv.click/" + list;
                    string name = nameRegex.Match(threadString).Value.Replace("class=\"s xst\">", "").Replace("</a>", "").Replace("/","#");
                    string path = Path.Combine(path1, DlTool.ReplaceUrl(name) + ".htm");
                    AsynObj o = new AsynObj();
                    o.Url = threadUrl;
                    o.Path = path;
                    if (!Config1.dictionary.ContainsKey(threadUrl))
                    {
                        string s = "https://www.52iv.click/forum.php?mod=viewthread&tid=" + threadUrl.Split( new string[] { "-" },StringSplitOptions.None)[1];
                        Config1.dictionary.Add(threadUrl, o);
                        Config1.dictionary.Add(s, o);
                    }
                    Config1.BlockingQueue.Enqueue(o);
                }

            }
            Config1.BlockingQueue.Dequeue();
            AsynObj asynObj1 = Config1.BlockingQueue.Peek();
            webBrowser1.Load(asynObj1.Url);
        }
    }
}
