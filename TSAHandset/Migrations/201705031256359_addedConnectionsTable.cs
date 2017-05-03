namespace TSAHandset.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedConnectionsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Connections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        PlanId = c.Int(nullable: false),
                        HandsetId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Handsets", t => t.HandsetId, cascadeDelete: true)
                .ForeignKey("dbo.Plans", t => t.PlanId, cascadeDelete: true)
                .Index(t => t.PlanId)
                .Index(t => t.HandsetId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Connections", "PlanId", "dbo.Plans");
            DropForeignKey("dbo.Connections", "HandsetId", "dbo.Handsets");
            DropIndex("dbo.Connections", new[] { "HandsetId" });
            DropIndex("dbo.Connections", new[] { "PlanId" });
            DropTable("dbo.Connections");
        }
    }
}
