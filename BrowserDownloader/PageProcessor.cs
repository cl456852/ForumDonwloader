
using CefSharp.Example.Handlers;
using CefSharp.WinForms;
using Framework.tool;
using RarbgDownloader;
using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BrowserDownloader
{
    public class PageProcessor: IPageProcessor
    {
        Regex regex = new Regex("href=\"/torrent/.*?\"");
        Regex torrentRegex = new Regex(@"download.php\?id=.*?\.torrent");
        Regex genresRegex = new Regex(@"Genres.*</a>");
        Regex genresRegex1 = new Regex("search=.*?\"");
        Regex releaseDateRegex = new Regex("\"releaseDate\">.*</td></tr>");

        ChromiumWebBrowser chromeBrowser;


        public void NavigateHandle(ChromiumWebBrowser chromeBrowser, string url , string path, string html)
        {
            this.chromeBrowser = chromeBrowser;
            if(String.IsNullOrEmpty(html))
            {
                return;
            }
            if(url.Contains("https://rarbgprx.org/torrents.php?r="))
            {
                Console.WriteLine("mainPage");
                AsynObj asynObj1 = Config1.BlockingQueue.Peek();
                chromeBrowser.Load(asynObj1.Url);
                return;
            }
            string content = html;
            if (content.Contains("Please wait while we try to verify your browser"))
            {
                Console.WriteLine("Please wait while we try to verify your browser");
                //chromeBrowser.Navigate(url + i);
                return;
            }
            if (content.Contains("detected abnormal "))
                return;
            if (content.Contains("We have too many requests from your ip in the past 24h"))
            {
                Console.WriteLine("We have too many requests from your ip in the past 24h");
                Config1.Flooding();
                chromeBrowser.Load(url);
                return;
            }
            if (content.Contains("There is something wrong with your browser"))
            {
                Console.WriteLine("There is something wrong with your browser");
                AsynObj asynObj1 = Config1.BlockingQueue.Peek();
                chromeBrowser.Load(asynObj1.Url);
                return;
            }
            if (html.Contains("torrents.php?r="))
            {
                Console.WriteLine("first Redirecting");
                chromeBrowser.Load("https://rarbg.to/torrents.php?category=1%3B4&page=" + path);
                return;
            }
            if (content.Contains("无法显示此页"))
            {
                Console.WriteLine("无法显示此页");
                AsynObj asynObj1 = Config1.BlockingQueue.Peek();
                chromeBrowser.Load(asynObj1.Url);
                return;
            }
            Config1.BlockingQueue.Dequeue();
            Process(url.ToString(), html, path);
            AsynObj asynObj = Config1.BlockingQueue.Peek();
            DownloadHandler.asynObj = asynObj;
            chromeBrowser.Load(asynObj.Url);

        }

        public void Process(string url, string html, string path)
        {

            if (url.Contains("page="))
            {
                var content = html.Split(new string[] { "<div id=\"pager_links\">Pages:" }, StringSplitOptions.RemoveEmptyEntries)[1];
                DlTool.SaveFile(content, Path.Combine(path, Config1.ValidePath(url) + ".htm"));

                var mc = regex.Matches(content);
                foreach (Match m in mc)
                    if (!m.Value.Contains("#comments") &&
                        !DlConfig.storage.Contains(m.Value.Replace("href=\"/torrent/", "").Replace("\"", "")))
                    {
                        AsynObj asynObj = new AsynObj();
                        asynObj.Url = "https://rarbg.to" + m.Value.Replace("href=", "").Replace("\"", "");
                        Config1.BlockingQueue.Enqueue(asynObj);
                    }
            }
            else if (url.Contains("torrent"))
            {
                Work(html,path);
            }
        }


        private void Work(string content, string path)
        {
            string genreStr = "";
            MatchCollection genresMatches;
            string url = "https://rarbgprx.org/" + torrentRegex.Match(content).Value;
            Match genres = genresRegex.Match(content);
            if (genres != null && genres.Value != "")
            {
                genresMatches = genresRegex1.Matches(genres.Value);
                foreach (Match m in genresMatches)
                    genreStr += m.Value.Replace("search=", "").Replace("\"", "").ToLower().Replace("+", " ") + ",";
            }
            if (!Check2(url.Substring(url.LastIndexOf('=') + 1).ToLower()))
            {
                path =
                    Path.Combine(path, "notok", genreStr + "$$" + url.Substring(url.LastIndexOf('=') + 1))
                        .Replace("%22", "");
            }
            else if (check1(url.Substring(url.LastIndexOf('=') + 1).ToLower()))
            {
                path = Path.Combine(path, genreStr + "$$" + url.Substring(url.LastIndexOf('=') + 1)).Replace("%22", "");
                //DlTool.downLoadFile(url, path, false, content, Config1.Cookie);
            }

            else
            {
                path = Path.Combine(path, "unknown", url.Substring(url.LastIndexOf('=') + 1)).Replace("%22", "");
            }
            path = path.Replace("%20", " ").Replace("%2C", " ");
            if (File.Exists(path))
            {
                path = Path.Combine(Path.GetDirectoryName(path), "duplicateName", Path.GetFileNameWithoutExtension(path) + "(" + System.Guid.NewGuid().ToString().Substring(0, 4) + ").torrent");
                Console.WriteLine("duplicate filename: " + path);
            }
            url=url.Replace("amp;", "");
            DlTool.SaveFile(content, path + ".html");
            AsynObj asynObj = new AsynObj(path, url);
            Config1.BlockingQueue.Enqueue(asynObj);
        }


        private bool Check2(string name)
        {
            string[] okname = DlConfig.demo4["notokname"].ToString().Split(',');
            foreach (string s in okname)
                if (name.Contains(s.ToLower()))
                    return false;
            return true;
        }

        private bool check1(string name)
        {
            string[] okname = DlConfig.demo4["okname"].ToString().Split(',');
            foreach (string s in okname)
                if (name.Contains(s.ToLower()))
                    return true;
            return false;
        }

        private int Check(string genreStr)
        {
            string[] notoks = DlConfig.demo4["notok"].ToString().Split(',');
            string[] oks = DlConfig.demo4["ok"].ToString().Split(',');
            string[] genres = genreStr.Split(',');
            foreach (string s in genres)
                if (((IList) notoks).Contains(s))
                {
                    return -1;
                }
            foreach (string s in genres)
                if (((IList) oks).Contains(s))
                {
                    return 1;
                }
            foreach (string s in genres)
            {
                if (DlConfig.demo4.Contains(s))
                {
                    string[] strs = DlConfig.demo4[s].ToString().Split(',');
                    foreach (string s1 in genres)
                        if (((IList) strs).Contains(s1))
                        {
                            return -1;
                        }
                    return 1;
                }
            }
            return 0;
        }
    }
}
