using DoñaTere.Api.Validators;
using DoñaTere.Core.DTOs;
using DoñaTere.Core.Validators;
using DoñaTereApi.Entities;
using DoñaTereApi.Mappers;
using DoñaTereApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoñaTere.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientesController : ControllerBase
{
    private readonly Repository<Cliente> repository;
    private readonly NewClienteValidator validator;

    public ClientesController(Repository<Cliente> repository, NewClienteValidator validator)
    {
        this.repository = repository;
        this.validator = validator;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var clientes = repository.GetAll().Select(ClienteMapper.ToDto).ToList();
        return Ok(clientes);
    }

    [HttpPost]
    public IActionResult Post(ClienteCreateDTO dto)
    {
        var validationResult = validator.IsValid(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var cliente = new Cliente { Nombre = dto.Nombre, Telefono = dto.Telefono };
        repository.Add(cliente);
        return Ok(ClienteMapper.ToDto(cliente));
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, ClienteCreateDTO dto)
    {
        var cliente = repository.GetById(id);
        if (cliente == null)
        {
            return NotFound();
        }

        var validationResult = new ClienteCreateValidator().IsValid(dto);
        if (repository.GetAll().Any(x => x.Id != id && x.Telefono == dto.Telefono))
        {
            validationResult.Errors.Add("Ya existe un cliente registrado con este número de teléfono.");
        }

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        cliente.Nombre = dto.Nombre;
        cliente.Telefono = dto.Telefono;
        repository.Update(cliente);
        return Ok(ClienteMapper.ToDto(cliente));
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var cliente = repository.GetById(id);
        if (cliente == null)
        {
            return NotFound();
        }

        try
        {
            repository.Delete(cliente);
            return NoContent();
        }
        catch (DbUpdateException)
        {
            return BadRequest(new List<string> { "No se puede eliminar el cliente porque tiene pedidos registrados." });
        }
    }
}
