﻿using ISO810_ComprasProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ISO810_ComprasProject.Data
{
    public class ComprasDBContext : IdentityDbContext<Users>
    {
        public ComprasDBContext(DbContextOptions<ComprasDBContext> options)
            : base(options) { }

        public DbSet<Articulos> Articulo { get; set; }
        public DbSet<UnidadesMedida> UnidadMedida { get; set; }
        public DbSet<OrdenCompras> OrdenCompra { get; set; }
        public DbSet<Departamentos> Departamento { get; set; }
        public DbSet<Proveedores> Proveedor { get; set; }
        public DbSet<LoginViewModel> Login { get; set; }
        public DbSet<RegisterViewModel> Registro { get; set; }
        public DbSet<Transaccion> Transaccion { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 

            modelBuilder.Entity<Articulos>()
                .HasIndex(a => a.Descripcion)
                .IsUnique();

            modelBuilder.Entity<OrdenCompras>()
                .HasOne(o => o.Articulo)
                .WithMany()
                .HasForeignKey(o => o.ArticuloId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrdenCompras>()
                .HasOne(o => o.UnidadMedida)
                .WithMany()
                .HasForeignKey(o => o.UnidadMedidaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Departamentos>()
                .HasIndex(d => d.Nombre)
                .IsUnique();

            modelBuilder.Entity<Proveedores>()
                .HasIndex(p => p.NombreComercial)
                .IsUnique();
        }
    }
}
