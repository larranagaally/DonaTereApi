using System.Net.Http.Json;
using DoñaTere.Core.DTOs;

namespace DonaTereApp.Services
{
    public class PedidosService
    {
        private readonly HttpClient cliente;

        public PedidosService()
        {
            cliente = new HttpClient
            {
                BaseAddress = new Uri("https://cwt183g0-44324.usw3.devtunnels.ms/api/")
            };
        }

        public async Task<List<PedidoDTO>> GetAll()
        {
            var response = await cliente.GetAsync("Pedidos");

            if (response.IsSuccessStatusCode)
            {
                var pedidos = await response.Content.ReadFromJsonAsync<List<PedidoDTO>>();
                return pedidos ?? new List<PedidoDTO>();
            }

            var message = await response.Content.ReadAsStringAsync();
            throw new Exception(message);
        }

        public async Task<PedidoDTO> Create(PedidoCreateDTO dto)
        {
            var response = await cliente.PostAsJsonAsync("Pedidos", dto);

            if (response.IsSuccessStatusCode)
            {
                var pedidoCreado = await response.Content.ReadFromJsonAsync<PedidoDTO>();

                if (pedidoCreado != null)
                {
                    return pedidoCreado;
                }
            }

            var message = await response.Content.ReadAsStringAsync();
            throw new Exception(message);
        }

        public async Task UpdateEstado(int id, string estado)
        {
            var response = await cliente.PutAsJsonAsync($"Pedidos/{id}/estado",
                new PedidoEstadoDTO { Estado = estado });

            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
        }

        public async Task UpdatePago(int id, bool pagado)
        {
            var response = await cliente.PutAsJsonAsync($"Pedidos/{id}/pago",
                new PedidoPagoDTO { Pagado = pagado });

            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
        }
    }
}
