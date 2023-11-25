using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminClientViewModels
{

    public class PartCollectionProxy<T, TKEY> : IEntityService<T, TKEY>
       where T : class, Models.IIdMapper<TKEY>
            where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {
        public List<T> data;
        public System.Reflection.PropertyInfo pr;
        public TKEY masterId;
        public IEntityService<T, TKEY> masterManager;

        public int Count => ((ICollection<T>)data).Count;

        public bool IsReadOnly => ((ICollection<T>)data).IsReadOnly;

        public T Add(T t)
        {
            pr.SetValue(t, masterId);
            data.Add(t);
            return t;
        }

       
       
        public T getFast(string id)
        {
            return masterManager.getFast(id);

        }
        public T getFromLoaded(TKEY id)
        {
            return masterManager.getFromLoaded(id);
        }
        public void refresh()
        {
            data = new List<T>();
            foreach (var row in (masterManager as IEnumerable<T>))
            {
                if (pr.GetValue(row).Equals(masterId))
                    data.Add(row);
            }
        }
        public async Task<IReadOnlyCollection<T>> getAll(bool froceReloadFromServer)
        {
            await masterManager.fetchFiltered(pr.Name, masterId);
            refresh();
            return data;
        }


        public IEntityService<T, TKEY> getFiltered(string itemName, TKEY masterId)
        {
            return masterManager.getFiltered(itemName, masterId);
        }

        public async Task<object> get00(object id)
        {
            return await masterManager.get00(id);
        }

        public Task<T> post(T t)
        {
            return masterManager.post(t);
        }

        public Task<T> update(T t)
        {
            return masterManager.update(t);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return data.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return data.GetEnumerator();
        }

        public async Task<IEnumerable<T>> fetchFiltered(string itemName, TKEY masterId)
        {
            return data;
        }
        public async Task<IReadOnlyCollection<T>> getAllSubTable<TMKEY>(string masterEntityName, string collectionName, TMKEY masterEnityId)
        {

            return data;
        }



        public async Task<T> get01(object id)
        {
            return (await get00((TKEY)id)) as T;
        }


        public async Task<object> get0s(string id)
        {
            return await masterManager.get0s(id);
        }

        public Task<T> get1s(string id)
        {
            return masterManager.get1s(id);
        }
        public async Task<T> get(TKEY id)
        {
            return await masterManager.get(id);
        }

        public IEnumerator<ForeignKey2<T, TKEY>> GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }


}
