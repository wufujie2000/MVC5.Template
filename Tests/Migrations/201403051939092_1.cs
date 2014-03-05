namespace Template.Tests.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Username = c.String(nullable: false),
                        Passhash = c.String(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        DateOfBirth = c.DateTime(),
                        RoleId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RolePrivileges",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        PrivilegeId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Privileges", t => t.PrivilegeId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.PrivilegeId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Privileges",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Area = c.String(),
                        Controller = c.String(nullable: false),
                        Action = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PrivilegeLanguages",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Area = c.String(),
                        Controller = c.String(nullable: false),
                        Action = c.String(nullable: false),
                        PrivilegeId = c.String(nullable: false, maxLength: 128),
                        LanguageId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Languages", t => t.LanguageId, cascadeDelete: true)
                .ForeignKey("dbo.Privileges", t => t.PrivilegeId, cascadeDelete: true)
                .Index(t => t.LanguageId)
                .Index(t => t.PrivilegeId);
            
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Abbreviation = c.String(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TestModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accounts", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RolePrivileges", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RolePrivileges", "PrivilegeId", "dbo.Privileges");
            DropForeignKey("dbo.PrivilegeLanguages", "PrivilegeId", "dbo.Privileges");
            DropForeignKey("dbo.PrivilegeLanguages", "LanguageId", "dbo.Languages");
            DropIndex("dbo.Accounts", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.RolePrivileges", new[] { "RoleId" });
            DropIndex("dbo.RolePrivileges", new[] { "PrivilegeId" });
            DropIndex("dbo.PrivilegeLanguages", new[] { "PrivilegeId" });
            DropIndex("dbo.PrivilegeLanguages", new[] { "LanguageId" });
            DropTable("dbo.TestModels");
            DropTable("dbo.Languages");
            DropTable("dbo.PrivilegeLanguages");
            DropTable("dbo.Privileges");
            DropTable("dbo.RolePrivileges");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.Accounts");
        }
    }
}
