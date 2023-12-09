using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackiSwift.Data;
using TrackiSwift.DataAccess.Repository.IRepository;

namespace TrackiSwift.DataAccess.Repository
{
    public class UnitOfWork: IUnitOfWork
    {
        private ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Orders = new OrderRepository(_context);

        }
        public IOrderRepository Orders { get;private set; }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

