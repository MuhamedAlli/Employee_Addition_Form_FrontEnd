using Employee_Addition_Form_FrontEnd.Enums;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Employee_Addition_Form_FrontEnd.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string JobRole { get; set; }=null!;
        public string Gender { get; set; }= null!;
        public bool IsFirstAppointment { get; set; }
        public DateTime StartDate { get; set; }
        public string? Notes { get; set; }
    }
}
