using CefSharp.WinForms;
using Framework.tool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
//http://www.1080fhd.com/forum-2-{0}.html?mod=forumdisplay&fid=2&forumdefstyle=yes
namespace BrowserDownloader
{
    
    class _456k : IPageProcessor
    {
        Regex listRegex = new Regex("href=\"thread.*? onclick=");
        Regex nameRegex = new Regex("deanforumtitname\">.*?</a>");
        public void NavigateHandle(ChromiumWebBrowser webBrowser1, string url, string path1, string html)
        {
            Console.WriteLine(url);
            if(html.Contains("The server is temporarily unable"))
            {
                Console.WriteLine("The server is temporarily unable");
                AsynObj asynObj2 = Config1.BlockingQueue.Peek();
                webBrowser1.Load(asynObj2.Url);
                return;
            }
            if (url.ToString().Contains("thread"))
            {
                string path = Config1.dictionary[url.ToString()].Path;
                DlTool.SaveFile(html, path);
            }
            else
            {
                DlTool.SaveFile(html, Path.Combine(path1, DlTool.ReplaceUrl(url) + ".htm"));

                string[] str = html.Split(new string[] { "normalthread_" },StringSplitOptions.None);
                foreach (string threadString  in str)
                {
                    if(threadString.Contains("<!DOCTYPE html PUBLIC "))
                    {
                        continue;
                    }
                    if (threadString.Contains("showMenu"))
                    {
                        string list = listRegex.Match(threadString).Value;

                        list = list.Split('\"')[1];
                        string threadUrl = "http://www.1080fhd.com/" + list;
                        string list1 = list.Split('-')[1];
                        string name = nameRegex.Match(threadString).Value.Replace("deanforumtitname\">", "").Replace("</a>", "").Replace("/", "#");
                        string path = Path.Combine(path1, DlTool.ReplaceUrl(name) + ".htm");
                        AsynObj o = new AsynObj();
                        o.Url = threadUrl;
                        o.Path = path;
                        if (!Config1.dictionary.ContainsKey(threadUrl))
                        {
                            Config1.dictionary.Add(threadUrl, o);
                            Config1.dictionary.Add("http://www.1080fhd.com/forum.php?mod=viewthread&tid=" + list1,o);
                        }
                        Config1.BlockingQueue.Enqueue(o);
                    }
                }
            }

            Config1.BlockingQueue.Dequeue();
            AsynObj asynObj1 = Config1.BlockingQueue.Peek();
            webBrowser1.Load(asynObj1.Url);


        }

    }
}