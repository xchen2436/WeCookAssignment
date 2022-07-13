using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using School.IRepository;
using School.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace School.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class MarkController : ControllerBase
    {
        private IConfiguration _config;
        IMarkRepository _markRepository = null;
        public MarkController(IConfiguration config, IMarkRepository markRepository)
        {
            _config = config;
            _markRepository = markRepository;
        }
        [HttpPost]
        [Route("InsertMark")]
        public async Task<IActionResult> InsertMark([FromBody] Mark mark)
        {
            try
            {
                mark = await _markRepository.Save(mark);
                return Ok(mark);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        [Route("UpdateMark")]
        public async Task<IActionResult> Update([FromBody] Mark mark)
        {
            try
            {
                mark = await _markRepository.Save(mark);
                return Ok(mark);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }
        [HttpGet]
        [Route("GetAllMarks")]
        public async Task<IActionResult> Gets()
        {
            var list = await _markRepository.Gets();
            return Ok(list);
        }
        [HttpGet]
        [Route("GetOneStudentMark")]
        public async Task<IActionResult> Get(int StudentId)
        {
            var list = await _markRepository.Get(StudentId);
            return Ok(list);
        }

        [HttpDelete]
        [Route("DeleteMark/{StudentId}")]
        public async Task<IActionResult> Delete(int StudentId)
        {
            try
            {
                Mark mark = new Mark()
                {
                    StudentId = StudentId
                };
                string message = await _markRepository.Delete(mark);
                if (message == "Deleted successful")
                {
                    return Ok(mark);
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
        

    }
}
