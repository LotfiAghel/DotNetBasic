using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{

   

    public interface IEntity0
    {

    }

    public class CUAT
    {


        //[Models.IgnoreDefultForm]
        [IgnoreDefultGird]
        [ReadOnly(true)]
        [Models.PersianLabel("تاریخ ساخت")]
        public DateTimeOffset? createdAt { get; set; }

        //[Models.IgnoreDefultForm]
        [IgnoreDefultGird]
        [ReadOnly(true)]
        [Models.PersianLabel("تاریخ اخرین تغیر")]
        public DateTimeOffset? updatedAt { get; set; }

        [IgnoreDefultGird]
        //[Models.IgnoreDefultForm]
        [ReadOnly(true)]
        [Models.PersianLabel("تاریخ حذف")]
        public DateTimeOffset? deletedAt { get; set; }
    }


    [ShowClassHirarci]
    public interface IIdMapper<T> : IEntity0 where T : IEquatable<T>, IComparable<T>, IComparable
    {
        [Key]
        [PersianLabel("شناسه")]
        [Models.IgnoreDefultForm]
        public T id { get; set; }

        [JsonIgnore]
        public ChangeEventList onChanges { get; set; }

    }

    [ShowClassHirarci]
    public class IdMapper<T> : CUAT, IIdMapper<T> where T : IEquatable<T>, IComparable<T>, IComparable
    {
        [Key]
        [PersianLabel("شناسه")]
        [ReadOnly(true)]
        public T id { get; set; }
        //ID        uuid.UUID  `gorm:"primary_key" sql:"type:uuid;default:uuid_generate_v4()"json:"id"`

        bool Equals(T? other)
        {
            return false;
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


