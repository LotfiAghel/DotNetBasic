using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using ClTool;
using System;
using Newtonsoft.Json.Linq;
using System.Collections;
namespace AdminClientViewModels
{

    public class NewEntityService<T, TKEY> : IEntityService<T,TKEY>,Models.IEntityManagerW<T,TKEY>
        where T : class, Models.IIdMapper<TKEY>
        where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {

        
        
        public List<T> data = new List<T>();
        Dictionary<TKEY, T> id2En = new ();
        GenericClientInt<T, TKEY> _ocg = null;
        GenericClientInt<T, TKEY> ocg{get{
            if(_ocg==null)
                _ocg = new GenericClientInt<T,TKEY>(WebClient.webClient);   
            return _ocg;
        } }

        public int Count => data.Count;

        public T insertOrUpdate( T inp)
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
                if(setMethod!=null)
                    pr.SetValue(o, pr.GetValue(inp));

            }
            try{
                o.onChanges?.invokeAll();
            }catch{

            }
            return o;

        }
        public async Task<IReadOnlyCollection<T>> getAll(bool forceReloadFromServer=false)
        {      
            bool bigTable=typeof(T).GetCustomAttributes(typeof(Attribute),true).ToList().GetFirst<object, Models.BigTable>()!=null;
            if (bigTable)
            {
                var x = new PaginateList<T, TKEY>()
                {
                    webClient = this.ocg.webClient,
                    filter = ocg.getPath(),
                    pageSize=50,
                    
                };
                x.Count = await x.getSize();
                return x;

            }
            var d = await ocg.getAll();
            foreach (var r in d)
                insertOrUpdate(r);
         
            return data;
        }
        public async Task<IReadOnlyCollection<T>> getAll2(Models.IQuery<T> inp)
        {      
                
            var d = await ocg.getAll2(inp);
            var res=new List<T>();
            foreach (var r in d)
                res.Add(insertOrUpdate(r));
         
            return res;
        }
        public async Task<T> sendAction(TKEY entityId,Models.IAction<T> inp)
        {      
                
            var r = await ocg.sendAction(entityId,inp);
            return insertOrUpdate(r);
         

        }

        public async Task<IReadOnlyCollection<T>> getAllSubTable<TMKEY>(string masterEntityName,string collectionName, TMKEY masterEnityId)
        {      
                
            var d = await ocg.getAll3(masterEntityName,collectionName,masterEnityId);
            var res=new List<T>();
            foreach (var r in d)
                res.Add(insertOrUpdate(r));
         
            return res;
        }

        public IEntityService<T,TKEY> getFiltered(string itemName, TKEY masterId)//TODO in function khafan tar az ina bayad bashe
        {
            
            var res=new PartCollectionProxy<T, TKEY>();
            res.masterManager = this;
            
            var pr=res.pr=typeof(T).GetProperty(itemName);
            res.masterId = masterId;
            res.refresh();
            return res;
        }
        public T getFast(string id){
            T res=null;
            System.Console.WriteLine("getFast :" + id.ToString());

            if (typeof(TKEY) == typeof(Guid))
            {
                id2En.TryGetValue((TKEY)((object)Guid.Parse(id)), out res);
            }

            if (typeof(TKEY) == typeof(string))
            {
                id2En.TryGetValue((TKEY)((object)id), out res);
            }
            if (typeof(TKEY) == typeof(int))
            {
                id2En.TryGetValue((TKEY)((object)Int32.Parse( (string)((object)id)  )), out res);
            }
            
            return res;
        }
        public async Task<T> get(TKEY id)
        {

            T res=null;
            System.Console.WriteLine("get0s :" + id.ToString());
            bool bigTable=typeof(T).GetCustomAttributes(typeof(Attribute),true).ToList().GetFirst<object, Models.BigTable>()!=null;
            
            

            if (id2En.TryGetValue(id,out res))
                return res;
            try
            {
                System.Console.WriteLine("not have :" + id.ToString());
                if(bigTable){
                   res = await ocg.get(id);
                   return insertOrUpdate(res);
                }else{
                    await getAll();
                    res = await ocg.get(id);
                }
            }catch{

            }
            if (res == null)
                return null;           

            return insertOrUpdate(res);
            

            
        }
        
        public T get0(TKEY id)
        {
            T res= default(T);
            id2En.TryGetValue(id, out res);
            return res;
        }
        public async Task<T> getWithObject(object id)
        {
            
            
            if(id.GetType()==typeof(string)){
                if ((string)id == "new")
                {
                    var newI = typeof(T).GetConstructor(new Type[] { }).Invoke(new object[] { }) as T;
                    return newI;
                }
            }
            return await get((TKEY)id);
            

            
            
        }
        public T getFromLoaded(TKEY id)
        {
            T res = null;
            id2En.TryGetValue(id, out res);
                
            
            return res;

            
        }

        public async Task<object> get00(object id){
            return await getWithObject(id);
        }
        public async Task<T> post(T t)
        {


            
            var x = await ocg.post(t);

             
            return insertOrUpdate(x);
            
        }

        public async Task<T> update(T t)
        {

            var x = await ocg.put(t.id,t);
            return insertOrUpdate(x);
            

            
        }

        public IEnumerator<T> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return data.GetEnumerator();
        }

        public T Add(T t)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> fetchFiltered(string itemName, TKEY masterId)
        {
            return  (await ocg.getAll(itemName, masterId)).ConvertAll(x => insertOrUpdate(x));
            
        }

        
        public async Task<object> get0s(string id)
        {
            if (typeof(TKEY) == typeof(string))
            {
                return await getWithObject(id);
            }
            if (typeof(TKEY) == typeof(int))
            {
                return await getWithObject(Int32.Parse(id));
            }
            if (typeof(TKEY) == typeof(Guid))
            {
                return await getWithObject(Guid.Parse(id));
            }
            throw new Exception($"can not ahndle type {typeof(TKEY).Name}");
            
        }

        public async Task<T> get01(object id)
        {
            return (await get00((TKEY)id)) as T;
        }

        public Task<T> get1s(string id)
        {
            throw new NotImplementedException();
        }

        IEnumerator<ForeignKey2<T, TKEY>> IEnumerable<ForeignKey2<T, TKEY>>.GetEnumerator()
        {
            return data.ConvertAll(x=>new ForeignKey2<T,TKEY>(x.id)).GetEnumerator();
        }
    }


    public class IEnumeratorPagination<T, TKEY> : IEnumerator<T>, IEnumerator
     where T : class, Models.IIdMapper<TKEY>
     where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {
        public PaginateList<T, TKEY> parent;
        public List<T> data = new List<T>();
        public int index = -1, offset, count, pageSize = 50;


        public IEnumeratorPagination(PaginateList<T, TKEY> ocg)
        {
            this.parent = ocg;
        }

        public T Current { get{
                if (index >= offset+data.Count || index< offset)
                {
                    var r = parent.fetch(index, index + pageSize).Result;
                    data = r.data;
                    index = 0;
                    offset = r.start;
                    //TOOD get data from server
                }
                return data[index- offset];
            } 
        }

        object IEnumerator.Current
        {
            get
            {
                if (index >= offset+data.Count || index < offset)
                {
                    var r = parent.fetch(index, index + pageSize).Result;
                    data = r.data;
                    index = 0;
                    offset = r.start;
                    //TOOD get data from server
                }
                return data[index- offset];
            }
        }

        public void Dispose()
        {

        }

        public bool MoveNext()
        {
            index++;
            if (index >= parent.Count)
                return false;
            
            return true; ;
        }

        public void Reset()
        {
            index = 0;
        }
    }


    public class PaginateList<T, TKEY> : IReadOnlyCollection<T>
        where T : class, Models.IIdMapper<TKEY>
        where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {


        public WebClient webClient;
        public string filter;
        public List<T> data = new List<T>();
        public int index,pageSize;
        
        GenericClientInt<T, TKEY> _ocg = null;
        GenericClientInt<T, TKEY> ocg
        {
            get
            {
                if (_ocg == null)
                    _ocg = new GenericClientInt<T, TKEY>(WebClient.webClient);
                return _ocg;
            }
        }

        public int Count { get; set; }

        int IReadOnlyCollection<T>.Count => Count;

        public class FetchR
        {
            public List<T> data;
            public int start, end, count;
        }
        public async Task<FetchR> fetch(int index,int pageSize)
        {
            var response = await webClient.fetch014(filter + $"?range=[{index},{index + pageSize}]", null,HttpMethod.Get);
            StreamReader reader = new StreamReader(response.GetResponseStream());
            var ss = reader.ReadToEnd();
            var data=JToken.Parse(ss).ToObject<List<T>>(webClient.settings1);// (settings1);
            string returnString = await reader.ReadToEndAsync();
            for (int i = 0; i < response.Headers.Count; i++)
            {
                string name = response.Headers.GetKey(i);
                if (name != "Content-Range")
                    continue;
                string value = response.Headers.Get(i)!;
                var z=value.Split("/");
                var z0=z[0].Split("-");
                int start= int.Parse(z0[0].Split(" ")[1]);
                int end = int.Parse(z0[1]);
                int count = int.Parse(z[1]);
                return new FetchR(){
                    start=start,
                    end=end,
                    count=count,
                    data= data
                };
            }
            return new FetchR()
            {
                start = 0,
                end = 0,
                count=0,
                data = data
            };

        }
        public async Task<int> getSize()
        {
            return (await fetch(0,0)).count;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new IEnumeratorPagination<T,TKEY>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new IEnumeratorPagination<T,TKEY>(this);
        }

        public List<T> GetRange(int virtualizeOffset, int virtualizeCount)
        {
            return fetch(virtualizeOffset, virtualizeCount).Result.data;
        }
    }


}