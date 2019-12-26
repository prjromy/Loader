namespace Loader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class used : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.ApplicationUser1");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ApplicationUser1",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserDesignationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
