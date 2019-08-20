namespace ui.Models
{
    public class ModelsEnginesIntersect
    {
        public string model_name { get; set; }
        public string engine_designation { get; set; }
        public decimal additional_cost { get; set; }
        public static string GetTableName()
        {
            return "models_engines_intersect";
        }

        public Model Model { get; set; }
        public Engine Engine { get; set; }
    }
}