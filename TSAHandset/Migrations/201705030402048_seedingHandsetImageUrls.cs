namespace TSAHandset.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seedingHandsetImageUrls : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE Handsets SET ImageUrl='http://cdn2.gsmarena.com/vv/bigpic/lg-nexus-5x-.jpg' WHERE Id=1");
            Sql("UPDATE Handsets SET ImageUrl='http://cdn2.gsmarena.com/vv/bigpic/motorola-moto-g4-.jpg' WHERE Id=2");
        }
        
        public override void Down()
        {
        }
    }
}
