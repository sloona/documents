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
                documents = session.CreateCriteria(typeof(Document))
                    .Add(Expression.Or(
                        Expression.Eq("Id", searchModel.Id),
                        Expression.Eq("Id")
                        ))

                    .Add(Expression.Or(
                        Expression.Like("Title", $"%{searchModel.Title}%"),
                        Expression.IsNotEmpty("Title")
                        ))
                    .List<Document>().ToList();
                // .Add(Expression.Eq("Author", searchModel.Author))



//                So, u can do like this,

//session = sessionFactory.getCurrentSession();
//                Criteria crit = session.createCriteria(PersonEntity.class).add(
//                                Restrictions.eq("FirstName", person.getFirstName())).add(
//                                Restrictions.eq("email", person.getUser().getEmail()));
//if(person.getLastName()!=null){
//crit.add(Restrictions.eq("LastName", person.getLastName()));
//}
//    person=(PersonVO)crit.list();

 


                //documents = criteria.List<Document>().ToList();
                //if (searchModel.Id.HasValue)
                //    documents = documents.Where(d => d.Id == searchModel.Id).ToList(); 
                //if (!String.IsNullOrEmpty(searchModel.Title))
                //    documents = documents.Where(d => d.Title.ToLower().Contains(searchModel.Title.ToLower())).ToList();
                //if(!String.IsNullOrEmpty(searchModel.Author))
                //    documents = documents.Where(d => d.Author.FullName.ToLower().Contains(searchModel.Author.ToLower())).ToList();
            }
            return documents;
        }
    }
}