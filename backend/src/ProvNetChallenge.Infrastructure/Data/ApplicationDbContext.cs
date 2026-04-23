using System.Collections.Generic;
using System.Reflection.Emit;
using ProvNetChallenge.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ProvNetChallenge.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        /*  
        For migrations run in the terminal from crc:
            1. cd ProvNetChallenge.Infrastructure dotnet ef migrations add Initial -o Data/Migrations --startup-project ../ProvNetChallenge.WebApi
            2. dotnet ef database update -p src/ProvNetChallenge.Infrastructure -s src/ProvNetChallenge.WebApi
        */
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext()
        {
        }

        // DbSet representa la tabla 'USER' en la base de datos.
        // Entity Framework usará esto para hacer SELECT, INSERT, UPDATE, DELETE.
        public DbSet<User> USER { get; set; }

        // Este método es opcional pero recomendado en arquitectura limpia.
        // Aquí configuramos reglas de la BD usando "Fluent API" si las DataAnnotations (atributos) no son suficientes.
        // Esto creará un índice único en SQL Server, evitando correos duplicados a nivel de base de datos.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ejemplo: Si quisiéramos asegurar que el email sea único en la BD.
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();
        }
    }
}