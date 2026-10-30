using DoñaTere.Core.DTOs;
using System.ComponentModel.DataAnnotations;

namespace DoñaTere.Core.Validators;

public class CategoriaCreateValidator
{
    public virtual ValidationResult IsValid(CategoriaCreateDTO dto)
    {
        var result = new ValidationResult();

        if (string.IsNullOrWhiteSpace(dto.Nombre))
        {
            result.Errors.Add("El nombre de la categoría es obligatorio.");
        }

        return result;
    }
}