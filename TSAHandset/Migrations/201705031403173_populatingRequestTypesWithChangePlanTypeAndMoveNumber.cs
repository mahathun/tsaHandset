namespace TSAHandset.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class populatingRequestTypesWithChangePlanTypeAndMoveNumber : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO RequestTypes (Id, RequestTypeName) VALUES (4, 'Change current TSA Plan')");
            Sql("INSERT INTO RequestTypes (Id, RequestTypeName) VALUES (5, 'Move existing number to a TSA Plan')");
            Sql("INSERT INTO RequestTypes (Id, RequestTypeName) VALUES (6, 'Move number from a TSA Plan to a Personal Plan')");
        }
        
        public override void Down()
        {
        }
    }
}
