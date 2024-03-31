using System.ComponentModel.DataAnnotations;

public class TableModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Введите имя таблицы")]
    public string TableName { get; set; }

    [Required(ErrorMessage = "Введите названия столбцов (через запятую)")]
    public string Columns { get; set; }
}