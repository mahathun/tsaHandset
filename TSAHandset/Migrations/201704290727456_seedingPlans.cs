namespace TSAHandset.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seedingPlans : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Plans (Name, MonthlyCharge, MonthlyDataAllowanceInGB, MonthlyMinuteAllowance, MonthlyTextAllowance, AccountCalling) VALUES ('Basic', 35, 1, 300, -1, 0)");
            Sql("INSERT INTO Plans (Name, MonthlyCharge, MonthlyDataAllowanceInGB, MonthlyMinuteAllowance, MonthlyTextAllowance, AccountCalling) VALUES ('High', 44, 4, -1, -1, 0)");
        }
        
        public override void Down()
        {
        }
    }
}
