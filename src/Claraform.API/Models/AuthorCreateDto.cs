using System;
using System.Collections.Generic;

namespace Claraform.API.Models
{
    public class AuthorCreateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string MainCategory { get; set; }
        public ICollection<CourseCreateDto> Courses { get; set; } = new List<CourseCreateDto>();
    }
}
