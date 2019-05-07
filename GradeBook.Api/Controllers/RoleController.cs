﻿namespace GradeBook.Api.Controllers
{
    using GradeBook.Application.Dto.Input;
    using GradeBook.Application.Service.Interfaces;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;

    [Produces("application/json")]
    [Route("api/roles")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Administrator")]
    public class RoleController : Controller
    {

        private readonly IRoleApplicationService _roleApplicationService;

        public RoleController(IRoleApplicationService roleApplicationService)
        {
            _roleApplicationService = roleApplicationService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Application.Dto.Output.PaginationDto), 200)]
        public IActionResult GetAll(int page = 1, int pageSize = 10, string sortBy = "id", string sortDirection = "asc")
        {
            return Ok(_roleApplicationService.GetAll(page, pageSize, sortBy, sortDirection));
        }

        [HttpGet("all")]
        [ProducesResponseType(typeof(List<Application.Dto.Output.RoleDto>), 200)]
        public IActionResult GetAll()
        {
            return Ok(_roleApplicationService.GetAll());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RoleDto), 200)]
        public IActionResult Get(int id)
        {
            return Ok(_roleApplicationService.Get(id));
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult Post([FromBody]RoleDto role)
        {
            _roleApplicationService.Add(role);
            return Ok("role was added sucessfully");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult Put(int id, [FromBody]RoleDto role)
        {
            _roleApplicationService.Update(id, role);
            return Ok("role was updated sucessfully");
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult Delete(int id)
        {
            _roleApplicationService.Delete(id);
            return Ok("role was deleted sucessfully");
        }
    }
}