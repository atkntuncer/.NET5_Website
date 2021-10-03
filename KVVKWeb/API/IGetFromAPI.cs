using System.Collections.Generic;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace KVVKWeb.API
{
    public interface IGetFromAPI
    {
        T GetMethod<T>(string uri, Dictionary<string, string> headers = null);
        T PostMethod<T>(object obj, string uri, Dictionary<string, string> headers = null);
        bool SSLCertNonVerificationHandler(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors error);
    }
}