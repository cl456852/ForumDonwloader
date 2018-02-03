using Framework.interf;
using Framework.tool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace JavBusDownloader
{
    public class JavBusSgDl : ISinglePageDonwloader
    {
        Regex regex = new Regex("https://www.javbus.com/.*?\">");
        Regex gidRegex = new Regex("var gid = .*?;");
        Regex imgRegex = new Regex("var img = '.*?;");
        public void Download(object obj)
        {
            AsynObj o = (AsynObj)obj;
            string[] threads1 = o.Content.Split(new string[] { "<div class=\"item\">" }, StringSplitOptions.RemoveEmptyEntries);
            foreach(string content in threads1)
            {
                if (content.Contains("movie-box"))
                {
                    Match macth = regex.Match(content);
                    string link = macth.Value.Replace("\">", "");
                    string vid = link.Replace("https://www.javbus.com/", "");
                    ThreadPool.QueueUserWorkItem(new JavBusSgDl().work, new AsynObj(Path.Combine( o.Path, Path.Combine(vid+".htm")), link));
                }
            }
        }

        void work(Object obj)
        {
            AsynObj asycObj = (AsynObj)obj;
            HttpWebRequest downloadParam = (HttpWebRequest)WebRequest.Create(Config1.EMPTY_URL);
            downloadParam.Host = "www.akiba-online.com";
            string content = NewDlTool.GetHtml(asycObj.Url, JavBusUtils.GenerateRequestParam());
            DlTool.SaveFile(content, asycObj.Path);
            string gid = gidRegex.Match(content).Value.Replace("var gid = ","").Replace(";","");
            string img = imgRegex.Match(content).Value.Replace("var img = '", "").Replace("';", "");
            string link = "https://www.javbus.com/ajax/uncledatoolsbyajax.php?gid=" + gid + "&lang=zh&img=" + img + "&uc=0&floor=900";
            string magnetContent = NewDlTool.GetHtml(link, JavBusUtils.GenerateRequestParam());
            DlTool.SaveFile(magnetContent, asycObj.Path + "_magenet");
        }
    }
}
