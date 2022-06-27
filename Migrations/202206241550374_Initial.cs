namespace TaskManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Memberships",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        ProjectId = c.Long(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("public.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("public.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ProjectId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "public.Projects",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 32),
                        Description = c.String(maxLength: 16384),
                        Logo = c.Binary(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        DetetedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "public.Tasks",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 256),
                        Description = c.String(maxLength: 16384),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        DetetedAt = c.DateTime(),
                        ProjectId = c.Long(nullable: false),
                        StatusId = c.Int(nullable: false),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("public.Status", t => t.StatusId, cascadeDelete: true)
                .ForeignKey("public.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.Title, unique: true)
                .Index(t => t.ProjectId)
                .Index(t => t.StatusId)
                .Index(t => t.UserId);
            
            CreateTable(
                "public.Status",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "public.Users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Login = c.String(nullable: false, maxLength: 32),
                        Password = c.String(nullable: false, maxLength: 256),
                        About = c.String(maxLength: 16384),
                        Avatar = c.Binary(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        DetetedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Login, unique: true);
            
            CreateTable(
                "public.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(nullable: false, maxLength: 1024),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true)
                .Index(t => t.Description, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("public.Memberships", "UserId", "public.Users");
            DropForeignKey("public.Memberships", "RoleId", "public.Roles");
            DropForeignKey("public.Memberships", "ProjectId", "public.Projects");
            DropForeignKey("public.Tasks", "UserId", "public.Users");
            DropForeignKey("public.Tasks", "StatusId", "public.Status");
            DropForeignKey("public.Tasks", "ProjectId", "public.Projects");
            DropIndex("public.Roles", new[] { "Description" });
            DropIndex("public.Roles", new[] { "Name" });
            DropIndex("public.Users", new[] { "Login" });
            DropIndex("public.Status", new[] { "Name" });
            DropIndex("public.Tasks", new[] { "UserId" });
            DropIndex("public.Tasks", new[] { "StatusId" });
            DropIndex("public.Tasks", new[] { "ProjectId" });
            DropIndex("public.Tasks", new[] { "Title" });
            DropIndex("public.Projects", new[] { "Name" });
            DropIndex("public.Memberships", new[] { "RoleId" });
            DropIndex("public.Memberships", new[] { "ProjectId" });
            DropIndex("public.Memberships", new[] { "UserId" });
            DropTable("public.Roles");
            DropTable("public.Users");
            DropTable("public.Status");
            DropTable("public.Tasks");
            DropTable("public.Projects");
            DropTable("public.Memberships");
        }
    }
}
