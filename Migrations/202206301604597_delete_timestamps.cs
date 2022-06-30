namespace TaskManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class delete_timestamps : DbMigration
    {
        public override void Up()
        {
            DropColumn("public.Projects", "CreatedAt");
            DropColumn("public.Projects", "UpdatedAt");
            DropColumn("public.Projects", "DetetedAt");
            DropColumn("public.Tasks", "CreatedAt");
            DropColumn("public.Tasks", "UpdatedAt");
            DropColumn("public.Tasks", "DetetedAt");
            DropColumn("public.Users", "CreatedAt");
            DropColumn("public.Users", "UpdatedAt");
            DropColumn("public.Users", "DetetedAt");
        }
        
        public override void Down()
        {
            AddColumn("public.Users", "DetetedAt", c => c.DateTime());
            AddColumn("public.Users", "UpdatedAt", c => c.DateTime());
            AddColumn("public.Users", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("public.Tasks", "DetetedAt", c => c.DateTime());
            AddColumn("public.Tasks", "UpdatedAt", c => c.DateTime());
            AddColumn("public.Tasks", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("public.Projects", "DetetedAt", c => c.DateTime());
            AddColumn("public.Projects", "UpdatedAt", c => c.DateTime());
            AddColumn("public.Projects", "CreatedAt", c => c.DateTime(nullable: false));
        }
    }
}
