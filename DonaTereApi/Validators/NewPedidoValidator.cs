using DoñaTere.Core.DTOs;
using DoñaTere.Core.Validators;
using DoñaTereApi.Services;


namespace DoñaTere.Api.Validators;

public class NewPedidoValidator : PedidoCreateValidator
{
    private readonly ClienteService clienteService;
    private readonly ProductoService productoService;

    public NewPedidoValidator(ClienteService clienteService, ProductoService productoService)
    {
        this.clienteService = clienteService;
        this.productoService = productoService;
    }

    public override ValidationResult IsValid(PedidoCreateDTO dto)
    {
        var valid = base.IsValid(dto);

      
        var cliente = clienteService.GetById(dto.IdCliente);
        if (cliente == null)
        {
            valid.Errors.Add($"El cliente con ID {dto.IdCliente} no existe.");
        }

       
        if (dto.Detalles != null)
        {
            foreach (var detalle in dto.Detalles)
            {
                var producto = productoService.GetById(detalle.IdProducto);
                if (producto == null)
                {
                    valid.Errors.Add($"El producto con ID {detalle.IdProducto} no existe.");
                }
            }
        }

        return valid;
    }
}