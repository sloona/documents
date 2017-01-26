using System.Collections.Generic;

namespace Services
{
    public interface IEntityRepository<T>
        where T : class
    {

        void Update(T operResult);

        T Create();

        IEnumerable<T> GetAll();

        //T Load(int d);

        //bool Delete(int Id);
    }
}
