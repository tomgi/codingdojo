using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IHS.ZlotePrzeboje.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        void Add(T item);
        void Delete(T item);
    }
}
