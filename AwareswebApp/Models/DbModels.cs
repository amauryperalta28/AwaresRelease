using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AwareswebApp.Models
{
    /**
     * Clase destinada a la declaracion de las tablas
     * que tendra la base de datos  y que utilizara
     * el codeFirst Migration.
     */
    public class DbModels : DbContext
    {
       
        public DbModels()
            : base("DefaultConnection")
            {
            }
        public DbSet<Consumo> Consumos { get; set; } 
        public DbSet<Reporte> Reportes { get; set; }
        public DbSet<Reportes_Ruta> Reportes_Ruta { get; set; }
        public DbSet<RutaH> RutaH { get; set; }
       
        
        public DbSet<Colaborador> Colaboradores { get; set; } //Esta es una subclase de OrdenModels

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reporte>().MapToStoredProcedures();
        }

       
    }
}