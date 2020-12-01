using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class EmployeeController : Controller
    {
        private TodoListDBEntities db = new TodoListDBEntities();
        // GET: Employee
        public ActionResult Index()
        {
            return RedirectToAction("Id");
            return View();
        }
        public ActionResult Id(int? id)
        {
            if (Session["IdUser"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            
            if (id == null)
            {
                id = int.Parse(Session["IdUser"].ToString());
            }
            if (id == int.Parse(Session["IdUser"].ToString()))
            {
                var result1 = from wl in db.WorkLists
                              join wle in db.WorkList_Employee on wl.Id equals wle.IdWorkList
                              where wle.IdEmployee == id
                              select wl;
                ViewData["dsWorklist"] = result1.ToList();
            }
            else
            {
                var result1 = from wl in db.WorkLists
                              join wle in db.WorkList_Employee on wl.Id equals wle.IdWorkList
                              where wle.IdEmployee == id && wl.IdWorkListStatus == 1
                              select wl;
                ViewData["dsWorklist"] = result1.ToList();
            }
            var employees = db.Employees.ToList();

            /* var result = from wl in db.WorkLists
                          join wle in db.WorkList_Employee on wl.Id equals wle.IdWorkList
                          where wle.IdEmployee == id
                          select wl;*/

            Session["Id"] = id;
            ViewData["dsEmployee"] = employees;
            return View();
        }

        public ActionResult Addworklist(int? id)
        {
            if (Session["IdUser"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (id == null)
            {
                id = int.Parse(Session["IdUser"].ToString());
            }

            var employees = db.Employees.ToList();



            
            ViewData["dsEmployee"] = employees;
            return View();
        }
        // tao worklist 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Addworklist(string Add_WorkListName, string Add_WorkListStatus,string add_id_employee)
        {
            if (ModelState.IsValid)
            {
                WorkList workList = new WorkList() { WorkListName = Add_WorkListName, IdWorkListStatus = int.Parse(Add_WorkListStatus),DateCreated=DateTime.Today };
                db.WorkLists.Add(workList);
                db.SaveChanges();

                WorkList_Employee workList_Employee = new WorkList_Employee() { IdEmployee = int.Parse(add_id_employee), IdWorkList = workList.Id,DateCreated= DateTime.Today };
                db.WorkList_Employee.Add(workList_Employee);
                db.SaveChanges();
                return RedirectToAction("Id/"+ add_id_employee);
            }
            return RedirectToAction("Id", "employee");

        }
        //xoa worklist
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteWorklist(string id_delete)
        {
            if (ModelState.IsValid)
            {
                int id = int.Parse(id_delete);
                WorkList temp = db.WorkLists.Where(wl => wl.Id == id ).FirstOrDefault();
                db.WorkLists.Remove(temp);
                db.SaveChanges();
                return RedirectToAction("Id/" + Session["Id"].ToString());
            }
            return Redirect(Request.UrlReferrer.ToString());

        }
        // edit worklist 
        public ActionResult Editworklist(int? id)
        {
            if (Session["IdUser"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (id == null)
            {
                return RedirectToAction("Id/"+ Session["Id"].ToString());
            }

            var employees = db.Employees.ToList();
            var worklist = db.WorkLists.Where(w => w.Id == id).FirstOrDefault();



            ViewData["dsEmployee"] = employees;
            ViewData["worklist"] = worklist;
            return View();
        }
        //edit worklist
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editworklist(string Edit_WorkListName,string Edit_WorkListStatus,string Edit_id_employee,string Edit_WorkListId)
        {
            if (ModelState.IsValid)
            {
                int id = int.Parse(Edit_WorkListId);
                int status = int.Parse(Edit_WorkListStatus);
                WorkList temp = new WorkList() { Id = id, WorkListName = Edit_WorkListName, IdWorkListStatus = status };
                db.Entry(temp).State = EntityState.Modified;

                // Do some more work...  
                db.SaveChanges();

                return RedirectToAction("Id/" + Edit_id_employee);
            }
            return View();

        }
        // hien thi chi tiet worklist
        public ActionResult Detailworklist(int? id)
        {
            if (Session["IdUser"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (id == null)
            {
                return RedirectToAction("Id/" + Session["Id"].ToString());
            }
            var employees = db.Employees.ToList();
            var danhsachcanlam = from c in db.Works where c.IdWorkList == id && c.IdWorkStatus == 1 select c;
            var danhsachdanglam = from c in db.Works where c.IdWorkList == id && c.IdWorkStatus == 2 select c;
            var danhsachhoanthanh = from c in db.Works where c.IdWorkList == id && c.IdWorkStatus == 3 select c;
            var danhsachtrehan = from c in db.Works where c.IdWorkList == id && c.IdWorkStatus == 4 select c;
            var notInWorklist = from c in db.Employees
                                where c.IdRole== 2 && !(from o in db.WorkList_Employee where o.IdWorkList == id select o.IdEmployee).Contains(c.Id) 
                                select c;
            var InWorklist = from c in db.Employees 
                             where c.IdRole == 2 && (from o in db.WorkList_Employee where o.IdWorkList == id select o.IdEmployee).Contains(c.Id)
                             select c;

            foreach(var item in danhsachcanlam)
            {
                item.setNguoiLamChung();        
            }
            foreach (var item in danhsachdanglam)
            {
                item.setNguoiLamChung();
            }
            foreach (var item in danhsachhoanthanh)
            {
                item.setNguoiLamChung();
            }
            foreach (var item in danhsachtrehan)
            {
                item.setNguoiLamChung();
            }

            ViewData["canlam"] = danhsachcanlam.ToList();
            ViewData["danglam"] = danhsachdanglam.ToList();
            ViewData["hoanthanh"] = danhsachhoanthanh.ToList();
            ViewData["trehan"] = danhsachtrehan.ToList();
            ViewData["notInWorkList"] = notInWorklist.ToList();
            ViewData["inWorkList"] = InWorklist.ToList();

            ViewData["idWorklist"] = id;

            ViewData["dsEmployee"] = employees;
            Session["WorkListId"] = id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemNguoiLamChung_worklist(string id_add_1_employee, string id_add_1_worklist)
        {
            if (ModelState.IsValid)
            {
                int id_emp = int.Parse(id_add_1_employee);
                int id_worklist= int.Parse(id_add_1_worklist);

                WorkList_Employee temp = new WorkList_Employee() { IdEmployee = id_emp, IdWorkList = id_worklist, DateCreated = DateTime.Today };
                db.WorkList_Employee.Add(temp);
                db.SaveChanges();
                return RedirectToAction("Detailworklist/" + id_add_1_worklist);
            }
            return RedirectToAction("Detailworklist/" + id_add_1_worklist);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XoaNguoiLamChung_worklist(string id_delete_1_employee, string id_delete_1_worklist)
        {
            if (ModelState.IsValid)
            {
                int id_emp = int.Parse(id_delete_1_employee);
                int id_worklist = int.Parse(id_delete_1_worklist);

                /*WorkList_Employee temp = new WorkList_Employee() { IdEmployee = id_emp, IdWorkList = id_worklist, DateCreated = DateTime.Today };*/
                var temp=db.WorkList_Employee.Where(w => w.IdEmployee == id_emp && w.IdWorkList == id_worklist).FirstOrDefault();
                db.WorkList_Employee.Remove(temp);
                db.SaveChanges();
                return RedirectToAction("Detailworklist/" + id_delete_1_worklist);
            }
            return RedirectToAction("Detailworklist/" + id_delete_1_worklist);

        }
    }
}