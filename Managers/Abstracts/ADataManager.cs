using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Managers.Interfaces;
using Managers.Context;

namespace DataManager.Abstracts
{
    public abstract class ADataManager<T> : IDisposable where T : class, IModel
    {
        public MainDataContext db;
        private bool AutoDisposeDB;

        public ADataManager(MainDataContext ndb){
            this.db = ndb;
            AutoDisposeDB = false;
        }

        T GetDataItem(int id)
        { return GetAllData().FirstOrDefault(a => a.ID.Equals(id)); }

        public IQueryable<T> GetAllData()
        { return db.Set<T>(); }

        #region Dispose
        ~ADataManager()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void Dispose(bool disposing)
        {
            if (AutoDisposeDB && db != null)
            {
                db.Dispose();
                db = null;
            }
        }
        #endregion
    }
}
