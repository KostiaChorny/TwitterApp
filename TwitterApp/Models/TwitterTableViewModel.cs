using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterApp.Models
{
    public class TwitterTableViewModel
    {
        public User User { get; set; }
        public List<Tweet> Tweets { get; set; }
        public int CurrentPage { get; set; }
        public int PagesCount { get; set; }
        public string Search { get; set; }
    }
}