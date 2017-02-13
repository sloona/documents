using Helpers;
using Models;
using PagedList;
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
            int pageSize = 5;
            int pageNumber = 1;
            return View("Index", documents.ToPagedList(pageNumber, pageSize));
        }
        //[Authorize]
        public ActionResult Index(string sortColumn, int? page)
        {
            {
                ViewBag.Login = "Ваш логин: " + HttpContext.User.Identity.Name;
                if (!string.IsNullOrEmpty(sortColumn))
                {
                    ViewBag.sortColumn = sortColumn;
                }
            }
            IEnumerable<Document> documents;

            switch (sortColumn)
            {
                case "Author":
                    documents = repository.GetAll().OrderBy(p => p.Author.LastName);
                    break;

                case "CreationDate":
                    documents = repository.GetAll().OrderBy(p => p.CreationDate);
                    break;

                case "Id":
                    documents = repository.GetAll().OrderBy(p => p.Id);
                    break;

                case "Title":
                    documents = repository.GetAll().OrderBy(p => p.Title);
                    break;

                default:
                    documents = repository.GetAll().OrderBy(p => p.CreationDate);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(documents.ToPagedList(pageNumber, pageSize));

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

            document.Title = model.Name;

            document.CreationDate = DateTime.Now;


            using (var session = NHibernateHelper.OpenSession())
            {
                document.Author = session.QueryOver<User>()
                    .And(u => u.Login == HttpContext.User.Identity.Name)
                    .List<User>()
                    .FirstOrDefault();
            }

            repository.Update(document);

            if (model.Attachment != null)
            {
                if (model.Attachment.ContentLength > 0)
                {
                    string fileExtension = System.IO.Path.GetExtension(model.Attachment.FileName);
                    document.FileName = $"{document.Id}{fileExtension}";
                    model.Attachment.SaveAs(Server.MapPath("~/Files/" + document.FileName));
                }
            }

            repository.Update(document);

            ViewBag.Success = $"Документ был успешно сохранен";
            return View("Create", model);
        }

    }
}