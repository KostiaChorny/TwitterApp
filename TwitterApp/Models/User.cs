using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TwitterApp.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long UserId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string ImageUrl { get; set; }
        public virtual ICollection<Tweet> Tweets { get; set; } = new List<Tweet>();
    }
}