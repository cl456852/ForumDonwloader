using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Framework.tool;
using RarbgDownloader;

namespace WindowsFormsApplication1
{
    public class PageProcessor
    {
        Regex regex = new Regex("href=\"/torrent/.*?\"");
        Regex torrentRegex = new Regex(@"download.php\?id=.*?\.torrent");
        Regex genresRegex = new Regex(@"Genres.*</a>");
        Regex genresRegex1 = new Regex("search=.*?\"");
        Regex releaseDateRegex = new Regex("\"releaseDate\">.*</td></tr>");

        public void Process(string url, WebBrowser webBrowser, string path)
        {
            var regex = new Regex("href=\"/torrent/.*?\"");
            var content = webBrowser.DocumentText;
            if (url.Contains("page="))
            {
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
                Work(webBrowser.DocumentText,path);
            }
        }


        private void Work(string content, string path)
        {
            string genreStr = "";
            MatchCollection genresMatches;
            string dateString =
                releaseDateRegex.Match(content).Value.Replace("\"releaseDate\">", "").Replace("</td></tr>", "");
            DateTime releaseDate = Convert.ToDateTime(dateString);
            string url = "https://rarbg.to/" + torrentRegex.Match(content).Value;
            Match genres = genresRegex.Match(content);
            if (genres != null && genres.Value != "")
            {
                genresMatches = genresRegex1.Matches(genres.Value);
                foreach (Match m in genresMatches)
                    genreStr += m.Value.Replace("search=", "").Replace("\"", "").ToLower().Replace("+", " ") + ",";
            }
            if (check1(url.Substring(url.LastIndexOf('=') + 1).ToLower()))
            {
                path = Path.Combine(path, genreStr + "$$" + url.Substring(url.LastIndexOf('=') + 1)).Replace("%22", "");
            }
            else if (!Check2(url.Substring(url.LastIndexOf('=') + 1).ToLower()))
            {
                path =
                    Path.Combine(path, "notok", genreStr + "$$" + url.Substring(url.LastIndexOf('=') + 1))
                        .Replace("%22", "");
            }
            else if (releaseDate.CompareTo(new DateTime(2014, 8, 1)) < 0)
            {
                if (genres != null && genres.Value != "")
                {
                    int res = Check(genreStr.Substring(0, genreStr.Length - 1).Replace("%22", ""));
                    if (res == 1)
                    {
                        path =
                            Path.Combine(path, genreStr + "$$" + url.Substring(url.LastIndexOf('=') + 1))
                                .Replace("%22", "");
                    }
                    else if (res == -1)
                    {
                        path =
                            Path.Combine(path, "notok", genreStr + "$$" + url.Substring(url.LastIndexOf('=') + 1))
                                .Replace("%22", "");
                    }
                    else
                    {
                        path =
                            Path.Combine(path, "unknown", genreStr + "$$" + url.Substring(url.LastIndexOf('=') + 1))
                                .Replace("%22", "");
                    }
                }
                else
                {
                    path = Path.Combine(path, "unknown", url.Substring(url.LastIndexOf('=') + 1)).Replace("%22", "");
                }
            }
            else
            {
                path = Path.Combine(path, "unknown", url.Substring(url.LastIndexOf('=') + 1)).Replace("%22", "");
            }
            path = path.Replace("%20", " ").Replace("%2C", " ");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (File.Exists(path))
            {
                path = Path.Combine(Path.GetDirectoryName(path), "duplicateName", Path.GetFileNameWithoutExtension(path) + "(" + System.Guid.NewGuid().ToString().Substring(0, 4) + ").torrent");
                Console.WriteLine("duplicate filename: " + path);
            }
            DlTool.downLoadFile(url,path,false,content,Config1.Cookie);
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
