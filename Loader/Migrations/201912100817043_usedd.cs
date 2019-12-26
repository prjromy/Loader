namespace Loader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usedd : DbMigration
    {
        public override void Up()
        {
            AddColumn("LG.User", "UserDesignationId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("LG.User", "UserDesignationId");
        }
    }
}
