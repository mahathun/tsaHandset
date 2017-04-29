namespace TSAHandset.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedPlansTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Plans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        MonthlyCharge = c.Short(nullable: false),
                        MonthlyDataAllowanceInGB = c.Byte(nullable: false),
                        MonthlyMinuteAllowance = c.Short(nullable: false),
                        MonthlyTextAllowance = c.Short(nullable: false),
                        AccountCalling = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Plans");
        }
    }
}
