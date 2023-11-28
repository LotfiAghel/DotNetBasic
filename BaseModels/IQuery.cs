using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public interface IEntityManagerW<T, TKEY>
       where T : class, Models.IIdMapper<TKEY>
       where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {

        T get0(TKEY id);
    }


    public interface IAssetManager
    {


        IEntityManagerW<T, TKEY> getManager<T, TKEY>()
            where T : class, Models.IIdMapper<TKEY>
            where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable;
        IQueryable<T> getDbSet<T>() where T : class;

        T2 getDbSet2<T2>() where T2 : class;

        public static IAssetManager instance;

    }


    public interface IMyServiceManager
    {

        IAssetManager getDB();


    }
    public class MyServiceManager2
    {

        public IAssetManager db;


    }

    public interface IQuery<T>
    {

        IQueryable<T> run(IQueryable<T> q);
    }


    public interface IAction<T> //where T :Entity
    {
        //public ForeignKey<T> foreignKey;       
        public Task<T> run(T entity, IServiceProvider Services);
    }

    public class IQueryContainer<T>{
        public IQuery<T> query { get; set; }
    }
}
