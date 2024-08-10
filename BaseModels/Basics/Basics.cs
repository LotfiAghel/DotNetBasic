using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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


