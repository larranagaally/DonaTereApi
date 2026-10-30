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
public class CategoriasController : ControllerBase
{
    private readonly Repository<Categorias> repository;
    private readonly NewCategoriaValidator validator;

    public CategoriasController(Repository<Categorias> repository, NewCategoriaValidator validator)
    {
        this.repository = repository;
        this.validator = validator;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var categorias = repository.GetAll().Select(CategoriaMapper.ToDto).ToList();
        return Ok(categorias);
    }

    [HttpPost]
    public IActionResult Post(CategoriaCreateDTO dto)
    {
        var validationResult = validator.IsValid(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var categoria = new Categorias { Nombre = dto.Nombre };
        repository.Add(categoria);
        return Ok(CategoriaMapper.ToDto(categoria));
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, CategoriaCreateDTO dto)
    {
        var categoria = repository.GetById(id);
        if (categoria == null)
        {
            return NotFound();
        }

        var validationResult = new CategoriaCreateValidator().IsValid(dto);
        if (repository.GetAll().Any(x => x.Id != id && x.Nombre.ToLower() == dto.Nombre.ToLower()))
        {
            validationResult.Errors.Add("Ya existe una categoría con el mismo nombre.");
        }

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        categoria.Nombre = dto.Nombre;
        repository.Update(categoria);
        return Ok(CategoriaMapper.ToDto(categoria));
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var categoria = repository.GetById(id);
        if (categoria == null)
        {
            return NotFound();
        }

        try
        {
            repository.Delete(categoria);
            return NoContent();
        }
        catch (DbUpdateException)
        {
            return BadRequest(new List<string> { "No se puede eliminar la categoría porque tiene productos." });
        }
    }
}
