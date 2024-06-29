using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

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
    public abstract class FroceFillter0 : Attribute 
    {
        public HashSet<CustomIgnoreTag.Kind> kinds;
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class FroceFillter<T> : FroceFillter0 where T : IQuery0
    {
        
        
        public FroceFillter(params CustomIgnoreTag.Kind[] args)
        {
            kinds = new HashSet<CustomIgnoreTag.Kind>();
            foreach (var t in args)
                kinds.Add(t);
        }

    }
    public class FroceAction<T> : Attribute where T : IAction0
    {

        public HashSet<CustomIgnoreTag.Kind> kinds;
        public FroceAction(params CustomIgnoreTag.Kind[] args)
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
    public interface IAction0 { }

    public interface IAction<T>: IAction0 //where T :Entity
    {
        //public ForeignKey<T> foreignKey;       
        public Task<T> run(T entity, IServiceProvider Services);
    }

    public class IQueryContainer<T>{
        public IQuery<T> query { get; set; }
    }

    public static class EXTENCTION
    {

        public static System.Guid getUserId(this HttpContext a)
        {
            object res = null;
            if (a.Items.TryGetValue("userId", out res) && res is Guid)
                return (Guid)res;
            return Guid.Empty;

        }

    }
    public class AccessCheckerVersion1<T> : IQuery<T>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccessCheckerVersion1(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public static Expression<Func<Guid, Guid, bool>> equal = (x, y) => x == y;
        public static Expression<Func<T, bool>> Equal(Expression<Func<T, Guid>> prop, Guid keyword)
        {
            var exp = Expression.Call(
                     Expression.Constant(keyword),
                    nameof(DbFunctionsExtensions.Equals),
                    null//new Type[] { typeof(Guid),typeof(Guid)}
                    ,
                    //Expression.Constant(EF.Functions),
                    prop.Body
                   );
            return Expression.Lambda<Func<T, bool>>(
                exp,
                prop.Parameters);
        }
        public static Expression<Func<T, bool>> Equal3(Expression<Func<T, Guid,bool>> accessCheck, Guid keyword)
        {
            var exp = Expression.Call(
                     Expression.Constant(keyword),
                    nameof(DbFunctionsExtensions.Equals),
                    null//new Type[] { typeof(Guid),typeof(Guid)}
                    ,
                    //Expression.Constant(EF.Functions),
                    accessCheck.Body
                   );
            return Expression.Lambda<Func<T, bool>>(
                accessCheck,
                accessCheck.Parameters);
        }
        public static Expression<Func<T, bool>> Equal2(Expression<Func<T, Guid, bool>> prop, Guid keyword)
        {
            var parameters = prop.Parameters.ToList();
            parameters.RemoveAll(x => x.Name == "userId");
            //parameters[1] = ParameterExpression.();
            return Expression.Lambda<Func<T, bool>>(
                prop.Body,
                parameters);
        }
        public IQueryable<T> run(IQueryable<T> q)
        {
            


            var userId = _httpContextAccessor.HttpContext.getUserId();
            
            var t2 = q.Where(Equal(access, userId));// thats work well
            
            //var t2 = q.Where(Equal3(accessCheck, userId));
            return t2;
        }
        public static Expression<Func<T, Guid>> access;
        //public static Expression<Func<T, Guid,bool>> accessCheck;
    }

}
