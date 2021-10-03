using System;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace KVVKWeb.API
{
    public class GetFromAPI : IGetFromAPI
    {
        private T GetResult<T>(RestClient client, RestRequest request, object obj = null, Dictionary<string, string> headers = null)
        {
            if (headers != null) //header varsa requeste headerları ekle
            {
                foreach (var header in headers)
                {
                    request.AddHeader(header.Key, header.Value);
                }
            }

            if (obj != null) //post,put,delete gibi işlemler için servise gönderilecek nesne varsa requeste ekle
            {
                request.AddJsonBody(obj);
            }
            //client üzerinden requesti servise yolla ve
            System.Net.ServicePointManager.ServerCertificateValidationCallback = SSLCertNonVerificationHandler;//ssl validate bypass
                                                                                                               // request.AddParameter("client", att.Customerid);
            var response = client.Execute<T>(request);

            return JsonConvert.DeserializeObject<T>(response.Content, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

        }

        public T GetMethod<T>(string uri, Dictionary<string, string> headers = null)
        {
            var client = new RestClient(uri);
            var request = new RestRequest(Method.GET) { RequestFormat = DataFormat.Json };

            var result = GetResult<T>(client, request, null, headers);

            return result;
        }

        public T PostMethod<T>(object obj, string uri, Dictionary<string, string> headers = null)
        {
            var client = new RestClient(uri);
            var request = new RestRequest(Method.POST) { RequestFormat = DataFormat.Json };

            var result = GetResult<T>(client, request, obj, headers);

            return result;
        }

        public bool SSLCertNonVerificationHandler(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors error)//ssl validate bypass
        {
            return true;
        }
    }
}
