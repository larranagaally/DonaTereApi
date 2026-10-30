using DoñaTere.Core.DTOs;
using System;
using System.ComponentModel.DataAnnotations;

namespace DoñaTere.Core.Validators;

public class PedidoCreateValidator
{
    public virtual ValidationResult IsValid(PedidoCreateDTO dto)
    {
        var result = new ValidationResult();

        if (dto.FechaEntrega.Date <= DateTime.Today)
        {
            result.Errors.Add("La fecha de entrega debe ser posterior al día de hoy.");
        }

        if (dto.Detalles == null || dto.Detalles.Count == 0)
        {
            result.Errors.Add("El pedido debe contener al menos un producto.");
        }
        else
        {
            foreach (var detalle in dto.Detalles)
            {
                if (detalle.Cantidad <= 0)
                {
                    result.Errors.Add($"La cantidad para el producto ID {detalle.IdProducto} debe ser mayor a 0.");
                }
            }
        }

        return result;
    }
}