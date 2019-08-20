namespace ui.Models
{
    public class Branch
    {
        public string branch_name { get; set; }
        public string postcode { get; set; }

        public static string GetTableName()
        {
            return "branches";
        }

    }
}