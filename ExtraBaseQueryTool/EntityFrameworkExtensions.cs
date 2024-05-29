using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;
using Models;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.DependencyInjection;

namespace AdminPanel
{
    public static class EntityFrameworkExtensions
    {
        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable query, string propertyName)
        {
            var entityType = typeof(TSource);

            //Create x=>x.PropName
            var propertyInfo = entityType.GetProperty(propertyName);
            ParameterExpression arg = Expression.Parameter(entityType, "x");
            MemberExpression property = Expression.Property(arg, propertyName);
            var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

            //Get System.Linq.Queryable.OrderBy() method.
            var enumarableType = typeof(System.Linq.Queryable);
            var method = enumarableType.GetMethods()
                 .Where(m => m.Name == "OrderBy" && m.IsGenericMethodDefinition)
                 .Where(m =>
                 {
                     var parameters = m.GetParameters().ToList();
                     return parameters.Count == 2;
                 }).Single();

            //The linq's OrderBy<TSource, TKey> has two generic types, which provided here
            MethodInfo genericMethod = method.MakeGenericMethod(entityType, propertyInfo.PropertyType);

            // Call query.OrderBy(selector), with query and selector: x=> x.PropName
            // Note that we pass the selector as Expression to the method and we don't compile it.
            // By doing so EF can extract "order by" columns and generate SQL for it
            var newQuery = (IOrderedQueryable)genericMethod.Invoke(genericMethod, new object[] { query, selector }) as IOrderedQueryable<TSource>;
            return newQuery;
        }

        public static IQueryable where0<TSource>(this IQueryable query, string propertyName)
        {
            var entityType = typeof(TSource);

            //Create x=>x.PropName
            var propertyInfo = entityType.GetProperty(propertyName);
            ParameterExpression arg = Expression.Parameter(entityType, "x");
            MemberExpression property = Expression.Property(arg, propertyName);
            var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

            //Get System.Linq.Queryable.OrderBy() method.
            var enumarableType = typeof(System.Linq.Queryable);
            var method = enumarableType.GetMethods()
                 .Where(m => m.Name == "OrderBy" && m.IsGenericMethodDefinition)
                 .Where(m =>
                 {
                     var parameters = m.GetParameters().ToList();
                     return parameters.Count == 2;
                 }).Single();

            //The linq's OrderBy<TSource, TKey> has two generic types, which provided here
            MethodInfo genericMethod = method.MakeGenericMethod(entityType, propertyInfo.PropertyType);

            // Call query.OrderBy(selector), with query and selector: x=> x.PropName
            // Note that we pass the selector as Expression to the method and we don't compile it.
            // By doing so EF can extract "order by" columns and generate SQL for it
            var newQuery = (IOrderedQueryable)genericMethod.Invoke(genericMethod, new object[] { query, selector });
            return newQuery;
        }

        public static IOrderedQueryable<TSource> OrderByDescending<TSource>(this IQueryable query, string propertyName)
        {
            var entityType = typeof(TSource);

            //Create x=>x.PropName
            var propertyInfo = entityType.GetProperty(propertyName);
            ParameterExpression arg = Expression.Parameter(entityType, "x");
            MemberExpression property = Expression.Property(arg, propertyName);
            var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

            //Get System.Linq.Queryable.OrderBy() method.
            var enumarableType = typeof(System.Linq.Queryable);
            var method = enumarableType.GetMethods()
                 .Where(m => m.Name == "OrderByDescending" && m.IsGenericMethodDefinition)
                 .Where(m =>
                 {
                     var parameters = m.GetParameters().ToList();
                     return parameters.Count == 2;
                 }).Single();

            //The linq's OrderByDescending<TSource, TKey> has two generic types, which provided here
            MethodInfo genericMethod = method.MakeGenericMethod(entityType, propertyInfo.PropertyType);

            // Call query.OrderByDescending(selector), with query and selector: x=> x.PropName
            // Note that we pass the selector as Expression to the method and we don't compile it.
            // By doing so EF can extract "order by" columns and generate SQL for it
            var newQuery = (IOrderedQueryable)genericMethod.Invoke(genericMethod, new object[] { query, selector }) as IOrderedQueryable<TSource>;
            return newQuery;
        }



