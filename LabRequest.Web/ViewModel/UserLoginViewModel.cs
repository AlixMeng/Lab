using System.ComponentModel.DataAnnotations;

namespace LabRequest.Web.ViewModel
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "نام کاربری را وارد کنید")]
        [DataType(DataType.Text)]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "رمز کاربری را وارد کنید")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز کاربری")]
        public string Password { get; set; }


        [Required(ErrorMessage="نام شرکت مربوطه را وارد کنید")]
        [Display(Name="شرکت محل کار")]
        public string Corporation { get; set; }
    }
}