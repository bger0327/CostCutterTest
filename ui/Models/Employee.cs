namespace ui.Models
{
    public class Employee
    {
        public int employee_number { get; set; }
        public int? line_manager_number { get; set; }
        public string branch_name { get; set; }
        public string forename { get; set; }
        public string surname { get; set; }
        public string FullName => $"{surname} {forename}";

        public static string GetTableName()
        {
            return "employees";
        }

        public Branch Branch { get; set; }

    }
}