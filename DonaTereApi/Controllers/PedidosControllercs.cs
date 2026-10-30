using DoñaTere.Api.Validators;
using DoñaTere.Core.DTOs;
using DoñaTereApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DoñaTere.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PedidosController : ControllerBase
{
    private static readonly string[] EstadosValidos = { "Pendiente", "Entregado", "Cancelado" };

    private readonly NewPedidoValidator validator;
    private readonly PedidoService pedidoService;

    public PedidosController(NewPedidoValidator validator, PedidoService pedidoService)
    {
        this.validator = validator;
        this.pedidoService = pedidoService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(pedidoService.GetAll());
    }

    [HttpPost]
    public IActionResult Post(PedidoCreateDTO dto)
    {
        var validationResult = validator.IsValid(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        try
        {
            var pedidoCreado = pedidoService.Create(dto);
            return Ok(pedidoCreado);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new List<string> { ex.Message });
        }
    }

    [HttpPut("{id}/estado")]
    public IActionResult CambiarEstado(int id, PedidoEstadoDTO dto)
    {
        if (!EstadosValidos.Contains(dto.Estado))
        {
            return BadRequest(new List<string> { "El estado debe ser Pendiente, Entregado o Cancelado." });
        }

        try
        {
            var actualizado = pedidoService.UpdateEstado(id, dto.Estado);
            if (!actualizado)
            {
                return NotFound();
            }

            return Ok(pedidoService.GetById(id));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new List<string> { ex.Message });
        }
    }

    [HttpPut("{id}/pago")]
    public IActionResult CambiarPago(int id, PedidoPagoDTO dto)
    {
        var actualizado = pedidoService.UpdatePago(id, dto.Pagado);
        if (!actualizado)
        {
            return NotFound();
        }

        return Ok(pedidoService.GetById(id));
    }
}
