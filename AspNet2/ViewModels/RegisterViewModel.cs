using System.ComponentModel.DataAnnotations;

namespace AspNet2.ViewModels
{
    public class RegisterViewModel : LoginViewModel
    {
        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
