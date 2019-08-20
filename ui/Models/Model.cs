namespace ui.Models
{
    public class Model
    {
        public string model_name { get; set; }
        public decimal base_cost { get; set; }
        public int average_lead_time { get; set; }
        public static string GetTableName()
        {
            return "models";
        }

    }
}