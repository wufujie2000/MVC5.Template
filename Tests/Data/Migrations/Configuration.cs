namespace Template.Tests.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using Template.Tests.Data;

    internal sealed class Configuration : DbMigrationsConfiguration<TestContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Template.Tests.Data";
        }
    }
}
