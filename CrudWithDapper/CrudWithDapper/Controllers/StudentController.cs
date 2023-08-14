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
        public async Task<IActionResult> GetStudent(int rollNumber)
        {
            try
            {
                var student = await _dbConnection.QuerySingleOrDefaultAsync<Students>("SELECT * FROM Master.StudentInfoTable WHERE RollNumber = @RollNumber", new { RollNumber = rollNumber });

                if (student == null)
                    return NotFound();

                return Ok(student);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(Students student)
        {
            try
            {
                const string sql = "INSERT INTO Master.StudentInfoTable (Name, Location,Email,PhoneNumber,DOB) VALUES (@Name, @Location, @Email, @PhoneNumber, @DOB)";
                await _dbConnection.ExecuteAsync(sql, student);

                return CreatedAtAction(nameof(GetStudent), new { id = student.RollNumber }, student);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int rolNumber, Students student)
        {
            try
            {
                student.RollNumber = rolNumber;
                const string sql = "UPDATE Master.StudentInfoTable" +
                                                                 "SET " +
                                                                 "Name = @Name," +
                                                                 "Location = @Location" +
                                                                 "PhoneNumber = @PhoneNumber" +
                                                                 "Email = @Email" +
                                                                 "DOB = @DOB" +
                                                                 " WHERE RollNumber = @RollNumber";
                await _dbConnection.ExecuteAsync(sql, student);

                return NoContent();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {

                const string sql = "DELETE FROM Master.StudentInfoTable WHERE RollNumber = @Id";
                await _dbConnection.ExecuteAsync(sql, new { Id = id });

                return NoContent();
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
