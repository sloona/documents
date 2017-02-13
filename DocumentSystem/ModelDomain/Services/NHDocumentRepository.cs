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
                var criteria = session.CreateCriteria(typeof(Document), "d").CreateCriteria("Author", "a");
                if (searchModel.Id.HasValue)
                {
                    criteria.Add(Restrictions.Eq("d.Id", searchModel.Id));
                }
                if (!string.IsNullOrEmpty(searchModel.Title))
                {
                    criteria.Add(Restrictions.InsensitiveLike("d.Title", searchModel.Title, MatchMode.Anywhere));
                }
                if (!string.IsNullOrEmpty(searchModel.Author))
                {
                    criteria.Add(Restrictions.Or(
                         Restrictions.InsensitiveLike("a.FirstName", searchModel.Author, MatchMode.Anywhere),
                         Restrictions.InsensitiveLike("a.LastName", searchModel.Author, MatchMode.Anywhere)
                        ));
                }



                documents = criteria.List<Document>().ToList();
            }

            return documents;
        }
    }
}