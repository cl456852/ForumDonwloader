using Framework.abs;
using Framework.tool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AkibaOnlineDownloader
{
    public class AkibaOnlineDl : AbsLstDl
    {
        public override void Download(object obj)
        {
            AsynObj o = (AsynObj)obj;

            HttpWebRequest downloadParam = (HttpWebRequest)WebRequest.Create(Config1.EMPTY_URL);
            downloadParam.Host = "www.akiba-online.com";
            string content = NewDlTool.GetHtml(o.Url, true, downloadParam);
            if (content != "")
            {
                if (o.Path != null)
                {
                    DlTool.SaveFile(content, Path.Combine(o.Path, o.Url.Replace('/', '_').Replace(":", "^").Replace("?", "wenhao")) + ".htm");

                }
                AkibaOnlineSgDl sgDl = new AkibaOnlineSgDl();
                AsynObj asynObj = new AsynObj();
                asynObj.Path = o.Path;
                asynObj.Content = content;
                ThreadPool.QueueUserWorkItem(sgDl.Download, asynObj);
            }
        }
    }
}
