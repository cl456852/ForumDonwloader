using System.Net;

namespace Framework.BO
{
    public class RequestParam
    {
        public CookieContainer Container { get;  set; }

        public string Referer { get;  set; }

        public string Host { get;  set; }

        public bool IsUseProxy { get; set; }

        public string Cookie { get;  set; }
    }
}