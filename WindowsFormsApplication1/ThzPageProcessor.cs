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
    class ThzPageProcessor : IPageProcessor
    {

        Regex nameRegex = new Regex("s xst\">.*</a>");
        Regex nameRegex1 = new Regex(">.*?</a>");
        Regex threadRegex = new Regex("\"thread.*html\"");
        Regex torrentLinkRegex = new Regex("imc_attachad-ad.html\\?aid=.*\"");

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
                string path = Config1.dictionary[webBrowser1.Url.ToString()].Path;
                DlTool.SaveFile(gethtml, path);

                MatchCollection torrentMatch = torrentLinkRegex.Matches(gethtml);
                if (torrentMatch.Count > 0)
                {
                    string[] strs = torrentMatch[torrentMatch.Count - 1].Value.Split('"');
                    string url = "http://taohuabt.cc/" + strs[0].Replace("&amp", "");
                    AsynObj o = new AsynObj();
                    o.Url = url;
                    o.Path = path+ ".htm";
                    Config1.dictionary.Add(url, o);
                    Config1.BlockingQueue.Enqueue(o);
                }
            }
            else if (webBrowser1.Url.ToString().Contains("forum"))
            {
                    DlTool.SaveFile(gethtml, Path.Combine(path1, DlTool.ReplaceUrl(e.Url.ToString()) + ".htm"));
                    try
                    {
                        string[] threads = gethtml.Split(new string[] { "新窗口打开" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string thread in threads)
                        {
                            if (thread.Contains("桃花族论坛"))
                                continue;

                            string path = Path.Combine(path1, DlTool.ReplaceUrl(nameRegex1.Match(nameRegex.Match(thread).Value).Value)).Replace(">","").Replace("</a","") + ".htm";
                            string link = "http://taohuabt.cc/" + threadRegex.Matches(thread)[0].Value.Replace("\"", "");

                            AsynObj o = new AsynObj();
                            o.Url = link;
                            o.Path = path;
                            Config1.dictionary.Add(link, o);
                            Config1.BlockingQueue.Enqueue(o);
                        }
                    }
                    catch (Exception ex)
                    {
                        Config1.appendFile(webBrowser1.Url.ToString() +ex.Message, Path.Combine(path1, "failList.txt"));
                    }


                
            } 
            else 
            {
                string path = Config1.dictionary[webBrowser1.Url.ToString()].Path;
                DlTool.SaveFile(gethtml, path);
            }

            AsynObj asynObj1 = Config1.BlockingQueue.Peek();
            webBrowser1.Navigate(asynObj1.Url);

        }
    }
}
