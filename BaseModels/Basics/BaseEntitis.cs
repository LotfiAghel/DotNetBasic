using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel;
using Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

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
    public abstract class Entity : IdMapper<int>
    {
        
        
        public virtual bool containsText(string txt)
        {
            if (txt == null)
                return true;
            return id.ToString().Contains(txt);
        }

        


    }


    [ShowClassHirarci]
    public abstract class Id4Entity : Entity
    {

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
