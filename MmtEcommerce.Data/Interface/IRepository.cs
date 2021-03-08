using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MmtEcommerce.Data.Interface
{
    public interface IRepository<T> where T: class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(int id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(int id);

    }
}
