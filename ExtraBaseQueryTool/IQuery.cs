using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public interface IQuery<T>
    {

        IQueryable<T> run(IQueryable<T> q);
    }


    public interface IAction<T> //where T :Entity
    {
        //public ForeignKey<T> foreignKey;       
        public Task<T> run(T entity, IMyServiceManager ims);
    }
}
