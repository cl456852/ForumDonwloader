using Framework.tool;
using RarbgDownloader;
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
    class SIS001PageProcessor: IPageProcessor
    {
        Regex nameRegex = new Regex("color.*?</a>");
        Regex nameRegex1 = new Regex(">.*?</a>");
        Regex threadRegex = new Regex("href=\"thread.*?\"><");
        Regex noColorNameRegex = new Regex("html\">.*?</a>");
        Regex sizeRegex = new Regex("<td class=\"nums\">.*?/");
        Regex regex = new Regex("thread-.*</a></span>");
        public void NavigateHandle(System.Windows.Forms.WebBrowser webBrowser1, WebBrowserDocumentCompletedEventArgs e, string path1)
        {
            System.IO.StreamReader getReader = new System.IO.StreamReader(webBrowser1.DocumentStream, System.Text.Encoding.GetEncoding("gb2312"));
            string gethtml = getReader.ReadToEnd();

            if (gethtml.Contains("500 Internal Privoxy Error")||gethtml.Contains("<title>无法访问此页</title>")||gethtml.Contains("<title>代理服务器没有响应</title>")||gethtml.Contains("<BODY></BODY>")||gethtml.Contains("Can not connect to MySQL server"))
            {
                Console.WriteLine("500 Internal Privoxy Error TRY AGAIN");
                webBrowser1.Navigate(e.Url);
                return;
            }

            Config1.BlockingQueue.Dequeue();
            if (webBrowser1.Url.ToString().Contains("thread"))
            {
                DlTool.SaveFile(gethtml, Config1.dictionary[webBrowser1.Url.ToString()].Path);
            }
            else
            {
               
                try
                {
                    DlTool.SaveFile(gethtml,Path.Combine(path1, DlTool.ReplaceUrl(e.Url.ToString())+".htm"));
                    string[] threads1;
                    if (gethtml.Contains("版块主题"))
                    {
                        threads1 = gethtml.Split(new string[] { "版块主题" }, StringSplitOptions.RemoveEmptyEntries);
                    }
                    else
                    {
                        threads1 = gethtml.Split(new string[] { "推荐主题" }, StringSplitOptions.RemoveEmptyEntries);

                    }
                    string[] threads = threads1[1].Split(new string[] { "normalthread_", "pages_btns" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string thread in threads)
                    {
                        try
                        {
                            if (thread.Contains("新窗口打开"))
                            {
                                string path;
                                if (thread.Contains("color:"))
                                {
                                    path = Path.Combine(path1, DlTool.ReplaceUrl(nameRegex1.Match(nameRegex.Match(thread).Value).Value));

                                }
                                else
                                {
                                    path = Path.Combine(path1, DlTool.ReplaceUrl(nameRegex1.Match(noColorNameRegex.Match(thread).Value).Value));

                                }
                                double size = 0;
                                try
                                {
                                    string sizeStr = sizeRegex.Matches(thread)[1].Value.Replace("<td class=\"nums\">", "").Replace(" /", "").ToUpper();
                                    string sizeStrWithoutUnit = sizeStr.Replace("GB", "").Replace("G", "").Replace("MB", "").Replace("M", "");
                                    size = Convert.ToDouble(sizeStrWithoutUnit);
                                    if (sizeStr.Contains("G"))
                                        size = size * 1024;
                                }
                                catch
                                {
                                    Console.WriteLine("Can not get Size:  " + thread);
                                }
                                path += " size^^^" + size + ".htm";

                                string link = "http://sis001.com/bbs/" + threadRegex.Match(thread).Value.Replace("href=\"", "").Replace("\" title=\"新窗口打开\" target=\"_blank\"><", "");

                                AsynObj o = new AsynObj();
                                o.Url = link;
                                o.Path = path;
                                Config1.dictionary.Add(link, o);
                                Config1.BlockingQueue.Enqueue(o);
                            }

                        } 
                        catch(Exception e2)
                        {
                            Config1.appendFile(thread, Path.Combine(path1, "failList.txt"));
                            Config1.appendFile(e2.StackTrace, Path.Combine(path1, "failList.txt"));
                        }
                    }
                }
                catch (Exception e1)
                {
                    Config1.appendFile(webBrowser1.Url.ToString(), Path.Combine(path1, "failList.txt"));
                    Config1.appendFile(e1.StackTrace, Path.Combine(path1, "failList.txt"));
                }
            }

            AsynObj asynObj1 = Config1.BlockingQueue.Peek();
            webBrowser1.Navigate(asynObj1.Url);
        }
    }
}
