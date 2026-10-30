using DoñaTereApi.Entities;
using DoñaTere.Core.DTOs;

namespace DoñaTereApi.Mappers
{
    public static class CategoriaMapper
    {
        public static CategoriaDTO ToDto(Categorias entidad)
        {
            return new CategoriaDTO
            {
                Id = entidad.Id,
                Nombre = entidad.Nombre
            };
        }

        public static Categorias ToEntity(CategoriaCreateDTO dto)
        {
            return new Categorias
            {
                Nombre = dto.Nombre
            };
        }
    }
}
