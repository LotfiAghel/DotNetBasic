using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using ClTool;
using System;
namespace AdminClientViewModels
{

   
    public interface IEntityService00
    {

        Task<object> get0s(string id);

        Task<object> get00(object id);
        
    }

    public interface IEntityService01<T>: IEntityService00
    {

        Task<T> get01(object id);
        Task<IEnumerable<T>> getAll(bool froceFromServer = false);
        T getFast(string id);
        Task<T> post(T id);
        Task<T> update(T id);
    }

    public interface IEntityService<T, TKEY> : IEntityService01<T>, IEnumerable<T>
         where T : class, Models.IIdMapper<TKEY>
            where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {


        Task<System.Collections.IEnumerable> getAllSubTable(string masterEntityName, string collectionName, TKEY masterEnityId);

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

    


    public class EntityService< T,TKEY> : IEntityService<T,TKEY> 
        where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable 
        where T : IdMapper<TKEY>
    {


        GenericClientInt<T, TKEY> ocg;
        public EntityService()
        {
            ocg = new GenericClientInt<T, TKEY>(ClTool.WebClient.webClient, "old/");
        }

        public List<T> data = null;
        public List<Task<List<T>>> dd = new List<Task<List<T>>>();
        Task<List<T>> prv=null;

        public async Task<IEnumerable<T>> getAll(bool froceFromServer=false)
        {

            //await Task.WhenAll(tasks: dd.ToArray());
            
                while (prv!=null && !prv.IsCompleted)
                {
                    await Task.Delay(100);
                }
            

            //dd.Clear();
            if (data == null)
            {
                var z=ocg.getAll();
                //dd.Add(z);
                prv = z;
                data = await z;
            }
            return data;
        }
        public async Task<T> get(TKEY id)
        {
            await getAll();

            return getFromLoaded(id);
        }
       
         public T getFast(string id){
            
            System.Console.WriteLine("getFast :" + id);
            
            

            return data.First(x => x.id.Equals(id));
            
        }
        public T getFromLoaded(TKEY id)
        {
            if (id.GetType() == typeof(string))
            {
                if (id as string == "new")
                {
                    var newI = typeof(T).GetConstructor(new Type[] { }).Invoke(new object[] { }) as T;
                    return newI;
                }
            }
            
            
            var res = data.FirstOrDefault(x => x.id.Equals(id));
            return res;

        }
        public async Task<object> get00(object id)
        {
            return await get((TKEY)id);
        }
        public async Task<T> get01(object id)
        {
            return await get((TKEY)id);
        }
        public async Task<T> post(T input)
        {
            var x = await ocg.post(input);
            foreach (var pr in typeof(T).GetProperties())
            {
                pr.SetValue(input, pr.GetValue(x));
            }
            var idx = data.FindIndex(x => x.id.Equals(input.id));
            if(idx!=-1)
                data[idx] = input;
            data.Append(input);
            return input;
        }

        public async Task<T> update(T t)
        {
            var x = await ocg.put(t.id, t);
            foreach (var pr in typeof(T).GetProperties())
            {
                pr.SetValue(t, pr.GetValue(x));
            }
            return t;
        }

        public IEntityService<T,TKEY> getFiltered(string itemName, TKEY masterId)
        {
            throw new NotImplementedException();
        }
        public async Task<System.Collections.IEnumerable> getAllSubTable(string masterEntityName,string collectionName, TKEY masterEnityId)
        {      
                
            var d = await ocg.getAll3(masterEntityName,collectionName,masterEnityId );
            var res=new List<T>();
            foreach (var r in d)
                res.Add(r);
         
            return res;
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public T Add(T t)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> fetchFiltered(string itemName, TKEY masterId)
        {
            throw new NotImplementedException();
        }

        public Task<object> get0s(string id)
        {
            throw new NotImplementedException();
        }

        public Task<T> get1s(string id)
        {
            throw new NotImplementedException();
        }

       
    }
}