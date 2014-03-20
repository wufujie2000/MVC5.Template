namespace Template.Tests.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
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
                        PersonId = c.String(nullable: false, maxLength: 128),
                        EntityDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id, clustered: false)
                .ForeignKey("dbo.People", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        DateOfBirth = c.DateTime(),
                        RoleId = c.String(maxLength: 128),
                        EntityDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id, clustered: false)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        EntityDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id, clustered: false);
            
            CreateTable(
                "dbo.RolePrivileges",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        PrivilegeId = c.String(nullable: false, maxLength: 128),
                        EntityDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id, clustered: false)
                .ForeignKey("dbo.Privileges", t => t.PrivilegeId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.PrivilegeId);
            
            CreateTable(
                "dbo.Privileges",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Area = c.String(),
                        Controller = c.String(nullable: false),
                        Action = c.String(nullable: false),
                        EntityDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id, clustered: false);
            
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Abbreviation = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        EntityDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id, clustered: false);
            
            CreateTable(
                "dbo.TestModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Text = c.String(),
                        EntityDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id, clustered: false);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accounts", "PersonId", "dbo.People");
            DropForeignKey("dbo.People", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RolePrivileges", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RolePrivileges", "PrivilegeId", "dbo.Privileges");
            DropIndex("dbo.RolePrivileges", new[] { "PrivilegeId" });
            DropIndex("dbo.RolePrivileges", new[] { "RoleId" });
            DropIndex("dbo.People", new[] { "RoleId" });
            DropIndex("dbo.Accounts", new[] { "PersonId" });
            DropTable("dbo.TestModels");
            DropTable("dbo.Languages");
            DropTable("dbo.Privileges");
            DropTable("dbo.RolePrivileges");
            DropTable("dbo.Roles");
            DropTable("dbo.People");
            DropTable("dbo.Accounts");
        }
    }
}
