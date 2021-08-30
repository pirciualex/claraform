using AutoMapper;
using Claraform.API.Models;
using Claraform.API.ResourceParameters;
using Claraform.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Claraform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly IClaraformRepository _ClaraformRepository;
        private readonly IMapper _mapper;

        public AuthorsController(IClaraformRepository ClaraformRepository, IMapper mapper)
        {
            _ClaraformRepository = ClaraformRepository;
            _mapper = mapper;
        }

        [HttpGet()]
        [HttpHead]
        public ActionResult<IEnumerable<AuthorDto>> GetAuthors([FromQuery] AuthorsResourceParameters authorsResourceParameters)
        {
            var authorsFromRepo = _ClaraformRepository.GetAuthors(authorsResourceParameters);
            return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo));
        }

        [HttpGet("{authorId}", Name = "GetAuthor")]
        public ActionResult<AuthorDto> GetAuthor(Guid authorId)
        {
            var authorFromRepo = _ClaraformRepository.GetAuthor(authorId);
            if (authorFromRepo == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<AuthorDto>(authorFromRepo));
        }

        [HttpPost]
        public ActionResult<AuthorDto> CreateAuthor(AuthorCreateDto author)
        {
            var authorToAdd = _mapper.Map<Entitites.Author>(author);
            _ClaraformRepository.AddAuthor(authorToAdd);
            _ClaraformRepository.Save();
            var authorToReturn = _mapper.Map<AuthorDto>(authorToAdd);
            return CreatedAtRoute("GetAuthor", new { authorId = authorToReturn.Id }, authorToReturn);
        }

        [HttpOptions]
        public IActionResult GetAuthorOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }

        [HttpDelete("{authorId}")]
        public IActionResult DeleteAuthor(Guid authorId)
        {
            var authorToDelete = _ClaraformRepository.GetAuthor(authorId);
            if (authorToDelete == null)
            {
                return NotFound();
            }
            _ClaraformRepository.DeleteAuthor(authorToDelete);
            _ClaraformRepository.Save();
            return NoContent();
        }
    }
}
