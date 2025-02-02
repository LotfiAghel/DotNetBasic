using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Models;
using System.Timers;
using System.Security.Cryptography;

namespace AdminClientViewModels
{

    
    public interface IEntityService00
    {
        
        Task<object> get0s(string id);

        Task<object> get00(object id);
        public static TKEY ConvertS<TKEY>(string id)
        {
            TKEY tid = default(TKEY);
            if (typeof(TKEY) == typeof(Guid))
            {
                tid = (TKEY)((object)Guid.Parse(id));
            }

            if (typeof(TKEY) == typeof(string))
            {
                tid = (TKEY)((object)id);
            }
            if (typeof(TKEY) == typeof(int))
            {
                tid = (TKEY)((object)Int32.Parse((string)((object)id)));
            }
            return tid;
        }
    }

    public interface IEntityService01<T>: IEntityService00 , IReadOnlyCollection<T>
    {
        
        Task<T> get01(object id);
        Task<IReadOnlyCollection<T>> getAll(bool froceFromServer = false);
        //IEnumerable<T> getAllFast();
        T getFast(string id);
        Task<T> post(T id);
        Task<T> update(T id);
        
    }

    public interface IEntityService<T, TKEY> : IEntityService01<T>, IEnumerable<T>,IReadOnlyCollection<ForeignKey2<T,TKEY>>
         where T : class, Models.IIdMapper<TKEY>
            where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {

        public static IEntityService<T, TKEY> instance;

        public T insertOrUpdate(T e);
        Task<IReadOnlyCollection<T>> getAllSubTable<TMASTER,TMKEY>(string collectionName, TMKEY masterEnityId);

        Task<T> get(TKEY id);
        //Task<T> get0s(TKEY id);
        T getFast(string id);
        T getFromLoaded(TKEY id);


        Task<T> post(T t);
        Task<T> update(T t);

        
        Task<T> get1s(string id);

        T Add(T t);

        IEntityService<T, TKEY> getFiltered(string itemName, TKEY masterId);

        Task<IEnumerable<T>> fetchFiltered(string itemName, TKEY masterId);
    }
}