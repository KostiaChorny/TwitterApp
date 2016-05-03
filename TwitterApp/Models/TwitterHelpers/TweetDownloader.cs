using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace TwitterApp.Models.TwitterHelpers
{
    public class TweetDownloader
    {
        public string Token { get; private set; }

        public TweetDownloader(string token)
        {
            Token = token;
        }

        public List<Tweet> GetTweetsFor(string user, int count = 100)
        {
            var json = GetTweetsJson(user, count);

            var serializer = new JavaScriptSerializer();
            dynamic obj = serializer.DeserializeObject(json);

            var result = new List<Tweet>();

            foreach (var texttweet in obj)
            {
                var newTweet = new Tweet
                {
                    Id = texttweet["id"],                                //"Sat Apr 30 08:18:29 +0000 2016"
                    Created = DateTime.ParseExact(texttweet["created_at"], "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.GetCultureInfo("en-US")),
                    Text = texttweet["text"],
                    UserId = texttweet["user"]["id"]
                };
                result.Add(newTweet);
            }

            return result;
        }

        public User GetUser(string name)
        {
            var json = GetUserJson(name);

            var serializer = new JavaScriptSerializer();
            dynamic obj = serializer.DeserializeObject(json);

            var user = new User
            {
                FullName = obj["name"],
                UserName = obj["screen_name"],
                UserId = obj["id"],
                ImageUrl = obj["profile_image_url"]
            };

            return user;
        }

        private string GetUserJson(string name)
        {
            var requestStr = $"https://api.twitter.com/1.1/users/show.json?screen_name={name}";
            string result = MakeRequest(requestStr);
            return result;
        }

        private string GetTweetsJson(string user, int count)
        {
            var requestStr = $"https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name={user}&count={count}";
            string result = MakeRequest(requestStr);
            return result;
        }

        private string MakeRequest(string requestStr)
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(requestStr);
            request.Headers[HttpRequestHeader.Authorization] = "Bearer " + Token;

            var response = request.GetResponse();
            var responseStream = new StreamReader(response.GetResponseStream());
            var result = responseStream.ReadToEnd();
            return result;
        }
    }
}