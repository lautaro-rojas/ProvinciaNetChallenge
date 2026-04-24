using ProvNetChallenge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace ProvNetChallenge.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
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
        public DbSet<Product> PRODUCT { get; set; }

        // Este método es opcional pero recomendado en arquitectura limpia.
        // Aquí configuramos reglas de la BD usando "Fluent API" si las DataAnnotations (atributos) no son suficientes.
        // Esto creará un índice único en SQL Server, evitando correos duplicados a nivel de base de datos.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Configuración de User
            modelBuilder.Entity<User>(entity =>
            {
                // Forzamos el nombre de la tabla
                entity.ToTable("USER");

                // Primary Key (EF Core asume que 'Id' es PK por defecto, pero ser explícito es buena práctica)
                entity.HasKey(u => u.Id);

                // Índices únicos
                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.UserName).IsUnique();

                // Propiedades de texto limitadas (Reemplaza a los [MaxLength] y [Required])
                entity.Property(u => u.Email).IsRequired().HasMaxLength(150);
                entity.Property(u => u.UserName).IsRequired().HasMaxLength(100); // Lo subí a 100 por tu código
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(100);

                // Password (Suele ser largo después del hash, no le ponemos MaxLength corto)
                entity.Property(u => u.PasswordHash).IsRequired();

                // Auditoría y Estados
                entity.Property(u => u.IsActive).HasDefaultValue(true);

                // Si querés que la base de datos maneje el default de la fecha de activación:
                entity.Property(u => u.DateActivation).HasDefaultValueSql("GETUTCDATE()");

                // Al ser DateTime?, EF Core automáticamente las mapea como columnas NULLABLE en SQL
                entity.Property(u => u.DateDeactivation).IsRequired(false);
                entity.Property(u => u.DateModification).IsRequired(false);
            });
            #endregion

            #region Configuración de Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("PRODUCT");

                entity.HasKey(p => p.Id);

                // El SKU debe ser único y obligatorio
                entity.HasIndex(p => p.SKU).IsUnique();
                entity.Property(p => p.SKU).IsRequired().HasMaxLength(50);

                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Description).HasMaxLength(500);

                // Precisión financiera
                entity.Property(p => p.Price).HasColumnType("decimal(18,2)");

                // Configuración de auditoría por defecto
                entity.Property(p => p.IsActive).HasDefaultValue(true);
                entity.Property(p => p.DateCreation).HasDefaultValueSql("GETUTCDATE()");
            });
            #endregion
        }
    }
}