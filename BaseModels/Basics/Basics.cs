using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace Models
{

   

    public interface IEntity0
    {
        object getId();
    }
    public interface IU
    {
    

        [ReadOnly(true)]
        [Models.PersianLabel("تاریخ اخرین تغیر")]
        [CustomIgnoreTag(CustomIgnoreTag.Kind.CLIENT)]
        public DateTime updatedAt { get; set; }


    
    }
    public interface ICUAT
    {
        [ReadOnly(true)]
        [Models.PersianLabel("تاریخ ساخت")]
        [CustomIgnoreTag(CustomIgnoreTag.Kind.CLIENT)]
        public DateTime createdAt { get; set; }

        //[Models.IgnoreDefultForm]

        [ReadOnly(true)]
        [Models.PersianLabel("تاریخ اخرین تغیر")]
        [CustomIgnoreTag(CustomIgnoreTag.Kind.CLIENT)]
        public DateTime? updatedAt { get; set; }


        //[Models.IgnoreDefultForm]
        [ReadOnly(true)]
        [Models.PersianLabel("تاریخ حذف")]
        [CustomIgnoreTag(CustomIgnoreTag.Kind.CLIENT)]
        public DateTime? deletedAt { get; set; }
    }

    public class SortByCreateAt<T> : IQuery<T> where T : ICUAT
    {
        public IQueryable<T> run(IQueryable<T> q)
        {
            return q.OrderBy(x => x.createdAt);
        }
    }
    public class SortByCreateAtDescending<T> : IQuery<T> where T : ICUAT
    {
        public IQueryable<T> run(IQueryable<T> q)
        {
            return q.OrderByDescending(x => x.createdAt);
        }
    }
    public class SortByCreateAtDescending2 : IQuery2<ICUAT> 
    {
        public IQueryable<T2> run<T2>(IQueryable<T2> q) where T2 : ICUAT
        {
            return q.OrderByDescending(x => x.createdAt);
        }
    }

    public abstract class CUAT:ICUAT
    {


        //[Models.IgnoreDefultForm]
        
        [ReadOnly(true)]
        [Models.PersianLabel("تاریخ ساخت")]
        [CustomIgnoreTag(CustomIgnoreTag.Kind.CLIENT)]
        public DateTime createdAt { get; set; }

        //[Models.IgnoreDefultForm]
        
        [ReadOnly(true)]
        [Models.PersianLabel("تاریخ اخرین تغیر")]
        [CustomIgnoreTag(CustomIgnoreTag.Kind.CLIENT)]
        public DateTime? updatedAt { get; set; }

        
        //[Models.IgnoreDefultForm]
        [ReadOnly(true)]
        [Models.PersianLabel("تاریخ حذف")]
        [CustomIgnoreTag(CustomIgnoreTag.Kind.CLIENT)]
        public DateTime? deletedAt { get; set; }
    }
    

    [ShowClassHirarci]
    public interface IIdMapper<T> : IEntity0 where T : IEquatable<T>, IComparable<T>, IComparable
    {
        [Key]
        [PersianLabel("شناسه")]
        [Models.IgnoreDefultForm]
        public T id { get; set; }

        [JsonIgnore]
        [NotMapped]
        public ChangeEventList onChanges { get; set; }

    }
    
    
    
    [GeneratedControllerAttribute]
    [DefultSort<SortByCreateAtDescending2>]
    [SelectAccess(AdminUserRole.SUPER_USER)]
    [ViewAccess(AdminUserRole.SUPER_USER)]
    public class EntityHistory<TKEY> : IdMapper<TKEY>  where TKEY : IEquatable<TKEY>, IComparable<TKEY>, IComparable
    {
        
        
        public Guid ?adminId { get; set; }
        
        public string entityName { get; set; }
        public TKEY entityId { get; set; }

        [Column(TypeName = "jsonb")]
        public JToken data { get; set; }

        [NotMapped]
        [JsonIgnore]//this attrbute cuse this prop hiden from AdminClient
        public IdMapper<TKEY> dataT { get => data.ToObject<IdMapper<TKEY>>();}


        public static EntityHistory<TKEY> Create<T>(IIdMapper<TKEY> e,Guid adminId)where T:IIdMapper<TKEY>
        {
            return new EntityHistory<TKEY>()
            {
                adminId = adminId,
                entityName = typeof(T).Name,
                entityId = e.id,
                createdAt = DateTime.UtcNow,
                data = JToken.FromObject(e)
            };
        }
    }

    [ShowClassHirarci]
    public abstract class AIdMapper<T> : IIdMapper<T> where T : IEquatable<T>, IComparable<T>, IComparable
    {
        [Key]
        [PersianLabel("شناسه")]
        [Models.IgnoreDefultForm]
        public T id { get; set; }
        
        
       

        [JsonIgnore]
        [NotMapped]
        public ChangeEventList onChanges { get; set; }

        public object getId()
        {
            return id;
        }
    }
    public static class IDEXT
    {
        public static Type getKeyType(this Type entity)
        {
            if (entity.IsAssignableTo(typeof(IIdMapper<string>)))
                return typeof(string);
            if (entity.IsAssignableTo(typeof(IIdMapper<Guid>)))
                return typeof(Guid);

            if (entity.IsAssignableTo(typeof(IIdMapper<int>)))
                return typeof(int);
            return null;

        }
    }
    public class ObjectContainer<T>
    {
        public T data { get; set; }
        public ObjectContainer() { }
        public ObjectContainer(T data)
        {
            this.data = data;
        }
    }

    [ShowClassHirarci]
    public class IdMapper<T> : CUAT, IIdMapper<T> where T : IEquatable<T>, IComparable<T>, IComparable
    {
        [Key]
        [PersianLabel("شناسه")]
        [ReadOnly(true)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T id { get; set; }
        //ID        uuid.UUID  `gorm:"primary_key" sql:"type:uuid;default:uuid_generate_v4()"json:"id"`

        bool Equals(T? other)
        {
            return false;
        }

        public object getId()
        {
            return id;
        }
        
        
        public IQueryable<EntityHistory<T>> History(IServiceProvider Services)
        {
            var oldDb = Services.GetRequiredService<IAssetManager>();
            return oldDb.getDbSet<EntityHistory<T>>().Where(x => x.entityName==this.GetType().Name && x.entityId.Equals(this.id)); //TODO has error in derived tables
        }

        [JsonIgnore]
        [NotMapped]
        public ChangeEventList onChanges { get; set; }
    }


    [ShowClassHirarci]
    public class OldIdMapperWithoutCUAt<T> where T : IEquatable<T>, IComparable<T>, IComparable
    {
        [Key]
        public T id { get; set; }

        bool Equals(T? other)
        {
            return false;
        }

    }



}


public class FFJ<T> where T : Models.IIdMapper<int>
{
    public int id;
    public FFJ(T t)
    {
        id = t.id;
    }
    public FFJ(int id)
    {
        this.id = id;
    }
}



public class FFJNew<T> where T : Models.Entity
{
    public int id;
    public FFJNew(T t)
    {
        id = t.id;
    }
    public FFJNew(int id)
    {
        this.id = id;
    }
}


