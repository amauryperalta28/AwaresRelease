namespace AwareswebApp.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AwareswebApp.Models.DbModels>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AwareswebApp.Models.DbModels context)
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
            string sql = "CREATE PROCEDURE [dbo].[Procedure](@usuario varchar(20) = '*',	@mes int = 0,	@anio int = 0)AS " + Environment.NewLine +
          "if(@usuario <>  '*' and @mes <> 0 and @anio <> 0)" + Environment.NewLine +
          "Begin" + Environment.NewLine +
          "select UsernameColaborador, sum(lectura)  from Consumoes where month(fechaCreacion) = @mes and  year(fechaCreacion) = @anio and UsernameColaborador =@usuario" + Environment.NewLine + "group by UsernameColaborador" + Environment.NewLine + "end" + Environment.NewLine +
          "else if( @mes <> 0 and @anio <>0)" + Environment.NewLine + "Begin" + Environment.NewLine +
          "select UsernameColaborador, sum(lectura) from Consumoes where month(fechaCreacion) = @mes and year(fechaCreacion) = @anio group by UsernameColaborador" + Environment.NewLine + "end" + Environment.NewLine + "else if(@anio <> 0 )" + Environment.NewLine + "begin" + Environment.NewLine +
          "select UsernameColaborador, sum(lectura) from Consumoes where year(fechaCreacion) = @anio group by UsernameColaborador" + Environment.NewLine +
          "end" + Environment.NewLine + "else if(@anio = 0 )" + Environment.NewLine + "begin" + Environment.NewLine +
          "select UsernameColaborador, sum(lectura) from Consumoes where year(fechaCreacion) =year(GETDATE()) group by usernameColaborador" + Environment.NewLine + "end" + Environment.NewLine;
            context.Database.ExecuteSqlCommand(sql);   
        }
    }
}
