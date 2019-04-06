using Framework.BO;

namespace JavBusDownloader
{
    public class JavBusUtils
    {
        public static RequestParam GenerateRequestParam()
        {
            RequestParam requestParam = new RequestParam();
            requestParam.IsUseProxy = true;
            requestParam.Cookie =
                "_ga=GA1.2.985626025.1465997438; __cfduid=d480f9adf3ba5ae4d8e7cebbcc7b5732d1484758115; HstCfa2807330=1484757907182; starinfo=glyphicon%20glyphicon-plus; HstCmu2807330=1500644059935; PHPSESSID=6aokse02psa6slmo7tjkct0nc6; HstCla2807330=1501308668831; HstPn2807330=15; HstPt2807330=16; HstCnv2807330=2; HstCns2807330=3; __dtsu=1EE70445D2F20655280AF41D02A69922; existmag=mag";
            requestParam.Host = "www.javbus.com";
            requestParam.Referer = "https://www.javbus.com/genre/62/4";
            return requestParam;
        }
    }
}