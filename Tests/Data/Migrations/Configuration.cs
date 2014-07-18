namespace MvcTemplate.Tests.Data.Migrations
{
    using MvcTemplate.Tests.Data;
    using MvcTemplate.Tests.Objects;
    using System;
    using System.Data.Entity.Migrations;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    internal sealed class Configuration : DbMigrationsConfiguration<TestingContext>
    {
        public Configuration()
        {
            MigrationsDirectory = "Data\\Migrations";
            AutomaticMigrationsEnabled = true;
            ContextKey = "MvcTemplate.Tests.Data";
        }

        protected override void Seed(TestingContext context)
        {
            for (Int32 i = 1; i <= 10; i++)
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
