namespace MvcTemplate.Tests.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Username = c.String(nullable: false, maxLength: 128),
                        Passhash = c.String(nullable: false, maxLength: 512),
                        Email = c.String(nullable: false, maxLength: 256),
                        IsLocked = c.Boolean(nullable: false),
                        RecoveryToken = c.String(maxLength: 128),
                        RecoveryTokenExpirationDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        RoleId = c.String(maxLength: 128),
                        CreationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id, clustered: false)
                .ForeignKey("dbo.Role", t => t.RoleId)
                .Index(t => t.Username, unique: true)
                .Index(t => t.Email, unique: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(nullable: false, maxLength: 128),
                        CreationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id, clustered: false)
                .Index(t => t.Title, unique: true);
            
            CreateTable(
                "dbo.RolePermission",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                        PermissionId = c.String(nullable: false, maxLength: 128),
                        CreationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id, clustered: false)
                .ForeignKey("dbo.Permission", t => t.PermissionId)
                .ForeignKey("dbo.Role", t => t.RoleId)
                .Index(t => t.RoleId)
                .Index(t => t.PermissionId);
            
            CreateTable(
                "dbo.Permission",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Area = c.String(maxLength: 128),
                        Controller = c.String(nullable: false, maxLength: 128),
                        Action = c.String(nullable: false, maxLength: 128),
                        CreationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id, clustered: false);
            
            CreateTable(
                "dbo.AuditLog",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AccountId = c.String(maxLength: 128),
                        Action = c.String(nullable: false, maxLength: 128),
                        EntityName = c.String(nullable: false, maxLength: 128),
                        EntityId = c.String(nullable: false, maxLength: 128),
                        Changes = c.String(nullable: false),
                        CreationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id, clustered: false);
            
            CreateTable(
                "dbo.TestModel",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Text = c.String(maxLength: 512),
                        CreationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id, clustered: false);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Account", "RoleId", "dbo.Role");
            DropForeignKey("dbo.RolePermission", "RoleId", "dbo.Role");
            DropForeignKey("dbo.RolePermission", "PermissionId", "dbo.Permission");
            DropIndex("dbo.RolePermission", new[] { "PermissionId" });
            DropIndex("dbo.RolePermission", new[] { "RoleId" });
            DropIndex("dbo.Role", new[] { "Title" });
            DropIndex("dbo.Account", new[] { "RoleId" });
            DropIndex("dbo.Account", new[] { "Email" });
            DropIndex("dbo.Account", new[] { "Username" });
            DropTable("dbo.TestModel");
            DropTable("dbo.AuditLog");
            DropTable("dbo.Permission");
            DropTable("dbo.RolePermission");
            DropTable("dbo.Role");
            DropTable("dbo.Account");
        }
    }
}
