namespace TSAHandset.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedRequestUserNameFieldToTheRequestModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "RequestUserName", c => c.String(nullable: false));
            AlterColumn("dbo.Requests", "SecurityGroupId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Requests", "SecurityGroupId", c => c.String());
            DropColumn("dbo.Requests", "RequestUserName");
        }
    }
}
