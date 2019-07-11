using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDatabaseStruct.Models
{
    public class ColumnDetail
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public string DataLength { get; set; }
        public string Nullable { get; set; }
        public string Comments { get; set; }
    }
}
