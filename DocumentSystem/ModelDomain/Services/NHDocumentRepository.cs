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
                        session.Save(document);

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
                var criteria = session.CreateCriteria(typeof(Document));
                documents = criteria.List<Document>().ToList();
                if (searchModel.Id.HasValue)
                    documents = documents.Where(d => d.Id == searchModel.Id).ToList();
                if (!String.IsNullOrEmpty(searchModel.Title))
                    documents = documents.Where(d => d.Title.ToLower().Contains(searchModel.Title.ToLower())).ToList();
                if(!String.IsNullOrEmpty(searchModel.Author))
                    documents = documents.Where(d => d.Author.FullName.ToLower().Contains(searchModel.Author.ToLower())).ToList();
            }
            return documents;
        }
    }
}