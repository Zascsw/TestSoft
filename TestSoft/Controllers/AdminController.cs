// Controllers/AdminController.cs
using Microsoft.AspNetCore.Mvc;
using TestSoft.Models;
using Newtonsoft.Json;


namespace TestSoft.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateTable(TableCreationModel model)
        {
            if (ModelState.IsValid)
            {
                // Преобразуем список имен столбцов в массив
                 var columnNames = string.Join(",", model.Columns);

                // Создаем объект для сохранения в JSON
                var tableData = new
                {
                    TableName = model.TableName,
                    Columns = columnNames,
                    Row = model.RowCount
                };

                // Преобразуем объект в JSON и сохраняем в файл
                var jsonData = JsonConvert.SerializeObject(tableData);
                var filePath = $"Data/{model.TableName}.json"; // Путь к файлу, куда сохранять
                System.IO.File.WriteAllText(filePath, jsonData);

                return RedirectToAction("Index"); // Перенаправляем пользователя после успешного создания
            }

            // Если модель не прошла валидацию, возвращаем форму с сообщениями об ошибках
           return View(Index);
        }
    }
}
