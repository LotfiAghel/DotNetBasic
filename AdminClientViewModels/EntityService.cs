using Models;
using ClTool;
namespace AdminClientViewModels
{
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
        Dictionary<TKEY, T> id2En = new();
        public List<Task<List<T>>> dd = new List<Task<List<T>>>();
        Task<List<T>> prv=null;

        public int Count => data.Count;
        public T insertOrUpdate(T inp)
        {
            T o;
            if (!id2En.TryGetValue(inp.id, out o))
            {
                data.Add(inp);
                id2En[inp.id] = inp;
                return inp;
            }
            foreach (var pr in typeof(T).GetProperties())
            {
                var setMethod = pr.GetSetMethod();
                if (setMethod != null)
                    pr.SetValue(o, pr.GetValue(inp));

            }
            try
            {
                o.onChanges?.invokeAll();
            }
            catch
            {

            }
            return o;

        }
        public async Task<IReadOnlyCollection<T>> getAll(bool froceFromServer=false)
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
        public ICollection<T> getAllFast()
        {





            return data;

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
        public async Task<IReadOnlyCollection<T>> getAllSubTable<TMASTER, TMKEY>(string collectionName, TMKEY masterEnityId)
        {      
                
            var d = await ocg.getAll3<TMASTER, TMKEY>(collectionName,masterEnityId );
            
            var res=new List<T>();
            foreach (var r in d)
            {
                //IEntityService<TMKEY>.instance.addOrUpdate(r);
                res.Add(r);
            }
         
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

        IEnumerator<ForeignKey2<T, TKEY>> IEnumerable<ForeignKey2<T, TKEY>>.GetEnumerator()
        {
            return data.ConvertAll(x => new ForeignKey2<T, TKEY>(x.id)).GetEnumerator();
        }
    }
}