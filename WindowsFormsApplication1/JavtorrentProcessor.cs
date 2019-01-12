using Framework.tool;
using JJCCX.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using TidyManaged;

namespace WindowsFormsApplication1
{
    class JavtorrentProcessor:IPageProcessor
    {
        Regex listRegex = new Regex("<li>.*?</li>");
        Regex threadRegex = new Regex("<a href=\"/censored/.*?/\">|<a href=\"/iv/.*?/\">");
        Regex nameRegex=new Regex("<span class=\"base-t\">.*?</span>");
        public void NavigateHandle(System.Windows.Forms.WebBrowser webBrowser1, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e, string path1)
        {
            System.IO.StreamReader getReader = new System.IO.StreamReader(webBrowser1.DocumentStream);
            Console.WriteLine(e.Url);
            string gethtml = getReader.ReadToEnd();
            if (gethtml.Contains("500 Internal Privoxy Error"))
            {
                Console.WriteLine("500 Internal Privoxy Error TRY AGAIN");
                webBrowser1.Navigate(e.Url);
                return;
            }

            Config1.BlockingQueue.Dequeue();

            if (!webBrowser1.Url.ToString().Contains("category")&& !webBrowser1.Url.ToString().Contains("?s="))
            {
                string path = Config1.dictionary[webBrowser1.Url.ToString()].Path;
                DlTool.SaveFile(gethtml, path);
            }
            else
            {
                DlTool.SaveFile(gethtml, Path.Combine(path1, DlTool.ReplaceUrl(e.Url.ToString()) + ".htm"));
                gethtml = gethtml.Replace(" class=\"b5\"", "");
                MatchCollection mc = listRegex.Matches(gethtml);
                foreach(Match match in mc)
                {
                    string list = match.Value;
                    //<li><a href="/iv/143835/">
                    string url = "http://javtorrent.re"+threadRegex.Match(list).Value.Replace("<a href=\"","").Replace("\">","");
                    if (url == "http://javtorrent.re" || url == "http://javtorrent.re/")
                        continue;
                    string name = nameRegex.Match(list).Value.Replace("<span class=\"base-t\">", "").Replace("</span>","");
                    string path = Path.Combine(path1, DlTool.ReplaceUrl(name) + ".htm");

                    AsynObj o = new AsynObj();
                    o.Url = url;
                    o.Path = path;
                    if (!Config1.dictionary.ContainsKey(url))
                    {
                        Config1.dictionary.Add(url, o);
                    }
                    Config1.BlockingQueue.Enqueue(o);

                }
            }
            AsynObj asynObj1 = Config1.BlockingQueue.Peek();
            webBrowser1.Navigate(asynObj1.Url);
        }
    }
}
