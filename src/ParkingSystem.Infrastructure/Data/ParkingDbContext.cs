using Microsoft.EntityFrameworkCore;
using ParkingSystem.Core.Entities;
using ParkingSystem.Core.Enums;

namespace ParkingSystem.Infrastructure.Data
{
    public class ParkingDbContext : DbContext
    {
        public ParkingDbContext(DbContextOptions<ParkingDbContext> options) : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<ParkingSpot> ParkingSpots { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da entidade Vehicle
            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.LicensePlate).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Model).HasMaxLength(50);
                entity.Property(e => e.Color).HasMaxLength(30);
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(10,2)");
                
                // Índice para performance
                entity.HasIndex(e => e.LicensePlate);
                
                // Relacionamento: Um Vehicle pertence a um ParkingSpot
                entity.HasOne(v => v.ParkingSpot)
                      .WithMany(p => p.VehicleHistory)
                      .HasForeignKey(v => v.ParkingSpotId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuração da entidade ParkingSpot
            modelBuilder.Entity<ParkingSpot>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Number).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Type).HasConversion<int>();
                
                // Índice único para o número da vaga
                entity.HasIndex(e => e.Number).IsUnique();
                
                // CurrentVehicle é uma propriedade calculada - não mapear no banco
                entity.Ignore(e => e.CurrentVehicle);
            });

            // Seed data - dados iniciais
            SeedData(modelBuilder);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            // Criar 50 vagas de estacionamento
            var parkingSpots = new List<ParkingSpot>();
            
            for (int i = 1; i <= 50; i++)
            {
                var spotType = i switch
                {
                    <= 40 => SpotType.Regular,
                    <= 45 => SpotType.Compact,
                    <= 47 => SpotType.Disabled,
                    _ => SpotType.Electric
                };

                parkingSpots.Add(new ParkingSpot
                {
                    Id = i,
                    Number = $"A{i:D2}",
                    IsOccupied = false,
                    Type = spotType
                });
            }

            modelBuilder.Entity<ParkingSpot>().HasData(parkingSpots);
        }
    }
}