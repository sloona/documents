using Helpers;
using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class DocumentController : Controller
    {
        private IDocumentRepository repository { get; set; }

        public DocumentController()
        {
            repository = new NHDocumentRepository();
        }

        public ActionResult Index(string sortColumn)
        {
            IEnumerable<Document> documents;
            if (sortColumn == "Author")
            {
                documents = repository.GetAll().OrderBy(p => p.Author.LastName);
            }
            else
            {
                documents = repository.GetAll().OrderBy(p => p.GetType().GetProperty(sortColumn ?? "CreationDate").GetValue(p, null));
            }
            
            //documents = repository.GetAll().OrderBy(d => d.Author);
            return View(documents);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult SaveDocument(DocumentModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }

            var document = repository.Create();

            document.Title = model.Name;

            document.CreationDate = DateTime.Today;

            using (var session = NHibernateHelper.OpenSession())
            {
                document.Author = session.Get<User>(2);
            }
            repository.Update(document);

            ViewData.Model = $"Документ был успешно сохранен";
            return View("Success");
        }

    }
}