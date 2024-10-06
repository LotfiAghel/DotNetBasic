using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminClientViewModels
{
    public class MVoid
    {

    }
    public class FuncV<TINP, TOUT>{ 
        public TINP inp{get;set;}=default(TINP);
        public TOUT output{get;set;}=default(TOUT);
    }
    public class APIDefine<TINP, TOUT>
    {
        public FuncV<TINP, TOUT> inpOut { get; set; }
        public string url { get; set; }
    }
    public abstract class ATreeNode
    {
        public string Name { get; set; }
    }
    public class TreeNode : ATreeNode
    {
        public bool expandSubNav { get; set; }
        public List<ATreeNode> children { get; set; }
    }
    
    public class EntityModel:ATreeNode
    {
        
        public Type Type { get; set; }
    }
    public class PageViewNode:ATreeNode
    {
        
        public string url { get; set; }
        public string name { get; set; }
    }

    public class ApiViewNode:ATreeNode
    {
        
        public string url { get; set; }
        public string name { get; set; }
    }
}
