using System.Net.Http.Json;
using DoñaTere.Core.DTOs;

namespace DonaTereApp.Services
{
    public class ProductosService
    {
        private readonly HttpClient cliente;

        public ProductosService()
        {
            cliente = new HttpClient
            {
                BaseAddress = new Uri("https://cwt183g0-44324.usw3.devtunnels.ms/api/")
            };
        }

        public async Task<List<ProductoDTO>> GetAll()
        {
            var response = await cliente.GetAsync("Productos");

            if (response.IsSuccessStatusCode)
            {
                var productos = await response.Content.ReadFromJsonAsync<List<ProductoDTO>>();
                return productos ?? new List<ProductoDTO>();
            }

            var message = await response.Content.ReadAsStringAsync();
            throw new Exception(message);
        }

        public async Task Create(ProductoCreateDTO dto)
        {
            var response = await cliente.PostAsJsonAsync("Productos", dto);

            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
        }

        public async Task Update(int id, ProductoCreateDTO dto)
        {
            var response = await cliente.PutAsJsonAsync($"Productos/{id}", dto);

            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
        }

        public async Task Delete(int id)
        {
            var response = await cliente.DeleteAsync($"Productos/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
        }

        public async Task CambiarDisponibilidad(int id, bool disponible)
        {
            var response = await cliente.PutAsJsonAsync($"Productos/{id}/disponibilidad",
                new ProductoDisponibilidadDTO { Disponible = disponible });

            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
        }
    }
}
