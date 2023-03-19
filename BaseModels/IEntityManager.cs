
using Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModelsManager
{

    public abstract class IEntityManager0
    {
        public static Dictionary<Type, IEntityManager0> managers = new Dictionary<Type, IEntityManager0>();
        
        public static IEntityManager<T> GetManager<Key, T>() where T : Models.IIdMapper<int>
        {
            return managers[typeof(T)] as IEntityManager<T>;
        }

    }
    public abstract class IEntityManager<T> : IEntityManager0 where T : Models.IIdMapper<int>
    {
        public Dictionary<int, T> data = new Dictionary<int, T>();

        private static IEntityManager<T> _instance;
        public static IEntityManager<T> instance
        {
            get
            {
                return _instance;
            }
            set {
                _instance = value;
            }

        }
        
        public static T2 getInstance<T2>()where T2 : IEntityManager<T>
        {
            return instance as T2;
        }



        public abstract Task<int> loadAll();
        public abstract Task<int> DeleteAll();


        public abstract List<T> getAllRamData();

        

        public abstract T get(int id);
        public abstract Task<T> reload(int id);
        public abstract  Task<T> insert(T d);

        public abstract  Task<T> update(T d);



       

    }

}
