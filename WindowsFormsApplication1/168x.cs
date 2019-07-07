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
    public class _168xProcessor : IPageProcessor
    {
        Regex nameRegex = new Regex("s xst\">.*</a>");
        Regex threadRegex = new Regex("\"thread.*html\"");
        public void NavigateHandle(System.Windows.Forms.WebBrowser webBrowser1, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e, string path1)
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
                string path="";
                if (Config1.dictionary.ContainsKey(webBrowser1.Url.ToString()))
                {
                     path = Config1.dictionary[webBrowser1.Url.ToString()].Path;
                }
                else
                {
                    path =Path.Combine( path1, Config1.getGUID() + ".html");
                }
                DlTool.SaveFile(gethtml, path);

            }
            else if (webBrowser1.Url.ToString().Contains("forum"))
            {
                DlTool.SaveFile(gethtml, Path.Combine(path1, DlTool.ReplaceUrl(e.Url.ToString()) + ".htm"));
                try
                {
                    string[] threads = gethtml.Split(new string[] { "新窗口打开" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string thread in threads)
                    {
                        if (thread.Contains("<!DOCTYPE html PUBLIC"))
                            continue;

                        string path = Path.Combine(path1, DlTool.ReplaceUrl(nameRegex.Match(thread).Value.Replace("s xst\">", "").Replace("</a>",""))) + ".htm";
                        string link =Util.domain + threadRegex.Matches(thread)[0].Value.Replace("\"", "");
                        //if (link.Contains("thread-12088-1-1")||link.Contains("thread-62622-1-1"))
                        //    continue;
                        AsynObj o = new AsynObj();
                        o.Url = link;
                        o.Path = path;
                        if (!Config1.dictionary.Keys.Contains(link))
                        {
                            Config1.dictionary.Add(link, o);
                            Config1.BlockingQueue.Enqueue(o);
                        }
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
