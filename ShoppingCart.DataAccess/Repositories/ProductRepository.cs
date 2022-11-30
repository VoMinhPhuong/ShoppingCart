using Microsoft.EntityFrameworkCore;
using ShoppingCart.DataAccess.Data;
using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.DataAccess.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ShoppingDbContext _context;
        public ProductRepository(ShoppingDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Product product)
        {
            var productDB = _context.Products.FirstOrDefault(x => x.Id == product.Id);
            if(productDB != null)
            {
                productDB.Name = product.Name;
                productDB.Price = product.Price;
                productDB.Description = product.Description;
                if(product.ImageUrl != null)
                {
                    productDB.ImageUrl = product.ImageUrl;
                }
            }
        }
    }
}
