using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToDoList.Models;
namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        private TodoListDBEntities _db = new TodoListDBEntities();

        public ActionResult Index()
        {
            var data = (from s in _db.Employees select s).ToList();
            ViewBag.users = data;
            ViewBag.title = "MVC5 - Hello World";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
    
            return View();

        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}