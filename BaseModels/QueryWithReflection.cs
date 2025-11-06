using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Models;

public class QueryWithReflection<T> : IQuery<T>
{
    public enum OPTYPE
    {
        NONE=0,
        LTE=1,
        GTE=2,
        EQUAL=3,
        IN=4
    }

    public interface IPropQuery
    {
        public dynamic parm { get; }
    }

    public class IPropQuerySignle:IPropQuery
    {
        public dynamic parm { get; set; }
    }

    public interface  IPropQueryMulti0 : IPropQuery
    {
            
    }
    public abstract class PropQueryMulti<T2>:IPropQueryMulti0
    {
        public dynamic parm { get=>this.parm0;  }
            
        public IQuery<T2> parm0 { get; set; }
    }
    public class PropQueryEqual:IPropQuerySignle
    {
            
    }
    public class PropQueryGTE:IPropQuerySignle
    {
            
    }
    public class PropQueryLTE:IPropQuerySignle
    {
            
    }
        
    public struct OP
    {
        public OPTYPE opType { get; set; }
        public dynamic parm { get; set; }
    }
    public Dictionary<string,OP> filters { get; set; }

    public Type getPropType(string name)
    {
        return typeof(T).GetProperty(name).PropertyType;
    }
        
    public IQueryable<T> run(IQueryable<T> q)
    {
        if (filters == null || filters.Count == 0)
            return q;

        var parameter = Expression.Parameter(typeof(T), "x");
        Expression combined = null;

        foreach (var filter in filters)
        {
            var propertyName = filter.Key;
            var op = filter.Value;

            var property = Expression.PropertyOrField(parameter, propertyName);
            var constant = Expression.Constant(op.parm);

            Expression exp = null;

            switch (op.opType)
            {
                case OPTYPE.EQUAL:
                    exp = Expression.Equal(property, Expression.Convert(constant, property.Type));
                    break;

                case OPTYPE.LTE:
                    exp = Expression.LessThanOrEqual(property, Expression.Convert(constant, property.Type));
                    break;

                case OPTYPE.GTE:
                    exp = Expression.GreaterThanOrEqual(property, Expression.Convert(constant, property.Type));
                    break;

                case OPTYPE.IN:
                    // parm باید IEnumerable داشته باشه
                    var iqueryInterface = typeof(IQuery<>).MakeGenericType(property.Type);

                    if (op.parm.GetType().IsAssignableTo(iqueryInterface))
                    {
                        /* // اجرا کردن IQuery روی DbSet واقعی
                            var dbSetType = typeof(T).Assembly.GetType("YourNamespace.YourDbContext");
                            var oldDb = DependesyContainer.IServiceProvider.GetRequiredService<IAssetManager>();
                            var subQueryable = op.parm.run(oldDb.GetDbSet<object>());

                            // x => subQueryable.Contains(x.Property)
                            var containsMethod = typeof(Queryable)
                                .GetMethods()
                                .Single(m => m.Name == "Contains" && m.GetParameters().Length == 2)
                                .MakeGenericMethod(property.Type);

                            exp = Expression.Call(containsMethod, Expression.Constant(subQueryable), property);*/
                    }
                    else
                    {
                        // قبلی: parm یک IEnumerable معمولی
                        var containsMethod = typeof(Enumerable)
                            .GetMethods()
                            .Single(m => m.Name == "Contains" && m.GetParameters().Length == 2)
                            .MakeGenericMethod(property.Type);

                        exp = Expression.Call(containsMethod, Expression.Constant(op.parm), property);
                    }
                    break;

                case OPTYPE.NONE:
                default:
                    continue;
            }

            if (combined == null)
                combined = exp;
            else
                combined = Expression.AndAlso(combined, exp);
        }

        if (combined == null)
            return q;

        var lambda = Expression.Lambda<Func<T, bool>>(combined, parameter);
        return q.Where(lambda);
    }
}