using CefSharp;
using CefSharp.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserDownloader
{
    class RequestHandler : DefaultRequestHandler
    {

        public static bool ifLoadPic = true;
        public override CefReturnValue OnBeforeResourceLoad(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request,
      IRequestCallback callback)
        {
            
            if( request.ResourceType == ResourceType.Image&&!ifLoadPic)
            {
                callback.Dispose();
                return CefReturnValue.Cancel;
            }
            callback.Dispose();
            return CefReturnValue.Continue;
        }

    }
}
