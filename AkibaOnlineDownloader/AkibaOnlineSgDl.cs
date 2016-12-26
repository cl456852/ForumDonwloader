using Framework.interf;
using Framework.tool;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AkibaOnlineDownloader
{
    public class AkibaOnlineSgDl : ISinglePageDonwloader
    {
        Regex threadRegex = new Regex("threads.* data-thumbnailurl");
        Regex nameRegex = new Regex("<title>.*</title>");
        Regex torrentRegex = new Regex("<a href=\"attachments/.*/\" target=\"_blank\">");
        public void Download(object obj)
        {
            AsynObj o = (AsynObj)obj;
            MatchCollection macthes = threadRegex.Matches(o.Content);
            foreach(Match match in macthes)
            {
                string link = match.Value.Replace("/\" data-thumbnailurl", "");
      
                ThreadPool.QueueUserWorkItem(new AkibaOnlineSgDl().work, new AsynObj(o.Path,"https://www.akiba-online.com/" +link));
            }
        }

        public void work(Object obj)
        {
            AsynObj asycObj = (AsynObj)obj;
            try
            {
         
                HttpWebRequest downloadParam = (HttpWebRequest)WebRequest.Create(Config1.EMPTY_URL);
                downloadParam.Host = "www.akiba-online.com";
                string content = NewDlTool.GetHtml(asycObj.Url, true, downloadParam);
                string name = nameRegex.Match(content).Value.Replace("<title>", "").Replace("</title>", "");
                string path = Path.Combine(asycObj.Path, Config1.ValidePath( name) + ".htm");
                MatchCollection mc = torrentRegex.Matches(content);
                ArrayList list = new ArrayList();
                foreach (Match match in mc)
                {
                    string torrentLink = "";
                    if (match.Value.Contains("torrent"))
                    {
                        torrentLink = "https://www.akiba-online.com/" + match.Value.Replace("<a href=\"", "").Replace("\" target=\"_blank\">", "");
                        list.Add(torrentLink);
                    }
                }

                path = Config1.InvalidPathFilter(path);
                foreach (string link in list)
                {
                    NewDlTool.downLoadFile(link, path + ".torrent", true, downloadParam);
                }
                DlTool.SaveFile(content, path);
            }catch(Exception e)
            {
                Console.WriteLine(e.Message + "  " + asycObj.Url);

                Config1.appendFile(asycObj.Url, "d:\\test\\failList.txt");

                
            }

        }
    }
}
