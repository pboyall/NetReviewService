//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ReviewService.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TaskNode
    {
        public int TaskId { get; set; }
        public int NodeId { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public Nullable<int> GroupId { get; set; }
        public int TaskNodeId { get; set; }
    
        public virtual Node Node { get; set; }
        public virtual ReviewTask Task { get; set; }
    }
}
