using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace FSDP.DATA.EF.Repositories
{
    public interface IGenericRepository<TEntity> : IDisposable where TEntity : class
    {
        List<TEntity> Get(string includeProperties = "");
        TEntity Find(object id);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        void Remove(object id);
        int CountRecords();
    }

    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        // Commented out because DbContext will be passed in from 
        // the UnitOfWork 
        //private cStoreEntities db = new cStoreEntities();

        // receiving the DbContext passed from the UnitOfWork
        internal DbContext db;

        //constructor
        public GenericRepository(DbContext context)
        {
            this.db = context;
        }

        public List<TEntity> Get(string includeProperties = "")
        {
            IQueryable<TEntity> query = db.Set<TEntity>();

            foreach (var prop in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(prop);
            }

            return query.ToList();
        }

        public TEntity Find(object id)
        {
            return db.Set<TEntity>().Find(id);
        }

        public void Add(TEntity entity)
        {
            db.Set<TEntity>().Add(entity);
            db.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        public void Remove(TEntity entity)
        {
            db.Set<TEntity>().Remove(entity);
            db.SaveChanges();
        }

        public void Remove(object id)
        {
            var entity = Find(id);
            Remove(entity);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public int CountRecords()
        {
            return Get().Count;
        }
    }
}

