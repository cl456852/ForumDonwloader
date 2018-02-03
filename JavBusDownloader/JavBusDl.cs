using Framework.abs;
using Framework.BO;
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


            string content = NewDlTool.GetHtml(o.Url, JavBusUtils.GenerateRequestParam());
            if (content != "")
            {
                if (o.Path != null)
                {
                    DlTool.SaveFile(content, Path.Combine(o.Path,DlTool.ReplaceUrl( o.Url)+".html"));

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
