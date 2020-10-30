using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CefSharp.WinForms;
using Framework.tool;

namespace BrowserDownloader
{
    class JavDB : IPageProcessor
    {
        Regex nameRegex = new Regex("title=\".*?\">");
        Regex threadRegex = new Regex("<a href=\".*?class=\"box\"");
        Regex idRegex = new Regex("<div class=\"uid\">.*?</div>");
        public void NavigateHandle(ChromiumWebBrowser chromeBrowser, string url, string path, string html)
        {
            Console.WriteLine(url);
            // Console.WriteLine("url:" + webBrowser1.Url);
            string gethtml = html;
            if (gethtml.Contains("500 Internal Privoxy Error"))
            {
                Console.WriteLine("500 Internal Privoxy Error TRY AGAIN");
                chromeBrowser.Load(url);
                return;
            }

            if(gethtml.Contains("javdb.com contain sexually explicit content"))
            {
                Console.WriteLine("javdb.com contain sexually explicit content");
                return;
            }

            Config1.BlockingQueue.Dequeue();

            if (gethtml.Contains("<strong>离线下载:</strong><br>")||gethtml.Contains("Mark I watched this movie</p>"))
            {
                DlTool.SaveFile(gethtml, Path.Combine(path, Config1.dictionary[url].Path ));
            }
            else
            {
                DlTool.SaveFile(gethtml, Path.Combine(path, DlTool.ReplaceUrl(url) + ".htm"));
                MatchCollection mc = Regex.Matches(gethtml, "div class=\"grid-item column\">.*?<div class=\"meta\">", RegexOptions.Singleline);
                foreach (Match match in mc)
                {
                    string id = idRegex.Match(match.Value).Value.Replace("<div class=\"uid\">","").Replace("</div>","");
                    string url1 = "https://javdb.com" + threadRegex.Match(match.Value).Value.Replace("<a href=\"", "").Replace("\" class=\"box\"", "");
                    string name = nameRegex.Match(match.Value).Value.Replace("title=\"", "").Replace("\">", "");
                    string savePath = Path.Combine(path, id+"$$$"+DlTool.ReplaceUrl(name) + ".htm");
                    AsynObj o = new AsynObj();
                    o.Url = url1;
                    o.Path = savePath;
                    if (!Config1.dictionary.ContainsKey(url1))
                    {
                        Config1.dictionary.Add(url1, o);
                    }
                    Config1.BlockingQueue.Enqueue(o);


                }
            }

            AsynObj asynObj1 = Config1.BlockingQueue.Peek();
            chromeBrowser.Load(asynObj1.Url);
        }
    }
}
