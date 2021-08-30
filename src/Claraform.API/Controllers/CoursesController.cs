using AutoMapper;
using Claraform.API.Models;
using Claraform.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Claraform.API.Controllers
{
    [ApiController]
    [Route("api/authors/{authorId}/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly IClaraformRepository _ClaraformRepository;
        private readonly IMapper _mapper;

        public CoursesController(IClaraformRepository ClaraformRepository, IMapper mapper)
        {
            _ClaraformRepository = ClaraformRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CourseDto>> GetCoursesForAuthor(Guid authorId)
        {
            if (!_ClaraformRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var coursesForAuthorFromRepo = _ClaraformRepository.GetCourses(authorId);
            return Ok(_mapper.Map<IEnumerable<CourseDto>>(coursesForAuthorFromRepo));
        }

        [HttpGet("{courseId}", Name = "GetCourseForAuthor")]
        public ActionResult<CourseDto> GetCourseForAuthor(Guid authorId, Guid courseId)
        {
            if (!_ClaraformRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var courseForAuthorFromRepo = _ClaraformRepository.GetCourse(authorId, courseId);
            if (courseForAuthorFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CourseDto>(courseForAuthorFromRepo));
        }

        [HttpPost()]
        public ActionResult<CourseDto> CreateCourseForAuthor(Guid authorId, CourseCreateDto course)
        {
            if (!_ClaraformRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var courseToAdd = _mapper.Map<Entitites.Course>(course);
            _ClaraformRepository.AddCourse(authorId, courseToAdd);
            _ClaraformRepository.Save();
            var courseToReturn = _mapper.Map<CourseDto>(courseToAdd);
            return CreatedAtRoute("GetCourseForAuthor", new { authorId = authorId, courseId = courseToReturn.Id }, courseToReturn);
        }

        [HttpDelete("{courseId}")]
        public ActionResult DeleteCourseForAuthor(Guid authorId, Guid courseId)
        {
            if (!_ClaraformRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var courseToDelete = _ClaraformRepository.GetCourse(authorId,courseId);
            if (courseToDelete == null)
            {
                return NotFound();
            }
            _ClaraformRepository.DeleteCourse(courseToDelete);
            _ClaraformRepository.Save();
            return NoContent();
        }
    }
}
