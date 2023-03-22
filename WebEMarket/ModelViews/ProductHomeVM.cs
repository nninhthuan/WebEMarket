using System.Collections.Generic;
using WebEMarket.Models;

namespace WebEMarket.ModelViews
{
    public class ProductHomeVM
    {
        public Category category { get; set; }
        public List<Product> lsProducts { get; set; }    
    }
}
