namespace IncidentPlus.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsDefaultPropertyToEntityLevel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Levels", "DefaultLevel", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Levels", "DefaultLevel");
        }
    }
}
