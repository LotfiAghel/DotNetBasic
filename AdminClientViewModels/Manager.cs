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

        Task<object> getObject(int id);
        Task<System.Collections.IEnumerable> getAllSubTable(string masterEntityName,string collectionName,int masterEnityId);
    }

    public interface IEntityService0<T> : IEntityService00, IEnumerable<T>
    {




        Task<IEnumerable<T>> getAll(bool froceFromServer=false);
        Task<T> get(int id);
        T getFast(string id);
        T getFromLoaded(int id);


        Task<T> post(T t);
        Task<T> update(T t);

        T Add(T t);

        IEntityService0<T> getFiltered(string itemName, int masterId);

        Task<IEnumerable<T>> fetchFiltered(string itemName, int masterId);
        
    }

    public interface IEntityService<TKEY, T> : IEntityService0<T> where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable where T : IdMapper<TKEY>
    {

        Task<T> get(TKEY id);

    }


    public class EntityService<TKEY, T> : IEntityService<TKEY, T> where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable where T : IdMapper<TKEY>
    {


        GenericClientInt<T> ocg;
        public EntityService()
        {
            ocg = new GenericClientInt<T>(ClTool.WebClient.webClient, "old/");
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

            return data.First(x => x.id.Equals(id));
        }
        public async Task<T> get(int id)
        {
            
            await getAll();
            return getFromLoaded(id);


        }
         public T getFast(string id){
            
            System.Console.WriteLine("getFast :" + id.ToString());
            
            

            return data.First(x => x.id.Equals(id));
            
        }
        public T getFromLoaded(int id)
        {
            if (id.GetType() == typeof(string))
            {
                if (id == -11)
                {
                    var newI = typeof(T).GetConstructor(new Type[] { }).Invoke(new object[] { }) as T;
                    return newI;
                }
            }
            
            
            var res = data.FirstOrDefault(x => x.id.Equals(id));
            if (res == null)
                return res;
            return res;

        }
        public async Task<object> getObject(int id)
        {
            return await get(id);
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

        public IEntityService0<T> getFiltered(string itemName, int masterId)
        {
            throw new NotImplementedException();
        }
        public async Task<System.Collections.IEnumerable> getAllSubTable(string masterEntityName,string collectionName,int masterEnityId)
        {      
                
            var d = await ocg.getAll3(masterEntityName,collectionName,masterEnityId);
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

        T IEntityService0<T>.Add(T t)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<T>> IEntityService0<T>.fetchFiltered(string itemName, int masterId)
        {
            throw new NotImplementedException();
        }
    }
}