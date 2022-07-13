using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using School.IRepository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace School.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private IConfiguration _config;
        IStudentRepository _studentRepository = null;
        public StudentController(IConfiguration config, IStudentRepository studentRepository)
        {
            _config = config;
            _studentRepository = studentRepository;
        }
        [HttpPost]
        [Route("Registration")]
        public async Task<IActionResult> Registration([FromBody] Student student)
        {
            try
            {
                student = await _studentRepository.Save(student);
                return Ok(student);
            }
            catch(Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,ex.Message);
            }
            
        }
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] Student student)
        {
            try
            {
                student = await _studentRepository.Save(student);
                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }


        [HttpGet]
        [Route("Login/{username}/{password}")]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                Student student = new Student()
                {
                    Username = username,
                    Password = password
                };
                var Students = await authenticationStudent(student);
                if (Students.StudentId == 0) return StatusCode((int)HttpStatusCode.NotFound, "Incorrect Information");
                Students.Token = generateToken(student);
                return Ok(Students);
            }
            catch(Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllStudents")]
        public async Task<IActionResult> Gets()
        {
            var list = await _studentRepository.Gets();
            return Ok(list);
        }

        [HttpDelete]
        [Route("Delete/{StudentId}")]
        public async Task<IActionResult> Delete(int StudentId)
        {
            try
            {
                Student student = new Student()
                {
                    StudentId = StudentId
                };
                string message = await _studentRepository.Delete(student);
                if (message == "Deleted successful") {
                    return Ok(student);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, message);
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private string generateToken(Student student)
        {
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var Credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Issuer"], null, expires: DateTime.Now.AddMinutes(120), signingCredentials: Credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<Student> authenticationStudent(Student student)
        {
            return await _studentRepository.GetStudent(student);
        }
    }
}
