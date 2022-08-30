using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Net.Mime.MediaTypeNames;

namespace Models
{

    public static class DBSetExtenction
    {


        public static async ValueTask<TEntity> MyFindAsync<TEntity>(this DbSet<TEntity> t, int id) where TEntity : Entity
        {
            return await t.FindAsync(id);
        }

        public static async ValueTask<TEntity> MyFindAsync2<TEntity>(this DbSet<TEntity> t, ForeignKey<TEntity> id) where TEntity : Entity
        {
            return await t.FindAsync(id.Value);
        }
        public static TEntity MyFind2<TEntity>(this DbSet<TEntity> t, ForeignKey<TEntity> id) where TEntity : Entity
        {
            return t.Find(id.Value);
        }
    }
   
    public interface IEntityManagerW<T,TKEY> 
        where T : Models.IIdMapper<TKEY>
        where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {
        Task<T> get(TKEY id);
        T get0(TKEY id);
    }


    public interface IAssetManager{


        IEntityManagerW<T, TKEY> getManager<T, TKEY>()
            where T: Models.IIdMapper<TKEY> 
            where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable; 
        IQueryable<T> getDbSet<T>()where T : class;

        T2 getDbSet2<T2>()where T2 : class;

        public static IAssetManager  instance;
        
    }
 

    public interface IMyServiceManager{
        
        IAssetManager getDB();
        

    }
    public class MyServiceManager2{
        
        public IAssetManager db;
        

    }
    public class FModelBuilder : IInfrastructure<IConventionModelBuilder>
    {
        IConventionModelBuilder IInfrastructure<IConventionModelBuilder>.Instance => throw new NotImplementedException();
    }
    public class FCollectionNavigationBuilder<TEntity, TRelatedEntity> : CollectionNavigationBuilder<TEntity, TRelatedEntity>
     where TEntity : class
        where TRelatedEntity : class
    {
        public FCollectionNavigationBuilder(IMutableEntityType declaringEntityType, IMutableEntityType relatedEntityType, MemberIdentity navigation, IMutableForeignKey foreignKey, IMutableSkipNavigation skipNavigation)
        : base(declaringEntityType, relatedEntityType, navigation, foreignKey, skipNavigation)
        {

        }

    }
    /*public class FEntityTypeBuilder<TEntity> : EntityTypeBuilder<TEntity> where TEntity : class
    {
#pragma warning disable EF1001 // Internal EF Core API usage.
        public FEntityTypeBuilder(IMutableEntityType entityType) : base(entityType) { }
        public override CollectionNavigationBuilder<TEntity, TRelatedEntity> HasMany<TRelatedEntity>(Expression<Func<TEntity, IEnumerable<TRelatedEntity>>> navigationExpression = null) where TRelatedEntity : class
        {
            return new CollectionNavigationBuilder<TEntity, TRelatedEntity>(null, null, null, null, null);
        }
#pragma warning restore EF1001 // Internal EF Core API usage.
    }/**/


}