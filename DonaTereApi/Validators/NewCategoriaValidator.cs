using DoñaTere.Core.DTOs;
using DoñaTere.Core.Validators;
using DoñaTereApi.Entities;
using DoñaTereApi.Repositories;


namespace DoñaTere.Api.Validators;

public class NewCategoriaValidator : CategoriaCreateValidator
{
    private readonly Repository<Categorias> repository;

    public NewCategoriaValidator(Repository<Categorias> repository)
    {
        this.repository = repository;
    }

    public override ValidationResult IsValid(CategoriaCreateDTO dto)
    {
        var valid = base.IsValid(dto);

       
        if (repository.GetAll().Any(x => x.Nombre.ToLower() == dto.Nombre.ToLower()))
        {
            valid.Errors.Add("Ya existe una categoría con el mismo nombre.");
        }

        return valid;
    }
}