using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetDatabaseStruct.Interfaces;
using GetDatabaseStruct.Models;
using Dapper;
using System.Data.SqlClient;
using System.Data;

namespace GetDatabaseStruct.Concretes
{
    public class SqlServerGetStruct : IGetStruct
    {
        public string ConnString { get; private set; }
        public SqlServerGetStruct(string connString)
        {
            ConnString = connString;
        }

        public IEnumerable<ColumnDetail> GetColumnDetail(string tableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            sb.Append(" [TableName] = i_s.TABLE_NAME,");
            sb.Append(" [ColumnName] = i_s.COLUMN_NAME,");
            sb.Append(" [DataType]=i_s.DATA_TYPE,");
            sb.Append(" [DataLength]=i_s.CHARACTER_MAXIMUM_LENGTH,");
            sb.Append(" [Nullable]=i_s.IS_Nullable,");
            sb.Append(" [Comments] = s.value");
            sb.Append(" FROM");
            sb.Append(" INFORMATION_SCHEMA.COLUMNS i_s");
            sb.Append(" LEFT OUTER JOIN sys.extended_properties s ON");
            sb.Append(" s.major_id = OBJECT_ID(i_s.TABLE_SCHEMA+'.'+i_s.TABLE_NAME)");
            sb.Append(" AND s.minor_id = i_s.ORDINAL_POSITION");
            sb.Append(" AND s.name = 'MS_Description'");
            sb.Append(" WHERE OBJECTPROPERTY(OBJECT_ID(i_s.TABLE_SCHEMA+'.'+i_s.TABLE_NAME), 'IsMsShipped')=0");
            sb.Append(" AND i_s.TABLE_NAME = @tablename");
            sb.Append(" ORDER BY i_s.TABLE_NAME, i_s.ORDINAL_POSITION");

            using (var conn = new SqlConnection(ConnString))
            {
                IEnumerable<ColumnDetail> list = conn.Query<ColumnDetail>(sb.ToString(),new { tablename=tableName});
                return list;
            }
        }

        public IEnumerable<TableDetail> GetTableDetail()
        {
            using (var conn = new SqlConnection(ConnString))
            {
                IEnumerable<TableDetail> list = conn.Query<TableDetail>("SELECT table_name as TableName FROM INFORMATION_SCHEMA.TABLES");
                return list;
            }
        }
    }
}
