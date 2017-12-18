namespace IncidentPlus.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCategoryEntityAndUniqueNameProject : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
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
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: false)
                .Index(t => t.ProjectID);
            
            CreateIndex("dbo.Projects", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Categories", "ProjectID", "dbo.Projects");
            DropIndex("dbo.Projects", new[] { "Name" });
            DropIndex("dbo.Categories", new[] { "ProjectID" });
            DropTable("dbo.Categories");
        }
    }
}
