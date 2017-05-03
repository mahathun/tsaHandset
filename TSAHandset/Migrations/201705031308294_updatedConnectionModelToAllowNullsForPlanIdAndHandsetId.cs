namespace TSAHandset.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedConnectionModelToAllowNullsForPlanIdAndHandsetId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Connections", "HandsetId", "dbo.Handsets");
            DropForeignKey("dbo.Connections", "PlanId", "dbo.Plans");
            DropIndex("dbo.Connections", new[] { "PlanId" });
            DropIndex("dbo.Connections", new[] { "HandsetId" });
            AlterColumn("dbo.Connections", "PlanId", c => c.Int());
            AlterColumn("dbo.Connections", "HandsetId", c => c.Int());
            CreateIndex("dbo.Connections", "PlanId");
            CreateIndex("dbo.Connections", "HandsetId");
            AddForeignKey("dbo.Connections", "HandsetId", "dbo.Handsets", "Id");
            AddForeignKey("dbo.Connections", "PlanId", "dbo.Plans", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Connections", "PlanId", "dbo.Plans");
            DropForeignKey("dbo.Connections", "HandsetId", "dbo.Handsets");
            DropIndex("dbo.Connections", new[] { "HandsetId" });
            DropIndex("dbo.Connections", new[] { "PlanId" });
            AlterColumn("dbo.Connections", "HandsetId", c => c.Int(nullable: false));
            AlterColumn("dbo.Connections", "PlanId", c => c.Int(nullable: false));
            CreateIndex("dbo.Connections", "HandsetId");
            CreateIndex("dbo.Connections", "PlanId");
            AddForeignKey("dbo.Connections", "PlanId", "dbo.Plans", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Connections", "HandsetId", "dbo.Handsets", "Id", cascadeDelete: true);
        }
    }
}
