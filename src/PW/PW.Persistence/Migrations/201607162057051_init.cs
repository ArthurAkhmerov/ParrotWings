namespace PW.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "PW.User",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Username = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Salt = c.String(nullable: false),
                        HashedPassword = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Role = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "PW.Transfer",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Amount = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UserFromId = c.Guid(nullable: false),
                        UserToId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("PW.User", t => t.UserFromId)
                .ForeignKey("PW.User", t => t.UserToId)
                .Index(t => t.UserFromId)
                .Index(t => t.UserToId);
            
            CreateTable(
                "PW.Session",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        LastUsage = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("PW.Transfer", "UserToId", "PW.User");
            DropForeignKey("PW.Transfer", "UserFromId", "PW.User");
            DropIndex("PW.Transfer", new[] { "UserToId" });
            DropIndex("PW.Transfer", new[] { "UserFromId" });
            DropTable("PW.Session");
            DropTable("PW.Transfer");
            DropTable("PW.User");
        }
    }
}
