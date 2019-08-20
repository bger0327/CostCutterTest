using System;

namespace ui.Models
{
    public class TrimEquipment
    {
        public string trim_name { get; set; }
        public DateTime start_date { get; set; }
        public DateTime? end_date { get; set; }
        public string wheel_type { get; set; }
        public string infotainment_type { get; set; }
        public string headlight_type { get; set; }
        public string upholstery_type { get; set; }
        public static string GetTableName()
        {
            return "trim_equipment";
        }
    }
}