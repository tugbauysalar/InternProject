using InternProject.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternProject.Persistence
{
    public class UnitofWork : IUnitofWork
    {
        private readonly AppDbContext _appDbContext;

        public UnitofWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public void Commit()
        {
            _appDbContext.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}
