namespace AwareswebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Colaboradors",
                c => new
                    {
                        idColaborador = c.Int(nullable: false, identity: true),
                        nombreUsuario = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        sector = c.String(),
                        localidad = c.String(),
                        tipoUsuario = c.String(),
                        fechaCreacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.idColaborador);
            
            CreateTable(
                "dbo.Consumoes",
                c => new
                    {
                        idConsumo = c.Int(nullable: false, identity: true),
                        UsernameColaborador = c.String(),
                        tipoConsumo = c.String(),
                        lectura = c.Double(nullable: false),
                        fechaCreacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.idConsumo);
            
            CreateTable(
                "dbo.Reportes",
                c => new
                    {
                        numReporte = c.Int(nullable: false, identity: true),
                        numReporteUsr = c.Int(nullable: false),
                        userName = c.String(),
                        Descripcion = c.String(),
                        situacion = c.String(),
                        pais = c.String(),
                        localidad = c.String(),
                        sector = c.String(),
                        calle = c.String(),
                        longitud = c.Double(nullable: false),
                        latitud = c.Double(nullable: false),
                        FotoUrl = c.String(),
                        Comentarios = c.String(),
                        estatus = c.String(),
                        fechaCreacion = c.DateTime(nullable: false),
                        fechaCorreccion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.numReporte);
            
            CreateTable(
                "dbo.Reportes_Ruta",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        numRuta = c.Int(nullable: false),
                        numReporte = c.Int(nullable: false),
                        fechaCreacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.RutaHs",
                c => new
                    {
                        Rutaid = c.Int(nullable: false, identity: true),
                        usuario = c.String(),
                        status = c.String(),
                    })
                .PrimaryKey(t => t.Rutaid);
            
            CreateStoredProcedure(
                "dbo.Reporte_Insert",
                p => new
                    {
                        numReporteUsr = p.Int(),
                        userName = p.String(),
                        Descripcion = p.String(),
                        situacion = p.String(),
                        pais = p.String(),
                        localidad = p.String(),
                        sector = p.String(),
                        calle = p.String(),
                        longitud = p.Double(),
                        latitud = p.Double(),
                        FotoUrl = p.String(),
                        Comentarios = p.String(),
                        estatus = p.String(),
                        fechaCreacion = p.DateTime(),
                        fechaCorreccion = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[Reportes]([numReporteUsr], [userName], [Descripcion], [situacion], [pais], [localidad], [sector], [calle], [longitud], [latitud], [FotoUrl], [Comentarios], [estatus], [fechaCreacion], [fechaCorreccion])
                      VALUES (@numReporteUsr, @userName, @Descripcion, @situacion, @pais, @localidad, @sector, @calle, @longitud, @latitud, @FotoUrl, @Comentarios, @estatus, @fechaCreacion, @fechaCorreccion)
                      
                      DECLARE @numReporte int
                      SELECT @numReporte = [numReporte]
                      FROM [dbo].[Reportes]
                      WHERE @@ROWCOUNT > 0 AND [numReporte] = scope_identity()
                      
                      SELECT t0.[numReporte]
                      FROM [dbo].[Reportes] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[numReporte] = @numReporte"
            );
            
            CreateStoredProcedure(
                "dbo.Reporte_Update",
                p => new
                    {
                        numReporte = p.Int(),
                        numReporteUsr = p.Int(),
                        userName = p.String(),
                        Descripcion = p.String(),
                        situacion = p.String(),
                        pais = p.String(),
                        localidad = p.String(),
                        sector = p.String(),
                        calle = p.String(),
                        longitud = p.Double(),
                        latitud = p.Double(),
                        FotoUrl = p.String(),
                        Comentarios = p.String(),
                        estatus = p.String(),
                        fechaCreacion = p.DateTime(),
                        fechaCorreccion = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Reportes]
                      SET [numReporteUsr] = @numReporteUsr, [userName] = @userName, [Descripcion] = @Descripcion, [situacion] = @situacion, [pais] = @pais, [localidad] = @localidad, [sector] = @sector, [calle] = @calle, [longitud] = @longitud, [latitud] = @latitud, [FotoUrl] = @FotoUrl, [Comentarios] = @Comentarios, [estatus] = @estatus, [fechaCreacion] = @fechaCreacion, [fechaCorreccion] = @fechaCorreccion
                      WHERE ([numReporte] = @numReporte)"
            );
            
            CreateStoredProcedure(
                "dbo.Reporte_Delete",
                p => new
                    {
                        numReporte = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Reportes]
                      WHERE ([numReporte] = @numReporte)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Reporte_Delete");
            DropStoredProcedure("dbo.Reporte_Update");
            DropStoredProcedure("dbo.Reporte_Insert");
            DropTable("dbo.RutaHs");
            DropTable("dbo.Reportes_Ruta");
            DropTable("dbo.Reportes");
            DropTable("dbo.Consumoes");
            DropTable("dbo.Colaboradors");
        }
    }
}
