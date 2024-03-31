using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using TestSoft.Data;

namespace TestSoft.Controllers
{
    public class TableController : Controller
    {
        private readonly string _connectionString = "Data Source=table.db";
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(TableModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var connection = new SqliteConnection(_connectionString))
                    {
                        connection.Open();

                        // Создание таблицы с указанным именем и столбцами
                        var createTableSql = $"CREATE TABLE {model.TableName} ({model.Columns})";
                        using (var command = new SqliteCommand(createTableSql, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }

                    // Верните пользователю сообщение об успешном создании таблицы
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    // Обработка ошибки (например, логирование или возврат сообщения об ошибке)
                    ModelState.AddModelError("", "Произошла ошибка при создании таблицы.");
                }
            }

            // Если данные некорректны, верните пользователя обратно на страницу ввода
            return RedirectToAction("Index", "Home");
        }
    }
}
