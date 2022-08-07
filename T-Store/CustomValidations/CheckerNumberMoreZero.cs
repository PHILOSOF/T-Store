using System.ComponentModel.DataAnnotations;
using T_Store.Infrastructure;

namespace T_Store.CustomValidations
{
    public class CheckerNumberMoreZero : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            
            if (Convert.ToInt32(value) <= 0)
            {
                return new ValidationResult(ApiErrorMessage.NumberLessOrEqualZero);

            }
            return null;
        }
    }
}
