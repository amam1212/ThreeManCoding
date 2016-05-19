namespace WebApplication3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newonew : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "Description", c => c.String());
            AddColumn("dbo.Product", "Retail_Price", c => c.Double(nullable: false));
            AddColumn("dbo.Product", "Wholesale_Price", c => c.Double(nullable: false));
            DropColumn("dbo.Product", "Price");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Product", "Price", c => c.Double(nullable: false));
            DropColumn("dbo.Product", "Wholesale_Price");
            DropColumn("dbo.Product", "Retail_Price");
            DropColumn("dbo.Product", "Description");
        }
    }
}
