using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CefSharp.Example.Handlers;
using CefSharp.WinForms;
using Framework.tool;

namespace BrowserDownloader
{
    class JavTorrents : IPageProcessor
    {
        Regex listRegex = new Regex("<div>.*?</div>");
        Regex threadRegex = new Regex("<a href=\"/censored/.*?/\">|<a href=\"/iv/.*?/\">");
        Regex nameRegex = new Regex("<span class=\"base-t\">.*?</span>");
        string listUrl;

       // public string ListUrl { get => listUrl; set => listUrl = value; }

        public void NavigateHandle(ChromiumWebBrowser webBrowser1, string url, string path1, string html)
        {
            Console.WriteLine(url);
            // Console.WriteLine("url:" + webBrowser1.Url);
            string gethtml = html;
            if (gethtml.Contains("500 Internal Privoxy Error"))
            {
                Console.WriteLine("500 Internal Privoxy Error TRY AGAIN");
                webBrowser1.Load(listUrl);
                return;
            }

            Config1.BlockingQueue.Dequeue();

            if (!url.ToString().Contains("category") && !url.ToString().Contains("?s="))
            {
                string path = Config1.dictionary[url.ToString()].Path;
                DlTool.SaveFile(gethtml, path);
            }
            else
            {

                DlTool.SaveFile(gethtml, Path.Combine(path1, DlTool.ReplaceUrl(url) + ".htm"));
                gethtml = gethtml.Replace(" class=\"b5\"", "");
                MatchCollection mc = listRegex.Matches(gethtml);
                foreach (Match match in mc)
                {
                    string list = match.Value;
                    //<li><a href="/iv/143835/">
                    string url1 = "http://javtorrent.re" + threadRegex.Match(list).Value.Replace("<a href=\"", "").Replace("\">", "");
                    if (url1 == "http://javtorrent.re" || url1 == "http://javtorrent.re/")
                        continue;
                    string name = nameRegex.Match(list).Value.Replace("<span class=\"base-t\">", "").Replace("</span>", "");
                    string path = Path.Combine(path1, DlTool.ReplaceUrl(name) + ".htm");

                    AsynObj o = new AsynObj();
                    o.Url = url1;
                    o.Path = path;
                    if (!Config1.dictionary.ContainsKey(url1))
                    {
                        Config1.dictionary.Add(url1, o);
                    }
                    Config1.BlockingQueue.Enqueue(o);

                }
            }
            AsynObj asynObj1 = Config1.BlockingQueue.Peek();
            //ListUrl = asynObj1.Url;
            webBrowser1.Load(asynObj1.Url);
        }
    }
}
