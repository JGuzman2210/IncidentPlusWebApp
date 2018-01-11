namespace IncidentPlus.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteUserTypePropertyFromUserEntity : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "UserType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "UserType", c => c.Int(nullable: false));
        }
    }
}
