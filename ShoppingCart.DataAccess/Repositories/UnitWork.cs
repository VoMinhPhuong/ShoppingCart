using ShoppingCart.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.DataAccess.Repositories
{
    public class UnitWork : IUnitWork
    {
        private ShoppingDbContext _context;

        public ICategoryRepository Category { get; private set; }

        public IProductRepository Product { get; private set; }

        public IOrderHeaderRepository OrderHeader { get; private set; }

        public IOrderDetailReponsitory OrderDetail { get; private set; }
        public UnitWork(ShoppingDbContext context)
        {
            _context = context;
            Category = new CategoryRepository(context);
            Product = new ProductRepository(context);
            OrderHeader = new OrderHeaderReponsitory(context);
            OrderDetail = new OrderDetailReponsitory(context);
        }

        public void Save()
        {
            _context.SaveChanges(); 
        }
    }
}
