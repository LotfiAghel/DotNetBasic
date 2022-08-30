using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel;
using Models;
using Newtonsoft.Json;
public class IgnoreDocAttribute : Attribute
{

}
namespace Models
{



    public class ChangeEventList
    {
        public Dictionary<object, List<Action>> actins = new Dictionary<object, List<Action>>();
        public void Connect(object obj, Action act)
        {
            if (!actins.ContainsKey(obj))
                actins[obj] = new List<Action>();
            actins[obj].Add(act);
        }
        public void Disconnect(object obj)
        {
            actins.Remove(obj);
        }
        public void invokeAll()
        {
            try
            {
                foreach (var x in actins)
                {
                    foreach (var y in x.Value)
                    {
                        y.Invoke();
                    }
                }
            }
            catch
            {

            }
        }
    }

    [ShowClassHirarci]
    public abstract class Entity : IIdMapper<int>
    {
        [Key]
        [PersianLabel("شناسه")]
        [ReadOnly(true)]
        public int id { get; set; }/**/
        //ID        uuid.UUID  `gorm:"primary_key" sql:"type:uuid;default:uuid_generate_v4()"json:"id"`
        public virtual bool containsText(string txt)
        {
            if (txt == null)
                return true;
            return id.ToString().Contains(txt);
        }

        [JsonIgnore]
        public ChangeEventList onChanges { get; set; }


    }


    [ShowClassHirarci]
    public abstract class Id4Entity : Entity
    {

        [PersianLabel("تاریخ ایجاد")]
        [CustomIgnoreTag(CustomIgnoreTag.Kind.CLIENT)]
        [ReadOnly(true)]
        public DateTime createdAt { get; set; }


        [PersianLabel("تاریخ ویرایش")]
        [CustomIgnoreTag(CustomIgnoreTag.Kind.ADMIN)]
        [ReadOnly(true)]
        public DateTime updatedAt { get; set; }


        [PersianLabel("تاریخ حذف")]
        [CustomIgnoreTag(CustomIgnoreTag.Kind.CLIENT)]
        [ReadOnly(true)]
        [Models.IgnoreDefultGird]
        public DateTime? deletedAt { get; set; }
        public static void cloneFromOld(Id4Entity newRow, Models.CUAT t)
        {
            newRow.createdAt = t.createdAt.HasValue ? t.createdAt.Value.UtcDateTime : DateTime.MinValue;
            newRow.updatedAt = t.updatedAt.HasValue ? t.updatedAt.Value.UtcDateTime : DateTime.MinValue;
            newRow.deletedAt = t.deletedAt.HasValue ? t.deletedAt.Value.UtcDateTime : null;
        }

        public static int CompareDinosByLength(Id4Entity x, Id4Entity y)
        {
            
            if(x.createdAt==y.createdAt){
                if(x.GetHashCode()==y.GetHashCode())
                    return 0;
                
                return x.GetHashCode()<y.GetHashCode()?-1:1;    
            }
            return x.createdAt<y.createdAt ?-1:1;
                
        }

    }

    
   

}
