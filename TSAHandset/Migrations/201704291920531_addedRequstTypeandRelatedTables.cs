namespace TSAHandset.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedRequstTypeandRelatedTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequestUserId = c.String(),
                        SecurityGroupId = c.String(),
                        PlanId = c.Int(nullable: false),
                        HandsetId = c.Int(nullable: false),
                        RequestDate = c.DateTime(nullable: false),
                        ApprovedDate = c.DateTime(nullable: false),
                        CompletedDate = c.DateTime(nullable: false),
                        Notification = c.Int(nullable: false),
                        ProgressId = c.Byte(nullable: false),
                        RequestTypeId = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Handsets", t => t.HandsetId, cascadeDelete: true)
                .ForeignKey("dbo.Plans", t => t.PlanId, cascadeDelete: true)
                .ForeignKey("dbo.ProgressLevels", t => t.ProgressId, cascadeDelete: true)
                .ForeignKey("dbo.RequestTypes", t => t.RequestTypeId, cascadeDelete: true)
                .Index(t => t.PlanId)
                .Index(t => t.HandsetId)
                .Index(t => t.ProgressId)
                .Index(t => t.RequestTypeId);
            
            CreateTable(
                "dbo.ProgressLevels",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        ProgressLevelName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RequestTypes",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        RequestTypeName = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "RequestTypeId", "dbo.RequestTypes");
            DropForeignKey("dbo.Requests", "ProgressId", "dbo.ProgressLevels");
            DropForeignKey("dbo.Requests", "PlanId", "dbo.Plans");
            DropForeignKey("dbo.Requests", "HandsetId", "dbo.Handsets");
            DropIndex("dbo.Requests", new[] { "RequestTypeId" });
            DropIndex("dbo.Requests", new[] { "ProgressId" });
            DropIndex("dbo.Requests", new[] { "HandsetId" });
            DropIndex("dbo.Requests", new[] { "PlanId" });
            DropTable("dbo.RequestTypes");
            DropTable("dbo.ProgressLevels");
            DropTable("dbo.Requests");
        }
    }
}
