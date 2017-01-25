using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class DocumentController : Controller
    {
        private IDocumentRepository repository { get; set; }

        public DocumentController()
        {
            repository = new NHDocumentRepository();
        }

        public ActionResult Index()
        {
            var documents = repository.GetAll();
            return View(documents);
        }
    }
}