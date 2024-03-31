using System.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TestSoft.Data;
using TestSoft.Models;

namespace TestSoft.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly string _connectionString = "Data Source=table.db";
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
   // [Authorize]
    public IActionResult Index()
    {
        return View();
    }
    [Authorize(Roles = "Administrators")]
    public IActionResult Privacy()
    {
        List<string> taleList = new List<string>();
        using (var connection = new SqliteConnection("Data Source=table.db"))
        {
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            connection.Open();
            const string sql_query = "SELECT name FROM sqlite_schema WHERE type ='table' AND name NOT LIKE 'sqlite_%';";
            var command = new SqliteCommand(sql_query, connection);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var tablename = reader.GetString(0);
                    taleList.Add(tablename);
                }
            }
        }
            return View(taleList);
    }
    //[Authorize]
    public IActionResult Create()
    {
        return View();
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
