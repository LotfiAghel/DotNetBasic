using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Models;

public class QueryWithReflection<T> : IQuery<T>
{

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
        public IQuery0 parm0 { get;  }
    }
    public abstract class PropQueryMulti<T2>:IPropQueryMulti0
    {
        public dynamic parm { get=>this.parm2;  }
        public IQuery0 parm0 { get=>this.parm2;   }
        public IQuery<T2> parm2 { get; set; }
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
        
   
    public Dictionary<string,IPropQuery> filters { get; set; }

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
            var propQuery = filter.Value;

            var property = Expression.PropertyOrField(parameter, propertyName);
            Expression exp = null;

            if (propQuery is PropQueryEqual equalQuery)
            {
                var constant = Expression.Constant(equalQuery.parm);
                exp = Expression.Equal(property, Expression.Convert(constant, property.Type));
            }
            else if (propQuery is PropQueryLTE lteQuery)
            {
                var constant = Expression.Constant(lteQuery.parm);
                exp = Expression.LessThanOrEqual(property, Expression.Convert(constant, property.Type));
            }
            else if (propQuery is PropQueryGTE gteQuery)
            {
                var constant = Expression.Constant(gteQuery.parm);
                exp = Expression.GreaterThanOrEqual(property, Expression.Convert(constant, property.Type));
            }
            else if (propQuery is IPropQueryMulti0 multiQuery)
            {
                // Assuming multiQuery.parm0 is IQuery<T2> and we want to build Contains expression
                var iqueryType = multiQuery.parm0.GetType();
                var elementType = iqueryType.GetGenericArguments().FirstOrDefault() ?? property.Type;

                var iqueryInterface = typeof(IQuery<>).MakeGenericType(elementType);

                if (multiQuery.parm0.GetType().IsAssignableTo(iqueryInterface))
                {
                    // We do not have the actual DbSet here, so we skip the subquery execution
                    // This part can be implemented if the context is available
                    // For now, skip or throw NotImplementedException
                    throw new NotImplementedException("IQuery subquery execution is not implemented.");
                }
                else
                {
                    // If parm0 is IEnumerable
                    var containsMethod = typeof(Enumerable)
                        .GetMethods()
                        .Single(m => m.Name == "Contains" && m.GetParameters().Length == 2)
                        .MakeGenericMethod(property.Type);

                    exp = Expression.Call(containsMethod, Expression.Constant(multiQuery.parm0), property);
                }
            }
            else
            {
                // Unknown IPropQuery type, skip
                continue;
            }

            if (exp == null)
                continue;

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