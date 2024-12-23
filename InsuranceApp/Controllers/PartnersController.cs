using Microsoft.AspNetCore.Mvc;
using Dapper;
using InsuranceApp.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Linq;

public class PartnersController : Controller
{
    private readonly string _connectionString;

    public PartnersController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public IActionResult Index()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            string query = @"
            SELECT 
                p.*, 
                COUNT(pol.Id) AS TotalPolicies,
                ISNULL(SUM(pol.PolicyAmount), 0) AS TotalAmount
            FROM Partners p
            LEFT JOIN Policies pol ON p.Id = pol.PartnerId
            GROUP BY p.Id, p.FirstName, p.LastName, p.Address, p.PartnerNumber, p.CroatianPIN,
                     p.PartnerTypeId, p.CreatedAtUtc, p.CreateByUser, p.IsForeign, p.ExternalCode, p.Gender
            ORDER BY p.CreatedAtUtc DESC";

            var partners = connection.Query<Partner>(query).ToList();

            // Dohvati najnovijeg partnera
            var newestPartner = partners.FirstOrDefault();  // Prvi partner je najnoviji jer su sortirani po CreatedAtUtc

            // Spremaj ID najnovijeg partnera u ViewData za korištenje u view-u
            ViewData["NewestPartnerId"] = newestPartner?.Id;

            return View(partners);
        }
    }

    public IActionResult Create() => View();
    [HttpPost]
    public IActionResult Create(Partner partner)
    {
        if (!ModelState.IsValid) return View(partner);

        using (var connection = new SqlConnection(_connectionString))
        {
            string query = @"
            INSERT INTO Partners (FirstName, LastName, Address, PartnerNumber, CroatianPIN, PartnerTypeId, 
                                  CreateByUser, IsForeign, ExternalCode, Gender)
            VALUES (@FirstName, @LastName, @Address, @PartnerNumber, @CroatianPIN, @PartnerTypeId, 
                    @CreateByUser, @IsForeign, @ExternalCode, @Gender)";
            connection.Execute(query, partner);
        }

        TempData["Success"] = "Partner successfully created!";
        return RedirectToAction("Index");
    }
    [HttpGet("api/partners/{id}")]
    public IActionResult GetPartnerDetails(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            string query = @"
                SELECT 
                    p.*, 
                    COUNT(pol.Id) AS TotalPolicies,
                    ISNULL(SUM(pol.PolicyAmount), 0) AS TotalAmount
                FROM Partners p
                LEFT JOIN Policies pol ON p.Id = pol.PartnerId
                WHERE p.Id = @Id
                GROUP BY p.Id, p.FirstName, p.LastName, p.Address, p.PartnerNumber, p.CroatianPIN,
                         p.PartnerTypeId, p.CreatedAtUtc, p.CreateByUser, p.IsForeign, p.ExternalCode, p.Gender";
            var partner = connection.QueryFirstOrDefault<Partner>(query, new { Id = id });

            if (partner == null)
                return NotFound(new { message = "Partner not found" });

            return Ok(partner);
        }
    }
}

