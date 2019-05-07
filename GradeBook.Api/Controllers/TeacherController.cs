namespace GradeBook.Api.Controllers
{
    using GradeBook.Application.Dto.Input;
    using GradeBook.Application.Service.Interfaces;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Produces("application/json")]
    [Route("api/teachers")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Administrator")]
    public class TeacherController : Controller
    {

        private readonly ITeacherApplicationService _teacherApplicationService;

        public TeacherController(ITeacherApplicationService teacherApplicationService)
        {
            _teacherApplicationService = teacherApplicationService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Application.Dto.Output.PaginationDto), 200)]
        public IActionResult GetAll(int page = 1, int pageSize = 10, string sortBy = "id", string sortDirection = "asc")
        {
            return Ok(_teacherApplicationService.GetAll(page, pageSize, sortBy, sortDirection));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Application.Dto.Output.TeacherDto), 200)]
        public IActionResult Get(int id)
        {
            return Ok(_teacherApplicationService.Get(id));
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult Post([FromBody]TeacherDto teacher)
        {
            _teacherApplicationService.Add(teacher);
            return Ok("Teacher was added sucessfully");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult Put(int id, [FromBody]TeacherDto teacher)
        {
            _teacherApplicationService.Update(id, teacher);
            return Ok("Teacher was updated sucessfully");
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult Delete(int id)
        {
            _teacherApplicationService.Delete(id);
            return Ok("Teacher was deleted sucessfully");
        }
    }
}
