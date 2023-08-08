using CrudWithDapper.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Threading.Tasks;

namespace CrudWithDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IDbConnection _dbConnection;

        public StudentController(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        [HttpGet("students")]
        public async Task<IActionResult> GetStudents()
        {
            try
            {
                var students = await _dbConnection.QueryAsync<Students>("SELECT * FROM Master.StudentInfoTable");

                return Ok(students);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            var student = await _dbConnection.QuerySingleOrDefaultAsync<Students>("SELECT * FROM Master.StudentInfoTable WHERE RollNumber = @Id", new { Id = id });

            if (student == null)
                return NotFound();

            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(Students student)
        {
            const string sql = "INSERT INTO Master.StudentInfoTable (Name, Location) VALUES (@Name, @Location)";
            await _dbConnection.ExecuteAsync(sql, student);

            return CreatedAtAction(nameof(GetStudent), new { id = student.RollNumber }, student);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, Students student)
        {
            student.RollNumber = id;
            const string sql = "UPDATE Master.StudentInfoTable SET Name = @Name, Location = @Location WHERE RollNumber = @RollNumber";
            await _dbConnection.ExecuteAsync(sql, student);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            const string sql = "DELETE FROM Master.StudentInfoTable WHERE RollNumber = @Id";
            await _dbConnection.ExecuteAsync(sql, new { Id = id });

            return NoContent();
        }

    }
}
