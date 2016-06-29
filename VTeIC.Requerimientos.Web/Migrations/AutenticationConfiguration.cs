namespace VTeIC.Requerimientos.Web.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class AutenticationConfiguration : DbMigrationsConfiguration<VTeIC.Requerimientos.Web.Models.ApplicationDbContext>
    {
        public AutenticationConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "VTeIC.Requerimientos.Web.Models.ApplicationDbContext";
        }

        protected override void Seed(VTeIC.Requerimientos.Web.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
