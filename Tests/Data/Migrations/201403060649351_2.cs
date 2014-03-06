namespace Template.Tests.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TestModels", "Text", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TestModels", "Text");
        }
    }
}
