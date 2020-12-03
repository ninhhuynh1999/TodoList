using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToDoList.Controllers.Mapping;
using ToDoList.Models;
using ToDoList.Controllers.DTO;

namespace ToDoList.Controllers
{
    public class EmployeeController : Controller
    {
        private TodoListDBEntities db = new TodoListDBEntities();
        private IMapper mapper = new MappingConfig().config();
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
                if (int.Parse(Session["IdRole"].ToString()) == 1)
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
        public ActionResult Work(int? id)
        {
            if (Session["IdUser"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (id == null)
            {
                return RedirectToAction("Detailworklist/"+Session["WorkListId"]);
            }
            var work = (from c in db.Works where c.Id == id select c).FirstOrDefault();
            if(work != null)
            {
                var employees = db.Employees.ToList();
                var dsTrangThai = db.WorkStatus.ToList();
                var dsBinhluan = from c in db.Comments join e in db.Employees on c.IdEmployee equals e.Id where c.IdWork == id select new CommentViewDTO() { Id=c.Id,IdEmployee=c.IdEmployee,IdWork=c.IdWork,CommentContent=c.CommentContent,DateCreated=c.DateCreated,NameEmployee=e.FullName};
                var dsNguoiLamChung = from c in db.Work_Employee where c.IdWork == id select c.Employee; 
                var dsNguoiKhongLamChung = from c in db.Employees
                                           where !(from o in db.Work_Employee where o.IdWork == id select o.IdEmployee).Contains(c.Id)
                                           select c;
                ViewData["dsEmployee"] = mapper.Map<List<Employee>,List<EmployeeDTO>>(employees);
                ViewData["dsTrangThai"] = mapper.Map<List<WorkStatu>, List<WorkStatusDTO>>(dsTrangThai);
                ViewData["dsBinhLuan"] = dsBinhluan.ToList();
                ViewData["dsNguoiLamChung"] = mapper.Map<List<Employee>, List<EmployeeDTO>>(dsNguoiLamChung.ToList());
                ViewData["dsNguoiKhongLamChung"] = mapper.Map<List<Employee>, List<EmployeeDTO>>(dsNguoiKhongLamChung.ToList());
                Session["IdWork"] = id;
                return View(mapper.Map<Work, WorkDTO>(work));
            }
            else
            {
                return RedirectToAction("Detailworklist/" + Session["WorkListId"]);
            }

        }
        [HttpPost]
        public ActionResult ThemComment(string id_cmt_employee,string add_cmt_idwork,string add_cmt_idcmt, string Add_Comment)
        { 
            if(Session["IdUser"] == null || Session["IdUser"].ToString() != id_cmt_employee)
            {
                return RedirectToAction("Index", "Login");
            }
            if(int.Parse(add_cmt_idcmt) == 0)
            {
                Comment comment = new Comment { IdEmployee = int.Parse(id_cmt_employee), IdWork = int.Parse(add_cmt_idwork), DateCreated = DateTime.Today, CommentContent = Add_Comment };
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Work/" + Session["IdWork"].ToString());
            }
            else
            {
                int idcomment = int.Parse(add_cmt_idcmt);
                var idCmt = db.Comments.Where(c => c.Id == idcomment).FirstOrDefault();

                if (idCmt == null)
                {
                    return RedirectToAction("Work/" + Session["IdWork"].ToString());
                }
                else
                {
                    idCmt.CommentContent = Add_Comment;
                    db.Entry(idCmt).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Work/" + Session["IdWork"].ToString());
                }
            }
            
        }
        [HttpPost]
        public ActionResult XoaWork(string id_delete_work)
        {
            
            if (Session["IdUser"] == null )
            {
                return RedirectToAction("Index", "Login");
            }
            int idDelete = int.Parse(id_delete_work);
            var work = db.Works.Where(w => w.Id == idDelete).FirstOrDefault();
            if(work != null ) { db.Works.Remove(work); db.SaveChanges(); }
            return RedirectToAction("Detailworklist/" + Session["WorkListId"]);


        }
        public ActionResult AddWork(int? id)
        {
            if (Session["IdUser"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (id == null)
            {
                return RedirectToAction("Detailworklist/" + Session["WorkListId"]);
            }
            var employees = db.Employees.ToList();
            ViewData["dsEmployee"] = mapper.Map<List<Employee>, List<EmployeeDTO>>(employees);

            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddWork(WorkDTO workDTO)
        {
            if (Session["IdUser"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (ModelState.IsValid)
            {
                Work work = mapper.Map<WorkDTO, Work>(workDTO);
                work.DateCreated = DateTime.Today;
                work.IdWorkStatus = 1;
                db.Works.Add(work);
                Work_Employee work_Employee = new Work_Employee { IdWork = work.Id, IdEmployee = workDTO.IdEmployee };
                db.Work_Employee.Add(work_Employee);

                db.SaveChanges();
                return RedirectToAction("Detailworklist/" + Session["WorkListId"]);
            }
            return RedirectToAction("Detailworklist/" + Session["WorkListId"]);


        }
        [HttpPost]
        public ActionResult EditStatusWork(string editstatus_idwork, string editstatus_id)
        {
            if (Session["IdUser"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            int idwork = int.Parse(editstatus_idwork);
            var work = db.Works.Where(w => w.Id == idwork).FirstOrDefault();
            if (work != null)
            {
                int statusID = int.Parse(editstatus_id);
                work.IdWorkStatus = statusID;
                db.Entry(work).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Work/" + Session["IdWork"]);

            }
                return RedirectToAction("Work/" + Session["IdWork"]);


            
        }
        [HttpPost]
        public ActionResult ThemNguoiLamChung_Work(string id_add_work_employee, string id_add_work)
        {
            if (Session["IdUser"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            int idWork = int.Parse(id_add_work);
            int idEmp = int.Parse(id_add_work_employee);
            var nguoilamchung = db.Work_Employee.Where(w => w.IdEmployee == idEmp && w.IdWork == idWork).FirstOrDefault();
            if(nguoilamchung != null)
            {
                return RedirectToAction("Work/" + Session["IdWork"]);
            }
            else
            {
                Work_Employee work_Employee = new Work_Employee { IdWork = idWork, IdEmployee = idEmp, DateCreated = DateTime.Today };
                db.Work_Employee.Add(work_Employee);
                db.SaveChanges();
                return RedirectToAction("Work/" + Session["IdWork"]);
            }


            
        }
        [HttpPost]
        public ActionResult XoaNguoiLamChung_Work(string id_delete_work_employee, string id_delete_work)
        {
            if (Session["IdUser"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            int idWork = int.Parse(id_delete_work);
            int idEmp = int.Parse(id_delete_work_employee);
            var nguoilamchung = db.Work_Employee.Where(w => w.IdEmployee == idEmp && w.IdWork == idWork).FirstOrDefault();
            if (nguoilamchung != null)
            {
               
                db.Work_Employee.Remove(nguoilamchung);
                db.SaveChanges();
                return RedirectToAction("Work/" + Session["IdWork"]);
            }
            else
            {
                return RedirectToAction("Work/" + Session["IdWork"]);
            }



        }
        public ActionResult EditWork(int? id)
        {
            if (Session["IdUser"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if(id == null)
            {
                return RedirectToAction("Work/" + Session["IdWork"]);
            }
            var work = db.Works.Where(w => w.Id == id).FirstOrDefault();
            if(work != null)
            {
                var employees = db.Employees.ToList();
                ViewData["dsEmployee"] = mapper.Map<List<Employee>, List<EmployeeDTO>>(employees);

                return View(mapper.Map<Work,WorkDTO>(work));
            }
            else
            {
                return RedirectToAction("Work/" + Session["IdWork"]);

            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditWork(WorkDTO workDTO)
        {
            if (Session["IdUser"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (ModelState.IsValid)
            {
                var work = db.Works.Where(w => w.Id == workDTO.Id).FirstOrDefault();
                if(work!= null)
                {
                    work.WorkContent = workDTO.WorkContent;
                    work.WorkName = workDTO.WorkName;
                    work.StartDate = workDTO.StartDate;
                    work.EndDate = workDTO.EndDate;
                    db.Entry(work).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Work/" + Session["IdWork"]);

                }
                else
                {
                    return RedirectToAction("Work/" + Session["IdWork"]);

                }
            }
            else
            {
                return RedirectToAction("Work/" + Session["IdWork"]);

            }


        }
    }
}