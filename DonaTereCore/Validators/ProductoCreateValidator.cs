using DoñaTere.Core.DTOs;
using System.ComponentModel.DataAnnotations;

namespace DoñaTere.Core.Validators;

public class ProductoCreateValidator
{
    public virtual ValidationResult IsValid(ProductoCreateDTO dto)
    {
        var result = new ValidationResult();

        if (string.IsNullOrWhiteSpace(dto.Nombre))
        {
            result.Errors.Add("El nombre del producto es obligatorio.");
        }

        if (dto.Precio <= 0)
        {
            result.Errors.Add("El precio del producto debe ser mayor a 0.");
        }

        if (dto.IdCategoria <= 0)
        {
            result.Errors.Add("Debe especificar una categoría válida para el producto.");
        }

        return result;
    }
}