using AutoMapper;
using Claraform.API.Helpers;
using Claraform.API.Models;
using Claraform.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Claraform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorCollectionsController : ControllerBase
    {
        private readonly IClaraformRepository _ClaraformRepository;
        private readonly IMapper _mapper;

        public AuthorCollectionsController(IClaraformRepository ClaraformRepository, IMapper mapper)
        {
            _ClaraformRepository = ClaraformRepository;
            _mapper = mapper;
        }

        [HttpGet("({ids})", Name = "GetAuthorCollection")]
        public IActionResult GetAuthorCollection([FromRoute][ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }
            var authorsFromRepo = _ClaraformRepository.GetAuthors(ids);
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
                _ClaraformRepository.AddAuthor(author);
            }
            _ClaraformRepository.Save();
            var authorsToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authorsToAdd);
            var ids = string.Join(",", authorsToReturn.Select(a => a.Id));
            return CreatedAtRoute("GetAuthorCollection", new { ids }, authorsToReturn);
        }
    }
}
