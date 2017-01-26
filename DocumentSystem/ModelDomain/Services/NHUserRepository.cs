using System.Collections.Generic;
using System.Linq;
using Models;
using Helpers;
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

        public User Create()
        {
            return new User() { Id = 0 };
        }

        public void Update(User user)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.Save(user);

                    }
                    catch
                    {
                        transaction.Rollback();
                        //return;
                        throw;
                    }

                    transaction.Commit();
                }
            }
        }
    }
}