using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToDoList.Controllers.DTO;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class LoginController : Controller
    {
        private TodoListDBEntities _db = new TodoListDBEntities();
        // GET: Home
        public ActionResult Index()
        {
            if(Session["FullName"] != null)
            {
                RedirectToRoute("employee");
            }
            
            return View();

        }

/*        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string login_username, string login_password)
        {
            if (ModelState.IsValid)
            {



                var data = _db.Employees.Where(s => s.Email.Equals(login_username) && s.Password.Equals(login_password)).ToList();
                if (data.Count() > 0)
                {
                    Session.Clear();
                    //add session
                    Session["FullName"] = data.FirstOrDefault().FullName;
                    Session["Email"] = data.FirstOrDefault().Email;
                    Session["IdUser"] = data.FirstOrDefault().Id;
                    Session["IdRole"] = data.FirstOrDefault().IdRole;
                    return RedirectToAction("index","employee");
                }
                else
                {
                    Session["loginFail"] = "Ten dang nhap hoac mat khau sai";
                    return RedirectToAction("Index");
                }
            }
            //Console.WriteLine(login_username + login_password);
            return RedirectToAction("Index");
        }*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(EmployeeDTO login)
        {
            if (ModelState.IsValid)
            {



                var data = _db.Employees.Where(s => s.Email.Equals(login.Email) && s.Password.Equals(login.Password)).FirstOrDefault();
                if (data != null)
                {
                    Session.Clear();
                    //add session
                    Session["FullName"] = data.FullName;
                    Session["Email"] = data.Email;
                    Session["IdUser"] = data.Id;
                    Session["IdRole"] = data.IdRole;
                    return RedirectToAction("index", "employee");
                }
                else
                {
                    login.LoginErrorMessage = "may nhap sai email hoac mat khau roi";
                    return View("Index",login);
                }
            }
            //Console.WriteLine(login_username + login_password);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(EmployeeDTO signup)
        {
            if (ModelState.IsValid)
            {



                var data = _db.Employees.Where(s => s.Email.Equals(signup.Email) ).ToList();
                if (data.Count() < 1)
                {
                    Session.Clear();
                    //add session
                    Employee employee = new Employee() { Email = signup.Email, Password = signup.Password, FullName = signup.FullName,IdRole=2,DateCreated=DateTime.Today };
                    _db.Employees.Add(employee);
                    _db.SaveChanges();
                    Session["signupMess"] = "Dang ky thanh cong";

                    return RedirectToAction("index");
                }
                else
                {
                    signup.RegisterErrorMessage = "Email bi trung";
                    Session["signupMess"] = "Email bi trung";
                    return RedirectToAction("Index");
                }
            }
            //Console.WriteLine(login_username + login_password);
            return RedirectToAction("Index");
        }


        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Index");
        }
        public int deleteLoginFail()
        {
            Session.Clear();
            return 1;
        }

    }
}