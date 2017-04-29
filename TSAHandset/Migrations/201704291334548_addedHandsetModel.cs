namespace TSAHandset.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedHandsetModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Handsets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Make = c.String(),
                        Model = c.String(),
                        WidthInMM = c.Single(nullable: false),
                        HeightInMM = c.Single(nullable: false),
                        ThicknessInMM = c.Single(nullable: false),
                        ScreenSizeInIn = c.Single(nullable: false),
                        MemoryInGB = c.Byte(nullable: false),
                        StorageInGB = c.Byte(nullable: false),
                        CameraInMP = c.Single(nullable: false),
                        BatteryInMAH = c.Short(nullable: false),
                        Processor = c.String(),
                        OS = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Handsets");
        }
    }
}
