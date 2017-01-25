using System.Collections.Generic;
using System.Linq;
using Models;
using DomainModel.Helpers;
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
    }
}