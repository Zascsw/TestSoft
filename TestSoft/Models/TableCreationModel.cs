// Models/TableCreationModel.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestSoft.Models
{
    public class TableCreationModel
    {
        public string TableName { get; set; }
        public List<string> Columns { get; set; }
        public int RowCount { get; set; }
       // public List<List<string>> Rows { get; set; }
    }
}
