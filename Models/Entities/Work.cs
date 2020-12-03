using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToDoList.Models
{
    public partial class Work
    {
        public List<Employee> nguoilamchung ;


        public void setNguoiLamChung()
        {
            nguoilamchung = new List<Employee>();
            using (var db = new TodoListDBEntities())
            {
                var nguoi = from w in db.Work_Employee join e in db.Employees on w.IdEmployee equals e.Id where w.IdWork == this.Id select e;
                nguoilamchung.AddRange(nguoi.ToList());
            }
        }
    }
}