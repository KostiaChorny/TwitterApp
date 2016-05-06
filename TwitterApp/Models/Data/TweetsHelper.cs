using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace TwitterApp.Models.Data
{
    public class TweetsHelper
    {
        public static List<Tweet> GetTweets(int page, long userid, string search = "")
        {
            search = search ?? String.Empty;
            List<Tweet> result = new List<Tweet>();
            var connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string rtn = "getTweets";
                using (MySqlCommand cmd = new MySqlCommand(rtn, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@pagenum", page);
                    cmd.Parameters.AddWithValue("@search", search);
                    cmd.Parameters.AddWithValue("@userid", userid);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var tweet = new Tweet();
                            tweet.Id = rdr.GetInt64(0);
                            tweet.Created = rdr.GetDateTime(1);
                            tweet.Text = rdr.GetString(2);
                            tweet.Deleted = rdr.GetBoolean(3);
                            tweet.UserId = rdr.GetInt64(4);
                            result.Add(tweet);
                        }
                    }
                }
            }

            return result;
        }

        public static int GetPagesCount(int page, long userid, string search = "")
        {
            search = search ?? String.Empty;
            int result = 0;
            var connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string rtn = "getPagesCount";
                using (MySqlCommand cmd = new MySqlCommand(rtn, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@pagenum", page);
                    cmd.Parameters.AddWithValue("@search", search);
                    cmd.Parameters.AddWithValue("@userid", userid);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            result = rdr.GetInt32(0);
                        }
                    }
                }
            }

            return result;
        }
    }
}