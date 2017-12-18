namespace IncidentPlus.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLevelEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Levels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 100),
                        ProjectID = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Update = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.ProjectID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Levels", "ProjectID", "dbo.Projects");
            DropIndex("dbo.Levels", new[] { "ProjectID" });
            DropTable("dbo.Levels");
        }
    }
}
