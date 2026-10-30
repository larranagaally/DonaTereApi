using DoñaTere.Core.DTOs;
using DoñaTereApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DoñaTere.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportesController : ControllerBase
{
    private readonly PedidoService pedidoService;

    public ReportesController(PedidoService pedidoService)
    {
        this.pedidoService = pedidoService;
    }

   
    [HttpGet("pedidos-pendientes")]
    public IActionResult GetPedidosPendientes( DateTime fecha)
    {
        var result = pedidoService.GetPedidosPendientes(fecha);
        return Ok(result);
    }

   
    [HttpGet("productos-mas-pedidos")]
    public IActionResult GetProductosMasPedidos()
    {
        var result = pedidoService.GetProductosMasPedidos();
        return Ok(result);
    }

    
    [HttpGet("ventas-mes")]
    public IActionResult GetVentasDelMes(int anio,  int mes)
    {
        var result = pedidoService.GetVentasDelMes(anio, mes);
        return Ok(result);
    }
}