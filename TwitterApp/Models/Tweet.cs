using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TwitterApp.Models
{
    public class Tweet
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        public DateTime Created { get; set; }
        [DataType("LONGBLOB")]
        public string Text { get; set; }
        public bool Deleted { get; set; }
        public long UserId { get; set; }
        public virtual User User { get; set; }
    }
}