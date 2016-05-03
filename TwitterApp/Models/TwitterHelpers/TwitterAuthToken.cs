using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace TwitterApp.Models.TwitterHelpers
{
    public class TwitterAuthToken
    {
        public string Key { get; private set; }
        public string Secret { get; private set; }

        private string token;

        public string Token
        {
            get
            {
                if (token == null)
                {
                    token = GetToken();
                }
                return token;
            }
        }


        public TwitterAuthToken(string key, string secret)
        {
            Key = key;
            Secret = secret;
        }

        private string GetBearerTokenCredentials()
        {
            var credentials = Key + ":" + Secret;
            var bytes = Encoding.UTF8.GetBytes(credentials);
            var result = Convert.ToBase64String(bytes);
            return result;
        }

        private string GetToken()
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(@"https://api.twitter.com/oauth2/token");
            request.ContentType = @"application/x-www-form-urlencoded;charset=UTF-8";
            request.Headers[HttpRequestHeader.Authorization] = "Basic " + GetBearerTokenCredentials();
            request.Method = "POST";
            var rstream = request.GetRequestStream();
            var bytes = Encoding.UTF8.GetBytes("grant_type=client_credentials");
            rstream.Write(bytes, 0, bytes.Length);

            var responce = request.GetResponse();
            var respstream = new StreamReader(responce.GetResponseStream());
            var resptext = respstream.ReadToEnd();

            var serializer = new JavaScriptSerializer();
            dynamic obj = serializer.DeserializeObject(resptext);
            var result = obj["access_token"];           

            return result;
        }
    }
}