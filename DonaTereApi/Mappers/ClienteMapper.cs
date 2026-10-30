using DoñaTereApi.Entities;
using DoñaTere.Core.DTOs;

namespace DoñaTereApi.Mappers
{
    public static class ClienteMapper
    {
        public static ClienteDTO ToDto(Cliente entidad)
        {
            return new ClienteDTO
            {
                Id = entidad.Id,
                Nombre = entidad.Nombre,
                Telefono = entidad.Telefono
            };
        }

        public static Cliente ToEntity(ClienteCreateDTO dto)
        {
            return new Cliente
            {
                Nombre = dto.Nombre,
                Telefono = dto.Telefono
            };
        }
    }
}
