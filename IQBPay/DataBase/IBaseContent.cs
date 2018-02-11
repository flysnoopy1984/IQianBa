using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IQBPay.DataBase
{
    public interface IBaseContent
    {
        T Update<T>(T entity) where T : class;
        T Insert<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        T Find<T>(params object[] keyValues) where T : class;
        List<T> FindAll<T>(Expression<Func<T, bool>> conditions = null) where T : class;
       
    }
}
