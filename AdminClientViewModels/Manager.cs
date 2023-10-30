using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
namespace AdminClientViewModels
{


    public interface IEntityService00
    {

        Task<object> get0s(string id);

        Task<object> get00(object id);
        
    }

    public interface IEntityService01<T>: IEntityService00 , IReadOnlyCollection<T>
    {

        Task<T> get01(object id);
        Task<IEnumerable<T>> getAll(bool froceFromServer = false);
        //IEnumerable<T> getAllFast();
        T getFast(string id);
        Task<T> post(T id);
        Task<T> update(T id);
    }

    public interface IEntityService<T, TKEY> : IEntityService01<T>, IEnumerable<T>,IReadOnlyCollection<ForeignKey2<T,TKEY>>
         where T : class, Models.IIdMapper<TKEY>
            where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {


        Task<System.Collections.IEnumerable> getAllSubTable<TMKEY>(string masterEntityName, string collectionName, TMKEY masterEnityId);

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