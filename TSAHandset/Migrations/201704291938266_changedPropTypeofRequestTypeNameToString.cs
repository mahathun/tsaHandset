namespace TSAHandset.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedPropTypeofRequestTypeNameToString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RequestTypes", "RequestTypeName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RequestTypes", "RequestTypeName", c => c.Byte(nullable: false));
        }
    }
}
