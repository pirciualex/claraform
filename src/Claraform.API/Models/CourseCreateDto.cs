using Claraform.API.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Claraform.API.Models
{
    [CourseTitleMustBeDifferentFromDescription(ErrorMessage = "The title and description must be different.")]
    public class CourseCreateDto //: IValidatableObject
    {
        [Required]
        [MaxLength(100, ErrorMessage = "The title shouldn't be longer than 100 characters.")]
        public string Title { get; set; }
        [MaxLength(1500, ErrorMessage = "The description shouldn't be longer than 1500 characters.")]
        public string Description { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (Title == Description)
        //    {
        //        yield return new ValidationResult("The description should be different from the title", new[] { "CourseCreateDto" });
        //    }
        //}
    }
}
