namespace ui.Models
{
    public class Engine
    {
        public string engine_designation { get; set; }
        public int capacity { get; set; }
        public string fuel_type { get; set; }

        public static string GetTableName()
        {
            return "engines";
        }

    }
}