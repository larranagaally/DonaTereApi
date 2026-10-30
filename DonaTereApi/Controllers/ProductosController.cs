using DoñaTere.Api.Validators;
using DoñaTere.Core.DTOs;
using DoñaTere.Core.Validators;
using DoñaTereApi.Entities;
using DoñaTereApi.Repositories;
using DoñaTereApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoñaTere.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly ProductoService productoService;
        private readonly NewProductoValidator validator;
        private readonly Repository<Producto> productoRepository;
        private readonly Repository<Categorias> categoriaRepository;

        public ProductosController(ProductoService productoService, NewProductoValidator validator,
            Repository<Producto> productoRepository, Repository<Categorias> categoriaRepository)
        {
            this.productoService = productoService;
            this.validator = validator;
            this.productoRepository = productoRepository;
            this.categoriaRepository = categoriaRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var productos = productoService.GetAll();
            return Ok(productos);
        }

        [HttpPost]
        public IActionResult Post(ProductoCreateDTO dto)
        {
            var validationResult = validator.IsValid(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var productoCreado = productoService.Create(dto);
            return Ok(productoCreado);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, ProductoCreateDTO dto)
        {
            var producto = productoRepository.GetById(id);
            if (producto == null)
            {
                return NotFound();
            }

            var validationResult = new ProductoCreateValidator().IsValid(dto);
            if (productoRepository.GetAll().Any(x => x.Id != id && x.Nombre.ToLower() == dto.Nombre.ToLower()))
            {
                validationResult.Errors.Add("Ya existe un producto con el mismo nombre.");
            }

            if (!categoriaRepository.GetAll().Any(x => x.Id == dto.IdCategoria))
            {
                validationResult.Errors.Add($"La categoría con ID {dto.IdCategoria} no existe.");
            }

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            producto.Nombre = dto.Nombre;
            producto.Precio = dto.Precio;
            producto.Disponible = dto.Disponible;
            producto.IdCategoria = dto.IdCategoria;
            productoRepository.Update(producto);

            return Ok(productoService.GetById(id));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var producto = productoRepository.GetById(id);
            if (producto == null)
            {
                return NotFound();
            }

            try
            {
                productoRepository.Delete(producto);
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return BadRequest(new List<string> { "No se puede eliminar el producto porque está en pedidos registrados." });
            }
        }

        [HttpPut("{id}/disponibilidad")]
        public IActionResult CambiarDisponibilidad(int id, ProductoDisponibilidadDTO dto)
        {
            var actualizado = productoService.UpdateDisponibilidad(id, dto);
            if (!actualizado)
            {
                return NotFound();
            }

            return Ok(dto);
        }
    }
}
