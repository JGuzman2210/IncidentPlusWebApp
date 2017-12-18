using IncidentPlus.Entity.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentPlus.Data
{
    public abstract class GenericRepository<T> where T : Entity.Entities.BaseEntity
    {
        public void Add(T entity)
        {
            using (var db = new ContextDB.IncidencPlusDBContext())
            {
                db.Set<T>().Add(entity);
                db.SaveChanges();
            }
        }

        public void Update(T entity)
        {
            using (var db = new ContextDB.IncidencPlusDBContext())
            {
                try
                {
                    db.Database.BeginTransaction();
                    db.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    db.Database.CurrentTransaction.Commit();
                }
                catch
                {
                    db.Database.CurrentTransaction.Rollback();
                }
            }
        }

        public T FindById(int id)
        {
            using (var db = new ContextDB.IncidencPlusDBContext())
            {
                return db.Set<T>().Where(_ => _.Id == id).FirstOrDefault();
            }
        }

        public void Delete(int id)
        {
            using (var db = new ContextDB.IncidencPlusDBContext())
            {
                try
                {
                    db.Database.BeginTransaction();
                    var _entityTemp = db.Set<T>().Where(_ => _.Id == id).FirstOrDefault();
                    _entityTemp.State = StateEntity.DELETED;
                    db.SaveChanges();

                    db.Database.CurrentTransaction.Commit();
                }
                catch
                {
                    db.Database.CurrentTransaction.Rollback();
                }
            }
        }

        public List<T> GetAll()
        {
            using (var db = new ContextDB.IncidencPlusDBContext())
            {
                //return db.Set<T>().Where(_ => _.State == StateEntity.ACTIVE).ToList();
                return db.Set<T>().ToList();
            }
        }

        public void Enable(int id)
        {
            using (var db = new ContextDB.IncidencPlusDBContext())
            {
                try
                {
                    db.Database.BeginTransaction();
                    var result = db.Set<T>().Where(_ => _.Id == id).FirstOrDefault();
                    result.State = StateEntity.ACTIVE;
                    db.SaveChanges();
                    db.Database.CurrentTransaction.Commit();
                }
                catch
                {
                    db.Database.CurrentTransaction.Rollback();
                }
            }
        }
    }
}