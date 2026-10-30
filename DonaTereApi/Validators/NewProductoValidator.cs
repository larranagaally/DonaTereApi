using DoñaTere.Core.DTOs;
using DoñaTere.Core.Validators;
using DoñaTereApi.Entities;
using DoñaTereApi.Repositories;

namespace DoñaTere.Api.Validators;

public class NewProductoValidator : ProductoCreateValidator
{
    private readonly Repository<Producto> productoRepo;
    private readonly Repository<Categorias> categoriaRepo;

    public NewProductoValidator(Repository<Producto> productoRepo, Repository<Categorias> categoriaRepo)
    {
        this.productoRepo = productoRepo;
        this.categoriaRepo = categoriaRepo;
    }

    public override ValidationResult IsValid(ProductoCreateDTO dto)
    {
        var valid = base.IsValid(dto);

        
        if (productoRepo.GetAll().Any(x => x.Nombre.ToLower() == dto.Nombre.ToLower()))
        {
            valid.Errors.Add("Ya existe un producto con el mismo nombre.");
        }

        
        if (!categoriaRepo.GetAll().Any(x => x.Id == dto.IdCategoria))
        {
            valid.Errors.Add($"La categoría con ID {dto.IdCategoria} no existe.");
        }

        return valid;
    }
}