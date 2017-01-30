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

        public ActionResult SearchForm()
        {
            return View("Search");
        }
        public ActionResult Search(DocumentSearchModel searchModel)
        {
            IEnumerable<Document> documents;
            documents = repository.Search(searchModel);
            return View("Index",documents);
        }
        //[Authorize]
        public ActionResult Index(string sortColumn)
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Login = "Ваш логин: " + User.Identity.Name;
            }
            IEnumerable<Document> documents;
            if (sortColumn == "Author")
            {
                documents = repository.GetAll().OrderBy(p => p.Author.LastName);
            }
            else
            {
                documents = repository.GetAll().OrderBy(p => p.GetType().GetProperty(sortColumn ?? "CreationDate").GetValue(p, null));
            }
            
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
                ViewBag.Error = "Неправильно заполнены данные";
                return View("Create", model);
            }

            var document = repository.Create();

            if (model.Attachment != null)
            {
                if (model.Attachment.ContentLength > 0)
                {
                    string fileName = System.IO.Path.GetFileName(model.Attachment.FileName);
                    document.FileName = fileName;
                    model.Attachment.SaveAs(Server.MapPath("~/Files/" + fileName));
                }
            }
                

            document.Title = model.Name;

            document.CreationDate = DateTime.Now;


            using (var session = NHibernateHelper.OpenSession())
            {
                document.Author = session.Get<User>(2);
            }
            repository.Update(document);

            ViewBag.Success = $"Документ был успешно сохранен";
            return View("Create", model);
        }

    }
}