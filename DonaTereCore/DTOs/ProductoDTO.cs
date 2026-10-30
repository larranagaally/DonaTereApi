namespace DoñaTere.Core.DTOs
{
    public class ProductoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public bool Disponible { get; set; }
        public int IdCategoria { get; set; }
        public string CategoriaNombre { get; set; } = string.Empty;
    }
}