namespace TSAHandset.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedHandsetImageUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Handsets", "ImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Handsets", "ImageUrl");
        }
    }
}
