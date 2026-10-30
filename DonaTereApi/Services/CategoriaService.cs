using DoñaTereApi.Entities;
using DoñaTereApi.Mappers;
using DoñaTereApi.Repositories;
using DoñaTere.Core.DTOs;

namespace DoñaTereApi.Services
{
    public class CategoriaService
    {
        private readonly Repository<Categorias> categoriaRepository;

        public CategoriaService(Repository<Categorias> categoriaRepository)
        {
            this.categoriaRepository = categoriaRepository;
        }

        public List<CategoriaDTO> GetAll()
        {
            var categorias = categoriaRepository.GetAll();
            return categorias.Select(CategoriaMapper.ToDto).ToList();
        }

        public CategoriaDTO? GetById(int id)
        {
            var categoria = categoriaRepository.GetById(id);
            return categoria == null ? null : CategoriaMapper.ToDto(categoria);
        }

        public CategoriaDTO Create(CategoriaCreateDTO model)
        {
            var categoria = CategoriaMapper.ToEntity(model);
            categoriaRepository.Add(categoria);
            return CategoriaMapper.ToDto(categoria);
        }
    }
}