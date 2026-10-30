using System.Net.Http.Json;
using DoñaTere.Core.DTOs;

namespace DonaTereApp.Services
{
    public class CategoriasService
    {
        private readonly HttpClient cliente;

        public CategoriasService()
        {
            cliente = new HttpClient
            {
                BaseAddress = new Uri("https://cwt183g0-44324.usw3.devtunnels.ms/api/")
            };
        }

        public async Task<List<CategoriaDTO>> GetAll()
        {
            var response = await cliente.GetAsync("Categorias");

            if (response.IsSuccessStatusCode)
            {
                var categorias = await response.Content.ReadFromJsonAsync<List<CategoriaDTO>>();
                return categorias ?? new List<CategoriaDTO>();
            }

            var message = await response.Content.ReadAsStringAsync();
            throw new Exception(message);
        }

        public async Task<CategoriaDTO> Create(CategoriaCreateDTO dto)
        {
            var response = await cliente.PostAsJsonAsync("Categorias", dto);

            if (response.IsSuccessStatusCode)
            {
                var categoriaCreada = await response.Content.ReadFromJsonAsync<CategoriaDTO>();

                if (categoriaCreada != null)
                {
                    return categoriaCreada;
                }
            }

            var message = await response.Content.ReadAsStringAsync();
            throw new Exception(message);
        }

        public async Task Update(int id, CategoriaCreateDTO dto)
        {
            var response = await cliente.PutAsJsonAsync($"Categorias/{id}", dto);

            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
        }

        public async Task Delete(int id)
        {
            var response = await cliente.DeleteAsync($"Categorias/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
        }
    }
}
