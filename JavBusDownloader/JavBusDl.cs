using Framework.abs;
using Framework.tool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JavBusDownloader
{
    public class JavBusDl : AbsLstDl
    {

        public override void Download(object obj)
        {
            AsynObj o = (AsynObj)obj;


            string content = Sis001DlTool.GetHtml(o.Url, true, "GB2312");
            if (content != "")
            {
                if (o.Path != null)
                {
                    DlTool.SaveFile(content, Path.Combine(o.Path, o.Url.Replace('/', '_').Replace(":", "^").Replace("?", "wenhao")) + ".htm");

                }
                JavBusSgDl sgDl = new JavBusSgDl();
                AsynObj asynObj = new AsynObj();
                asynObj.Path = o.Path;
                asynObj.Content = content;
                ThreadPool.QueueUserWorkItem(sgDl.Download, asynObj);
            }
        }
    }
}
