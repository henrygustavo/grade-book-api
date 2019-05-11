namespace GradeBook.Api.Controllers
{
    using GradeBook.Application.Dto.Input;
    using GradeBook.Application.Service.Interfaces;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    [Produces("application/json")]
    [Route("api/gradebooks")]
    public class GradeBookController: Controller
    {
     
        private readonly IGradeBookApplicationService _gradeBookApplicationService;

        public GradeBookController(IGradeBookApplicationService gradeBookApplicationService)
        {
            _gradeBookApplicationService = gradeBookApplicationService;
        }


        [HttpGet]
        [ProducesResponseType(typeof(Application.Dto.Output.PaginationDto), 200)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetAll(int page = 1, int pageSize = 10, string sortBy = "id", string sortDirection = "asc")
        {
            int userId =int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return Ok(_gradeBookApplicationService.GetAllByRole(page, pageSize, sortBy, sortDirection, userId));
        }


        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdministratorTeacher")]
        [ProducesResponseType(typeof(Application.Dto.Output.GradeBookDto), 200)]
        public IActionResult Get(int id)
        {
            return Ok(_gradeBookApplicationService.Get(id));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Teacher")]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult Post([FromBody]GradeBookDto gradeBook)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            gradeBook.TeacherUserId = userId;
            _gradeBookApplicationService.Add(gradeBook);
            return Ok("GradeBook was added sucessfully");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Teacher")]
        public IActionResult Put(int id, [FromBody]GradeBookDto gradeBook)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            gradeBook.TeacherUserId = userId;

            _gradeBookApplicationService.Update(id, gradeBook);
            return Ok("GradeBook was updated sucessfully");
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Teacher")]
        public IActionResult Delete(int id)
        {
            _gradeBookApplicationService.Delete(id);
            return Ok("GradeBook was deleted sucessfully");
        }
    }
}
