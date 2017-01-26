using System.Collections.Generic;
using System.Linq;
using Models;
using Helpers;
using NHibernate.Criterion;

namespace Services
{
    public class NHDocumentRepository : IDocumentRepository
    {
        public IEnumerable<Document> GetAll()
        {
            var users = new List<Document>();
            using (var session = NHibernateHelper.OpenSession())
            {
                var criteria = session.CreateCriteria(typeof(Document));
                //criteria.Add(Restrictions.Ge("Id",3));
                users = criteria.List<Document>().ToList();
            }
            return users;
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
    }
}