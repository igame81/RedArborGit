using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedArbor.Dao.Interface
{
    public interface IGenericDao<T> where T : class
    {
        T FindById(int id);
        T[] FindAll();
        T SaveOrUpdate(T employee);
        bool Delete(int id);
    }
}
