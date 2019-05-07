namespace GradeBook.Api.Controllers
{
    using GradeBook.Application.Dto.Input;
    using GradeBook.Application.Service.Interfaces;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;

    [Produces("application/json")]
    [Route("api/students")]
   
    public class StudentController : Controller
    {

        private readonly IStudentApplicationService _studentApplicationService;

        public StudentController(IStudentApplicationService studentApplicationService)
        {
            _studentApplicationService = studentApplicationService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Administrator")]
        [ProducesResponseType(typeof(Application.Dto.Output.PaginationDto), 200)]
        public IActionResult GetAll(int page = 1, int pageSize = 10, string sortBy = "id", string sortDirection = "asc")
        {
            return Ok(_studentApplicationService.GetAll(page, pageSize, sortBy, sortDirection));
        }

        [HttpGet("all")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdministratorTeacher")]
        [ProducesResponseType(typeof(List<Application.Dto.Output.StudentDto>), 200)]
        public IActionResult GetAll()
        {
            return Ok(_studentApplicationService.GetAll());
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Administrator")]
        [ProducesResponseType(typeof(Application.Dto.Output.StudentDto), 200)]
        public IActionResult Get(int id)
        {
            return Ok(_studentApplicationService.Get(id));
        }


        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Administrator")]
        public IActionResult Post([FromBody]StudentDto student)
        {
            _studentApplicationService.Add(student);
            return Ok("Student was added sucessfully");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Administrator")]
        public IActionResult Put(int id, [FromBody]StudentDto student)
        {
            _studentApplicationService.Update(id, student);
            return Ok("Student was updated sucessfully");
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Administrator")]
        public IActionResult Delete(int id)
        {
            _studentApplicationService.Delete(id);
            return Ok("Student was deleted sucessfully");
        }
    }
}
