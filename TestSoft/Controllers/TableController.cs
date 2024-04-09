using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
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
        [HttpPost]
        public IActionResult Fill(string tableName, Dictionary<string,string> columns)
        {
            try
            {
                // Создать SQL-запрос для вставки данных в таблицу
                string columnsString = string.Join(",", columns.Keys.Skip(1).Select(c => $"[{c}]"));

                // Формируем строку для значений, пропуская первый элемент
                string valuesString = string.Join(",", columns.Keys.Skip(1).Select(c => $"@{c}"));

                // Создаем SQL-запрос для вставки данных
                var insertSql = $"INSERT INTO {tableName} ({columnsString}) VALUES ({valuesString})";

                // Подготовить параметры для SQL-запроса
                var parameters = columns.Keys.Select(c => new SqliteParameter("@" + c, Request.Form[c].FirstOrDefault()));

                // Выполнить SQL-запрос
                using (var connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new SqliteCommand(insertSql, connection))
                    {
                        command.Parameters.AddRange(parameters.ToArray());
                        command.ExecuteNonQuery();
                    }
                }



                // Вернуть пользователю сообщение об успешном добавлении данных
                return RedirectToAction("Privacy", "Home") ;


            }
            catch (Exception ex)
            {
                // Обработка ошибки (например, логирование или возврат сообщения об ошибке)
                ModelState.AddModelError("", "Произошла ошибка при добавлении данных в таблицу.");
                return RedirectToAction("Index", "Home"); // Верните пользователя обратно на страницу ввода
            }

        }
        public List<Dictionary<string, object>> GetAllDataFromTable(string tableName)
        {
            var result = new List<Dictionary<string, object>>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM {tableName}";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var columnName = reader.GetName(i);
                            var columnValue = reader.GetValue(i);
                            row[columnName] = columnValue;
                        }

                        result.Add(row);
                    }
                }
            }

            return result;
        }
        public IActionResult Data(string tableName)
        {
            var data = GetAllDataFromTable(tableName);
            return View(data);
        }
    }
}
