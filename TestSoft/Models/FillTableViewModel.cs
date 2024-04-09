using Microsoft.AspNetCore.Mvc;

public class FillTableViewModel
{
    public string TableName { get; set; }
    public List<string> Columns { get; set; }
}