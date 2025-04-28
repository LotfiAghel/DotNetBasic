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
        
        
        [Column(TypeName = "jsonb")]
        public JToken dif { get; set; }

        [NotMapped]
        [JsonIgnore]//this attrbute cuse this prop hiden from AdminClient
        public IdMapper<TKEY> dataT { get => data.ToObject<IdMapper<TKEY>>();}

        public static JToken GetJsonDiff(JToken first, JToken second)
        {
            if (JToken.DeepEquals(first, second))
                return null;
            
            if(first==null)
                return second;
            
            if (first.Type != second.Type)
                return second;

            if (first is JObject obj1 && second is JObject obj2)
            {
                var diffObj = new JObject();
                foreach (var property in obj2.Properties())
                {
                    var propName = property.Name;
                    var firstProp = obj1.Property(propName);

                    var diff = GetJsonDiff(firstProp?.Value, property.Value);
                    if (diff != null)
                    {
                        diffObj[propName] = diff;
                    }
                }
                return diffObj;
            }
            else if (first is JArray arr1 && second is JArray arr2)
            {
                var diffArray = new JArray();
                int maxLength = Math.Max(arr1.Count, arr2.Count);

                for (int i = 0; i < maxLength; i++)
                {
                    if (i >= arr1.Count)
                    {
                        // آیتم جدید اضافه شده
                        diffArray.Add(arr2[i]);
                    }
                    else if (i >= arr2.Count)
                    {
                        // آیتمی حذف شده (میتونی حذف شده‌ها رو مدیریت کنی اگه خواستی)
                        diffArray.Add(null);
                    }
                    else
                    {
                        var diff = GetJsonDiff(arr1[i], arr2[i]);
                        diffArray.Add(diff);
                    }
                }

                // اگر تمام دیف ها null بودن، یعنی آرایه بدون تغییر بوده
                if (diffArray.All(x => x == null))
                    return null;

                return diffArray;
            }
            else
            {
                return second;
            }
        }

        public static EntityHistory<TKEY> Create<T>(TKEY id, JToken e,Guid adminId,JToken dd)where T:IIdMapper<TKEY>
        {
            var m = e;
            
            return new EntityHistory<TKEY>()
            {
                adminId = adminId,
                entityName = typeof(T).Name,
                entityId = id,
                createdAt = DateTime.UtcNow,
                data = m,
                dif = GetJsonDiff(dd, m)
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
    public static class TypeHelper
    {
        public static List<Type> GetBaseClasses(Type type)
        {
            var bases = new List<Type>();

            while (type.BaseType != null)
            {
                type = type.BaseType;
                bases.Add(type);
            }

            return bases;
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
            var tyn=TypeHelper.GetBaseClasses(this.GetType()).Select(x=>x.Name);
            return oldDb.getDbSet<EntityHistory<T>>().Where(x => tyn.Contains(x.entityName) 
                                                                 && x.entityId.Equals(this.id)).OrderByDescending(x=> x.createdAt); //TODO has performance issue becus of derived tables
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


