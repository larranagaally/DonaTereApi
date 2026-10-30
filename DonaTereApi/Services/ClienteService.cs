using DoñaTereApi.Entities;
using DoñaTereApi.Mappers;
using DoñaTereApi.Repositories;
using DoñaTere.Core.DTOs;

namespace DoñaTereApi.Services
{
    public class ClienteService
    {
        private readonly Repository<Cliente> clienteRepository;

        public ClienteService(Repository<Cliente> clienteRepository)
        {
            this.clienteRepository = clienteRepository;
        }

        public List<ClienteDTO> GetAll()
        {
            var clientes = clienteRepository.GetAll();
            return clientes.Select(ClienteMapper.ToDto).ToList();
        }

        public ClienteDTO? GetById(int id)
        {
            var cliente = clienteRepository.GetById(id);
            return cliente == null ? null : ClienteMapper.ToDto(cliente);
        }

        public ClienteDTO Create(ClienteCreateDTO model)
        {
            var cliente = ClienteMapper.ToEntity(model);
            clienteRepository.Add(cliente);
            return ClienteMapper.ToDto(cliente);
        }
    }
}
