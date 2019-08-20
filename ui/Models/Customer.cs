namespace ui.Models
{
    public class Customer
    {
        public int customer_number { get; set; }
        public string forename { get; set; }
        public string surname { get; set; }
        public string telephone_number { get; set; }

        public string FullName => $"{surname} {forename}";

        public static string GetTableName()
        {
            return "customers";
        }

    }
}