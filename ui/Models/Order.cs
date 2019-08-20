using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ui.Models
{
    public class Order
    {
        public int order_number { get; set; }
        public int customer_number { get; set; }
        public int employee_number { get; set; }
        [Range(0, Double.MaxValue)]
        public decimal sale_price { get; set; }
        [Range(0,Double.MaxValue)]
        public decimal deposit { get; set; }

        public DateTime order_date { get; set; }
        public static string GetTableName()
        {
            return "orders";
        }

        public Employee Employee { get; set; }
        public Customer Customer { get; set; }
        public IEnumerable<OrderedVehicles> OrderedVehicles { get; set; }
    }
}