using AutoMapper;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorCollectionsController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;

        public AuthorCollectionsController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository;
            _mapper = mapper;
        }

        [HttpGet("({ids})", Name = "GetAuthorCollection")]
        public IActionResult GetAuthorCollection([FromRoute][ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }
            var authorsFromRepo = _courseLibraryRepository.GetAuthors(ids);
            if (ids.Count() != authorsFromRepo.Count())
            {
                return NotFound();
            }
            var authorsToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo);
            return Ok(authorsToReturn);
        }

        [HttpPost]
        public ActionResult<IEnumerable<AuthorDto>> CreateAuthorCollection(IEnumerable<AuthorCreateDto> authorCollection)
        {
            var authorsToAdd = _mapper.Map<IEnumerable<Entitites.Author>>(authorCollection);
            foreach (var author in authorsToAdd)
            {
                _courseLibraryRepository.AddAuthor(author);
            }
            _courseLibraryRepository.Save();
            var authorsToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authorsToAdd);
            var ids = string.Join(",", authorsToReturn.Select(a => a.Id));
            return CreatedAtRoute("GetAuthorCollection", new { ids }, authorsToReturn);
        }
    }
}
