namespace DoñaTere.Core.DTOs
{
    public class ProductoCreateDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public bool Disponible { get; set; } = true;
        public int IdCategoria { get; set; }
    }
}