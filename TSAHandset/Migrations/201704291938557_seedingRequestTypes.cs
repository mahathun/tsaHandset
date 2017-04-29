namespace TSAHandset.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seedingRequestTypes : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO RequestTypes (Id, RequestTypeName) VALUES (1, 'Mobile Connection Only')");
            Sql("INSERT INTO RequestTypes (Id, RequestTypeName) VALUES (2, 'Mobile Handset Only')");
            Sql("INSERT INTO RequestTypes (Id, RequestTypeName) VALUES (3, 'Mobile Handset and Connection')");
        }
        
        public override void Down()
        {
        }
    }
}
