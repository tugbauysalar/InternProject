using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InternProject.Application.Services
{
    public interface IService<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression);
        Task AddAsync(TEntity entity);
    }
}
