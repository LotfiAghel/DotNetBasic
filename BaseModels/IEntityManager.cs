
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
        
        public static IEntityManager<T, Key> GetManager<Key, T>() where T : Models.IIdMapper<Key>
            where Key : IEquatable<Key>, IComparable<Key>, IComparable
        {
            return managers[typeof(T)] as IEntityManager<T, Key>;
        }

    }
    public abstract class IEntityManager<T,TKEY> : IEntityManager0 where T : Models.IIdMapper<TKEY>
        where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {
        public Dictionary<TKEY, T> data = new Dictionary<TKEY, T>();

        private static IEntityManager<T,TKEY> _instance;
        public static IEntityManager<T, TKEY> instance
        {
            get
            {
                return _instance;
            }
            set {
                _instance = value;
            }

        }
        
        public static T2 getInstance<T2>()where T2 : IEntityManager<T, TKEY>
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


    public abstract class IEntityManager<T> : IEntityManager<T, int> where T : Models.IIdMapper<int>
    {
        //private static IEntityManager<T,int> instance { get =>}
    }


}
