﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CENTROS.SMSNotifications.Service.Models;
using DocumentFormat.OpenXml.Math;
using KONTAKTOR.DA.Models;
using KONTAKTOR.DA.Mongo.Repository;
using KONTAKTOR.DA.Repository;
using KONTAKTOR.Notifications.DA.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using log4net;

namespace netcoreservice.Service.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CompanyController : ControllerBase
    {
        private CompanyRepository _repo;
        private EmployeeRepository _employees;
        private UserInformationRepository _users;

        // private readonly log4net.ILog _logger;
        public CompanyController(CompanyRepository repo, EmployeeRepository employees, UserInformationRepository users)
        {
            _repo = repo;
            _employees = employees;
            _users = users;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Company model)
        {
            var company = await _repo.CreateAsync(model);
            return Ok(company);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Company model)
        {
            var result = await _repo.UpdateAsync(model);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _repo.GetAsync(id);

            return result != null
                ? (IActionResult)Ok(result)
                : NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _repo.GetAllAsync();

            return result != null
                ? (IActionResult)Ok(result)
                : NotFound();
        }

        [HttpGet("employees/{companyId}")]
        public async Task<IActionResult> GetEmployees(string companyId)
        {
            var employs=  (await _employees.GetAllAsync()).Where(c => c.CompanyId == companyId);
            var users = (await _users.GetAllAsync()).Where(u => employs.Select(e => e.UserId).Contains(u.Id));

            return Ok(users);
        }

    }
}
