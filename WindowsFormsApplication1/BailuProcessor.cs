using Framework.tool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{

    public class BailuProcessor : IPageProcessor
    {
        Regex nameRegex = new Regex("atarget\\(this\\)\" title=\".*\">");
        Regex threadRegex = new Regex("href=\".*\" style=\"font-weight");
        public void NavigateHandle(WebBrowser webBrowser1, WebBrowserDocumentCompletedEventArgs e, string path1)
        {
            System.IO.StreamReader getReader = new System.IO.StreamReader(webBrowser1.DocumentStream);
            string gethtml = getReader.ReadToEnd();
            Console.WriteLine(e.Url);
            if (gethtml.Contains("500 Internal Privoxy Error"))
            {
                Console.WriteLine("500 Internal Privoxy Error TRY AGAIN");
                webBrowser1.Navigate(e.Url);
                return;
            }
            Config1.BlockingQueue.Dequeue();

            if (webBrowser1.Url.ToString().Contains("thread"))
            {
                string path = Config1.dictionary[webBrowser1.Url.ToString()].Path;
                DlTool.SaveFile(gethtml, path);

            }
            else if (webBrowser1.Url.ToString().Contains("forum"))
            {
                DlTool.SaveFile(gethtml, Path.Combine(path1, DlTool.ReplaceUrl(e.Url.ToString()) + ".htm"));
                try
                {
                    string[] threads = gethtml.Split(new string[] { "喜欢:" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string thread in threads)
                    {
  
                        Console.WriteLine(thread);
                        string path = Path.Combine(path1, DlTool.ReplaceUrl(nameRegex.Match(thread).Value.Replace("atarget(this)\" title=\"", "").Replace("\">", ""))) + ".htm";
                        MatchCollection mc = threadRegex.Matches(thread);
                        if (mc.Count == 0)
                            continue;
                        string link = Util.domain + mc[0].Value.Replace("href=\"", "").Replace("\" style=\"font-weight","");
                        AsynObj o = new AsynObj();
                        o.Url = link;
                        o.Path = path;
                        Config1.dictionary.Add(link, o);
                        Config1.BlockingQueue.Enqueue(o);
                    }
                }
                catch (Exception ex)
                {
                    Config1.appendFile(webBrowser1.Url.ToString() + ex.Message, Path.Combine(path1, "failList.txt"));
                }



            }
            AsynObj asynObj1 = Config1.BlockingQueue.Peek();
            webBrowser1.Navigate(asynObj1.Url);
        }
    }
}
