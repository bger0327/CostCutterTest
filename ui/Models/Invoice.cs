using System;

namespace ui.Models
{
    public class Invoice
    {
        public int invoice_number { get; set; }
        public int order_number { get; set; }
        public decimal invoice_value { get; set; }
        public DateTime? settlement_date { get; set; }
        public static string GetTableName()
        {
            return "invoices";
        }

        public Order Order { get; set; }

    }
}