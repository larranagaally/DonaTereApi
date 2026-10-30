using DoñaTereApi.Entities;
using DoñaTereApi.Mappers;
using DoñaTereApi.Repositories;
using DoñaTere.Core.DTOs;

namespace DoñaTereApi.Services
{
    public class ProductoService
    {
        private readonly Repository<Producto> productoRepository;

        public ProductoService(Repository<Producto> productoRepository)
        {
            this.productoRepository = productoRepository;
        }

        public List<ProductoDTO> GetAll()
        {
            var productos = productoRepository.GetAll();
            return productos.Select(ProductoMapper.ToDto).ToList();
        }

        public ProductoDTO? GetById(int id)
        {
            var producto = productoRepository.GetById(id);
            return producto == null ? null : ProductoMapper.ToDto(producto);
        }

        public ProductoDTO Create(ProductoCreateDTO model)
        {
            var producto = ProductoMapper.ToEntity(model);
            productoRepository.Add(producto);
            return ProductoMapper.ToDto(producto);
        }

        public bool UpdateDisponibilidad(int id, ProductoDisponibilidadDTO model)
        {
            var producto = productoRepository.GetById(id);
            if (producto == null) return false;

            ProductoMapper.UpdateDisponibilidad(model, producto);
            productoRepository.Update(producto);
            return true;
        }
    }
}
