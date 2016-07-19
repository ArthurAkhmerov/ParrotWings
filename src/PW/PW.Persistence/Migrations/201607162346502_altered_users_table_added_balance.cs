namespace PW.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class altered_users_table_added_balance : DbMigration
    {
        public override void Up()
        {
            AddColumn("PW.User", "Balance", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("PW.User", "Balance");
        }
    }
}
