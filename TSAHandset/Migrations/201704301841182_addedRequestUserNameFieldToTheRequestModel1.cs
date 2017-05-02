namespace TSAHandset.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedRequestUserNameFieldToTheRequestModel1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Requests", "RequestUserName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Requests", "RequestUserName", c => c.String(nullable: false));
        }
    }
}
