using Microsoft.AspNetCore.Mvc;
using InsuranceApp.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace InsuranceApp.Controllers
{
    [Route("api/policies")]
    [ApiController]
    public class PoliciesController : ControllerBase
    {
        private readonly string _connectionString;

        public PoliciesController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost]
        public IActionResult Create([FromBody] Policy policy)
        {
            if (policy == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid policy data.");
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    INSERT INTO Policies (PartnerId, PolicyNumber, PolicyAmount)
                    VALUES (@PartnerId, @PolicyNumber, @PolicyAmount)";
                connection.Execute(query, policy);
            }

            return Ok("Policy successfully created!");
        }
    }
}
