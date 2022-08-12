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

    public class PartCollectionProxy<T>: IEntityService0<T> {
        public List<T> data;
        public System.Reflection.PropertyInfo pr;
        public int masterId;
        public IEntityService0<T> masterManager;

        public int Count => ((ICollection<T>)data).Count;

        public bool IsReadOnly => ((ICollection<T>)data).IsReadOnly;

        public T Add(T t){
            pr.SetValue(t, masterId);
            data.Add(t);
            return t;
        }

        public Task<T> get(int id)
        {
            return masterManager.get(id);
        }
        public T getFast(string id){
            return masterManager.getFast(id);
           
        }
        public T getFromLoaded(int id)
        {
            return masterManager.getFromLoaded(id);
        }
        public void refresh()
        {
            data = new List<T>();
            foreach (var row in masterManager)
            {
                if (pr.GetValue(row).Equals(masterId))
                    data.Add(row);
            }
        }
        public async Task<IEnumerable<T>> getAll(bool froceReloadFromServer)
        {
            await masterManager.fetchFiltered(pr.Name,masterId);
            refresh();
            return data;
        }


        public IEntityService0<T> getFiltered(string itemName, int masterId)
        {
            return masterManager.getFiltered(itemName,masterId);
        }

        public Task<object> getObject(int id)
        {
            return masterManager.getObject(id);
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

        public async Task<IEnumerable<T>> fetchFiltered(string itemName, int masterId)
        {
            return data;
        }
        public async Task<System.Collections.IEnumerable> getAllSubTable(string masterEntityName,string collectionName,int masterEnityId)
        {      
                
           return data;
        }
    }


    public class NewEntityService<T> : IEntityService0<T>,Models.IEntityManagerW<T> where T : Entity
    {

        
        
        public List<T> data = new List<T>();
        Dictionary<int, T> id2En = new Dictionary<int, T>();
        GenericClientInt<T> _ocg=null;
        GenericClientInt<T> ocg{get{
            if(_ocg==null)
                _ocg = new GenericClientInt<T>(WebClient.webClient);   
            return _ocg;
        } }
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
                o.onChanges.invokeAll();
            }catch{

            }
            return o;

        }
        public async Task<IEnumerable<T>> getAll(bool forceReloadFromServer=false)
        {      
            bool bigTable=typeof(T).GetCustomAttributes(typeof(Attribute),true).ToList().GetFirst<object, Models.BigTable>()!=null;
            if(bigTable)    
                return new List<T>();
            var d = await ocg.getAll();
            foreach (var r in d)
                insertOrUpdate(r);
         
            return data;
        }
        public async Task<IEnumerable<T>> getAll2(Models.IQuery<T> inp)
        {      
                
            var d = await ocg.getAll2(inp);
            var res=new List<T>();
            foreach (var r in d)
                res.Add(insertOrUpdate(r));
         
            return res;
        }
        public async Task<T> sendAction(int entityId,Models.IAction<T> inp)
        {      
                
            var r = await ocg.sendAction(entityId,inp);
            return insertOrUpdate(r);
         

        }

        public async Task<System.Collections.IEnumerable> getAllSubTable(string masterEntityName,string collectionName,int masterEnityId)
        {      
                
            var d = await ocg.getAll3(masterEntityName,collectionName,masterEnityId);
            var res=new List<T>();
            foreach (var r in d)
                res.Add(insertOrUpdate(r));
         
            return res;
        }

        public IEntityService0<T> getFiltered(string itemName,int masterId)//TODO in function khafan tar az ina bayad bashe
        {
            
            var res=new PartCollectionProxy<T>();
            res.masterManager = this;
            
            var pr=res.pr=typeof(T).GetProperty(itemName);
            res.masterId = masterId;
            res.refresh();
            return res;
        }
        public T getFast(string id){
            T res=null;
            System.Console.WriteLine("getFast :" + id.ToString());
            
            

            id2En.TryGetValue(Int32.Parse(id),out res);
            return res;
        }
        public async Task<T> get(int id)
        {

            T res=null;
            System.Console.WriteLine("get :" + id.ToString());
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
        
        public T get0(int id)
        {
            T res= default(T);
            id2En.TryGetValue(id, out res);
            return res;
        }
        public async Task<T> get(object id)
        {
            
            
            if(id.GetType()==typeof(string)){
                if ((string)id == "new")
                {
                    var newI = typeof(T).GetConstructor(new Type[] { }).Invoke(new object[] { }) as T;
                    return newI;
                }
                id =int.Parse((string)id);
            }



            return await get((int)id);
            
        }
        public T getFromLoaded(int id)
        {
            T res = null;
            id2En.TryGetValue((int)id, out res);
                
            
            return res;

            
        }

        public async Task<object> getObject(int id){
            return await get(id);
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

        T IEntityService0<T>.Add(T t)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> fetchFiltered(string itemName, int masterId)
        {
            return  (await ocg.getAll(itemName, masterId)).ConvertAll(x => insertOrUpdate(x));
            
        }

       
    }
}