using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using AdminSoft.Data.Interfaces.Base;
using PLNFramework.Expressions;

namespace AdminSoft.Data.Base
{
    /// <summary>
    /// Clase base para repositorios
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Contexto de datos
        /// </summary>
        protected AdminSoftContext _context;
        /// <summary>
        /// Entities
        /// </summary>
        private readonly DbSet<TEntity> _entities;
        public BaseRepository(DbContext context)
        {
            if (context == null)
                throw new Exception("El contexto no puede se nulo: BaseRepository");
            if (context is AdminSoftContext)
                _context = context as AdminSoftContext;
            else throw new Exception("El contexto no es valido: BaseRepository");
            _entities = _context.Set<TEntity>();
        }
        public object ObjectState { get; private set; }

        protected virtual void Reload(TEntity entity)
        {
            (_context as DbContext).Entry(entity).Reload();
        }
        public TEntity Find(params object[] keyValues)
        {
            return _entities.Find(keyValues);
        }
        public IQueryable<TEntity> GetAllQuery()
        {
            return _entities;
        }
        public ICollection<TEntity> GetAll()
        {
            return _entities.ToList();
        }
        public ICollection<TEntity> Query(TEntity criteria, string orders = "", params Expression<Func<TEntity, object>>[] includes)
        {
            if (criteria == null) throw new Exception("Criteria no puede ser nula");

            var where = new QueryBuilder<TEntity>(criteria).Action();

            return Query(where, orders, includes);
        }
        public ICollection<TEntity> Query(Expression<Func<TEntity, bool>> where, string orders = "", params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _entities.OfType<TEntity>().AsNoTracking();
            if (includes != null && includes.Any())
                query = includes.Aggregate(query, (current, expression) => current.Include(expression));

            if (!string.IsNullOrEmpty(orders))
                query = query.OrderBy(orders);

            if (where != null)
                query = query.Where(where).OfType<TEntity>();

            return query.ToList();
        }
        public ICollection<TEntity> QueryPage(TEntity criteria, out int totalPages, out int totalRows, int page = 0, int pageSize = 10, string orders = "", params Expression<Func<TEntity, object>>[] includes)
        {
            if (criteria == null) throw new Exception("Criteria no puede ser nula");

            var where = new QueryBuilder<TEntity>(criteria).Action();

            return QueryPage(where, out totalPages, out totalRows, page, pageSize, orders, includes);
        }
        public ICollection<TEntity> QueryPage(Expression<Func<TEntity, bool>> where, out int totalPages, out int totalRows, int page = 0, int pageSize = 10, string orders = "", params Expression<Func<TEntity, object>>[] includes)
        {
            if (pageSize <= 0) throw new Exception("El valor del parámetro 'pageSize' debe ser mayor que cero");
            if (string.IsNullOrEmpty(orders)) throw new Exception("Es necesario indicar un orden para una consulta paginada");

            IQueryable<TEntity> query = _entities.OfType<TEntity>().AsNoTracking();
            if (includes != null && includes.Any())
                query = includes.Aggregate(query, (current, expression) => current.Include(expression));

            if (where != null)
                query = query.Where(where).OfType<TEntity>();

            totalRows = query.Count();

            var results = query.OrderBy(orders).Skip(pageSize * page).Take(pageSize).ToList();
            totalPages = (int)Math.Ceiling((double)totalRows / pageSize);
            return results;
        }

        public void Insert(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Insert(entity);
            }
            _context.SaveChanges();
        }
        public void Insert(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _context.Entry<TEntity>(entity).State = EntityState.Added;
            _entities.Add(entity);
            _context.SaveChanges();
        }
        public void Update(TEntity entity)
        {
            if (!_entities.Local.Any(e => e == entity))
                _entities.Attach(entity);

            _context.Entry<TEntity>(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public void Delete(TEntity entity)
        {
            if (!_entities.Local.Any(e => e == entity))
                _entities.Attach(entity);
            _context.Entry(entity).State = EntityState.Deleted;
            _context.SaveChanges();
        }
        public void Delete(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
            _context.SaveChanges();
        }
    }
}
