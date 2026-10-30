using DoñaTere.Core.DTOs;

namespace PROYECTOFINAL_API.Models
{
    public class Productos
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public decimal Precio { get; set; }
        public bool Disponible { get; set; }
        
        public bool Activo { get; set; } = true;
        public int CategoriaId { get; set; }
        public string NombreCategoria { get; set; } = "";

        public static Productos Desde(ProductoDTO dto)
        {
            return new Productos
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                Precio = dto.Precio,
                Disponible = dto.Disponible,
                CategoriaId = dto.IdCategoria,
                NombreCategoria = dto.CategoriaNombre
            };
        }
    }

    public class GrupoProductos : List<Productos>
    {
        public GrupoProductos(string nombre, IEnumerable<Productos> productos) : base(productos)
        {
            Nombre = nombre;
        }

        public string Nombre { get; }
    }
}
