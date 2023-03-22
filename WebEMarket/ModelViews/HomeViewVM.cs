using System.Collections.Generic;
using WebEMarket.Models;

namespace WebEMarket.ModelViews
{
    public class HomeViewVM
    {
        public List<TinTuc> TinTucs { get; set; }
        public List<ProductHomeVM> Products { get; set; }

        public QuangCao quangcao { get; set; }
    }
}
