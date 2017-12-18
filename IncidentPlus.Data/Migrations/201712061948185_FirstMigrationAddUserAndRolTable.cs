namespace IncidentPlus.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigrationAddUserAndRolTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        Description = c.String(maxLength: 100),
                        Created = c.DateTime(nullable: false),
                        Update = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 60),
                        Email = c.String(nullable: false, maxLength: 60),
                        UserName = c.String(nullable: false, maxLength: 60),
                        Password = c.String(nullable: false, maxLength: 100),
                        UserType = c.Int(nullable: false),
                        RolID = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Update = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RolID, cascadeDelete: true)
                .Index(t => t.UserName, unique: true)
                .Index(t => t.RolID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "RolID", "dbo.Roles");
            DropIndex("dbo.Users", new[] { "RolID" });
            DropIndex("dbo.Users", new[] { "UserName" });
            DropTable("dbo.Users");
            DropTable("dbo.Roles");
        }
    }
}
