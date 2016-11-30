namespace VGrad_Empty.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "ProjectDescription", c => c.String(nullable: false));
            AddColumn("dbo.Projects", "SupervisorName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "SupervisorName");
            DropColumn("dbo.Projects", "ProjectDescription");
        }
    }
}
