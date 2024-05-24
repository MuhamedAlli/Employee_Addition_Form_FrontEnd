using Employee_Addition_Form_FrontEnd.Consts;
using Employee_Addition_Form_FrontEnd.Enums;
using System.ComponentModel.DataAnnotations;

namespace Employee_Addition_Form_FrontEnd.ViewModels
{
    public class EmployeeViewModel
    {
		public int Id { get; set; }

		[Required(ErrorMessage = ErrorMessages.Required)]
		[StringLength(50,MinimumLength =5,ErrorMessage =ErrorMessages.NameLength)]
		[RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = ErrorMessages.NotString)]
		public string Name { get; set; } = null!;

		[Required(ErrorMessage = ErrorMessages.Required)]
		public int JobRole { get; set; }

		[Required(ErrorMessage =ErrorMessages.Required)]
		public int Gender { get; set; }
        public bool IsFirstAppointment { get; set; }

		[Required(ErrorMessage = ErrorMessages.Required)]
		public DateTime StartDate { get; set; } = DateTime.Now;
		public string? Notes { get; set; }
    }
}
