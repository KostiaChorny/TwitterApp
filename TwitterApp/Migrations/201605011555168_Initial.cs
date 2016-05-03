namespace TwitterApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tweets",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        Created = c.DateTime(nullable: false, precision: 0),
                        Text = c.String(unicode: true, storeType: "LONGBLOB"),
                        Deleted = c.Boolean(nullable: false),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Long(nullable: false),
                        FullName = c.String(unicode: false),
                        UserName = c.String(unicode: false),
                        ImageUrl = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tweets", "UserId", "dbo.Users");
            DropIndex("dbo.Tweets", new[] { "UserId" });
            DropTable("dbo.Users");
            DropTable("dbo.Tweets");
        }
    }
}
