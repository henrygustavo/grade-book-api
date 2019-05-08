namespace GradeBook.Api.Controllers
{
    using GradeBook.Application.Dto.Input;
    using GradeBook.Application.Service.Interfaces;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;

    [Produces("application/json")]
    [Route("api/courses")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdministratorTeacher")]
    public class CourseController : Controller
    {

        private readonly ICourseApplicationService _courseApplicationService;

        public CourseController(ICourseApplicationService courseApplicationService)
        {
            _courseApplicationService = courseApplicationService;
        }

        [HttpGet("all")]
        [ProducesResponseType(typeof(List<Application.Dto.Output.CourseDto>), 200)]
     
        public IActionResult GetAll()
        {
            return Ok(_courseApplicationService.GetAll());
        }

        [HttpGet]
        [ProducesResponseType(typeof(Application.Dto.Output.PaginationDto), 200)]
        public IActionResult GetAll(int page = 1, int pageSize = 10, string sortBy = "id", string sortDirection = "asc")
        {
            return Ok(_courseApplicationService.GetAll(page, pageSize, sortBy, sortDirection));
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Application.Dto.Output.CourseDto), 200)]
        public IActionResult Get(int id)
        {
            return Ok(_courseApplicationService.Get(id));
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult Post([FromBody]CourseDto course)
        {
            _courseApplicationService.Add(course);
            return Ok("Course was added sucessfully");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult Put(int id, [FromBody]CourseDto course)
        {
            _courseApplicationService.Update(id, course);
            return Ok("Course was updated sucessfully");
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult Delete(int id)
        {
            _courseApplicationService.Delete(id);
            return Ok("Course was deleted sucessfully");
        }
    }
}
