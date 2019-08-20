using System;

namespace ui.Models
{
    public class OrderedVehicles
    {
        public int vehicle_number { get; set; }
        public int order_number { get; set; }
        public string model_name { get; set; }
        public string trim_name { get; set; }
        public string engine_designation { get; set; }
        public string colour { get; set; }
        public DateTime expected_delivery_date { get; set; }
        public DateTime actual_delivery_date { get; set; }
        public static string GetTableName()
        {
            return "ordered_vehicles";
        }

        public Model Model { get; set; }
        public Trim Trim { get; set; }
        public Engine Engine { get; set; }

    }
}