using Framework.interf;
using Framework.tool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ThzDownloader
{
    public class ThzSgDl : ISinglePageDonwloader
    {
        Regex nameRegex = new Regex("s xst\">.*</a>");
        Regex nameRegex1 = new Regex(">.*?</a>");
        Regex threadRegex = new Regex("\"thread.*html\"");
        Regex torrentLinkRegex = new Regex("imc_attachad-ad.html\\?aid=.*\"");
        public void Download(object obj)
        {
            AsynObj o = (AsynObj)obj;
            try
            {
                string[] threads = o.Content.Split(new string[] { "新窗口打开" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string thread in threads)
                {
                    if (thread.Contains("桃花族论坛"))
                        continue;
                   
                    string path= Path.Combine(o.Path, DlTool.ReplaceUrl( nameRegex1.Match(nameRegex.Match(thread).Value).Value))+".htm";
                    string link = "http://taohuabt.info/" + threadRegex.Matches(thread)[0].Value.Replace("\"", "");
                    ThreadPool.QueueUserWorkItem(new ThzSgDl().work, new AsynObj(path, link));
                    

                }
            }
            catch (Exception e)
            {
                Config1.appendFile(o.Url, Path.Combine(o.Path, "failList.txt"));
            }
        }

        void work(Object obj)
        {
            AsynObj asycObj = (AsynObj)obj;
            string content = DownloadTool.GetHtml(asycObj.Url, true, Common.CreateHttpWebRequest(asycObj.Url));
            DlTool.SaveFile(content, asycObj.Path);
            MatchCollection torrentMatch= torrentLinkRegex.Matches(content);
            if (torrentMatch.Count > 0)
            {
                string[] strs = torrentMatch[torrentMatch.Count - 1].Value.Split('"');
                string url = "http://taohuabt.info/" + strs[0].Replace("&amp", "");
                string torrentContent = DownloadTool.GetHtml(url, true, Common.CreateHttpWebRequest(url));
                DlTool.SaveFile(torrentContent, asycObj.Path + ".htm");
            }
            else
            {
                Console.WriteLine("没有匹配 " + asycObj.Url);
            }
        }


    }
}
