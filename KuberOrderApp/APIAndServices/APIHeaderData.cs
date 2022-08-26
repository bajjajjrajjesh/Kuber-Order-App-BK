using System;
namespace KuberOrderApp.APIAndServices
{
    public class APIHeaderData
    {
        public string strHeaderName;

        public string strHeaderValue;
        public APIHeaderData(string HeaderName, string HeaderValue)
        {
            strHeaderName = HeaderName;
            strHeaderValue = HeaderValue;

        }
    }
}
