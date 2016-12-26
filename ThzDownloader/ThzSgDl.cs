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
        Regex threadRegex = new Regex("\"thread.*\" style=");
        Regex torrentLinkRegex = new Regex("imc_attachad-ad.html\\?aid=.*&amp");
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
                   
                    string path= Path.Combine(o.Path, nameRegex1.Match(nameRegex.Match(thread).Value).Value.Replace(">", "").Replace("</a", "").Replace('/', '_').Replace(":", "^").Replace("?", "wenhao"))+".htm";
                    string link = "http://thzbt.biz/" + threadRegex.Match(thread).Value.Replace("\"", "").Replace("style=", "");
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
            string content = Sis001DlTool.GetHtml(asycObj.Url, true, "UTF-8");
            DlTool.SaveFile(content, asycObj.Path);
            Match torrentMatch= torrentLinkRegex.Match(content);
            string torrentContent = Sis001DlTool.GetHtml("http://thzbt.biz/"+torrentMatch.Value.Replace("&amp",""), true, "UTF-8");
            DlTool.SaveFile(torrentContent, asycObj.Path + ".htm");
        }


    }
}
