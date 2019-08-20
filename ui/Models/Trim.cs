using System.Collections.Generic;

namespace ui.Models
{
    public class Trim
    {
        public string trim_name { get; set; }
        public decimal additional_cost { get; set; }
        public static string GetTableName()
        {
            return "trims";
        }

        public IEnumerable<TrimEquipment> TrimEquipment { get; set; }
    }
}