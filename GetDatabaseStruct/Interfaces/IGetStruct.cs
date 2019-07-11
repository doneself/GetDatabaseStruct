using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetDatabaseStruct.Models;

namespace GetDatabaseStruct.Interfaces
{
    public interface IGetStruct
    {
        IEnumerable<TableDetail> GetTableDetail();

        IEnumerable<ColumnDetail> GetColumnDetail(string tableName);
    }
}
