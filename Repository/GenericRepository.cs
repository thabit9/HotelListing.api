using System.Linq.Expressions;
using HotelListing.api.Data.EFContext;
using HotelListing.api.IRepository;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.api.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly HotelContext _context;
        private readonly DbSet<T> _db;
        public GenericRepository(HotelContext context)
        {
            _context = context;
            _db = _context.Set<T>();
        }
        public async Task Delete(int id)
        {
           var entity = await _db.FindAsync(id);
           _db.Remove(entity!);
        }
        public void DeleteRange(IEnumerable<T> entities)
        {
            _db.RemoveRange(entities);
        }
        public async Task<T> Get(Expression<Func<T, bool>> expression, List<string>? includes = null)
        {
            IQueryable<T> query = _db;
            if(includes != null)
            {
                foreach (var includeProperty in includes)
                {
                    query = query.Include(includeProperty);
                }
            }
            return await query.AsNoTracking().FirstOrDefaultAsync(expression);
        }
        public async Task<IList<T>> GetAll(Expression<Func<T, bool>>? expression = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, List<string>? includes = null)
        {
            IQueryable<T> query = _db;
            //check if expression was passed
            //then use the where clause
            if(expression != null)
            {
                query = query.Where(expression);
            }
            //check includes 
            if(includes != null)
            {
                foreach (var includeProperty in includes)
                {
                    query = query.Include(includeProperty);
                }
            }
            //order then by
            if(orderBy != null)
            {
                query = orderBy(query);
            }
            return await query.AsNoTracking().ToListAsync();
        }
        public async Task Insert(T entity)
        {
            await _db.AddAsync(entity);
        }
        public async Task InsertRange(IEnumerable<T> entities)
        {
            await _db.AddRangeAsync(entities);
        }
        public void Update(T entity)
        {
            _db.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}