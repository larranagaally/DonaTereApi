using DoñaTereApi.Data;
using System.Reflection.Metadata;

namespace DoñaTereApi.Repositories
{
    public class Repository<T> where T: class
    {

        private DoñaTereDbContext _context;
        public DoñaTereDbContext Context { get { return _context; } }

        public Repository(DoñaTereDbContext context)
        {
            _context = context;
        }


        public IQueryable<T> Query()
        {
            return _context.Set<T>().AsQueryable();

        }


        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }


        public T? GetById(object id)
        {

            return _context.Find<T>(id);
        }


        public T Add(T entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
            return entity;

        }

        public void Update(T entity)
        {
            _context.Update(entity);
            _context.SaveChanges();

        }
        
        public void Delete(T entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }
    }
}
