//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ToDoList.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class WorkList_Employee
    {
        public int Id { get; set; }
        public Nullable<int> IdEmployee { get; set; }
        public Nullable<int> IdWorkList { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual WorkList WorkList { get; set; }
    }
}
