using Employee_Addition_Form_FrontEnd.Models;

namespace Employee_Addition_Form_FrontEnd.ViewModels
{
    public class successResponseViewModel<T>
    {
        public string Message { get; set; }
        public T Data { get; set; } 
    }
}
