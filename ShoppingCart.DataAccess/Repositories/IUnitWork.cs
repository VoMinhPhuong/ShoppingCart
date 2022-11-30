using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.DataAccess.Repositories
{
    public interface IUnitWork
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }

        // Thiếu user và cart
        IOrderHeaderRepository OrderHeader { get; }
        IOrderDetailReponsitory OrderDetail { get; }

        void Save();
    }
}
