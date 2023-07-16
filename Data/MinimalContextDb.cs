using Microsoft.EntityFrameworkCore;
using VehiclesMinimalAPI.Models;
using VehiclesMinimalAPI.Seeds;

namespace VehiclesMinimalAPI.Data
{
    public class MinimalContextDb : DbContext
    {
        public MinimalContextDb(DbContextOptions<MinimalContextDb> options) : base(options) { }

        public DbSet<Automovel> Automoveis { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Automovel>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Automovel>()
                .Property(p => p.Modelo)
                .IsRequired()
                .HasColumnType("varchar(50)");

            modelBuilder.Entity<Automovel>()
                .Property(p => p.Marca)
                .IsRequired()
                .HasColumnType("varchar(50)");

            modelBuilder.Entity<Automovel>()
                .Property(p => p.Cor)
                .IsRequired()
                .HasColumnType("varchar(50)");

            modelBuilder.Entity<Automovel>()
                .Property(p => p.Placa)
                .IsRequired()
                .HasColumnType("varchar(7)");

            modelBuilder.Entity<Automovel>()
                .ToTable("Automoveis");

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Automovel>().HasData(AutomovelSeed.Seed);
        }
    }
}
