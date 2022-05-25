using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalanShoppingAccessLayer.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string Name { get; set; }
        public string DateCrea { get; set; }
        public double Total { get; set; }
        public int NumberOfProducts { get; set; }
        public List<ProductOrder> ProductOrders { get; set; } = new List<ProductOrder>();
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
