using DoñaTereApi.Entities;
using DoñaTereApi.Repositories;
using DoñaTere.Core.DTOs;
using DoñaTere.Core.Validators;


namespace DoñaTere.Api.Validators;

public class NewClienteValidator : ClienteCreateValidator
{
    private readonly Repository<Cliente> repository;

    public NewClienteValidator(Repository<Cliente> repository)
    {
        this.repository = repository;
    }

    public override ValidationResult IsValid(ClienteCreateDTO dto)
    {
        var valid = base.IsValid(dto);

        
        if (repository.GetAll().Any(x => x.Telefono == dto.Telefono))
        {
            valid.Errors.Add("Ya existe un cliente registrado con este número de teléfono.");
        }

        return valid;
    }
}