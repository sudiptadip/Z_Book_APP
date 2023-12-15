using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.Model
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> ShoppingCartLIst { get; set; }
        public OrderHeader OrderHeader { get; set; }
        public double OrderTotal { get; set; }

    }
}
