namespace WebApplication3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newtitle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "NewsTitle", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "NewsTitle");
        }
    }
}
