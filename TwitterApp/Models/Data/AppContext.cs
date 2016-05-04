namespace TwitterApp.Models.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class AppContext : DbContext
    {
        // Your context has been configured to use a 'AppContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'TwitterApp.Models.Data.AppContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'AppContext' 
        // connection string in the application configuration file.
        public AppContext()
            : base("name=AppContext")
            //: base("server=eu-cdbr-azure-west-d.cloudapp.net;user id=b71dda2a6c908b;password=54fccd18586a68d;persistsecurityinfo=True;database=twitterAmB34pKED")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Tweet> Tweets { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
