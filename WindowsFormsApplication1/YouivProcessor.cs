using Framework.tool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class YouivProcessor : IPageProcessor
    {
        Regex idRegex1 = new Regex("[A-Z]{1,}-[0-9]{1,}|[A-Z]{1,}[0-9]{1,}|[A-Z]{1,}‐[0-9]{1,}");
        Regex theadRegex =new Regex("<a href=\"youiv-.*?class=\"z\">");
        Regex idRegex=new Regex("<a href=\"forum.php\\?mod=attachment&.*? target=\"_blank\">.*?torrent</a>");
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

            if (webBrowser1.Url.ToString().Contains("youiv-"))
            {
                //<a href="forum.php?mod=attachment&amp;aid=MjM4Nzc5fGUxZDdlNmFmfDE1MjMwOTI3MDN8MHwxNjc5NDE%3D" target="_blank">www.youiv.pw_SHIB-039.torrent</a>
                string path = Config1.dictionary[webBrowser1.Url.ToString()].Path;
                string value = idRegex.Match(gethtml).Value;
                if (!String.IsNullOrEmpty( value))
                {
                    string torernt = idRegex.Match(gethtml).Value.Split(new string[] { "target=\"_blank\"", "</a>" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    string id = idRegex1.Match(torernt.ToUpper().Replace("-","")).Value;
                    path = Path.Combine(path1, "[" + id + "]" + DlTool.ReplaceUrl(path) + ".htm");
                    DlTool.SaveFile(gethtml, path);

                } 
                else
                {
                    Console.WriteLine(webBrowser1.Url + "  value is null");
                }
    
            }
            else
            {
                //<a href="youiv-168123-1-1.html"  onclick="atarget(this)" title="シースルーラブ 藤本彩美 HD" class="z">
                DlTool.SaveFile(gethtml, Path.Combine(path1, DlTool.ReplaceUrl(e.Url.ToString()) + ".htm"));
                MatchCollection mc = theadRegex.Matches(gethtml);
                foreach(Match match in mc)
                {
                    string url ="https://youiv.tv/"+ match.Value.Split(new string[] { "<a href=\"", "\"" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    string name = match.Value.Split(new string[] { "title=\"", "\" class=\"z\"" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    

                    AsynObj o = new AsynObj();
                    o.Url = url;
                    o.Path = name;
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
