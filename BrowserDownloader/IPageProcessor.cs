using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrowserDownloader
{
    interface IPageProcessor
    {
        void NavigateHandle(ChromiumWebBrowser chromeBrowser, string url, string path, string html); 
    }
}
