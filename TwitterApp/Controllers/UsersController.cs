using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwitterApp.Models.Data;
using TwitterApp.Models.TwitterHelpers;

namespace TwitterApp.Controllers
{
    public class UsersController : Controller
    {
        // POST: AddUser
        [HttpPost]
        public ActionResult Add(string username)
        {
            if (username == null) throw new ArgumentNullException("username", "You must specify user name!");
            if (username == String.Empty) throw new ArgumentException("Username is empty", "username");

            var apiKey = ConfigurationManager.AppSettings["api_key"];
            var apiSecret = ConfigurationManager.AppSettings["api_secret"];
            var token = new TwitterAuthToken(apiKey, apiSecret);
            var downloader = new TweetDownloader(token.Token);
            var user = downloader.GetUser(username);

            var context = new AppContext();
            var dbuser = context.Users.SingleOrDefault(u => u.UserId == user.UserId);
            if (dbuser == null)
            {
                dbuser = user;
                context.Users.Add(user);
                context.SaveChanges();
            }

            return RedirectToAction("Index", "Tweets", new { username = dbuser.UserName });
        }
    }
}