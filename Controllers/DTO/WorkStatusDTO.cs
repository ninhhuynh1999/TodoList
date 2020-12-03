using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToDoList.Controllers.DTO
{
    public class WorkStatusDTO
    {
        public int Id { get; set; }
        public string WorkStatus { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
    }
}