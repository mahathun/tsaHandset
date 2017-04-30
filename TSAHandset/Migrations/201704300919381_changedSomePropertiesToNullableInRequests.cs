namespace TSAHandset.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedSomePropertiesToNullableInRequests : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requests", "HandsetId", "dbo.Handsets");
            DropForeignKey("dbo.Requests", "PlanId", "dbo.Plans");
            DropIndex("dbo.Requests", new[] { "PlanId" });
            DropIndex("dbo.Requests", new[] { "HandsetId" });
            AlterColumn("dbo.Requests", "PlanId", c => c.Int());
            AlterColumn("dbo.Requests", "HandsetId", c => c.Int());
            AlterColumn("dbo.Requests", "ApprovedDate", c => c.DateTime());
            AlterColumn("dbo.Requests", "CompletedDate", c => c.DateTime());
            CreateIndex("dbo.Requests", "PlanId");
            CreateIndex("dbo.Requests", "HandsetId");
            AddForeignKey("dbo.Requests", "HandsetId", "dbo.Handsets", "Id");
            AddForeignKey("dbo.Requests", "PlanId", "dbo.Plans", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "PlanId", "dbo.Plans");
            DropForeignKey("dbo.Requests", "HandsetId", "dbo.Handsets");
            DropIndex("dbo.Requests", new[] { "HandsetId" });
            DropIndex("dbo.Requests", new[] { "PlanId" });
            AlterColumn("dbo.Requests", "CompletedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Requests", "ApprovedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Requests", "HandsetId", c => c.Int(nullable: false));
            AlterColumn("dbo.Requests", "PlanId", c => c.Int(nullable: false));
            CreateIndex("dbo.Requests", "HandsetId");
            CreateIndex("dbo.Requests", "PlanId");
            AddForeignKey("dbo.Requests", "PlanId", "dbo.Plans", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Requests", "HandsetId", "dbo.Handsets", "Id", cascadeDelete: true);
        }
    }
}
