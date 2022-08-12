using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Reflection;

namespace Models
{
    public enum AdminUserRole
    {
        NONE = 0,
        SUPER_USER = 1,
        DEVELOPER = 2,
        DATA_ENTRY = 3,
        SUPPORT = 4,

    }
    public interface IentityManager<T>
    {
        T get(int id);
    }
    public interface IDB
    {
        IentityManager<T> getManager<T>();
    }

    public class DocAttr : Attribute
    {
        public string doc;
        public DocAttr(string doc)
        {
            this.doc = doc;
        }
    }
    public class RialAttr : Attribute
    {
    }
    
    public class PhoneNumAttr : Attribute
    {
    }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class BigTable : Attribute
    {
    }


    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AdminWriteBan : Attribute
    {
    }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GeneratedControllerAttribute : Attribute
    {


        public enum ClientType
        {
            ADMIN = 1,
            USER = 2,
            //OTHER =4 8 16
        }
        private List<ClientType> types;

        public GeneratedControllerAttribute(string route)
        {
            Route = route;
            //this.types=types;
        }
        public GeneratedControllerAttribute()
        {
            Route = null;
            //this.types=types;
        }

        public string Route { get; set; }
    }

    public class ShowClassHirarci : Attribute
    {
        public ShowClassHirarci()
        {

        }
    }
    public class OldName : Attribute
    {
        public string s;
        public OldName(string ss)
        {
            s = ss;
        }
    }


    

    public class ACLAtr : Attribute
    {
        /*public enum Kind
        {
            ADMIN = 1,
            DEVELOPER = 2,
            DATA_ENTRY = 3,
            SUPPORT = 4,
            CLIENT = 5,
            
        }*/
        public HashSet<AdminUserRole> kinds;
        public ACLAtr(params AdminUserRole[] args)
        {
            kinds = new HashSet<AdminUserRole>();
            foreach (var t in args)
                kinds.Add(t);
        }
    }
    public class ViewAccess : ACLAtr
    {
        public ViewAccess(params AdminUserRole[] args) : base(args)
        {
        }
    }
    public class SelectAccess : ACLAtr
    {
        public SelectAccess(params AdminUserRole[] args) : base(args)
        {
        }
    }
    public class InsertAccess : ACLAtr
    {
        public InsertAccess(params AdminUserRole[] args) : base(args)
        {
        }
    }
    public class UpdateAccess : ACLAtr
    {
        public UpdateAccess(params AdminUserRole[] args) : base(args)
        {
        }
    }
    public class DeleteAccess : ACLAtr
    {
        public DeleteAccess(params AdminUserRole[] args) : base(args)
        {
        }
    }

    public static class ListExtenction
    {
        public static T2 GetFirst<T, T2>(this List<T> t) where T : class where T2 : T
        {
            var x = t.FindAll(x => x is T2);
            if (x == null)
                return default(T2);
            if (x.Count == 0)
                return default(T2);
            return (T2)x[0];

        }
        

    }
   
    public class CollectionClearingContractResolver : MyContractResolver
{
    protected override JsonArrayContract CreateArrayContract(Type objectType)
    {
        var c = base.CreateArrayContract(objectType);
        c.OnDeserializingCallbacks.Add((obj, streamingContext) =>
        {
            var list = obj as IList;
            if (list != null && !list.IsReadOnly)
                list.Clear();
        });
        return c;
    }
}

    public class GoContractResolver : MyContractResolver
    {

        public static GoContractResolver client = new GoContractResolver() { tag = CustomIgnoreTag.Kind.CLIENT };


        [CustomIgnoreTag(CustomIgnoreTag.Kind.CLIENT)]
        public CustomIgnoreTag.Kind tag;


        public IEnumerable<T> GetCustomAttributes<T>(Type type, string name) where T : Attribute
        {
            {
                var pi = type.GetProperty(name);
                if (pi != null)
                    return pi.GetCustomAttributes<T>(false);
            }
            {
                var pi = type.GetField(name);
                if (pi != null)
                    return pi.GetCustomAttributes<T>(false);
            }
            return null;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {

            IList<JsonProperty> props = base.CreateProperties(type, memberSerialization);
            var pp = type.GetProperties();
            foreach (var prop in props)
                prop.PropertyName = "TODO_PROPERTY_NAME_ABCD"; //TODO

            return props;
        }
    }
    // Internal representation is identical to IEEE binary32 floatingpoints




    /*
    [DebuggerDisplay("{ToStringInv()}")]
    
    public readonly struct Rial2 : IComparable<Rial>, IEquatable<Rial>
    {
        private readonly long _raw;

        internal Rial(long raw)
        {
            _raw = raw;
        }
        internal Rial(string raw)
        {
            _raw = Int64.Parse(raw);
        }

        public override string ToString()
        {
            return ((long)this).ToString();
        }

        public static explicit operator Rial(long f)
        {
            return new Rial(f);
        }
        public static explicit operator Rial(string f)
        {
            return new Rial(f);
        }

        public static explicit operator long(Rial f)
        {
            return f._raw;
        }

        public static Rial operator -(Rial f)
        {
            return new Rial(-f._raw);
        }



        public static Rial operator +(Rial f1, Rial f2)
        {
            return new Rial(f1._raw + f2._raw);
        }

        public static Rial operator -(Rial f1, Rial f2)
        {
            return new Rial(f1._raw - f2._raw);
        }

        public static Rial operator *(Rial f1, int f2)
        {
            return new Rial(f1._raw * f2);
        }

        public static Rial operator /(Rial f1, int f2)
        {
            return new Rial(f1._raw / f2);
        }


        private static unsafe uint ReinterpretFloatToInt32(float f)
		{
			return *((uint*)&f);
		}

		private static unsafe float ReinterpretIntToFloat32(uint i)
		{
			return *((float*)&i);
		}

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return this.Equals((Rial)obj);
        }

        public bool Equals(Rial other)
        {
            return _raw == other._raw;
        }

        public static bool operator ==(Rial f1, Rial f2)
        {
            return f1._raw == f2._raw;
        }

        public static bool operator !=(Rial f1, Rial f2)
        {
            return !(f1 == f2);
        }

        public static bool operator <(Rial f1, Rial f2) => f1._raw < f2._raw;


        public static bool operator >(Rial f1, Rial f2) => f1._raw > f2._raw;

        public static bool operator <=(Rial f1, Rial f2) => f1._raw <= f2._raw;

        public static bool operator >=(Rial f1, Rial f2) => f1._raw >= f2._raw;

        public int CompareTo(Rial other)
        {
            if (_raw == other._raw)
                return 0;
            return _raw < other._raw ? -1 : 1;
        }




        public int CompareTo(object obj)
        {
            if (!(obj is Rial))
                throw new ArgumentException("obj");
            return CompareTo((Rial)obj);
        }
        public static Rial fromString(string s)
        {
            return new Rial(Int64.Parse(s));
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ((int)this).ToString(format, formatProvider);
        }

        public string ToString(string format)
        {
            return ((int)this).ToString(format);
        }

        public string ToString(IFormatProvider provider)
        {
            return ((int)this).ToString(provider);
        }

        public string ToStringInv()
        {
            return ((int)this).ToString(System.Globalization.CultureInfo.InvariantCulture);
        }


    }/**/
}