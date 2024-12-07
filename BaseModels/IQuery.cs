using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
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
        
        public Range(T v, T end) 
        {
            this.start = v;
            this.end = end;
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
    public interface IQuery<T,T2>: IQuery0
    {

        IQueryable<T2> run(IQueryable<T> q);
        

    }
    public interface IQuery<T>: IQuery<T,T>
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

        public static ConcurrentDictionary<Guid, HashSet<Guid>> techer2Student = new ConcurrentDictionary<Guid, HashSet<Guid>>();
        public static System.Guid getUser2Id(this HttpContext a)
        {
            object res = null;
            object connectionId = null;
            if (a.Items.TryGetValue("connectionId", out res) && res is Guid)
                return (Guid)res;
            if (a.Items.TryGetValue("user2Id", out res) && res is Guid)
            {
                var stId=(Guid)res;
                if(techer2Student[getUserId()].Contains(stId))
                    return stId;
            }

            return a.getUserId();

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
        public static Expression<Func<T, bool>> Equal(Expression<Func<T, Guid>> prop, Guid userId)
        {
            /*var value = new BaseUser();
            foreach (var x in value.GetType().GetProperties())
                Console.WriteLine($"{x.Name} = {x.GetValue(value)}");*/

            
            //x => x.LeitnerCard.CustomerId

            //x => x.LeitnerCard.CustomerI == keyword
            //  => Equals(x.LeitnerCard.CustomerI , keyword)
            // a+b = +(body,key)
            return Expression.Lambda<Func<T, bool>>(
                Expression.Call(
                    typeof(DbFunctionsExtensions),
                    nameof(DbFunctionsExtensions.Equals),
                    null,
                    Expression.Constant(EF.Functions),
                    prop.Body,
                    Expression.Constant(userId)),
                prop.Parameters);
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
            var concatMethod = typeof(Guid).GetMethod(nameof(Guid.Equals), new[] { typeof(Guid), typeof(Guid) });


            var userId = _httpContextAccessor.HttpContext.getUserId();
            //var t = q.Where(x => x.leitnerCardUser.CustomerId == userId);

            var t2 = q.Where(Equal(access, userId));

            //var t3 = q.Where(Equal2(UserLeitnerCardCheckPoint.access2, userId));
            return t2;
        }
        public static Expression<Func<T, Guid>> access;


    }

}
