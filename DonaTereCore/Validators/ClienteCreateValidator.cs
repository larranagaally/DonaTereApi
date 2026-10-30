using DoñaTere.Core.DTOs;
using System.ComponentModel.DataAnnotations;

namespace DoñaTere.Core.Validators;

public class ClienteCreateValidator
{
    public virtual ValidationResult IsValid(ClienteCreateDTO dto)
    {
        var result = new ValidationResult();

        if (string.IsNullOrWhiteSpace(dto.Nombre))
        {
            result.Errors.Add("El nombre del cliente es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(dto.Telefono))
        {
            result.Errors.Add("El teléfono del cliente es obligatorio.");
        }

        return result;
    }
}