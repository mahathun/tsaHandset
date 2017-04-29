namespace TSAHandset.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seedingHandsets : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Handsets " +
                "(Make, Model, WidthInMM, HeightInMM, ThicknessInMM, ScreenSizeInIn, MemoryInGB, StorageInGB, CameraInMP, BatteryInMAH, Processor, OS ) VALUES" +
                "('LG', 'Nexus 5X', 147, 72.6, 7.9, 5.2, 2, 16, 12, 2700, 'Hexa-core Snapdragon 808', 'Android 6.0')");
            Sql("INSERT INTO Handsets " +
               "(Make, Model, WidthInMM, HeightInMM, ThicknessInMM, ScreenSizeInIn, MemoryInGB, StorageInGB, CameraInMP, BatteryInMAH, Processor, OS ) VALUES" +
               "('Motorola', 'Moto G4', 153, 76.6, 9.8, 5.5, 2, 16, 13, 3000, 'Octa-core Snapdragon 617', 'Android 6.0.1')");
        }
        
        public override void Down()
        {
        }
    }
}
