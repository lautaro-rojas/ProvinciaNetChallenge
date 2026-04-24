namespace ProvNetChallenge.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        
        // Datos Maestros
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        // SKU (Stock Keeping Unit): Fundamental en cualquier inventario real.
        // Agregué un SKU además del ID porque en la vida real, los inventarios se manejan por códigos de barras o alfanuméricos, no por la Primary Key autoincremental de la base de datos
        public string SKU { get; set; } = string.Empty;
        
        // El precio SIEMPRE debe ser decimal en aplicaciones financieras/comerciales
        // Elegí decimal para el precio en lugar de double o float para evitar la pérdida de precisión por redondeo de coma flotante
        public decimal Price { get; set; } 
        
        // Cantidad disponible
        public int Stock { get; set; }

        // Auditoría y Borrado Lógico (Consistente con tu entidad User)
        public bool IsActive { get; set; } = true;
        
        public DateTime DateCreation { get; set; } = DateTime.UtcNow;
        
        // Anulables porque al crearse el producto no tiene modificaciones ni bajas
        public DateTime? DateModification { get; set; } 
        public DateTime? DateDeactivation { get; set; } 
    }
}