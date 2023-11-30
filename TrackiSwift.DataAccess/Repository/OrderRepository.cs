using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackiSwift.Data;
using TrackiSwift.DataAccess.Repository.IRepository;
using TrackiSwift.Models;

namespace TrackiSwift.DataAccess.Repository
{
    public  class OrderRepository: Repository<Order>, IOrderRepository
    {
        private ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }



        public void Update(Order order)
        {
            _context.Orders.Update(order);
        }
    }
}
