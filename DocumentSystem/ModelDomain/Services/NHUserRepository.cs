using System.Collections.Generic;
using System.Linq;
using Models;
using DomainModel.Helpers;
using NHibernate.Criterion;

namespace Services
{
    public class NHUserRepository : IUserRepository
    {
        public IEnumerable<User> GetAll()
        {
            var users = new List<User>();
            using (var session = NHibernateHelper.OpenSession())
            {
                var criteria = session.CreateCriteria(typeof(User));
                //criteria.Add(Restrictions.Ge("Id",3));
                users = criteria.List<User>().ToList();
            }
            return users;
        }
    }
}