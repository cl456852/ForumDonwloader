using Framework.abs;
using Framework.interf;
using Framework.tool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThzDownloader
{
    public class ThzLstDl : AbsLstDl
    {
        public override void Download(object obj)
        {
            AsynObj o = (AsynObj)obj;


            string content = DownloadTool.GetHtml(o.Url, false,Common.CreateHttpWebRequest(o.Url));
            if (content != "")
            {
                //string[] contents = content.Split(new string[] { "<td align=\"center\">" }, StringSplitOptions.RemoveEmptyEntries);
                //content = contents[1];
                if (o.Path != null)
                {
                    DlTool.SaveFile(content, Path.Combine(o.Path, DlTool.ReplaceUrl(o.Url)) + ".htm");

                }
                ISinglePageDonwloader sgDl = new ThzSgDl();
                AsynObj asynObj = new AsynObj();
                asynObj.Path = o.Path;
                asynObj.Content = content;
                ThreadPool.QueueUserWorkItem(sgDl.Download, asynObj);
            }


        }
    }
}
