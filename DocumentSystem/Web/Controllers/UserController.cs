using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Services;

namespace Web.Controllers
{
    public class UserController : Controller
    {
        private IUserRepository repository { get; set; }

        public UserController()
        {
            repository = new NHUserRepository();
        }
        
        public ActionResult Index()
        {
            var users = repository.GetAll();
            return View(users);
        }
    }
}