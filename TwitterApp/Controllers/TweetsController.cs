using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TwitterApp.Models;
using TwitterApp.Models.Data;
using TwitterApp.Models.TwitterHelpers;

namespace TwitterApp.Controllers
{
    public class TweetsController : Controller
    {
        private AppContext db = new AppContext();

        // GET: Tweets
        public ActionResult Index(string username, int page = 1, string search = null)
        {
            if (username == null) throw new ArgumentNullException("username", "You must specify user name!");
            if (username == String.Empty) throw new ArgumentException("Username is empty", "username");

            var user = db.Users.SingleOrDefault(u => u.UserName == username);

            List<Tweet> tweets = TweetsHelper.GetTweets(page, user.UserId, search);

            if (user != null)
            {
                Session["TwitterTableData"] = new TwitterTableViewModel
                {
                    User = user,
                    CurrentPage = page,
                    Tweets = tweets,
                    PagesCount = TweetsHelper.GetPagesCount(page, user.UserId, search),
                    Search = search
                };
                return View();
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult IndexPost(int page = 1, string search = null)
        {
            var data = Session["TwitterTableData"] as TwitterTableViewModel;
            if (search != null)
            {
                data.Search = search;
            }
            var user = db.Users.SingleOrDefault(u => u.UserName == data.User.UserName);
            List<Tweet> tweets = TweetsHelper.GetTweets(page, user.UserId, search);
            if (!string.IsNullOrEmpty(data.Search))
            {
                tweets = tweets.Where(t => t.Text.Contains(data.Search)).ToList();
            }             
            if (user != null)
            {
                data.CurrentPage = page;
                data.Tweets = tweets;
                data.PagesCount = TweetsHelper.GetPagesCount(page, user.UserId, search);
                return PartialView("TweetsTable");
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult Pull(string username)
        {
            if (username == null) throw new ArgumentNullException("username", "You must specify user name!");
            if (username == String.Empty) throw new ArgumentException("Username is empty", "username");

            var apiKey = ConfigurationManager.AppSettings["api_key"];
            var apiSecret = ConfigurationManager.AppSettings["api_secret"];
            var token = new TwitterAuthToken(apiKey, apiSecret);
            var downloader = new TweetDownloader(token.Token);

            var tweets = downloader.GetTweetsFor(username);
            db.Tweets.Load();
            foreach (var tweet in tweets)
            {
                if (db.Tweets.SingleOrDefault(t => t.Id == tweet.Id) == null)
                {
                    db.Tweets.Add(tweet);
                }
            }
            db.SaveChanges();
            var user = db.Users.SingleOrDefault(u => u.UserName == username);
            tweets = TweetsHelper.GetTweets(1, user.UserId, "");
            Session["TwitterTableData"] = new TwitterTableViewModel
            {
                User = user,
                CurrentPage = 1,
                Tweets = tweets,
                PagesCount = TweetsHelper.GetPagesCount(1, user.UserId, ""),
                Search = null
            };
            return PartialView("TweetsTable");
        }

        [HttpPost]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tweet tweet = db.Tweets.Find(id);
            if (tweet == null)
            {
                return HttpNotFound();
            }
            tweet.Deleted = true;
            db.SaveChanges();
            var data = Session["TwitterTableData"] as TwitterTableViewModel;
            return IndexPost(data.CurrentPage);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }




    }
}
