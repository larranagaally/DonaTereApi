using System.Net.Http.Json;
using DoñaTere.Core.DTOs;

namespace DonaTereApp.Services
{
    public class ClientesService
    {
        private readonly HttpClient cliente;

        public ClientesService()
        {
            cliente = new HttpClient
            {
                BaseAddress = new Uri("https://cwt183g0-44324.usw3.devtunnels.ms/api/")
            };
        }

        public async Task<List<ClienteDTO>> GetAll()
        {
            var response = await cliente.GetAsync("Clientes");

            if (response.IsSuccessStatusCode)
            {
                var clientes = await response.Content.ReadFromJsonAsync<List<ClienteDTO>>();
                return clientes ?? new List<ClienteDTO>();
            }

            var message = await response.Content.ReadAsStringAsync();
            throw new Exception(message);
        }

        public async Task<ClienteDTO> Create(ClienteCreateDTO dto)
        {
            var response = await cliente.PostAsJsonAsync("Clientes", dto);

            if (response.IsSuccessStatusCode)
            {
                var clienteCreado = await response.Content.ReadFromJsonAsync<ClienteDTO>();

                if (clienteCreado != null)
                {
                    return clienteCreado;
                }
            }

            var message = await response.Content.ReadAsStringAsync();
            throw new Exception(message);
        }

        public async Task Update(int id, ClienteCreateDTO dto)
        {
            var response = await cliente.PutAsJsonAsync($"Clientes/{id}", dto);

            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
        }

        public async Task Delete(int id)
        {
            var response = await cliente.DeleteAsync($"Clientes/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
        }
    }
}
