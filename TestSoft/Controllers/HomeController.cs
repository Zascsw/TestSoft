using Grpc.Core;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using TestSoft.Models;

namespace TestSoft.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // Читаем JSON файл и десериализуем его в объект TableStructure
            string jsonFilePath = Server.MapPath("~/App_Data/table.json");
            string jsonData = System.IO.File.ReadAllText(jsonFilePath);
            TableStructure tableStructure = JsonConvert.DeserializeObject<TableStructure>(jsonData);

            // Передаем объект TableStructure в представление
            return View(tableStructure);
        }

        [HttpPost]
        public ActionResult Index(TableStructure model)
        {
            // Обработка данных, отправленных формой
            // Здесь вы можете сохранить данные или выполнить другие действия

            // Перенаправляем пользователя обратно на страницу с формой
            return RedirectToAction("Index");
        }
    }
    public class TableStructure
    {
        public string TableName { get; set; }
        public List<string> Columns { get; set; }
        public int Row { get; set; }
    }
}