using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Data
{

    public enum SerilizingIgnoreTag
    {
        DATABASE = 1,
        CLUSTER = 2,
        CLIENT = 3,
    }
    public class SetDefultIgnore : Attribute
    {
    }
    public class NotIgnore : Attribute
    {
        public bool[] tags = new bool[8];
        public NotIgnore(params SerilizingIgnoreTag[] args)
        {
            if (args == null || args.Length == 0)
            {
                for (int i = 0; i < tags.Length; ++i)
                    tags[i] = true;
                return;
            }
            foreach (var t in args)
                tags[(int)t] = true;
        }
    }

    public class SerilizableObject
    {

    }

    public class TypeInfoTools
    {

    }
   

    public class CustomIgnoreTag : Attribute
    {
        public enum Kind
        {
            CLIENT = 1,
            DB = 2,
            CLUSTER = 3,
            DB_ANALYSIS = 4,
            GO_CLINT = 1000,
        }
        public HashSet<Kind> kinds;
        public CustomIgnoreTag(params Kind[] args)
        {
            kinds = new HashSet<Kind>();
            foreach (var t in args)
                kinds.Add(t);
        }

    }
   
   
    public class Class
    {
    }
}
