namespace TSAHandset.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedRequestUserNameFieldToTheRequestModel2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Requests", "SecurityGroupId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Requests", "SecurityGroupId", c => c.String(nullable: false));
        }
    }
}
