using ShoppingCart.DataAccess.Data;
using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.DataAccess.Repositories
{
    public class OrderDetailReponsitory : Repository<OrderDetail>, IOrderDetailReponsitory
    {
        private ShoppingDbContext _context;

        public OrderDetailReponsitory(ShoppingDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(OrderDetail orderDetail) 
        {
            _context.OrderDetails.Update(orderDetail);
        }
    }
}
