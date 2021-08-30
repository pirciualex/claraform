using Claraform.API.Models;
using System.ComponentModel.DataAnnotations;

namespace Claraform.API.ValidationAttributes
{
    public class CourseTitleMustBeDifferentFromDescriptionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var course = (CourseCreateDto)validationContext.ObjectInstance;
            if (course.Title == course.Description)
            {
                return new ValidationResult(ErrorMessage, new[] { "CourseCreateDto" });
            }
            return ValidationResult.Success;
        }
    }
}
