namespace Template.Tests.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Diagnostics.CodeAnalysis;
    using Template.Tests.Data;
    using Template.Tests.Objects.Components.Services;

    [ExcludeFromCodeCoverage]
    internal sealed class Configuration : DbMigrationsConfiguration<TestingContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Template.Tests.Data";
        }

        protected override void Seed(TestingContext context)
        {
            for (Int32 i = 1; i <= 100; i++)
                if (context.Set<TestModel>().Find(i.ToString()) == null)
                    context.Set<TestModel>().Add(new TestModel()
                    {
                        Id = i.ToString(),
                        Text = "Text" + i.ToString()
                    });

            context.SaveChanges();
        }
    }
}
