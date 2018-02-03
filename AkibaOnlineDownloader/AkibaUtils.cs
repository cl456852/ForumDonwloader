using Framework.BO;

namespace AkibaOnlineDownloader
{
    public class AkibaUtils
    {
        public static RequestParam GenerateRequestParam()
        {
            RequestParam requestParam=new RequestParam();
            requestParam.Host = "www.akiba-online.com";
            return requestParam;
        }
    }
}