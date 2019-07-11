using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetDatabaseStruct.Interfaces;
using GetDatabaseStruct.Models;
using Oracle.ManagedDataAccess.Client;
using Dapper;

namespace GetDatabaseStruct.Concretes
{
    public class OracleGetStruct : IGetStruct
    {
        public string ConnString { get; private set; }
        public OracleGetStruct(string connString)
        {
            this.ConnString = connString;
        }
        public IEnumerable<TableDetail> GetTableDetail()
        {
            using (OracleConnection conn = new OracleConnection(ConnString))
            {
                var list = conn.Query<TableDetail>("select TABLE_NAME as tablename,TABLE_LOCK as tablelock from user_tables");
                return list;
            }
        }

        public IEnumerable<ColumnDetail> GetColumnDetail(string tableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select tc.Column_Name as columnname,");
            sb.Append("tc.data_type as datatype,");
            sb.Append("tc.data_length as datalength,");
            sb.Append("tc.nullable,cc.comments ");
            sb.Append("from user_tab_columns tc ");
            sb.Append("left join user_col_comments cc ");
            sb.Append("on  cc.column_name = tc.column_name ");
            sb.Append("and cc.table_name  = tc.table_name ");
            sb.Append("where tc.Table_Name= :tablename ");
            using (OracleConnection conn = new OracleConnection(ConnString))
            {
                var list = conn.Query<ColumnDetail>(sb.ToString(),new { tablename=tableName});
                return list;
            }
        }
    }
}