        public static Tuple<int, int> convertToRange(this string range)
        {

            List<int> range2 = new List<int>() { 0, 100000000 };
            try
            {
                var j = JToken.Parse(range);
                range2 = j.ToObject<List<int>>();
            }
            catch
            {

            }
            return new(range2[0], range2[1]);
        }
        public static Tuple<string, string> convertToSort(this string sort)
        {

            try
            {

                try
                {
                    var j = JToken.Parse(sort);
                    var sort2 = j.ToObject<List<string>>();
                    return new Tuple<string, string>(sort2[0], sort2[1]);
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
            catch
            {

            }
            return null;
        }
        static object GetObject() { return null; }
        static void SetObject(object obj) { }

        static string GetString() { return ""; }
        static void SetString(string str) { }
        static public Microsoft.Extensions.DependencyInjection.IServiceCollection serviceCollection = new Microsoft.Extensions.DependencyInjection.ServiceCollection();

        public static IQueryable<TT> addSecurityFilter<TT>(this IQueryable<TT> q)
        {
            {
                IEnumerable<string> strings = new List<string>();
                // An object that is instantiated with a more derived type argument
                // is assigned to an object instantiated with a less derived type argument.
                // Assignment compatibility is preserved.
                IEnumerable<object> objects = strings;

                Func<object> del = GetString;

                // Contravariance. A delegate specifies a parameter type as string,  
                // but you can assign a method that takes an object.  
                Action<string> del2 = SetObject;
            }
            var zl = typeof(TT).GetCustomAttributes<Attribute>().Where(x => x.GetType().IsGenericType && x.GetType().GetGenericTypeDefinition() == typeof(FroceFillter<>));
            foreach (var z in zl)
            {
                var t=serviceCollection.BuildServiceProvider().GetService(z.GetType().GenericTypeArguments[0]);
                //object t = z.GetType().GenericTypeArguments[0].GetConstructor(new Type[] { }).Invoke(new object[] { });
                //if(t.GetType().IsInstanceOfType(typeof(IQuery<>)) && t.GetType().GetGenericTypeDefinition() == typeof(IQuery<>))
                {
                    var f = t.GetType().GetMethod("run");
                    if(f.IsGenericMethod)
                        f=f.MakeGenericMethod(new Type[] { typeof(TT)});
                    q = f.Invoke(t, new object[] { q}) as IQueryable<TT>;
                }
                //q = t.run(q);//have probelm with IOuery only work with IQuery2
            }

            return q;
        }
        public static IQueryable<TT> addSort<TT>(this IQueryable<TT> q, Tuple<string, string>? sr)
        {
            if (sr == null)
            {
                var z = typeof(TT).GetCustomAttributes<Attribute>().Where(x => x.GetType().IsGenericType && x.GetType().GetGenericTypeDefinition() == typeof(DefultSortAttribute<>)).FirstOrDefault();
                if (z != null)
                {
                    var t = z.GetType().GenericTypeArguments[0].GetConstructor(new Type[] { }).Invoke(new object[] { }) as IQuery<TT>;
                    q = t.run(q);
                }
            }
            else
            {
                if (sr.Item2 == "DESC")
                    q = q.OrderByDescending<TT>(sr.Item1);
                else
                    q = q.OrderBy<TT>(sr.Item1);
            }
            return q;
        }
        public static IQueryable<TT> addPagination<TT>(this IQueryable<TT> q, Tuple<int, int> range, Tuple<string,string> sr, string filter)
        {

            q=q.addSort<TT>(sr);

            var x = q.Skip(range.Item1).Take(range.Item2);
            return x;
        }




        public static IOrderedQueryable<T> Where1<T>(this IQueryable<T> queryable, string propertyName, string propertyValue)
        {
            var parameterExp = Expression.Parameter(typeof(T), "type");
            var propertyExp = Expression.Property(parameterExp, propertyName);
            var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var someValue = Expression.Constant(propertyValue, typeof(string));
            var containsMethodExp = Expression.Call(propertyExp, method, someValue);

            var expression = Expression.Lambda<Func<T, bool>>(containsMethodExp, parameterExp);

            var expcall = Expression.Call(typeof(Queryable), "Where", new[] { typeof(T) }, queryable.Expression, Expression.Quote(expression));

            return (IOrderedQueryable<T>)queryable.Provider.CreateQuery<T>(expcall);
        }
    }

}
