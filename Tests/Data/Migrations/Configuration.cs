namespace MvcTemplate.Tests.Data.Migrations
{
    using MvcTemplate.Tests.Data;
    using System.Data.Entity.Migrations;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    internal sealed class Configuration : DbMigrationsConfiguration<TestingContext>
    {
        public Configuration()
        {
            MigrationsDirectory = "Data\\Migrations";
            ContextKey = "MvcTemplate.Tests.Data";
        }
    }
}
