using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class IntIntPair
    {
        public int key { get; set; }
        public int count { get; set; }
    }
    public class IntRangeToIntPair
    {
        public IntRangeRequest key { get; set; }
        public int count { get; set; }


    }
    public struct Range<T>
    { //TODO must be struct
        
        public Range(T v, T count) 
        {
            this.start = v;
            this.end = count;
        }

        public T start { get; set; }
        public T end { get; set; }
    }
    public struct IntRange: IEnumerator<int>
    {
        public IntRange(Range<int> range) : this()
        {
            this.range = range;
        }

        public Range<int> range { get; set; }

        public int Current { get; set; }

        object IEnumerator.Current => throw new NotImplementedException();

        public void Dispose()
        {
            
        }

        public bool MoveNext()
        {
            Current++;
            return Current < range.end;
        }

        public void Reset()
        {
            Current=range.start;
        }
    }
    public struct TimeRangeRequest
    {
        public DateTime start { get; set; }
        public DateTime end { get; set; }


        public void setLastDays(int days)
        {
            start = DateTime.UtcNow.AddDays(-days);
            end = DateTime.UtcNow;
        }
        public void setLast30Days() => setLastDays(30);
    }
    public class IntRangeRequest
    {
        public int start { get; set; }
        public int end { get; set; }
        public override string ToString() => $"[{start},{end}]";
    }

    public interface IEntityManagerW<T, TKEY>
       where T : class, Models.IIdMapper<TKEY>
       where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {

        T get0(TKEY id);
    }


    public interface IAssetManager
    {


        IEntityManagerW<T, TKEY> getManager<T, TKEY>()
            where T : class, Models.IIdMapper<TKEY>
            where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable;
        IQueryable<T> getDbSet<T>() where T : class;

        T2 getDbSet2<T2>() where T2 : class;

        public static IAssetManager instance;

    }


    public interface IMyServiceManager
    {

        IAssetManager getDB();


    }
    public class MyServiceManager2
    {

        public IAssetManager db;


    }
    public interface IQuery0
    {

    }
    public interface IQuery<T>: IQuery0
    {

        IQueryable<T> run(IQueryable<T> q);
        

    }
    

    public interface IQuery2<T> : IQuery0
    {

        
        IQueryable<T2> run<T2>(IQueryable<T2> q) where T2 : T;

    }


    public class FroceFillter<T> : Attribute where T : IQuery0
    {
        
        public HashSet<CustomIgnoreTag.Kind> kinds;
        public FroceFillter(params CustomIgnoreTag.Kind[] args)
        {
            kinds = new HashSet<CustomIgnoreTag.Kind>();
            foreach (var t in args)
                kinds.Add(t);
        }

    }
    
    
    public class DefultSortAttribute<T> : Attribute where T : IQuery0 
    {
        
    }
    public interface IQueryConvertor<T,T2>
    {

        IQueryable<T2> run(IQueryable<T> q);
    }


    public interface IAction<T> //where T :Entity
    {
        //public ForeignKey<T> foreignKey;       
        public Task<T> run(T entity, IServiceProvider Services);
    }

    public class IQueryContainer<T>{
        public IQuery<T> query { get; set; }
    }
}
