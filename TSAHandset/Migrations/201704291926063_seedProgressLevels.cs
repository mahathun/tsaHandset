namespace TSAHandset.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seedProgressLevels : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO ProgressLevels (Id,ProgressLevelName) VALUES (1, 'Pending')");
            Sql("INSERT INTO ProgressLevels (Id, ProgressLevelName) VALUES (2, 'Approved')");
            Sql("INSERT INTO ProgressLevels (Id, ProgressLevelName) VALUES (3, 'Rejected')");
            Sql("INSERT INTO ProgressLevels (Id, ProgressLevelName) VALUES (4, 'Completed')");
        }
        
        public override void Down()
        {
        }
    }
}
