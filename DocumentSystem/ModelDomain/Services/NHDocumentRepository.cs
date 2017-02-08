using System.Collections.Generic;
using System.Linq;
using Models;
using Helpers;
using NHibernate.Criterion;
using System;

namespace Services
{
    public class NHDocumentRepository : IDocumentRepository
    {
        public IEnumerable<Document> GetAll()
        {
            var documents = new List<Document>();
            using (var session = NHibernateHelper.OpenSession())
            {
                var criteria = session.CreateCriteria(typeof(Document));
                //criteria.Add(Restrictions.Ge("Id",3));
                documents = criteria.List<Document>().ToList();
            }
            return documents;
        }

        public Document Create()
        {
            return new Document() { Id = 0 };
        }

        public void Update(Document document)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.SaveOrUpdate(document);

                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }

                    transaction.Commit();
                }
            }
        }

        public IEnumerable<Document> Search(DocumentSearchModel searchModel)
        {

            var documents = new List<Document>();
            using (var session = NHibernateHelper.OpenSession())
            {
                var criteria = session.CreateCriteria(typeof(Document)).CreateCriteria("Author", "a");
                //var criteria = session.CreateCriteria(Document);
                if (searchModel.Id.HasValue)
                {
                    criteria.Add(Expression.Eq("Id", searchModel.Id));
                }
                if (!string.IsNullOrEmpty(searchModel.Title))
                {
                    criteria.Add(Expression.InsensitiveLike("Title", $"%{searchModel.Title}%"));
                }
                if (!string.IsNullOrEmpty(searchModel.Author))
                {
                    criteria.Add(Expression.Or(
                        Expression.InsensitiveLike("a.FirstName", $"%{searchModel.Author}%"),
                        Expression.InsensitiveLike("a.LastName", $"%{searchModel.Author}%")
                        ));
                }



                documents = criteria.List<Document>().ToList();
            }

            return documents;
        }
    }
}