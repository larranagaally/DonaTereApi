using DoñaTereApi.Entities;
using DoñaTere.Core.DTOs;

namespace DoñaTereApi.Mappers
{
    public static class ProductoMapper
    {
        public static ProductoDTO ToDto(Producto entidad)
        {
            return new ProductoDTO
            {
                Id = entidad.Id,
                Nombre = entidad.Nombre,
                Precio = entidad.Precio,
                Disponible = entidad.Disponible,
                IdCategoria = entidad.IdCategoria,
                CategoriaNombre = entidad.IdCategoriaNavigation?.Nombre ?? string.Empty
            };
        }

        public static Producto ToEntity(ProductoCreateDTO dto)
        {
            return new Producto
            {
                Nombre = dto.Nombre,
                Precio = dto.Precio,
                Disponible = dto.Disponible,
                IdCategoria = dto.IdCategoria
            };
        }

        public static void UpdateDisponibilidad(ProductoDisponibilidadDTO dto, Producto entidad)
        {
            entidad.Disponible = dto.Disponible;
        }
    }
}
