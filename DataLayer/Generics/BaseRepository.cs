using DataLayer.Base;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class BaseRepository<TEntity> where TEntity:class
    {
        private DatabaseEntities _context;
        private DbSet<TEntity> _dbSet;

        public BaseRepository(DatabaseEntities context) { _context = context; _dbSet = context.Set<TEntity>(); }
        
        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includes = "") {
            IQueryable<TEntity> query = _dbSet;
            if (filter != null) { query = query.Where(filter); } 
            foreach (string s in includes.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) { query = query.Include(s); } 
            if (orderBy != null) { query = orderBy(query); }
            return query.FirstOrDefault();
        }

        public virtual IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includes="") {
           IQueryable<TEntity> query = _dbSet;
           if (filter != null) { query = query.Where(filter); } 
           foreach (string s in includes.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) { query = query.Include(s); } 
           if (orderBy != null) { query = orderBy(query); }
           return query;
       }

        public virtual TKey Max<TKey>(Func<TEntity, TKey> selector)
        {
            IQueryable<TEntity> query = _dbSet;
            return query.Max(selector);
        }

        /*
        public virtual TKey Max<TSource, TKey>(Func<TEntity, TKey> keySelector)
        {
            IQueryable<TEntity> query = _dbSet;
            return query.Max(keySelector);
        }
        */

        public virtual IOrderedEnumerable<TEntity> OrderBy<TSource, TKey>(Func<TEntity, TKey> keySelector)
        {
            IQueryable<TEntity> query = _dbSet;
            return query.OrderBy(keySelector);
        }

        public virtual IOrderedEnumerable<TEntity> OrderByDescending<TSource, TKey>(Func<TEntity, TKey> keySelector)
        {
            IQueryable<TEntity> query = _dbSet;
            return query.OrderByDescending(keySelector);
        }

        public virtual List<TEntity> GetAll() { IQueryable<TEntity> query = _dbSet; return query.ToList(); }
        public virtual int Count() { IQueryable<TEntity> query = _dbSet; return query.Count(); }
        public virtual int Count(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = _dbSet;
            return query.Count(filter);
        }
        public virtual TEntity GetById(object id) { return _dbSet.Find(id); } 
        public virtual void Insert(TEntity tEntity) { _dbSet.Add(tEntity); }
        public virtual void Update(TEntity tEntity) { _dbSet.Attach(tEntity); _context.Entry(tEntity).State=EntityState.Modified; }
        public virtual void Delete(TEntity tEntity) { if (_context.Entry(tEntity).State == EntityState.Detached) { _dbSet.Attach(tEntity); } _dbSet.Remove(tEntity); }
        public virtual void Delete(object id) { var entity = GetById(id); Delete(entity); }
        public void Save() { _context.SaveChanges(); }
        
        public virtual bool IsNullOrEmptyId(string value)
        {
            bool result = false;
            if (value == null)
                result = true;
            else if (value == "null")
                result = true;
            else if (value == "")
                result = true;
            else if (value == "0")
                result = true;
            return result;
        }

        public virtual List<int> SplitIds(string Ids)
        {
            List<int> finalIds = new List<int>();
            if (Ids != null)
            {
                string[] Split = Ids.Split('-');
                foreach (string item in Split)
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        finalIds.Add(itemId);
                    }
                }
            }
            return finalIds;
        }
    }
}
