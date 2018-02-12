using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    interface IPageProcessor
    {
        void NavigateHandle(WebBrowser webBrowser1, WebBrowserDocumentCompletedEventArgs e, string path); 
    }
}
